using NSK_B112172.Common;
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

namespace NSK_B112172
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
        private static string BATCH_USER_NAME = "NSK_112172B";
        // エラー出力先フォルダ（ZIPファイル格納フォルダ）
        private const string DestinationFolder = @"C:\NSK112170B\ErrorFiles";
        // 年産条件（例：外部設定などで取得する値）
        private const string 年産_JOKEN = "2024";

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
            var ukeireRirekiId = string.Empty;
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
            var tempErrorFolderPath = string.Empty;
            var preNm = string.Empty;
            bool forcedShutdownFlg = false;
            var 支所町村コード = string.Empty;

            var batchSts = "";
            var torikomiRirekiId = "";

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

            logger.Debug("dbConnectionInfo.ConnectionString : " + dbConnectionInfo.ConnectionString);
            logger.Debug("dbConnectionInfo.DefaultSchema : " + dbConnectionInfo.DefaultSchema);

            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema, ConfigUtil.GetInt(Constants.CONFIG_COMMAND_TIMEOUT)))
            {
                var transaction = db.Database.BeginTransaction();
                try
                {
                    // バッチ条件情報取得
                    List<T01050バッチ条件> joukenList = GetBatchJoukenList(db, bid, Constants.JOUKEN_NENSAN, Constants.JOUKEN_FILE_PATH, Constants.JOUKEN_UKEIRERIREKI_ID);
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
                        if (Constants.JOUKEN_UKEIRERIREKI_ID.Equals(joukenList[i].条件名称))
                        {
                            ukeireRirekiId = joukenList[i].条件値;
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
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    if (string.IsNullOrEmpty(ukeireRirekiId))
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    if (string.IsNullOrEmpty(filePath))
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    if (string.IsNullOrEmpty(fileHash))
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        logger.Error(errorMsg);
                        goto forcedShutdown;
                    }

                    T01060大量データ受入履歴 ukeireRirekiResult = GetUkeireRireki(db, ukeireRirekiId);
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

                    UpdateUkeireRireki(db, ukeireRirekiId, ukeireSts, sysDate);

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

                    if (!kumiaitoCd.Equals(kumiaitoCdUkeire))
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

                    // --- 2.1～2.5 範囲レコードの存在・位置チェック ---
                    List<int> rangeRecordLineNumbers = new List<int>();
                    for (int i = 0; i < allLines.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(allLines[i]))
                            continue;

                        // カンマ区切りに分割し、先頭項目が"範囲"ならば記録
                        string[] fields = allLines[i].Split(',');
                        if (fields.Length > 0 && fields[0].Trim().Trim('\uFEFF').Trim('"') == "範囲")
                        {
                            rangeRecordLineNumbers.Add(i);
                        }
                    }
                    // 2.3 範囲レコードが存在しない場合
                    if (rangeRecordLineNumbers.Count == 0)
                    {
                        logger.Error("ERROR: 範囲レコードが存在しません。");
                        errorMsg = "ERROR: 範囲レコードが存在しません。";
                        goto forcedShutdown;
                    }
                    // 2.4 複数の範囲レコードが存在する場合
                    if (rangeRecordLineNumbers.Count > 1)
                    {
                        logger.Error(MessageUtil.Get("ME10058", "範囲レコード"));
                        errorMsg = MessageUtil.Get("ME10058", "範囲レコード");
                        goto forcedShutdown;
                    }
                    // 2.5 範囲レコードが先頭行でない場合
                    if (rangeRecordLineNumbers[0] != 0)
                    {
                        logger.Error("ERROR: 範囲レコードが先頭行ではありません。");
                        errorMsg = "ERROR: 範囲レコードが先頭行ではありません。";
                        goto forcedShutdown;
                    }

                    // --- 3. 範囲レコード（1行目）の処理 ---
                    string[] rangeFields = allLines[0].Split(',');
                    if (rangeFields.Length < 9)
                    {
                        logger.Error("ERROR: 範囲レコードの項目数が不足しています。");
                        errorMsg = "ERROR: 範囲レコードの項目数が不足しています。";
                        goto forcedShutdown;
                    }
                    string valiResult = ValidateRangeRecord(rangeFields, 1);

                    if (!string.IsNullOrEmpty(valiResult))
                    {
                        // ValidateRangeRecord 内でエラーログ出力済み
                        errorMsg = valiResult;
                        goto forcedShutdown;
                    }
                    // 各項目を変数に格納
                    string 範囲 = rangeFields[0].Trim().Trim('"');                // 固定："範囲"
                    string 年産範囲 = rangeFields[1].Trim().Trim('"');
                    string 共済目的コード範囲 = rangeFields[2].Trim().Trim('"');
                    string 抽出区分 = rangeFields[3].Trim().Trim('"');
                    string 範囲パラメータ1 = rangeFields[4].Trim().Trim('"');
                    string 範囲パラメータ2 = rangeFields[5].Trim().Trim('"');
                    string 範囲パラメータ3 = rangeFields[6].Trim().Trim('"');
                    string 範囲日付 = rangeFields[7].Trim().Trim('"');
                    string GISデータ出力のタイプ = rangeFields[8].Trim().Trim('"');

                    // 3.3 年産チェック
                    if (nensan != 年産範囲)
                    {
                        logger.Error(MessageUtil.Get("ME90015", "年産"));
                        errorMsg = MessageUtil.Get("ME90015", "年産");
                        goto forcedShutdown;
                    }
                    // 3.4 GISデータ出力のタイプチェック（"2"以外はNG）
                    if (GISデータ出力のタイプ != "2")
                    {
                        logger.Error(MessageUtil.Get("ME90015", "GISデータ出力のタイプ"));
                        errorMsg = MessageUtil.Get("ME90015", "GISデータ出力のタイプ");
                        goto forcedShutdown;
                    }

                    // --- 4. 項目名レコード（2行目）のチェック ---
                    if (allLines.Length < 2 || string.IsNullOrWhiteSpace(allLines[1]))
                    {
                        logger.Error("ERROR: 項目名レコードが存在しません。");
                        errorMsg = "ERROR: 項目名レコードが存在しません。";
                        goto forcedShutdown;
                    }
                    string[] headerFields = allLines[1].Split(',');
                    if (headerFields.Length != 31)
                    {
                        logger.Error(MessageUtil.Get("ME10066"));
                        errorMsg = MessageUtil.Get("ME10066");
                        goto forcedShutdown;
                    }

                    // --- 5. データスタートレコード（3行目）のチェック ---
                    if (allLines.Length < 3 || string.IsNullOrWhiteSpace(allLines[2]))
                    {
                        logger.Error(MessageUtil.Get("ME10066"));
                        errorMsg = MessageUtil.Get("ME10066");
                        goto forcedShutdown;
                    }
                    string[] startFields = allLines[2].Split(',');
                    // ※仕様書では1項目目は "# DATA-START" であること、2項目目が日付であることをチェック
                    if (startFields.Length < 2 || startFields[0].Trim().Trim('"') != "# DATA-START …" ||
                        !DateTime.TryParse(startFields[1].Trim(), out DateTime dataStartDate))
                    {
                        logger.Error("ERROR: データスタートレコードの形式が不正です。");
                        errorMsg = "ERROR: データスタートレコードの形式が不正です。";
                        goto forcedShutdown;
                    }
                    if (allLines.Length < 4)
                    {
                        errorMsg = MessageUtil.Get("ME90015", "データ行");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    // --- 6. 4行目以降のデータ行の処理 ---

                    int errListSeq = 0;

                    // 行番号は実際のファイル上の番号（例：1行目が「範囲レコード」なので、データ行は4行目から＝インデックス3）
                    for (int i = 3; i < allLines.Length; i++)
                    {
                        int 行番号 = OK件数 + エラー件数 + 4;  // ファイル上の行番号（1:範囲、2:項目名、3:データスタート）
                        string line = allLines[i];
                        string dataError = "";
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            // 空行の場合はデータの終端とする（またはループを抜ける）
                            break;
                        }

                        string[] dataFields = line.Split(',');
                        // 最終レコード「データエンド」のチェック：1項目目が "終了" ならば終了とみなす
                        if (dataFields[0].Trim() == "終了")
                        {
                            break;
                        }

                        // 6.3 必須チェック＋6.5 桁数チェック＋6.6 形式チェックを実施
                        // ※ 各項目について、必須項目は空欄エラー、必須でない項目は値がある場合のみチェックする
                        dataError = ValidateDataRow(dataFields, 行番号);
                        if (!string.IsNullOrEmpty(dataError))
                        {
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }
                        // 7.3 各項目の変数設定（必須項目はもちろん、その他項目も必要に応じて取り出す）
                        string 共済目的コード = dataFields[0].Trim().Trim('"');
                        string 組合員等コード = dataFields[1].Trim().Trim('"');
                        string 耕地番号 = dataFields[2].Trim().Trim('"');
                        string 分筆番号 = dataFields[3].Trim().Trim('"');
                        string 類区分 = (dataFields.Length > 4) ? dataFields[4].Trim().Trim('"') : "";
                        string 地名地番 = (dataFields.Length > 5) ? dataFields[5].Trim().Trim('"') : "";
                        string 耕地面積 = (dataFields.Length > 6) ? dataFields[6].Trim().Trim('"') : "";
                        string 引受面積 = (dataFields.Length > 7) ? dataFields[7].Trim().Trim('"') : "";
                        string 転作等面積 = (dataFields.Length > 8) ? dataFields[8].Trim().Trim('"') : "";
                        string 受委託区分 = (dataFields.Length > 9) ? dataFields[9].Trim().Trim('"') : "";
                        string 備考 = (dataFields.Length > 10) ? dataFields[10].Trim().Trim('"') : "";
                        string 田畑区分 = (dataFields.Length > 11) ? dataFields[11].Trim().Trim('"') : "";
                        string 区分コード = (dataFields.Length > 12) ? dataFields[12].Trim().Trim('"') : "";
                        string 種類コード = (dataFields.Length > 13) ? dataFields[13].Trim().Trim('"') : "";
                        string 品種コード = (dataFields.Length > 14) ? dataFields[14].Trim().Trim('"') : "";
                        string 収量等級コード = (dataFields.Length > 15) ? dataFields[15].Trim().Trim('"') : "";
                        string 参酌コード = (dataFields.Length > 16) ? dataFields[16].Trim().Trim('"') : "";
                        string 基準単収 = (dataFields.Length > 17) ? dataFields[17].Trim().Trim('"') : "";
                        string 基準収穫量 = (dataFields.Length > 18) ? dataFields[18].Trim().Trim('"') : "";
                        string 修正日付 = (dataFields.Length > 19) ? dataFields[19].Trim().Trim('"') : "";
                        string 計算日付 = (dataFields.Length > 20) ? dataFields[20].Trim().Trim('"') : "";
                        string 年産 = (dataFields.Length > 21) ? dataFields[21].Trim().Trim('"') : "";
                        string 実量基準単収 = (dataFields.Length > 22) ? dataFields[22].Trim().Trim('"') : "";
                        string RS区分 = (dataFields.Length > 23) ? dataFields[23].Trim().Trim('"') : "";
                        string GISデータ = (dataFields.Length > 24) ? dataFields[24].Trim().Trim('"') : "";
                        string 統計市町村コード = (dataFields.Length > 25) ? dataFields[25].Trim().Trim('"') : "";
                        string 統計地域コード = (dataFields.Length > 26) ? dataFields[26].Trim().Trim('"') : "";
                        string 統計単収 = (dataFields.Length > 27) ? dataFields[27].Trim().Trim('"') : "";
                        string 麦用途区分 = (dataFields.Length > 28) ? dataFields[28].Trim().Trim('"') : "";
                        string 産地銘柄コード = (dataFields.Length > 29) ? dataFields[29].Trim().Trim('"') : "";
                        string 受委託者コード = (dataFields.Length > 30) ? dataFields[30].Trim().Trim('"') : "";

                        // 各変数を初期値 null で宣言
                        string 局都道府県コード = null;  // 文字数: 4
                        string 市区町村コード = null;  // 文字数: 3
                        string 大字コード = null;  // 文字数: 8
                        string 小字コード = null;  // 文字数: 4
                        string 地番 = null;  // 文字数: 16
                        string 枝番 = null;  // 文字数: 14
                        string 子番 = null;  // 文字数: 10
                        string 孫番 = null;  // 文字数: 10

                        string 大地区コード = string.Empty;
                        string 小地区コード = string.Empty;

                        // GISデータが null または空文字でない場合のみ substring する
                        if (!string.IsNullOrEmpty(GISデータ))
                        {
                            // ※ 各 substring の開始位置と文字数は以下の通りです。
                            // 局都道府県コード：開始位置 0,  文字数 4
                            // 市区町村コード  ：開始位置 4,  文字数 3
                            // 大字コード      ：開始位置 7,  文字数 8
                            // 小字コード      ：開始位置 15, 文字数 4
                            // 地番            ：開始位置 19, 文字数 16
                            // 枝番            ：開始位置 35, 文字数 14
                            // 子番            ：開始位置 49, 文字数 10
                            // 孫番            ：開始位置 59, 文字数 10
                            //
                            // ※ GISデータの合計文字数は 4+3+8+4+16+14+10+10 = 69 文字
                            局都道府県コード = GISデータ.Substring(0, 4);
                            市区町村コード = GISデータ.Substring(4, 3);
                            大字コード = GISデータ.Substring(7, 8);
                            小字コード = GISデータ.Substring(15, 4);
                            地番 = GISデータ.Substring(19, 16);
                            枝番 = GISデータ.Substring(35, 14);
                            子番 = GISデータ.Substring(49, 10);
                            孫番 = GISデータ.Substring(59, 10);
                        }


                        if (0 == GetKyosaiMokutekiCount(db, 共済目的コード))
                        {
                            dataError = MessageUtil.Get("ME10005", "共済目的コード");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }

                        if (0 == GetRuiKbnCount(db, 類区分))
                        {
                            dataError = MessageUtil.Get("ME10005", "類区分");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }

                        if (0 == GetTabatakeKbnCount(db, 田畑区分))
                        {
                            dataError = MessageUtil.Get("ME10005", "類区分");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }

                        if (0 == GetKubunCount(db, kumiaitoCd, nensan, 共済目的コード, 区分コード))
                        {
                            dataError = MessageUtil.Get("ME10005", "区分コード");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }

                        if (0 == GetShuruiCount(db, 共済目的コード, 種類コード))
                        {
                            dataError = MessageUtil.Get("ME10005", "種類コード");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }

                        if (0 == GetYoutoKbnCount(db, 共済目的コード, 麦用途区分))
                        {
                            dataError = MessageUtil.Get("ME10005", "麦用途区分");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }
                        if (0 == GetHinshuKeisuuCount(db, kumiaitoCd, nensan, 共済目的コード, 品種コード))
                        {
                            dataError = MessageUtil.Get("ME10005", "品種コード");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }
                        if (0 == GetSanchiBetuMeigaraCount(db, kumiaitoCd, nensan, 共済目的コード, 産地銘柄コード))
                        {
                            dataError = MessageUtil.Get("ME10005", "産地銘柄コード");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }

                        if (0 == GetShuryoToukyuCount(db, kumiaitoCd, nensan, 共済目的コード, 類区分, shishoCd, 収量等級コード))
                        {
                            dataError = MessageUtil.Get("ME10005", "収量等級コード");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }

                        if (0 == GetSansyakuKeisuuCount(db, kumiaitoCd, nensan, 共済目的コード, 参酌コード))
                        {
                            dataError = MessageUtil.Get("ME10005", "参酌コード");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }

                        if (0 == GetToukeitaniChiikiCount(db, kumiaitoCd, nensan, 共済目的コード, 統計地域コード))
                        {
                            dataError = MessageUtil.Get("ME10005", "統計地域コード");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }
                        // 農業者チェック情報
                        if (0 == GetNogyoshaCount(db, kumiaitoCd, 組合員等コード))
                        {
                            dataError = MessageUtil.Get("ME10016", "農業者情報管理システム", 組合員等コード);
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }
                        VNogyosha tnogyosya = GetNogyoshaByCodes(db, kumiaitoCd, 組合員等コード);
                        大地区コード = tnogyosya.DaichikuCd;
                        小地区コード = tnogyosya.ShochikuCd;
                        支所町村コード = tnogyosya.ShichosonCd;
                        if (0 < GetJoinApplicationOkCount(db, kumiaitoCd, nensan, 共済目的コード, 組合員等コード, 耕地番号, 分筆番号))
                        {
                            var errJoinApp = GetJoinApplicationError(db, kumiaitoCd, nensan, 共済目的コード, 組合員等コード, 耕地番号, 分筆番号);
                            dataError = MessageUtil.Get("ME10010"
                                , "取込ファイル内"
                                , errJoinApp.行番号.ToString()
                                , $"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {共済目的コード}, 組合員等コード {組合員等コード}, 耕地番号 {耕地番号}, 分筆番号 {分筆番号}");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }

                        if (0 == decimal.Parse(引受面積) && 0 == decimal.Parse(転作等面積))
                        {
                            dataError = MessageUtil.Get("ME10011", "引受面積", "転作等面積");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }

                        if (0 < decimal.Parse(引受面積) && 0 < decimal.Parse(転作等面積))
                        {
                            dataError = MessageUtil.Get("ME10013", "引受面積", "転作等面積");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }

                        if (decimal.Parse(引受面積) > decimal.Parse(耕地面積))
                        {
                            dataError = MessageUtil.Get("ME10022", "耕地面積", "引受面積");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }

                        if (decimal.Parse(転作等面積) > decimal.Parse(耕地面積))
                        {
                            dataError = MessageUtil.Get("ME10022", "耕地面積", "転作等面積");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }

                        if (decimal.Parse(引受面積) + decimal.Parse(転作等面積) > decimal.Parse(耕地面積))
                        {
                            dataError = MessageUtil.Get("ME10022", "耕地面積", "転作等面積と引受面積の合計");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }

                        if ("0".Equals(区分コード) && 0 == decimal.Parse(引受面積))
                        {
                            dataError = MessageUtil.Get("ME90015", "区分コード");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }
                        if ("0".Equals(区分コード) && 0 < decimal.Parse(転作等面積))
                        {
                            dataError = MessageUtil.Get("ME90015", "区分コード");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }
                        if ("3".Equals(区分コード) && 0 == decimal.Parse(転作等面積))
                        {
                            dataError = MessageUtil.Get("ME90015", "区分コード");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }
                        if ("3".Equals(区分コード) && 0 < decimal.Parse(引受面積))
                        {
                            dataError = MessageUtil.Get("ME90015", "区分コード");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }
                        List<M00030区分名称> kbnMeishoList = GetKubunMeishouRecords(db, kumiaitoCd, nensan, 共済目的コード, 区分コード);

                        List<M10140種類名称> shuruiMeishoList = GetShuruiMeishouRecords(db, 共済目的コード);
                        bool gotoFlg = false;
                        for (int j = 0; j < kbnMeishoList.Count; j++)
                        {
                            for (int k = 0; k < shuruiMeishoList.Count; k++)
                            {
                                if ("2".Equals(kbnMeishoList[j].エラータイプ) && 種類コード.Equals(shuruiMeishoList[k].共済目的コード))
                                {
                                    dataError = MessageUtil.Get("ME10007", "区分と種類の組合せ");
                                    AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                                    エラー件数++;
                                    gotoFlg = true;
                                    goto ExitLoops;
                                }
                                if ("3".Equals(kbnMeishoList[j].エラータイプ) && !種類コード.Equals(shuruiMeishoList[k].共済目的コード))
                                {
                                    dataError = MessageUtil.Get("ME10007", "区分と種類の組合せ");
                                    AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                                    エラー件数++;
                                    gotoFlg = true;
                                    goto ExitLoops;
                                }
                                if ("0".Equals(区分コード) && "0".Equals(収量等級コード))
                                {
                                    dataError = MessageUtil.Get("ME10008", "収量等級");
                                    AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                                    エラー件数++;
                                    gotoFlg = true;
                                    goto ExitLoops;
                                }
                                if ("1".Equals(kbnMeishoList[j].参酌フラグ) && "0".Equals(参酌コード))
                                {
                                    dataError = MessageUtil.Get("ME10007", "区分と参酌コード の組合せ");
                                    AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                                    エラー件数++;
                                    gotoFlg = true;
                                    goto ExitLoops;
                                }
                                if ("20".Equals(共済目的コード) && 0 < decimal.Parse(引受面積) && !"2".Equals(田畑区分))
                                {
                                    dataError = MessageUtil.Get("ME10007", "田畑区分");
                                    AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                                    エラー件数++;
                                    gotoFlg = true;
                                    goto ExitLoops;
                                }
                            }
                        }
                    ExitLoops:;
                        if (gotoFlg)
                        {
                            continue;
                        }

                        if (string.IsNullOrEmpty(共済目的コード範囲))
                        {
                            dataError = MessageUtil.Get("ME10004", "共済目的コード");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }
                        if (!nensan.Equals(年産範囲))
                        {
                            dataError = MessageUtil.Get("ME10004", "年産");
                            AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                            エラー件数++;
                            continue;
                        }
                        if ("2".Equals(抽出区分))
                        {
                            if (string.IsNullOrEmpty(範囲パラメータ1))
                            {
                                dataError = MessageUtil.Get("ME10004", "組合員等コード");
                                AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                                エラー件数++;
                                continue;
                            }
                            if (string.IsNullOrEmpty(範囲パラメータ2) && int.Parse(範囲パラメータ1) > int.Parse(組合員等コード))
                            {
                                dataError = MessageUtil.Get("ME10004", "組合員等コード");
                                AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                                エラー件数++;
                                continue;
                            }
                            if (string.IsNullOrEmpty(範囲パラメータ2) &&
                                (int.Parse(範囲パラメータ1) > int.Parse(組合員等コード) || int.Parse(組合員等コード) < int.Parse(範囲パラメータ2)))
                            {
                                dataError = MessageUtil.Get("ME10004", "組合員等コード");
                                AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                                エラー件数++;
                                continue;
                            }
                        }
                        if ("3".Equals(抽出区分))
                        {
                            if (string.IsNullOrEmpty(範囲パラメータ1))
                            {
                                dataError = MessageUtil.Get("ME10004", "大地区コード");
                                AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                                エラー件数++;
                                continue;
                            }
                            if (!範囲パラメータ1.Equals(大地区コード))
                            {
                                dataError = MessageUtil.Get("ME10004", "大地区コード");
                                AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                                エラー件数++;
                                continue;
                            }
                            if (string.IsNullOrEmpty(範囲パラメータ2) && int.Parse(範囲パラメータ3) > int.Parse(小地区コード))
                            {
                                dataError = MessageUtil.Get("ME10004", "小地区コード");
                                AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                                エラー件数++;
                                continue;
                            }
                            if (string.IsNullOrEmpty(範囲パラメータ3) && int.Parse(小地区コード) > int.Parse(範囲パラメータ3))
                            {
                                dataError = MessageUtil.Get("ME10004", "市区町村コード");
                                AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                                エラー件数++;
                                continue;
                            }
                        }
                        if ("4".Equals(抽出区分))
                        {
                            if (string.IsNullOrEmpty(範囲パラメータ1))
                            {
                                dataError = MessageUtil.Get("ME10004", "市区町村コード");
                                AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                                エラー件数++;
                                continue;
                            }
                            if (!範囲パラメータ1.Equals(支所町村コード))
                            {
                                dataError = MessageUtil.Get("ME10004", "市区町村コード");
                                AddError(ukeireRirekiId, errListSeq++, 行番号, dataError, sysDate, db);
                                エラー件数++;
                                continue;
                            }
                        }

                        // 受入OKデータ作成
                        var dataOk = new T19070大量データ受入加入申込書ok
                        {
                            受入履歴id = long.Parse(ukeireRirekiId),
                            行番号 = 行番号,
                            範囲 = 範囲,
                            年産範囲 = short.Parse(年産範囲),
                            共済目的コード範囲 = 共済目的コード範囲,
                            抽出区分 = 抽出区分,
                            範囲パラメータ１ = 範囲パラメータ1,
                            範囲パラメータ２ = 範囲パラメータ2,
                            範囲パラメータ３ = 範囲パラメータ3,
                            日付 = DateTime.Parse(範囲日付),
                            gisデータ出力のタイプ = GISデータ出力のタイプ,
                            共済目的コード = 共済目的コード,
                            組合員等コード = 組合員等コード,
                            耕地番号 = 耕地番号,
                            分筆番号 = 分筆番号,
                            類区分 = 類区分,
                            地名地番 = 地名地番,
                            耕地面積 = string.IsNullOrEmpty(耕地面積) ? (decimal?)null : decimal.Parse(耕地面積, CultureInfo.InvariantCulture),
                            引受面積 = string.IsNullOrEmpty(引受面積) ? (decimal?)null : decimal.Parse(引受面積, CultureInfo.InvariantCulture),
                            転作等面積 = string.IsNullOrEmpty(転作等面積) ? (decimal?)null : decimal.Parse(転作等面積, CultureInfo.InvariantCulture),
                            受委託区分 = 受委託区分,
                            備考 = 備考,
                            田畑区分 = 田畑区分,
                            区分コード = 区分コード,
                            種類コード = 種類コード,
                            品種コード = 品種コード,
                            収量等級コード = 収量等級コード,
                            参酌コード = 参酌コード,
                            基準単収 = string.IsNullOrEmpty(基準単収) ? (decimal?)null : decimal.Parse(基準単収, CultureInfo.InvariantCulture),
                            基準収穫量 = string.IsNullOrEmpty(基準収穫量) ? (decimal?)null : decimal.Parse(基準収穫量, CultureInfo.InvariantCulture),
                            修正日付 = string.IsNullOrEmpty(修正日付) ? (DateTime?)null : DateTime.ParseExact(修正日付, "yyyy/MM/dd", CultureInfo.InvariantCulture),
                            計算日付 = string.IsNullOrEmpty(計算日付) ? (DateTime?)null : DateTime.ParseExact(計算日付, "yyyy/MM/dd", CultureInfo.InvariantCulture),
                            年産 = short.Parse(年産),
                            実量基準単収 = string.IsNullOrEmpty(実量基準単収) ? (decimal?)null : decimal.Parse(実量基準単収, CultureInfo.InvariantCulture),
                            rs区分 = RS区分,
                            局都道府県コード = 局都道府県コード,
                            市区町村コード = 市区町村コード,
                            大字コード = 大字コード,
                            小字コード = 小字コード,
                            地番 = 地番,
                            枝番 = 枝番,
                            子番 = 子番,
                            孫番 = 孫番,
                            統計市町村コード = 統計市町村コード,
                            統計単位地域コード = 統計地域コード,
                            統計単収 = string.IsNullOrEmpty(統計単収) ? (decimal?)null : decimal.Parse(統計単収, CultureInfo.InvariantCulture),
                            用途区分 = 麦用途区分,
                            産地別銘柄コード = 産地銘柄コード,
                            受委託者コード = 受委託者コード,
                            登録日時 = sysDate,
                            登録ユーザid = BATCH_USER_NAME,
                        };

                        // DBへ追加して保存
                        db.T19070大量データ受入加入申込書oks.Add(dataOk);
                        db.SaveChanges();
                        OK件数++;
                    } // end for
                    
                    // --- 9. タスク終了 ---
                    Console.WriteLine($"処理終了。受入OK件数：{OK件数} 件、エラー件数：{エラー件数} 件");
                    if (OK件数 > 0)
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
                        string okFileName = $"{preNm}-OK-{ukeireRirekiId}.csv";
                        string tempFilePath = Path.Combine(tempFolderPath, okFileName);
                        List<T19070大量データ受入加入申込書ok> getOkListByTorikomiId = GetOkListByTorikomiId(db);

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("\"受入履歴ID\",\"行番号\",\"範囲\",\"年産範囲\",\"共済目的コード範囲\",\"抽出区分\",\"範囲パラメータ１\",\"範囲パラメータ２\",\"範囲パラメータ３\",\"日付\",\"GISデータ出力のタイプ\",\"共済目的コード\",\"組合員等コード\",\"耕地番号\",\"分筆番号\",\"類区分\",\"地名地番\",\"耕地面積\",\"引受面積\",\"転作等面積\",\"受委託区分\",\"備考\",\"田畑区分\",\"区分コード\",\"種類コード\",\"品種コード\",\"収量等級コード\",\"参酌コード\",\"基準単収\",\"基準収穫量\",\"修正日付\",\"計算日付\",\"年産\",\"実量基準単収\",\"ＲＳ区分\",\"局都道府県コード\",\"市区町村コード\",\"大字コード\",\"小字コード\",\"地番\",\"枝番\",\"子番\",\"孫番\",\"統計市町村コード\",\"統計地域コード\",\"統計単収\",\"用途区分\",\"産地銘柄コード\",\"受委託者コード\"");
                        foreach (var ok in getOkListByTorikomiId)
                        {
                            sb.AppendLine(
                                $"\"{ok.受入履歴id}\"," +
                                $"\"{ok.行番号}\"," +
                                $"\"{範囲}\"," +
                                $"\"{ok.年産範囲}\"," +
                                $"\"{ok.共済目的コード範囲}\"," +
                                $"\"{ok.抽出区分}\"," +
                                $"\"{ok.範囲パラメータ１}\"," +
                                $"\"{ok.範囲パラメータ２}\"," +
                                $"\"{ok.範囲パラメータ３}\"," +
                                $"\"{ok.日付}\"," +
                                $"\"{ok.gisデータ出力のタイプ}\"," +
                                $"\"{ok.共済目的コード}\"," +
                                $"\"{ok.組合員等コード}\"," +
                                $"\"{ok.耕地番号}\"," +
                                $"\"{ok.分筆番号}\"," +
                                $"\"{ok.類区分}\"," +
                                $"\"{ok.地名地番}\"," +
                                $"\"{ok.耕地面積}\"," +
                                $"\"{ok.引受面積}\"," +
                                $"\"{ok.転作等面積}\"," +
                                $"\"{ok.受委託区分}\"," +
                                $"\"{ok.備考}\"," +
                                $"\"{ok.田畑区分}\"," +
                                $"\"{ok.区分コード}\"," +
                                $"\"{ok.種類コード}\"," +
                                $"\"{ok.品種コード}\"," +
                                $"\"{ok.収量等級コード}\"," +
                                $"\"{ok.参酌コード}\"," +
                                $"\"{ok.基準単収}\"," +
                                $"\"{ok.基準収穫量}\"," +
                                $"\"{ok.修正日付}\"," +
                                $"\"{ok.計算日付}\"," +
                                $"\"{ok.年産}\"," +
                                $"\"{ok.実量基準単収}\"," +
                                $"\"{ok.rs区分}\"," +
                                $"\"{ok.局都道府県コード}\"," +
                                $"\"{ok.市区町村コード}\"," +
                                $"\"{ok.大字コード}\"," +
                                $"\"{ok.小字コード}\"," +
                                $"\"{ok.地番}\"," +
                                $"\"{ok.枝番}\"," +
                                $"\"{ok.子番}\"," +
                                $"\"{ok.孫番}\"," +
                                $"\"{ok.統計市町村コード}\"," +
                                $"\"{ok.統計単位地域コード}\"," +
                                $"\"{ok.統計単収}\"," +
                                $"\"{ok.用途区分}\"," +
                                $"\"{ok.産地別銘柄コード}\"," +
                                $"\"{ok.受委託者コード}\""
                            );
                        }

                        string tempFolder = Path.Combine(ConfigUtil.Get(Constants.CSV_TEMP_FOLDER), "OkCsvTemp");
                        if (!Directory.Exists(tempFolder))
                        {
                            Directory.CreateDirectory(tempFolder);
                        }
                        // ファイル出力（Shift_JIS、改行はCRLF）
                        File.WriteAllText(tempFilePath, sb.ToString(), Encoding.UTF8);

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
                        okListPath = destinationZipFilePath;
                        okListName = okFileName;
                        ukeireSts = "成功";
                        shoriSts = "03";
                        // 10.1.5 一時領域に作成したフォルダとファイルを削除する
                        Directory.Delete(tempFolderPath, true);
                    }
                    if (エラー件数 > 0)
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
                        string errorFileName = $"{preNm}-ERR-{ukeireRirekiId}.csv";
                        string tempFilePath = Path.Combine(tempFolderPath, errorFileName);
                        List<T01080大量データ受入エラーリスト> getErrorListByTorikomiId = GetErrorListByUkeireId(db);

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("\"エラー対象行数\",\"エラー内容\"");
                        foreach (var errorRecord in getErrorListByTorikomiId)
                        {
                            // 行番号は数値→文字列変換、エラー内容はそのまま
                            sb.AppendLine($"\"{errorRecord.行番号.ToString()}\",\"{errorRecord.エラー内容}\"");
                        }

                        string tempFolder = Path.Combine(ConfigUtil.Get(Constants.CSV_TEMP_FOLDER), "ErrorCsvTemp");
                        if (!Directory.Exists(tempFolder))
                        {
                            Directory.CreateDirectory(tempFolder);
                        }
                        // ファイル出力（Shift_JIS、改行はCRLF）
                        File.WriteAllText(tempFilePath, sb.ToString(), Encoding.UTF8);

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
                        ukeireSts = "成功";
                        shoriSts = "03";
                        // 10.1.5 一時領域に作成したフォルダとファイルを削除する
                        Directory.Delete(tempFolderPath, true);
                    }

                    

                forcedShutdown:;
                    totalCount = OK件数 + エラー件数;

                    int updateResult = UpdateUkeireRirekiFinally(db, ukeireSts, totalCount, エラー件数, errorListName, errorListPath
                        , errorListHashValue, OK件数, okListName, okListPath, okListHashValue, sysDate, ukeireRirekiId);
                    // トランザクションコミット
                    db.SaveChanges();
                    transaction.Commit();
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
                    BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), batchSts, "", BATCH_USER_NAME, ref refMessage);
                    if (0 == BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), batchSts, "", BATCH_USER_NAME, ref refMessage))
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
                    Environment.ExitCode = result;
                }
            }
        }
        // 1. バッチ条件テーブルから、指定されたバッチ条件idおよび条件名称（nensan, filePath, ukeirerirekiId）のいずれかに一致するレコードを取得
        private static List<T01050バッチ条件> GetBatchJoukenList(
            NskAppContext db,
            string joukenId,
            string nensan,
            string filePath,
            string ukeirerirekiId)
        {
            logger.Info("バッチ条件テーブルから、指定されたバッチ条件idおよび条件名称に一致するデータを取得します。");
            var conditionNames = new[] { nensan, filePath, ukeirerirekiId };

            return db.Set<T01050バッチ条件>()
                     .Where(b => b.バッチ条件id == joukenId &&
                                 conditionNames.Contains(b.条件名称))
                     .ToList();
        }


        // 2. 大量データ受入履歴テーブルから、受入履歴idに一致するレコードを取得
        private static T01060大量データ受入履歴 GetUkeireRireki(NskAppContext db, string ukeirerirekiId)
        {
            logger.Info("大量データ受入履歴テーブルから、受入履歴idに一致するレコードを取得します。");
            return db.Set<T01060大量データ受入履歴>()
                     .FirstOrDefault(x => x.受入履歴id == ukeirerirekiId);
        }


        // 3. 大量データ受入履歴テーブルの対象レコードを更新（ステータス、開始日時、更新日時、更新ユーザid）
        private static int UpdateUkeireRireki(
            NskAppContext db,
            string ukeirerirekiId,
            string ukeireSts,
            DateTime systemDate)
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


        // 4. 大量データ受入履歴テーブルの最終更新（各種件数、エラー情報、終了日時、更新日時、更新ユーザid）を更新
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
            string ukeireRirekiId)
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
            entity.終了日時 = sysDate;
            entity.更新日時 = sysDate;
            entity.更新ユーザid = "NSK_112170B";

            return db.SaveChanges();
        }

        // 6. t_todohukenテーブルから都道府県件数取得
        private static int GetTodofukenCount(NskAppContext db, string todofukenCd)
        {
            logger.Info($"都道府県コード {todofukenCd} の件数を取得します。");
            int todofuken = db.VTodofukens
                .Where(x => x.TodofukenCd == todofukenCd)
                .Count();
            return todofuken;
        }


        // 7. m_kumiaitoテーブルから組合員件数取得
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
        /// 支所コード件数取得
        /// </summary>
        private static int GetShishoCdCount(NskAppContext db, string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            int shishoCn = db.VShishoNms
                     .Where(x => x.TodofukenCd == todofukenCd &&
                                 x.KumiaitoCd == kumiaitoCd &&
                                 x.ShishoCd == shishoCd)
                     .Count();

            return shishoCn;
        }

        // 8. m_00010_共済目的名称テーブルから件数取得
        private static int GetKyosaiMokutekiCount(NskAppContext db, string kyosaiMokutekiCd)
        {
            logger.Info($"共済目的コード {kyosaiMokutekiCd} の件数を取得します。");
            return db.Set<M00010共済目的名称>()
                     .Count(x => x.共済目的コード == kyosaiMokutekiCd);
        }


        // 9. m_00020_類名称テーブルから、類区分に一致する件数を取得
        private static int GetRuiKbnCount(NskAppContext db, string ruiKbn)
        {
            logger.Info($"類区分 {ruiKbn} の件数を取得します。");
            return db.Set<M00020類名称>()
                     .Count(x => x.類区分 == ruiKbn);
        }


        // 10. m_00040_田畑名称テーブルから、田畑区分に一致する件数を取得
        private static int GetTabatakeKbnCount(NskAppContext db, string tabatakeKbn)
        {
            logger.Info($"田畑区分 {tabatakeKbn} の件数を取得します。");
            return db.Set<M00040田畑名称>()
                     .Count(x => x.田畑区分 == tabatakeKbn);
        }


        // 11. m_00030_区分名称テーブルから、組合等コード、年産、共済目的コード、区分コードに一致する件数を取得
        private static int GetKubunCount(NskAppContext db, string kumiaitoCd, string nensan, string kyousaimokutekiCd, string kbnCd)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {kyousaimokutekiCd}, 区分コード {kbnCd} の件数を取得します。");
            return db.Set<M00030区分名称>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyousaimokutekiCd &&
                                 x.区分コード == kbnCd);
        }


        // 12. m_10140_種類名称テーブルから、共済目的コードおよび種類コードに一致する件数を取得
        private static int GetShuruiCount(NskAppContext db, string kyosaimokutekiCd, string shuruiCd)
        {
            logger.Info($"共済目的コード {kyosaimokutekiCd} と種類コード {shuruiCd} の件数を取得します。");
            return db.Set<M10140種類名称>()
                     .Count(x => x.共済目的コード == kyosaimokutekiCd &&
                                 x.種類コード == shuruiCd);
        }


        // 13. m_10110_用途区分名称テーブルから、共済目的コードおよび用途区分に一致する件数を取得
        private static int GetYoutoKbnCount(NskAppContext db, string kyosaimokutekiCd, string youtoKbn)
        {
            logger.Info($"共済目的コード {kyosaimokutekiCd} と用途区分 {youtoKbn} の件数を取得します。");
            return db.Set<M10110用途区分名称>()
                     .Count(x => x.共済目的コード == kyosaimokutekiCd &&
                                 x.用途区分 == youtoKbn);
        }


        // 14. m_00110_品種係数テーブルから、組合等コード、年産、共済目的コード、品種コードに一致する件数を取得
        private static int GetHinshuKeisuuCount(NskAppContext db, string kumiaitoCd, string nensan, string kyosaimokutekiCd, string hinshuCd)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {kyosaimokutekiCd}, 品種コード {hinshuCd} の件数を取得します。");
            return db.Set<M00110品種係数>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.品種コード == hinshuCd);
        }


        // 15. m_00130_産地別銘柄名称設定テーブルから、組合等コード、年産、共済目的コード、産地別銘柄コードに一致する件数を取得
        private static int GetSanchiBetuMeigaraCount(NskAppContext db, string kumiaitoCd, string nensan, string kyosaimokutekiCd, string sanchiBetuMeigaraCd)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {kyosaimokutekiCd}, 産地別銘柄コード {sanchiBetuMeigaraCd} の件数を取得します。");
            return db.Set<M00130産地別銘柄名称設定>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.産地別銘柄コード == sanchiBetuMeigaraCd);
        }


        // 16. m_10060_収量等級テーブルから、組合等コード、年産、共済目的コード、類区分、支所コード、収量等級コードに一致する件数を取得
        private static int GetShuryoToukyuCount(NskAppContext db, string kumiaitoCd, string nensan, string kyosaimokutekiCd, string ruiKbn, string sishoCd, string shuryoToukyuCd)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {kyosaimokutekiCd}, 類区分 {ruiKbn}, 支所コード {sishoCd}, 収量等級コード {shuryoToukyuCd} の件数を取得します。");
            return db.Set<M10060収量等級>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.類区分 == ruiKbn &&
                                 x.支所コード == sishoCd &&
                                 x.収量等級コード == shuryoToukyuCd);
        }


        // 17. m_10040_参酌係数テーブルから、組合等コード、年産、共済目的コード、参酌コードに一致する件数を取得
        private static int GetSansyakuKeisuuCount(NskAppContext db, string kumiaitoCd, string nensan, string kyosaimokutekiCd, string sansyakuCd)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {kyosaimokutekiCd}, 参酌コード {sansyakuCd} の件数を取得します。");
            return db.Set<M10040参酌係数>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.参酌コード == sansyakuCd);
        }


        // 18. m_00170_統計単位地域テーブルから、組合等コード、年産、共済目的コード、統計単位地域コードに一致する件数を取得
        private static int GetToukeitaniChiikiCount(NskAppContext db, string kumiaitoCd, string nensan, string kyosaimokutekiCd, string toukeitaniChiikiCd)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {kyosaimokutekiCd}, 統計単位地域コード {toukeitaniChiikiCd} の件数を取得します。");
            return db.Set<M00170統計単位地域>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.統計単位地域コード == toukeitaniChiikiCd);
        }


        // 19. t_19070_大量データ受入_加入申込書OKテーブルから、各条件に一致する件数を取得
        private static int GetJoinApplicationOkCount(
            NskAppContext db,
            string kumiaitoCd,
            string nensan,
            string kyosaimokutekiCd,
            string kumiaiintoCd,
            string kouchiBango,
            string bunpituBango)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {kyosaimokutekiCd}, 組合員等コード {kumiaiintoCd}, 耕地番号 {kouchiBango}, 分筆番号 {bunpituBango} の件数を取得します。");
            return db.Set<T19070大量データ受入加入申込書ok>()
                     .Count(x => x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd &&
                                 x.耕地番号 == kouchiBango &&
                                 x.分筆番号 == bunpituBango);
        }

        private static T19070大量データ受入加入申込書ok GetJoinApplicationError(NskAppContext db, string kumiaitoCd, string nensan, string kyosaimokutekiCd, string kumiaiintoCd, string kouchiBango, string bunpituBango)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {kyosaimokutekiCd}, 組合員等コード {kumiaiintoCd}, 耕地番号 {kouchiBango}, 分筆番号 {bunpituBango} の件数を取得します。");
            return db.Set<T19070大量データ受入加入申込書ok>()
                     .Where(x => x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd &&
                                 x.耕地番号 == kouchiBango &&
                                 x.分筆番号 == bunpituBango)
                     .FirstOrDefault();
        }


        // 20. t_nogyoshaテーブルから、組合員コードおよび組合員等コードに一致する件数を取得（FimContextを利用）
        private static int GetNogyoshaCount(NskAppContext db, string kumiaitoCd, string kumiaiintoCd)
        {
            logger.Info($"組合員コード {kumiaitoCd}, 組合員等コード {kumiaiintoCd} の件数を取得します。");
            return db.Set<VNogyosha>()
                     .Count(x => x.KumiaitoCd == kumiaitoCd &&
                                 x.KumiaiintoCd == kumiaiintoCd);
        }


        // 21. t_01080_大量データ受入_エラーリストテーブルから、処理区分が '2' かつ指定の履歴idに一致するレコードを取得
        private static List<T01080大量データ受入エラーリスト> GetErrorListByUkeireId(NskAppContext db)
        {
            return db.Set<T01080大量データ受入エラーリスト>()
                     .ToList();
        }


        // 23. m_00030_区分名称テーブルから、組合等コード、年産、共済目的コード、区分コードに一致するレコードを取得
        private static List<M00030区分名称> GetKubunMeishouRecords(
            NskAppContext db,
            string kumiaitoCd,
            string nensan,
            string kyosaimokuteCd,
            string kbnCd)
        {
            logger.Info($"Fetching records from m_00030_区分名称 with 組合等コード={kumiaitoCd}, 年産={nensan}, 共済目的コード={kyosaimokuteCd}, 区分コード={kbnCd}");
            return db.Set<M00030区分名称>()
                     .Where(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokuteCd &&
                                 x.区分コード == kbnCd)
                     .ToList();
        }


        // 24. m_10140_種類名称テーブルから、共済目的コードに一致するレコードを取得
        private static List<M10140種類名称> GetShuruiMeishouRecords(NskAppContext db, string kyosaimokutekiCd)
        {
            logger.Info($"共済目的コード {kyosaimokutekiCd} のレコードを取得します。");
            return db.Set<M10140種類名称>()
                     .Where(x => x.共済目的コード == kyosaimokutekiCd)
                     .ToList();
        }

        private static VNogyosha GetNogyoshaByCodes(NskAppContext db, string kumiaitoCd, string kumiaiintoCd)
        {
            logger.Info($"組合員コード {kumiaitoCd}, 組合員等コード {kumiaiintoCd} のレコードを取得します。");

            return db.Set<VNogyosha>()
                     .Where(x => x.KumiaitoCd == kumiaitoCd &&
                                 x.KumiaiintoCd == kumiaiintoCd)
                     .FirstOrDefault();
        }


        /// <summary>
        /// 文字列がすべて半角数字かどうかチェックする
        /// </summary>
        private static bool IsAllDigits(string s)
        {
            return s.All(char.IsDigit);
        }

        /// <summary>
        /// 範囲レコードのチェックを行う
        /// 必須チェック／桁数チェック／形式チェック（抽出区分毎）
        /// </summary>
        /// <param name="fields">範囲レコードの各項目（配列）</param>
        /// <returns>チェック通過の場合 true、エラーの場合 false</returns>
        private static string ValidateRangeRecord(string[] fields, int lineNumber)
        {
            string errMsg = string.Empty;
            // ※ fields[0]～fields[8]：各項目（Trim済みでチェック）
            string 抽出区分 = fields[3].Trim();

            switch (抽出区分)
            {
                case "1":
                    // 1. 範囲：必須、文字数2
                    if (string.IsNullOrWhiteSpace(fields[0].Trim().Trim('"')))
                    {
                        errMsg = MessageUtil.Get("ME00001", "範囲", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (fields[0].Trim().Trim('"').Length > 2)
                    {
                        errMsg = MessageUtil.Get("ME00020", "範囲", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    // 2. 年産：必須、半角数字、文字数4
                    string field2 = fields[1].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(field2))
                    {
                        errMsg = MessageUtil.Get("ME00001", "年産", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (field2.Length > 4)
                    {
                        errMsg = MessageUtil.Get("ME00020", "年産", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!IsAllDigits(field2))
                    {
                        errMsg = MessageUtil.Get("ME00003", "年産", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    // 3. 共済目的コード：（入力があれば）半角数字、文字数2
                    string field3 = fields[2].Trim().Trim('"');
                    if (!string.IsNullOrEmpty(field3))
                    {
                        if (field3.Length > 2)
                        {
                            errMsg = MessageUtil.Get("ME00020", "共済目的コード", "桁数", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                        if (!IsAllDigits(field3))
                        {
                            errMsg = MessageUtil.Get("ME00003", "共済目的コード", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                    }
                    // 4. 抽出区分：必須、半角数字、文字数1
                    string field4 = fields[3].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(field4))
                    {
                        errMsg = MessageUtil.Get("ME00001", "抽出区分", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (field4.Length > 1)
                    {
                        errMsg = MessageUtil.Get("ME00020", "抽出区分", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!IsAllDigits(field4))
                    {
                        errMsg = MessageUtil.Get("ME00003", "抽出区分", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    // 5. 範囲パラメータ１：（入力があれば）半角数字チェック
                    string field5 = fields[4].Trim().Trim('"');
                    if (!string.IsNullOrEmpty(field5) && !IsAllDigits(field5))
                    {
                        errMsg = MessageUtil.Get("ME00003", "範囲パラメータ１", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    // 6. 範囲パラメータ２：（入力があれば）文字列、文字数2
                    string field6 = fields[5].Trim().Trim('"');
                    if (!string.IsNullOrEmpty(field6))
                    {
                        if (field6.Length > 2)
                        {
                            errMsg = MessageUtil.Get("ME00020", "範囲パラメータ２", "桁数", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                    }
                    // 8. 日付：必須、日付型、文字数19、表示形式 "yyyy/MM/dd HH:mm:ss"
                    string field8 = fields[7].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(field8))
                    {
                        errMsg = MessageUtil.Get("ME00001", "日付", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (field8.Length > 19)
                    {
                        errMsg = MessageUtil.Get("ME00020", "日付", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!DateTime.TryParseExact(field8, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    {
                        errMsg = MessageUtil.Get("ME80013", "日付", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    // 9. GISデータ出力のタイプ：必須、半角数字、文字数1
                    string field9 = fields[8].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(field9))
                    {
                        errMsg = MessageUtil.Get("ME00001", "GISデータ出力のタイプ", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (field9.Length > 1)
                    {
                        errMsg = MessageUtil.Get("ME00020", "GISデータ出力のタイプ", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!IsAllDigits(field9))
                    {
                        errMsg = MessageUtil.Get("ME00003", "GISデータ出力のタイプ", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    break;

                case "2":
                    // 抽出区分 = 2 のチェック
                    if (string.IsNullOrWhiteSpace(fields[0].Trim().Trim('"')))
                    {
                        errMsg = MessageUtil.Get("ME00001", "範囲", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (fields[0].Trim().Trim('"').Length > 2)
                    {
                        errMsg = MessageUtil.Get("ME00020", "範囲", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    string f2 = fields[1].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(f2))
                    {
                        errMsg = MessageUtil.Get("ME00001", "年産", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (f2.Length > 4)
                    {
                        errMsg = MessageUtil.Get("ME00020", "年産", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!IsAllDigits(f2))
                    {
                        errMsg = MessageUtil.Get("ME00003", "年産", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    string f3 = fields[2].Trim().Trim('"');
                    if (!string.IsNullOrEmpty(f3))
                    {
                        if (f3.Length > 2)
                        {
                            errMsg = MessageUtil.Get("ME00020", "共済目的コード", "桁数", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                        if (!IsAllDigits(f3))
                        {
                            errMsg = MessageUtil.Get("ME00003", "共済目的コード", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                    }
                    string f4 = fields[3].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(f4))
                    {
                        errMsg = MessageUtil.Get("ME00001", "抽出区分", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (f4.Length > 1)
                    {
                        errMsg = MessageUtil.Get("ME00020", "抽出区分", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!IsAllDigits(f4))
                    {
                        errMsg = MessageUtil.Get("ME00003", "抽出区分", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    string f5 = fields[4].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(f5))
                    {
                        errMsg = MessageUtil.Get("ME00001", "範囲パラメータ１", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (f5.Length > 13)
                    {
                        errMsg = MessageUtil.Get("ME00020", "範囲パラメータ１", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!IsAllDigits(f5))
                    {
                        errMsg = MessageUtil.Get("ME00003", "範囲パラメータ１", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    string f6 = fields[5].Trim().Trim('"');
                    if (!string.IsNullOrEmpty(f6))
                    {
                        if (f6.Length > 13)
                        {
                            errMsg = MessageUtil.Get("ME00020", "範囲パラメータ２", "桁数", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                        if (!IsAllDigits(f6))
                        {
                            errMsg = MessageUtil.Get("ME00003", "範囲パラメータ２", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                    }
                    // Field7（範囲パラメータ３）は特にチェックなし
                    string f8 = fields[7].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(f8))
                    {
                        errMsg = MessageUtil.Get("ME00001", "日付", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (f8.Length > 19)
                    {
                        errMsg = MessageUtil.Get("ME00020", "日付", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!DateTime.TryParseExact(f8, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    {
                        errMsg = MessageUtil.Get("ME80013", "日付", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    string f9 = fields[8].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(f9))
                    {
                        errMsg = MessageUtil.Get("ME00001", "GISデータ出力のタイプ", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (f9.Length > 1)
                    {
                        errMsg = MessageUtil.Get("ME00020", "GISデータ出力のタイプ", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!IsAllDigits(f9))
                    {
                        errMsg = MessageUtil.Get("ME00003", "GISデータ出力のタイプ", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    break;

                case "3":
                    // 抽出区分 = 3 のチェック
                    if (string.IsNullOrWhiteSpace(fields[0].Trim().Trim('"')))
                    {
                        errMsg = MessageUtil.Get("ME00001", "範囲", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (fields[0].Trim().Trim('"').Length > 2)
                    {
                        errMsg = MessageUtil.Get("ME00020", "範囲", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    string s2 = fields[1].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(s2))
                    {
                        errMsg = MessageUtil.Get("ME00001", "年産", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (s2.Length > 4)
                    {
                        errMsg = MessageUtil.Get("ME00020", "年産", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!IsAllDigits(s2))
                    {
                        errMsg = MessageUtil.Get("ME00003", "年産", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    string s3 = fields[2].Trim().Trim('"');
                    if (!string.IsNullOrEmpty(s3))
                    {
                        if (s3.Length > 2)
                        {
                            errMsg = MessageUtil.Get("ME00020", "共済目的コード", "桁数", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                        if (!IsAllDigits(s3))
                        {
                            errMsg = MessageUtil.Get("ME00003", "共済目的コード", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                    }
                    string s4 = fields[3].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(s4))
                    {
                        errMsg = MessageUtil.Get("ME00001", "抽出区分", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (s4.Length > 1)
                    {
                        errMsg = MessageUtil.Get("ME00020", "抽出区分", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!IsAllDigits(s4))
                    {
                        errMsg = MessageUtil.Get("ME00003", "抽出区分", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    string s5 = fields[4].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(s5))
                    {
                        errMsg = MessageUtil.Get("ME00001", "範囲パラメータ１", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (s5.Length > 2)
                    {
                        errMsg = MessageUtil.Get("ME00020", "範囲パラメータ１", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!IsAllDigits(s5))
                    {
                        errMsg = MessageUtil.Get("ME00003", "範囲パラメータ１", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    string s6 = fields[5].Trim().Trim('"');
                    if (!string.IsNullOrEmpty(s6))
                    {
                        if (s6.Length > 4)
                        {
                            errMsg = MessageUtil.Get("ME00020", "範囲パラメータ２", "桁数", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                        if (!IsAllDigits(s6))
                        {
                            errMsg = MessageUtil.Get("ME00003", "範囲パラメータ２", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                    }
                    string s7 = fields[6].Trim().Trim('"');
                    // ※ 抽出区分=3 では、範囲パラメータ２が存在する場合は s7（範囲パラメータ３）は必須
                    if (!string.IsNullOrEmpty(s6))
                    {
                        if (string.IsNullOrWhiteSpace(s7))
                        {
                            errMsg = MessageUtil.Get("ME00001", "範囲パラメータ３", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                        if (s7.Length > 4)
                        {
                            errMsg = MessageUtil.Get("ME00020", "範囲パラメータ３", "桁数", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                        if (!IsAllDigits(s7))
                        {
                            errMsg = MessageUtil.Get("ME00003", "範囲パラメータ３", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                    }
                    string s8 = fields[7].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(s8))
                    {
                        errMsg = MessageUtil.Get("ME00001", "日付", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (s8.Length > 19)
                    {
                        errMsg = MessageUtil.Get("ME00020", "日付", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!DateTime.TryParseExact(s8, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    {
                        errMsg = MessageUtil.Get("ME80013", "日付", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    string s9 = fields[8].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(s9))
                    {
                        errMsg = MessageUtil.Get("ME00001", "GISデータ出力のタイプ", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (s9.Length > 1)
                    {
                        errMsg = MessageUtil.Get("ME00020", "GISデータ出力のタイプ", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!IsAllDigits(s9))
                    {
                        errMsg = MessageUtil.Get("ME00003", "GISデータ出力のタイプ", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    break;

                case "4":
                    // 抽出区分 = 4 のチェック
                    if (string.IsNullOrWhiteSpace(fields[0].Trim().Trim('"')))
                    {
                        errMsg = MessageUtil.Get("ME00001", "範囲", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (fields[0].Trim().Trim('"').Length > 2)
                    {
                        errMsg = MessageUtil.Get("ME00020", "範囲", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    string t2 = fields[1].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(t2))
                    {
                        errMsg = MessageUtil.Get("ME00001", "年産", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (t2.Length > 4)
                    {
                        errMsg = MessageUtil.Get("ME00020", "年産", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!IsAllDigits(t2))
                    {
                        errMsg = MessageUtil.Get("ME00003", "年産", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    string t3 = fields[2].Trim().Trim('"');
                    if (!string.IsNullOrEmpty(t3))
                    {
                        if (t3.Length > 2)
                        {
                            errMsg = MessageUtil.Get("ME00020", "共済目的コード", "桁数", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                        if (!IsAllDigits(t3))
                        {
                            errMsg = MessageUtil.Get("ME00003", "共済目的コード", "(" + lineNumber + "行目)");
                            logger.Error(errMsg);
                            return errMsg;
                        }
                    }
                    string t4 = fields[3].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(t4))
                    {
                        errMsg = MessageUtil.Get("ME00001", "抽出区分", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (t4.Length > 1)
                    {
                        errMsg = MessageUtil.Get("ME00020", "抽出区分", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!IsAllDigits(t4))
                    {
                        errMsg = MessageUtil.Get("ME00003", "抽出区分", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    string t5 = fields[4].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(t5))
                    {
                        errMsg = MessageUtil.Get("ME00001", "範囲パラメータ１", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (t5.Length > 3)
                    {
                        errMsg = MessageUtil.Get("ME00020", "範囲パラメータ１", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!IsAllDigits(t5))
                    {
                        errMsg = MessageUtil.Get("ME00003", "範囲パラメータ１", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    // Field6, Field7：特にチェックなし
                    string t8 = fields[7].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(t8))
                    {
                        errMsg = MessageUtil.Get("ME00001", "日付", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (t8.Length > 19)
                    {
                        errMsg = MessageUtil.Get("ME00020", "日付", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!DateTime.TryParseExact(t8, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    {
                        errMsg = MessageUtil.Get("ME80013", "日付", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    string t9 = fields[8].Trim().Trim('"');
                    if (string.IsNullOrWhiteSpace(t9))
                    {
                        errMsg = MessageUtil.Get("ME00001", "GISデータ出力のタイプ", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (t9.Length > 1)
                    {
                        errMsg = MessageUtil.Get("ME00020", "GISデータ出力のタイプ", "桁数", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    if (!IsAllDigits(t9))
                    {
                        errMsg = MessageUtil.Get("ME00003", "GISデータ出力のタイプ", "(" + lineNumber + "行目)");
                        logger.Error(errMsg);
                        return errMsg;
                    }
                    break;

                default:
                    errMsg = MessageUtil.Get("ME00003", "抽出区分", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
            }

            return string.Empty;
        }


        #region データ行チェック（4行目以降）
        /// <summary>
        /// データ行の各項目について、必須チェック・桁数チェック・形式チェックを行う
        /// ※ 必須項目は空欄の場合エラー、必須でない項目は値が存在する場合のみチェックし、最初のエラーで即時リターンします。
        /// </summary>
        /// <param name="fields">データ行の各項目</param>
        /// <returns>エラーがあればエラーメッセージ、なければ空文字</returns>
        private static string ValidateDataRow(string[] fields, int lineNumber)
        {
            string errMsg = string.Empty;

            // 1. 共済目的コード（必須、半角数字、2桁、表示形式：99）
            string 共済目的コード = GetField(fields, 0).Trim().Trim('"');
            if (string.IsNullOrEmpty(共済目的コード))
            {
                errMsg = MessageUtil.Get("ME00001", "共済目的コード", "(" + lineNumber + "行目)");
                logger.Error(errMsg);
                return errMsg;
            }
            if (共済目的コード.Length > 2)
            {
                errMsg = MessageUtil.Get("ME00020", "共済目的コード", "桁数", "(" + lineNumber + "行目)");
                logger.Error(errMsg);
                return errMsg;
            }
            if (!IsAllDigits(共済目的コード))
            {
                errMsg = MessageUtil.Get("ME00003", "共済目的コード", "(" + lineNumber + "行目)");
                logger.Error(errMsg);
                return errMsg;
            }

            // 2. 組合員等コード（必須、半角数字、13桁、表示形式：9999999999999）
            string 組合員等コード = GetField(fields, 1).Trim().Trim('"');
            if (string.IsNullOrEmpty(組合員等コード))
            {
                errMsg = MessageUtil.Get("ME00001", "組合員等コード", "(" + lineNumber + "行目)");
                logger.Error(errMsg);
                return errMsg;
            }
            if (組合員等コード.Length > 13)
            {
                errMsg = MessageUtil.Get("ME00020", "組合員等コード", "桁数", "(" + lineNumber + "行目)");
                logger.Error(errMsg);
                return errMsg;
            }
            if (!IsAllDigits(組合員等コード))
            {
                errMsg = MessageUtil.Get("ME00003", "組合員等コード", "(" + lineNumber + "行目)");
                logger.Error(errMsg);
                return errMsg;
            }

            // 3. 耕地番号（必須、半角数字、5桁、表示形式：99999）
            string 耕地番号 = GetField(fields, 2).Trim().Trim('"');
            if (string.IsNullOrEmpty(耕地番号))
            {
                errMsg = MessageUtil.Get("ME00001", "耕地番号", "(" + lineNumber + "行目)");
                logger.Error(errMsg);
                return errMsg;
            }
            if (耕地番号.Length > 5)
            {
                errMsg = MessageUtil.Get("ME00020", "耕地番号", "桁数", "(" + lineNumber + "行目)");
                logger.Error(errMsg);
                return errMsg;
            }
            if (!IsAllDigits(耕地番号))
            {
                errMsg = MessageUtil.Get("ME00003", "耕地番号", "(" + lineNumber + "行目)");
                logger.Error(errMsg);
                return errMsg;
            }

            // 4. 分筆番号（必須、半角数字、4桁、表示形式：9999）
            string 分筆番号 = GetField(fields, 3).Trim().Trim('"');
            if (string.IsNullOrEmpty(分筆番号))
            {
                errMsg = MessageUtil.Get("ME00001", "分筆番号", "(" + lineNumber + "行目)");
                logger.Error(errMsg);
                return errMsg;
            }
            if (分筆番号.Length > 4)
            {
                errMsg = MessageUtil.Get("ME00020", "分筆番号", "桁数", "(" + lineNumber + "行目)");
                logger.Error(errMsg);
                return errMsg;
            }
            if (!IsAllDigits(分筆番号))
            {
                errMsg = MessageUtil.Get("ME00003", "分筆番号", "(" + lineNumber + "行目)");
                logger.Error(errMsg);
                return errMsg;
            }

            // 5. 類区分（任意、存在する場合、半角数字、2桁、表示形式：99）
            string 類区分 = GetField(fields, 4).Trim().Trim('"');
            if (!string.IsNullOrEmpty(類区分))
            {
                if (類区分.Length > 2)
                {
                    errMsg = MessageUtil.Get("ME00020", "類区分", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!IsAllDigits(類区分))
                {
                    errMsg = MessageUtil.Get("ME00003", "類区分", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 6. 地名地番（任意、存在する場合、文字列、40文字）
            string 地名地番 = GetField(fields, 5).Trim().Trim('"');
            if (!string.IsNullOrEmpty(地名地番))
            {
                if (地名地番.Length > 40)
                {
                    errMsg = MessageUtil.Get("ME00020", "地名地番", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 7. 耕地面積（任意、存在する場合、半角数字、4文字、表示形式：z9.99）
            string str耕地面積 = GetField(fields, 6).Trim().Trim('"');
            if (!string.IsNullOrEmpty(str耕地面積))
            {
                if (str耕地面積.Length > 4)
                {
                    errMsg = MessageUtil.Get("ME00020", "耕地面積", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!decimal.TryParse(str耕地面積, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
                {
                    errMsg = MessageUtil.Get("ME00003", "耕地面積", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 8. 引受面積（任意、存在する場合、半角数字、4文字、表示形式：z9.99）
            string str引受面積 = GetField(fields, 7).Trim().Trim('"');
            if (!string.IsNullOrEmpty(str引受面積))
            {
                if (str引受面積.Length > 4)
                {
                    errMsg = MessageUtil.Get("ME00020", "引受面積", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!decimal.TryParse(str引受面積, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
                {
                    errMsg = MessageUtil.Get("ME00003", "引受面積", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 9. 転作等面積（任意、存在する場合、半角数字、4文字、表示形式：z9.99）
            string str転作等面積 = GetField(fields, 8).Trim().Trim('"');
            if (!string.IsNullOrEmpty(str転作等面積))
            {
                if (str転作等面積.Length > 4)
                {
                    errMsg = MessageUtil.Get("ME00020", "転作等面積", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!decimal.TryParse(str転作等面積, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
                {
                    errMsg = MessageUtil.Get("ME00003", "転作等面積", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 10. 受委託区分（任意、存在する場合、半角数字、1桁、表示形式：9）
            string 受委託区分 = GetField(fields, 9).Trim().Trim('"');
            if (!string.IsNullOrEmpty(受委託区分))
            {
                if (受委託区分.Length > 1)
                {
                    errMsg = MessageUtil.Get("ME00020", "受委託区分", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!IsAllDigits(受委託区分))
                {
                    errMsg = MessageUtil.Get("ME00003", "受委託区分", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 11. 備考（任意、存在する場合、文字列、40文字）
            string 備考 = GetField(fields, 10).Trim().Trim('"');
            if (!string.IsNullOrEmpty(備考))
            {
                if (備考.Length > 40)
                {
                    errMsg = MessageUtil.Get("ME00020", "備考", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 12. 田畑区分（任意、存在する場合、半角数字、1桁、表示形式：9）
            string 田畑区分 = GetField(fields, 11).Trim().Trim('"');
            if (!string.IsNullOrEmpty(田畑区分))
            {
                if (田畑区分.Length > 1)
                {
                    errMsg = MessageUtil.Get("ME00020", "田畑区分", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!IsAllDigits(田畑区分))
                {
                    errMsg = MessageUtil.Get("ME00003", "田畑区分", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 13. 区分コード（任意、存在する場合、半角数字、2桁、表示形式：99）
            string 区分コード = GetField(fields, 12).Trim().Trim('"');
            if (!string.IsNullOrEmpty(区分コード))
            {
                if (区分コード.Length > 2)
                {
                    errMsg = MessageUtil.Get("ME00020", "区分コード", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!IsAllDigits(区分コード))
                {
                    errMsg = MessageUtil.Get("ME00003", "区分コード", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 14. 種類コード（任意、存在する場合、半角数字、2桁、表示形式：99）
            string 種類コード = GetField(fields, 13).Trim().Trim('"');
            if (!string.IsNullOrEmpty(種類コード))
            {
                if (種類コード.Length > 2)
                {
                    errMsg = MessageUtil.Get("ME00020", "種類コード", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!IsAllDigits(種類コード))
                {
                    errMsg = MessageUtil.Get("ME00003", "種類コード", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 15. 品種コード（任意、存在する場合、半角数字、3桁、表示形式：999）
            string 品種コード = GetField(fields, 14).Trim().Trim('"');
            if (!string.IsNullOrEmpty(品種コード))
            {
                if (品種コード.Length > 3)
                {
                    errMsg = MessageUtil.Get("ME00020", "品種コード", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!IsAllDigits(品種コード))
                {
                    errMsg = MessageUtil.Get("ME00003", "品種コード", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 16. 収量等級コード（任意、存在する場合、半角数字、3桁、表示形式：999）
            string 収量等級コード = GetField(fields, 15).Trim().Trim('"');
            if (!string.IsNullOrEmpty(収量等級コード))
            {
                if (収量等級コード.Length > 3)
                {
                    errMsg = MessageUtil.Get("ME00020", "収量等級コード", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!IsAllDigits(収量等級コード))
                {
                    errMsg = MessageUtil.Get("ME00003", "収量等級コード", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 17. 参酌コード（任意、存在する場合、半角数字、3桁、表示形式：999）
            string 参酌コード = GetField(fields, 16).Trim().Trim('"');
            if (!string.IsNullOrEmpty(参酌コード))
            {
                if (参酌コード.Length > 3)
                {
                    errMsg = MessageUtil.Get("ME00020", "参酌コード", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!IsAllDigits(参酌コード))
                {
                    errMsg = MessageUtil.Get("ME00003", "参酌コード", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 18. 基準単収（任意、存在する場合、半角数字、4文字、表示形式：zzz9）
            string 基準単収 = GetField(fields, 17).Trim().Trim('"');
            if (!string.IsNullOrEmpty(基準単収))
            {
                if (基準単収.Length > 4)
                {
                    errMsg = MessageUtil.Get("ME00020", "基準単収", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!decimal.TryParse(基準単収, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
                {
                    errMsg = MessageUtil.Get("ME00003", "基準単収", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 19. 基準収穫量（任意、存在する場合、半角数字、7文字、表示形式：zzzzzz9）
            string 基準収穫量 = GetField(fields, 18).Trim().Trim('"');
            if (!string.IsNullOrEmpty(基準収穫量))
            {
                if (基準収穫量.Length > 7)
                {
                    errMsg = MessageUtil.Get("ME00020", "基準収穫量", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!decimal.TryParse(基準収穫量, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
                {
                    errMsg = MessageUtil.Get("ME00003", "基準収穫量", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 20. 修正日付（任意、存在する場合、日付型、10文字、表示形式：YYYY/MM/DD）
            string 修正日付 = GetField(fields, 19).Trim().Trim('"');
            if (!string.IsNullOrEmpty(修正日付))
            {
                if (修正日付.Length > 10)
                {
                    errMsg = MessageUtil.Get("ME00020", "修正日付", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!DateTime.TryParseExact(修正日付, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    errMsg = MessageUtil.Get("ME80013", "日付", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 21. 計算日付（任意、存在する場合、日付型、10文字、表示形式：YYYY/MM/DD）
            string 計算日付 = GetField(fields, 20).Trim().Trim('"');
            if (!string.IsNullOrEmpty(計算日付))
            {
                if (計算日付.Length > 10)
                {
                    errMsg = MessageUtil.Get("ME00020", "計算日付", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!DateTime.TryParseExact(計算日付, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    errMsg = MessageUtil.Get("ME80013", "日付", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 22. 年産（必須、半角数字、4桁、表示形式：9999）
            string 年産 = GetField(fields, 21).Trim().Trim('"');
            if (string.IsNullOrEmpty(年産))
            {
                errMsg = MessageUtil.Get("ME00001", "年産", "(" + lineNumber + "行目)");
                logger.Error(errMsg);
                return errMsg;
            }
            if (年産.Length > 4)
            {
                errMsg = MessageUtil.Get("ME00020", "年産", "桁数", "(" + lineNumber + "行目)");
                logger.Error(errMsg);
                return errMsg;
            }
            if (!IsAllDigits(年産))
            {
                errMsg = MessageUtil.Get("ME00003", "年産", "(" + lineNumber + "行目)");
                logger.Error(errMsg);
                return errMsg;
            }

            // 23. 実量基準単収（任意、存在する場合、半角数字、4文字、表示形式：zzz9）
            string 実量基準単収 = GetField(fields, 22).Trim().Trim('"');
            if (!string.IsNullOrEmpty(実量基準単収))
            {
                if (実量基準単収.Length > 4)
                {
                    errMsg = MessageUtil.Get("ME00020", "実量基準単収", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!decimal.TryParse(実量基準単収, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
                {
                    errMsg = MessageUtil.Get("ME00003", "実量基準単収", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 24. ＲＳ区分（任意、存在する場合、半角数字、2桁、表示形式：99）
            string RS区分 = GetField(fields, 23).Trim().Trim('"');
            if (!string.IsNullOrEmpty(RS区分))
            {
                if (RS区分.Length > 2)
                {
                    errMsg = MessageUtil.Get("ME00020", "ＲＳ区分", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!IsAllDigits(RS区分))
                {
                    errMsg = MessageUtil.Get("ME00003", "ＲＳ区分", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 25. GISデータ（任意、存在する場合、文字列、69文字）
            string GISデータ = GetField(fields, 24).Trim().Trim('"');
            if (!string.IsNullOrEmpty(GISデータ))
            {
                if (GISデータ.Length != 69)
                {
                    errMsg = MessageUtil.Get("ME00020", "GISデータ", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 26. 統計市町村コード（任意、存在する場合、半角数字、5桁）
            string 統計市町村コード = GetField(fields, 25).Trim().Trim('"');
            if (!string.IsNullOrEmpty(統計市町村コード))
            {
                if (統計市町村コード.Length > 5)
                {
                    errMsg = MessageUtil.Get("ME00020", "統計市町村コード", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!IsAllDigits(統計市町村コード))
                {
                    errMsg = MessageUtil.Get("ME00003", "統計市町村コード", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 27. 統計地域コード（任意、存在する場合、半角数字、5桁）
            string 統計地域コード = GetField(fields, 26).Trim().Trim('"');
            if (!string.IsNullOrEmpty(統計地域コード))
            {
                if (統計地域コード.Length > 5)
                {
                    errMsg = MessageUtil.Get("ME00020", "統計地域コード", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!IsAllDigits(統計地域コード))
                {
                    errMsg = MessageUtil.Get("ME00003", "統計地域コード", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 28. 統計単収（任意、存在する場合、半角数字、4文字）
            string 統計単収 = GetField(fields, 27).Trim().Trim('"');
            if (!string.IsNullOrEmpty(統計単収))
            {
                if (統計単収.Length > 4)
                {
                    errMsg = MessageUtil.Get("ME00020", "統計単収", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!decimal.TryParse(統計単収, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
                {
                    errMsg = MessageUtil.Get("ME00003", "統計単収", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 29. 麦用途区分（任意、存在する場合、半角数字、3桁、表示形式：999）
            string 麦用途区分 = GetField(fields, 28).Trim().Trim('"');
            if (!string.IsNullOrEmpty(麦用途区分))
            {
                if (麦用途区分.Length > 3)
                {
                    errMsg = MessageUtil.Get("ME00020", "麦用途区分", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!IsAllDigits(麦用途区分))
                {
                    errMsg = MessageUtil.Get("ME00003", "麦用途区分", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 30. 産地銘柄コード（任意、存在する場合、半角数字、5桁、表示形式：99999）
            string 産地銘柄コード = GetField(fields, 29).Trim().Trim('"');
            if (!string.IsNullOrEmpty(産地銘柄コード))
            {
                if (産地銘柄コード.Length > 5)
                {
                    errMsg = MessageUtil.Get("ME00020", "産地銘柄コード", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!IsAllDigits(産地銘柄コード))
                {
                    errMsg = MessageUtil.Get("ME00003", "産地銘柄コード", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            // 31. 受委託者コード（任意、存在する場合、半角数字、13桁、表示形式：9999999999999）
            string 受委託者コード = GetField(fields, 30).Trim().Trim('"');
            if (!string.IsNullOrEmpty(受委託者コード))
            {
                if (受委託者コード.Length > 13)
                {
                    errMsg = MessageUtil.Get("ME00020", "受委託者コード", "桁数", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
                if (!IsAllDigits(受委託者コード))
                {
                    errMsg = MessageUtil.Get("ME00003", "受委託者コード", "(" + lineNumber + "行目)");
                    logger.Error(errMsg);
                    return errMsg;
                }
            }

            return string.Empty;
        }


        private static string GetField(string[] fields, int index)
        {
            return fields.Length > index ? fields[index] : "";
        }

        #endregion
        private static List<T19070大量データ受入加入申込書ok> GetOkListByTorikomiId(NskAppContext db)
        {
            return db.Set<T19070大量データ受入加入申込書ok>()
                     .AsNoTracking()
                     .ToList();
        }

        private static void AddError(
            string ukeireRirekiId
            , int errListSeq
            , int 行番号
            , string dataError
            , DateTime sysDate
            , NskAppContext db)
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
            db.SaveChanges();
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
    }
}
