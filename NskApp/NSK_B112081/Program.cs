using NSK_B112081.Common;
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

namespace NSK_B112081
{
    /// <summary>
    /// 定時実行予約登録
    /// </summary>
    class Program
    {
        /// <summary>
        /// バッチ名
        /// </summary>
        private static string BATCH_NAME = "組合員等類別設定受入データ取込";
        private static string BATCH_USER_NAME = "NSK_112081B";

        static string defaultSchemaSystemCommon = string.Empty;
        static string defaultSchemaJigyoCommon = string.Empty;
        static string sysDateTimeFlag = string.Empty;
        static string sysDateTimePath = string.Empty;
        static string preNm = string.Empty;

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
            logger.Info(string.Concat(CoreConst.LOG_START_KEYWORD, " 組合員等類別設定受入データ取込"));
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
            var torikomiRirekiId = string.Empty;
            var filePath = string.Empty;
            var fileHash = string.Empty;

            var torikomiSts = "失敗";
            var kumiaitoCdUkeire = string.Empty;
            var shoriSts = "99";
            var errorMsg = string.Empty;
            string msgTest = string.Empty;
            int errCount = 0;
            var total = 0;
            var errorListName = string.Empty;
            var errorListPath = string.Empty;
            var errorListHashValue = string.Empty;
            var okListName = string.Empty;
            var okListPath = string.Empty;
            var okListHashValue = string.Empty;
            bool errFlg = false;
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

