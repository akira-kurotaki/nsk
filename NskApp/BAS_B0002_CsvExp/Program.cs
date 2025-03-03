using BAS_B0002_CsvExp.Common;
using NskAppModelLibrary.Context;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Utility;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;
using NskReportLibrary.Core.Consts;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace BAS_B0002_CsvExp
{
    /// <summary>
    /// CSV出力本体プログラム
    /// </summary>
    class Program
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 初期化
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
        /// 一括CSV出力実行
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
            // args = new string[] { "59", "04", "004", "44", "mode=TBL,filenm=CsvUtilTest_20241220111152.zip" }; // テスト用

            logger.Info(string.Format(
            Constants.METHOD_BEGIN_LOG,
            Constants.CLASS_NM_CSV_OUTPUT,
            args[0],
            string.Empty,
            string.Join(Constants.PARAM_SEPARATOR, new string[]{
                        Constants.PARAM_NAME_BATCH_ID + Constants.PARAM_NAME_VALUE_SEPARATOR + args[0],
                        Constants.PARAM_NAME_TODOFUKEN_CD + Constants.PARAM_NAME_VALUE_SEPARATOR + args[1],
                        Constants.PARAM_NAME_KUMIAITO_CD + Constants.PARAM_NAME_VALUE_SEPARATOR + args[2],
                        Constants.PARAM_NAME_SHISHO_CD + Constants.PARAM_NAME_VALUE_SEPARATOR + args[3],
                        Constants.PARAM_NAME_BATCH_JOKEN + Constants.PARAM_NAME_VALUE_SEPARATOR + args[4] })));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // エラーメッセージ
            string message = null;

            // ステータス
            string status = Constants.STATUS_SUCCESS;
            // 処理結果（正常：0、エラー：1）
            int result = Constants.BATCH_EXECUT_SUCCESS;

            // appsettings.jsonに設定あるシステム時間フラグ
            bool sysDateTimeFlag = false;
            // appsettings.jsonに設定あるシステム時間ファイルパス
            string sysDateTimePath = string.Empty;

            // CSVの一時出力フォルダ
            var tempFolder = string.Empty;

            // バッチID
            var bid = 0L;
            // 都道府県コード
            var todofukenCd = string.Empty;
            // 組合等コード
            var kumiaitoCd = string.Empty;
            // 支所コード
            var shishoCd = string.Empty;
            // バッチ条件のキー情報
            var jid = string.Empty;
            // [変数：処理モード]
            var mode = string.Empty;
            // [変数：zipファイル名]
            var zipFileNm = string.Empty;
            // システム区分
            var systemKbn = string.Empty;

            /// DB接続情報
            DbConnectionInfo dbInfo = null;

            try
            {
                try
                {
                    // システム時間フラグ
                    sysDateTimeFlag = ConfigUtil.GetBool(Constants.SYS_DATE_TIME_FLAG);
                }
                catch (Exception e)
                {
                    logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                    // エラーメッセージをログに出力し、処理を中断する。
                    Throw("ME90015", "システム時間フラグ");
                }

                // システム時間フラグ=trueの場合
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

                // 引数チェック
                result = CheckArguments(args, ref message, ref status, ref mode, ref zipFileNm);

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
                jid = args[4];

                List<TBatchCsvQuery> csvJoukenList = null;
                // DB接続
                if (result == Constants.BATCH_EXECUT_SUCCESS)
                {

                    try
                    {
                        // システム共通DBに接続する
                        using (var db = new SystemCommonContext(ConfigUtil.GetInt("CommandTimeout")))
                        {
                            logger.Info("「システム区分」の取得を行います。");
                            // 引数：バッチIDをキーにして、システム共通スキーマの「バッチCSV取得条件」テーブルから「バッチCSV取得条件リスト」を取得
                            systemKbn = db.TBatchYoyakus.Where(t => t.BatchId == bid).Select(t => t.SystemKbn).SingleOrDefault();
                        }
                    }
                    catch (Exception e)
                    {
                        // エラーログに以下のメッセージを出力する		
                        // メッセージ：「バッチCSV取得条件」が取得できませんでした。
                        logger.Error("「システム区分」が取得できませんでした。");
                        // ※例外が発生した場合は例外の内容も出力する。	
                        logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));

                        // 変数を以下に設定して８へ		
                        // [変数：エラーメッセージ]：「バッチCSV取得条件」が取得できませんでした。	
                        // [変数：ステータス]："99"（失敗）	
                        result = Constants.BATCH_EXECUT_FAILED;
                        message = "「システム区分」が取得できませんでした。";
                        status = Constants.STATUS_ERROR;
                    }

                    // システム共通スキーマからログインユーザの所属に応じた都道府県別事業スキーマのDB接続先を取得する
                    dbInfo = DBUtil.GetDbConnectionInfo(systemKbn, todofukenCd, kumiaitoCd, shishoCd);
                    if (dbInfo == null)
                    {
                        Throw("ME90014");
                    }

                    try
                    {
                        // システム共通DBに接続する
                        using (var db = new SystemCommonContext(ConfigUtil.GetInt("CommandTimeout")))
                        {
                            logger.Info("「バッチCSV取得条件リスト」の取得を行います。");
                            // 引数：バッチIDをキーにして、システム共通スキーマの「バッチCSV取得条件」テーブルから「バッチCSV取得条件リスト」を取得
                            csvJoukenList = db.TBatchCsvQuerys.Where(t => t.BatchId == bid).OrderBy(t => t.SerialNumber).ToList();
                            // 「バッチCSV取得条件リスト」が0件または例外で取得できない場合
                            if (csvJoukenList.IsNullOrEmpty())
                            {
                                throw new ArgumentException("「バッチCSV取得条件」が取得できませんでした。");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // エラーログに以下のメッセージを出力する		
                        // メッセージ：「バッチCSV取得条件」が取得できませんでした。
                        logger.Error("「バッチCSV取得条件」が取得できませんでした。");
                        // ※例外が発生した場合は例外の内容も出力する。	
                        logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));

                        // 変数を以下に設定して８へ		
                        // [変数：エラーメッセージ]：「バッチCSV取得条件」が取得できませんでした。	
                        // [変数：ステータス]："99"（失敗）	
                        result = Constants.BATCH_EXECUT_FAILED;
                        message = "「バッチCSV取得条件」が取得できませんでした。";
                        status = Constants.STATUS_ERROR;
                    }
                }

                // CSV作成
                if (result == Constants.BATCH_EXECUT_SUCCESS)
                {
                    // 定数（設定ファイル）：CSV一時出力フォルダ\yyyyMMdd\HHmmss\（GUIDを生成したフォルダ名）\[変数：zipファイル名]の拡張子以外
                    tempFolder = Path.Combine(ConfigUtil.Get(Constants.FILE_TEMP_FOLDER_PATH),
                                                  systemDate.ToString("yyyyMMdd"),
                                                  systemDate.ToString("HHmmss"),
                                                  Guid.NewGuid().ToString(),
                                                  Path.GetFileNameWithoutExtension(zipFileNm));

                    // CSV一時出力フォルダがすでに存在する場合、削除する
                    if (File.Exists(tempFolder))
                    {
                        File.Delete(tempFolder);
                    }
                    // CSVの一時出力フォルダを作成する。
                    Directory.CreateDirectory(tempFolder);

                    // 「バッチCSV取得条件リスト」の件数分、繰り返す
                    foreach (var csvJouken in csvJoukenList)
                    {
                        var csvMessage = string.Empty;
                        var sqlParams = string.IsNullOrEmpty(csvJouken.QueryParam) ? new List<SqlParam>() : JsonSerializer.Deserialize<List<SqlParam>>(csvJouken.QueryParam);
                        using (var jigyoBb = new NskAppContext(dbInfo.ConnectionString, dbInfo.DefaultSchema, ConfigUtil.GetInt("CommandTimeout")))
                        {
                            // [変数：処理モード]が"TBL"の場合、CSVファイル取得共通部品を「バッチCSV取得条件リスト」の件数分、繰り返し、呼び出す
                            if (CsvUtil.BATCH_JOKEN_MODE_TBL.Equals(mode))
                            {
                                CsvUtil.CsvFileOutput(tempFolder,
                                                        csvJouken.TargetEntity,
                                                        csvJouken.QueryCondition,
                                                        sqlParams,
                                                        csvJouken.Sort,
                                                        csvJouken.CharacterCd,
                                                        csvJouken.CsvFileName,
                                                        Constants.FLG_1.Equals(csvJouken.HeaderUmu) ? true : false,
                                                        csvJouken.Separator,
                                                        Constants.FLG_1.Equals(csvJouken.BomCdUmu) ? true : false,
                                                        jigyoBb,
                                                        ref csvMessage);

                            }
                            else
                            {
                                var headerList = string.IsNullOrEmpty(csvJouken.HeaderList) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(csvJouken.HeaderList);
                                // [変数：処理モード]が"SQL"の場合、CSVファイル取得共通部品（SQL設定）を「バッチCSV取得条件リスト」の件数分、繰り返し、呼び出す
                                CsvUtil.CsvFileOutputSql(tempFolder,
                                                            csvJouken.QueryCondition,
                                                            sqlParams,
                                                            csvJouken.CharacterCd,
                                                            csvJouken.CsvFileName,
                                                            Constants.FLG_1.Equals(csvJouken.HeaderUmu) ? true : false,
                                                            headerList,
                                                            csvJouken.Separator,
                                                            Constants.FLG_1.Equals(csvJouken.BomCdUmu) ? true : false,
                                                            jigyoBb,
                                                            ref csvMessage
                                                            );
                            }

                            if (!string.IsNullOrEmpty(csvMessage))
                            {
                                // エラーが発生した場合		
                                // エラーログに以下のメッセージを出力する	
                                // メッセージ：CSVファイルの出力に失敗しました。バッチID：「バッチCSV取得条件リスト」.バッチID、連番：「バッチCSV取得条件リスト」.連番
                                logger.Error("CSVファイルの出力に失敗しました。バッチID：" + csvJouken.BatchId + "、連番：" + csvJouken.SerialNumber);
                                // ※例外が発生した場合は例外の内容も出力する。
                                // 変数を以下に設定して８へ	
                                // [変数：エラーメッセージ]：CSVファイルの出力に失敗しました。バッチID：「バッチCSV取得条件リスト」.バッチID、連番：「バッチCSV取得条件リスト」.連番
                                // [変数：ステータス]："99"（失敗）
                                result = Constants.BATCH_EXECUT_FAILED;
                                message = "CSVファイルの出力に失敗しました。バッチID：" + csvJouken.BatchId + "、連番：" + csvJouken.SerialNumber;
                                status = Constants.STATUS_ERROR;
                                break;
                            }
                        }
                    }

                    // Zip暗号化を行う
                    if (result == Constants.BATCH_EXECUT_SUCCESS)
                    {
                        // Zip暗号化を行う
                        var zipFilePath = ZipUtil.CreateZip(tempFolder);

                        // Zipファイルを引数（設定ファイル）：zip保管フォルダ\yyyyMMdd\HHmmss\（GUIDを生成したフォルダ名）に移動する。
                        var filePath = Path.Combine(ConfigUtil.Get(Constants.CSV_OUTPUT_FOLDER_PATH),
                                                  systemDate.ToString("yyyyMMdd"),
                                                  systemDate.ToString("HHmmss"),
                                                  Guid.NewGuid().ToString(),
                                                  zipFileNm);
                        FolderUtil.MoveFile(zipFilePath, filePath, Constants.BATCH_USER_ID, bid);
                    }

                    // PDF出力用一時フォルダを削除する
                    FolderUtil.DeleteTempFolder(tempFolder);
                }

                // バッチ実行状況更新
                string refMessage = string.Empty;
                if (BatchUtil.UpdateBatchYoyakuSts(bid, status, message, Constants.BATCH_USER_ID, ref refMessage) == 0)
                {
                    // 更新に失敗した場合
                    logger.Error(refMessage);
                    logger.Error(string.Format(Constants.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, refMessage));
                    Console.Error.WriteLine(string.Format(Constants.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, refMessage));
                    result = Constants.BATCH_EXECUT_FAILED;
                }
                else
                {
                    // 更新に成功した場合
                    logger.Info(string.Format(Constants.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status));
                    Console.Out.WriteLine(string.Format(Constants.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status));
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
                // エラーログに出力する
                logger.Error(MessageUtil.GetErrorMessage(ex, CoreConst.LOG_MAX_INNER_EXCEPTION));

                // 戻り値を設定する
                if (ex is AppException)
                {
                    Console.Error.WriteLine(ex.Message);
                    message = ex.Message;
                }
                else
                {
                    Console.Error.WriteLine(MessageUtil.Get("MF00001"));
                    message = MessageUtil.Get("MF00001") + Environment.NewLine + message;
                }
                logger.Error(string.Format(ReportConst.ERROR_LOG, bid, message));

                // PDF出力用一時フォルダを削除する
                if (!string.IsNullOrEmpty(tempFolder))
                {
                    FolderUtil.DeleteTempFolder(tempFolder);
                }

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
        /// <param name="args">引数</param>
        /// <param name="message">[変数：エラーメッセージ]</param>
        /// <param name="status">[変数：ステータス]</param>
        /// <param name="mode">[変数：処理モード]</param>
        /// <param name="zipFileNm">[変数：zipファイル名]</param>
        /// <returns>処理結果（0：成功、1：失敗）</returns>
        private static int CheckArguments(string[] args, ref string message, ref string status, ref string mode, ref string zipFileNm)
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
                status = Constants.STATUS_ERROR;
                return Constants.BATCH_EXECUT_FAILED;
            }

            // 処理モード、zipファイル名チェック
            // [引数：バッチ条件のキー情報]から、処理モード、および、zipファイル名を取得し、[変数：処理モード]、[変数：zipファイル名]に設定する。
            var modeFileNm = GetModeFileNm(args[4]);
            mode = modeFileNm.GetValueOrDefault("mode", "");
            zipFileNm = modeFileNm.GetValueOrDefault("filenm", "");

            // [変数：処理モード]が未設定の場合、[変数：ステータス]、[変数：エラーメッセージ]を設定し、「８．」へ進む。
            if (string.IsNullOrEmpty(mode))
            {
                message = MessageUtil.Get("ME90009", "処理モード");
                status = Constants.STATUS_ERROR;
                return Constants.BATCH_EXECUT_FAILED;
            }

            // [変数：処理モード]が設定されている、かつ、[変数：処理モード]が"TBL"、もしくは、"SQL"ではない場合、、[変数：ステータス]、
            // [変数：エラーメッセージ]を設定し、「８．」へ進む。
            // （"引数{0}の値が不正です。システム管理者に連絡してください。"　引数{0}："処理モード"）
            if (!(CsvUtil.BATCH_JOKEN_MODE_TBL.Equals(mode) || CsvUtil.BATCH_JOKEN_MODE_SQL.Equals(mode)))
            {
                message = MessageUtil.Get("ME90012", "処理モード");
                status = Constants.STATUS_ERROR;
                return Constants.BATCH_EXECUT_FAILED;
            }

            // [変数：zipファイル名]が未設定の場合、[変数：ステータス]、[変数：エラーメッセージ]を設定し、「８．」へ進む。
            // （"{0}が引数に設定されていません。システム管理者に連絡してください。"　引数{0}："zipファイル名"）
            if (string.IsNullOrEmpty(zipFileNm))
            {
                message = MessageUtil.Get("ME90009", "zipファイル名");
                status = Constants.STATUS_ERROR;
                return Constants.BATCH_EXECUT_FAILED;
            }

            return Constants.BATCH_EXECUT_SUCCESS;
        }

        /// <summary>
        /// [変数：処理モード]、[変数：zipファイル名]を取得する
        /// </summary>
        /// <param name="input">[引数：バッチ条件のキー情報]</param>
        /// <returns>[変数：処理モード]、[変数：zipファイル名]情報</returns>
        private static Dictionary<string, string> GetModeFileNm(string input)
        {
            var dic = new Dictionary<string, string>();
            var keyValuePairs = input.Split(',');

            foreach (var pair in keyValuePairs)
            {
                var parts = pair.Split(new[] { '=' }, 2);
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();
                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                    {
                        dic[key] = value;
                    }
                }
            }

            return dic;
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