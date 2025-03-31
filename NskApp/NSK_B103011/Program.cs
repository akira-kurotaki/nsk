using NSK_B103011.Common;
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

namespace NSK_B103011
{
    /// <summary>
    /// 定時実行予約登録
    /// </summary>
    class Program
    {
        /// <summary>
        /// バッチ名
        /// </summary>
        private static string BATCH_NAME = "基準統計単収取込処理（テキスト）";
        private static string BATCH_USER_NAME = "NSK_B103011";
        // エラー出力先フォルダ（ZIPファイル格納フォルダ）
        private const string DestinationFolder = @"C:\NSK102011B\ErrorFiles";

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
            logger.Info(string.Concat(CoreConst.LOG_START_KEYWORD, " 基準統計単収取込処理（テキスト）"));
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
            string extractedFilePath = string.Empty;
            string extractFolder = string.Empty;

            int result = Constants.BATCH_EXECUT_SUCCESS;
            string msgTest = string.Empty;
            
            foreach (string arg in args)
            {
                if (arg.StartsWith("--bid=")) bid = arg.Split("=")[1];
                if (arg.StartsWith("--todofukenCd=")) todofukenCd = arg.Split("=")[1];
                if (arg.StartsWith("--kumiaitoCd=")) kumiaitoCd = arg.Split("=")[1];
                if (arg.StartsWith("--shishoCd=")) shishoCd = arg.Split("=")[1];
                if (arg.StartsWith("--jid=")) jid = arg.Split("=")[1];
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

            logger.Debug("dbConnectionInfo.ConnectionString : " + dbConnectionInfo.ConnectionString);
            logger.Debug("dbConnectionInfo.DefaultSchema : " + dbConnectionInfo.DefaultSchema);

            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema, ConfigUtil.GetInt(Constants.CONFIG_COMMAND_TIMEOUT)))
            {
                var transaction = db.Database.BeginTransaction();
                try
                {
                    // パフォーマンス向上のため、変更検知を一時的に無効化
                    //db.ChangeTracker.AutoDetectChangesEnabled = false;
                    var bulkData = new List<W10070統計単収引受>();
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
                    logger.Debug("GetBatchJoukenList @@@@@@@@@@@@@@@@@@@@@@@");
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
                    logger.Debug("GetBatchJoukenList nensan : " + nensan);
                    logger.Debug("GetBatchJoukenList filePath : " + filePath);
                    logger.Debug("GetBatchJoukenList fileHash : " + fileHash);
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


                    logger.Debug("GetTodofukenCount @@@@@@@@@@@@@@@@@@@@@@@");
                    logger.Debug("filePath :: " + filePath);
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
                    string parentFolder = Path.GetDirectoryName(zipFilePath);
                    extractFolder = Path.Combine(parentFolder, "temp");
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

                    // 3. 解凍先から対象のTXTファイルを取得（csvであることが前提）
                    extractedFilePath = Directory.GetFiles(extractFolder, "*.*", SearchOption.TopDirectoryOnly)
                                                        .FirstOrDefault(f => Path.GetExtension(f).ToLower() == ".csv");

                    logger.Debug("extractedFilePath :::: " + extractedFilePath);
                    if (string.IsNullOrEmpty(extractedFilePath))
                    {
                        logger.Error("ERROR: 解凍されたcsvファイルが見つかりません。");
                        //acceptanceErrorContent = MessageUtil.Get("ME10050");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    // ファイル拡張子の再チェック（念のため）
                    if (Path.GetExtension(extractedFilePath)?.ToLower() != ".csv")
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
                    //  最後のディレクトリ名（"test2011"）を取得して ".csv" を付与する
                    string originalFileName = new DirectoryInfo(filePath).Name;  // "test2011"
                    string expectedFileName = originalFileName + ".csv";          // "test2011.csv"

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
                    if (headerFields.Length != 6)
                    {
                        logger.Error("ERROR: ヘッダ行の項目数が7ではありません。");
                        acceptanceErrorContent = "ERROR: ヘッダ行の項目数が7ではありません。";
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    DeleteAllToukei(db);

                    // 13. 初期変数の設定
                    int lineNumber = 1; // ヘッダ行を1行目とする

                    // 14. データ行（2行目以降）のチェック
                    for (int i = 1; i < allLines.Length; i++)
                    {
                        string line = allLines[i];
                        // 14.2 次の行がない（EOF）または空白行の場合、終了
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME90015", "データ行");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }

                        // 14.3 行番号の更新
                        lineNumber++;

                        // 14.4 項目数チェック
                        string[] fields = line.Split(',');
                        if (fields.Length != 6)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10066");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }

                        // 14.5 各項目のチェック
                        acceptanceErrorContent = "";

                        logger.Debug("各項目のチェック fields[0].Trim('\"')Length : " + fields[0].Trim('"').Length);
                        logger.Debug("各項目のチェック fields[0].Trim('\"') : " + fields[0].Trim('"'));

                        // 1. 組合等コード（必須、数字、文字数3）
                        if (string.IsNullOrWhiteSpace(fields[0].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "組合等コード", "(" + lineNumber + "行目)");
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
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "共済目的コード", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (fields[2].Trim('"').Length > 2)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "共済目的コード", "2", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }

                        // 4. 類区分（必須、数字、文字数2）
                        if (string.IsNullOrWhiteSpace(fields[3].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "類区分", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (!IsNumeric(fields[3].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "類区分", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (fields[3].Trim('"').Length > 2)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "類区分", "2", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }

                        // 5. 統計単位地域コード（必須、数字、文字数5）
                        if (string.IsNullOrWhiteSpace(fields[4].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "統計単位地域コード", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (!IsNumeric(fields[4].Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "統計単位地域コード", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }
                        else if (fields[4].Trim('"').Length > 5)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "統計単位地域コード", "5", "(" + lineNumber + "行目)");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }

                        // 6. 統計単収（必須、数字、文字数4）
                        if (string.IsNullOrWhiteSpace(fields[5].Trim('"')))
                        {
                            if (!IsNumeric(fields[5].Trim('"')))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00003", "統計単収", "(" + lineNumber + "行目)");
                                forcedShutdownFlg = true;
                                goto forcedShutdown;
                            }
                            else if (fields[5].Trim('"').Length > 4)
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00020", "統計単収", "4", "(" + lineNumber + "行目)");
                                forcedShutdownFlg = true;
                                goto forcedShutdown;
                            }
                        }

                        // 各変数にファイルの各項目を設定
                        string 組合等コード = fields[0].Trim('"');
                        string 年産 = fields[1].Trim('"');
                        string 共済目的コード = fields[2].Trim('"');
                        string 類区分 = fields[3].Trim('"');
                        string 統計単位地域コード = fields[4].Trim('"');
                        string 統計単収 = fields[5].Trim('"');


                        var entity = new W10070統計単収引受
                        {
                            組合等コード = 組合等コード,
                            年産 = short.Parse(年産),
                            共済目的コード = 共済目的コード,
                            類区分 = 類区分,
                            統計単位地域コード = 統計単位地域コード,
                            統計単収 = 統計単収
                        };
                        bulkData.Add(entity);
                        counter++;

                        // バッチサイズに達したら一括登録
                        if (counter % batchSize == 0)
                        {
                            db.W10070統計単収引受s.AddRange(bulkData);
                            db.SaveChanges();

                            // ChangeTracker をクリアしてメモリ節約
                            db.ChangeTracker.Entries().ToList().ForEach(e => e.State = EntityState.Detached);

                            bulkData.Clear();
                        }
                    } // end for
                    if (bulkData.Any())
                    {
                        db.W10070統計単収引受s.AddRange(bulkData);
                        db.SaveChanges();
                    }

                    logger.Debug("大量SUS");
                    List<W10070統計単収引受> kikenListResult = GetErrorListByToukei(db, kumiaitoCd, nensan);

                    logger.Debug("!kikenListResult.IsNullOrEmpty() : " + !kikenListResult.IsNullOrEmpty());
                    logger.Debug("kikenListResult.Count" + kikenListResult.Count);

                    if (!kikenListResult.IsNullOrEmpty() && kikenListResult.Count >= 1)
                    {
                        acceptanceErrorContent = MessageUtil.Get("ME91003", "w_10070_統計単収引受", "マスタ");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    acceptanceErrorContent = UpsertM10070Toukei(db, sysDate);
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
                    logger.Error("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                    logger.Error(e.StackTrace);
                    logger.Error("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
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
                    // File.Delete(tempFilePath);
                    File.Delete(extractFolder);
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

        private static int DeleteAllToukei(NskAppContext db)
        {
            string sql = @"DELETE FROM ""w_10070_統計単収引受""";

            int affectedRows = db.Database.ExecuteSqlRaw(sql);

            return affectedRows;
        }

        private static List<W10070統計単収引受> GetErrorListByToukei(NskAppContext db, string kumiaitoCd, string nensan)
        {
            var query = db.Set<W10070統計単収引受>().Where(w =>
                // 1. 共済目的コードが m_00010_共済目的名称 に存在しない
                !db.Set<M00010共済目的名称>()
                    .Select(m => m.共済目的コード)
                    .Contains(w.共済目的コード)
                ||
                // 2. 類区分が m_00020_類名称 に存在しない
                !db.Set<M00020類名称>()
                    .Where(m => m.共済目的コード == w.共済目的コード)
                    .Select(m => m.類区分)
                    .Contains(w.類区分)
                ||
                // 3. 統計単位地域コードが m_00170_統計単位地域 に存在しない
                !db.Set<M00170統計単位地域>()
                    .Where(m => m.組合等コード == w.組合等コード &&
                                m.年産 == w.年産 &&
                                m.共済目的コード == w.共済目的コード)
                    .Select(m => m.統計単位地域コード)
                    .Contains(w.統計単位地域コード)
                ||
                // 4. w.組合等コード != w.組合等コード （常に false になるはずですが、SQL と同様の記述）
                w.組合等コード != kumiaitoCd
                ||
                // 5. w.年産 != w.年産 （常に false になるはずですが、SQL と同様の記述）
                w.年産 != short.Parse(nensan)
            );

            return query.ToList();
        }

        /// <summary>
        /// m_10070_統計単収引受 テーブルへのUPSERTを実行します。
        /// </summary>
        /// <param name="connectionString">PostgreSQLの接続文字列</param>
        /// <param name="sysDate">システム日付（登録・更新日時に使用）</param>
        /// <returns>影響を受けた行数</returns>
        public static string UpsertM10070Toukei(
            NskAppContext db
            ,DateTime sysDate)
        {
            // SQL文の組み立て
            StringBuilder sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();
            var errMsg = string.Empty;

            sql.AppendLine(@"INSERT INTO ""m_10070_統計単収引受""
                            (
                              ""組合等コード"",
                              ""年産"",
                              ""共済目的コード"",
                              ""類区分"",
                              ""統計単位地域コード"",
                              ""統計単収"",
                              ""登録日時"",
                              ""登録ユーザid"",
                              ""更新日時"",
                              ""更新ユーザid""
                            )
                            SELECT 
                              w.""組合等コード"",
                              w.""年産"",
                              w.""共済目的コード"",
                              w.""類区分"",
                              w.""統計単位地域コード"",
                              w.""統計単収"",
                              @sysDate,
                              'NSK_103011B',
                              @sysDate,
                              'NSK_103011B'
                            FROM 
                              ""w_10070_統計単収引受"" w
                            ON CONFLICT 
                            (
                              ""組合等コード"",
                              ""年産"",
                              ""共済目的コード"",
                              ""類区分"",
                              ""統計単位地域コード""
                            )
                            DO UPDATE SET
                              ""統計単収"" = EXCLUDED.""統計単収"",
                              ""更新日時"" = @sysDate,
                              ""更新ユーザid"" = 'NSK_103011B'
                            WHERE
                              ""m_10070_統計単収引受"".""xmin"" = (
                                SELECT ""xmin""
                                FROM ""m_10070_統計単収引受""
                                WHERE ""組合等コード"" = w.""組合等コード""
                                  AND ""年産"" = w.""年産""
                                  AND ""共済目的コード"" = w.""共済目的コード""
                                  AND ""類区分"" = w.""類区分""
                                  AND ""統計単位地域コード"" = w.""統計単位地域コード""
                              );");

            // sysDate パラメータの追加
            parameters.Add(new NpgsqlParameter("@sysDate", sysDate));

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
