using NSK_B112190.Common;
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
using CoreLibrary.Core.Exceptions;
using System.IO.Compression;


namespace NSK_B112190
{
    /// <summary>
    /// 定時実行予約登録
    /// </summary>
    class Program
    {
        /// <summary>
        /// バッチ名
        /// </summary>
        private static string BATCH_NAME = "加入申込書大量受入データ取込";
        private static string BATCH_USER_NAME = "NSK_112190B";

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
            logger.Info(string.Concat(CoreConst.LOG_START_KEYWORD, " 加入申込書大量受入データ取込"));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //ConfigUtil.Get(

            var bid = string.Empty;
            var todofukenCd = string.Empty;
            var kumiaitoCd = string.Empty;
            var shishoCd = string.Empty;
            var jid = string.Empty;

            var nensan = string.Empty;
            var torikomiRirekiId = string.Empty;
            var filePath = string.Empty;

            var batchSts = "99";

            var torikomiRirekiSts = "失敗";
            var kumiaitoCdToriko = string.Empty;
            var shoriSts = "99";
            var errorMsg = string.Empty;

            int okCount = 0;
            int errCount = 0;
            var dataCount = 0;
            var errorListName = string.Empty;
            var errorListPath = string.Empty;
            var errorListHashValue = string.Empty;
            var tempErrorFolderPath = string.Empty;
            var okListName = string.Empty;
            var okListPath = string.Empty;
            var okListHashValue = string.Empty;

