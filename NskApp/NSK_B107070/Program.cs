using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using ModelLibrary.Context;
using NLog;
using NskAppModelLibrary.Context;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;
using NskReportMain.Controllers;
using System.Text;
using Core = CoreLibrary.Core.Consts;
using NskCommon = NskCommonLibrary.Core.Consts;

namespace NSK_B107070
{
    /// <summary>
    /// 一括帳票出力本体プログラム
    /// </summary>
    class Program
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 
        /// </summary>
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
        /// 細目データ出力処理カンマ編集データ
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

            // [変数：エラーメッセージ] string.Empty
            string errorMessage = string.Empty;
            // [変数：ステータス]       STATUS_SUCCESS = "03"（成功）
            string status = NskCommon.CoreConst.STATUS_SUCCESS;
            // バッチID(数値)
            long nBid = 0;
            // 処理結果（正常：0、エラー：1）
            int result = NskCommon.CoreConst.BATCH_EXECUT_SUCCESS;
            // バッチ予約ユーザID
            string batchYoyakuId = string.Empty;

            try
            {
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

                //１．設定ファイルから、以下の内容を取得し、グローバル変数へ保存する。
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
                    // エラーメッセージをログに出力し、処理を中断する。
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

                // ３．引数のチェック
                // ３．１．必須チェック
                // ３．１．１．[変数：バッチID] が未入力の場合
                if (string.IsNullOrEmpty(bid))
                {
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０.」へ進む。
                    // （"ME01054" 引数{0} ：バッチID）
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチID"));
                }

                // ３．１．２．[変数：バッチID]が数値変換不可の場合
                // 数値化したバッチID
                if (!long.TryParse(bid, out nBid))
                {
                    // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    // （"ME90012"　引数{0} ：バッチID)
                    throw new AppException("ME90012", MessageUtil.Get("ME90012", "バッチID"));
                }

                // ３．１．３．[変数：都道府県コード]が未入力の場合
                if (string.IsNullOrEmpty(todofukenCd))
                {
                    // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    // （"ME01054"　引数{0} ：都道府県コード)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "都道府県コード"));
                }

                //３．１．４．[変数：組合等コード] が未入力の場合
                if (string.IsNullOrEmpty(kumiaitoCd))
                {
                    //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01054" 引数{0} ：組合等コード)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "組合等コード"));
                }

                //３．１．５．[変数：バッチ条件のキー情報] が未入力の場合
                if (string.IsNullOrEmpty(jid))
                {
                    //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01054" 引数{0} ：バッチ条件のキー情報)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチ条件のキー情報"));
                }

                // バッチ予約状況取得引数の設定
                BatchUtil.GetBatchYoyakuListParam param = new()
                {
                    BatchId = nBid
                };

                SystemCommonContext db = new();
                ModelLibrary.Models.TBatchYoyaku? batchYoyaku = db.TBatchYoyakus.FirstOrDefault(x =>
                    (x.BatchId == param.BatchId) &&
                    (x.DeleteFlg == BatchUtil.DELETE_FLG_NOT_DELETED));

                // バッチ予約が存在する場合
                if (batchYoyaku != null)
                {
                    // [引数：バッチID]に一致する場合
                    // 取得した「バッチ予約状況」から値を取得し変数に設定する。
                    // バッチ予約ユーザID = バッチ予約情報.予約ユーザID
                    batchYoyakuId = batchYoyaku.BatchYoyakuId;
                }
                else
                {
                    // バッチ予約が存在しない場合、
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01645" 引数{0} ：予約ユーザID)
                    throw new AppException("ME01645", MessageUtil.Get("ME01645", "予約ユーザIDの取得"));
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
                    // ５．コードの整合性チェック
                    // ５．１．「都道府県コード存在情報」を取得する。
                    int todofuken = GetTodofukenCdSonzaiJoho(dbContext, todofukenCd);

                    // ５．２．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                    if (todofuken == 0)
                    {
                        // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME10005"　引数{0} ：都道府県コード)
                        throw new AppException("ME10005", MessageUtil.Get("ME10005", "都道府県コード"));
                    }

                    // ５．３．[変数：組合等コード] が入力されている場合
                    // 「組合等コード存在情報」を取得する。
                    int kumiaito = GetKumiaitoCdSonzaiJoho(dbContext, todofukenCd, kumiaitoCd);

                    // ５．４．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                    if (kumiaito == 0)
                    {
                        // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                        // （"ME10005"　引数{0} ：組合等コード)
                        throw new AppException("ME10005", MessageUtil.Get("ME10005", "組合等コード"));
                    }

                    // ５．５．[変数：支所コード]が入力されている場合
                    if (!string.IsNullOrEmpty(shishoCd))
                    {
                        // 「支所コード存在情報」を取得する。
                        int shisho = GetShishoCdSonzaiJoho(dbContext, todofukenCd, kumiaitoCd, shishoCd);

                        // ５．６．データが取得できない場合（該当データがマスタデータに登録されていない場合）
                        if (shisho == 0)
                        {
                            // 以下のエラーメッセージを設定し、ERRORログに出力して「１０.」へ進む。
                            // （"ME10005"　引数{0} ：支所コード)
                            throw new AppException("ME10005", MessageUtil.Get("ME10005", "支所コード"));
                        }
                    }

                    // ６．帳票を作成する。
                    using (var reportController = new NskReportP107070Controller(dbInfo))
                    {
                        ControllerResult controllerResult = null;
                        // ６．１．「帳票制御処理設計書_NSK_107070P_徴収管理簿」を呼び出す。
                        controllerResult = reportController.ManageReports("NSK_107070P", jid, todofukenCd, kumiaitoCd, shishoCd, nBid);

                        if (controllerResult.Result == ReportConst.RESULT_SUCCESS)
                        {
                            // ６．１．１．成功した(戻り値(0)）場合
                            // [変数：処理ステータス] に"03"（成功）を設定
                            status = NskCommon.CoreConst.STATUS_SUCCESS;
                            // [変数：エラーメッセージ] に正常終了メッセージを設定
                            // （"MI10005"：処理が正常に終了しました。)
                            errorMessage = MessageUtil.Get("MI10005");
                            // 処理結果（正常：0）
                            result = NskCommon.CoreConst.BATCH_EXECUT_SUCCESS;
                        }
                        else
                        {
                            // ６．１．２．失敗した(戻り値(0)以外）場合
                            // [変数：処理ステータス] に"03"（成功）を設定
                            status = NskCommon.CoreConst.STATUS_ERROR;
                            // 戻り値で返されたエラーメッセージを、[変数：エラーメッセージ]に設定
                            errorMessage = controllerResult.ErrorMessage;
                            // 処理結果（異常：1）
                            result = NskCommon.CoreConst.BATCH_EXECUT_FAILED;
                        }
                    }
                }

                // ７．バッチ実行状況更新
                result = SetBatchYoyakuSts(nBid, errorMessage, status, batchYoyakuId, result);
            }
            catch (Exception ex)
            {
                long.TryParse(bid, out nBid);

                // ８．１．例外（エクセプション）の場合
                //  [変数：処理ステータス] に"99"（エラー）を設定
                status = NskCommon.CoreConst.STATUS_ERROR;
                // 処理結果（異常：1）
                result = NskCommon.CoreConst.BATCH_EXECUT_FAILED;

                // 各処理でエラーの場合
                if (ex is AppException)
                {
                    // エラーログ設定
                    logger.Error(ex.Message);

                    // ７．バッチ実行状況更新
                    SetBatchYoyakuSts(nBid, ex.Message, status, batchYoyakuId, result);
                }
                // 各処理でのエラー以外（例外）の場合
                else
                {
                    //  [変数：エラーメッセージ] にエラーメッセージを設定
                    // （"MF00001")
                    errorMessage = MessageUtil.Get("MF00001");

                    // 例外の内容をログに出力する。
                    logger.Error(ex.Message + string.Join(string.Empty, new string[]
                    {
                        ex.InnerException == null ? string.Empty : ReportConst.NEW_LINE_SEPARATOR + ex.InnerException.ToString(),
                        string.IsNullOrEmpty(ex.StackTrace) ? string.Empty : ReportConst.NEW_LINE_SEPARATOR + ex.StackTrace
                    }));

                    // ８．２．共通機能の「バッチ実行状況更新」を呼び出し、バッチ予約テーブルを更新する。
                    string refMessage = string.Empty;
                    BatchUtil.UpdateBatchYoyakuSts(nBid, status, errorMessage, batchYoyakuId, ref refMessage);
                }
            }

