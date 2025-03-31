using NSK_B112091.Common;
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
using System.Runtime.InteropServices;
using System.IO.Compression;

namespace NSK_B112091
{
    /// <summary>
    /// 定時実行予約登録
    /// </summary>
    class Program
    {
        /// <summary>
        /// バッチ名
        /// </summary>
        private static string BATCH_NAME = "組合員等類別平均単収大量受入データ取込";
        private static string BATCH_USER_NAME = "NSK_112091B";

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
            logger.Info(string.Concat(CoreConst.LOG_START_KEYWORD, " 組合員等類別平均単収大量受入データ取込"));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //ConfigUtil.Get(

            var bid = string.Empty;
            var todofukenCd = string.Empty;
            var kumiaitoCd = string.Empty;
            var shishoCd = string.Empty;
            var jid = string.Empty;
            //バッチ条件テーブルから取得する条件
            var nensan = string.Empty;
            var ukeirerirekiId = string.Empty;
            var filePath = string.Empty;
            var fileHash = string.Empty;

            var ukeireSts = "失敗";
            var kumiaitoCdUkeire = string.Empty;
            var shoriSts = "99";
            var errorMsg = string.Empty;

            int OK件数 = 0;
            int エラー件数 = 0;
            var totalCount = 0;
            var errorListName = string.Empty;
            var errorListPath = string.Empty;
            var errorListHashValue = string.Empty;
            var okListName = string.Empty;
            var okListPath = string.Empty;
            var okListHashValue = string.Empty;
            var extractedFilePath = string.Empty;
            string preNm = string.Empty;

            var tempOkFolderPath = string.Empty;
            var tempErrorFolderPath = string.Empty;

