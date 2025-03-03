using NSK_B112051_RuibetuSetInputDataUkeire.Common;
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

namespace NSK_B112051_RuibetuSetInputDataUkeire
{
    /// <summary>
    /// 定時実行予約登録
    /// </summary>
    class Program
    {
        /// <summary>
        /// バッチ名
        /// </summary>
        private static string BATCH_NAME = "組合員等類別設定大量データ受入";
        private static string BATCH_USER_NAME = "NSK_B112051";

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
            var ukeirerirekiId = string.Empty;
            var filePath = string.Empty;
            var fileHash = string.Empty;

            bool forcedShutdownFlg = false;
            var shoriSts = Constants.TASK_FAILED;
            var errorMsg = string.Empty;
            string acceptanceErrorContent = "";
            int lineNumber = 0;
            bool skipByErrorFlg = false;
            
            int result = Constants.BATCH_EXECUT_SUCCESS;
            string extractedFilePath = string.Empty;

            int okCount = 0;
            int errCount = 0;
            int total = 0;

            var preNm = string.Empty;
            var sufNm = string.Empty;


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
            var ukeireErrorList = new List<ErrorList>();
            var ukeireOkList = new List<T19050大量データ受入組合員等類別設定ok>();
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

                    if (string.IsNullOrEmpty(ukeirerirekiId))
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
                    if (string.IsNullOrEmpty(fileHash))
                    {
                        acceptanceErrorContent = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    var tairyoData = GetTairyoDataTorikomi(db, ukeirerirekiId);
                    
                    preNm = tairyoData.取込ファイル_変更前ファイル名;
                    if (tairyoData == null)
                    {
                        errorMsg = MessageUtil.Get("ME01645", "受入履歴の取得");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }
                    if (!"01".Equals(tairyoData.ステータス)  /* 処理待ち */)
                    {
                        errorMsg = MessageUtil.Get("ME10042", "受入処理");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    // 処理中
                    var runningSts = "02";
                    UpdateUkeireRireki(db, ukeirerirekiId, runningSts, sysDate);


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
                    for (int i = 0; i < headerFields.Length; i++)
                    {
                        logger.Debug("headerFields[i] : " + headerFields[i]);
                    }
                    if (headerFields.Length != 21)
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
                    string 処理区分F = string.Empty;
                    string 組合等コードF = string.Empty;
                    string 年産F = string.Empty;
                    string 共済目的コードF = string.Empty;
                    string 組合員等コードF = string.Empty;
                    string 類区分F = string.Empty;
                    string 引受区分F = string.Empty;
                    string 引受方式F = string.Empty;
                    string 特約区分F = string.Empty;
                    string 補償割合コードF = string.Empty;
                    string 付保割合F = string.Empty;
                    string 種類区分F = string.Empty;
                    string 作付時期F = string.Empty;
                    string 田畑区分F = string.Empty;
                    string 共済金額選択順位F = string.Empty;
                    string 収穫量確認方法F = string.Empty;
                    string 全相殺基準単収F = string.Empty;
                    string 営農支払以外フラグF = string.Empty;
                    string 担手農家区分F = string.Empty;
                    string 全相殺受託者等名称F = string.Empty;
                    string 備考F = string.Empty;

                    // 13. 初期変数の設定
                    lineNumber = 1; // ヘッダ行を1行目とする

                    // 14. データ行（2行目以降）のチェック
                    for (int i = 1; i < allLines.Length; i++)
                    {
                        string line = allLines[i];
                        // 次の行がない（EOF）または空白行の場合、終了
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME90015", "データ行");
                            forcedShutdownFlg = true;
                            goto forcedShutdown;
                        }

                        // 行番号の更新
                        lineNumber++;
                        total++;

                        // 項目数チェック
                        string[] fields = line.Split(',');
                        if (fields.Length != 21)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10066");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        acceptanceErrorContent = "";
　
                        // 14.5 属性チェック
                        // カラム順（インデックス）および仕様
                        //  0: 処理区分        全角, 文字数(1)
                        //  1: 組合等コード    数字, 文字数(3) 必須
                        //  2: 年産            数字, 文字数(4) 必須
                        //  3: 共済目的コード  数字, 文字数(2) 必須
                        //  4: 組合員等コード  数字, 文字数(13) 必須
                        //  5: 類区分          数字, 文字数(2) 必須
                        //  6: 引受区分        数字, 文字数(2) 必須
                        //  7: 引受方式        数字, 文字数(1) 
                        //  8: 特約区分        数字, 文字数(1)
                        //  9: 補償割合コード  数字, 文字数(2) 
                        // 10: 付保割合        数字, 文字数(2) 
                        // 11: 種類区分        数字, 文字数(1) 
                        // 12: 作付時期        数字, 文字数(1) 
                        // 13: 田畑区分        数字, 文字数(2) 
                        // 14: 共済金額選択順位 数字, 文字数(2) 
                        // 15: 収穫量確認方法   数字, 文字数(2) 
                        // 16: 全相殺基準単収   数字, 文字数(4) 
                        // 17: 営農支払以外フラグ 数字, 文字数(1)
                        // 18: 担手農家区分     数字, 文字数(1) 
                        // 19: 全相殺受託者等名称 全角, 最大文字数(30)
                        // 20: 備考            全角, 最大文字数(30)

                        // 各フィールドのトリム
                        string col処理区分 = fields[0].Trim();
                        string col組合等コード = fields[1].Trim();
                        string col年産 = fields[2].Trim();
                        string col共済目的コード = fields[3].Trim();
                        string col組合員等コード = fields[4].Trim();
                        string col類区分 = fields[5].Trim();
                        string col引受区分 = fields[6].Trim();
                        string col引受方式 = fields[7].Trim();
                        string col特約区分 = fields[8].Trim();
                        string col補償割合コード = fields[9].Trim();
                        string col付保割合 = fields[10].Trim();
                        string col種類区分 = fields[11].Trim();
                        string col作付時期 = fields[12].Trim();
                        string col田畑区分 = fields[13].Trim();
                        string col共済金額選択順位 = fields[14].Trim();
                        string col収穫量確認方法 = fields[15].Trim();
                        string col全相殺基準単収 = fields[16].Trim();
                        string col営農支払以外フラグ = fields[17].Trim();
                        string col担手農家区分 = fields[18].Trim();
                        string col全相殺受託者等名称 = fields[19].Trim();
                        string col備考 = fields[20].Trim();
                        // 14.5.1 必須チェック（必須項目が空の場合）

                        if (string.IsNullOrEmpty(col組合等コード.Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "組合等コード", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        if (string.IsNullOrEmpty(col年産.Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "年産", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        if (string.IsNullOrEmpty(col共済目的コード.Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "共済目的コード", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        if (string.IsNullOrEmpty(col組合員等コード.Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "組合員等コード", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        if (string.IsNullOrEmpty(col類区分.Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "類区分", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        if (string.IsNullOrEmpty(col引受区分.Trim('"')))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00001", "引受区分", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        // 14.5.2 桁数チェックおよび形式チェック（数字項目は正規表現で形式チェック）
                        if (!string.IsNullOrEmpty(col処理区分.Trim('"')))
                        {
                            if (col処理区分.Length != 1)
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00020", "処理区分", "1", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                        }
                        // 組合等コード：数字, 3桁
                        if (col組合等コード.Trim('"').Length > 3)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "組合等コード", "3", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        if (!Regex.IsMatch(col組合等コード.Trim('"'), @"^\d+$"))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "組合等コード", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        // 年産：数字, 4桁
                        if (col年産.Trim('"').Length > 4)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "年産", "4", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        if (!Regex.IsMatch(col年産.Trim('"'), @"^\d+$"))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "年産", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        // 共済目的コード：数字, 2桁
                        if (col共済目的コード.Trim('"').Length > 2)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "共済目的コード", "2", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        if (!Regex.IsMatch(col共済目的コード.Trim('"'), @"^\d+$"))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "共済目的コード", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        // 組合員等コード：数字, 13桁
                        if (col組合員等コード.Trim('"').Length > 13)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "組合員等コード", "13", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        if (!Regex.IsMatch(col組合員等コード.Trim('"'), @"^\d+$"))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "組合員等コード", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        // 類区分：数字, 2桁
                        if (col類区分.Trim('"').Length > 2)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "類区分", "2", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        if (!Regex.IsMatch(col類区分.Trim('"'), @"^\d+$"))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "類区分", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        // 引受区分：数字, 2桁
                        if (col引受区分.Trim('"').Length > 2)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "引受区分", "2", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        if (!Regex.IsMatch(col引受区分.Trim('"'), @"^\d+$"))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00003", "引受区分", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        // ※ 引受方式、特約区分、補償割合コード、付保割合、種類区分、作付時期、田畑区分、共済金額選択順位、
                        //     収穫量確認方法、全相殺基準単収、営農支払以外フラグ、担手農家区分は任意項目として、空の場合はチェックを行わず、
                        //     入力があれば桁数・形式チェックを実施
                        if (!string.IsNullOrEmpty(col引受方式.Trim('"')))
                        {
                            if (col引受方式.Length > 1)
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00020", "引受方式", "1", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                            if (!Regex.IsMatch(col引受方式.Trim('"'), @"^\d+$"))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00003", "引受方式", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                        }
                        if (!string.IsNullOrEmpty(col特約区分.Trim('"')))
                        {
                            if (col特約区分.Length > 1)
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00020", "特約区分", "1", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                            if (!Regex.IsMatch(col特約区分.Trim('"'), @"^\d+$"))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00003", "特約区分", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                        }
                        if (!string.IsNullOrEmpty(col補償割合コード.Trim('"')))
                        {
                            if (col補償割合コード.Trim('"').Length > 2)
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00020", "補償割合コード", "2", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                            if (!Regex.IsMatch(col補償割合コード.Trim('"'), @"^\d+$"))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00003", "補償割合コード", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                        }
                        if (!string.IsNullOrEmpty(col付保割合.Trim('"')))
                        {
                            if (col付保割合.Trim('"').Length > 2)
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00020", "付保割合", "2", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                            if (!Regex.IsMatch(col付保割合.Trim('"'), @"^\d+$"))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00003", "付保割合", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                        }
                        if (!string.IsNullOrEmpty(col種類区分.Trim('"')))
                        {
                            if (col種類区分.Trim('"').Length > 1)
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00020", "種類区分", "1", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                            if (!Regex.IsMatch(col種類区分.Trim('"'), @"^\d+$"))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00003", "種類区分", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                        }
                        if (!string.IsNullOrEmpty(col作付時期.Trim('"')))
                        {
                            if (col作付時期.Trim('"').Length > 1)
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00020", "作付時期", "1", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                            if (!Regex.IsMatch(col作付時期.Trim('"'), @"^\d+$"))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00003", "作付時期", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                        }
                        if (!string.IsNullOrEmpty(col田畑区分.Trim('"')))
                        {
                            if (col田畑区分.Trim('"').Length > 2)
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00020", "田畑区分", "2", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                            if (!Regex.IsMatch(col田畑区分.Trim('"'), @"^\d+$"))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00003", "田畑区分", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                        }
                        if (!string.IsNullOrEmpty(col共済金額選択順位.Trim('"')))
                        {
                            if (col共済金額選択順位.Trim('"').Length > 2)
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00020", "共済金額選択順位", "2", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                            if (!Regex.IsMatch(col共済金額選択順位.Trim('"'), @"^\d+$"))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00003", "共済金額選択順位", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                        }
                        if (!string.IsNullOrEmpty(col収穫量確認方法.Trim('"')))
                        {
                            if (col収穫量確認方法.Trim('"').Length > 2)
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00020", "収穫量確認方法", "2", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                            if (!Regex.IsMatch(col収穫量確認方法.Trim('"'), @"^\d+$"))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00003", "収穫量確認方法", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                        }
                        if (!string.IsNullOrEmpty(col全相殺基準単収.Trim('"')))
                        {
                            if (col全相殺基準単収.Trim('"').Length > 4)
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00020", "全相殺基準単収", "4", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                            if (!Regex.IsMatch(col全相殺基準単収.Trim('"'), @"^\d+$"))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00003", "全相殺基準単収", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                        }
                        if (!string.IsNullOrEmpty(col営農支払以外フラグ.Trim('"')))
                        {
                            if (col営農支払以外フラグ.Trim('"').Length > 1)
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00020", "営農支払以外フラグ", "1", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                            if (!Regex.IsMatch(col営農支払以外フラグ.Trim('"'), @"^\d+$"))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00003", "営農支払以外フラグ", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                        }
                        if (!string.IsNullOrEmpty(col担手農家区分.Trim('"')))
                        {
                            if (col担手農家区分.Trim('"').Length > 1)
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00020", "担手農家区分", "1", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                            if (!Regex.IsMatch(col担手農家区分.Trim('"'), @"^\d+$"))
                            {
                                acceptanceErrorContent = MessageUtil.Get("ME00003", "担手農家区分", "(" + lineNumber + "行目)");
                                skipByErrorFlg = true;
                                goto skipByError;
                            }
                        }
                        // 全相殺受託者等名称：全角, 最大30文字（必須でなければ空でも可）
                        if (!string.IsNullOrEmpty(col全相殺受託者等名称.Trim('"')) && col全相殺受託者等名称.Trim('"').Length > 30)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "全相殺受託者等名称", "30", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        // 備考：全角, 最大30文字（任意）
                        if (!string.IsNullOrEmpty(col備考.Trim('"')) && col備考.Trim('"').Length > 30)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME00020", "備考", "30", "(" + lineNumber + "行目)");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        //14.6 各変数へ値を設定
                        処理区分F = col処理区分.Trim('"');
                        組合等コードF = col組合等コード.Trim('"');
                        年産F = col年産.Trim('"');
                        共済目的コードF = col共済目的コード.Trim('"');
                        組合員等コードF = col組合員等コード.Trim('"');
                        類区分F = col類区分.Trim('"');
                        引受区分F = col引受区分.Trim('"');
                        引受方式F = col引受方式.Trim('"');
                        特約区分F = col特約区分.Trim('"');
                        補償割合コードF = col補償割合コード.Trim('"');
                        付保割合F = col付保割合.Trim('"');
                        種類区分F = col種類区分.Trim('"');
                        作付時期F = col作付時期.Trim('"');
                        田畑区分F = col田畑区分.Trim('"');
                        共済金額選択順位F = col共済金額選択順位.Trim('"');
                        収穫量確認方法F = col収穫量確認方法.Trim('"');
                        全相殺基準単収F = col全相殺基準単収.Trim('"');
                        営農支払以外フラグF = col営農支払以外フラグ.Trim('"');
                        担手農家区分F = col担手農家区分.Trim('"');
                        全相殺受託者等名称F = col全相殺受託者等名称.Trim('"');
                        備考F = col備考.Trim('"');

                        if (tairyoData.組合等コード.Equals(組合等コードF))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10085", "組合等コード");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        if (nensan.Equals(年産F))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME90015", "年産");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        // 共済目的コードのチェック
                        if (0 == GetKyosaiMokutekiCount(db, 共済目的コードF))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10005", "共済目的コード");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        if (0 == GetRuiKbnCount(db, 共済目的コードF))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10005", "類区分");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        if (0 == GetHikiukeKbnCount(db, 共済目的コードF, 引受区分F))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10005", "類区分");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        if (0 == GetHikiukeHousiki(db, 引受方式F))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10005", "引受方式");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        if (0 == GetTokuyakuKbn(db, 特約区分F))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10005", "特約区分");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        if (0 == GetHoshowariai(db, 補償割合コードF))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10005", "補償割合コード");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        if (0 == GetShuruiKbn(db, 共済目的コードF, 種類区分F))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10005", "種類区分");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        if (0 == GetTabatakeKbn(db, 田畑区分F))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10005", "田畑区分");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        if (0 == GetShukakuryoChk(db, 収穫量確認方法F))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10005", "収穫量確認方法");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        if (0 == GetNogyoshaCount(db, 組合等コードF, 組合員等コードF))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10016", "農業者情報管理システム", 組合員等コードF);
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        if (0 == GetTairyoOkCount(db, 組合等コードF, short.Parse(年産F), 共済目的コードF, 組合員等コードF, 類区分F, 引受区分F))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME90018", "取込ファイル内のデータ");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        if (0 == GetKojinSetCount(db, 組合等コードF, short.Parse(年産F),共済目的コードF, 組合員等コードF))
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10027");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }

                        var kojinrui = GetKojinRuiCount(db, 組合等コードF, short.Parse(年産F), 共済目的コードF, 組合員等コードF, 類区分F, 引受区分F);
                        
                        if ("1".Equals(処理区分F) && kojinrui == 0)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10014", "個人設定類テーブル", "");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        if ("2".Equals(処理区分F) && kojinrui >= 1)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10016", "個人設定類テーブル", "");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }
                        if ("3".Equals(処理区分F) && kojinrui == 0)
                        {
                            acceptanceErrorContent = MessageUtil.Get("ME10014", "個人設定類テーブル", "");
                            skipByErrorFlg = true;
                            goto skipByError;
                        }


                    skipByError:;
                        if (skipByErrorFlg)
                        {
                            var entity = new T01080大量データ受入エラーリスト
                            {
                                処理区分 = "1",
                                履歴id = long.Parse(ukeirerirekiId),
                                枝番 = lineNumber,
                                エラー内容 = acceptanceErrorContent,
                                登録日時 = sysDate,
                                登録ユーザid = BATCH_USER_NAME
                            };
                            db.T01080大量データ受入エラーリストs.Add(entity);
                            var entityForFile = new ErrorList
                            {
                                エラー内容 = acceptanceErrorContent,
                                行番号 = lineNumber
                            };
                            ukeireErrorList.Add(entityForFile);
                            errCount++;
                        } 
                        else
                        {
                            var entity = new T19050大量データ受入組合員等類別設定ok
                            {
                                処理区分 = 処理区分F,
                                組合等コード = 組合等コードF,
                                年産 = short.Parse(年産F),
                                共済目的コード = 共済目的コードF,
                                組合員等コード = 組合員等コードF,
                                類区分 = 類区分F,
                                引受区分 = 引受区分F,
                                引受方式 = 引受方式F,
                                特約区分 = 特約区分F,
                                補償割合コード = 補償割合コードF,
                                付保割合 = 付保割合F,
                                種類区分 = 種類区分F,
                                作付時期 = 作付時期F,
                                田畑区分 = 田畑区分F,
                                共済金額選択順位 = 共済金額選択順位F,
                                収穫量確認方法 = 収穫量確認方法F,
                                全相殺基準単収 = decimal.Parse(全相殺基準単収F),
                                営農支払以外フラグ = 営農支払以外フラグF,
                                担手農家区分 = 担手農家区分F,
                                全相殺受託者等名称 = 全相殺受託者等名称F,
                                備考 = 備考F,
                                登録日時 = sysDate,
                                登録ユーザid = BATCH_USER_NAME
                            };
                            db.T19050大量データ受入組合員等類別設定oks.Add(entity);
                            ukeireOkList.Add(entity);
                            okCount++;
                        }

                    } // end for

                    db.SaveChanges();

                forcedShutdown:;
                    /*
                     * TODO:ファイル格納先と処理のロジックが決まってないため、
                     * 必要に応じて修正
                     */
                    if (1 <= errCount)
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
                        string okFileName = $"{preNm}-OK-{ukeirerirekiId}.csv";
                        string tempFilePath = Path.Combine(tempFolderPath, okFileName);

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
                        db.SaveChanges();
                        // ファイル出力（UTF-8、改行はCRLF）
                        File.WriteAllText(tempFilePath, sb.ToString(), Encoding.GetEncoding("Shift_JIS"));

                        // 10.1.3 ファイルのハッシュ値を取得して変数に設定する
                        byte[] csvBytes = File.ReadAllBytes(tempFilePath);
                        var errorListHashValue = CryptoUtil.GetSHA256Hex(csvBytes);

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
                        var errorListPath = destinationZipFilePath;

                        // 10.1.5 一時領域に作成したフォルダとファイルを削除する
                        Directory.Delete(tempFolderPath, true);
                        logger.Error(MessageUtil.Get("ME10026", "取込対象"));
                    }
                    /*
                     * TODO:ファイル格納先と処理のロジックが決まってないため、
                     * 必要に応じて修正
                     */
                    if (1 <= errCount)
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
                        string errorFileName = $"{preNm}-ERR-{ukeirerirekiId}.csv";
                        string tempFilePath = Path.Combine(tempFolderPath, errorFileName);

                        // 10.1.2 エラーリストの内容をCSV形式で作成
                        // ヘッダ： "エラー対象行数","エラー内容"
                        // 各データ行もすべての項目をダブルクォーテーションで囲む
                        StringBuilder sb = new StringBuilder();
                        // ヘッダ行（各項目をダブルクォーテーションで囲む、カンマ区切り）
                        sb.AppendLine("\"処理区分\",\"組合等コード\",\"年産\",\"共済目的コード\",\"組合員等コード\",\"類区分\",\"引受区分\",\"引受方式\",\"特約区分\",\"補償割合コード\",\"付保割合\",\"種類区分\",\"作付時期\",\"田畑区分\",\"共済金額選択順位\",\"収穫量確認方法\",\"全相殺基準単収\",\"営農支払以外フラグ\",\"担手農家区分\",\"全相殺受託者等名称\",\"備考\"");

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
                                $"\"{ok.引受方式}\"," +
                                $"\"{ok.特約区分}\"," +
                                $"\"{ok.補償割合コード}\"," +
                                $"\"{ok.付保割合}\"," +
                                $"\"{ok.種類区分}\"," +
                                $"\"{ok.作付時期}\"," +
                                $"\"{ok.田畑区分}\"," +
                                $"\"{ok.共済金額選択順位}\"," +
                                $"\"{ok.収穫量確認方法}\"," +
                                $"\"{ok.全相殺基準単収}\"," +
                                $"\"{ok.営農支払以外フラグ}\"," +
                                $"\"{ok.担手農家区分}\"," +
                                $"\"{ok.全相殺受託者等名称}\"," +
                                $"\"{ok.備考}\""
                            );
                        }
                        // ファイル出力（UTF-8、改行はCRLF）
                        File.WriteAllText(tempFilePath, sb.ToString(), Encoding.GetEncoding("Shift_JIS"));

                        // 10.1.3 ファイルのハッシュ値を取得して変数に設定する
                        byte[] csvBytes = File.ReadAllBytes(tempFilePath);
                        var errorListHashValue = CryptoUtil.GetSHA256Hex(csvBytes);

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
                        var errorListPath = destinationZipFilePath;

                        // 10.1.5 一時領域に作成したフォルダとファイルを削除する
                        Directory.Delete(tempFolderPath, true);
                        logger.Error(MessageUtil.Get("ME10026", "取込対象"));
                    }

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

        private static T01060大量データ受入履歴 GetTairyoDataTorikomi(NskAppContext db, string ukeirerirekiId)
        {
            logger.Info("大量データ取込履歴テーブルから、取込履歴IDに一致するレコードを取得します。");
            return db.Set<T01060大量データ受入履歴>()
                     .FirstOrDefault(x => x.受入履歴id == ukeirerirekiId);
        }

        // 3. 大量データ受入履歴の更新（開始日時、ステータス等）
        private static int UpdateUkeireRireki(NskAppContext db, string ukeirerirekiId, string ukeireSts, DateTime systemDate)
        {
            var entity = db.Set<T01060大量データ受入履歴>()
                           .FirstOrDefault(x => x.受入履歴id == ukeirerirekiId);
            if (entity == null)
            {
                return 0;
            }

            entity.ステータス = ukeireSts;
            entity.開始日時 = systemDate;
            entity.更新日時 = systemDate;
            entity.更新ユーザid = BATCH_USER_NAME;

            return db.SaveChanges();
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
        private static int GetRuiKbnCount(NskAppContext db, string kyosaiMokutekiCd)
        {
            return db.Set<M00020類名称>()
                     .Count(x => x.共済目的コード == kyosaiMokutekiCd);
        }

        /// <summary>
        /// 引受区分名称件数取得
        /// </summary>
        private static int GetHikiukeKbnCount(NskAppContext db, string kyosaiMokutekiCd, string hikiukeKbn)
        {
            return db.Set<M10090引受区分名称>()
                     .Count(x => x.共済目的コード == kyosaiMokutekiCd &&
                                 x.引受区分 == hikiukeKbn);
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
        /// 特約区分件数取得
        /// </summary>
        private static int GetTokuyakuKbn(NskAppContext db, string tokuyakuKbn)
        {
            return db.Set<M10100特約区分名称>()
                     .Count(x => x.特約区分 == tokuyakuKbn);
        }

        /// <summary>
        /// 補償割合名称件数取得
        /// </summary>
        private static int GetHoshowariai(NskAppContext db, string hoshowariaiCd)
        {
            return db.Set<M20030補償割合名称>()
                     .Count(x => x.補償割合コード == hoshowariaiCd);
        }

        /// <summary>
        /// 補償割合名称件数取得
        /// </summary>
        private static int GetShuruiKbn(NskAppContext db, string kyosaimokutekiCd, string shuruiKbn)
        {
            return db.Set<M10130種類区分名称>()
                     .Count(x => x.共済目的コード == kyosaimokutekiCd &&
                                 x.種類区分 == shuruiKbn);
        }

        /// <summary>
        /// 田畑区分件数取得
        /// </summary>
        private static int GetTabatakeKbn(NskAppContext db, string tabatakeKbn)
        {
            return db.Set<M00040田畑名称>()
                     .Count(x => x.田畑区分 == tabatakeKbn);
        }

        /// <summary>
        /// 収穫量確認方法件数取得
        /// </summary>
        private static int GetShukakuryoChk(NskAppContext db, string shukakuryochk)
        {
            return db.Set<M00070収穫量確認方法名称>()
                     .Count(x => x.収穫量確認方法 == shukakuryochk);
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
        /// T11030個人設定類取得
        /// </summary>
        private static int GetKojinRuiCount(
            NskAppContext db
            , string kumiaitoCd
            , short? nensan
            , string kyosaimokutekiCd
            , string kumiaiintoCd
            , string ruiKbn
            , string hikiukeKbn)
        {
            return db.Set<T11030個人設定類>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == nensan &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd &&
                                 x.類区分 == ruiKbn &&
                                 x.引受区分 == hikiukeKbn);
        }

        /// <summary>
        /// T11010個人設定取得
        /// </summary>
        private static int GetKojinSetCount(
            NskAppContext db
            , string kumiaitoCd
            , short? nensan
            , string kyosaimokutekiCd
            , string kumiaiintoCd)
        {
            return db.Set<T11010個人設定>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == nensan &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd);
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
