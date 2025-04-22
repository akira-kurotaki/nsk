using System.Text;
using NLog;
using Core = CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NskAppModelLibrary.Context;
using NskCommon = NskCommonLibrary.Core.Consts;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NSK_B111011.Models;

namespace NSK_B111011
{
    /// <summary>
    /// 交付金計算処理
    /// </summary>
    class Program
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static Program()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// 交付金計算処理
        /// </summary>
        /// <param name="args">
        /// 引数配列要素1：バッチID
        /// 引数配列要素2：都道府県コード
        /// 引数配列要素3：組合等コード
        /// 引数配列要素4：支所コード
        /// 引数配列要素5：バッチ条件のキー情報
        /// </param>
        public static void Main(string[] args)
        {
            // 引数
            // バッチID
            string bid = string.Empty;
            // 都道府県コード
            string todofukenCd = string.Empty;
            // 組合等コード
            string kumiaitoCd = string.Empty;
            // 支所コード
            string shishoCd = string.Empty;
            // バッチ条件のキー情報
            string jid = string.Empty;

            // バッチ予約ユーザID
            string batchYoyakuId = string.Empty;
            // [変数：エラーメッセージ] string.Empty
            string errorMessage = string.Empty;
            // [変数：ステータス] "03"（成功）
            string status = NskCommon.CoreConst.STATUS_SUCCESS;
            // 処理結果（正常：0、エラー：1）
            int result = NskCommon.CoreConst.BATCH_EXECUT_SUCCESS;

            try
            {
                // １．設定ファイルから、以下の内容を取得し、グローバル変数へ保存する。
                // １．１．上記の何れか（DBデフォルトスキーマを除外）が正しく取得できなかった場合、以下のエラーメッセージをログに出力し、処理を中断する。
                // システム時間フラグ    （検索キー：SysDateTimeFlag）
                // appsetting.jsonの設定にあるシステム時間フラグ
                bool sysDateTimeFlag = false;
                if (bool.TryParse(ConfigUtil.Get(Core.CoreConst.SYS_DATE_TIME_FLAG), out sysDateTimeFlag))
                {
                    // システム時間ファイルパス （検索キー：SysDateTimePath）※本項目は「システム時間フラグ」が"true"で取得できた場合のみ対象
                    // appsetting.jsonの設定にあるシステム時間ファイルパス
                    string sysDateTimePath = string.Empty;
                    if (sysDateTimeFlag)
                    {
                        // システム時間ファイルパス
                        sysDateTimePath = ConfigUtil.Get(Core.CoreConst.SYS_DATE_TIME_PATH);

                        if (string.IsNullOrEmpty(sysDateTimePath))
                        {
                            // エラーメッセージをログに出力し、処理を中断する。
                            // （"ME90015"、{0}：システム時間ファイルパス）
                            throw new AppException("ME90015", MessageUtil.Get("ME90015", "システム時間ファイルパス"));
                        }
                    }
                }
                else
                {
                    // エラーメッセージをログに出力し、処理を中断する。5
                    // （"ME90015"、{0}：システム時間フラグ）
                    throw new AppException("ME90015", MessageUtil.Get("ME90015", "システム時間フラグ"));
                }

                // ２．システム日付の設定を行う。
                // ２．１．システム日付の設定
                // （ア）[変数：システム日付] に以下の条件に従って設定を行う。
                // （１）[グローバル変数：システム時間フラグ] がtrueの場合、[グローバル変数：システム時間ファイルパス] の年月日＋マシン時間を設定する。
                // （２）上記以外、マシン時間を設定する。
                // [変数：システム日付]
                DateTime systemDate = DateUtil.GetSysDateTime(); // DateUtil.GetSysDateTime()内で全てやっている

                // args から 各変数へ展開する
                // バッチID
                if (args.Length > 0)
                {
                    bid = args[0];
                }
                // 都道府県コード
                if (args.Length > 1)
                {
                    todofukenCd = args[1];
                }
                // 組合等コード
                if (args.Length > 2)
                {
                    kumiaitoCd = args[2];
                }
                // 支所コード
                if (args.Length > 3)
                {
                    shishoCd = args[3];
                }
                // バッチ条件のキー情報
                if (args.Length > 4)
                {
                    jid = args[4];
                }

                // ３．引数のチェック
                // ３．１．必須チェック
                // ３．１．１．[変数：バッチID] が未入力の場合
                if (string.IsNullOrEmpty(bid))
                {
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１４.」へ進む。
                    // （"ME01645" 引数{0} ：パラメータの取得）
                    throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
                }

                // ３．１．２．[変数：バッチID]が数値変換不可の場合
                // 数値化したバッチID
                int nBid = 0;
                if (!int.TryParse(bid, out nBid))
                {
                    // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「１４．」へ進む。
                    // （"ME90012"　引数{0} ：バッチID)
                    throw new AppException("ME90012", MessageUtil.Get("ME90012", "バッチID"));
                }

                // ３．１．３．[変数：都道府県コード]が未入力の場合
                if (string.IsNullOrEmpty(todofukenCd))
                {
                    // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「１４．」へ進む。
                    // （"ME01054"　引数{0} ：都道府県コード)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "都道府県コード"));
                }

                //３．１．４．[変数：組合等コード] が未入力の場合
                if (string.IsNullOrEmpty(kumiaitoCd))
                {
                    //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１４．」へ進む。
                    //（"ME01054" 引数{0} ：組合等コード)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "組合等コード"));
                }

                //３．１．５．[変数：支所コード] が未入力の場合
                if (string.IsNullOrEmpty(shishoCd))
                {
                    //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１４．」へ進む。
                    //（"ME01054" 引数{0} ：支所コード)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "支所コード"));
                }

                //３．１．６．[変数：バッチ条件のキー情報] が未入力の場合
                if (string.IsNullOrEmpty(jid))
                {
                    //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１４．」へ進む。
                    //（"ME01054" 引数{0} ：バッチ条件のキー情報)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチ条件のキー情報"));
                }

                // ３．１．２．バッチ予約状況取得
                // バッチ予約状況取得引数の設定
                BatchUtil.GetBatchYoyakuListParam batchYoyakuListparam = new()
                {
                    SystemKbn = Core.CoreConst.SystemKbn.Nsk,
                    TodofukenCd = todofukenCd,
                    KumiaitoCd = kumiaitoCd,
                    ShishoCd = shishoCd,
                    BatchId = (long)nBid
                };
                // 総件数取得フラグ
                bool boolAllCntFlg = false;
                // 件数（出力パラメータ）
                int intAllCnt = 0;
                // エラーメッセージ（出力パラメータ）
                string message = string.Empty;
                // バッチ予約状況取得登録（BatchUtil.GetBatchYoyakuList()）を呼び出し、バッチ予約状況を取得する。
                List<BatchYoyaku> batchYoyakus = BatchUtil.GetBatchYoyakuList(batchYoyakuListparam, boolAllCntFlg, ref intAllCnt, ref message);

                // バッチ予約が存在しない場合、
                if (batchYoyakus.Count == 0)
                {
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01645" 引数{0} ：パラメータの取得)
                    throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
                }

                // バッチ予約が存在する場合
                BatchYoyaku? batchYoyaku = batchYoyakus.FirstOrDefault(x => x.BatchId == nBid);
                // [引数：バッチID]に一致する場合
                if (batchYoyaku is not null)
                {
                    // 取得した「バッチ予約状況」から値を取得し変数に設定する。
                    // バッチ予約ユーザID = バッチ予約情報.予約ユーザID
                    batchYoyakuId = batchYoyaku.BatchYoyakuId;
                }
                // [引数：バッチID]に一致するバッチ予約状況が取得できない場合、
                else
                {
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01645" 引数{0} ：パラメータの取得)
                    throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
                }

                // ４．DB接続
                // ※「共通機能設計_070_DB切替」の「バッチのDB接続先取得処理」を参照。
                // システム共通スキーマからログインユーザの所属に応じた都道府県別事業スキーマのDB接続先を取得する
                // DB接続情報
                DbConnectionInfo? dbInfo = null;
                dbInfo = DBUtil.GetDbConnectionInfo(Core.CoreConst.SystemKbn.Nsk, todofukenCd, kumiaitoCd, shishoCd);
                if (dbInfo == null)
                {
                    throw new AppException("ME90014", MessageUtil.Get("ME90014"));
                }

                using (NskAppContext dbContext = new(dbInfo.ConnectionString, dbInfo.DefaultSchema, ConfigUtil.GetInt(Core.CoreConst.COMMAND_TIMEOUT)))
                {
                    // バッチ条件情報
                    BatchJouken batchJouken = new();
                    // ５．コードの整合性チェック
                    batchJouken.IsConsistency(dbContext, todofukenCd, kumiaitoCd, shishoCd);

                    // ６．計算対象報告回・引受回取得
                    batchJouken.GetBatchJokens(dbContext, jid);

                    // ６．４．「報告回」を取得する。
                    List<Houkokukai> houkokukais = GetHoukokukai(dbContext, batchJouken, kumiaitoCd);
                    // 取得データが０件の場合
                    if (houkokukais.Count == 0)
                    {
                        // [変数：エラーメッセージ]を設定し、「１４．」へ進む。
                        // （"ME01645"　引数{0} ：報告回)
                        throw new AppException("ME01645", MessageUtil.Get("ME01645", "報告回"));
                    }

                    // ６．５．取得した「引受回・報告回」の判定
                    int futankinKofuKbnNumber = int.Parse(batchJouken.JoukenFutankinKofuKbn);

                    Houkokukai houkokukaiSuitou = new();
                    Houkokukai houkokukaiRikutou = new();
                    Houkokukai houkokukaiMugi = new();

                    switch (futankinKofuKbnNumber)
                    {
                        case (int)NskCommon.CoreConst.FutankinKofuKbnNumber.Ine:

                            foreach (Houkokukai houkoku in houkokukais)
                            {
                                int kyosaiMokutekiCdNumber = int.Parse(houkoku.共済目的コード);

                                switch (kyosaiMokutekiCdNumber)
                                {
                                    case (int)NskCommon.CoreConst.KyosaiMokutekiCdNumber.Suitou:

                                        houkokukaiSuitou = houkoku;
                                        break;

                                    case (int)NskCommon.CoreConst.KyosaiMokutekiCdNumber.Rikutou:

                                        houkokukaiRikutou = houkoku;
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;

                        case (int)NskCommon.CoreConst.FutankinKofuKbnNumber.Mugi:

                            foreach (Houkokukai houkoku in houkokukais)
                            {
                                int kyosaiMokutekiCdNumber = int.Parse(houkoku.共済目的コード);

                                if (kyosaiMokutekiCdNumber.Equals((int)NskCommon.CoreConst.KyosaiMokutekiCdNumber.Mugi))
                                {
                                    houkokukaiMugi = houkoku;
                                }
                            }
                            break;
                    }

                    // ７．トランザクションの開始
                    using (IDbContextTransaction dbTransaction = dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            // ８．交付金計算データクリア
                            KoufukinDataClear(dbContext, batchJouken, kumiaitoCd);
                            dbContext.SaveChanges();

                            // ９．組合等交付データ取得
                            List<KumiaitoKoufuData> kumiaitoKoufuData = GetKumiaitouKoufuData(dbContext, houkokukaiSuitou, houkokukaiRikutou, houkokukaiMugi, batchJouken, futankinKofuKbnNumber, kumiaitoCd);

                            // ９．１．取得した組合等交付データの判定
                            if (kumiaitoKoufuData.Count == 0)
                            {
                                // [変数：エラーメッセージ]を設定し、「１２．１．」へ進む。
                                // （"ME01645"　引数{0} ：組合等交付データ)
                                throw new AppException("ME01645", MessageUtil.Get("ME01645", "組合等交付データ"));
                            }

                            // １０．組合等交付計算結果登録
                            foreach (KumiaitoKoufuData kumiaitoKoufu in kumiaitoKoufuData)
                            {
                                EntryKumiaitoKoufuKeisanRecord(dbContext, batchJouken, kumiaitoKoufu, kumiaitoCd, batchYoyakuId);
                                dbContext.SaveChanges();
                            }

                            // １１．交付計算実施日更新
                            switch (futankinKofuKbnNumber)
                            {
                                case (int)NskCommon.CoreConst.FutankinKofuKbnNumber.Ine:

                                    string suitou = ((int)NskCommon.CoreConst.KyosaiMokutekiCdNumber.Suitou).ToString();
                                    EntryKoufuKeisanJisshibiRecord(dbContext, batchJouken, kumiaitoCd, batchYoyakuId, suitou);
                                    dbContext.SaveChanges();

                                    string rikutou = ((int)NskCommon.CoreConst.KyosaiMokutekiCdNumber.Rikutou).ToString();
                                    EntryKoufuKeisanJisshibiRecord(dbContext, batchJouken, kumiaitoCd, batchYoyakuId, rikutou);
                                    dbContext.SaveChanges();

                                    EntryKoufukaiRecord(dbContext, batchJouken, kumiaitoCd, batchYoyakuId, suitou);
                                    dbContext.SaveChanges();

                                    EntryKoufukaiRecord(dbContext, batchJouken, kumiaitoCd, batchYoyakuId, rikutou);
                                    dbContext.SaveChanges();

                                    break;

                                case (int)NskCommon.CoreConst.FutankinKofuKbnNumber.Mugi:

                                    string mugi = ((int)NskCommon.CoreConst.KyosaiMokutekiCdNumber.Mugi).ToString();
                                    EntryKoufuKeisanJisshibiRecord(dbContext, batchJouken, kumiaitoCd, batchYoyakuId, mugi);
                                    dbContext.SaveChanges();

                                    EntryKoufukaiRecord(dbContext, batchJouken, kumiaitoCd, batchYoyakuId, mugi);
                                    dbContext.SaveChanges();

                                    break;
                            }

                            // １２．トランザクションのコミット
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // １２．１．トランザクション中の例外処理
                            // トランザクションのロールバック
                            dbTransaction.Rollback();

                            // 「１４．」へ進む。
                            throw new Exception();
                        }
                    }

                    // 処理正常終了時
                    // [変数：処理ステータス] に"03"（成功）を設定
                    status = NskCommon.CoreConst.STATUS_SUCCESS;

                    // [変数：エラーメッセージ] に正常終了メッセージを設定
                    // （"MI10005"：処理が正常に終了しました。)
                    errorMessage = MessageUtil.Get("MI10005");
                    logger.Info(errorMessage);
                }

                // １３．バッチ実行状況更新
                string refMessage = string.Empty;

                // １３．１．『バッチ実行状況更新』インターフェースに引数を渡す。
                // １３．２．『バッチ実行状況更新』インターフェースから戻り値を受け取る。
                // バッチ実行状況更新（BatchUtil.UpdateBatchYoyakuSts()）を呼び出し、ステータスを更新する。
                if (BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), status, errorMessage, batchYoyakuId, ref refMessage) == BatchUtil.RET_FAIL)
                {
                    // （１）失敗の場合
                    logger.Error(string.Format(NskCommon.CoreConst.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, refMessage));
                    result = NskCommon.CoreConst.BATCH_EXECUT_FAILED;
                }
                else
                {
                    // （２）成功の場合
                    logger.Info(string.Format(NskCommon.CoreConst.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, refMessage));
                    result = NskCommon.CoreConst.BATCH_EXECUT_SUCCESS;
                }

                // １３．３．処理を終了する。
                Environment.ExitCode = result;
            }
            catch (Exception ex)
            {
                if (ex is AppException aEx)
                {
                    logger.Error(aEx.Message);
                }
                // １４．エラー処理
                string refMessage = string.Empty;

                // １４．１．例外（エクセプション）の場合
                // [変数：ステータス]を「99：エラー」に更新する。
                status = NskCommon.CoreConst.STATUS_ERROR;

                //  [変数：エラーメッセージ] にエラーメッセージを設定
                // （"MF00001"：予期せぬエラーが発生しました。システム管理者に連絡してください。)
                errorMessage = MessageUtil.Get("MF00001");

                // １４．２．共通機能の「バッチ実行状況更新」を呼び出し、バッチ予約テーブルを更新する。
                BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), status, errorMessage, batchYoyakuId, ref refMessage);
            }
        }

        /// <summary>
        /// 「報告回」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="batchJouken">バッチ条件情報</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns></returns>
        private static List<Houkokukai> GetHoukokukai(NskAppContext dbContext, BatchJouken batchJouken, string kumiaitoCd)
        {
            StringBuilder sql = new();

            sql.Append($"SELECT ");
            sql.Append($"   T2.共済目的コード ");
            sql.Append($",  T2.報告回 ");

            sql.Append($"FROM ");
            sql.Append($"   t_00050_交付回 T1 ");

            sql.Append($"INNER JOIN ");
            sql.Append($"   t_00040_報告回 T2 ");
            sql.Append($"ON ");
            sql.Append($"       T2.共済目的コード = T1.共済目的コード ");
            sql.Append($"   AND T2.年産 = T1.年産 ");
            sql.Append($"   AND T2.報告回 = T1.紐づけ報告回 ");
            sql.Append($"   AND T2.組合等コード = T1.組合等コード ");

            sql.Append($"WHERE ");
            sql.Append($"       T1.負担金交付区分 = @負担金交付区分 ");
            sql.Append($"   AND T1.交付回 = @交付回 ");
            sql.Append($"   AND T1.年産 = @年産 ");
            sql.Append($"   AND T1.組合等コード = @組合等コード ");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("負担金交付区分", batchJouken.JoukenFutankinKofuKbn),
                new("交付回", int.Parse(batchJouken.JoukenKoufuKai)),
                new("年産", int.Parse(batchJouken.JoukenNensan)),
                new("組合等コード", kumiaitoCd)
            ];

            // SQLのクエリ結果をListに格納する
            List<Houkokukai> Houkokukais = dbContext.Database.SqlQueryRaw<Houkokukai>(sql.ToString(), parameters.ToArray()).ToList();

            return Houkokukais;
        }

        /// <summary>
        /// 交付金計算データクリア
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="batchJouken">バッチ条件情報</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        private static void KoufukinDataClear(NskAppContext dbContext, BatchJouken batchJouken, string kumiaitoCd)
        {
            StringBuilder sql = new();

            sql.Append($"DELETE ");
            sql.Append($"FROM ");
            sql.Append($"   t_15020_組合等交付 ");

            sql.Append($"WHERE ");
            sql.Append($"       組合等コード = @組合等コード ");
            sql.Append($"   AND 年産 = @年産 ");
            sql.Append($"   AND 負担金交付区分 = @負担金交付区分 ");
            sql.Append($"   AND 交付回 = @交付回 ");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("組合等コード", kumiaitoCd),
                new("負担金交付区分", batchJouken.JoukenFutankinKofuKbn),
                new("交付回", int.Parse(batchJouken.JoukenKoufuKai)),
                new("年産", int.Parse(batchJouken.JoukenNensan))
            ];

            // SQLのクエリ結果をListに格納する
            dbContext.Database.ExecuteSqlRaw(sql.ToString(), parameters.ToArray());
        }

        /// <summary>
        /// 「組合等引受_合計部の集計値」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="houkokukaiSuitou">報告回（水稲）</param>
        /// <param name="houkokukaiRikutou">報告回（陸稲）</param>
        /// <param name="houkokukaiMugi">報告回（麦）</param>
        /// <param name="batchJouken">バッチ条件情報</param>
        /// <param name="futankinKofuKbnNumber">負担金交付区分番号</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        private static List<KumiaitoKoufuData> GetKumiaitouKoufuData
            (NskAppContext dbContext, Houkokukai houkokukaiSuitou, Houkokukai houkokukaiRikutou, Houkokukai houkokukaiMugi,
            BatchJouken batchJouken, int futankinKofuKbnNumber, string kumiaitoCd)
        {
            StringBuilder sql = new();

            sql.Append($"SELECT ");
            sql.Append($"   UT.組合等コード ");
            sql.Append($",  SUM( UT.引受面積 ) AS 引受面積 ");
            sql.Append($",  SUM( UT.引受収量 ) AS 引受収量 ");
            sql.Append($",  SUM( UT.共済金額 ) AS 共済金額 ");
            sql.Append($",  SUM( UT.保険料総額 ) AS 保険料総額 ");
            sql.Append($",  SUM( UT.組合等別国庫負担金 ) AS 組合等別国庫負担金 ");
            sql.Append($",  SUM( UT.組合員等負担共済掛金 ) AS 組合員等負担共済掛金 ");
            sql.Append($",  SUM( UT.徴収済額 ) AS 組合員等負担共済掛金徴収済額 ");
            sql.Append($",  TRUNC( SUM( UT.徴収済額 ) / SUM( UT.組合員等負担共済掛金 ) * 100,2 ) AS 共済掛金徴収割合 ");
            sql.Append($",  SUM( UT.組合等別国庫負担金 ) - SUM( UT.保険料総額 ) AS 組合等交付対象金額 ");

            sql.Append($"FROM ");
            sql.Append($"   ( ");

            switch (futankinKofuKbnNumber)
            {
                case (int)NskCommon.CoreConst.FutankinKofuKbnNumber.Ine:

                    // 水稲
                    sql.Append($"SELECT ");
                    sql.Append($"   ST.組合等コード ");
                    sql.Append($",  SUM( ST.組合等計引受面積 ) AS 引受面積 ");
                    sql.Append($",  SUM( ST.組合等計引受収量 ) AS 引受収量 ");
                    sql.Append($",  SUM( ST.組合等計共済金額 ) AS 共済金額 ");
                    sql.Append($",  SUM( ST.組合等計保険料 ) AS 保険料総額 ");
                    sql.Append($",  SUM( ST.組合等計交付対象負担金額 ) AS 組合等別国庫負担金 ");
                    sql.Append($",  SUM( ST.組合等計組合員等負担共済掛金 ) AS 組合員等負担共済掛金 ");
                    sql.Append($",  ( ");

                    sql.Append($"       SELECT ");
                    sql.Append($"           SSQ.掛金徴収済額 ");

                    sql.Append($"       FROM ");
                    sql.Append($"           t_15010_交付徴収 SSQ ");

                    sql.Append($"       WHERE ");
                    sql.Append($"               SSQ.共済目的コード = '11' ");
                    sql.Append($"           AND SSQ.年産 = @年産 ");
                    sql.Append($"           AND SSQ.交付回 = @交付回 ");
                    sql.Append($"           AND SSQ.組合等コード = ST.組合等コード ");

                    sql.Append($"   ) AS 徴収済額 ");

                    sql.Append($"FROM ");
                    sql.Append($"   t_13010_組合等引受_合計部 ST ");

                    sql.Append($"WHERE ");
                    sql.Append($"       ST.組合等コード = @組合等コード ");
                    sql.Append($"   AND ST.共済目的コード = '11' ");
                    sql.Append($"   AND ST.年産 = @年産 ");
                    sql.Append($"   AND ST.報告回 = @報告回_水稲 ");
                    sql.Append($"   AND ST.類区分 <> '0' ");

                    sql.Append($"GROUP BY ");
                    sql.Append($"   ST.組合等コード ");

                    sql.Append($"UNION ALL ");

                    // 陸稲
                    sql.Append($"SELECT ");
                    sql.Append($"   RT.組合等コード ");
                    sql.Append($",  SUM( RT.組合等計引受面積 ) AS 引受面積 ");
                    sql.Append($",  SUM( RT.組合等計引受収量 ) AS 引受収量 ");
                    sql.Append($",  SUM( RT.組合等計共済金額 ) AS 共済金額 ");
                    sql.Append($",  SUM( RT.組合等計保険料 ) AS 保険料総額 ");
                    sql.Append($",  SUM( RT.組合等計交付対象負担金額 ) AS 組合等別国庫負担金 ");
                    sql.Append($",  SUM( RT.組合等計組合員等負担共済掛金 ) AS 組合員等負担共済掛金 ");
                    sql.Append($",  ( ");

                    sql.Append($"       SELECT ");
                    sql.Append($"           RSQ.掛金徴収済額 ");

                    sql.Append($"       FROM ");
                    sql.Append($"           t_15010_交付徴収 RSQ ");

                    sql.Append($"       WHERE ");
                    sql.Append($"               RSQ.共済目的コード = '20' ");
                    sql.Append($"           AND RSQ.年産 = @年産 ");
                    sql.Append($"           AND RSQ.交付回 = @交付回 ");
                    sql.Append($"           AND RSQ.組合等コード = RT.組合等コード ");

                    sql.Append($"   ) AS 徴収済額 ");

                    sql.Append($"FROM ");
                    sql.Append($"   t_13010_組合等引受_合計部 RT ");

                    sql.Append($"WHERE ");
                    sql.Append($"       RT.組合等コード = @組合等コード ");
                    sql.Append($"   AND RT.共済目的コード = '20' ");
                    sql.Append($"   AND RT.年産 = @年産 ");
                    sql.Append($"   AND RT.報告回 = @報告回_陸稲 ");
                    sql.Append($"   AND RT.類区分 <> '0' ");

                    sql.Append($"GROUP BY ");
                    sql.Append($"   RT.組合等コード ");

                    break;

                case (int)NskCommon.CoreConst.FutankinKofuKbnNumber.Mugi:

                    // 麦
                    sql.Append($"SELECT ");
                    sql.Append($"   MT.組合等コード ");
                    sql.Append($",  SUM( MT.組合等計引受面積 ) AS 引受面積 ");
                    sql.Append($",  SUM( MT.組合等計引受収量 ) AS 引受収量 ");
                    sql.Append($",  SUM( MT.組合等計共済金額 ) AS 共済金額 ");
                    sql.Append($",  SUM( MT.組合等計保険料 ) AS 保険料総額 ");
                    sql.Append($",  SUM( MT.組合等計交付対象負担金額 ) AS 組合等別国庫負担金 ");
                    sql.Append($",  SUM( MT.組合等計組合員等負担共済掛金 ) AS 組合員等負担共済掛金 ");
                    sql.Append($",  ( ");

                    sql.Append($"       SELECT ");
                    sql.Append($"           MSQ.掛金徴収済額 ");

                    sql.Append($"       FROM ");
                    sql.Append($"           t_15010_交付徴収 MSQ ");

                    sql.Append($"       WHERE ");
                    sql.Append($"               MSQ.共済目的コード = '30' ");
                    sql.Append($"           AND MSQ.年産 = @年産 ");
                    sql.Append($"           AND MSQ.交付回 = @交付回 ");
                    sql.Append($"           AND MSQ.組合等コード = MT.組合等コード ");

                    sql.Append($"   ) AS 徴収済額 ");

                    sql.Append($"FROM ");
                    sql.Append($"   t_13010_組合等引受_合計部 MT ");

                    sql.Append($"WHERE ");
                    sql.Append($"       MT.組合等コード = @組合等コード ");
                    sql.Append($"   AND MT.共済目的コード = '30' ");
                    sql.Append($"   AND MT.年産 = @年産 ");
                    sql.Append($"   AND MT.報告回 = @報告回_麦 ");
                    sql.Append($"   AND MT.類区分 <> '0' ");

                    sql.Append($"GROUP BY ");
                    sql.Append($"   MT.組合等コード ");

                    break;
            }

            sql.Append($"   ) UT ");
            sql.Append($"GROUP BY ");
            sql.Append($"   UT.組合等コード ");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("年産", int.Parse(batchJouken.JoukenNensan)),
                new("交付回", int.Parse(batchJouken.JoukenKoufuKai)),
                new("組合等コード", kumiaitoCd)
            ];
            switch (futankinKofuKbnNumber)
            {
                case (int)NskCommon.CoreConst.FutankinKofuKbnNumber.Ine:
                    parameters.Add(new("報告回_水稲", houkokukaiSuitou.報告回));
                    parameters.Add(new("報告回_陸稲", houkokukaiRikutou.報告回));
                    break;

                case (int)NskCommon.CoreConst.FutankinKofuKbnNumber.Mugi:
                    parameters.Add(new("報告回_麦", houkokukaiMugi.報告回));
                    break;
            }

            // SQLのクエリ結果をListに格納する
            List<KumiaitoKoufuData> kumiaitouKoufuDatas = dbContext.Database.SqlQueryRaw<KumiaitoKoufuData>(sql.ToString(), parameters.ToArray()).ToList();

            return kumiaitouKoufuDatas;
        }

        /// <summary>
        /// 組合等交付計算結果登録
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="batchJouken">バッチ条件情報</param>
        /// <param name="kumiaitoKoufu">組合等交付データ</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="batchYoyakuId">バッチ予約ID</param>
        private static void EntryKumiaitoKoufuKeisanRecord(NskAppContext dbContext, BatchJouken batchJouken, KumiaitoKoufuData kumiaitoKoufu, string kumiaitoCd, string batchYoyakuId)
        {
            StringBuilder sql = new();

            sql.Append($"INSERT INTO t_15020_組合等交付 ( ");

            sql.Append($"   組合等コード ");
            sql.Append($",  年産 ");
            sql.Append($",  負担金交付区分 ");
            sql.Append($",  交付回 ");
            sql.Append($",  引受面積 ");
            sql.Append($",  引受収量 ");
            sql.Append($",  共済金額 ");
            sql.Append($",  保険料総額 ");
            sql.Append($",  組合等別国庫負担金 ");
            sql.Append($",  組合員等負担共済掛金 ");
            sql.Append($",  組合員等負担共済掛金徴収済額 ");
            sql.Append($",  共済掛金徴収割合 ");
            sql.Append($",  組合等交付金の金額 ");
            sql.Append($",  登録日時 ");
            sql.Append($",  登録ユーザid ");
            sql.Append($",  更新日時 ");
            sql.Append($",  更新ユーザid ");

            sql.Append($") VALUES ( ");

            sql.Append($"   @組合等コード ");
            sql.Append($",  @年産 ");
            sql.Append($",  @負担金交付区分 ");
            sql.Append($",  @交付回 ");
            sql.Append($",  @引受面積 ");
            sql.Append($",  @引受収量 ");
            sql.Append($",  @共済金額 ");
            sql.Append($",  @保険料総額 ");
            sql.Append($",  @組合等別国庫負担金 ");
            sql.Append($",  @組合員等負担共済掛金 ");
            sql.Append($",  @組合員等負担共済掛金徴収済額 ");
            sql.Append($",  @共済掛金徴収割合 ");
            sql.Append($",  @組合等交付金の金額 ");
            sql.Append($",  @登録日時 ");
            sql.Append($",  @登録ユーザid ");
            sql.Append($",  @更新日時 ");
            sql.Append($",  @更新ユーザid ");

            sql.Append($") ");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("組合等コード", kumiaitoCd),
                new("年産", int.Parse(batchJouken.JoukenNensan)),
                new("負担金交付区分", batchJouken.JoukenFutankinKofuKbn),
                new("交付回", int.Parse(batchJouken.JoukenKoufuKai)),
                new("引受面積", kumiaitoKoufu.引受面積),
                new("引受収量", kumiaitoKoufu.引受収量),
                new("共済金額", kumiaitoKoufu.共済金額),
                new("保険料総額", kumiaitoKoufu.保険料総額),
                new("組合等別国庫負担金", kumiaitoKoufu.組合等別国庫負担金),
                new("組合員等負担共済掛金", kumiaitoKoufu.組合員等負担共済掛金),
                new("組合員等負担共済掛金徴収済額", kumiaitoKoufu.組合員等負担共済掛金徴収済額),
                new("共済掛金徴収割合", kumiaitoKoufu.共済掛金徴収割合),
                new("組合等交付金の金額", Math.Truncate(kumiaitoKoufu.組合等交付対象金額 * kumiaitoKoufu.共済掛金徴収割合 / 100)),
                new("登録日時", DateUtil.GetSysDateTime()),
                new("登録ユーザid", batchYoyakuId),
                new("更新日時", DateUtil.GetSysDateTime()),
                new("更新ユーザid", batchYoyakuId)
            ];

            // SQLのクエリ結果をListに格納する
            dbContext.Database.ExecuteSqlRaw(sql.ToString(), parameters.ToArray());
        }
        /// <summary>
        /// 組合等交付計算結果登録
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="batchJouken">バッチ条件情報</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="kyosaiMokutekiCd">共済目的コード</param>
        /// <param name="batchYoyakuId">バッチ予約ID</param>
        private static void EntryKoufukaiRecord(NskAppContext dbContext, BatchJouken batchJouken, string kumiaitoCd, string batchYoyakuId, string KyosaiMokutekiCdNumber)
        {
            StringBuilder sql = new();

            sql.Append($"INSERT INTO t_00050_交付回 ( ");

            sql.Append($"   組合等コード ");
            sql.Append($",  共済目的コード ");
            sql.Append($",  年産 ");
            sql.Append($",  負担金交付区分 ");
            sql.Append($",  交付回 ");
            sql.Append($",  登録日時 ");
            sql.Append($",  登録ユーザid ");
            sql.Append($",  更新日時 ");
            sql.Append($",  更新ユーザid ");

            sql.Append($") VALUES ( ");

            sql.Append($"   @組合等コード ");
            sql.Append($",  @共済目的コード ");
            sql.Append($",  @年産 ");
            sql.Append($",  @負担金交付区分 ");
            sql.Append($",  @交付回 ");
            sql.Append($",  @登録日時 ");
            sql.Append($",  @登録ユーザid ");
            sql.Append($",  @更新日時 ");
            sql.Append($",  @更新ユーザid ");

            sql.Append($") ");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("組合等コード", kumiaitoCd),
                new("共済目的コード", KyosaiMokutekiCdNumber),
                new("年産", int.Parse(batchJouken.JoukenNensan)),
                new("負担金交付区分", batchJouken.JoukenFutankinKofuKbn),
                new("交付回", int.Parse(batchJouken.JoukenKoufuKai) + 1),
                new("登録日時", DateUtil.GetSysDateTime()),
                new("登録ユーザid", batchYoyakuId),
                new("更新日時", DateUtil.GetSysDateTime()),
                new("更新ユーザid", batchYoyakuId)
            ];

            // SQLのクエリ結果をListに格納する
            dbContext.Database.ExecuteSqlRaw(sql.ToString(), parameters.ToArray());
        }

        /// <summary>
        /// 交付計算実施日更新
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="batchJouken">バッチ条件情報</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="batchYoyakuId">バッチ予約ID</param>
        /// <param name="KyosaiMokutekiCdNumber">共済目的コード番号</param>
        private static void EntryKoufuKeisanJisshibiRecord(NskAppContext dbContext, BatchJouken batchJouken, string kumiaitoCd, string batchYoyakuId, string KyosaiMokutekiCdNumber)
        {
            StringBuilder sql = new();

            sql.Append($"UPDATE t_00050_交付回 ");

            sql.Append($"SET ");
            sql.Append($"    交付計算実施日 = @システム日付 ");
            sql.Append($",   更新日時 = @システム日時 ");
            sql.Append($",   更新ユーザid = @ユーザID ");

            sql.Append($"WHERE ");
            sql.Append($"    組合等コード = @組合等コード ");
            sql.Append($"AND 共済目的コード = @共済目的コード ");
            sql.Append($"AND 年産 = @年産 ");
            sql.Append($"AND 負担金交付区分 = @負担金交付区分 ");
            sql.Append($"AND 交付回 = @交付回 ");

            // パラメータに値を付与する
            List<NpgsqlParameter> parameters =
            [
                new("システム日付", DateUtil.GetSysDateTime().Date),
                new("システム日時", DateUtil.GetSysDateTime()),
                new("ユーザid", batchYoyakuId),
                new("組合等コード", kumiaitoCd),
                new("共済目的コード", KyosaiMokutekiCdNumber),
                new("年産", int.Parse(batchJouken.JoukenNensan)),
                new("負担金交付区分", batchJouken.JoukenFutankinKofuKbn),
                new("交付回", int.Parse(batchJouken.JoukenKoufuKai))
            ];

            // SQLのクエリ結果をListに格納する
            dbContext.Database.ExecuteSqlRaw(sql.ToString(), parameters.ToArray());
        }

    }
}