            logger.Debug("dbConnectionInfo.ConnectionString : " + dbConnectionInfo.ConnectionString);
            logger.Debug("dbConnectionInfo.DefaultSchema : " + dbConnectionInfo.DefaultSchema);

            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema, ConfigUtil.GetInt(Constants.CONFIG_COMMAND_TIMEOUT)))
            {
                var transaction = db.Database.BeginTransaction();
                try
                {
                    // バッチ条件情報取得
                    List<T01050バッチ条件> joukenList = GetBatchJoukenList(db, jid,Constants.JOUKEN_NENSAN ,Constants.JOUKEN_FILE_PATH, Constants.JOUKEN_TORIKOMIRIREKI_ID, Constants.JOUKEN_FILE_HASH);
                    if (joukenList.IsNullOrEmpty())
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        goto forcedShutdown;
                    }
                    for (int i = 0; i < joukenList.Count; i++)
                    {
                        if (Constants.JOUKEN_NENSAN.Equals(joukenList[i].条件名称))
                        {
                            nensan = joukenList[i].条件値;
                        }
                        if (Constants.JOUKEN_TORIKOMIRIREKI_ID.Equals(joukenList[i].条件名称))
                        {
                            torikomiRirekiId = joukenList[i].条件値;
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
                        goto forcedShutdown;
                    }

                    if (string.IsNullOrEmpty(torikomiRirekiId))
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        goto forcedShutdown;
                    }

                    if (string.IsNullOrEmpty(filePath))
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        goto forcedShutdown;
                    }
                    if (string.IsNullOrEmpty(fileHash))
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        goto forcedShutdown;
                    }


                    T01070大量データ取込履歴 tairyoData = GetTairyoDataTorikomi(db, torikomiRirekiId);
                    if (tairyoData == null || !"01".Equals(tairyoData.ステータス) || string.IsNullOrEmpty(preNm))  /* 処理待ち */
                    {
                        errorMsg = MessageUtil.Get("ME10042", "取込処理");
                        goto forcedShutdown;
                    }

                    // 都道府県マスタチェック情報
                    int todoCount = GetTodofukenCount(db, todofukenCd);
                    if (todoCount == 0)
                    {
                        errorMsg = MessageUtil.Get("ME10005", "都道府県コード");
                        goto forcedShutdown;
                    }

                    //組合等マスタチェック情報
                    int kumiCount = GetKumiaitoCount(db, todofukenCd, kumiaitoCd);
                    if (kumiCount == 0)
                    {
                        errorMsg = MessageUtil.Get("ME10005", "組合等コード");
                        goto forcedShutdown;
                    }

                    var ukeireOkList = GetOkListByUkeireRirekiId(db);
                    
                    if (ukeireOkList.IsNullOrEmpty())
                    {
                        errorMsg = MessageUtil.Get("ME10025", "取込対象");
                        goto forcedShutdown;
                    }


                    // 13. 変数の初期化
                    int errListSeq = 0;

                    for (int i = 0; i < ukeireOkList.Count; i++)
                    {
                        total++;
                        errListSeq++;
                        var record = ukeireOkList[i];
                        string 大地区コード = string.Empty;
                        string 小地区コード = string.Empty;
                        string 支所町村コード = string.Empty;

                        // 1. 処理区分（数字 1）
                        string 処理区分 = record.処理区分;
                        if (string.IsNullOrWhiteSpace(処理区分))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "処理区分", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        else if (処理区分.Length > 1)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "処理区分", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(処理区分, @"^\d+$"))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "処理区分", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                        // 2. 組合等コード（数字 3）
                        string 組合等コード = record.組合等コード;
                        if (string.IsNullOrWhiteSpace(組合等コード))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "組合等コード", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        else if (組合等コード.Length > 3)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "組合等コード", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(組合等コード, @"^\d+$"))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "組合等コード", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                        // 3. 年産（数字 4）
                        short? 年産 = record.年産;
                        if (年産 == null)
                        {
                            errorMsg = MessageUtil.Get("ME00001", "年産", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        else if (年産 > 9999)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "年産", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(年産.ToString(), @"^\d+$"))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "年産", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                        // 4. 共済目的コード（数字 2）
                        string 共済目的コード = record.共済目的コード;
                        if (string.IsNullOrWhiteSpace(共済目的コード))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "共済目的コード", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        else if (共済目的コード.Length > 2)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "共済目的コード", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(共済目的コード, @"^\d+$"))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "共済目的コード", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                        // 5. 組合員等コード（数字 13）
                        string 組合員等コード = record.組合員等コード;
                        if (string.IsNullOrWhiteSpace(組合員等コード))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "組合員等コード", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        else if (組合員等コード.Length > 13)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "組合員等コード", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(組合員等コード, @"^\d+$"))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "組合員等コード", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                        // 6. 類区分（数字 2）
                        string 類区分 = record.類区分;
                        if (string.IsNullOrWhiteSpace(類区分))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "類区分", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        else if (類区分.Length > 2)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "類区分", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(類区分, @"^\d+$"))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "類区分", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                        // 7. 引受区分（数字 2）
                        string 引受区分 = record.引受区分;
                        if (string.IsNullOrWhiteSpace(引受区分))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "引受区分", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        else if (引受区分.Length > 2)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "引受区分", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        else if (!System.Text.RegularExpressions.Regex.IsMatch(引受区分, @"^\d+$"))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "引受区分", "(" + ukeireOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                        // 8. 引受方式（数字 1）
                        string 引受方式 = record.引受方式;
                        if (!string.IsNullOrWhiteSpace(引受方式))
                        {
                            if (引受方式.Length > 1)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "引受方式", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                            else if (!System.Text.RegularExpressions.Regex.IsMatch(引受方式, @"^\d+$"))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "引受方式", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                        }

                        // 9. 特約区分（数字 1）
                        string 特約区分 = record.特約区分;
                        if (!string.IsNullOrWhiteSpace(特約区分))
                        {
                            if (特約区分.Length > 1)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "特約区分", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                            else if (!System.Text.RegularExpressions.Regex.IsMatch(特約区分, @"^\d+$"))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "特約区分", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                        }

                        // 10. 補償割合コード（数字 2）
                        string 補償割合コード = record.補償割合コード;
                        if (!string.IsNullOrWhiteSpace(補償割合コード))
                        {
                            if (補償割合コード.Length > 2)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "補償割合コード", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                            else if (!System.Text.RegularExpressions.Regex.IsMatch(補償割合コード, @"^\d+$"))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "補償割合コード", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                        }

                        // 11. 付保割合（数字 2）
                        string 付保割合 = record.付保割合;
                        if (!string.IsNullOrWhiteSpace(付保割合))
                        {
                            if (付保割合.Length > 2)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "付保割合", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                            else if (!System.Text.RegularExpressions.Regex.IsMatch(付保割合, @"^\d+$"))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "付保割合", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                        }

                        // 12. 種類区分（数字 1）
                        string 種類区分 = record.種類区分;
                        if (!string.IsNullOrWhiteSpace(種類区分))
                        {
                            if (種類区分.Length > 1)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "種類区分", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                            else if (!System.Text.RegularExpressions.Regex.IsMatch(種類区分, @"^\d+$"))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "種類区分", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                        }

                        // 13. 作付時期（数字 1）
                        string 作付時期 = record.作付時期;
                        if (!string.IsNullOrWhiteSpace(作付時期))
                        {
                            if (作付時期.Length > 1)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "作付時期", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                            else if (!System.Text.RegularExpressions.Regex.IsMatch(作付時期, @"^\d+$"))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "作付時期", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                        }

                        // 14. 田畑区分（数字 2）
                        string 田畑区分 = record.田畑区分;
                        if (!string.IsNullOrWhiteSpace(田畑区分))
                        {
                            if (田畑区分.Length > 2)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "田畑区分", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                            else if (!System.Text.RegularExpressions.Regex.IsMatch(田畑区分, @"^\d+$"))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "田畑区分", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                        }

                        // 15. 共済金額選択順位（数字 2）
                        string 共済金額選択順位 = record.共済金額選択順位;
                        if (!string.IsNullOrWhiteSpace(共済金額選択順位))
                        {
                            if (共済金額選択順位.Length > 2)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "共済金額選択順位", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                            else if (!System.Text.RegularExpressions.Regex.IsMatch(共済金額選択順位, @"^\d+$"))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "共済金額選択順位", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                        }

                        // 16. 収穫量確認方法（数字 2）
                        string 収穫量確認方法 = record.収穫量確認方法;
                        if (!string.IsNullOrWhiteSpace(収穫量確認方法))
                        {
                            if (収穫量確認方法.Length > 2)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "収穫量確認方法", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                            else if (!System.Text.RegularExpressions.Regex.IsMatch(収穫量確認方法, @"^\d+$"))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "収穫量確認方法", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                        }

                        // 17. 全相殺基準単収（数字 4）
                        decimal? 全相殺基準単収 = record.全相殺基準単収;
                        if (全相殺基準単収 != null)
                        {
                            if (全相殺基準単収 > 9999)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "全相殺基準単収", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                            else if (!System.Text.RegularExpressions.Regex.IsMatch(全相殺基準単収.ToString(), @"^\d+$"))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "全相殺基準単収", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                        }

                        // 18. 営農支払以外フラグ（数字 1）
                        string 営農支払以外フラグ = record.営農支払以外フラグ;
                        if (!string.IsNullOrWhiteSpace(営農支払以外フラグ))
                        {
                            if (営農支払以外フラグ.Length > 1)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "営農支払以外フラグ", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                            else if (!System.Text.RegularExpressions.Regex.IsMatch(営農支払以外フラグ, @"^\d+$"))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "営農支払以外フラグ", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                        }

                        // 19. 担手農家区分（数字 1）
                        string 担手農家区分 = record.担手農家区分;
                        if (!string.IsNullOrWhiteSpace(担手農家区分))
                        {
                            if (担手農家区分.Length > 1)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "担手農家区分", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                            else if (!System.Text.RegularExpressions.Regex.IsMatch(担手農家区分, @"^\d+$"))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "担手農家区分", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                        }

                        // 20. 全相殺受託者等名称（全角 30）
                        string 全相殺受託者等名称 = record.全相殺受託者等名称;
                        if (!string.IsNullOrWhiteSpace(全相殺受託者等名称))
                        {
                            if (全相殺受託者等名称.Length > 30)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "全相殺受託者等名称", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                            else if (!IsFullWidth(全相殺受託者等名称))
                            {
                                // 全角チェックエラー
                                errorMsg = MessageUtil.Get("ME10034", "全相殺受託者等名称");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                        }

                        // 21. 備考（全角 30）
                        string 備考 = record.備考;
                        if (!string.IsNullOrWhiteSpace(備考))
                        {
                            if (備考.Length > 30)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "備考", "桁数", "(" + ukeireOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true;
                                continue;
                            }
                            else if (!IsFullWidth(備考))
                            {
                                // 全角チェックエラー
                                errorMsg = MessageUtil.Get("ME10034", "備考");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                                errCount++;
                                errFlg = true; 
                                continue;
                            }
                        }
                        
                        if (!kumiaitoCd.Equals(組合等コード))
                        {
                            // 全角チェックエラー
                            errorMsg = MessageUtil.Get("ME10026", "組合等コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        
                        if (!nensan.Equals(年産.ToString()))
                        {
                            errorMsg = MessageUtil.Get("ME90015", "年産");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        if (0 == GetKyosaiMokutekiCount(db, 共済目的コード))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "共済目的コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                        if (0 == GetRuiKbnCount(db, 類区分))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "類区分");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        if (0 == GetHikiukeKbnCount(db, 共済目的コード, 引受区分))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "引受区分");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                            if (0 == GetHikiukeHousiki(db, 引受方式))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "引受方式");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                        if (0 == GetTokuyakuKbn(db, 特約区分))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "特約区分");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                        if (0 == GetHoshoWariaiMeisho(db, 補償割合コード))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "補償割合コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                        if (0 == GetShuruiKbnMeisho(db, 共済目的コード, 種類区分))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "種類区分");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                        if (0 == GetTabatakeKbnCount(db, 田畑区分))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "田畑区分");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                        if (0 == GetShukakuryoChkMeisho(db, 収穫量確認方法))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "収穫量確認方法");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                        if (0 == GetNogyoshaCount(db, kumiaitoCd, 組合員等コード))
                        {
                            errorMsg = MessageUtil.Get("ME10016", "農業者情報管理システム", "組合員等コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }

                        var dupChk = GetDuplicateCheck(db, 組合等コード, 年産.ToString(), 共済目的コード, 組合員等コード, 類区分, 引受区分);
                        // TODO:重複チェックのロジック確認必要
                        // 1ROWは必ず存在するため 1個以上の条件に引っかかる
                        if (1 <= dupChk)
                        {
                            var errJoinApp = GetDuplicateError(db, 組合等コード, 年産.ToString(), 共済目的コード, 組合員等コード, 類区分, 引受区分);
                            errorMsg = MessageUtil.Get("ME10010"
                                , "取込ファイル内"
                                , errJoinApp.行番号.ToString()
                                , $"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {共済目的コード}, 組合員等コード {組合員等コード}, 類区分 {類区分}, 引受区分 {引受区分}");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true;
                            continue;
                        }
                        var kojinRuiResult = GetKojinSettingRui(db, 組合等コード, 年産.ToString(), 共済目的コード, 組合員等コード, 類区分, 引受区分);

                        if (処理区分.Equals("1") && 0 == kojinRuiResult)
                        {
                            errorMsg = MessageUtil.Get("ME10014", "農業者情報管理システム", "対象のデータが個人設定類");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true; 
                            continue;
                        }

                        if (処理区分.Equals("2") && 1 <= kojinRuiResult)
                        {
                            errorMsg = MessageUtil.Get("ME10068", "対象のデータが個人設定類に");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true; 
                            continue;
                        }

                        if (処理区分.Equals("3") && 0 >= kojinRuiResult)
                        {
                            errorMsg = MessageUtil.Get("ME10014", "農業者情報管理システム", "対象のデータが個人設定類");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true; logger.Error(errorMsg);
                            continue;
                        }

                        if (0 == GetKojinSetting(db, 組合等コード, 年産.ToString(), 共済目的コード, 組合員等コード))
                        {
                            errorMsg = MessageUtil.Get("ME10027");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errListSeq, ukeireOkList[i].行番号, errorMsg, sysDate, db);
                            errCount++;
                            errFlg = true; logger.Error(errorMsg);
                            continue;
                        }
                    } // end for
                    

                    // --- 9. タスク終了 ---
                    if (errCount > 0)
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
                        string errorFileName = $"{preNm}-ERR-{torikomiRirekiId}.csv";
                        string tempFilePath = Path.Combine(tempFolderPath, errorFileName);
                        List<T01080大量データ受入エラーリスト> getErrorListByTorikomiId = GetErrorListByTorikomiId(db, torikomiRirekiId);

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
                        UpdateTorikomiHistory(db, torikomiRirekiId, torikomiSts, total, errCount, errorListName, errorListPath, errorListHashValue, sysDate, sysDate);
                        
                        // 10.1.5 一時領域に作成したフォルダとファイルを削除する
                        Directory.Delete(tempFolderPath, true);
                    } 
                    else
                    {
                        Delete個人設定類Records(db);
                        Insert個人設定類Records(db, sysDate, BATCH_USER_NAME);
                        Update個人設定類Records(db, sysDate);
                    }
                    batchSts = "03";
                    UpdateTorikomiRirekiFinally(db, batchSts, total, errCount, errorListName, errorListPath, errorListHashValue, sysDate, torikomiRirekiId);
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
                    if (string.IsNullOrEmpty(errorMsg))
                    {
                        errorMsg = MessageUtil.Get("MF00001");
                    }
                    
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
                    logger.Debug("yoyakuResult @@@@@@@@@@@@ ");
                    logger.Debug(bid);
                    logger.Debug(batchSts);
                    logger.Debug(errorMsg);
                    logger.Debug("yoyakuResult @@@@@@@@@@@@ ");
                    var yoyakuResult = BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), batchSts, errorMsg, BATCH_USER_NAME, ref refMessage);
                    logger.Debug("yoyakuResult yoyakuResultyoyakuResultyoyakuResult");
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

                    Environment.ExitCode = result;
                }
            }
        }
        // 1. バッチ条件テーブルからデータ取得（例：GetBatchJoukenList）
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

        private static T01070大量データ取込履歴 GetTairyoDataTorikomi(NskAppContext db, string torikomiRerekiId)
        {
            logger.Info("大量データ取込履歴テーブルからデータを取得する。");

            var record = (
                from x in db.Set<T01070大量データ取込履歴>()
                join y in db.Set<T01060大量データ受入履歴>() on x.受入履歴id equals y.受入履歴id
                where x.取込履歴id == torikomiRerekiId
                select new
                {
                    x,
                    y.取込ファイル_変更前ファイル名
                }
            ).FirstOrDefault();

            if (!string.IsNullOrEmpty(record.取込ファイル_変更前ファイル名))
            {
                preNm = record.取込ファイル_変更前ファイル名;
            }
            

            return record.x;
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


        // 4. 大量データ受入履歴の最終更新（各種件数、エラー情報、終了日時等）
        private static int UpdateTorikomiRirekiFinally(
            NskAppContext db,
            string ukeireSts,
            int taishokensuu,
            int errorCount,
            string errorListName,
            string errorListPath,
            string errorListHashValue,
            DateTime sysDate,
            string torikomiRirekiId)
        {
            var entity = db.Set<T01070大量データ取込履歴>()
                           .FirstOrDefault(x => x.受入履歴id == torikomiRirekiId);
            if (entity == null)
            {
                return 0;
            }

            entity.ステータス = ukeireSts;
            entity.対象件数 = taishokensuu;
            entity.エラー件数 = errorCount;
            entity.エラーリスト名 = errorListName;
            entity.エラーリストパス = errorListPath;
            entity.エラーリストハッシュ値 = errorListHashValue;
            entity.終了日時 = sysDate;
            entity.更新日時 = sysDate;
            entity.更新ユーザid = BATCH_USER_NAME;

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

        private static List<T19050大量データ受入組合員等類別設定ok> GetOkListByUkeireRirekiId(NskAppContext db)
        {
            return db.Set<T19050大量データ受入組合員等類別設定ok>()
                     .ToList();
        }

        // 8. m_00010_共済目的名称テーブルから件数取得
        private static int GetKyosaiMokutekiCount(NskAppContext db, string kyosaiMokutekiCd)
        {
            logger.Info($"共済目的コード {kyosaiMokutekiCd} の件数を取得します。");
            return db.Set<M00010共済目的名称>()
                     .Count(x => x.共済目的コード == kyosaiMokutekiCd);
        }

        // 9. m_00020_類名称テーブルから件数取得
        private static int GetRuiKbnCount(NskAppContext db, string ruiKbn)
        {
            logger.Info($"類区分 {ruiKbn} の件数を取得します。");
            return db.Set<M00020類名称>()
                     .Count(x => x.類区分 == ruiKbn);
        }

        private static int Delete個人設定類Records(NskAppContext db)
        {
            logger.Info("t_11030_個人設定類 の対象レコードを削除します。");

            // t_11030_個人設定類 と t_19xxx_大量データ受入OK のエンティティクラス名は実際のものに合わせてください
            var query = db.Set<T11030個人設定類>()
                .Where(t => db.Set<T19050大量データ受入組合員等類別設定ok>()
                    .Any(ok => t.組合等コード == ok.組合等コード &&
                               t.年産 == ok.年産 &&
                               t.共済目的コード == ok.共済目的コード &&
                               t.組合員等コード == ok.組合員等コード &&
                               t.類区分 == ok.類区分 &&
                               t.引受区分 == ok.引受区分 &&
                               ok.処理区分 == "1"));

            return query.ExecuteDelete();
        }

        private static int Insert個人設定類Records(NskAppContext db, DateTime systemDate, string systemUserId)
        {
            logger.Info("t_11030_個人設定類 へのレコード追加処理を実行します。");

            // ソースのエンティティ（t_19xxx_大量データ取込 組合員等類別設定大量データ取込OK）のクラス名は実際のものに合わせてください
            // また、処理区分「2:新規」は数値 2 として扱う前提です。
            var recordsToInsert = db.Set<T19050大量データ受入組合員等類別設定ok>()
                .Where(ok => ok.処理区分 == "2")
                .Select(ok => new T11030個人設定類
                {
                    組合等コード = ok.組合等コード,
                    年産 = ok.年産 ?? 0,
                    共済目的コード = ok.共済目的コード,
                    組合員等コード = ok.組合員等コード,
                    類区分 = ok.類区分,
                    引受区分 = ok.引受区分,
                    引受方式 = ok.引受方式,
                    特約区分 = ok.特約区分,
                    補償割合コード = ok.補償割合コード,
                    付保割合 = string.IsNullOrEmpty(ok.付保割合) ? 0 : decimal.Parse(ok.付保割合),
                    種類区分 = ok.種類区分,
                    作付時期 = ok.作付時期,
                    田畑区分 = ok.田畑区分,
                    共済金額選択順位 = string.IsNullOrEmpty(ok.共済金額選択順位) ? 0 : decimal.Parse(ok.共済金額選択順位),
                    収穫量確認方法 = ok.収穫量確認方法,
                    全相殺基準単収 = ok.全相殺基準単収,
                    営農対象外フラグ = ok.営農支払以外フラグ,
                    担手農家区分 = ok.担手農家区分,
                    全相殺受託者等名称 = ok.全相殺受託者等名称,
                    備考 = ok.備考,
                    登録日時 = systemDate,
                    登録ユーザid = systemUserId,
                    更新日時 = systemDate,
                    更新ユーザid = systemUserId
                })
                .ToList();

            db.AddRange(recordsToInsert);
            return db.SaveChanges();
        }

        private static int Update個人設定類Records(NskAppContext db, DateTime systemDate)
        {
            logger.Info("EF Core による t_11030_個人設定類 の更新処理を実行します。");

            // JOIN により対象となるレコードを抽出
            var query = from t in db.Set<T11030個人設定類>()
                        join ok in db.Set<T19050大量データ受入組合員等類別設定ok>()
                          on new
                          {
                              t.組合等コード,
                              年産 = t.年産, 
                              t.共済目的コード,
                              t.組合員等コード,
                              t.類区分,
                              t.引受区分
                          }
                          equals new
                          {
                              ok.組合等コード,
                              年産 = ok.年産.Value,
                              ok.共済目的コード,
                              ok.組合員等コード,
                              ok.類区分,
                              ok.引受区分
                          }
                        where ok.処理区分 == "3"
                        select new { t, ok };

            // 対象レコードを取得して更新
            var list = query.ToList();
            foreach (var item in list)
            {
                item.t.引受方式 = item.ok.引受方式;
                item.t.特約区分 = item.ok.特約区分;
                item.t.補償割合コード = item.ok.補償割合コード;
                item.t.付保割合 = decimal.Parse(item.ok.付保割合);
                item.t.種類区分 = item.ok.種類区分;
                item.t.作付時期 = item.ok.作付時期;
                item.t.田畑区分 = item.ok.田畑区分;
                item.t.共済金額選択順位 = decimal.Parse(item.ok.共済金額選択順位);
                item.t.収穫量確認方法 = item.ok.収穫量確認方法;
                item.t.全相殺基準単収 = item.ok.全相殺基準単収;
                item.t.営農対象外フラグ = item.ok.営農支払以外フラグ;
                item.t.担手農家区分 = item.ok.担手農家区分;
                item.t.全相殺受託者等名称 = item.ok.全相殺受託者等名称;
                item.t.備考 = item.ok.備考;
                item.t.更新日時 = systemDate;
                item.t.更新ユーザid = BATCH_USER_NAME;
            }

            // SaveChanges により、更新内容をデータベースに反映
            return db.SaveChanges();
        }

        // 引受区分名称の件数取得
        private static int GetHikiukeKbnCount(NskAppContext db, string kyosaiMokutekiCd, string hikiukeKbn)
        {
            logger.Info($"共済目的コード {kyosaiMokutekiCd} と引受区分 {hikiukeKbn} の件数を取得します。");
            // 仮に m_10090_引受区分名称 に対応するエンティティを HikiukeKbn とする
            return db.Set<M10090引受区分名称>()
                     .Count(x => x.共済目的コード == kyosaiMokutekiCd && x.引受区分 == hikiukeKbn);
        }

        private static int GetHikiukeHousiki(NskAppContext db, string hikiukeHousiki)
        {
            logger.Info($"引受方式 {hikiukeHousiki} のデータを取得します。");
            return db.Set<M10080引受方式名称>()
                     .Count(x => x.引受方式 == hikiukeHousiki);
        }

        private static int GetTokuyakuKbn(NskAppContext db, string tokuyakuKbn)
        {
            logger.Info($"特約区分 {tokuyakuKbn} のデータを取得します。");
            return db.Set<M10100特約区分名称>()
                     .Count(x => x.特約区分 == tokuyakuKbn);
        }

        private static int GetHoshoWariaiMeisho(NskAppContext db, string hoshoWariaiCd)
        {
            logger.Info($"補償割合コード {hoshoWariaiCd} のデータを取得します。");
            return db.Set<M20030補償割合名称>()
                     .Count(x => x.補償割合コード == hoshoWariaiCd);
        }

        private static int GetShuruiKbnMeisho(NskAppContext db, string kyosaimokutekiCd, string shuruiKbn)
        {
            logger.Info($"種類区分ko-da {kyosaimokutekiCd} のデータを取得します。");
            return db.Set<M10130種類区分名称>()
                     .Count(x => x.共済目的コード == kyosaimokutekiCd &&
                                 x.種類区分 == shuruiKbn);
        }

        // 10. m_00040_田畑名称テーブルから件数取得
        private static int GetTabatakeKbnCount(NskAppContext db, string tabatakeKbn)
        {
            logger.Info($"田畑区分 {tabatakeKbn} の件数を取得します。");
            return db.Set<M00040田畑名称>()
                     .Count(x => x.田畑区分 == tabatakeKbn);
        }

        private static int GetShukakuryoChkMeisho(NskAppContext db, string shukakuryoKakuninHouho)
        {
            logger.Info($"収穫量確認方法 {shukakuryoKakuninHouho} の件数を取得します。");
            return db.Set<M00070収穫量確認方法名称>()
                     .Count(x => x.収穫量確認方法 == shukakuryoKakuninHouho);
        }


        // 20. t_nogyoshaテーブルから件数取得（FimContextを使用）
        private static int GetNogyoshaCount(NskAppContext db, string kumiaitoCd, string kumiaiintoCd)
        {
            logger.Info($"組合員コード {kumiaitoCd}, 組合員等コード {kumiaiintoCd} の件数を取得します。");
            return db.Set<VNogyosha>()
                     .Count(x => x.KumiaitoCd == kumiaitoCd &&
                                 x.KumiaiintoCd == kumiaiintoCd);
        }

        private static int GetKojinSettingRui(
            NskAppContext db
            ,string kumiaitoCd
            ,string nensan
            ,string kyosaimokutekiCd
            ,string kumiaiintoCd
            ,string ruiKbn
            ,string hikiukeKbn)
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

        private static T19050大量データ受入組合員等類別設定ok GetDuplicateError(
            NskAppContext db
            , string kumiaitoCd
            , string nensan
            , string kyosaimokutekiCd
            , string kumiaiintoCd
            , string ruiKbn
            , string hikiukeKbn)
        {
            return db.Set<T19050大量データ受入組合員等類別設定ok>()
                     .Where(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd &&
                                 x.類区分 == ruiKbn &&
                                 x.引受区分 == hikiukeKbn)
                     .FirstOrDefault();
        }

        private static int GetDuplicateCheck(
            NskAppContext db
            , string kumiaitoCd
            , string nensan
            , string kyosaimokutekiCd
            , string kumiaiintoCd
            , string ruiKbn
            , string hikiukeKbn)
        {
            return db.Set<T19050大量データ受入組合員等類別設定ok>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd &&
                                 x.類区分 == ruiKbn &&
                                 x.引受区分 == hikiukeKbn);
        }

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


        // 21. t_01080_大量データ受入_エラーリストテーブルから該当データ取得
        private static List<T01080大量データ受入エラーリスト> GetErrorListByTorikomiId(NskAppContext db, string torikomiId)
        {
            logger.Info($"処理区分 2, 履歴ID {torikomiId} のデータを取得します。");
            return db.Set<T01080大量データ受入エラーリスト>()
                     .Where(x => x.処理区分 == "2" && x.履歴id == long.Parse(torikomiId))
                     .AsNoTracking()
                     .ToList();
        }
        private static int UpdateTorikomiHistory(
            NskAppContext db,
            string torikomiRirekiId,
            string torikomiSts,
            int dataCount,
            int errorCount,
            string errorListName,
            string errorListPass,
            string errorListValue,
            DateTime systemDate,
            DateTime sysdateDate)
        {
            logger.Info($"取込履歴ID {torikomiRirekiId} のデータを更新します。");
            var entity = db.Set<T01070大量データ取込履歴>()
                           .FirstOrDefault(x => x.取込履歴id == torikomiRirekiId);
            if (entity == null)
            {
                return 0;
            }

            entity.ステータス = torikomiSts;
            entity.対象件数 = dataCount;
            entity.エラー件数 = errorCount;
            entity.エラーリスト名 = errorListName;
            entity.エラーリストパス = errorListPass;
            entity.エラーリストハッシュ値 = errorListValue;
            entity.終了日時 = systemDate;
            entity.更新日時 = sysdateDate;
            entity.更新ユーザid = BATCH_USER_NAME;

            return db.SaveChanges();
        }

        private static VNogyosha GetNogyoshaByCodes(NskAppContext db, string kumiaitoCd, string kumiaiintoCd)
        {
            logger.Info($"組合員コード {kumiaitoCd}, 組合員等コード {kumiaiintoCd} のレコードを取得します。");

            return db.Set<VNogyosha>()
                     .Where(x => x.KumiaitoCd == kumiaitoCd &&
                                 x.KumiaiintoCd == kumiaiintoCd)
                     .FirstOrDefault();
        }

        private static void AddError(
            string torikomiRirekiId
            ,int errListSeq
            ,decimal 行番号
            ,string dataError
            ,DateTime sysDate
            ,NskAppContext db )
        {
            var dataErrorList = new T01080大量データ受入エラーリスト
            {
                処理区分 = "2",
                履歴id = long.Parse(torikomiRirekiId),
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

        /// <summary>
        /// 文字列が全角かどうかを簡易的にチェックするメソッド
        /// ※ 各文字のコードが 0xFF 以上なら全角とみなす（実際の判定は要件に合わせて調整してください）
        /// </summary>
        private static bool IsFullWidth(string input)
        {
            foreach (char c in input)
            {
                if (c < 0xFF)
                    return false;
            }
            return true;
        }

        public class ErrorList
        {
            public string エラー内容 { get; set; }
            public decimal 行番号 { get; set; }
        }
    }
}
