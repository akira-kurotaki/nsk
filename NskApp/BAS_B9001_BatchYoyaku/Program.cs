using BAS_B9001_BatchYoyaku.Common;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;
using Npgsql;
using NpgsqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace BAS_B9001_BatchYoyaku
{
    /// <summary>
    /// 定時実行予約登録
    /// </summary>
    class Program
    {
        /// <summary>
        /// 定時実行予約登録
        /// </summary>
        private static string BATCH_NAME = "定時実行予約登録";

        /// <summary>
        /// BAS_B9001
        /// </summary>
        private static string BAS_B9001 = "BAS_B9001";

        /// <summary>
        /// ヘッダ部
        /// </summary>
        private static string[] HEADER_NAMES =
            {
                "予約月",
                "予約曜日",
                "予約日",
                "バッチ分類",
                "システム区分",
                "都道府県コード",
                "組合等コード",
                "支所コード",
                "バッチ名",
                "バッチ条件",
                "バッチ条件（画面表示用）",
                "複数実行禁止フラグ",
                "複数実行禁止ID",
                "定時実行バッチ予約日時（時刻部分のみ）",
                "ロック対象フラグ"
            };

        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static Program()
        {
            // Npgsql 6.0での「timestamp with time zone」非互換対応
            // Npgsql 6.0より前の動作に戻す
            // https://www.npgsql.org/doc/types/datetime.html#timestamps-and-timezones
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            // SJIS(Shift_JIS)を使用可能にする
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// メイン処理
        /// </summary>
        /// <param name="args">パラメータ配列</param>
        static void Main(string[] args)
        {
            // 処理開始のログを出力する。
            logger.Info(string.Concat(CoreConst.LOG_START_KEYWORD, " 定時実行予約登録処理"));
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = Constants.BATCH_EXECUT_SUCCESS;
            var sysDate = DateUtil.GetSysDateTime();
            using (var db = new SystemCommonContext())
            {
                using (var tx = db.Database.BeginTransaction())
                {
                    try
                    {
                        var model = GetScheBatchRunManageInfo(db);
                        if (model == null)
                        {
                            // 結果が空の場合、新規登録する
                            InsertTScheBatchRunManage(db, sysDate);
                        }
                        else
                        {
                            // 結果が空以外の場合、更新登録する
                            var count = UpdateTScheBatchRunManage(db, sysDate);
                            if (count == 0)
                            {
                                logger.Error("既に他サーバのバッチが実行中のため、処理をキャンセルしました。");
                                return;
                            }
                        }

                        // 定時実行予約設定ファイルを取得する。
                        string filePath = ConfigUtil.Get(Constants.SCHE_BATCH_RUN_SETTING);
                        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                        {
                            logger.Error("バッチ予約の設定ファイルが存在しません。管理者に連絡してください。");
                            return;
                        }

                        var list = new List<TBatchYoyaku>();
                        using (var reader = new StreamReader(filePath, Encoding.UTF8))
                        {
                            var col = 0;
                            // ファイル内容を読み取る
                            while (!reader.EndOfStream)
                            {
                                // 行数の読込む
                                var line = reader.ReadLine();
                                col += 1;
                                if (col == 1)
                                {
                                    // ヘッダ行読込まない
                                    continue;
                                }

                                // 設定ファイルをエラーチェック
                                // エラーがある場合は、エラーログを出力し、次の行を読み取る。
                                TBatchYoyaku keyaku = null;
                                if (DataCheck(db, col, line, sysDate, ref keyaku))
                                {
                                    // エラーがなければList：バッチ予約に追加する。
                                    list.Add(keyaku);
                                }
                            }
                        }
                        if (list.Count > 0)
                        {
                            ImportBatchYoyaku(db, list);
                        }
                        tx.Commit();
                    }
                    catch (Exception e)
                    {
                        // ロールバックする。
                        tx.Rollback();
                        // 業務ログを出力する
                        logger.Error("バッチ予約テーブルの登録に失敗しました。");
                        logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                        // 処理結果（正常：0、エラー：1）
                        result = Constants.BATCH_EXECUT_FAILED;
                    }
                }
            }

            // 処理時間
            stopwatch.Stop();
            var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + CoreConst.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + CoreConst.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
            logger.Info(excutingTime);

            // 処理終了のログを出力し、処理を終了する。	
            logger.Info(string.Concat(CoreConst.LOG_END_KEYWORD, " 定時実行予約登録処理"));

            // 処理結果（正常：0、エラー：1）
            Environment.ExitCode = result;
        }

        #region エラーチェック
        /// <summary>
        /// エラーチェック
        /// </summary>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="col">行数</param>
        /// <param name="line">行のデータ</param>
        /// <param name="sysDate">システム日時</param>
        /// <param name="keyaku">バッチ予約</param>
        /// <returns>true: エラーなし  false: エラーあり </returns>
        private static bool DataCheck(SystemCommonContext db, int col, string line, DateTime sysDate, ref TBatchYoyaku keyaku)
        {
            if (string.IsNullOrEmpty(line))
            {
                return false;
            }

            var result = true;
            string[] items = CsvUtil.GetItems(line.Replace("\"", ""));

            keyaku = new TBatchYoyaku()
            {
                BatchYoyakuDate = sysDate,
                BatchYoyakuId = BAS_B9001,
                BatchYoyakuShori = BATCH_NAME,
                BatchType = BatchUtil.BATCH_TYPE_SCHEDULED,
                BatchStatus = BatchUtil.BATCH_STATUS_WAITING,
                BatchStartDate = null,
                BatchEndDate = null,
                BatchRunServer = null,
                ErrorInfo = null,
                DeleteFlg = BatchUtil.DELETE_FLG_NOT_DELETED,
                InsertUserId = BAS_B9001,
                InsertDate = sysDate,
                UpdateUserId = BAS_B9001,
                UpdateDate = sysDate
            };

            // 列数チェック	
            // 条件：1行読み取った際の項目数が、定時実行予約設定ファイルの列数と一致しない場合
            if (items == null || items.Length != Constants.SETTING_FILE_ROW_NUMS)
            {
                // ログ出力内容：XX行目の列数が不正です。
                logger.Error(string.Concat(col, "行目の列数が不正です。"));
                return false;
            }

            // 必須チェック
            // 条件：必須項目が空欄""の場合
            var requiredIndexs = new int[] { 1, 2, 3, 4, 5, 6, 9, 12, 14, 15 };
            foreach (var i in requiredIndexs)
            {
                var index = i - 1;
                if (string.IsNullOrEmpty(items[index]))
                {
                    // ログ出力内容：XX行目のYYが設定されていません。
                    logger.Error(string.Concat(col, "行目の", HEADER_NAMES[index], "が設定されていません。"));
                    result = false;
                }
            }

            // 桁数チェック
            // 全桁チェック 項目
            var fullLengthIndexs = new Dictionary<int, int>
            {
                { 4, 2 },
                { 5, 2 },
                { 6, 2 },
                { 7, 3 },
                { 8, 2 },
                { 12, 1 },
                { 14, 8 },
                { 15, 1 }
            };
            foreach (var one in fullLengthIndexs)
            {
                var index = one.Key - 1;
                var fullLength = one.Value;
                var value = items[index];
                if (!string.IsNullOrEmpty(items[index]) && value.ToString().Length != fullLength)
                {
                    // ログ出力内容：XX行目のYYの桁数が不正です。
                    logger.Error(string.Concat(col, "行目の", HEADER_NAMES[index], "の桁数が不正です。"));
                    result = false;
                }
            }
            // 最大桁数チェック 項目
            var maxLengthIndexs = new Dictionary<int, int>
            {
                { 9, 30 },
                { 10, 100 },
                { 13, 100 },
            };
            foreach (var one in maxLengthIndexs)
            {
                var index = one.Key - 1;
                var maxLength = one.Value;
                var value = items[index];
                if (!string.IsNullOrEmpty(items[index]) && maxLength < value.ToString().Length)
                {
                    // ログ出力内容：XX行目のYYの桁数が不正です。
                    logger.Error(string.Concat(col, "行目の", HEADER_NAMES[index], "の桁数が不正です。"));
                    result = false;
                }
            }

            // 整合性チェック
            //条件：値が空欄""以外かつ、[備考]に示される条件、マスタと値が一致しない。
            var list = CheckParamMaster(db, items);
            if (!list.IsNullOrEmpty())
            {
                foreach (var one in list)
                {
                    //出力内容：XX行目のYYの設定がマスタに存在しません。
                    logger.Error(string.Concat(col, "行目の", one, "の設定がマスタに存在しません。"));
                    result = false;
                }
            }

            // 数字と*のみ(予約月、予約曜日、予約日)
            var yoyakuIndexs = new List<int>() { 1, 2, 3 };
            foreach (int i in yoyakuIndexs)
            {
                var index = i - 1;
                var value = items[index];
                if (!string.IsNullOrEmpty(value) && !Constants.SYMBOL_STAR.Equals(value) && !StringUtil.IsNumeric(value))
                {
                    logger.Error(string.Concat(col, "行目の", HEADER_NAMES[index], "が不正です。(数字と* のみ)"));
                    result = false;
                }
            }

            // 外字以外(バッチ条件、バッチ条件（画面表示用）、複数実行禁止ID)
            var exceptGaijiIndexs = new List<int>() { 10, 11, 13 };
            foreach (int i in exceptGaijiIndexs)
            {
                var index = i - 1;
                var value = items[index];
                if (!string.IsNullOrEmpty(value) && StringUtil.CheckMS932ExceptGaiji(value).Length != 0)
                {
                    logger.Error(string.Concat(col, "行目の", HEADER_NAMES[index], "が不正です。(外字以外)"));
                    result = false;
                }
            }

            // 定時実行バッチ予約日時（時刻部分のみ）
            // システム日付と結合してDATETIMEに変換できるか確認
            var timeIndex = 14;
            var timeString = items[timeIndex - 1];
            var nowStr = sysDate.Date.ToString("yyyy/MM/dd");
            DateTime? scheDateTime = null;
            if (!string.IsNullOrEmpty(timeString))
            {
                var dateStr = nowStr + CoreConst.HALF_WIDTH_SPACE + timeString;
                if (IsDateTime(dateStr, "yyyy/MM/dd hh:mm:ss"))
                {
                    scheDateTime = DateTime.Parse(dateStr);
                }
                else
                {
                    scheDateTime = null;
                    logger.Error(string.Concat(col, "行目の", HEADER_NAMES[timeIndex - 1], "が不正です。(システム日付と結合してDATETIMEに変換できる)"));
                    result = false;
                }
            }

            // 上記いずれのエラーもない場合、「予約月」～「予約日」列の内容にシステム日付が合致する確認する。
            if (result)
            {
                //（１）曜日と日付が両方数字で設定されている場合
                var yoyakuMonth = items[0];
                var yoyakuDay = items[1];
                var yoyakuDate = items[2];
                if (!string.IsNullOrEmpty(yoyakuDay) && !Constants.SYMBOL_STAR.Equals(yoyakuDay) && StringUtil.IsNumeric(yoyakuDay)
                    && !string.IsNullOrEmpty(yoyakuDate) && !Constants.SYMBOL_STAR.Equals(yoyakuDate) && StringUtil.IsNumeric(yoyakuDate))
                {
                    //エラーとして、次の行の処理を行う。
                    //ログ出力内容：XX行目について、曜日と日付は両方設定することはできません。
                    logger.Error(string.Concat(col, "行目について、曜日と日付は両方設定することはできません。"));
                    return false;
                }

                //（２）予約月
                //*= の場合、合致する
                //* 以外の場合、システム日付の月と一致する場合、合致する。
                //（３）予約曜日
                //*= の場合、合致する
                //* 以外の場合、システム日付の曜日（月曜が1）と一致する場合、合致する。
                //（４）予約日
                //*= の場合、合致する
                //* 以外の場合、システム日付の日と一致する場合、合致する。
                //（５）（２）～（４）がすべて合致する場合は、データをList：バッチ予約に追加する。
                if (!((Constants.SYMBOL_STAR.Equals(yoyakuMonth) || sysDate.Month.ToString().Equals(yoyakuMonth))
                        && (Constants.SYMBOL_STAR.Equals(yoyakuDay) || sysDate.DayOfWeek.ToString("d").Equals(yoyakuDay))
                        && (Constants.SYMBOL_STAR.Equals(yoyakuDate) || sysDate.Day.ToString().Equals(yoyakuDate)))
                    )
                {
                    result = false;
                }
            }

            // エラーなしの場合
            if (result)
            {
                // バッチID取得
                var seqBatchId = DBUtil.NextSeqVal(db, "seq_batch_id");
                keyaku.BatchId = seqBatchId;
                keyaku.BatchBunrui = items[3];
                keyaku.SystemKbn = items[4];
                keyaku.TodofukenCd = items[5];
                keyaku.KumiaitoCd = items[6];
                keyaku.ShishoCd = items[7];
                keyaku.BatchNm = items[8];
                keyaku.BatchJoken = items[9];
                keyaku.BatchJokenDisp = items[10];
                keyaku.MultiRunNgFlg = items[11];
                keyaku.MultiRunNgId = items[12];
                keyaku.BatchScheDatetime = scheDateTime;
                keyaku.LockTargetFlg = items[14];
            }

            return result;
        }
        #endregion

        #region マスタ整合性チェック（バッチ予約登録）
        /// <summary>
        /// マスタ整合性チェック（バッチ予約登録）
        /// </summary>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="items">行のデータ</param>
        /// <returns>エラー項目</returns>
        private static List<string> CheckParamMaster(SystemCommonContext db, string[] items)
        {
            var list = new List<string>();
            // バッチ分類：汎用区分マスタ「batch_bunrui」
            if (!string.IsNullOrEmpty(items[3]))
            {
                var name = HanyokubunUtil.GetKbnNm("batch_bunrui", items[3]);
                if (string.IsNullOrEmpty(name))
                {
                    list.Add(HEADER_NAMES[3]);
                }
            }

            // システム区分：汎用区分マスタ「system_kbn」
            if (!string.IsNullOrEmpty(items[4]))
            {
                var name = HanyokubunUtil.GetKbnNm("system_kbn", items[4]);
                if (string.IsNullOrEmpty(name))
                {
                    list.Add(HEADER_NAMES[4]);
                }
            }

            // 都道府県コード：都道府県マスタ
            if (!string.IsNullOrEmpty(items[5]))
            {
                var mTodofuken = TodofukenUtil.GetTodofukenNm(items[5]);
                if (string.IsNullOrEmpty(mTodofuken))
                {
                    list.Add(HEADER_NAMES[5]);
                }
            }

            // 組合等コード：組合等マスタ
            if (!string.IsNullOrEmpty(items[5]) && !string.IsNullOrEmpty(items[6]))
            {
                var mKumiaito = KumiaitoUtil.GetKumiaitoNm(items[5], items[6]);
                if (string.IsNullOrEmpty(mKumiaito))
                {
                    list.Add(HEADER_NAMES[6]);
                }
            }

            // 支所コード：名称M支所
            if (!string.IsNullOrEmpty(items[5]) && !string.IsNullOrEmpty(items[6]) && !string.IsNullOrEmpty(items[7]))
            {
                var mShishoNm = ShishoUtil.GetShishoNm(items[5], items[6], items[7]);
                if (string.IsNullOrEmpty(mShishoNm))
                {
                    list.Add(HEADER_NAMES[7]);
                }
            }

            // バッチ名：予約処理マスタ
            if (!string.IsNullOrEmpty(items[4]) && !string.IsNullOrEmpty(items[8]))
            {
                logger.Info("予約処理マスタからデータを取得します。");
                var mYoyaku = db.MYoyakus.Where(m => m.SystemKbn == items[4] && m.BatchNm == items[8]).SingleOrDefault();
                if (mYoyaku == null)
                {
                    list.Add(HEADER_NAMES[8]);
                }
            }

            // 複数実行禁止フラグ：汎用区分マスタ「multi_run_ng_flg」
            if (!string.IsNullOrEmpty(items[11]))
            {
                var name = HanyokubunUtil.GetKbnNm("multi_run_ng_flg", items[11]);
                if (string.IsNullOrEmpty(name))
                {
                    list.Add(HEADER_NAMES[11]);
                }
            }

            // ロック対象フラグ：汎用区分マスタ「lock_target_flg」
            if (!string.IsNullOrEmpty(items[14]))
            {
                var name = HanyokubunUtil.GetKbnNm("lock_target_flg", items[14]);
                if (string.IsNullOrEmpty(name))
                {
                    list.Add(HEADER_NAMES[14]);
                }
            }

            return list;
        }
        #endregion

        #region 定時実行予約登録のデータを排他ロックする。
        /// <summary>
        /// 定時実行予約登録のデータを排他ロックする。
        /// </summary>
        /// <param name="db">DBコンテキスト</param>
        /// <returns>定時バッチ起動管理情報</returns>
        private static TScheBatchRunManage GetScheBatchRunManageInfo(SystemCommonContext db)
        {
            // sql用定数定義
            StringBuilder sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            sql.Append("SELECT");
            sql.Append("    batch_nm ");
            sql.Append("    , batch_run_date ");
            sql.Append("    , xmin ");
            sql.Append("    , system_kbn ");
            //sql.Append("    batch_nm  AS \"BatchNm\" ");
            //sql.Append("    , batch_run_date  AS \"BatchRunDate\" ");
            //sql.Append("    , system_kbn  AS \"SystemKbn\" ");
            sql.Append("FROM");
            sql.Append("    t_sche_batch_run_manage ");
            sql.Append("WHERE");
            sql.Append("    batch_nm = @BatchNm FOR UPDATE");

            parameters.Add(new NpgsqlParameter("@BatchNm", BATCH_NAME));

            logger.Info("「システム共通スキーマの定時バッチ起動管理テーブルの定時実行予約登録のデータを排他ロックする」の処理を実行します。");
            var list = db.Database.SqlQueryRaw<TScheBatchRunManage>(sql.ToString(), parameters.ToArray()).ToList();
            if (list.IsNullOrEmpty())
            {
                return null;
            }
            return list[0];
        }
        #endregion

        #region 定時バッチ起動管理新規登録処理
        /// <summary>
        /// 定時バッチ起動管理新規登録処理
        /// </summary>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="sysDate">システム日時</param>
        /// <returns>新規件数</returns>
        private static int InsertTScheBatchRunManage(SystemCommonContext db, DateTime sysDate)
        {
            var model = new TScheBatchRunManage()
            {
                SystemKbn = ConfigUtil.Get(Constants.SYSTEM_KBN_COMMON),
                BatchNm = BATCH_NAME,
                BatchRunDate = sysDate.Date
            };

            logger.Info("「定時バッチ起動管理新規登録」の処理を実行します。");
            db.TScheBatchRunManages.Add(model);
            return db.SaveChanges();
        }
        #endregion

        #region 定時バッチ起動管理更新登録処理
        /// <summary>
        /// 定時バッチ起動管理更新登録処理
        /// </summary>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="sysDate">システム日時</param>
        /// <returns>更新件数</returns>
        private static int UpdateTScheBatchRunManage(SystemCommonContext db, DateTime sysDate)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            sql.Append("UPDATE t_sche_batch_run_manage ");
            sql.Append("SET");
            sql.Append("    batch_run_date = @batch_run_date ");
            sql.Append("WHERE");
            sql.Append("    system_kbn = @SystemKbn ");
            sql.Append("    AND batch_nm = @BatchNm ");
            sql.Append("    AND batch_run_date <> @BatchRunDate");

            parameters.Add(new NpgsqlParameter("@BatchRunDate", sysDate.Date));
            parameters.Add(new NpgsqlParameter("@SystemKbn", ConfigUtil.Get(Constants.SYSTEM_KBN_COMMON)));
            parameters.Add(new NpgsqlParameter("@BatchNm", BATCH_NAME));

            logger.Info("「定時バッチ起動管理更新登録」の処理を実行します。");
            return db.Database.ExecuteSqlRaw(sql.ToString(), parameters.ToArray());
        }
        #endregion

        #region 日付判断
        /// <summary>
        /// 対象の文字列がフォーマットに沿った日付か判断する。
        /// </summary>
        /// <param name="strDate">日付文字列</param>
        /// <param name="format">日付フォーマット</param>
        /// <returns>true：フォーマットに沿った日付、false：その他</returns>
        private static bool IsDateTime(string strDate, string format)
        {
            return DateTime.TryParseExact(strDate, format, null, DateTimeStyles.None, out DateTime d);
        }
        #endregion

        #region バッチ予約登録（一括）
        /// <summary>
        /// バッチ予約登録（一括）
        /// </summary>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="batchYoyakus">バッチ予約状況</param>
        /// <returns>true:OK / false:NG</returns>
        private static bool ImportBatchYoyaku(SystemCommonContext db, List<TBatchYoyaku> batchYoyakus)
        {
            var result = true;
            try
            {
                var conn = db.Database.GetDbConnection() as NpgsqlConnection;
                StringBuilder sql = new StringBuilder();
                sql.Append(
                    "COPY t_batch_yoyaku (" +
                    "batch_id, " +
                    "batch_bunrui, " +
                    "system_kbn, " +
                    "todofuken_cd, " +
                    "kumiaito_cd, " +
                    "shisho_cd, " +
                    "batch_yoyaku_date, " +
                    "batch_yoyaku_id, " +
                    "batch_yoyaku_shori, " +
                    "batch_nm, " +
                    "batch_joken, " +
                    "batch_joken_disp, " +
                    "multi_run_ng_flg, " +
                    "multi_run_ng_id, " +
                    "batch_type, " +
                    "batch_sche_datetime, " +
                    "lock_target_flg, " +
                    "batch_status, " +
                    "batch_start_date, " +
                    "batch_end_date, " +
                    "batch_run_server, " +
                    "error_info, " +
                    "delete_flg, " +
                    "insert_user_id, " +
                    "insert_date, " +
                    "update_user_id, " +
                    "update_date ) " +
                    "FROM STDIN (FORMAT BINARY)");

                using (var writer = conn.BeginBinaryImport(sql.ToString()))
                {
                    foreach (var yoyaku in batchYoyakus)
                    {
                        writer.StartRow();
                        writer.Write(yoyaku.BatchId, NpgsqlDbType.Bigint);
                        writer.Write(yoyaku.BatchBunrui, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.SystemKbn, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.TodofukenCd, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.KumiaitoCd, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.ShishoCd, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.BatchYoyakuDate, NpgsqlDbType.Timestamp);
                        writer.Write(yoyaku.BatchYoyakuId, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.BatchYoyakuShori, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.BatchNm, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.BatchJoken, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.BatchJokenDisp, NpgsqlDbType.Text);
                        writer.Write(yoyaku.MultiRunNgFlg, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.MultiRunNgId, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.BatchType, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.BatchScheDatetime, NpgsqlDbType.Timestamp);
                        writer.Write(yoyaku.LockTargetFlg, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.BatchStatus, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.BatchStartDate, NpgsqlDbType.Timestamp);
                        writer.Write(yoyaku.BatchEndDate, NpgsqlDbType.Timestamp);
                        writer.Write(yoyaku.BatchRunServer, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.ErrorInfo, NpgsqlDbType.Text);
                        writer.Write(yoyaku.DeleteFlg, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.InsertUserId, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.InsertDate, NpgsqlDbType.Timestamp);
                        writer.Write(yoyaku.UpdateUserId, NpgsqlDbType.Varchar);
                        writer.Write(yoyaku.UpdateDate, NpgsqlDbType.Timestamp);
                    }

                    logger.Info("「バッチ予約登録（一括）」の処理を実行します。");
                    writer.Complete();
                }
                logger.Info(sql.ToString());
            }
            catch (Exception e)
            {
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                result = false;
            }
            return result;
        }
        #endregion

    }
}