            Environment.ExitCode = result;
        }

        /// <summary>
        /// 「都道府県コード存在情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        private static int GetTodofukenCdSonzaiJoho(NskAppContext dbContext, string todofukenCd)
        {
            int todofuken = dbContext.VTodofukens
                .Where(x => x.TodofukenCd == todofukenCd)
                .Count();

            return todofuken;
        }

        /// <summary>
        /// 「組合等コード存在情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns></returns>
        private static int GetKumiaitoCdSonzaiJoho(NskAppContext dbContext, string todofukenCd, string kumiaitoCd)
        {
            int kumiaito = dbContext.VKumiaitos
                 .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd)
                 .Count();

            return kumiaito;
        }

        /// <summary>
        /// 「支所コード存在情報」を取得する。
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <returns></returns>
        private static int GetShishoCdSonzaiJoho(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            int shisho = dbContext.VShishoNms
                .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd && x.ShishoCd == shishoCd)
                .Count();

            return shisho;
        }

        /// <summary>
        /// ７．バッチ実行状況更新
        /// </summary>
        /// <param name="bid">バッチID</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        /// <param name="status">ステータス</param>
        /// <param name="batchYoyakuId">バッチ予約ID</param>
        /// <param name="result">バッチ予約ID</param>
        /// <returns></returns>
        private static int SetBatchYoyakuSts(long bid, string errorMessage, string status, string batchYoyakuId, int result)
        {
            string refMessage = string.Empty;

            // ７．１．『バッチ実行状況更新』インターフェースに引数を渡す。
            // ７．２．『バッチ実行状況更新』インターフェースから戻り値を受け取る。
            if (BatchUtil.UpdateBatchYoyakuSts(bid, status, errorMessage, batchYoyakuId, ref refMessage) == BatchUtil.RET_FAIL)
            {
                // （１）失敗の場合
                logger.Error(string.Format(NskCommon.CoreConst.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, errorMessage));
                result = NskCommon.CoreConst.BATCH_EXECUT_FAILED;
            }
            else
            {
                // （２）成功の場合
                logger.Info(string.Format(NskCommon.CoreConst.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, bid.ToString(), status, errorMessage));
            }

            // ７．３．処理を終了する。
            return result;
        }
    }
}