            var batchSts = "";

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
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, errorMsg);
                return;
            }

            if (bid == null || "".Equals(bid))
            {
                errorMsg = MessageUtil.Get("ME01054", "バッチID");
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, errorMsg);
                return;
            }

            if (todofukenCd == null || "".Equals(todofukenCd))
            {
                errorMsg = MessageUtil.Get("ME01054", "都道府県コード");
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, errorMsg);
                return;
            }

            if (kumiaitoCd == null || "".Equals(kumiaitoCd))
            {
                errorMsg = MessageUtil.Get("ME01054", "組合等コード");
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                logger.Info(excutingTime);
                UpdateBatchYoyakuNsk(result, bid, shoriSts, errorMsg);
                return;
            }
            if (jid == null || "".Equals(jid))
            {
                errorMsg = MessageUtil.Get("ME01054", "条件ID");
                stopwatch.Stop();
                var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
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
            var ukeireErrorList = new List<ErrorList>();
            var ukeireOkList = new List<T19050大量データ受入組合員等類別設定ok>();
            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema, ConfigUtil.GetInt(Constants.CONFIG_COMMAND_TIMEOUT)))
            {
                var transaction = db.Database.BeginTransaction();
                try
                {
                    // バッチ条件情報取得
                    List<T01050バッチ条件> joukenList = GetBatchJoukenList(db, bid,Constants.JOUKEN_NENSAN ,Constants.JOUKEN_FILE_PATH, Constants.JOUKEN_UKEIRERIREKI_ID, Constants.JOUKEN_FILE_HASH);
                    if (joukenList.IsNullOrEmpty())
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        logger.Error(errorMsg);
                        goto forcedShutdown;
                    }
                    for (int i = 0; i < joukenList.Count; i++)
                    {
                        if (Constants.JOUKEN_NENSAN.Equals(joukenList[i].条件名称))
                        {
                            nensan = joukenList[i].条件値;
                        }
                        if (Constants.JOUKEN_UKEIRERIREKI_ID.Equals(joukenList[i].条件名称))
                        {
                            ukeirerirekiId = joukenList[i].条件値;
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
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        logger.Error(errorMsg);
                        goto forcedShutdown;
                    }

                    if (string.IsNullOrEmpty(ukeirerirekiId))
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        logger.Error(errorMsg);
                        goto forcedShutdown;
                    }

                    if (string.IsNullOrEmpty(filePath))
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        logger.Error(errorMsg);
                        goto forcedShutdown;
                    }
                    if (string.IsNullOrEmpty(fileHash))
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        logger.Error(errorMsg);
                        goto forcedShutdown;
                    }

                    T01060大量データ受入履歴 ukeireRirekiResult = GetUkeireRireki(db, ukeirerirekiId);
                    if (ukeireRirekiResult == null)
                    {
                        errorMsg = MessageUtil.Get("ME01645", "パラメータの取得");
                        logger.Error(errorMsg);
                        goto forcedShutdown;
                    }

                    ukeireSts = ukeireRirekiResult.ステータス;
                    kumiaitoCdUkeire = ukeireRirekiResult.組合等コード;
                    preNm = ukeireRirekiResult.取込ファイル_変更前ファイル名;
                    // 処理待ち
                    if (!"01".Equals(ukeireSts))
                    {
                        errorMsg = MessageUtil.Get("ME10042", "受入処理");
                        logger.Error(errorMsg);
                        goto forcedShutdown;
                    }
                    // 処理中
                    ukeireSts = "02";

                    UpdateUkeireRireki(db, ukeirerirekiId, ukeireSts, sysDate);

                    // 都道府県マスタチェック情報
                    int todoCount = GetTodofukenCount(db, todofukenCd);
                    if (todoCount == 0)
                    {
                        errorMsg = MessageUtil.Get("ME10005", "都道府県コード");
                        logger.Error(errorMsg);
                        goto forcedShutdown;
                    }

                    //組合等マスタチェック情報
                    int kumiCount = GetKumiaitoCount(db, todofukenCd, kumiaitoCd);
                    if (kumiCount == 0)
                    {
                        errorMsg = MessageUtil.Get("ME10005", "組合等コード");
                        logger.Error(errorMsg);
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
                        errorMsg = MessageUtil.Get("ME01645", "ファイル取得");
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
                        errorMsg = MessageUtil.Get("ME01645", "ファイルの解凍");
                        goto forcedShutdown;
                    }

                    // 3. 解凍先から対象のTXTファイルを取得（csvであることが前提）
                    extractedFilePath = Directory.GetFiles(extractFolder, "*.*", SearchOption.TopDirectoryOnly)
                                                        .FirstOrDefault(f => Path.GetExtension(f).ToLower() == ".csv");

                    logger.Debug("extractedFilePath :::: " + extractedFilePath);
                    if (string.IsNullOrEmpty(extractedFilePath))
                    {
                        logger.Error("ERROR: 解凍されたcsvファイルが見つかりません。");
                        errorMsg = MessageUtil.Get("ME10050");
                        goto forcedShutdown;
                    }

                    // ファイル拡張子の再チェック（念のため）
                    if (Path.GetExtension(extractedFilePath)?.ToLower() != ".csv")
                    {
                        logger.Error("ERROR: ファイル拡張子の再チェック");
                        errorMsg = MessageUtil.Get("ME10050");
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
                        errorMsg = MessageUtil.Get("ME10052", "ファイル名");
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
                        goto forcedShutdown;
                    }
                    string headerLine = allLines[0];
                    string[] headerFields = headerLine.Split(',');
                    for (int i = 0; i < headerFields.Length; i++)
                    {
                        logger.Debug("headerFields[i] : " + headerFields[i]);
                    }
                    if (headerFields.Length != 7)
                    {
                        errorMsg = MessageUtil.Get("ME10066");
                        goto forcedShutdown;
                    }
                    
                    // 11.2 ヘッダ部の１項目目が「組合等コード」でない場合
                    if (headerFields[0] != "組合等コード")
                    {
                        errorMsg = MessageUtil.Get("ME10066");
                        goto forcedShutdown;
                    }
                    if (allLines.Length < 2)
                    {
                        errorMsg = MessageUtil.Get("ME90015", "データ行");
                        goto forcedShutdown;
                    }

                    // 13. 初期変数の設定
                    int lineNumber = 1; // ヘッダ行を1行目とする
                    string errorContent = "";
                    int errListSeq = 0;

                    // 14. 取込ファイルのデータ行（2行目以降）チェック
                    for (int i = 1; i < allLines.Length; i++)
                    {
                        string line = allLines[i];
                        // 14.2 空行の場合は処理終了（EOFと同様）
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            break;
                        }

                        // 14.3 行番号の設定（読み込むたびに +1）
                        lineNumber++;

                        // CSV の項目はカンマ区切りとして分割
                        string[] fields = line.Split(',');

                        // 14.4 項目数チェック（必ず 7 項目であることを確認）
                        if (fields.Length != 7)
                        {
                            errorMsg = MessageUtil.Get("ME10066");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }

                        // 各項目のチェック
                        // --- 項目番号と内容 ---
                        // 1: 組合等コード　　→ 必須、数字、文字数3（例："123"）
                        // 2: 年産　　　　　　→ 必須、数字、文字数4（例："2023"）
                        // 3: 共済目的コード　→ 必須、数字、文字数2（例："01"）
                        // 4: 組合員等コード　→ 必須、数字、文字数13（例："1234567890123"）
                        // 5: 類区分　　　　　→ 必須、数字、文字数2（例："02"）
                        // 6: 引受区分　　　　→ 必須、数字、文字数2（例："03"）
                        // 7: 全相殺基準単収　→ 数字、文字数4、表示形式(zzz9) ※必須でない可能性あり

                        errorMsg = ""; // 初期化

                        // 項目1: 組合等コードチェック
                        if (string.IsNullOrWhiteSpace(fields[0].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "組合等コード", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }
                        else if (fields[0].Trim('"').Length > 3)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "組合等コード", "3", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }
                        else if (!IsNumeric(fields[0].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "組合等コード", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }

                        // 項目2: 年産チェック
                        if (string.IsNullOrWhiteSpace(fields[1].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "年産", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }
                        else if (fields[1].Trim('"').Length > 4)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "年産", "3", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }
                        else if (!IsNumeric(fields[1].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "年産", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }

                        // 項目3: 共済目的コードチェック
                        if (string.IsNullOrWhiteSpace(fields[2].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "共済目的コード", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }
                        else if (fields[2].Trim('"').Length > 2)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "共済目的コード", "3", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }
                        else if (!IsNumeric(fields[2].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "共済目的コード", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }

                        // 項目4: 組合員等コードチェック
                        if (string.IsNullOrWhiteSpace(fields[3].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "組合員等コード", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }
                        else if (fields[3].Trim('"').Length > 13)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "組合員等コード", "3", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }
                        else if (!IsNumeric(fields[3].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "組合員等コード", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }

                        // 項目5: 類区分チェック
                        if (string.IsNullOrWhiteSpace(fields[4].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "類区分", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }
                        else if (fields[4].Trim('"').Length > 2)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "類区分", "3", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }
                        else if (!IsNumeric(fields[4].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "類区分", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }

                        // 項目6: 引受区分チェック
                        if (string.IsNullOrWhiteSpace(fields[5].Trim('"')))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "引受区分", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }
                        else if (!IsNumeric(fields[5]) || fields[5].Length > 2)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "引受区分", "3", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }
                        else if (!IsNumeric(fields[5].Trim('"')) || fields[5].Trim('"').Length > 2)
                        {
                            errorMsg = MessageUtil.Get("ME00003", "引受区分", "(" + lineNumber + "行目)");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }

                        // 項目7: 全相殺基準単収チェック（※必須ではない場合）
                        if (!string.IsNullOrWhiteSpace(fields[6].Trim('"')))
                        {
                            if (fields[6].Trim('"').Length > 4)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "全相殺基準単収", "4", "(" + lineNumber + "行目)");
                                AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                                var entityForFile = new ErrorList
                                {
                                    エラー内容 = errorMsg,
                                    行番号 = lineNumber
                                };
                                ukeireErrorList.Add(entityForFile);
                                エラー件数++;
                                continue; // 以降のチェックはスキップ
                            }
                            else if (!IsNumeric(fields[6].Trim('"')))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "全相殺基準単収", "(" + lineNumber + "行目)");
                                AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                                var entityForFile = new ErrorList
                                {
                                    エラー内容 = errorMsg,
                                    行番号 = lineNumber
                                };
                                ukeireErrorList.Add(entityForFile);
                                エラー件数++;
                                continue; // 以降のチェックはスキップ
                            }
                        }
                        /*
                         * TODO:ファイル設計書がないため再確認必要
                         */
                        // 14.6 各変数にファイルの各項目を設定
                        string 組合等コードF = fields[0].Trim('"');
                        string 年産F = fields[1].Trim('"');
                        string 共済目的コード = fields[2].Trim('"');
                        string 組合員等コード = fields[3].Trim('"');
                        string 類区分 = fields[4].Trim('"');
                        string 引受区分 = fields[5].Trim('"');
                        string 全相殺基準単収 = fields[6].Trim('"');

                        if (!kumiaitoCd.Equals(組合等コードF))
                        {
                            errorMsg = MessageUtil.Get("ME10085", "組合等コード");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }
                        if (!nensan.Equals(年産F))
                        {
                            errorMsg = MessageUtil.Get("ME90015", "年産");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }
                        if (0 == GetKyosaiMokutekiCount(db, 共済目的コード))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "共済目的コード");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }

                        if (0 == GetRuiKbnCount(db, 類区分))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "類区分");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }

                        if (0 == GetHikiukeKbnCount(db, 共済目的コード, 引受区分))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "引受区分");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }

                        if (0 == GetNogyoshaCount(db, 組合等コードF, 組合員等コード))
                        {
                            errorMsg = MessageUtil.Get("ME10016", "農業者情報管理システム", 組合員等コード);
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }

                        //重複チェック
                        if (0 == GetTairyoOkCount(db, 組合等コードF, short.Parse(年産F), 共済目的コード, 組合員等コード, 類区分, 引受区分))
                        {
                            errorMsg = MessageUtil.Get("ME90018", "取込ファイル内のデータ");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue; // 以降のチェックはスキップ
                        }


                        if (0 == GetKojinSetting(db, 組合等コードF, 年産F, 共済目的コード, 組合員等コード))
                        {
                            errorMsg = MessageUtil.Get("ME10016", "個人設定類テーブル", "");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue;
                        }

                        int kojinsetRuiResult = GetKojinSettingRui(db, 組合等コードF, 年産F, 共済目的コード, 組合員等コード, 類区分, 引受区分);

                        if (0 == kojinsetRuiResult)
                        {
                            errorMsg = MessageUtil.Get("ME10016", "個人設定類テーブル", "");
                            AddError(ukeirerirekiId, errListSeq++, lineNumber, errorContent, sysDate, db);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            エラー件数++;
                            continue;
                        }

                        // 15. 登録処理
                        // ※ここで受入エラーが無い（errorContentが ""）ため、OK件数をカウント
                        OK件数++;
                        var entity = new T19050大量データ受入組合員等類別設定ok
                        {
                            組合等コード = 組合等コードF,
                            年産 = short.Parse(年産F),
                            共済目的コード = 共済目的コード,
                            組合員等コード = 組合員等コード,
                            類区分 = 類区分,
                            引受区分 = 引受区分,
                            全相殺基準単収 = decimal.Parse(全相殺基準単収),
                            登録日時 = sysDate,
                            登録ユーザid = BATCH_USER_NAME
                        };
                        db.T19050大量データ受入組合員等類別設定oks.Add(entity);
                        ukeireOkList.Add(entity);
                    } // end for

                    // --- 9. タスク終了 ---
                    /*
                     * TODO:ファイル格納先と処理のロジックが決まってないため、
                     * 必要に応じて修正
                     */
                    if (1 <= エラー件数)
                    {
                        // 10.1.1 一時領域にデータ一時出力フォルダとファイルを作成する
                        // ※設定ファイルから一時領域のルートパスを取得（例：Constants.FILE_TEMP_FOLDER_PATH）
                        //     フォルダ名は "[バッチID]_[yyyyMMddHHmmss]" の形式とする
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string tempRootFolder = ConfigUtil.Get(Constants.FILE_TEMP_FOLDER_PATH); // 例："C:\\SYN\\112041Temp"
                        string tempFolderPath = Path.Combine(tempRootFolder, $"{bid}_{timestamp}");
                        tempErrorFolderPath = tempFolderPath;

                        if (!Directory.Exists(tempFolderPath))
                        {
                            Directory.CreateDirectory(tempFolderPath);
                        }

                        // エラーリストファイル名の生成
                        // 形式: [取込前ファイル名]-ERR-[取込履歴ID].csv
                        string errorFileName = $"{preNm}-OK-{ukeirerirekiId}.csv";
                        string tempFilePath = Path.Combine(tempFolderPath, errorFileName);

                        // 10.1.2 エラーリストの内容をCSV形式で作成
                        // ヘッダ： "エラー対象行数","エラー内容"
                        // 各データ行もすべての項目をダブルクォーテーションで囲む
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("\"エラー対象行数\",\"エラー内容\"");
                        foreach (var error in ukeireErrorList)
                        {
                            // 行番号は数値→文字列変換、エラー内容はそのまま
                            sb.AppendLine($"\"{error.行番号}\",\"{error.エラー内容}\"");
                        }
                        // ファイル出力（Shift_JIS、改行はCRLF）
                        File.WriteAllText(tempFilePath, sb.ToString(), Encoding.GetEncoding("Shift_JIS"));

                        // 10.1.3 ファイルのハッシュ値を取得して変数に設定する
                        byte[] csvBytes = File.ReadAllBytes(tempFilePath);
                        errorListHashValue = CryptoUtil.GetSHA256Hex(csvBytes);

                        // 10.1.4 ファイルをZIP化して暗号化し、出力先に保存する
                        // 暗号化は、CryptoUtil.Encrypt(byte[] targetData, string fileName) を利用
                        // ここでは、CSVファイルのバイト列を暗号化した上でZIPアーカイブに格納する
                        byte[] encryptedData = CryptoUtil.Encrypt(csvBytes, errorFileName);

                        // 出力先フォルダは例："C:\NSK\112041Real\[yyyyMMddHHmmss]\"
                        string outputRootFolder = @"C:\NSK\112041Real";
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
                        errorListName = errorFileName;

                        // 10.1.5 一時領域に作成したフォルダとファイルを削除する
                        Directory.Delete(tempFolderPath, true);
                        logger.Error(MessageUtil.Get("ME10026", "取込対象"));
                    }
                    /*
                     * TODO:ファイル格納先と処理のロジックが決まってないため、
                     * 必要に応じて修正
                     */
                    if (1 <= OK件数)
                    {
                        // 10.1.1 一時領域にデータ一時出力フォルダとファイルを作成する
                        // ※設定ファイルから一時領域のルートパスを取得（例：Constants.FILE_TEMP_FOLDER_PATH）
                        //     フォルダ名は "[バッチID]_[yyyyMMddHHmmss]" の形式とする
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string tempRootFolder = ConfigUtil.Get(Constants.FILE_TEMP_FOLDER_PATH); // 例："C:\\SYN\\112041Temp"
                        string tempFolderPath = Path.Combine(tempRootFolder, $"{bid}_{timestamp}");
                        tempOkFolderPath = tempFolderPath;

                        if (!Directory.Exists(tempFolderPath))
                        {
                            Directory.CreateDirectory(tempFolderPath);
                        }

                        // エラーリストファイル名の生成
                        // 形式: [取込前ファイル名]-ERR-[取込履歴ID].csv
                        string okFileName = $"{preNm}-ERR-{ukeirerirekiId}.csv";
                        string tempFilePath = Path.Combine(tempFolderPath, okFileName);

                        // 10.1.2 エラーリストの内容をCSV形式で作成
                        // ヘッダ： "エラー対象行数","エラー内容"
                        // 各データ行もすべての項目をダブルクォーテーションで囲む
                        StringBuilder sb = new StringBuilder();
                        // ヘッダ行（各項目をダブルクォーテーションで囲む、カンマ区切り）
                        sb.AppendLine("\"処理区分\",\"組合等コード\",\"年産\",\"共済目的コード\",\"組合員等コード\",\"類区分\",\"引受区分\",\"全相殺基準単収\"");

                        foreach (var ok in ukeireOkList) 
                        {
                            // 各項目の値を取得し、ダブルクォーテーションで囲んで連結（全項目を出力）
                            sb.AppendLine(
                                $"\"{ok.処理区分}\"," +
                                $"\"{ok.組合等コード}\"," +
                                $"\"{ok.年産}\"," +
                                $"\"{ok.共済目的コード}\"," +
                                $"\"{ok.組合員等コード}\"," +
                                $"\"{ok.類区分}\"," +
                                $"\"{ok.引受区分}\"," +
                                $"\"{ok.全相殺基準単収}\","
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

                        // 出力先フォルダは例："C:\NSK\112041Real\[yyyyMMddHHmmss]\"
                        string outputRootFolder = @"C:\NSK\112041Real";
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
                        ukeireSts = "成功";
                        shoriSts = "03";

                        okListName = okFileName;
                        okListPath = destinationZipFilePath;
                        // 10.1.5 一時領域に作成したフォルダとファイルを削除する
                        Directory.Delete(tempFolderPath, true);
                        logger.Error(MessageUtil.Get("ME10026", "取込対象"));
                    }
                    Console.WriteLine($"処理終了。受入OK件数：{OK件数} 件、エラー件数：{エラー件数} 件");
                    totalCount = OK件数 + エラー件数;

                    int updateResult = UpdateUkeireRirekiFinally(db, ukeireSts, totalCount, エラー件数, errorListName, errorListPath
                        , errorListHashValue, OK件数, okListName, okListPath, okListHashValue, sysDate, ukeirerirekiId, preNm);
                    db.SaveChanges();
                    // トランザクションコミット
                    transaction.Commit();

                forcedShutdown:;
                }
                catch (Exception e)
                {
                    logger.Error(e.StackTrace);
                    Console.Error.WriteLine(MessageUtil.Get("MF00001"));
                    logger.Error(MessageUtil.Get("MF00001"));
                    logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                    transaction.Rollback();
                    // 処理結果（正常：0、エラー：1）
                    Environment.ExitCode = Constants.BATCH_EXECUT_FAILED;
                    return;
                }
                finally 
                {
                    if (!"03".Equals(batchSts))
                    {
                        batchSts = "99";
                    }
                    string refMessage = string.Empty;
                    int yoyakuResult = BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), batchSts, errorMsg, BATCH_USER_NAME, ref refMessage);
                    if (0 == yoyakuResult)
                    {
                        // 更新に失敗した場合
                        logger.Error(refMessage);
                        logger.Error(string.Format(Constants.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, batchSts, refMessage));
                        result = Constants.BATCH_EXECUT_FAILED;
                    }
                    else
                    {
                        // 更新に成功した場合
                        logger.Info(string.Format(Constants.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, bid, batchSts));
                        result = Constants.BATCH_EXECUT_SUCCESS;
                    }
                    // 処理時間
                    stopwatch.Stop();
                    var excutingTime = CoreConst.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + CoreConst.LOG_TIMER_END_MESSAGE;
                    logger.Info(excutingTime);
                    Directory.Delete(tempOkFolderPath, true);
                    Directory.Delete(tempErrorFolderPath, true);
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
            string ukeirerirekiId,
            string fileHash)
        {
            logger.Info("バッチ条件テーブルから、指定されたバッチ条件idおよび条件名称に一致するデータを取得します。");

            // IN句に相当する条件を配列で用意
            var conditionNames = new[] { nensan, filePath, ukeirerirekiId, fileHash };

            // LINQクエリで条件に合致するレコードを取得
            var results = db.Set<T01050バッチ条件>()
                            .Where(b => b.バッチ条件id == joukenId &&
                                        conditionNames.Contains(b.条件名称))
                            .ToList();

            return results;
        }

        /// <summary>
        /// 大量データ受入履歴テーブルから受入履歴IDに一致するレコード取得
        /// </summary>
        /// <param name="db">NskDB</param>
        /// <param name="ukeirerirekiId">受入履歴id</param>
        /// <returns>大量データ受入履歴</returns>
        private static T01060大量データ受入履歴 GetUkeireRireki(NskAppContext db, string ukeirerirekiId)
        {
            logger.Info("大量データ受入履歴テーブルから、受入履歴idに一致するレコードを取得します。");
            return db.Set<T01060大量データ受入履歴>()
                     .FirstOrDefault(x => x.受入履歴id == ukeirerirekiId);
        }

        /// <summary>
        /// 大量データ受入履歴の更新（開始日時、ステータス等）
        /// </summary>
        /// <param name="db">NskDB</param>
        /// <param name="ukeirerirekiId">受入履歴id</param>
        /// <param name="ukeireSts">受入ステータス</param>
        /// <param name="systemDate">システム日時</param>
        /// <returns>return 0 エラ、1 成功</returns>
        private static int UpdateUkeireRireki(NskAppContext db, string ukeirerirekiId, string ukeireSts, DateTime systemDate)
        {
            logger.Info("大量データ受入履歴テーブルの受入履歴idに一致するレコードを更新します。");
            var entity = db.Set<T01060大量データ受入履歴>()
                           .FirstOrDefault(x => x.受入履歴id == ukeirerirekiId);
            if (entity == null)
            {
                return 0;
            }

            entity.ステータス = ukeireSts;
            entity.開始日時 = systemDate;
            entity.更新日時 = systemDate;
            entity.更新ユーザid = "NSK_112170B";

            return db.SaveChanges();
        }

        /// <summary>
        ///  大量データ受入履歴の最終更新（各種件数、エラー情報、終了日時等）
        /// </summary>
        /// <param name="db">NskDB</param>
        /// <param name="ukeireSts">受入履歴id</param>
        /// <param name="taishokensuu">対象件数</param>
        /// <param name="errorCount">エラー件数</param>
        /// <param name="errorListName">エラーリスト名</param>
        /// <param name="errorListPath">エラーリストパス</param>
        /// <param name="errorListHashValue">エラーリストハッシュ値</param>
        /// <param name="okKensuu">ok件数</param>
        /// <param name="okListName">okリスト名</param>
        /// <param name="okListPath">okリストパス</param>
        /// <param name="okListHashValue">okリストハッシュ値</param>
        /// <param name="systemDate">システム日時</param>
        /// <param name="ukeireRirekiId">受入履歴id</param>
        /// <returns>return 0 エラ、1 成功</returns>
        private static int UpdateUkeireRirekiFinally(
            NskAppContext db,
            string ukeireSts,
            int taishokensuu,
            int errorCount,
            string errorListName,
            string errorListPath,
            string errorListHashValue,
            int okKensuu,
            string okListName,
            string okListPath,
            string okListHashValue,
            DateTime sysDate,
            string ukeireRirekiId,
            string torikomiFileNm)
        {
            logger.Info($"Updating t_01060_大量データ受入履歴: 受入履歴id={ukeireRirekiId}, ステータス={ukeireSts}, 対象件数={taishokensuu}, エラー件数={errorCount}, OK件数={okKensuu}");
            var entity = db.Set<T01060大量データ受入履歴>()
                           .FirstOrDefault(x => x.受入履歴id == ukeireRirekiId);
            if (entity == null)
            {
                return 0;
            }

            entity.ステータス = ukeireSts;
            entity.取込区分 = "1";
            entity.対象件数 = taishokensuu;
            entity.エラー件数 = errorCount;
            entity.エラーリスト名 = errorListName;
            entity.エラーリストパス = errorListPath;
            entity.エラーリストハッシュ値 = errorListHashValue;
            entity.OK件数 = okKensuu;
            entity.OKリスト名 = okListName;
            entity.OKリストパス = okListPath;
            entity.OKリストハッシュ値 = okListHashValue;
            entity.取込ファイル_変更前ファイル名 = torikomiFileNm;
            entity.終了日時 = sysDate;
            entity.更新日時 = sysDate;
            entity.更新ユーザid = "NSK_112170B";

            return db.SaveChanges();
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
        /// m_00010_共済目的名称テーブルから件数取得
        /// </summary>
        /// <param name="db">NskDB</param>
        /// <param name="kyosaiMokutekiCd">共済目的コード</param>
        /// <returns>共済目的件数</returns>
        private static int GetKyosaiMokutekiCount(NskAppContext db, string kyosaiMokutekiCd)
        {
            logger.Info($"共済目的コード {kyosaiMokutekiCd} の件数を取得します。");
            return db.Set<M00010共済目的名称>()
                     .Count(x => x.共済目的コード == kyosaiMokutekiCd);
        }

        /// <summary>
        /// m_00010_類名称テーブルから件数取得
        /// </summary>
        /// <param name="db">NskDB</param>
        /// <param name="ruiKbn">類区分</param>
        /// <returns>類区分件数</returns>
        private static int GetRuiKbnCount(NskAppContext db, string ruiKbn)
        {
            logger.Info($"類区分 {ruiKbn} の件数を取得します。");
            return db.Set<M00020類名称>()
                     .Count(x => x.類区分 == ruiKbn);
        }

        /// <summary>
        /// m_10090_引受区分名称テーブルから件数取得
        /// </summary>
        /// <param name="db">NskDB</param>
        /// <param name="kyosaimokutekiCd">共済目的コード</param>
        /// <param name="hikiukeKbn">引受区分</param>
        /// <returns>引受区分名数</returns>
        private static int GetHikiukeKbnCount(NskAppContext db, string kyosaimokutekiCd, string hikiukeKbn)
        {
            logger.Info($"共済目的コード {kyosaimokutekiCd}, 引受区分 {hikiukeKbn}  の件数を取得します。");
            return db.Set<M10090引受区分名称>()
                     .Count(x => x.共済目的コード == hikiukeKbn &&
                                 x.引受区分 == hikiukeKbn);
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
        /// 個人設定テーブルから件数取得
        /// </summary>
        /// <param name="db">NskDB</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="nensan">年産</param>
        /// <param name="kyosaimokutekiCd">共済目的コード</param>
        /// <param name="kumiaiintoCd">組合員等コード</param>
        /// <returns>個人設定件数</returns>
        private static int GetKojinSetting(
            NskAppContext db
            , string kumiaitoCd
            , string nensan
            , string kyosaimokutekiCd
            , string kumiaiintoCd)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan},"
                + $" 共済目的コード {kyosaimokutekiCd}, 組合員等コード {kumiaiintoCd} の件数を取得します。");
            return db.Set<T11010個人設定>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd);
        }

        /// <summary>
        /// 個人設定類>テーブルから件数取得
        /// </summary>
        /// <param name="db">NskDB</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="nensan">年産</param>
        /// <param name="kyosaimokutekiCd">共済目的コード</param>
        /// <param name="kumiaiintoCd">組合員等コード</param>
        /// <param name="ruiKbn">類区分</param>
        /// <param name="hikiukeKbn">引受区分</param>
        /// <returns>個人設定類>件数</returns>
        private static int GetKojinSettingRui(
            NskAppContext db
            , string kumiaitoCd
            , string nensan
            , string kyosaimokutekiCd
            , string kumiaiintoCd
            , string ruiKbn
            , string hikiukeKbn)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan},"
                + $" 共済目的コード {kyosaimokutekiCd}, 組合員等コード {kumiaiintoCd},"
                + $" 類区分 {ruiKbn}, 引受区分 {hikiukeKbn} の件数を取得します。");
            return db.Set<T11030個人設定類>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd &&
                                 x.類区分 == ruiKbn &&
                                 x.引受区分 == hikiukeKbn);
        }

        /// <summary>
        /// 大量データ受入エラーリストにエラー追加
        /// </summary>
        /// <param name="ukeireRirekiId">履歴id</param>
        /// <param name="errListSeq">seq</param>
        /// <param name="行番号">行番号</param>
        /// <param name="dataError">エラー内容</param>
        /// <param name="sysDate">登録日時</param>
        /// <param name="db">NskDB</param>
        private static void AddError(
            string ukeireRirekiId
            ,int errListSeq
            ,int 行番号
            ,string dataError
            ,DateTime sysDate
            ,NskAppContext db )
        {
            var dataErrorList = new T01080大量データ受入エラーリスト
            {
                処理区分 = "2",
                履歴id = long.Parse(ukeireRirekiId),
                枝番 = errListSeq,
                行番号 = 行番号.ToString(),
                エラー内容 = dataError,
                登録日時 = sysDate,
                登録ユーザid = BATCH_USER_NAME,
            };
            // DbSet に追加
            db.T01080大量データ受入エラーリストs.Add(dataErrorList);
        }

        /// <summary>
        /// 数値かどうかをチェック（整数としてパースできるかどうか）
        /// </summary>
        private static bool IsNumeric(string input)
        {
            return long.TryParse(input, out _);
        }

        /// <summary>
        /// T19050大量データ受入組合員等類別設定ok取得
        /// </summary>
        private static int GetTairyoOkCount(
            NskAppContext db
            , string kumiaitoCd
            , short? nensan
            , string kyosaimokutekiCd
            , string kumiaiintoCd
            , string ruiKbn
            , string hikiukeKbn)
        {
            return db.Set<T19050大量データ受入組合員等類別設定ok>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == nensan &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd &&
                                 x.類区分 == ruiKbn &&
                                 x.引受区分 == hikiukeKbn);
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
        /// エラーリスト格納クラス
        /// </summary>
        public class ErrorList
        {
            public string エラー内容 { get; set; }
            public Decimal 行番号 { get; set; }
        }
    }
}
