using NSK_B102121.Common;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Utility;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;
using Npgsql;
using System.Diagnostics;
using System.Text;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NpgsqlTypes;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Data.Common;
using System.IO.Compression;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace NSK_B102121
{
    /// <summary>
    /// 定時実行予約登録
    /// </summary>
    class Program
    {
        /// <summary>
        /// バッチ名
        /// </summary>
        private static string BATCH_NAME = "危険段階データ取込（告示料率）（テキスト）";
        private static string BATCH_USER_NAME = "NSK_B102121";

        static string defaultSchemaSystemCommon = string.Empty;
        static string defaultSchemaJigyoCommon = string.Empty;
        static string sysDateTimeFlag = string.Empty;
        static string sysDateTimePath = string.Empty;

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
            logger.Info(string.Concat(CoreConst.LOG_START_KEYWORD, " " + BATCH_NAME));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var bid = string.Empty;
            var todofukenCd = string.Empty;
            var kumiaitoCd = string.Empty;
            var shishoCd = string.Empty;
            var jid = string.Empty;
            //バッチ条件テーブルから取得する条件
            var nensan = string.Empty;
            var fileHash = string.Empty;
            var filePath = string.Empty;

            bool forcedShutdownFlg = false;
            var shoriSts = Constants.TASK_FAILED;
            var errorMsg = string.Empty;
            string acceptanceErrorContent = "";

            int result = Constants.BATCH_EXECUT_SUCCESS;
            string extractedFilePath = string.Empty;


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

            logger.Debug($"bid: {bid}");
            logger.Debug($"todofukenCd: {todofukenCd}");
            logger.Debug($"kumiaitoCd: {kumiaitoCd}");
            logger.Debug($"shishoCd: {shishoCd}");
            logger.Debug($"jid: {jid}");

            var sysDate = DateUtil.GetSysDateTime();
            defaultSchemaSystemCommon = ConfigUtil.Get(Constants.DEFAULT_SCHEMA_SYSTEM_COMMON);
            defaultSchemaJigyoCommon = ConfigUtil.Get(Constants.DEFAULT_SCHEMA_JIGYO_COMMON);

            if (defaultSchemaJigyoCommon == null || "".Equals(defaultSchemaJigyoCommon))
            {
                //ERROR
                acceptanceErrorContent = MessageUtil.Get("ME90015", Constants.DEFAULT_SCHEMA_JIGYO_COMMON);
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, acceptanceErrorContent);
                return;
            }

            if (defaultSchemaSystemCommon == null || "".Equals(defaultSchemaSystemCommon))
            {
                //ERROR
                acceptanceErrorContent = MessageUtil.Get("ME90015", Constants.DEFAULT_SCHEMA_JIGYO_COMMON);
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, acceptanceErrorContent);
                return;
            }

            if (bid == null || "".Equals(bid))
            {
                acceptanceErrorContent = MessageUtil.Get("ME01054", "バッチID");
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, acceptanceErrorContent);
                return;
            }

            if (todofukenCd == null || "".Equals(todofukenCd))
            {
                acceptanceErrorContent = MessageUtil.Get("ME01054", "都道府県コード");
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, acceptanceErrorContent);
                return;
            }

            if (kumiaitoCd == null || "".Equals(kumiaitoCd))
            {
                acceptanceErrorContent = MessageUtil.Get("ME01054", "組合等コード");
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, acceptanceErrorContent);
                return;
            }
            if (jid == null || "".Equals(jid))
            {
                acceptanceErrorContent = MessageUtil.Get("ME01054", "条件ID");
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, acceptanceErrorContent);
                return;
            }

            // バッチのDB接続先取得処理
            DbConnectionInfo dbConnectionInfo =
                DBUtil.GetDbConnectionInfo(ConfigUtil.Get(Constants.SYSTEM_KBN)
                , todofukenCd
                , kumiaitoCd
                , shishoCd);

            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema, ConfigUtil.GetInt(Constants.CONFIG_COMMAND_TIMEOUT)))
            {
                var transaction = db.Database.BeginTransaction();
                try
                {
                    var bulkData = new List<W00120料率>();
                    int batchSize = 1000;
                    int counter = 0;

                    // バッチ条件情報取得
                    List<T01050バッチ条件> joukenList = GetBatchJoukenList(db, jid, Constants.JOUKEN_NENSAN, Constants.JOUKEN_FILE_PATH, Constants.JOUKEN_FILE_HASH);
                    if (joukenList.IsNullOrEmpty())
                    {
                        acceptanceErrorContent = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    for (int i = 0; i < joukenList.Count; i++)
                    {
                        if (Constants.JOUKEN_NENSAN.Equals(joukenList[i].条件名称))
                        {
                            nensan += joukenList[i].条件値;
                        }
                        if (Constants.JOUKEN_FILE_PATH.Equals(joukenList[i].条件名称))
                        {
                            filePath += joukenList[i].条件値;
                        }
                        if (Constants.JOUKEN_FILE_HASH.Equals(joukenList[i].条件名称))
                        {
                            fileHash += joukenList[i].条件値;
                        }
                    }
                    
                    if (string.IsNullOrEmpty(nensan))
                    {
                        acceptanceErrorContent = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    if (string.IsNullOrEmpty(fileHash))
                    {
                        acceptanceErrorContent = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    if (string.IsNullOrEmpty(filePath))
                    {
                        acceptanceErrorContent = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    // 都道府県マスタチェック情報
                    int todoCount = GetTodofukenCount(db, todofukenCd);
                    if (todoCount == 0)
                    {
                        acceptanceErrorContent = MessageUtil.Get("ME10005", "都道府県コード");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    //組合等マスタチェック情報
                    int kumiCount = GetKumiaitoCount(db, todofukenCd, kumiaitoCd);
                    if (kumiCount == 0)
                    {
                        acceptanceErrorContent = MessageUtil.Get("ME10005", "組合等コード");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    // ----- 復号化側の処理 -----
                    /*
                     * TODO:ファイル格納先と処理のロジックが決まってないため、
                     * 必要に応じて修正
                     */
                    // 1. ZIPファイルパスの取得
                    string zipFilePath = Directory.GetFiles(filePath, "*.zip").FirstOrDefault();
                    if (string.IsNullOrEmpty(zipFilePath))
                    {
                        // エラー処理
                        acceptanceErrorContent = MessageUtil.Get("ME01645", "ファイル取得");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    // 2. ZIPファイルと同じフォルダに解凍する
                    string extractFolder = Path.GetDirectoryName(zipFilePath);
                    try
                    {
                        // ※同名フォルダが存在すると例外となるので注意
                        ZipFile.ExtractToDirectory(zipFilePath, extractFolder);
                    }
                    catch (Exception fileEx)
                    {
                        acceptanceErrorContent = MessageUtil.Get("ME01645", "ファイルの解凍");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    // 3. 解凍先から対象のTXTファイルを取得（txtであることが前提）
                    extractedFilePath = Directory.GetFiles(extractFolder, "*.*", SearchOption.TopDirectoryOnly)
                                                        .FirstOrDefault(f => Path.GetExtension(f).ToLower() == ".txt");

                    logger.Debug("extractedFilePath :::: " + extractedFilePath);
                    if (string.IsNullOrEmpty(extractedFilePath))
                    {
                        logger.Error("ERROR: 解凍されたTXTファイルが見つかりません。");
                        //acceptanceErrorContent = MessageUtil.Get("ME10050");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    // ファイル拡張子の再チェック（念のため）
                    if (Path.GetExtension(extractedFilePath)?.ToLower() != ".txt")
                    {
                        logger.Error("ERROR: ファイル拡張子の再チェック");
                        acceptanceErrorContent = MessageUtil.Get("ME10050");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    

                    // 4. ファイルの読み込み
                    byte[] extractedFileData = File.ReadAllBytes(extractedFilePath);

                    // 5. 暗号化時に使用した元ファイル名を、復号化時にも使用する
                    //  filePath は "C:\SYN\2011Real\20250219111614\_0303000001\test2011" のようになっているので、
                    //  最後のディレクトリ名（"test2011"）を取得して ".txt" を付与する
                    string originalFileName = new DirectoryInfo(filePath).Name;  // "test2011"
                    string expectedFileName = originalFileName + ".txt";          // "test2011.txt"

                    // 6. ファイルの復号化（expectedFileName を使用）
                    byte[] decryptedData = CryptoUtil.Decrypt(extractedFileData, expectedFileName);
                    string hashByFile = CryptoUtil.GetSHA256Hex(decryptedData);
                    if (!hashByFile.Equals(fileHash))
                    {
                        acceptanceErrorContent = MessageUtil.Get("ME10052", "ファイル名");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    // 8. 復号化された内容を文字列として取得
                    string fileContent = Encoding.UTF8.GetString(decryptedData);


                    // 10. ファイル全体を行単位に分割（改行コード CRLF）
                    string[] allLines = fileContent.Split(new[] { "\r\n" }, StringSplitOptions.None);

                    // 11. ヘッダ行のチェック
                    if (allLines.Length < 1 || string.IsNullOrWhiteSpace(allLines[0]))
                    {
                        acceptanceErrorContent = MessageUtil.Get("ME90015", "ヘッダー");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    string headerLine = allLines[0];
                    string[] headerFields = headerLine.Split(',');
                    for (int i = 0; i < headerFields.Length; i++)
                    {
                        logger.Debug("headerFields[i] : " + headerFields[i]);
                    }
                    if (headerFields.Length != 16)
                    {
                        acceptanceErrorContent = MessageUtil.Get("ME10066");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    if (allLines.Length < 2)
                    {
                        acceptanceErrorContent = MessageUtil.Get("ME90015", "データ行");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    DeleteAllRyoritu(db);

                    // 13. 初期変数の設定
                    int lineNumber = 1; // ヘッダ行を1行目とする

                    // 14. データ行（2行目以降）のチェック
                    for (int i = 1; i < allLines.Length; i++)
                    {
                        string line = allLines[i];
                        // 14.2 次の行がない（EOF）または空白行の場合、終了
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            break;
                        }

                        // 14.3 行番号の更新
                        lineNumber++;

                        // 14.4 項目数チェック
                        string[] fields = line.Split(',');
                        if (fields.Length != 16)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10066");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }

                        // 14.5 各項目のチェック
                        acceptanceErrorContent = "";

                        // 1. 組合等コード（必須、数字、文字数3）
                        if (string.IsNullOrWhiteSpace(fields[0].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "組合等コード", "("+ lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (!IsNumeric(fields[0].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "組合等コード", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (fields[0].Trim('"').Length > 3)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "組合等コード", "3", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }

                        // 2. 年産（必須、数字、文字数4）
                        if (string.IsNullOrWhiteSpace(fields[1].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "年産", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (!IsNumeric(fields[1].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "年産", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (fields[1].Trim('"').Length > 4)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "年産", "4", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }

                        // 3. 共済目的コード（必須、数字、文字数2）
                        if (string.IsNullOrWhiteSpace(fields[2].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "共済目的コード", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (!IsNumeric(fields[2].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "共済目的コード", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (fields[2].Trim('"').Length > 2)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "共済目的コード", "2", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }

                        // 4. 合併時識別コード（必須、数字、文字数3）
                        if (string.IsNullOrWhiteSpace(fields[3].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "合併時識別コード", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (!IsNumeric(fields[3].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "合併時識別コード", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (fields[3].Trim('"').Length > 13)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "合併時識別コード", "13", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }

                        // 5. 類区分（必須、数字、文字数2）
                        if (string.IsNullOrWhiteSpace(fields[4].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "類区分", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (!IsNumeric(fields[4].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "類区分", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (fields[4].Trim('"').Length > 2)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "類区分", "2", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }

                        // 6. 引受方式（必須、数字、文字数5）
                        if (string.IsNullOrWhiteSpace(fields[5].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "引受方式", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (!IsNumeric(fields[5].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "引受方式", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (fields[5].Trim('"').Length > 2)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "引受方式", "2", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        // 7. 特約区分（必須、数字、文字数1）
                        if (string.IsNullOrWhiteSpace(fields[6].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "引受方式", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (!IsNumeric(fields[6].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "引受方式", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (fields[6].Trim('"').Length > 1)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "引受方式", "2", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        // 8. 補償割合コード（必須、数字、文字数5）
                        if (string.IsNullOrWhiteSpace(fields[7].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "補償割合コード", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (!IsNumeric(fields[7].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "補償割合コード", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (fields[7].Trim('"').Length > 2)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "補償割合コード", "2", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        // 9. 統計単位地域コード（必須、数字、文字数5）
                        if (string.IsNullOrWhiteSpace(fields[8].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "統計単位地域コード", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (!IsNumeric(fields[8].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "統計単位地域コード", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (fields[8].Trim('"').Length > 5)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "統計単位地域コード", "5", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        // 10. 責任保険歩合（数値、文字数2）※必須ではないので、値がある場合のみチェック
                        if (!string.IsNullOrWhiteSpace(fields[9].Trim('"')))
                        {
                            if (!IsNumeric(fields[9].Trim('"')) || fields[9].Trim('"').Replace(".", "").Length > 2)
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00020", "責任保険歩合", "2", "(" + lineNumber + "行目)");
                                forcedShutdownFlg = true;
                                goto forcedShutdown;
                            }
                        }
                        string pattern63 = @"^\d{1,3}\.\d{1,3}$";
                        // 11. 共済掛金標準率（数値、文字数6,3）※必須ではないので、値がある場合のみチェック
                        if (!string.IsNullOrWhiteSpace(fields[10].Trim('"')))
                        {
                            
                            if (!Regex.IsMatch(fields[10].Trim('"'), pattern63))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00004", "共済掛金標準率", "3", "3", "(" + lineNumber + "行目)");
                                forcedShutdownFlg = true;
                                goto forcedShutdown;
                            }
                        }
                        // 12. 通常標準被害率（数値、文字数6,3）※必須ではないので、値がある場合のみチェック
                        if (!string.IsNullOrWhiteSpace(fields[11].Trim('"')))
                        {

                            if (!Regex.IsMatch(fields[11].Trim('"'), pattern63))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00004", "通常標準被害率", "3", "3", "(" + lineNumber + "行目)");
                                forcedShutdownFlg = true;
                                goto forcedShutdown;
                            }
                        }
                        // 13. 保険料基礎率（数値、文字数6,3）※必須ではないので、値がある場合のみチェック
                        if (!string.IsNullOrWhiteSpace(fields[12].Trim('"')))
                        {

                            if (!Regex.IsMatch(fields[12].Trim('"'), pattern63))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00004", "保険料基礎率", "3", "3", "(" + lineNumber + "行目)");
                                forcedShutdownFlg = true;
                                goto forcedShutdown;
                            }
                        }
                        // 14. 異常標準被害率（数値、文字数6,3）※必須ではないので、値がある場合のみチェック
                        if (!string.IsNullOrWhiteSpace(fields[13].Trim('"')))
                        {

                            if (!Regex.IsMatch(fields[13].Trim('"'), pattern63))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00004", "異常標準被害率", "3", "3", "(" + lineNumber + "行目)");
                                forcedShutdownFlg = true;
                                goto forcedShutdown;
                            }
                        }
                        // 15. 再保険料基礎率（数値、文字数6,3）※必須ではないので、値がある場合のみチェック
                        if (!string.IsNullOrWhiteSpace(fields[14].Trim('"')))
                        {

                            if (!Regex.IsMatch(fields[14].Trim('"'), pattern63))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00004", "再保険料基礎率", "3", "3", "(" + lineNumber + "行目)");
                                forcedShutdownFlg = true;
                                goto forcedShutdown;
                            }
                        }
                        string pattern31 = @"^\d{1,2}\.\d{1}$";
                        // 16. 国庫負担割合（数値、文字数3,1）※必須ではないので、値がある場合のみチェック
                        if (!string.IsNullOrWhiteSpace(fields[15].Trim('"')))
                        {

                            if (!Regex.IsMatch(fields[15].Trim('"'), pattern31))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00004", "国庫負担割合", "2", "1", "(" + lineNumber + "行目)");
                                forcedShutdownFlg = true;
                                goto forcedShutdown;
                            }
                        }
                       
                        // 各変数にファイルの各項目を設定
                        string 組合等コード = fields[0].Trim('"');
                        string 年産 = fields[1].Trim('"');
                        string 共済目的コード = fields[2].Trim('"');
                        string 合併時識別コード = fields[3].Trim('"');
                        string 類区分 = fields[4].Trim('"');
                        string 引受方式 = fields[5].Trim('"');
                        string 特約区分 = fields[6].Trim('"');
                        string 補償割合コード = fields[7].Trim('"');
                        string 統計単位地域コード = fields[8].Trim('"');
                        string 責任保険歩合 = fields[9].Trim('"');
                        string 共済掛金標準率 = fields[10].Trim('"');
                        string 通常標準被害率 = fields[11].Trim('"');
                        string 保険料基礎率 = fields[12].Trim('"');
                        string 異常標準被害率 = fields[13].Trim('"');
                        string 再保険料基礎率 = fields[14].Trim('"');
                        string 国庫負担割合 = fields[15].Trim('"');

                        var entity = new W00120料率
                        {
                            組合等コード = 組合等コード,
                            年産 = short.Parse(年産),
                            共済目的コード = 共済目的コード,
                            合併時識別コード = 合併時識別コード,
                            類区分 = 類区分,
                            引受方式 = 引受方式,
                            特約区分 = 特約区分,
                            補償割合コード = 補償割合コード,
                            統計単位地域コード = 統計単位地域コード,
                            責任保険歩合 = decimal.Parse(責任保険歩合),
                            共済掛金標準率 = decimal.Parse(共済掛金標準率),
                            通常標準被害率 = decimal.Parse(通常標準被害率),
                            保険料基礎率 = decimal.Parse(保険料基礎率),
                            異常標準被害率 = decimal.Parse(異常標準被害率),
                            再保険料基礎率 = decimal.Parse(再保険料基礎率),
                            国庫負担割合 = decimal.Parse(国庫負担割合)
                        };
                        bulkData.Add(entity);
                        counter++;

                        // バッチサイズに達したら一括登録
                        if (counter % batchSize == 0)
                        {
                            db.W00120料率s.AddRange(bulkData);
                            db.SaveChanges();

                            // ChangeTracker をクリアしてメモリ節約
                            db.ChangeTracker.Entries().ToList().ForEach(e => e.State = EntityState.Detached);

                            bulkData.Clear();
                        }
                    } // end for
                    if (bulkData.Any())
                    {
                        db.W00120料率s.AddRange(bulkData);
                        db.SaveChanges();
                    }

                    List<W00120料率> kikenListResult = GetErrorListByCriteria(db, kumiaitoCd, short.Parse(nensan), todofukenCd);

                    if (!kikenListResult.IsNullOrEmpty() && kikenListResult.Count >= 1)
                    {
                        acceptanceErrorContent = MessageUtil.Get("ME90008");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    acceptanceErrorContent = UpsertM11040Kojin(db, sysDate);
                    if (!string.IsNullOrEmpty(acceptanceErrorContent))
                    {
                        forcedShutdownFlg = true;
                    }
                forcedShutdown:;
                    
                    if (forcedShutdownFlg)
                    {
                        shoriSts = Constants.TASK_FAILED;
                    }
                    else
                    {
                        shoriSts = Constants.TASK_SUCCESS;
                    }
                    
                    string refMessage = string.Empty;
                    int updateResult = BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), shoriSts, acceptanceErrorContent, BATCH_USER_NAME, ref refMessage);
                    // 更新に成功した場合
                    
                    if (0 == updateResult)
                    {
                        // 更新に失敗した場合
                        logger.Error(refMessage);
                        logger.Error(string.Format(Constants.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, shoriSts, refMessage));
                        result = Constants.BATCH_EXECUT_FAILED;
                    }
                    else
                    {
                        logger.Info(string.Format(Constants.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, bid, shoriSts));
                        result = Constants.BATCH_EXECUT_SUCCESS;
                    }
                    
                    if (forcedShutdownFlg)
                    {
                        logger.Debug("INIT CHK : Rollback");
                        transaction.Rollback();
                    }
                    else
                    {
                        // トランザクションコミット
                        transaction.Commit();
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e.StackTrace);
                    Console.Error.WriteLine(MessageUtil.Get("MF00001"));
                    logger.Error(MessageUtil.Get("MF00001"));
                    logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                    acceptanceErrorContent = MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION);
                    result = Constants.BATCH_EXECUT_FAILED;
                    shoriSts = "99";
                    UpdateBatchYoyakuNsk(result, bid, shoriSts, acceptanceErrorContent);
                    transaction.Rollback();
                    // 処理結果（正常：0、エラー：1）
                    Environment.ExitCode = Constants.BATCH_EXECUT_FAILED;
                    return;
                }
                finally
                {
                    if (!"03".Equals(shoriSts))
                    {
                        shoriSts = Constants.TASK_FAILED;
                        result = Constants.BATCH_EXECUT_FAILED;
                    }
                    
                    Directory.Delete(extractedFilePath, true);
                    // 処理時間
                    stopwatch.Stop();
                    var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                    logger.Info(excutingTime);
                    Environment.ExitCode = result;
                }
            }
        }
        /// <summary>
        /// バッチ条件テーブルからデータ取得
        /// </summary>
        /// <param name="db">NskDB</param>
        /// <param name="joukenId">条件id</param>
        /// <param name="nensan">年産</param>
        /// <param name="filePath">ファイルパス</param>
        /// <param name="ukeirerirekiId">受入履歴id</param>
        /// <returns>バッチ条件リスト</returns>
        private static List<T01050バッチ条件> GetBatchJoukenList(
            NskAppContext db,
            string joukenId,
            string nensan,
            string filePath,
            string ukeirerirekiId)
        {
            logger.Info("バッチ条件テーブルから、指定されたバッチ条件idおよび条件名称に一致するデータを取得します。");

            List<T01050バッチ条件> results = db.Set<T01050バッチ条件>()
                            .AsNoTracking()
                            .Where(b => b.バッチ条件id == joukenId &&
                                        (b.条件名称 == nensan ||
                                         b.条件名称 == filePath ||
                                         b.条件名称 == ukeirerirekiId))
                            .ToList();
            return results;
        }
        
        /// <summary>
        /// t_todohukenテーブルから都道府県件数取得
        /// </summary>
        /// <param name="db">NskDB</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <returns>都道府県件数</returns>
        private static int GetTodofukenCount(NskAppContext db, string todofukenCd)
        {
            logger.Info($"都道府県コード {todofukenCd} の件数を取得します。");
            int todofuken = db.VTodofukens
                .Where(x => x.TodofukenCd == todofukenCd)
                .Count();
            return todofuken;
        }

        /// <summary>
        /// m_kumiaitoテーブルから組合員件数取得
        /// </summary>
        /// <param name="db">NskDB</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <returns>組合員件数</returns>
        private static int GetKumiaitoCount(NskAppContext db, string todofukenCd, string kumiaitoCd)
        {
            logger.Info($"都道府県コード {todofukenCd} と組合員コード {kumiaitoCd} の件数を取得します。");
            int kumiaitoCn = db.VKumiaitos
                     .Where(x => x.TodofukenCd == todofukenCd &&
                                 x.KumiaitoCd == kumiaitoCd)
                     .Count();

            return kumiaitoCn;
        }

        /// <summary>
        /// w_00120_料率 のデータを全件削除
        /// </summary>
        private static int DeleteAllRyoritu(NskAppContext db)
        {
            string sql = @"DELETE FROM ""w_00120_料率""";

            int affectedRows = db.Database.ExecuteSqlRaw(sql);
            logger.Info($"{affectedRows} 件のデータを削除しました。");

            return affectedRows;
        }

        /// <summary>
        /// エラー情報確認処理
        /// </summary>
        private static List<W00120料率> GetErrorListByCriteria(
            NskAppContext db,
            string 組合等コードParam,
            int 年産Param,
            string 都道府県コードParam)
        {
            var query = db.Set<W00120料率>().Where(w =>
                // 1. 共済目的コードが m_00010_共済目的名称 に存在しない
                !db.Set<M00010共済目的名称>()
                    .Select(m => m.共済目的コード)
                    .Contains(w.共済目的コード)
                ||
                // 2. 合併時識別コードが、m_10220_地区別設定 と fim_m_shochiku_nm / fim_m_daichiku_nm の結合結果に存在しない
                !(
                    from m0 in db.Set<M10220地区別設定>()
                    join v in db.Set<VShochikuNm>()
                        on new { m0.組合等コード, m0.大地区コード, m0.小地区コード }
                        equals new { 組合等コード = v.KumiaitoCd, 大地区コード = v.DaichikuCd, 小地区コード = v.ShochikuCd }
                    join v0 in db.Set<VDaichikuNm>()
                        on new { v.TodofukenCd, v.KumiaitoCd, v.DaichikuCd }
                        equals new { v0.TodofukenCd, v0.KumiaitoCd, v0.DaichikuCd }
                    where v.TodofukenCd == 都道府県コードParam &&
                          m0.組合等コード == 組合等コードParam &&
                          m0.年産 == 年産Param &&
                          m0.共済目的コード == w.共済目的コード
                    select m0.合併時識別コード
                ).Contains(w.合併時識別コード)
                ||
                // 3. 類区分が m_00020_類名称（共済目的コードが w.共済目的コード に一致するもの）に存在しない
                !db.Set<M00020類名称>()
                    .Where(l => l.共済目的コード == w.共済目的コード)
                    .Select(l => l.類区分)
                    .Contains(w.類区分)
                ||
                // 4. 引受方式が m_10080_引受方式名称 に存在しない
                !db.Set<M10080引受方式名称>()
                    .Select(i => i.引受方式)
                    .Contains(w.引受方式)
                ||
                // 5. 特約区分が m_10100_特約区分名称 に存在しない
                !db.Set<M10100特約区分名称>()
                    .Select(t => t.特約区分)
                    .Contains(w.特約区分)
                ||
                // 6. 補償割合コードが m_20030_補償割合名称 に存在しない
                !db.Set<M20030補償割合名称>()
                    .Select(h => h.補償割合コード)
                    .Contains(w.補償割合コード)
                ||
                // 7. 統計単位地域コードが m_00170_統計単位地域 に存在しない
                !db.Set<M00170統計単位地域>()
                    .Where(i => i.組合等コード == 組合等コードParam &&
                                 i.年産 == 年産Param &&
                                 i.共済目的コード == w.共済目的コード)
                    .Select(l => l.統計単位地域コード)
                    .Contains(w.統計単位地域コード)
                ||
                // 8. 組合等コードが引数の値と一致しない
                w.組合等コード != 組合等コードParam
                ||
                // 9. 年産が引数の値と一致しない
                w.年産 != 年産Param
            );

            return query.ToList();
        }

        /// <summary>
        /// w_00120_料率 テーブルへのUPSERTを実行します。
        /// </summary>
        /// <param name="connectionString">PostgreSQLの接続文字列</param>
        /// <param name="sysDate">システム日付（登録・更新日時に使用）</param>
        /// <returns>影響を受けた行数</returns>
        public static string UpsertM11040Kojin(
            NskAppContext db
            ,DateTime sysDate)
        {
            // SQL文の組み立て
            StringBuilder sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();
            var errMsg = string.Empty;

            sql.AppendLine(@"INSERT INTO ""m_00120_料率""
                (
                  ""組合等コード"",
                  ""年産"",
                  ""共済目的コード"",
                  ""合併時識別コード"",
                  ""類区分"",
                  ""引受方式"",
                  ""特約区分"",
                  ""補償割合コード"",
                  ""統計単位地域コード"",
                  ""責任保険歩合"",
                  ""共済掛金標準率"",
                  ""通常標準被害率"",
                  ""保険料基礎率"",
                  ""異常標準被害率"",
                  ""再保険料基礎率"",
                  ""国庫負担割合"",
                  ""登録日時"",
                  ""登録ユーザid"",
                  ""更新日時"",
                  ""更新ユーザid""
                )
                SELECT 
                  w.""組合等コード"",
                  w.""年産"",
                  w.""共済目的コード"",
                  w.""合併時識別コード"",
                  w.""類区分"",
                  w.""引受方式"",
                  w.""特約区分"",
                  w.""補償割合コード"",
                  w.""統計単位地域コード"",
                  w.""責任保険歩合"",
                  w.""共済掛金標準率"",
                  w.""通常標準被害率"",
                  w.""保険料基礎率"",
                  w.""異常標準被害率"",
                  w.""再保険料基礎率"",
                  w.""国庫負担割合"",
                  @sysDate,
                  'NSK_102101B',
                  @sysDate,
                  'NSK_102101B'
                FROM 
                  ""w_00120_料率"" w
                ON CONFLICT 
                (
                  ""組合等コード"",
                  ""年産"",
                  ""共済目的コード"",
                  ""合併時識別コード"",
                  ""類区分"",
                  ""引受方式"",
                  ""特約区分"",
                  ""補償割合コード"",
                  ""統計単位地域コード""
                )
                DO UPDATE SET
                  ""責任保険歩合"" = EXCLUDED.""責任保険歩合"",
                  ""共済掛金標準率"" = EXCLUDED.""共済掛金標準率"",
                  ""通常標準被害率"" = EXCLUDED.""通常標準被害率"",
                  ""保険料基礎率"" = EXCLUDED.""保険料基礎率"",
                  ""異常標準被害率"" = EXCLUDED.""異常標準被害率"",
                  ""再保険料基礎率"" = EXCLUDED.""再保険料基礎率"",
                  ""国庫負担割合"" = EXCLUDED.""国庫負担割合"",
                  ""更新日時"" = @sysDate,
                  ""更新ユーザid"" = 'NSK_102121B';");

            // sysDate パラメータの追加
            parameters.Add(new NpgsqlParameter("@sysDate", sysDate));

            logger.Info($"w_00120_料率 テーブルに対して Upsert を実行します。sysDate: {sysDate}");

            try
            {
                db.Database.ExecuteSqlRaw(sql.ToString(), parameters.ToArray());
            }
            catch (Exception e) 
            {
                logger.Error(e.StackTrace);
                errMsg = MessageUtil.Get("ME01645", "テーブルの登録・更新処理");
            }

            // ExecuteSqlRawを利用してSQLを実行、影響を受けた行数を返します。
            return errMsg;
        }

        /// <summary>
        /// タスク終了バッチ更新処理
        /// </summary>
        private static void UpdateBatchYoyakuNsk(int result, string bid, string shoriSts, string acceptanceErrorContent)
        {
            string refMessage = string.Empty;
            int updateResult = BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), shoriSts, acceptanceErrorContent, BATCH_USER_NAME, ref refMessage);
            if (0 == updateResult)
            {
                // 更新に失敗した場合
                logger.Error(refMessage);
                logger.Error(string.Format(Constants.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, shoriSts, refMessage));
                result = Constants.BATCH_EXECUT_FAILED;
            }
            else
            {
                // 更新に成功した場合
                logger.Info(string.Format(Constants.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, bid, shoriSts));
                result = Constants.BATCH_EXECUT_SUCCESS;
            }

            Environment.ExitCode = result;
        }

        /// <summary>
        /// 数値かどうかをチェック（整数としてパースできるかどうか）
        /// </summary>
        private static bool IsNumeric(string input)
        {
            return long.TryParse(input, out _);
        }
    }
}
