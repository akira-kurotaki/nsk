using NSK_B112011.Common;
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
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NLog.Fluent;
using System.Numerics;

namespace NSK_B112011
{
    /// <summary>
    /// 定時実行予約登録
    /// </summary>
    class Program
    {
        /// <summary>
        /// バッチ名
        /// </summary>
        private static string BATCH_NAME = "引受大量データ基準収穫量データ取込";
        private static string BATCH_USER_NAME = "NSK_112041B";

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
            var ukeireRirekiId = "";
            var filePath = string.Empty;

            var kumiaitoCdUkeire = string.Empty;
            var shoriSts = "03";
            var errorMsg = string.Empty;
            string msgTest = string.Empty;
            bool forcedShutdownFlg = false;

            var errorListName = string.Empty;
            var errorListPath = string.Empty;
            var errorListHashValue = string.Empty;
            var okListName = string.Empty;
            var okListPath = string.Empty;
            var okListHashValue = string.Empty;
            var extractedFilePath = string.Empty;
            var errCount = 0;
            var okCount = 0;
            var tairyoDataSts = string.Empty;

            int result = Constants.BATCH_EXECUT_SUCCESS;

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
                errorMsg = MessageUtil.Get("ME90015", Constants.DEFAULT_SCHEMA_JIGYO_COMMON);
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Error(errorMsg);
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, errorMsg);
                return;
            }

            if (defaultSchemaSystemCommon == null || "".Equals(defaultSchemaSystemCommon))
            {
                //ERROR
                errorMsg = MessageUtil.Get("ME90015", Constants.DEFAULT_SCHEMA_JIGYO_COMMON);
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Error(errorMsg);
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, errorMsg);
                return;
            }

            if (bid == null || "".Equals(bid))
            {
                errorMsg = MessageUtil.Get("ME01054", "バッチID");
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Error(errorMsg);
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, errorMsg);
                return;
            }

            if (todofukenCd == null || "".Equals(todofukenCd))
            {
                errorMsg = MessageUtil.Get("ME01054", "都道府県コード");
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Error(errorMsg);
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, errorMsg);
                return;
            }

            if (kumiaitoCd == null || "".Equals(kumiaitoCd))
            {
                errorMsg = MessageUtil.Get("ME01054", "組合等コード");
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Error(errorMsg);
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, errorMsg);
                return;
            }
            if (jid == null || "".Equals(jid))
            {
                errorMsg = MessageUtil.Get("ME01054", "条件ID");
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Error(errorMsg);
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, errorMsg);
                return;
            }

            // バッチのDB接続先取得処理
            DbConnectionInfo dbConnectionInfo =
                DBUtil.GetDbConnectionInfo(ConfigUtil.Get(Constants.SYSTEM_KBN)
                , todofukenCd
                , kumiaitoCd
                , shishoCd);

            var resultTairyoData = new TairyoDataDto();

            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema, ConfigUtil.GetInt(Constants.CONFIG_COMMAND_TIMEOUT)))
            {
                var transaction = db.Database.BeginTransaction();
                try
                {
                    // バッチ条件情報取得
                    List<T01050バッチ条件> joukenList = GetBatchJoukenList(db, jid, Constants.JOUKEN_NENSAN, Constants.JOUKEN_UKEIRE_RIREKI_ID);
                    if (joukenList.IsNullOrEmpty())
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    for (int i = 0; i < joukenList.Count; i++)
                    {
                        if (Constants.JOUKEN_NENSAN.Equals(joukenList[i].条件名称))
                        {
                            nensan = joukenList[i].条件値;
                        }
                        if (Constants.JOUKEN_UKEIRE_RIREKI_ID.Equals(joukenList[i].条件名称))
                        {
                            ukeireRirekiId = joukenList[i].条件値;
                        }
                    }
                    if (string.IsNullOrEmpty(nensan))
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    if (string.IsNullOrEmpty(ukeireRirekiId))
                    {
                        //"DUMMY_MESSAGE_ID"：取込履歴ID　{0}が引数に設定されていません。
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    // 都道府県マスタチェック情報
                    int todoCount = GetTodofukenCount(db, todofukenCd);
                    if (todoCount == 0)
                    {
                        errorMsg = MessageUtil.Get("ME10005", "都道府県コード");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    // 組合等マスタチェック情報
                    int kumiCount = GetKumiaitoCount(db, todofukenCd, kumiaitoCd);
                    if (kumiCount == 0)
                    {
                        errorMsg = MessageUtil.Get("ME10005", "組合等コード");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    int shishoCount = GetShishoCdCount(db, todofukenCd, kumiaitoCd, shishoCd);
                    if (shishoCount == 0)
                    {
                        errorMsg = MessageUtil.Get("ME10005", "支所コード");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    resultTairyoData = GetTairyoData(db, ukeireRirekiId);
                    if (resultTairyoData == null || !"01".Equals(resultTairyoData.ステータス)  /* 処理待ち */)
                    {
                        errorMsg = MessageUtil.Get("ME10042", "取込処理");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    tairyoDataSts = resultTairyoData.ステータス;

                    // ----- 復号化側の処理 -----
                    /*
                     * TODO:ファイル格納先と処理のロジックが決まってないため、
                     * 必要に応じて修正
                     * ファイル名だけでは取得できないため、ファイルパスがあることを
                     * 前提として作成する
                     */
                    // 1. ZIPファイルパスの取得
                    // string zipFilePath = Directory.GetFiles(filePath, "*.zip").FirstOrDefault();
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(resultTairyoData.取込ファイル_変更後ファイル名);
                    //string zipFilePath = Directory.GetFiles(ConfigUtil.Get(Constants.FILE_BATCH_PATH), fileNameWithoutExtension + ".zip").FirstOrDefault();
                    string zipFilePath = Directory.GetFiles("C:\\SYN\\112011Real\\20250319163136\\_0303000002\\112011FORERRTEST", fileNameWithoutExtension + ".zip").FirstOrDefault();
                    if (string.IsNullOrEmpty(zipFilePath))
                    {
                        //エラー処理
                        errorMsg = MessageUtil.Get("ME01645", "ファイル取得");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    logger.Debug("INFILETEST @@@@@@@@@@@@@@@");
                    // 2. ZIPファイルと同じフォルダに解凍する
                    string extractFolder = Path.GetDirectoryName(zipFilePath);
                    try
                    {
                        // ※同名フォルダが存在すると例外となるので注意
                        ZipFile.ExtractToDirectory(zipFilePath, extractFolder);
                    }
                    catch (Exception fileEx)
                    {
                        errorMsg = MessageUtil.Get("ME01645", "ファイルの解凍");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    logger.Debug("INFILETEST @@@@@@@@@@@@@@@2");

                    // 3. 解凍先から対象のTXTファイルを取得（txtであることが前提）
                    extractedFilePath = Directory.GetFiles(extractFolder, "*.*", SearchOption.TopDirectoryOnly)
                                                        .FirstOrDefault(f => Path.GetExtension(f).ToLower() == ".csv");

                    if (string.IsNullOrEmpty(extractedFilePath))
                    {
                        errorMsg = MessageUtil.Get("ME10050");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    // ファイル拡張子の再チェック（念のため）
                    if (Path.GetExtension(extractedFilePath)?.ToLower() != ".csv")
                    {
                        errorMsg = MessageUtil.Get("ME10050");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    // 4. ファイルの読み込み
                    byte[] extractedFileData = File.ReadAllBytes(extractedFilePath);

                    // 5. 暗号化時に使用した元ファイル名を、復号化時にも使用する
                    //  filePath は "C:\SYN\2011Real\20250219111614\_0303000001\test2011" のようになっているので、
                    //  最後のディレクトリ名（"test2011"）を取得して ".txt" を付与する
                    // string originalFileName = new DirectoryInfo(filePath).Name;  // "test2011"
                    // string expectedFileName = originalFileName + ".csv";          // "test2011.txt"
                    string expectedFileName = Path.GetFileName(extractedFilePath);
                    logger.Debug("expectedFileName :::: " + expectedFileName);

                    // 6. ファイルの復号化（expectedFileName を使用）
                    byte[] decryptedData = CryptoUtil.Decrypt(extractedFileData, expectedFileName);
                    string hashByFile = CryptoUtil.GetSHA256Hex(decryptedData);
                    if (!hashByFile.Equals(resultTairyoData.取込ファイルハッシュ値))
                    {
                        errorMsg = MessageUtil.Get("ME10052", "ファイル名");
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
                        errorMsg = MessageUtil.Get("ME90015", "ヘッダー");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    string headerLine = allLines[0];
                    string[] headerFields = headerLine.Split(',');
                    for (int i = 0; i < headerFields.Length; i++)
                    {
                        logger.Debug("headerFields[i] : " + headerFields[i]);
                    }
                    
                    if (headerFields.Length != 62)
                    {
                        errorMsg = MessageUtil.Get("ME10066");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    logger.Debug("@@@@@@@@@@@@@@@@@@@@@@@");
                    logger.Debug("処理区分 : " + headerFields[0].TrimStart('\uFEFF'));
                    logger.Debug("!処理区分.Equals(headerFields[0].Trim()) : " + !"処理区分".Equals(headerFields[0].TrimStart('\uFEFF')));
                    logger.Debug("@@@@@@@@@@@@@@@@@@@@@@@");
                    if (!"処理区分".Equals(headerFields[0].TrimStart('\uFEFF')))
                    {
                        errorMsg = MessageUtil.Get("ME90015", "ヘッダー");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    if (allLines.Length < 2)
                    {
                        errorMsg = MessageUtil.Get("ME10025", "取込");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    // 13. 初期変数の設定
                    int lineNumber = 1; // ヘッダ行を1行目とする
                    var ukeireErrorList = new List<ErrorList>();
                    var okListForCsvFile = new List<T19020大量データ受入基準収穫量ok>();
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
                        if (fields.Length != 62)
                        {
                            errorMsg = MessageUtil.Get("ME10066");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            });
                            continue;
                        }
                        // ------------------------
                        // 【必須・桁数チェック】
                        // ※以下、項目についてエラー発生時は continue
                        // ------------------------
                        logger.Debug("@@@@@@");
                        logger.Debug("fields[0].Trim('\"') " + fields[0].Trim('"'));

                        logger.Debug("fields[0] " + fields[0]);
                        logger.Debug("fields[0].Trim('\"').Length " + fields[0].Trim('"').Length);
                        logger.Debug("@@@@@@");
                        // (0) 処理区分　※必須・文字数1
                        if (string.IsNullOrWhiteSpace(fields[0].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "処理区分", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (fields[0].Trim('"').Length > 1)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "処理区分", "1", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        // (1) 組合等コード　※必須・文字数3
                        if (string.IsNullOrWhiteSpace(fields[1].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "組合等コード", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (fields[1].Trim('"').Length > 3)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "組合等コード", "3", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        // (2) 組合等名 文字数(50)
                        if (!string.IsNullOrWhiteSpace(fields[2].Trim('"')) && fields[2].Trim('"').Length > 50)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "組合等名", "50", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            goto forcedShutdown;
                        }

                        // (3) 年産　※必須・文字数4
                        if (string.IsNullOrWhiteSpace(fields[3].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "年産", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (fields[3].Trim('"').Length > 4)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "年産", "4", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        // (4) 共済目的コード　※必須・文字数4
                        if (string.IsNullOrWhiteSpace(fields[4].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "共済目的コード", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (fields[4].Trim('"').Length > 4)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "共済目的コード", "4", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        // (5) 共済目的名 文字数(20)

                        if (!string.IsNullOrWhiteSpace(fields[5].Trim('"')) && fields[5].Trim('"').Length > 20)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "共済目的名", "20", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            goto forcedShutdown;
                        }

                        // (6) 引受方式　※必須・文字数1
                        if (string.IsNullOrWhiteSpace(fields[6].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "引受方式", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (fields[6].Trim('"').Length > 1)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "引受方式", "1", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        // (7) 引受方式名称 文字数(20)	
                        if (!string.IsNullOrWhiteSpace(fields[7].Trim('"')) && fields[7].Trim('"').Length > 20)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "引受方式名称", "20", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            goto forcedShutdown;
                        }

                        // (8) 支所コード　※必須・文字数2
                        if (string.IsNullOrWhiteSpace(fields[8].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "支所コード", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (fields[8].Trim('"').Length > 2)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "支所コード", "2", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        // (9) 支所名 文字数(20)
                        if (!string.IsNullOrWhiteSpace(fields[9].Trim('"')) && fields[9].Trim('"').Length > 20)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "支所名", "20", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            goto forcedShutdown;
                        }

                        // (10) 大地区コード　※必須・文字数2
                        if (string.IsNullOrWhiteSpace(fields[10].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "大地区コード", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (fields[10].Trim('"').Length > 2)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "大地区コード", "2", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        // (11) 大地区名 文字数(10)

                        if (!string.IsNullOrWhiteSpace(fields[11].Trim('"')) && fields[11].Trim('"').Length > 10)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "大地区名", "10", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            goto forcedShutdown;
                        }

                        // (12) 小地区コード　※必須・文字数4
                        if (string.IsNullOrWhiteSpace(fields[12].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "小地区コード", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (fields[12].Trim('"').Length > 4)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "小地区コード", "4", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        // (13)
                        if (!string.IsNullOrWhiteSpace(fields[13].Trim('"')) && fields[13].Trim('"').Length > 10)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "小地区名", "10", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            goto forcedShutdown;
                        }

                        // (14) 組合員等コード　※必須・文字数13
                        if (string.IsNullOrWhiteSpace(fields[14].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "組合員等コード", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (fields[14].Trim('"').Length > 13)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "組合員等コード", "13", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        // (15)
                        if (!string.IsNullOrWhiteSpace(fields[15].Trim('"')) && fields[15].Trim('"').Length > 30)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "組合員等氏名", "30", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            goto forcedShutdown;
                        }

                        // (16) 類区分　※必須・文字数2
                        if (string.IsNullOrWhiteSpace(fields[16].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "類区分", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (fields[16].Trim('"').Length > 2)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "類区分", "2", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        // (17)類区分名 文字数40
                        if (!string.IsNullOrWhiteSpace(fields[17].Trim('"')) && fields[17].Trim('"').Length > 40)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "類区分名", "40", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        // (18)産地別銘柄等コード（ 半角数字 文字数(5)）※値が存在する場合のみチェック
                        if (!string.IsNullOrWhiteSpace(fields[18].Trim('"')))
                        {
                            if (fields[18].Trim('"').Length > 5)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "産地別銘柄等コード", "5", "(" + lineNumber + "行目)");
                                logger.Error(errorMsg);
                                errCount++;
                                ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                                continue;
                            }
                            // 数値チェック（半角数字かどうか）
                            if (!int.TryParse(fields[18].Trim('"'), out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "産地別銘柄等コード", "(" + lineNumber + "行目)");
                                logger.Error(errorMsg);
                                errCount++;
                                ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                                continue;
                            }
                        }

                        // (19)産地別銘柄等名称（ 半角数字 文字数(30)）※値が存在する場合のみチェック
                        if (!string.IsNullOrWhiteSpace(fields[19].Trim('"')))
                        {
                            if (fields[19].Trim('"').Length > 30)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "産地別銘柄等名称", "30", "(" + lineNumber + "行目)");
                                logger.Error(errorMsg);
                                errCount++;
                                ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                                continue;
                            }
                            // 数値チェック（必要であれば）
                            if (!BigInteger.TryParse(fields[19].Trim('"'), out BigInteger bigNumber))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "産地別銘柄等名称", "(" + lineNumber + "行目)");
                                logger.Error(errorMsg);
                                errCount++;
                                ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                                continue;
                            }
                        }

                        // (20) 営農対象外フラグ　※必須・文字数1
                        if (string.IsNullOrWhiteSpace(fields[20].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "営農対象外フラグ", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (fields[20].Trim('"').Length > 1)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "営農対象外フラグ", "1", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        logger.Debug("@@@@@@@@@@@@");
                        logger.Debug("fields[21].Trim('\"').Trim('.').Length : " + fields[21].Trim('"').Trim('.').Length);
                        logger.Debug("@@@@@@@@@@@@");
                        // (21) 平均単収　※必須・numeric(6,2)、範囲: 0.0～9999.99
                        if (string.IsNullOrWhiteSpace(fields[21].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "平均単収", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (fields[21].Trim('"').Replace(".", "").Length > 6)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "平均単収", "6", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (!decimal.TryParse(fields[21].Trim('"'), out decimal averageYield))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "平均単収", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (averageYield < 0.0m || averageYield > 9999.99m)
                        {
                            errorMsg = MessageUtil.Get("ME00003", "平均単収", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        // ------------------------
                        // 【規格別割合（規格1～規格40）のチェック】
                        // ※フィールドインデックス 22～61
                        // 各項目は numeric(4,3) で、範囲は 0.0～1.0、また全体の合計が 1.0 を超えないこと
                        // ------------------------
                        decimal sumPercentages = 0.0m;
                        var exloopFlg = false;
                        for (int j = 22; j <= 61; j++)
                        {
                            logger.Debug("@@@@@@@@");
                            logger.Debug("fields[j] : " + fields[j]);
                            logger.Debug("string.IsNullOrWhiteSpace(fields[j].Trim('\"')) : " + string.IsNullOrWhiteSpace(fields[j].Trim('"')));
                            logger.Debug("@@@@@@@@");
                            // 空欄の場合は 0.0 とみなす（必要に応じて変更）
                            if (!string.IsNullOrWhiteSpace(fields[j].Trim('"')))
                            {
                                if (fields[j].Trim('"').Replace(".", "").Length > 4)
                                {
                                    errorMsg = MessageUtil.Get("ME00020", $"規格別割合_規格{j - 21}", "4", "(" + lineNumber + "行目)");
                                    logger.Error(errorMsg);
                                    errCount++;
                                    ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                                    exloopFlg = true;
                                    goto ExitLoop;
                                }
                                if (!decimal.TryParse(fields[j].Trim('"'), out decimal percentage))
                                {
                                    errorMsg = MessageUtil.Get("ME00003", $"規格別割合_規格{j - 21}", "(" + lineNumber + "行目)");
                                    logger.Error(errorMsg);
                                    errCount++;
                                    ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                                    exloopFlg = true;
                                    goto ExitLoop;
                                }
                                if (percentage < 0.0m || percentage > 1.0m)
                                {
                                    errorMsg = MessageUtil.Get("ME00003", $"規格別割合_規格{j - 21}", "(" + lineNumber + "行目)");
                                    logger.Error(errorMsg);
                                    errCount++;
                                    ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                                    exloopFlg = true;
                                    goto ExitLoop;
                                }
                                sumPercentages += percentage;
                            }
                        }
                    ExitLoop:;
                        if (exloopFlg)
                        {
                            continue;
                        }
                        // 全体の合計が 1.0 を超えていないかチェック
                        if (sumPercentages > 1.0m)
                        {
                            errorMsg = MessageUtil.Get("ME00003", "規格別割合合計", "(" + lineNumber + "行目)");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        // fields[] 配列は、0～61 のインデックスで各項目が格納されている前提

                        // (0) 処理区分
                        string shoriKubun = fields[0].Trim('"');

                        // (1) 組合等コード
                        string kumiaiTouCode = fields[1].Trim('"');

                        // (2) 組合等名
                        string kumiaiTouMei = fields[2].Trim('"');

                        // (3) 年産
                        string nensanF = fields[3].Trim('"');

                        // (4) 共済目的コード
                        string kyosaiMokutekiCd = fields[4].Trim('"');

                        // (5) 共済目的名
                        string kyosaiMokutekiMei = fields[5].Trim('"');

                        // (6) 引受方式
                        string hikiukeHoshiki = fields[6].Trim('"');

                        // (7) 引受方式名称
                        string hikiukeHoshikiMei = fields[7].Trim('"');

                        // (8) 支所コード
                        string shishoCode = fields[8].Trim('"');

                        // (9) 支所名
                        string shishoName = fields[9].Trim('"');

                        // (10) 大地区コード
                        string daichikuCode = fields[10].Trim('"');

                        // (11) 大地区名
                        string daichikuName = fields[11].Trim('"');

                        // (12) 小地区コード
                        string shouchikuCode = fields[12].Trim('"');

                        // (13) 小地区名
                        string shouchikuName = fields[13].Trim('"');

                        // (14) 組合員等コード
                        string kumiaiIntoCode = fields[14].Trim('"');

                        // (15) 組合員等氏名
                        string kumiaiIntoName = fields[15].Trim('"');

                        // (16) 類区分
                        string ruikubun = fields[16].Trim('"');

                        // (17) 類区分名
                        string ruikubunName = fields[17].Trim('"');

                        // (18) 産地別銘柄等コード
                        string sanchiBetsuMeigaraCd = fields[18].Trim('"');

                        // (19) 産地別銘柄等名称
                        string sanchiBetsuMeigaraName = fields[19].Trim('"');

                        // (20) 営農対象外フラグ
                        string einouTaishogaiFlag = fields[20].Trim('"');

                        // (21) 平均単収
                        string heikinTanshu = fields[21].Trim('"');

                        // (22) 規格別割合（規格1）
                        string kikakuWariai1 = fields[22].Trim('"');

                        // (23) 規格別割合（規格2）
                        string kikakuWariai2 = fields[23].Trim('"');

                        // (24) 規格別割合（規格3）
                        string kikakuWariai3 = fields[24].Trim('"');

                        // (25) 規格別割合（規格4）
                        string kikakuWariai4 = fields[25].Trim('"');

                        // (26) 規格別割合（規格5）
                        string kikakuWariai5 = fields[26].Trim('"');

                        // (27) 規格別割合（規格6）
                        string kikakuWariai6 = fields[27].Trim('"');

                        // (28) 規格別割合（規格7）
                        string kikakuWariai7 = fields[28].Trim('"');

                        // (29) 規格別割合（規格8）
                        string kikakuWariai8 = fields[29].Trim('"');

                        // (30) 規格別割合（規格9）
                        string kikakuWariai9 = fields[30].Trim('"');

                        // (31) 規格別割合（規格10）
                        string kikakuWariai10 = fields[31].Trim('"');

                        // (32) 規格別割合（規格11）
                        string kikakuWariai11 = fields[32].Trim('"');

                        // (33) 規格別割合（規格12）
                        string kikakuWariai12 = fields[33].Trim('"');

                        // (34) 規格別割合（規格13）
                        string kikakuWariai13 = fields[34].Trim('"');

                        // (35) 規格別割合（規格14）
                        string kikakuWariai14 = fields[35].Trim('"');

                        // (36) 規格別割合（規格15）
                        string kikakuWariai15 = fields[36].Trim('"');

                        // (37) 規格別割合（規格16）
                        string kikakuWariai16 = fields[37].Trim('"');

                        // (38) 規格別割合（規格17）
                        string kikakuWariai17 = fields[38].Trim('"');

                        // (39) 規格別割合（規格18）
                        string kikakuWariai18 = fields[39].Trim('"');

                        // (40) 規格別割合（規格19）
                        string kikakuWariai19 = fields[40].Trim('"');

                        // (41) 規格別割合（規格20）
                        string kikakuWariai20 = fields[41].Trim('"');

                        // (42) 規格別割合（規格21）
                        string kikakuWariai21 = fields[42].Trim('"');

                        // (43) 規格別割合（規格22）
                        string kikakuWariai22 = fields[43].Trim('"');

                        // (44) 規格別割合（規格23）
                        string kikakuWariai23 = fields[44].Trim('"');

                        // (45) 規格別割合（規格24）
                        string kikakuWariai24 = fields[45].Trim('"');

                        // (46) 規格別割合（規格25）
                        string kikakuWariai25 = fields[46].Trim('"');

                        // (47) 規格別割合（規格26）
                        string kikakuWariai26 = fields[47].Trim('"');

                        // (48) 規格別割合（規格27）
                        string kikakuWariai27 = fields[48].Trim('"');

                        // (49) 規格別割合（規格28）
                        string kikakuWariai28 = fields[49].Trim('"');

                        // (50) 規格別割合（規格29）
                        string kikakuWariai29 = fields[50].Trim('"');

                        // (51) 規格別割合（規格30）
                        string kikakuWariai30 = fields[51].Trim('"');

                        // (52) 規格別割合（規格31）
                        string kikakuWariai31 = fields[52].Trim('"');

                        // (53) 規格別割合（規格32）
                        string kikakuWariai32 = fields[53].Trim('"');

                        // (54) 規格別割合（規格33）
                        string kikakuWariai33 = fields[54].Trim('"');

                        // (55) 規格別割合（規格34）
                        string kikakuWariai34 = fields[55].Trim('"');

                        // (56) 規格別割合（規格35）
                        string kikakuWariai35 = fields[56].Trim('"');

                        // (57) 規格別割合（規格36）
                        string kikakuWariai36 = fields[57].Trim('"');

                        // (58) 規格別割合（規格37）
                        string kikakuWariai37 = fields[58].Trim('"');

                        // (59) 規格別割合（規格38）
                        string kikakuWariai38 = fields[59].Trim('"');

                        // (60) 規格別割合（規格39）
                        string kikakuWariai39 = fields[60].Trim('"');

                        // (61) 規格別割合（規格40）
                        string kikakuWariai40 = fields[61].Trim('"');

                        
                        if (0 == GetKyosaiMokutekiCount(db, kyosaiMokutekiCd))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "共済目的コード");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (0 == GetHikiukeHousiki(db, hikiukeHoshiki))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "引受方式");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        if (0 == GetRuiKbnCount(db, ruikubun, kyosaiMokutekiCd))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "類区分");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (0 == GetSanchiBetuCount(db, kumiaitoCd, nensanF, kyosaiMokutekiCd, sanchiBetsuMeigaraCd))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "産地別銘柄");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        if (0 == GetNogyoshaCount(db, kumiaitoCd, kumiaiIntoCode))
                        {
                            errorMsg = MessageUtil.Get("ME10016", "組合員等コード", "農業者情報管理システム");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        if (!kumiaitoCd.Equals(kumiaiTouCode))
                        {
                            errorMsg = "組合等コードが他県のコードとなっています。";
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        if (!nensan.Equals(nensanF))
                        {
                            errorMsg = "年産が不正です。ポータル画面選択された年産と一致していません。";
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        if (0 < GetDuplicateCheck(db,ukeireRirekiId,lineNumber, kumiaiTouCode, nensanF,kyosaiMokutekiCd,kumiaiIntoCode,ruikubun,sanchiBetsuMeigaraCd))
                        {
                            errorMsg = MessageUtil.Get("ME90018", "受入ファイル");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        
                        
                        int shukakuSetResult = GetShukakuryoSettingCount(db, kumiaitoCd, nensanF, kyosaiMokutekiCd, kumiaiIntoCode, ruikubun, sanchiBetsuMeigaraCd, einouTaishogaiFlag);
                        if ("1".Equals(shoriKubun) && 0 == shukakuSetResult)
                        {
                            errorMsg = MessageUtil.Get("ME90018", "受入ファイル");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        else if ("2".Equals(shoriKubun) && 0 < shukakuSetResult)
                        {
                            errorMsg = MessageUtil.Get("ME90018", "受入ファイル");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }
                        else if ("3".Equals(shoriKubun) && 0 == shukakuSetResult)
                        {
                            errorMsg = MessageUtil.Get("ME90018", "受入ファイル");
                            logger.Error(errorMsg);
                            errCount++;
                            ukeireErrorList.Add(new ErrorList { エラー内容 = errorMsg, 行番号 = lineNumber });
                            continue;
                        }

                        var dataOk = new T19020大量データ受入基準収穫量ok
                        {
                            受入履歴id = long.Parse(ukeireRirekiId),
                            行番号 = lineNumber,
                            処理区分 = shoriKubun,
                            組合等コード = kumiaiTouCode,
                            組合等名 = kumiaiTouMei,
                            年産 = short.Parse(nensanF),
                            共済目的コード = kyosaiMokutekiCd,
                            共済目的名 = kyosaiMokutekiMei,
                            引受方式 = hikiukeHoshiki,
                            引受方式名称 = hikiukeHoshikiMei,
                            支所コード = shishoCode,
                            支所名 = shishoName,
                            大地区コード = daichikuCode,
                            大地区名 = daichikuName,
                            小地区コード = shouchikuCode,
                            小地区名 = shouchikuName,
                            組合員等コード = kumiaiIntoCode,
                            組合員等氏名 = kumiaiIntoName,
                            類区分 = ruikubun,
                            類区分名 = ruikubunName,
                            産地別銘柄コード = sanchiBetsuMeigaraCd,
                            営農対象外フラグ = einouTaishogaiFlag,
                            産地別銘柄等名称 = sanchiBetsuMeigaraName,
                            平均単収 = averageYield,

                            規格別割合_規格１ = string.IsNullOrWhiteSpace(kikakuWariai1) ? 0m : decimal.Parse(kikakuWariai1),
                            規格別割合_規格２ = string.IsNullOrWhiteSpace(kikakuWariai2) ? 0m : decimal.Parse(kikakuWariai2),
                            規格別割合_規格３ = string.IsNullOrWhiteSpace(kikakuWariai3) ? 0m : decimal.Parse(kikakuWariai3),
                            規格別割合_規格４ = string.IsNullOrWhiteSpace(kikakuWariai4) ? 0m : decimal.Parse(kikakuWariai4),
                            規格別割合_規格５ = string.IsNullOrWhiteSpace(kikakuWariai5) ? 0m : decimal.Parse(kikakuWariai5),
                            規格別割合_規格６ = string.IsNullOrWhiteSpace(kikakuWariai6) ? 0m : decimal.Parse(kikakuWariai6),
                            規格別割合_規格７ = string.IsNullOrWhiteSpace(kikakuWariai7) ? 0m : decimal.Parse(kikakuWariai7),
                            規格別割合_規格８ = string.IsNullOrWhiteSpace(kikakuWariai8) ? 0m : decimal.Parse(kikakuWariai8),
                            規格別割合_規格９ = string.IsNullOrWhiteSpace(kikakuWariai9) ? 0m : decimal.Parse(kikakuWariai9),
                            規格別割合_規格１０ = string.IsNullOrWhiteSpace(kikakuWariai10) ? 0m : decimal.Parse(kikakuWariai10),
                            規格別割合_規格１１ = string.IsNullOrWhiteSpace(kikakuWariai11) ? 0m : decimal.Parse(kikakuWariai11),
                            規格別割合_規格１２ = string.IsNullOrWhiteSpace(kikakuWariai12) ? 0m : decimal.Parse(kikakuWariai12),
                            規格別割合_規格１３ = string.IsNullOrWhiteSpace(kikakuWariai13) ? 0m : decimal.Parse(kikakuWariai13),
                            規格別割合_規格１４ = string.IsNullOrWhiteSpace(kikakuWariai14) ? 0m : decimal.Parse(kikakuWariai14),
                            規格別割合_規格１５ = string.IsNullOrWhiteSpace(kikakuWariai15) ? 0m : decimal.Parse(kikakuWariai15),
                            規格別割合_規格１６ = string.IsNullOrWhiteSpace(kikakuWariai16) ? 0m : decimal.Parse(kikakuWariai16),
                            規格別割合_規格１７ = string.IsNullOrWhiteSpace(kikakuWariai17) ? 0m : decimal.Parse(kikakuWariai17),
                            規格別割合_規格１８ = string.IsNullOrWhiteSpace(kikakuWariai18) ? 0m : decimal.Parse(kikakuWariai18),
                            規格別割合_規格１９ = string.IsNullOrWhiteSpace(kikakuWariai19) ? 0m : decimal.Parse(kikakuWariai19),
                            規格別割合_規格２０ = string.IsNullOrWhiteSpace(kikakuWariai20) ? 0m : decimal.Parse(kikakuWariai20),
                            規格別割合_規格２１ = string.IsNullOrWhiteSpace(kikakuWariai21) ? 0m : decimal.Parse(kikakuWariai21),
                            規格別割合_規格２２ = string.IsNullOrWhiteSpace(kikakuWariai22) ? 0m : decimal.Parse(kikakuWariai22),
                            規格別割合_規格２３ = string.IsNullOrWhiteSpace(kikakuWariai23) ? 0m : decimal.Parse(kikakuWariai23),
                            規格別割合_規格２４ = string.IsNullOrWhiteSpace(kikakuWariai24) ? 0m : decimal.Parse(kikakuWariai24),
                            規格別割合_規格２５ = string.IsNullOrWhiteSpace(kikakuWariai25) ? 0m : decimal.Parse(kikakuWariai25),
                            規格別割合_規格２６ = string.IsNullOrWhiteSpace(kikakuWariai26) ? 0m : decimal.Parse(kikakuWariai26),
                            規格別割合_規格２７ = string.IsNullOrWhiteSpace(kikakuWariai27) ? 0m : decimal.Parse(kikakuWariai27),
                            規格別割合_規格２８ = string.IsNullOrWhiteSpace(kikakuWariai28) ? 0m : decimal.Parse(kikakuWariai28),
                            規格別割合_規格２９ = string.IsNullOrWhiteSpace(kikakuWariai29) ? 0m : decimal.Parse(kikakuWariai29),
                            規格別割合_規格３０ = string.IsNullOrWhiteSpace(kikakuWariai30) ? 0m : decimal.Parse(kikakuWariai30),
                            規格別割合_規格３１ = string.IsNullOrWhiteSpace(kikakuWariai31) ? 0m : decimal.Parse(kikakuWariai31),
                            規格別割合_規格３２ = string.IsNullOrWhiteSpace(kikakuWariai32) ? 0m : decimal.Parse(kikakuWariai32),
                            規格別割合_規格３３ = string.IsNullOrWhiteSpace(kikakuWariai33) ? 0m : decimal.Parse(kikakuWariai33),
                            規格別割合_規格３４ = string.IsNullOrWhiteSpace(kikakuWariai34) ? 0m : decimal.Parse(kikakuWariai34),
                            規格別割合_規格３５ = string.IsNullOrWhiteSpace(kikakuWariai35) ? 0m : decimal.Parse(kikakuWariai35),
                            規格別割合_規格３６ = string.IsNullOrWhiteSpace(kikakuWariai36) ? 0m : decimal.Parse(kikakuWariai36),
                            規格別割合_規格３７ = string.IsNullOrWhiteSpace(kikakuWariai37) ? 0m : decimal.Parse(kikakuWariai37),
                            規格別割合_規格３８ = string.IsNullOrWhiteSpace(kikakuWariai38) ? 0m : decimal.Parse(kikakuWariai38),
                            規格別割合_規格３９ = string.IsNullOrWhiteSpace(kikakuWariai39) ? 0m : decimal.Parse(kikakuWariai39),
                            規格別割合_規格４０ = string.IsNullOrWhiteSpace(kikakuWariai40) ? 0m : decimal.Parse(kikakuWariai40),


                            登録日時 = sysDate,
                            登録ユーザid = BATCH_USER_NAME
                        };
                        // DBへ追加して保存
                        db.T19020大量データ受入基準収穫量oks.Add(dataOk);
                        okListForCsvFile.Add(dataOk);
                        okCount++;
                    }
                    db.SaveChanges();
                    /*
                     * TODO:ファイル格納先と処理のロジックが決まってないため、
                     * 必要に応じて修正
                     */
                    if (1 <= okListForCsvFile.Count)
                    {
                        // 10.1.1 一時領域にデータ一時出力フォルダとファイルを作成する
                        // ※設定ファイルから一時領域のルートパスを取得（例：Constants.FILE_TEMP_FOLDER_PATH）
                        //     フォルダ名は "[バッチID]_[yyyyMMddHHmmss]" の形式とする
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string tempRootFolder = ConfigUtil.Get(Constants.FILE_TEMP_FOLDER_PATH); // 例："C:\\SYN\\112041Temp"
                        string tempFolderPath = Path.Combine(tempRootFolder, $"{bid}_{timestamp}");

                        if (!Directory.Exists(tempFolderPath))
                        {
                            Directory.CreateDirectory(tempFolderPath);
                        }

                        // エラーリストファイル名の生成
                        // 形式: [取込前ファイル名]-ERR-[取込履歴ID].csv
                        string okFileName = $"{resultTairyoData.取込ファイル_変更後ファイル名}-OK-{ukeireRirekiId}.csv";
                        string tempFilePath = Path.Combine(tempFolderPath, okFileName);

                        okListName = okFileName; 

                        // CSV生成用の StringBuilder
                        StringBuilder sb = new StringBuilder();

                        // ◆ヘッダ部（1行目）
                        // すべての列名をダブルクォートで囲み、カンマ区切りで連結します
                        sb.AppendLine("\"処理区分\",\"組合等コード\",\"組合等名\",\"年産\",\"共済目的コード\",\"共済目的名\",\"引受方式\",\"引受方式名称\",\"支所コード\",\"支所名\",\"大地区コード\",\"大地区名\",\"小地区コード\",\"小地区名\",\"組合員等コード\",\"組合員等氏名\",\"類区分\",\"類区分名\",\"産地別銘柄コード\",\"営農対象外フラグ\",\"産地別銘柄等名称\",\"平均単収\",\"規格別割合_規格１\",\"規格別割合_規格２\",\"規格別割合_規格３\",\"規格別割合_規格４\",\"規格別割合_規格５\",\"規格別割合_規格６\",\"規格別割合_規格７\",\"規格別割合_規格８\",\"規格別割合_規格９\",\"規格別割合_規格１０\",\"規格別割合_規格１１\",\"規格別割合_規格１２\",\"規格別割合_規格１３\",\"規格別割合_規格１４\",\"規格別割合_規格１５\",\"規格別割合_規格１６\",\"規格別割合_規格１７\",\"規格別割合_規格１８\",\"規格別割合_規格１９\",\"規格別割合_規格２０\",\"規格別割合_規格２１\",\"規格別割合_規格２２\",\"規格別割合_規格２３\",\"規格別割合_規格２４\",\"規格別割合_規格２５\",\"規格別割合_規格２６\",\"規格別割合_規格２７\",\"規格別割合_規格２８\",\"規格別割合_規格２９\",\"規格別割合_規格３０\",\"規格別割合_規格３１\",\"規格別割合_規格３２\",\"規格別割合_規格３３\",\"規格別割合_規格３４\",\"規格別割合_規格３５\",\"規格別割合_規格３６\",\"規格別割合_規格３７\",\"規格別割合_規格３８\",\"規格別割合_規格３９\",\"規格別割合_規格４０\"");

                        // ◆データ部（2行目以降）
                        // 各項目をダブルクォートで囲み、カンマ区切りで連結します。
                        foreach (var dataOk in okListForCsvFile)
                        {
                            sb.AppendLine(
                                $"\"{dataOk.処理区分}\"," +
                                $"\"{dataOk.組合等コード}\"," +
                                $"\"{dataOk.組合等名}\"," +
                                $"\"{dataOk.年産}\"," +
                                $"\"{dataOk.共済目的コード}\"," +
                                $"\"{dataOk.共済目的名}\"," +
                                $"\"{dataOk.引受方式}\"," +
                                $"\"{dataOk.引受方式名称}\"," +
                                $"\"{dataOk.支所コード}\"," +
                                $"\"{dataOk.支所名}\"," +
                                $"\"{dataOk.大地区コード}\"," +
                                $"\"{dataOk.大地区名}\"," +
                                $"\"{dataOk.小地区コード}\"," +
                                $"\"{dataOk.小地区名}\"," +
                                $"\"{dataOk.組合員等コード}\"," +
                                $"\"{dataOk.組合員等氏名}\"," +
                                $"\"{dataOk.類区分}\"," +
                                $"\"{dataOk.類区分名}\"," +
                                $"\"{dataOk.産地別銘柄コード}\"," +
                                $"\"{dataOk.営農対象外フラグ}\"," +
                                $"\"{dataOk.産地別銘柄等名称}\"," +
                                $"\"{dataOk.平均単収}\"," +
                                $"\"{dataOk.規格別割合_規格１}\"," +
                                $"\"{dataOk.規格別割合_規格２}\"," +
                                $"\"{dataOk.規格別割合_規格３}\"," +
                                $"\"{dataOk.規格別割合_規格４}\"," +
                                $"\"{dataOk.規格別割合_規格５}\"," +
                                $"\"{dataOk.規格別割合_規格６}\"," +
                                $"\"{dataOk.規格別割合_規格７}\"," +
                                $"\"{dataOk.規格別割合_規格８}\"," +
                                $"\"{dataOk.規格別割合_規格９}\"," +
                                $"\"{dataOk.規格別割合_規格１０}\"," +
                                $"\"{dataOk.規格別割合_規格１１}\"," +
                                $"\"{dataOk.規格別割合_規格１２}\"," +
                                $"\"{dataOk.規格別割合_規格１３}\"," +
                                $"\"{dataOk.規格別割合_規格１４}\"," +
                                $"\"{dataOk.規格別割合_規格１５}\"," +
                                $"\"{dataOk.規格別割合_規格１６}\"," +
                                $"\"{dataOk.規格別割合_規格１７}\"," +
                                $"\"{dataOk.規格別割合_規格１８}\"," +
                                $"\"{dataOk.規格別割合_規格１９}\"," +
                                $"\"{dataOk.規格別割合_規格２０}\"," +
                                $"\"{dataOk.規格別割合_規格２１}\"," +
                                $"\"{dataOk.規格別割合_規格２２}\"," +
                                $"\"{dataOk.規格別割合_規格２３}\"," +
                                $"\"{dataOk.規格別割合_規格２４}\"," +
                                $"\"{dataOk.規格別割合_規格２５}\"," +
                                $"\"{dataOk.規格別割合_規格２６}\"," +
                                $"\"{dataOk.規格別割合_規格２７}\"," +
                                $"\"{dataOk.規格別割合_規格２８}\"," +
                                $"\"{dataOk.規格別割合_規格２９}\"," +
                                $"\"{dataOk.規格別割合_規格３０}\"," +
                                $"\"{dataOk.規格別割合_規格３１}\"," +
                                $"\"{dataOk.規格別割合_規格３２}\"," +
                                $"\"{dataOk.規格別割合_規格３３}\"," +
                                $"\"{dataOk.規格別割合_規格３４}\"," +
                                $"\"{dataOk.規格別割合_規格３５}\"," +
                                $"\"{dataOk.規格別割合_規格３６}\"," +
                                $"\"{dataOk.規格別割合_規格３７}\"," +
                                $"\"{dataOk.規格別割合_規格３８}\"," +
                                $"\"{dataOk.規格別割合_規格３９}\"," +
                                $"\"{dataOk.規格別割合_規格４０}\""
                            );
                        }
                        // ファイル出力（UTF-8、改行はCRLF）
                        File.WriteAllText(tempFilePath, sb.ToString(), Encoding.GetEncoding("Shift_JIS"));

                        // 10.1.3 ファイルのハッシュ値を取得して変数に設定する
                        byte[] csvBytes = File.ReadAllBytes(tempFilePath);
                        okListHashValue = CryptoUtil.GetSHA256Hex(csvBytes);

                        // 10.1.4 ファイルをZIP化して暗号化し、出力先に保存する
                        // 暗号化は、CryptoUtil.Encrypt(byte[] targetData, string fileName) を利用
                        // ここでは、CSVファイルのバイト列を暗号化した上でZIPアーカイブに格納する
                        byte[] encryptedData = CryptoUtil.Encrypt(csvBytes, okFileName);

                        // 出力先フォルダは例："C:\NSK\112041RealOK\[yyyyMMddHHmmss]\"
                        string outputRootFolder = @"C:\NSK\112011RealOK";
                        string outputFolder = Path.Combine(outputRootFolder, timestamp);
                        if (!Directory.Exists(outputFolder))
                        {
                            Directory.CreateDirectory(outputFolder);
                        }
                        // 出力先ファイル名は、エラーリストファイル名に ".zip" を付加（※内部に元のファイル名で格納）
                        string destinationZipFilePath = Path.Combine(outputFolder, okFileName + ".zip");
                        using (var zipStream = new FileStream(destinationZipFilePath, FileMode.Create))
                        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
                        {
                            // ZIP内のエントリ名は元のCSVファイル名とする
                            var zipEntry = archive.CreateEntry(okFileName);
                            using (var entryStream = zipEntry.Open())
                            {
                                entryStream.Write(encryptedData, 0, encryptedData.Length);
                            }
                        }
                        // 保存先のパスを変数に設定する
                        okListPath = destinationZipFilePath;

                        // 10.1.5 一時領域に作成したフォルダとファイルを削除する
                        Directory.Delete(tempFolderPath, true);
                    }
                    if (1 <= ukeireErrorList.Count)
                    {
                        // 10.1.1 一時領域にデータ一時出力フォルダとファイルを作成する
                        // ※設定ファイルから一時領域のルートパスを取得（例：Constants.FILE_TEMP_FOLDER_PATH）
                        //     フォルダ名は "[バッチID]_[yyyyMMddHHmmss]" の形式とする
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string tempRootFolder = ConfigUtil.Get(Constants.FILE_TEMP_FOLDER_PATH); // 例："C:\\SYN\\112041Temp"
                        string tempFolderPath = Path.Combine(tempRootFolder, $"{bid}_{timestamp}");
                        
                        if (!Directory.Exists(tempFolderPath))
                        {
                            Directory.CreateDirectory(tempFolderPath);
                        }

                        // エラーリストファイル名の生成
                        // 形式: [取込ファイル_変更後ファイル名]-ERR-[取込履歴ID].csv
                        string errorFileName = $"{resultTairyoData.取込ファイル_変更後ファイル名}-ERR-{ukeireRirekiId}.csv";
                        string tempFilePath = Path.Combine(tempFolderPath, errorFileName);
                        errorListName = errorFileName;

                        // 10.1.2 エラーリストの内容をCSV形式で作成
                        // ヘッダ： "エラー対象行数","エラー内容"
                        // 各データ行もすべての項目をダブルクォーテーションで囲む
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("\"エラー対象行数\",\"エラー内容\"");
                        int errSeq = 0;
                        foreach (var error in ukeireErrorList)
                        {
                            // 行番号は数値→文字列変換、エラー内容はそのまま
                            sb.AppendLine($"\"{error.行番号}\",\"{error.エラー内容}\"");
                            var dataErrorList = new T01080大量データ受入エラーリスト
                            {
                                処理区分 = "2",
                                履歴id = long.Parse(ukeireRirekiId),
                                枝番 = errSeq,
                                行番号 = error.行番号.ToString(),
                                エラー内容 = error.エラー内容,
                                登録日時 = sysDate,
                                登録ユーザid = BATCH_USER_NAME,
                            };
                            // DbSet に追加
                            db.T01080大量データ受入エラーリストs.Add(dataErrorList);
                            errSeq++;
                        }
                        db.SaveChanges();
                        // ファイル出力（SJIS、改行はCRLF）
                        File.WriteAllText(tempFilePath, sb.ToString(), Encoding.GetEncoding("shift_jis"));

                        // 10.1.3 ファイルのハッシュ値を取得して変数に設定する
                        byte[] csvBytes = File.ReadAllBytes(tempFilePath);
                        errorListHashValue = CryptoUtil.GetSHA256Hex(csvBytes);

                        // 10.1.4 ファイルをZIP化して暗号化し、出力先に保存する
                        // 暗号化は、CryptoUtil.Encrypt(byte[] targetData, string fileName) を利用
                        // ここでは、CSVファイルのバイト列を暗号化した上でZIPアーカイブに格納する
                        byte[] encryptedData = CryptoUtil.Encrypt(csvBytes, errorFileName);

                        // 出力先フォルダは例："C:\NSK\112041Real\[yyyyMMddHHmmss]\"
                        string outputRootFolder = @"C:\NSK\112011RealErr";
                        string outputFolder = Path.Combine(outputRootFolder, timestamp);
                        if (!Directory.Exists(outputFolder))
                        {
                            Directory.CreateDirectory(outputFolder);
                        }
                        // 出力先ファイル名は、エラーリストファイル名に ".zip" を付加（※内部に元のファイル名で格納）
                        string destinationZipFilePath = Path.Combine(outputFolder, errorFileName + ".zip");
                        using (var zipStream = new FileStream(destinationZipFilePath, FileMode.Create))
                        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
                        {
                            // ZIP内のエントリ名は元のCSVファイル名とする
                            var zipEntry = archive.CreateEntry(errorFileName);
                            using (var entryStream = zipEntry.Open())
                            {
                                entryStream.Write(encryptedData, 0, encryptedData.Length);
                            }
                        }
                        // 保存先のパスを変数に設定する
                        errorListPath = destinationZipFilePath;

                        // 10.1.5 一時領域に作成したフォルダとファイルを削除する
                        Directory.Delete(tempFolderPath, true);
                        logger.Error(MessageUtil.Get("ME10026", "取込対象"));
                    }

                    // 12 最終処理
                    forcedShutdown:;
                    
                    if (forcedShutdownFlg)
                    {
                        shoriSts = Constants.TASK_FAILED;
                        logger.Error(errorMsg);
                    }
                    else
                    {
                        shoriSts = Constants.TASK_SUCCESS;
                    }
                    
                    var upStsAfr = "03";
                    
                    if (string.IsNullOrEmpty(tairyoDataSts))
                    {
                        tairyoDataSts = "99";
                    }

                    UpdateUkeireRireki(
                        db
                        , ukeireRirekiId
                        , kumiaitoCd
                        , tairyoDataSts
                        , okCount + errCount
                        , errCount
                        , errorListName
                        , errorListPath
                        , errorListHashValue
                        , okCount
                        , okListName
                        , okListPath
                        , okListHashValue
                        , sysDate
                        , BATCH_USER_NAME
                        , upStsAfr);

                    string refMessage = string.Empty;
                    int updateResult = BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), shoriSts, errorMsg, BATCH_USER_NAME, ref refMessage);
                    // 更新に成功した場合
                    if (0 == updateResult)
                    {
                        // 更新に失敗した場合
                        logger.Error(refMessage);
                        logger.Error(string.Format(Constants.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, shoriSts, refMessage));
                        forcedShutdownFlg = true;
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

                    stopwatch.Stop();
                    var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                    logger.Info(excutingTime);
                }
                catch (Exception e)
                {
                    logger.Error(e.StackTrace);
                    Console.Error.WriteLine(MessageUtil.Get("MF00001"));
                    logger.Error(MessageUtil.Get("MF00001"));
                    logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                    shoriSts = "99";
                    errorMsg = MessageUtil.Get("MF00001");
                    result = Constants.BATCH_EXECUT_FAILED;
                    UpdateBatchYoyakuNsk(result, bid, shoriSts, errorMsg);
                    transaction.Rollback();
                    // 処理結果（正常：0、エラー：1）
                    Environment.ExitCode = Constants.BATCH_EXECUT_FAILED;
                    return;
                }
            }
        }
        /// <summary>
        ///　バッチ条件テーブルからデータ取得
        /// </summary>
        private static List<T01050バッチ条件> GetBatchJoukenList(
            NskAppContext db,
            string joukenId,
            string nensan,
            string ukeirerirekiId)
        {
            logger.Info("バッチ条件テーブルから、指定されたバッチ条件idおよび条件名称に一致するデータを取得します。");

            // IN句に相当する条件を配列で用意
            var conditionNames = new[] { nensan, ukeirerirekiId };

            // LINQクエリで条件に合致するレコードを取得
            var results = db.Set<T01050バッチ条件>()
                            .Where(b => b.バッチ条件id == joukenId &&
                                        conditionNames.Contains(b.条件名称))
                            .ToList();

            return results;
        }
        /// <summary>
        ///　大量データ履歴の取得
        /// </summary>
        private static TairyoDataDto GetTairyoData(NskAppContext db, string ukeireRirekiId)
        {
            
            var result = (
                from t1 in db.Set<T01060大量データ受入履歴>()
                where t1.受入履歴id == ukeireRirekiId
                select new TairyoDataDto
                {
                    受入履歴id = t1.受入履歴id,
                    組合等コード = t1.組合等コード,
                    ステータス = t1.ステータス,
                    対象データ区分 = t1.対象データ区分,
                    取込ファイル_変更後ファイル名 = t1.取込ファイル_変更後ファイル名,
                    取込ファイルハッシュ値 = t1.取込ファイルハッシュ値
                }
            ).FirstOrDefault();

            return result;
        }



        /// <summary>
        ///　エラーリスト履歴更新処理
        /// </summary>
        private static int UpdateUkeireRireki(
            NskAppContext db,
            string ukeireRirekiId,
            string kumiaitoCd,
            string upStsBf,
            int totalCount,
            int errorCount,
            string errorListName,
            string errorListPath,
            string errorListHashValue,
            int okCount,
            string okListName,
            string okListPath,
            string okListHashValue,
            DateTime sysDate,
            string userId,
            string upStsAft)
        {
            var entity = db.Set<T01060大量データ受入履歴>()
                           .FirstOrDefault(x => x.受入履歴id == ukeireRirekiId &&
                                                x.組合等コード == kumiaitoCd &&
                                                x.ステータス == upStsBf);
            if (entity == null)
            {
                return 0;
            }

            entity.ステータス = upStsAft;
            entity.対象件数 = totalCount;
            entity.エラー件数 = errorCount;
            entity.エラーリスト名 = errorListName;
            entity.エラーリストパス = errorListPath;
            entity.エラーリストハッシュ値 = errorListHashValue;
            entity.OK件数 = okCount;
            entity.OKリスト名 = okListName;
            entity.OKリストパス = okListPath;
            entity.OKリストハッシュ値 = okListHashValue;
            entity.終了日時 = sysDate;
            entity.更新日時 = sysDate;
            entity.更新ユーザid = BATCH_USER_NAME;

            return db.SaveChanges();
        }

        /// <summary>
        ///　都道府県件数取得
        /// </summary>
        private static int GetTodofukenCount(NskAppContext db, string todofukenCd)
        {
            int todofuken = db.VTodofukens
                .Where(x => x.TodofukenCd == todofukenCd)
                .Count();
            return todofuken;
        }


        /// <summary>
        /// 組合等コード件数取得
        /// </summary>
        private static int GetKumiaitoCount(NskAppContext db, string todofukenCd, string kumiaitoCd)
        {
            int kumiaitoCn = db.VKumiaitos
                     .Where(x => x.TodofukenCd == todofukenCd &&
                                 x.KumiaitoCd == kumiaitoCd)
                     .Count();

            return kumiaitoCn;
        }

        /// <summary>
        /// 支所コード件数取得
        /// </summary>
        private static int GetShishoCdCount(NskAppContext db, string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            int shishoCn = db.VShishoNms
                     .Where(x => x.TodofukenCd == todofukenCd &&
                                 x.KumiaitoCd == kumiaitoCd   &&
                                 x.ShishoCd == shishoCd)
                     .Count();

            return shishoCn;
        }

        private static int GetDuplicateCheck(
            NskAppContext db
            , string ukeireRirekiId
            , int lineNum
            , string kumiaitoCd
            , string nensan
            , string kyosaimokutekiCd
            , string kumiaiintoCd
            , string ruiKbn
            , string sanchiBetsuMeigaraCd)
        {
            return db.Set<T19020大量データ受入基準収穫量ok>()
                     .Count(x => x.受入履歴id == long.Parse(ukeireRirekiId) &&
                                 x.行番号 == lineNum &&
                                 x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd &&
                                 x.類区分 == ruiKbn &&
                                 x.産地別銘柄コード == sanchiBetsuMeigaraCd);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="kumiaitoCd"></param>
        /// <param name="nensan"></param>
        /// <param name="kyosaimokutekiCd"></param>
        /// <param name="kumiaiintoCd"></param>
        /// <param name="ruiKbn"></param>
        /// <param name="sanchiBetsuMeigaraCd"></param>
        /// <param name="einouTaishogaiFlg"></param>
        /// <returns></returns>
        private static int GetShukakuryoSettingCount(
            NskAppContext db
            , string kumiaitoCd
            , string nensan
            , string kyosaimokutekiCd
            , string kumiaiintoCd
            , string ruiKbn
            , string sanchiBetsuMeigaraCd
            , string einouTaishogaiFlg)
        {
            return db.Set<T11060基準収穫量設定>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd &&
                                 x.類区分 == ruiKbn &&
                                 x.産地別銘柄コード == sanchiBetsuMeigaraCd &&
                                 x.営農対象外フラグ == einouTaishogaiFlg);
        }

        /// <summary>
        /// 共済目的名称件数取得
        /// </summary>
        private static int GetKyosaiMokutekiCount(NskAppContext db, string kyosaiMokutekiCd)
        {
            return db.Set<M00010共済目的名称>()
                     .Count(x => x.共済目的コード == kyosaiMokutekiCd);
        }

        /// <summary>
        /// 類名称件数取得
        /// </summary>
        private static int GetRuiKbnCount(NskAppContext db, string ruiKbn, string kyousaimokutekiCd)
        {
            return db.Set<M00020類名称>()
                     .Count(x => x.類区分 == ruiKbn && 
                                 x.共済目的コード == kyousaimokutekiCd);
        }

        /// <summary>
        /// 産地別銘柄名称設定件数取得
        /// </summary>
        private static int GetSanchiBetuCount(
            NskAppContext db
            ,string kumiaitoCd
            ,string nensan
            ,string kyosaimokutekiCd
            ,string sanchibetuMeigaraCd)
        {
            return db.Set<M00130産地別銘柄名称設定>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.産地別銘柄コード == sanchibetuMeigaraCd);
        }

        /// <summary>
        /// 引受方式名称件数取得
        /// </summary>
        private static int GetHikiukeHousiki(NskAppContext db, string hikiukeHousiki)
        {
            logger.Info($"引受方式 {hikiukeHousiki} のデータを取得します。");
            return db.Set<M10080引受方式名称>()
                     .Count(x => x.引受方式 == hikiukeHousiki);
        }

        /// <summary>
        /// 農業者件数取得
        /// </summary>
        private static int GetNogyoshaCount(NskAppContext db, string kumiaitoCd, string kumiaiintoCd)
        {
            logger.Info($"組合員コード {kumiaitoCd}, 組合員等コード {kumiaiintoCd} の件数を取得します。");
            return db.Set<VNogyosha>()
                     .Count(x => x.KumiaitoCd == kumiaitoCd &&
                                 x.KumiaiintoCd == kumiaiintoCd);
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
        /// OKデータDTOクラス
        /// </summary>
        public class TairyoDataDto
        {
            public string 受入履歴id { get; set; }
            public string 組合等コード { get; set; }
            public string ステータス { get; set; }
            public string 対象データ区分 { get; set; }
            public string 取込ファイル_変更後ファイル名 { get; set; }
            public string 取込ファイルハッシュ値 { get; set; }
        }

        /// <summary>
        /// エラーリスト格納クラス
        /// </summary>
        public class ErrorList
        {
            public string エラー内容 { get; set; }
            public Decimal 行番号 { get; set; }
        }
    }
}