            var kyosaimokutekiCd = string.Empty;
            var haniParam1 = string.Empty;
            var haniParam2 = string.Empty;
            var haniParam3 = string.Empty;
            var chushutuKbn = string.Empty;
            var torikomiSts = string.Empty;

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
                { // バッチ条件情報取得
                    List<T01050バッチ条件> joukenList = GetBatchJoukenList(db, jid, Constants.JOUKEN_NENSAN, Constants.JOUKEN_FILE_PATH, Constants.JOUKEN_TORIKOMIRIREKI_ID);
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
                    // 取込履歴取得

                    T01070大量データ取込履歴 tairyoData = GetTairyoDataTorikomi(db, torikomiRirekiId);

                    if (tairyoData == null || !"01".Equals(tairyoData.ステータス))  /* 処理待ち */
                    {
                        errorMsg = MessageUtil.Get("ME10042", "取込処理");
                        goto forcedShutdown;
                    }

                    //取込ステータス、組合等コード
                    var ukeireRirekiId = tairyoData.受入履歴id;
                    torikomiSts = tairyoData.ステータス;
                    var kumiaitoCdTorikomi = tairyoData.組合等コード;
                    torikomiSts = "02";
                    UpdateT01070TorikomiRireki(db, tairyoData.取込履歴id, torikomiSts, sysDate, BATCH_USER_NAME);

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

                    if (!kumiaitoCd.Equals(kumiaitoCdTorikomi))
                    {
                        errorMsg = MessageUtil.Get("ME10005", "組合等コード");
                        goto forcedShutdown;
                    }

                    int shishoCount = GetShishoCount(db, todofukenCd, kumiaitoCd, shishoCd);
                    if (shishoCount == 0)
                    {
                        errorMsg = MessageUtil.Get("ME10005", "支所コード");
                        goto forcedShutdown;
                    }

                    // 受入OKテーブルからデータを取得
                    List<T19070大量データ受入加入申込書ok> tairyoOkList = GetUkeireAppFormData(db, ukeireRirekiId);
                    if (tairyoOkList.IsNullOrEmpty())
                    {
                        errorMsg = MessageUtil.Get("ME10025", "取込");
                        goto forcedShutdown;
                    }

                    int errSeq = 0;
                    for (int i = 0; i < tairyoOkList.Count; i++)
                    {
                        dataCount++;
                        errSeq++;
                        errorMsg = "";
                        var nensanHani = tairyoOkList[i].年産範囲;
                        var kyosaimokutekiHani = tairyoOkList[i].共済目的コード範囲;
                        chushutuKbn = tairyoOkList[i].抽出区分;
                        haniParam1 = tairyoOkList[i].範囲パラメータ１;
                        haniParam2 = tairyoOkList[i].範囲パラメータ２;
                        haniParam3 = tairyoOkList[i].範囲パラメータ３;
                        kyosaimokutekiCd = tairyoOkList[i].共済目的コード;

                        string 大地区コード = string.Empty;
                        string 小地区コード = string.Empty;
                        var 支所町村コード = string.Empty;

                        if (!"1".Equals(chushutuKbn) && string.IsNullOrEmpty(haniParam1))
                        {
                            errorMsg = MessageUtil.Get("ME10012", "抽出区分", "範囲パラメータ");
                            logger.Error(errorMsg);
                            goto forcedShutdown;
                        }
                        if (!nensan.Equals(nensanHani.ToString()))
                        {
                            errorMsg = "年産が不正です。ポータル画面選択された年産と一致していません。";
                            logger.Error(errorMsg);
                            goto forcedShutdown;
                        }

                        #region データチェック
                        // 例：受入履歴ID（数字、文字数19）
                        if (tairyoOkList[i].受入履歴id.ToString().Length > 19)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "受入履歴ID", "19", "(" + tairyoOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }
                        if (!long.TryParse(tairyoOkList[i].受入履歴id.ToString(), out _))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "受入履歴ID", "(" + tairyoOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        // 行番号（数字、文字数6）
                        if (tairyoOkList[i].行番号.ToString().Length > 6)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "行番号", "6", "(" + tairyoOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }
                        if (!int.TryParse(tairyoOkList[i].行番号.ToString(), out _))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "行番号", "(" + tairyoOkList[i].行番号 + "行目)");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }


                        // 範囲（文字列、文字数2）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].範囲))
                        {
                            if (tairyoOkList[i].範囲.Length > 2)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "範囲", "2", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 年産範囲（数字、文字数4）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].年産範囲.ToString()))
                        {
                            if (tairyoOkList[i].年産範囲.ToString().Length > 4)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "年産範囲", "4", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].年産範囲.ToString(), out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "年産範囲", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 共済目的コード範囲（数字、文字数2）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].共済目的コード範囲))
                        {
                            if (tairyoOkList[i].共済目的コード範囲.Length > 2)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "共済目的コード範囲", "2", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].共済目的コード範囲, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "共済目的コード範囲", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 抽出区分（数字、文字数1）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].抽出区分))
                        {
                            if (tairyoOkList[i].抽出区分.Length > 1)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "抽出区分", "1", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].抽出区分, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "抽出区分", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 範囲パラメータ１（数字、文字数13）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].範囲パラメータ１))
                        {
                            if (tairyoOkList[i].範囲パラメータ１.Length > 13)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "範囲パラメータ１", "13", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!long.TryParse(tairyoOkList[i].範囲パラメータ１, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "範囲パラメータ１", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 範囲パラメータ２（文字列、文字数13）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].範囲パラメータ２))
                        {
                            if (tairyoOkList[i].範囲パラメータ２.Length > 13)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "範囲パラメータ２", "13", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 範囲パラメータ３（数字、文字数4）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].範囲パラメータ３))
                        {
                            if (tairyoOkList[i].範囲パラメータ３.Length > 4)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "範囲パラメータ３", "4", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].範囲パラメータ３, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "範囲パラメータ３", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 日付（日時）
                        if (tairyoOkList[i].日付.HasValue)
                        {
                            if (!DateTime.TryParse(tairyoOkList[i].日付.ToString(), out _))
                            {
                                errorMsg = MessageUtil.Get("ME80013", "日付", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // GISデータ出力のタイプ（数字、文字数1）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].gisデータ出力のタイプ))
                        {
                            if (tairyoOkList[i].gisデータ出力のタイプ.Length > 1)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "GISデータ出力のタイプ", "1", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].gisデータ出力のタイプ, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "GISデータ出力のタイプ", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 共済目的コード（数字、文字数2）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].共済目的コード))
                        {
                            if (tairyoOkList[i].共済目的コード.Length > 2)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "共済目的コード", "2", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].共済目的コード, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "共済目的コード", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 組合員等コード（数字、文字数13）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].組合員等コード))
                        {
                            if (tairyoOkList[i].組合員等コード.Length > 13)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "組合員等コード", "13", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!long.TryParse(tairyoOkList[i].組合員等コード, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "組合員等コード", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 耕地番号（数字、文字数5）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].耕地番号))
                        {
                            if (tairyoOkList[i].耕地番号.Length > 5)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "耕地番号", "5", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].耕地番号, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "耕地番号", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 分筆番号（数字、文字数4）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].分筆番号))
                        {
                            if (tairyoOkList[i].分筆番号.Length > 4)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "分筆番号", "4", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].分筆番号, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "分筆番号", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 類区分（数字、文字数2）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].類区分))
                        {
                            if (tairyoOkList[i].類区分.Length > 2)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "類区分", "2", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].類区分, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "類区分", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 地名地番（数字、文字数40）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].地名地番))
                        {
                            if (tairyoOkList[i].地名地番.Length > 40)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "地名地番", "40", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!long.TryParse(tairyoOkList[i].地名地番, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "地名地番", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 耕地面積（数字、文字数4）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].耕地面積.ToString()))
                        {
                            if (tairyoOkList[i].耕地面積.ToString().Length > 4)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "耕地面積", "4", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!decimal.TryParse(tairyoOkList[i].耕地面積.ToString(), out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "耕地面積", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 引受面積（数字、文字数4）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].引受面積.ToString()))
                        {
                            if (tairyoOkList[i].引受面積.ToString().Length > 4)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "引受面積", "4", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!decimal.TryParse(tairyoOkList[i].引受面積.ToString(), out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "引受面積", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 転作等面積（数字、文字数4）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].転作等面積.ToString()))
                        {
                            if (tairyoOkList[i].転作等面積.ToString().Length > 4)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "転作等面積", "4", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!decimal.TryParse(tairyoOkList[i].転作等面積.ToString(), out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "転作等面積", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 受委託区分（数字、文字数1）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].受委託区分))
                        {
                            if (tairyoOkList[i].受委託区分.Length > 1)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "受委託区分", "1", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].受委託区分, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "受委託区分", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 備考（文字列、文字数40）※最大長チェック
                        if (!string.IsNullOrEmpty(tairyoOkList[i].備考))
                        {
                            if (tairyoOkList[i].備考.Length > 40)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "備考", "40", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 田畑区分（数字、文字数1）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].田畑区分))
                        {
                            if (tairyoOkList[i].田畑区分.Length > 1)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "田畑区分", "1", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].田畑区分, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "田畑区分", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 区分コード（数字、文字数2）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].区分コード))
                        {
                            if (tairyoOkList[i].区分コード.Length > 2)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "区分コード", "2", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].区分コード, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "区分コード", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 種類コード（数字、文字数2）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].種類コード))
                        {
                            if (tairyoOkList[i].種類コード.Length > 2)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "種類コード", "2", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].種類コード, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "種類コード", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 品種コード（数字、文字数3）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].品種コード))
                        {
                            if (tairyoOkList[i].品種コード.Length > 3)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "品種コード", "3", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].品種コード, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "品種コード", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 収量等級コード（数字、文字数3）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].収量等級コード))
                        {
                            if (tairyoOkList[i].収量等級コード.Length > 3)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "収量等級コード", "3", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].収量等級コード, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "収量等級コード", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 参酌コード（数字、文字数3）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].参酌コード))
                        {
                            if (tairyoOkList[i].参酌コード.Length > 3)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "参酌コード", "3", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].参酌コード, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "参酌コード", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 基準単収（数字、文字数4）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].基準単収.ToString()))
                        {
                            if (tairyoOkList[i].基準単収.ToString().Length > 4)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "基準単収", "4", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!decimal.TryParse(tairyoOkList[i].基準単収.ToString(), out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "基準単収", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 基準収穫量（数字、文字数7）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].基準収穫量.ToString()))
                        {
                            if (tairyoOkList[i].基準収穫量.ToString().Length > 7)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "基準収穫量", "7", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!decimal.TryParse(tairyoOkList[i].基準収穫量.ToString(), out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "基準収穫量", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 修正日付（日時）
                        if (tairyoOkList[i].修正日付.HasValue)
                        {
                            if (!DateTime.TryParse(tairyoOkList[i].修正日付.ToString(), out _))
                            {
                                errorMsg = MessageUtil.Get("ME80013", "修正日付", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 計算日付（日時）
                        if (tairyoOkList[i].計算日付.HasValue)
                        {
                            if (!DateTime.TryParse(tairyoOkList[i].計算日付.ToString(), out _))
                            {
                                errorMsg = MessageUtil.Get("ME80013", "計算日付", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 年産（数字、文字数4）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].年産.ToString()))
                        {
                            if (tairyoOkList[i].年産.ToString().Length > 4)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "年産", "4", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].年産.ToString(), out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "年産", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 実量基準単収（数字、文字数4）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].実量基準単収.ToString()))
                        {
                            if (tairyoOkList[i].実量基準単収.ToString().Length > 4)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "実量基準単収", "4", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!decimal.TryParse(tairyoOkList[i].実量基準単収.ToString(), out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "実量基準単収", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // ＲＳ区分（数字、文字数2）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].rs区分))
                        {
                            if (tairyoOkList[i].rs区分.Length > 2)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "ＲＳ区分", "2", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].rs区分, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "ＲＳ区分", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 局都道府県コード（数字、文字数4）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].局都道府県コード))
                        {
                            if (tairyoOkList[i].局都道府県コード.Length > 4)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "局都道府県コード", "4", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].局都道府県コード, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "局都道府県コード", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 市区町村コード（数字、文字数3）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].市区町村コード))
                        {
                            if (tairyoOkList[i].市区町村コード.Length > 3)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "市区町村コード", "3", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].市区町村コード, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "市区町村コード", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 大字コード（数字、文字数8）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].大字コード))
                        {
                            if (tairyoOkList[i].大字コード.Length > 8)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "大字コード", "8", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].大字コード, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "大字コード", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 小字コード（数字、文字数4）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].小字コード))
                        {
                            if (tairyoOkList[i].小字コード.Length > 4)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "小字コード", "4", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].小字コード, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "小字コード", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 地番（文字列、文字数16）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].地番))
                        {
                            if (tairyoOkList[i].地番.Length > 16)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "地番", "16", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 枝番（文字列、文字数14）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].枝番))
                        {
                            if (tairyoOkList[i].枝番.Length > 14)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "枝番", "14", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 子番（文字列、文字数10）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].子番))
                        {
                            if (tairyoOkList[i].子番.Length > 10)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "子番", "10", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 孫番（文字列、文字数10）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].孫番))
                        {
                            if (tairyoOkList[i].孫番.Length > 10)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "孫番", "10", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 統計市町村コード（数字、文字数5）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].統計市町村コード))
                        {
                            if (tairyoOkList[i].統計市町村コード.Length > 5)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "統計市町村コード", "5", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].統計市町村コード, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "統計市町村コード", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 統計地域コード（数字、文字数5）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].統計単位地域コード))
                        {
                            if (tairyoOkList[i].統計単位地域コード.Length > 5)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "統計単位地域コード", "5", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].統計単位地域コード, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "統計地域コード", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 統計単収（数字、文字数4）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].統計単収.ToString()))
                        {
                            if (tairyoOkList[i].統計単収.ToString().Length > 4)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "統計単収", "4", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!decimal.TryParse(tairyoOkList[i].統計単収.ToString(), out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "統計単収", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 用途区分（数字、文字数3）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].用途区分))
                        {
                            if (tairyoOkList[i].用途区分.Length > 3)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "用途区分", "3", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].用途区分, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "用途区分", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 産地銘柄コード（数字、文字数3）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].産地別銘柄コード))
                        {
                            if (tairyoOkList[i].産地別銘柄コード.Length > 3)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "産地別銘柄コード", "3", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!int.TryParse(tairyoOkList[i].産地別銘柄コード, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "産地別銘柄コード", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        // 受委託者コード（数字、文字数13）
                        if (!string.IsNullOrEmpty(tairyoOkList[i].受委託者コード))
                        {
                            if (tairyoOkList[i].受委託者コード.Length > 13)
                            {
                                errorMsg = MessageUtil.Get("ME00020", "受委託者コード", "13", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (!long.TryParse(tairyoOkList[i].受委託者コード, out _))
                            {
                                errorMsg = MessageUtil.Get("ME00003", "受委託者コード", "(" + tairyoOkList[i].行番号 + "行目)");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }
                        // ここまでが属性チェックの全項目
                        #endregion

                        if (0 == GetKyosaiMokutekiCount(db, tairyoOkList[i].共済目的コード))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "組合等コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if (0 == GetRuiKbnCount(db, tairyoOkList[i].類区分))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "類区分");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if (0 == GetTabatakeCount(db, tairyoOkList[i].田畑区分))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "田畑区分");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if (0 == GetKubunCount(db, kumiaitoCd, nensan, tairyoOkList[i].共済目的コード, tairyoOkList[i].区分コード))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "区分コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if (0 == GetShuruiCount(db, tairyoOkList[i].共済目的コード, tairyoOkList[i].種類コード))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "種類コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if (0 == GetYoutoKbnCount(db, tairyoOkList[i].共済目的コード, tairyoOkList[i].用途区分))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "用途区分");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if (0 == GetHinshuKeisuuCount(db, kumiaitoCd, nensan, tairyoOkList[i].共済目的コード, tairyoOkList[i].品種コード))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "品種コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if (0 == GetSanchiBetuMeigaraCount(db, kumiaitoCd, nensan, tairyoOkList[i].共済目的コード, tairyoOkList[i].産地別銘柄コード))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "産地別銘柄コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if (0 == GetShuryoToukyuCount(db, kumiaitoCd, nensan, tairyoOkList[i].共済目的コード, tairyoOkList[i].類区分, shishoCd, tairyoOkList[i].収量等級コード))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "収量等級コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if (0 == GetSansyakuKeisuuCount(db, kumiaitoCd, nensan, tairyoOkList[i].共済目的コード, tairyoOkList[i].参酌コード))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "参酌コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if (0 == GetToukeitaniChiikiCount(db, kumiaitoCd, nensan, tairyoOkList[i].共済目的コード, tairyoOkList[i].統計単位地域コード))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "統計単位地域コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }
                        // 農業者チェック情報
                        if (0 == GetNogyoshaCount(db, kumiaitoCd, tairyoOkList[i].組合員等コード))
                        {
                            errorMsg = MessageUtil.Get("ME10016", "農業者情報管理システム", "組合員等コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }
                        VNogyosha tnogyosya = GetNogyoshaByCodes(db, kumiaitoCd, tairyoOkList[i].組合員等コード);
                        大地区コード = tnogyosya.DaichikuCd;
                        小地区コード = tnogyosya.ShochikuCd;
                        支所町村コード = tnogyosya.ShichosonCd;

                        if (0 == tairyoOkList[i].引受面積 && 0 == tairyoOkList[i].転作等面積)
                        {
                            errorMsg = MessageUtil.Get("ME10011", "引受面積", "転作等面積");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if (0 < tairyoOkList[i].引受面積 && 0 < tairyoOkList[i].転作等面積)
                        {
                            errorMsg = MessageUtil.Get("ME10013", "引受面積", "転作等面積");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if (tairyoOkList[i].引受面積 > tairyoOkList[i].耕地面積)
                        {
                            errorMsg = MessageUtil.Get("ME10022", "耕地面積", "引受面積");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if (tairyoOkList[i].転作等面積 > tairyoOkList[i].耕地面積)
                        {
                            errorMsg = MessageUtil.Get("ME10022", "耕地面積", "転作等面積");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if ((tairyoOkList[i].引受面積 + tairyoOkList[i].転作等面積) > tairyoOkList[i].耕地面積)
                        {
                            errorMsg = "転作等面積と引受面積の合計が耕地面積を超えています。";
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }
                        

                        if ("0".Equals(tairyoOkList[i].区分コード) && 0 == tairyoOkList[i].引受面積)
                        {
                            errorMsg = MessageUtil.Get("ME90015", "区分コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if ("0".Equals(tairyoOkList[i].区分コード) && 0 < tairyoOkList[i].転作等面積)
                        {
                            errorMsg = MessageUtil.Get("ME90015", "区分コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        if ("3".Equals(tairyoOkList[i].区分コード) && 0 < tairyoOkList[i].転作等面積)
                        {
                            errorMsg = MessageUtil.Get("ME90015", "区分コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }
                        if ("3".Equals(tairyoOkList[i].区分コード) && 0 < tairyoOkList[i].引受面積)
                        {
                            errorMsg = MessageUtil.Get("ME90015", "区分コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        List<M00030区分名称> kbnMeishoList = GetKubunMeishouRecords(db, kumiaitoCd, nensan, tairyoOkList[i].共済目的コード, tairyoOkList[i].区分コード);

                        List<M10140種類名称> shuruiMeishoList = GetShuruiMeishouRecords(db, tairyoOkList[i].共済目的コード);
                        bool gotoFlg = false;
                        for (int j = 0; j < kbnMeishoList.Count; j++)
                        {
                            for (int k = 0; k < shuruiMeishoList.Count; k++)
                            {
                                if ("2".Equals(kbnMeishoList[j].エラータイプ) && tairyoOkList[i].種類コード.Equals(shuruiMeishoList[k].共済目的コード))
                                {
                                    errorMsg = "区分と種類の組合せが誤っています。引受け可能な種類を指定する事は出来ません。";
                                    logger.Error(errorMsg);
                                    AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                    errCount++;
                                    gotoFlg = true;
                                    goto ExitLoops;
                                }
                                if ("3".Equals(kbnMeishoList[j].エラータイプ) && !tairyoOkList[i].種類コード.Equals(shuruiMeishoList[k].共済目的コード))
                                {
                                    errorMsg = "区分と種類の組合せが誤っています。引受け可能な種類を指定する事は出来ません。";
                                    logger.Error(errorMsg);
                                    AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                    errCount++;
                                    gotoFlg = true;
                                    goto ExitLoops;
                                }
                                if ("0".Equals(tairyoOkList[i].区分コード) && "0".Equals(tairyoOkList[i].収量等級コード))
                                {
                                    errorMsg = MessageUtil.Get("ME10008", "収量等級");
                                    logger.Error(errorMsg);
                                    AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                    errCount++;
                                    gotoFlg = true;
                                    goto ExitLoops;
                                }
                                if ("1".Equals(kbnMeishoList[j].参酌フラグ) && "0".Equals(tairyoOkList[i].参酌コード))
                                {
                                    errorMsg = MessageUtil.Get("ME10012", "区分", "参酌コード");
                                    logger.Error(errorMsg);
                                    AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                    errCount++;
                                    gotoFlg = true;
                                    goto ExitLoops;
                                }
                                if ("20".Equals(tairyoOkList[i].共済目的コード) && 0 < tairyoOkList[i].引受面積 && !"2".Equals(tairyoOkList[i].田畑区分))
                                {
                                    errorMsg = MessageUtil.Get("ME10007", "田畑区分");
                                    logger.Error(errorMsg);
                                    AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                    errCount++;
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

                        if (string.IsNullOrEmpty(tairyoOkList[i].共済目的コード範囲))
                        {
                            errorMsg = MessageUtil.Get("ME10004", "共済目的コード");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }
                        if (!nensan.Equals(tairyoOkList[i].年産範囲.ToString()))
                        {
                            errorMsg = MessageUtil.Get("ME10004", "年産");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }
                        if ("2".Equals(tairyoOkList[i].抽出区分))
                        {
                            if (string.IsNullOrEmpty(tairyoOkList[i].範囲パラメータ２) && int.Parse(tairyoOkList[i].範囲パラメータ１) > int.Parse(tairyoOkList[i].組合員等コード))
                            {
                                errorMsg = MessageUtil.Get("ME10004", "組合員等コード");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (string.IsNullOrEmpty(tairyoOkList[i].範囲パラメータ２) &&
                                (int.Parse(tairyoOkList[i].範囲パラメータ１) > int.Parse(tairyoOkList[i].組合員等コード) || int.Parse(tairyoOkList[i].組合員等コード) < int.Parse(tairyoOkList[i].範囲パラメータ２)))
                            {
                                errorMsg = MessageUtil.Get("ME10004", "組合員等コード");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }
                        if ("3".Equals(tairyoOkList[i].抽出区分))
                        {
                            if (!tairyoOkList[i].範囲パラメータ１.Equals(大地区コード))
                            {
                                errorMsg = MessageUtil.Get("ME10004", "大地区コード");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (string.IsNullOrEmpty(tairyoOkList[i].範囲パラメータ２) && int.Parse(tairyoOkList[i].範囲パラメータ２) > int.Parse(小地区コード))
                            {
                                errorMsg = MessageUtil.Get("ME10004", "小地区コード");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                            if (string.IsNullOrEmpty(tairyoOkList[i].範囲パラメータ３) && int.Parse(小地区コード) > int.Parse(tairyoOkList[i].範囲パラメータ３))
                            {
                                errorMsg = MessageUtil.Get("ME10004", "小地区コード");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }
                        if ("4".Equals(tairyoOkList[i].抽出区分))
                        {
                            if (!tairyoOkList[i].範囲パラメータ１.Equals(支所町村コード))
                            {
                                errorMsg = MessageUtil.Get("ME10004", "支所町村コード");
                                logger.Error(errorMsg);
                                AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                                errCount++;
                                continue;
                            }
                        }

                        if (1 <= GetJoinApplicationOkCount(db,ukeireRirekiId, tairyoOkList[i].行番号 , nensan, tairyoOkList[i].共済目的コード, tairyoOkList[i].組合員等コード, tairyoOkList[i].耕地番号, tairyoOkList[i].分筆番号))
                        {
                            var errJoinApp = GetDuplicateError(db, nensan, tairyoOkList[i].共済目的コード, tairyoOkList[i].組合員等コード, tairyoOkList[i].耕地番号, tairyoOkList[i].分筆番号);
                            errorMsg = MessageUtil.Get("ME10010"
                                , "取込ファイル内"
                                , errJoinApp.行番号.ToString()
                                , $"年産 {nensan}, 共済目的コード {tairyoOkList[i].共済目的コード}, 組合員等コード {tairyoOkList[i].組合員等コード}, 耕地番号 {tairyoOkList[i].耕地番号}, 分筆番号 {tairyoOkList[i].分筆番号}");
                            logger.Error(errorMsg);
                            AddError(torikomiRirekiId, errSeq, errorMsg, sysDate, db);
                            errCount++;
                            continue;
                        }

                        okCount++;
                    }

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
                        string errorFileName = $"{BATCH_USER_NAME}-ERR-{torikomiRirekiId}.csv";
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

                        // 10.1.5 一時領域に作成したフォルダとファイルを削除する
                        Directory.Delete(tempFolderPath, true);
                        torikomiSts = "99";
                        goto forcedShutdown;
                    }


                    if ("1".Equals(chushutuKbn))
                    {
                        DeleteKouchGis1(db, kumiaitoCd, short.Parse(nensan), kyosaimokutekiCd);
                    }
                    else if ("2".Equals(chushutuKbn))
                    {
                        DeleteKouchGis2(db, kumiaitoCd, short.Parse(nensan), kyosaimokutekiCd, haniParam1, haniParam2);
                    }
                    else if ("3".Equals(chushutuKbn))
                    {
                        DeleteKouchGis3(db, kumiaitoCd, short.Parse(nensan), kyosaimokutekiCd, haniParam1, haniParam2, haniParam3);
                    }
                    else if ("4".Equals(chushutuKbn))
                    {
                        DeleteKouchGis4(db, kumiaitoCd, short.Parse(nensan), kyosaimokutekiCd, haniParam1);
                    }

                    String strJoukenId = Guid.NewGuid().ToString();
                    var joukenHikiukeOk = new T01050バッチ条件
                    {
                        バッチ条件id = strJoukenId,
                        連番 = 1,
                        条件名称 = "受入履歴id",
                        表示用条件値 = ukeireRirekiId,
                        条件値 = ukeireRirekiId,
                        登録日時 = sysDate,
                        登録ユーザid = BATCH_USER_NAME,
                        更新日時 = sysDate,
                        更新ユーザid = BATCH_USER_NAME
                    };
                    db.T01050バッチ条件s.Add(joukenHikiukeOk);
                    var 予約を実行した処理名 = string.Empty;
                    var バッチ名 = string.Empty;
                    /*
                     * TODO : バッチ登録時、30桁数制限に引っかかるため、
                     * 登録名修正必要 -> "NSK_106012B_引受計算処理（水稲）エラーチェック処理"
                     * ->_引受計算処理（水稲）エラーチェック処理
                     * ->_引受計算処理（麦）エラーチェック処理
                     */
                    if ("11".Equals(kyosaimokutekiCd))
                    {
                        // "NSK_106012B_引受計算処理（水稲）エラーチェック処理"
                        予約を実行した処理名 = "NSK_106012B";
                        バッチ名 = "引受計算処理（水稲）エラーチェック処理";
                    }
                    else if ("20".Equals(kyosaimokutekiCd))
                    {
                        予約を実行した処理名 = "NSK_106022B_引受計算処理（陸稲）エラーチェック処理";
                        バッチ名 = "引受計算処理（陸稲）エラーチェック処理";
                    }
                    else if ("30".Equals(kyosaimokutekiCd))
                    {
                        予約を実行した処理名 = "NSK_106032B_引受計算処理（麦）エラーチェック処理";
                        バッチ名 = "引受計算処理（麦）エラーチェック処理";
                    }

                    logger.Info("バッチ予約登録処理を実行する。");
                    var refMsg = string.Empty;
                    long batchId = 0;
                    int newYoyakuResult = BatchUtil.InsertBatchYoyaku(Constants.BatchBunrui.BATCH_BUNRUI_90_OTHER,
                    "02", /* 農作物共済 */
                    todofukenCd,
                    kumiaitoCd,
                    shishoCd,
                    sysDate,
                    BATCH_USER_NAME,
                    予約を実行した処理名,
                    バッチ名,
                    strJoukenId,
                    filePath,
                    "0",
                    "1",
                    null,
                    "0",
                    ref refMsg,
                    ref batchId
                    );

                    // 処理結果がエラーだった場合
                    if (newYoyakuResult == 0)
                    {
                        errorMsg = MessageUtil.Get("ME90008", "（" + refMsg + "）");
                        logger.Error(errorMsg);
                        goto forcedShutdown;
                    }


                    torikomiSts = "03";
                    batchSts = "03";
                // トランザクションコミット
                forcedShutdown:;
                    if (!"03".Equals(torikomiSts))
                    {
                        torikomiSts = "99";
                    }
                    
                    UpdateTorikomiHistory(db, torikomiRirekiId, torikomiSts, okCount+errCount, errCount,
                            errorListName, errorListPath, errorListHashValue, sysDate, BATCH_USER_NAME);
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    logger.Error(e.StackTrace);
                    Console.Error.WriteLine(MessageUtil.Get("MF00001"));
                    logger.Error(MessageUtil.Get("MF00001"));
                    logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                    batchSts = "99";
                    errorMsg = MessageUtil.Get("MF00001");
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
                    int upSts = BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), batchSts, errorMsg, BATCH_USER_NAME, ref refMessage);
                    if (0 == upSts)
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
        private static List<T01050バッチ条件> GetBatchJoukenList(
            NskAppContext db,
            string joukenId,
            string nensan,
            string filePath,
            string ukeirerirekiId)
        {
            logger.Info("バッチ条件テーブルから、指定されたバッチ条件idおよび条件名称に一致するデータを取得します。");
            var conditionNames = new[] { nensan, filePath, ukeirerirekiId };
            var results = db.Set<T01050バッチ条件>()
                            .Where(x => x.バッチ条件id == joukenId && conditionNames.Contains(x.条件名称))
                            .ToList();
            return results;
        }

        private static T01070大量データ取込履歴 GetTairyoDataTorikomi(NskAppContext db, string torikomiRerekiId)
        {
            return db.Set<T01070大量データ取込履歴>()
                     .Where(x => x.取込履歴id == torikomiRerekiId)
                     .FirstOrDefault();
        }

        private static int UpdateT01070TorikomiRireki(
            NskAppContext db,
            string torikomiRirekiId,
            string torikomiSts,
            DateTime updateDate,
            string updateUserId)
        {
            logger.Info("「大量データ取込履歴更新登録」の処理を実行します。");
            var entity = db.Set<T01070大量データ取込履歴>()
                           .FirstOrDefault(x => x.取込履歴id == torikomiRirekiId);
            if (entity == null)
            {
                logger.Warn($"取込履歴ID {torikomiRirekiId} のレコードが見つかりません。");
                return 0;
            }
            entity.ステータス = torikomiSts;
            entity.更新日時 = updateDate;
            entity.更新ユーザid = updateUserId;
            return db.SaveChanges();
        }


        private static int GetTodofukenCount(NskAppContext db, string todofukenCd)
        {
            logger.Info($"都道府県コード {todofukenCd} の件数を取得します。");
            int todofuken = db.VTodofukens
                .Where(x => x.TodofukenCd == todofukenCd)
                .Count();
            return todofuken;
        }


        private static int GetKumiaitoCount(NskAppContext db, string todofukenCd, string kumiaitoCd)
        {
            logger.Info($"都道府県コード {todofukenCd} と組合員コード {kumiaitoCd} の件数を取得します。");
            int kumiaitoCn = db.VKumiaitos
                     .Where(x => x.TodofukenCd == todofukenCd &&
                                 x.KumiaitoCd == kumiaitoCd)
                     .Count();

            return kumiaitoCn;
        }

        private static int GetShishoCount(NskAppContext db, string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            logger.Info("m_shisho_nmテーブルから、指定の条件に一致する件数を取得します。");
            return db.Set<VShishoNm>()
                     .Count(x => x.TodofukenCd == todofukenCd &&
                                 x.KumiaitoCd == kumiaitoCd &&
                                 x.ShishoCd == shishoCd);
        }


        private static List<T19070大量データ受入加入申込書ok> GetUkeireAppFormData(
            NskAppContext db,
            string ukeireRirekiId)
        {
            logger.Info($"t_19070_大量データ受入_加入申込書OK のデータを取得します。受入履歴id: {ukeireRirekiId}");
            return db.Set<T19070大量データ受入加入申込書ok>()
                     .Where(x => x.受入履歴id == long.Parse(ukeireRirekiId))
                     .ToList();
        }


        private static int GetKyosaiMokutekiCount(NskAppContext db, string kyosaiMokutekiCd)
        {
            logger.Info($"共済目的コード {kyosaiMokutekiCd} の件数を取得します。");
            return db.Set<M00010共済目的名称>()
                     .Count(x => x.共済目的コード == kyosaiMokutekiCd);
        }


        private static int GetRuiKbnCount(NskAppContext db, string ruiKbn)
        {
            logger.Info($"類区分 {ruiKbn} の件数を取得します。");
            return db.Set<M00020類名称>()
                     .Count(x => x.類区分 == ruiKbn);
        }
        
        private static int GetTabatakeCount(NskAppContext db, string tabatakeKbn)
        {
            return db.Set<M00040田畑名称>()
                     .Count(x => x.田畑区分 == tabatakeKbn);
        }

        private static int GetKubunCount(
            NskAppContext db,
            string kumiaitoCd,
            string nensan,
            string kyousaimokutekiCd,
            string kbnCd)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {kyousaimokutekiCd}, 区分コード {kbnCd} の件数を取得します。");
            return db.Set<M00030区分名称>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyousaimokutekiCd &&
                                 x.区分コード == kbnCd);
        }


        private static int GetShuruiCount(NskAppContext db, string kyosaimokutekiCd, string shuruiCd)
        {
            return db.Set<M10140種類名称>()
                     .Count(x => x.共済目的コード == kyosaimokutekiCd && x.種類コード == shuruiCd);
        }


        private static int GetYoutoKbnCount(NskAppContext db, string kyosaimokutekiCd, string youtoKbn)
        {
            logger.Info($"共済目的コード {kyosaimokutekiCd} と用途区分 {youtoKbn} の件数を取得します。");
            return db.Set<M10110用途区分名称>()
                     .Count(x => x.共済目的コード == kyosaimokutekiCd && x.用途区分 == youtoKbn);
        }


        private static int GetHinshuKeisuuCount(
            NskAppContext db,
            string kumiaitoCd,
            string nensan,
            string kyosaimokutekiCd,
            string hinshuCd)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {kyosaimokutekiCd}, 品種コード {hinshuCd} の件数を取得します。");
            return db.Set<M00110品種係数>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.品種コード == hinshuCd);
        }


        private static int GetSanchiBetuMeigaraCount(
            NskAppContext db,
            string kumiaitoCd,
            string nensan,
            string kyosaimokutekiCd,
            string sanchiBetuMeigaraCd)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {kyosaimokutekiCd}, 産地別銘柄コード {sanchiBetuMeigaraCd} の件数を取得します。");
            return db.Set<M00130産地別銘柄名称設定>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.産地別銘柄コード == sanchiBetuMeigaraCd);
        }


        private static int GetShuryoToukyuCount(
            NskAppContext db,
            string kumiaitoCd,
            string nensan,
            string kyosaimokutekiCd,
            string ruiKbn,
            string sishoCd,
            string shuryoToukyuCd)
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

        private static int GetSansyakuKeisuuCount(
            NskAppContext db,
            string kumiaitoCd,
            string nensan,
            string kyosaimokutekiCd,
            string sansyakuCd)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {kyosaimokutekiCd}, 参酌コード {sansyakuCd} の件数を取得します。");
            return db.Set<M10040参酌係数>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.参酌コード == sansyakuCd);
        }


        private static int GetToukeitaniChiikiCount(
            NskAppContext db,
            string kumiaitoCd,
            string nensan,
            string kyosaimokutekiCd,
            string toukeitaniChiikiCd)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {kyosaimokutekiCd}, 統計単位地域コード {toukeitaniChiikiCd} の件数を取得します。");
            return db.Set<M00170統計単位地域>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.統計単位地域コード == toukeitaniChiikiCd);
        }

        private static int GetNogyoshaCount(NskAppContext db, string kumiaitoCd, string kumiaiintoCd)
        {
            logger.Info($"組合員コード {kumiaitoCd}, 組合員等コード {kumiaiintoCd} の件数を取得します。");
            return db.Set<VNogyosha>()
                     .Count(x => x.KumiaitoCd == kumiaitoCd && x.KumiaiintoCd == kumiaiintoCd);
        }

        private static VNogyosha GetNogyoshaByCodes(NskAppContext db, string kumiaitoCd, string kumiaiintoCd)
        {
            logger.Info($"組合員コード {kumiaitoCd}, 組合員等コード {kumiaiintoCd} のレコードを取得します。");

            return db.Set<VNogyosha>()
                     .Where(x => x.KumiaitoCd == kumiaitoCd &&
                                 x.KumiaiintoCd == kumiaiintoCd)
                     .FirstOrDefault();
        }

        private static List<M00030区分名称> GetKubunMeishouRecords(
            NskAppContext db,
            string kumiaitoCd,
            string nensan,
            string kyosaimokuteCd,
            string kbnCd)
        {
            logger.Info($"m_00030_区分名称から、組合等コード={kumiaitoCd}, 年産={nensan}, 共済目的コード={kyosaimokuteCd}, 区分コード={kbnCd} のレコードを取得します。");
            return db.Set<M00030区分名称>()
                     .Where(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokuteCd &&
                                 x.区分コード == kbnCd)
                     .ToList();
        }


        private static List<M10140種類名称> GetShuruiMeishouRecords(
            NskAppContext db,
            string kyosaimokutekiCd)
        {
            logger.Info($"共済目的コード {kyosaimokutekiCd} のレコードを取得します。");
            return db.Set<M10140種類名称>()
                     .Where(x => x.共済目的コード == kyosaimokutekiCd)
                     .ToList();
        }

        private static int GetJoinApplicationOkCount(
            NskAppContext db,
            string ukeireRirekiId,
            decimal lineNum,
            string nensan,
            string kyosaimokutekiCd,
            string kumiaiintoCd,
            string kouchiBango,
            string bunpituBango)
        {
            logger.Info($"年産 {nensan}, 共済目的コード {kyosaimokutekiCd}, 組合員等コード {kumiaiintoCd}, 耕地番号 {kouchiBango}, 分筆番号 {bunpituBango} の件数を取得します。");
            return db.Set<T19070大量データ受入加入申込書ok>()
                     .Count(x => x.受入履歴id == long.Parse(ukeireRirekiId) &&
                                 x.行番号 == lineNum &&
                                 x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd &&
                                 x.耕地番号 == kouchiBango &&
                                 x.分筆番号 == bunpituBango);
        }

        private static T19070大量データ受入加入申込書ok GetDuplicateError(
            NskAppContext db,
            string nensan,
            string kyosaimokutekiCd,
            string kumiaiintoCd,
            string kouchiBango,
            string bunpituBango)
        {
            logger.Info($"年産 {nensan}, 共済目的コード {kyosaimokutekiCd}, 組合員等コード {kumiaiintoCd}, 耕地番号 {kouchiBango}, 分筆番号 {bunpituBango} の件数を取得します。");
            return db.Set<T19070大量データ受入加入申込書ok>()
                     .Where(x => x.年産 == short.Parse(nensan) &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd &&
                                 x.耕地番号 == kouchiBango &&
                                 x.分筆番号 == bunpituBango)
                     .FirstOrDefault();
        }

        private static List<T01080大量データ受入エラーリスト> GetErrorListByTorikomiId(
            NskAppContext db,
            string torikomiId)
        {
            logger.Info($"処理区分 '2', 履歴ID {torikomiId} のエラーリストを取得します。");
            long id = long.Parse(torikomiId);
            return db.Set<T01080大量データ受入エラーリスト>()
                     .Where(x => x.処理区分 == "2" && x.履歴id == id)
                     .AsNoTracking()
                     .ToList();
        }


        private static int UpdateTorikomiHistory(
            NskAppContext db,
            string torikomiRirekiId,
            string torikomiSts,
            int tgtCount,
            int errorCount,
            string errorListName,
            string errorListPath,
            string errorListValue,
            DateTime systemDate,
            string updateUser)
        {
            logger.Info($"取込履歴ID {torikomiRirekiId} のデータを更新します。");
            var entity = db.Set<T01070大量データ取込履歴>()
                           .FirstOrDefault(x => x.取込履歴id == torikomiRirekiId);
            if (entity == null)
            {
                logger.Warn($"取込履歴ID {torikomiRirekiId} のレコードが見つかりません。");
                return 0;
            }
            entity.ステータス = torikomiSts;
            entity.対象件数 = tgtCount;
            entity.エラー件数 = errorCount;
            entity.エラーリスト名 = errorListName;
            entity.エラーリストパス = errorListPath;
            entity.エラーリストハッシュ値 = errorListValue;
            entity.終了日時 = systemDate;
            entity.更新日時 = systemDate;
            entity.更新ユーザid = updateUser;
            return db.SaveChanges();
        }

        /// <summary>
        /// 削除処理1：
        /// ・T11090引受耕地とT11100引受gisを、両テーブル間の結合キー（組合等コード、年産、共済目的コード、組合員等コード、耕地番号、分筆番号）で内部結合し、
        ///   kouch.組合等コード = 指定値、kouch.年産 = 指定値、
        ///   さらに kouch.共済目的コード が指定値と一致する（または指定値が null）のレコードを削除する。
        /// </summary>
        private static int DeleteKouchGis1(NskAppContext db, string kumiaitoCd, short nensan, string kyosaiMokutekiCd)
        {
            logger.Info($"削除処理1開始: 組合等コード={kumiaitoCd}, 年産={nensan}, 共済目的コード={kyosaiMokutekiCd ?? "NULL"}");
            var items = (from kouch in db.T11090引受耕地s
                         join gis in db.T11100引受giss
                           on new { kouch.組合等コード, kouch.年産, kouch.共済目的コード, kouch.組合員等コード, kouch.耕地番号, kouch.分筆番号 }
                           equals new { gis.組合等コード, gis.年産, gis.共済目的コード, gis.組合員等コード, gis.耕地番号, gis.分筆番号 }
                         where kouch.組合等コード == kumiaitoCd &&
                               kouch.年産 == nensan &&
                               (kouch.共済目的コード == kyosaiMokutekiCd || kyosaiMokutekiCd == null)
                         select new { kouch, gis }).ToList();

            db.T11090引受耕地s.RemoveRange(items.Select(x => x.kouch));
            db.T11100引受giss.RemoveRange(items.Select(x => x.gis));
            int affected = db.SaveChanges();
            logger.Info($"削除処理1完了: 削除対象件数={items.Count}");
            return affected;
        }

        /// <summary>
        /// 削除処理2：
        /// ・Delete1 に加え、さらに kouch.組合員等コード が指定された範囲内（下限は必須、上限は null も可）である条件を追加する。
        /// </summary>
        private static int DeleteKouchGis2(NskAppContext db, string kumiaitoCd, short nensan, string kyosaiMokutekiCd, string rangeParam1, string rangeParam2)
        {
            logger.Info($"削除処理2開始: 組合等コード={kumiaitoCd}, 年産={nensan}, 共済目的コード={kyosaiMokutekiCd ?? "NULL"}, 組合員等コード >= {rangeParam1}, 組合員等コード <= {rangeParam2 ?? "NULL"}");
            var items = (from kouch in db.T11090引受耕地s
                         join gis in db.T11100引受giss
                           on new { kouch.組合等コード, kouch.年産, kouch.共済目的コード, kouch.組合員等コード, kouch.耕地番号, kouch.分筆番号 }
                           equals new { gis.組合等コード, gis.年産, gis.共済目的コード, gis.組合員等コード, gis.耕地番号, gis.分筆番号 }
                         where kouch.組合等コード == kumiaitoCd &&
                               kouch.年産 == nensan &&
                               (kouch.共済目的コード == kyosaiMokutekiCd || kyosaiMokutekiCd == null) &&
                               string.Compare(kouch.組合員等コード, rangeParam1) >= 0 &&
                               (rangeParam2 == null || string.Compare(kouch.組合員等コード, rangeParam2) <= 0)
                         select new { kouch, gis }).ToList();

            db.T11090引受耕地s.RemoveRange(items.Select(x => x.kouch));
            db.T11100引受giss.RemoveRange(items.Select(x => x.gis));
            int affected = db.SaveChanges();
            logger.Info($"削除処理2完了: 削除対象件数={items.Count}");
            return affected;
        }

        /// <summary>
        /// 削除処理3：
        /// ・T11090引受耕地とT11100引受gisの結合に加え、fim_t_nogyosha (FimTNogyosha) を内部結合し、
        ///   gis.組合等コード、gis.年産、gis.共済目的コード の条件に加え、
        ///   nogyosha.大地区コード = 指定値、
        ///   nogyosha.小地区コード が指定された範囲内（下限、上限それぞれ null も可）の条件を追加して削除する。
        /// </summary>
        private static int DeleteKouchGis3(NskAppContext db, string kumiaitoCd, short nensan, string kyosaiMokutekiCd, string rangeParam1, string rangeParam2, string rangeParam3)
        {
            logger.Info($"削除処理3開始: 組合等コード={kumiaitoCd}, 年産={nensan}, 共済目的コード={kyosaiMokutekiCd ?? "NULL"}, 大地区コード={rangeParam1}, 小地区コード >= {rangeParam2}, 小地区コード <= {rangeParam3 ?? "NULL"}");
            var items = (from kouch in db.T11090引受耕地s
                         join gis in db.T11100引受giss
                           on new { kouch.組合等コード, kouch.年産, kouch.共済目的コード, kouch.組合員等コード, kouch.耕地番号, kouch.分筆番号 }
                           equals new { gis.組合等コード, gis.年産, gis.共済目的コード, gis.組合員等コード, gis.耕地番号, gis.分筆番号 }
                         join nogyosha in db.VNogyoshas
                          on new { 組合等コード = kouch.組合等コード, 組合員等コード = kouch.組合員等コード }
                          equals new { 組合等コード = nogyosha.KumiaitoCd, 組合員等コード = nogyosha.KumiaiintoCd }
                         where gis.組合等コード == kumiaitoCd &&
                               gis.年産 == nensan &&
                               (gis.共済目的コード == kyosaiMokutekiCd || kyosaiMokutekiCd == null) &&
                               nogyosha.DaichikuCd == rangeParam1 &&
                               (rangeParam2 == null || string.Compare(nogyosha.ShochikuCd, rangeParam2) >= 0) &&
                               (rangeParam3 == null || string.Compare(nogyosha.ShochikuCd, rangeParam3) <= 0)
                         select new { kouch, gis }).ToList();

            db.T11090引受耕地s.RemoveRange(items.Select(x => x.kouch));
            db.T11100引受giss.RemoveRange(items.Select(x => x.gis));
            int affected = db.SaveChanges();
            logger.Info($"削除処理3完了: 削除対象件数={items.Count}");
            return affected;
        }

        /// <summary>
        /// 削除処理4：
        /// ・T11090引受耕地とT11100引受gisの結合条件は共通で、
        ///   WHERE 句では gis.組合等コード、gis.年産、(gis.共済目的コード = 指定値 OR 指定値が null) に加え、
        ///   gis.市町村コード = 指定値 の条件を用いる。
        /// </summary>
        private static int DeleteKouchGis4(NskAppContext db, string kumiaitoCd, short nensan, string kyosaiMokutekiCd, string rangeParam1)
        {
            logger.Info($"削除処理4開始: 組合等コード={kumiaitoCd}, 年産={nensan}, 共済目的コード={kyosaiMokutekiCd ?? "NULL"}, 市町村コード={rangeParam1}");
            var items = (from kouch in db.T11090引受耕地s
                         join gis in db.T11100引受giss
                           on new { kouch.組合等コード, kouch.年産, kouch.共済目的コード, kouch.組合員等コード, kouch.耕地番号, kouch.分筆番号 }
                           equals new { gis.組合等コード, gis.年産, gis.共済目的コード, gis.組合員等コード, gis.耕地番号, gis.分筆番号 }
                         where gis.組合等コード == kumiaitoCd &&
                               gis.年産 == nensan &&
                               (gis.共済目的コード == kyosaiMokutekiCd || kyosaiMokutekiCd == null) &&
                               gis.市区町村コード == rangeParam1
                         select new { kouch, gis }).ToList();

            db.T11090引受耕地s.RemoveRange(items.Select(x => x.kouch));
            db.T11100引受giss.RemoveRange(items.Select(x => x.gis));
            int affected = db.SaveChanges();
            logger.Info($"削除処理4完了: 削除対象件数={items.Count}");
            return affected;
        }



        private static void AddError(
            string torikomiRirekiId
            , int errListSeq
            , string dataError
            , DateTime sysDate
            , NskAppContext db)
        {
            var dataErrorList = new T01080大量データ受入エラーリスト
            {
                処理区分 = "2",
                履歴id = long.Parse(torikomiRirekiId),
                枝番 = errListSeq,
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
