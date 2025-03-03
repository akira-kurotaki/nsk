using BAS_B0001_BatchReport.Common;
using NskAppModelLibrary.Context;
using NskReportMain.Common;
using NskReportMain.Controllers;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Utility;
using ModelLibrary.Models;
using NLog;
using NskReportLibrary.Core.Consts;
using System.Diagnostics;
using System.Text;

namespace BAS_B0001_BatchReport
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
        /// 一括帳票出力実行
        /// </summary>
        /// <param name="args">
        /// 引数配列要素1：バッチID
        /// 引数配列要素2：都道府県コード
        /// 引数配列要素3：組合等コード
        /// 引数配列要素4：支所コード
        /// 引数配列要素5：バッチ条件のキー情報
        /// </param>
        static void Main(string[] args)
        {
            // args = new string[] { "2", "01", "001", "", "50" }; // テスト用

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // appsettings.jsonに設定あるシステム時間フラグ
            bool sysDateTimeFlag = false;

            // appsettings.jsonに設定あるシステム時間ファイルパス
            string sysDateTimePath = string.Empty;

            // エラーメッセージ
            string message = null;

            // ステータス
            string status = Constants.STATUS_ERROR;

            // 処理結果（正常：0、エラー：1）
            int result = Constants.BATCH_EXECUT_SUCCESS;

            // バッチID
            var bid = 0L;
            // 都道府県コード
            var todofukenCd = string.Empty;
            // 組合等コード
            var kumiaitoCd = string.Empty;
            // 支所コード
            var shishoCd = string.Empty;
            // バッチ条件のキー情報
            var jid = 0;

            /// DB接続情報
            DbConnectionInfo dbInfo = null;

            // 帳票作成の実行結果
            ControllerResult controllerResult = null;
            // 帳票条件リスト
            List<TReportJouken> tReportJoukenList = null;

            try
            {
                // システム時間フラグ
                bool isSysDateTimeFlag = bool.TryParse(ConfigUtil.Get(Constants.SYS_DATE_TIME_FLAG), out sysDateTimeFlag);
                if (!isSysDateTimeFlag)
                {
                    // エラーメッセージをログに出力し、処理を中断する。
                    Throw("ME90015", "システム時間フラグ");
                }

                if (sysDateTimeFlag)
                {
                    // システム時間ファイルパス
                    sysDateTimePath = ConfigUtil.Get(Constants.SYS_DATE_TIME_PATH);
                    if (string.IsNullOrEmpty(sysDateTimePath))
                    {
                        // エラーメッセージをログに出力し、処理を中断する。
                        Throw("ME90015", "システム時間ファイルパス");
                    }
                }

                // システム日時
                var systemDate = DateUtil.GetSysDateTime();

                logger.Info(string.Format(
                    Constants.METHOD_BEGIN_LOG,
                    Constants.CLASS_NM_REPORT_OUTPUT,
                    args[0],
                    string.Empty,
                    string.Join(Constants.PARAM_SEPARATOR, new string[]{
                        Constants.PARAM_NAME_BATCH_ID + Constants.PARAM_NAME_VALUE_SEPARATOR + args[0],
                        Constants.PARAM_NAME_TODOFUKEN_CD + Constants.PARAM_NAME_VALUE_SEPARATOR + args[1],
                        Constants.PARAM_NAME_KUMIAITO_CD + Constants.PARAM_NAME_VALUE_SEPARATOR + args[2],
                        Constants.PARAM_NAME_SHISHO_CD + Constants.PARAM_NAME_VALUE_SEPARATOR + args[3],
                        Constants.PARAM_NAME_BATCH_JOKEN + Constants.PARAM_NAME_VALUE_SEPARATOR + args[4] })));

                // 引数チェック
                result = CheckArguments(args, ref message);

                if (result == Constants.BATCH_EXECUT_SUCCESS)
                {
                    // 引数配列から引数を取得
                    // バッチID
                    bid = long.Parse(args[0]);
                    // 都道府県コード
                    todofukenCd = args[1];
                    // 組合等コード
                    kumiaitoCd = args[2];
                    // 支所コード
                    shishoCd = args[3];
                    // バッチ条件のキー情報
                    jid = int.Parse(args[4]);

                    // システム共通スキーマからログインユーザの所属に応じた都道府県別事業スキーマのDB接続先を取得する
                    dbInfo = DBUtil.GetDbConnectionInfo(Constants.KYOSAI_JIGYO_CD, todofukenCd, kumiaitoCd, shishoCd);
                    if (dbInfo == null)
                    {
                        message = MessageUtil.Get("ME90014");
                        result = Constants.BATCH_EXECUT_FAILED;
                    }
                }

                // コードの整合性チェック
                if (result == Constants.BATCH_EXECUT_SUCCESS)
                {
                    using (var db = new NskAppContext(dbInfo.ConnectionString, dbInfo.DefaultSchema, ConfigUtil.GetInt("CommandTimeout")))
                    {
                        // コードの整合性チェック
                        result = CheckMaster(db, todofukenCd, kumiaitoCd, shishoCd, ref message);

                        if (result == Constants.BATCH_EXECUT_SUCCESS)
                        {
                            // 「帳票条件リスト」を取得する。
                            List<string> joukenNmList = [JoukenNameConst.JOUKEN_REPORT_CONTROL_ID, JoukenNameConst.JOUKEN_REPORT_PATH, JoukenNameConst.JOUKEN_SHISHO_LIST];
                            tReportJoukenList = db.TReportJoukens.Where(a => a.JoukenId == jid && joukenNmList.Contains(a.JoukenNm)).ToList();

                            if (tReportJoukenList.IsNullOrEmpty())
                            {
                                message = MessageUtil.Get("ME90010");
                                result = Constants.BATCH_EXECUT_FAILED;
                            }
                        }
                    }
                }

                // 帳票作成
                if (result == Constants.BATCH_EXECUT_SUCCESS)
                {
                    // 帳票制御ID
                    var reportControlId = string.Empty;
                    // 帳票パス
                    var filePath = string.Empty;
                    // 利用可能な支所一覧
                    var shishoCdList = new List<string>();

                    foreach (TReportJouken tReportJouken in tReportJoukenList)
                    {
                        if (JoukenNameConst.JOUKEN_REPORT_CONTROL_ID.Equals(tReportJouken.JoukenNm))
                        {
                            reportControlId = tReportJouken.JoukenValue;
                        }
                        else if (JoukenNameConst.JOUKEN_REPORT_PATH.Equals(tReportJouken.JoukenNm))
                        {
                            filePath = tReportJouken.JoukenValue;
                        }
                        else if (JoukenNameConst.JOUKEN_SHISHO_LIST.Equals(tReportJouken.JoukenNm))
                        {
                            if (!string.IsNullOrEmpty(tReportJouken.JoukenValue))
                            {
                                var arryShisho = tReportJouken.JoukenValue.Split(ReportConst.SYMBOL_COMMA);
                                shishoCdList = new List<string>(arryShisho);
                            }
                        }
                    }

                    // 帳票制御IDにより、該当帳票出力制御クラスを呼び出す
                    // ■ 帳票出力制御クラスに以下の引数を渡す
                    // ・ユーザID
                    // ・条件ID
                    // ・バッチフラグ（1：バッチ）
                    // ・帳票パス
                    // ■ 帳票出力制御クラスは以下の戻り値を返却する
                    // ・処理結果（正常：0、エラー：1）
                    // ・エラーメッセージ

                    // 出力対象帳票制御クラスの帳票作成メソッドを呼び出す
                    switch (reportControlId)
                    {
                        case "H001":
                            // 「帳票制御処理_H001_加入者情報（一覧）」を呼び出す。
                            using (var reportController = new NskReportH001Controller(dbInfo))
                            {
                                controllerResult = reportController.ManageReports(Constants.BATCH_USER_ID, jid, todofukenCd, kumiaitoCd, shishoCd, shishoCdList, ReportConst.REPORT_BATCH, filePath, bid);
                            }

                            break;

                        case "H002":
                            // 「帳票制御処理_H002_加入者情報（単票）」を呼び出す。
                            using (var reportController = new NskReportH002Controller(dbInfo))
                            {
                                controllerResult = reportController.ManageReports(Constants.BATCH_USER_ID, jid, todofukenCd, kumiaitoCd, shishoCd, shishoCdList, ReportConst.REPORT_BATCH, filePath, bid);
                            }
                            break;

                        default:
                            message = MessageUtil.Get("ME90011", reportControlId);
                            break;
                    }

                    if (controllerResult != null)
                    {
                        // 戻り値を設定する
                        if (controllerResult.Result == ReportConst.RESULT_SUCCESS)
                        {
                            status = Constants.STATUS_SUCCESS;
                        }
                        else
                        {
                            message = controllerResult.ErrorMessage;
                        }
                    }
                }

                // バッチ実行状況更新
                string refMessage = string.Empty;
                if (BatchUtil.UpdateBatchYoyakuSts(bid, status, message, Constants.BATCH_USER_ID, ref refMessage) == 0)
                {
                    // 更新に失敗した場合
                    logger.Error(refMessage);
                    Console.Error.WriteLine(refMessage);
                    logger.Error(string.Format(Constants.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, message));
                    Console.Error.WriteLine(message + string.Join(Constants.PARAM_SEPARATOR, args));
                    result = Constants.BATCH_EXECUT_FAILED;
                }
                else
                {
                    // 更新に成功した場合
                    logger.Info(string.Format(Constants.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, message));
                    Console.Out.WriteLine(message + string.Join(Constants.PARAM_SEPARATOR, args));
                    result = Constants.BATCH_EXECUT_SUCCESS;
                }

                // 処理時間
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + ReportConst.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + ReportConst.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Info(string.Format(
                                ReportConst.METHOD_END_LOG,
                                ReportConst.CLASS_NM_REPORT_OUTPUT,
                                bid,
                                excutingTime));
            }
            catch (Exception ex)
            {
                message = ex.Message + string.Join(string.Empty, new string[]{
                    ex.InnerException == null ? string.Empty : ReportConst.NEW_LINE_SEPARATOR + ex.InnerException.ToString(),
                    string.IsNullOrEmpty(ex.StackTrace) ? string.Empty : ReportConst.NEW_LINE_SEPARATOR + ex.StackTrace
                });

                // 戻り値を設定する
                if (ex is AppException)
                {
                    Console.Error.WriteLine(ex.Message);
                }
                else
                {
                    Console.Error.WriteLine(MessageUtil.Get("MF00001"));
                    message = MessageUtil.Get("MF00001") + ReportConst.NEW_LINE_SEPARATOR + message;
                }
                logger.Error(string.Format(ReportConst.ERROR_LOG, bid, message));

                Environment.ExitCode = Constants.BATCH_EXECUT_FAILED;
            }
            finally
            {
                // ロガーシャットダウン
                LogManager.Shutdown();
            }

            Environment.ExitCode = result;
        }

        /// <summary>
        /// 引数チェック
        /// </summary>
        /// <param name="args">プログラム引数</param>
        /// <param name="message">メッセージ（出力パラメータ）</param>
        /// <returns>処理結果（正常：0、エラー：1）</returns>
        private static int CheckArguments(string[] args, ref string message)
        {
            // 引数配列の要素数をチェックする
            if (args.Length != 5)
            {
                Throw("ME90013", string.Empty);
            }

            // バッチID
            if (string.IsNullOrEmpty(args[0]))
            {
                Throw("ME90009", "バッチID");
            }

            long bid = 0L;
            if (!long.TryParse(args[0], out bid))
            {
                Throw("ME90012", "バッチID");
            }

            // 都道府県コード
            if (string.IsNullOrEmpty(args[1]))
            {
                Throw("ME90009", "都道府県コード");
            }

            // バッチ条件のキー情報
            if (string.IsNullOrEmpty(args[4]))
            {
                message = MessageUtil.Get("ME90009", "バッチ条件のキー情報");
                return Constants.BATCH_EXECUT_FAILED;
            }

            int jid = 0;
            if (!int.TryParse(args[4], out jid))
            {
                message = MessageUtil.Get("ME90012", "バッチ条件のキー情報");
                return Constants.BATCH_EXECUT_FAILED;
            }

            return Constants.BATCH_EXECUT_SUCCESS;
        }

        /// <summary>
        /// コードの整合性チェック
        /// </summary>
        /// <param name="db">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="message">メッセージ（出力パラメータ）</param>
        /// <returns>処理結果（正常：0、エラー：1）</returns>
        private static int CheckMaster(NskAppContext db, string todofukenCd, string kumiaitoCd, string shishoCd, ref string message)
        {
            // 「都道府県コード存在情報」を取得する。
            var todofuken = db.VTodofukens.Where(a => a.TodofukenCd == todofukenCd).SingleOrDefault();
            if (todofuken == null)
            {
                message = MessageUtil.Get("ME91003", "都道府県コード", "都道府県マスタ");
                return Constants.BATCH_EXECUT_FAILED;
            }

            // [変数：組合等コード]が入力されている場合、「組合等コード存在情報」を取得する。
            if (!string.IsNullOrEmpty(kumiaitoCd))
            {
                var kumiaito = db.VKumiaitos.Where(a => a.TodofukenCd == todofukenCd && a.KumiaitoCd == kumiaitoCd).SingleOrDefault();
                if (kumiaito == null)
                {
                    message = MessageUtil.Get("ME91003", "組合等コード", "組合等マスタ");
                    return Constants.BATCH_EXECUT_FAILED;
                }
            }

            // [変数：支所コード]が入力されている場合、「支所コード存在情報」を取得する。
            if (!string.IsNullOrEmpty(shishoCd))
            {
                var shisho = db.VShishoNms.Where(a => a.TodofukenCd == todofukenCd && a.KumiaitoCd == kumiaitoCd && a.ShishoCd == shishoCd).SingleOrDefault();
                if (shisho == null)
                {
                    message = MessageUtil.Get("ME91003", "組合等コード", "組合等マスタ");
                    return Constants.BATCH_EXECUT_FAILED;
                }
            }

            return Constants.BATCH_EXECUT_SUCCESS;
        }

        /// <summary>
        /// AppExceptionをスローする
        /// </summary>
        /// <param name="messageId">メッセージID</param>
        /// <param name="arg">置換用文字列の配列(可変長)</param>
        /// <exception cref="AppException">AppException</exception>
        internal static void Throw(string messageId, params string[] arg) =>
            throw new AppException(messageId, MessageUtil.Get(messageId, arg));
    }
}