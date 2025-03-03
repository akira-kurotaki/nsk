using NSK_B112041_KijunShukakuryoDataTorikomi.Common;
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

namespace NSK_B112041_KijunShukakuryoDataTorikomi
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
            var torikomiRirekiId = "";
            var filePath = string.Empty;

            var kumiaitoCdUkeire = string.Empty;
            var shoriSts = "03";
            var errorMsg = string.Empty;
            string msgTest = string.Empty;
            bool forcedShutdownFlg = false;

            var errorListName = string.Empty;
            var errorListPath = string.Empty;
            var errorListHashValue = string.Empty;

            int result = Constants.BATCH_EXECUT_SUCCESS;

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

            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema, ConfigUtil.GetInt(Constants.CONFIG_COMMAND_TIMEOUT)))
            {
                var transaction = db.Database.BeginTransaction();
                try
                {
                    // バッチ条件情報取得
                    List<T01050バッチ条件> joukenList = GetBatchJoukenList(db, jid, Constants.JOUKEN_NENSAN, Constants.JOUKEN_TORIKOMI_RIREKI_ID);
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
                        if (Constants.JOUKEN_TORIKOMI_RIREKI_ID.Equals(joukenList[i].条件名称))
                        {
                            torikomiRirekiId = joukenList[i].条件値;
                        }
                    }
                    if (string.IsNullOrEmpty(nensan))
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    if (string.IsNullOrEmpty(torikomiRirekiId))
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

                    var resultTairyoData = GetTairyoData(db, torikomiRirekiId);
                    if (resultTairyoData == null || !"01".Equals(resultTairyoData.ステータス)  /* 処理待ち */)
                    {
                        errorMsg = MessageUtil.Get("ME10042", "取込処理");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    // 処理中
                    var runningSts = "02";
                    UpdateTorikomiRireki(db, torikomiRirekiId, runningSts, sysDate);

                    List<T19020大量データ受入基準収穫量ok> ukeireOkList = GetOkListByUkeireRirekiId(db, resultTairyoData.受入履歴id);
                    var ukeireErrorList = new List<ErrorList>();
                    if (ukeireOkList.IsNullOrEmpty())
                    {
                        errorMsg = MessageUtil.Get("ME10025", "取込対象");
                        forcedShutdownFlg = true;
                        goto forcedShutdown;
                    }

                    for (int i = 0; i < ukeireOkList.Count; i++)
                    {
                        // 属性チェック
                        // ------------------------------
                        // 各カラムの属性チェック（必須、文字数、数値チェック）
                        // ------------------------------

                        // 処理区分（文字列、必須、文字数=1）
                        if (string.IsNullOrWhiteSpace(ukeireOkList[i].処理区分))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00001", "処理区分", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (ukeireOkList[i].処理区分.Length != 1)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "処理区分", "1", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 組合等コード（半角数字、必須、文字数=3）
                        if (string.IsNullOrWhiteSpace(ukeireOkList[i].組合等コード))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00001", "組合等コード", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (ukeireOkList[i].組合等コード.Length > 3)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "組合等コード", "3", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (!int.TryParse(ukeireOkList[i].組合等コード, out _))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00003", "組合等コード", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 組合等名（文字列、任意、最大文字数=50）
                        if (!string.IsNullOrWhiteSpace(ukeireOkList[i].組合等名) && ukeireOkList[i].組合等名.Length > 50)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "組合等コード", "50", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 年産（半角数字、必須、文字数=4）
                        if (ukeireOkList[i].年産 == null)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00001", "年産", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (ukeireOkList[i].年産 > 9999)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "年産", "4", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 共済目的コード（文字列、必須、文字数=4）
                        if (string.IsNullOrWhiteSpace(ukeireOkList[i].共済目的コード))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00001", "共済目的コード", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (ukeireOkList[i].共済目的コード.Length > 4)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "共済目的コード", "4", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 共済目的名（半角数字または文字列、任意、最大文字数=20）
                        if (!string.IsNullOrWhiteSpace(ukeireOkList[i].共済目的名) && ukeireOkList[i].共済目的名.Length > 20)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "共済目的名", "4", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 引受方式（半角数字、必須、文字数=1）
                        if (string.IsNullOrWhiteSpace(ukeireOkList[i].引受方式))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00001", "引受方式", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (ukeireOkList[i].引受方式.Length != 1)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "引受方式", "1", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (!int.TryParse(ukeireOkList[i].引受方式, out _))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00003", "引受方式", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 引受方式名称（文字列、任意、最大文字数=20）
                        if (!string.IsNullOrWhiteSpace(ukeireOkList[i].引受方式名称) && ukeireOkList[i].引受方式名称.Length > 20)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "引受方式名称", "20", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 支所コード（半角数字、必須、文字数=2）
                        if (string.IsNullOrWhiteSpace(ukeireOkList[i].支所コード))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00001", "支所コード", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (ukeireOkList[i].支所コード.Length > 2)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "支所コード", "20", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (!int.TryParse(ukeireOkList[i].支所コード, out _))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00003", "支所コード", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 支所名（文字列、任意、最大文字数=20）
                        if (!string.IsNullOrWhiteSpace(ukeireOkList[i].支所名) && ukeireOkList[i].支所名.Length > 20)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "支所名", "20", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 大地区コード（半角数字、必須、文字数=2）
                        if (string.IsNullOrWhiteSpace(ukeireOkList[i].大地区コード))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00001", "大地区コード", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (ukeireOkList[i].大地区コード.Length > 2)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "大地区コード", "2", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (!int.TryParse(ukeireOkList[i].大地区コード, out _))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00003", "大地区コード", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 大地区名（文字列、任意、最大文字数=10）
                        if (!string.IsNullOrWhiteSpace(ukeireOkList[i].大地区名) && ukeireOkList[i].大地区名.Length > 10)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "大地区名", "10", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 小地区コード（半角数字、必須、文字数=4）
                        if (string.IsNullOrWhiteSpace(ukeireOkList[i].小地区コード))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00001", "小地区コード", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (ukeireOkList[i].小地区コード.Length > 4)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "小地区コード", "4", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (!int.TryParse(ukeireOkList[i].小地区コード, out _))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00003", "小地区コード", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 小地区名（文字列、任意、最大文字数=10）
                        if (!string.IsNullOrWhiteSpace(ukeireOkList[i].小地区名) && ukeireOkList[i].小地区名.Length > 10)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "小地区名", "4", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 組合員等コード（半角数字、必須、文字数=13）
                        if (string.IsNullOrWhiteSpace(ukeireOkList[i].組合員等コード))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00001", "組合員等コード", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (ukeireOkList[i].組合員等コード.Length > 13)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "組合員等コード", "13", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (!long.TryParse(ukeireOkList[i].組合員等コード, out _))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00003", "組合員等コード", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 組合員等氏名（文字列、任意、最大文字数=30）
                        if (!string.IsNullOrWhiteSpace(ukeireOkList[i].組合員等氏名) && ukeireOkList[i].組合員等氏名.Length > 30)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "組合員等コード", "30", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 類区分（半角数字、必須、文字数=2）
                        if (string.IsNullOrWhiteSpace(ukeireOkList[i].類区分))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00001", "類区分", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (ukeireOkList[i].類区分.Length > 2)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "類区分", "2", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (!int.TryParse(ukeireOkList[i].類区分, out _))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00003", "類区分", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 類区分名（文字列、任意、最大文字数=40）
                        if (!string.IsNullOrWhiteSpace(ukeireOkList[i].類区分名) && ukeireOkList[i].類区分名.Length > 40)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "類区分名", "40", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 産地別銘柄等コード（半角数字、任意、文字数=5 ※値が存在する場合）
                        
                        if (!string.IsNullOrWhiteSpace(ukeireOkList[i].産地別銘柄コード) && !int.TryParse(ukeireOkList[i].産地別銘柄コード, out _))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00003", "産地別銘柄等コード", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (!string.IsNullOrWhiteSpace(ukeireOkList[i].産地別銘柄コード) && ukeireOkList[i].産地別銘柄コード.Length > 5)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "産地別銘柄等コード", "5", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 産地別銘柄等名称（半角数字または文字列、任意、最大文字数=30）
                        if (!string.IsNullOrWhiteSpace(ukeireOkList[i].産地別銘柄等名称) &&
                            ukeireOkList[i].産地別銘柄等名称.Length > 30)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "産地別銘柄等コード", "30", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 営農対象外フラグ（半角数字、必須、文字数=1）
                        if (string.IsNullOrWhiteSpace(ukeireOkList[i].営農対象外フラグ))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00001", "営農対象外フラグ", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (ukeireOkList[i].営農対象外フラグ.Length != 1)
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00020", "営農対象外フラグ", "1", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }
                        if (!int.TryParse(ukeireOkList[i].営農対象外フラグ, out _))
                        {
                            ukeireErrorList.Add(new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME00003", "産地別銘柄等コード", ""),
                                行番号 = ukeireOkList[i].行番号
                            });
                            continue;
                        }

                        // 共済目的コードのチェック
                        if (0 == GetKyosaiMokutekiCount(db, ukeireOkList[i].共済目的コード))
                        {
                            var entity = new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME10005", "共済目的コード"),
                                行番号 = ukeireOkList[i].行番号
                            };
                            ukeireErrorList.Add(entity);
                            continue;
                        }
                        // 引受方式のチェック
                        if (0 == GetHikiukeHousiki(db, ukeireOkList[i].引受方式))
                        {
                            var entity = new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME10005", "引受方式"),
                                行番号 = ukeireOkList[i].行番号
                            };
                            ukeireErrorList.Add(entity);
                            continue;
                        }
                        // 類区分のチェック
                        if (0 == GetRuiKbnCount(db, ukeireOkList[i].類区分, ukeireOkList[i].共済目的コード))
                        {
                            var entity = new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME10005", "類区分"),
                                行番号 = ukeireOkList[i].行番号
                            };
                            ukeireErrorList.Add(entity);
                            continue;
                        }
                        // 産地別銘柄のチェック
                        if (0 == GetSanchiBetuCount(db, kumiaitoCd, nensan, ukeireOkList[i].共済目的コード, ukeireOkList[i].産地別銘柄コード))
                        {
                            var entity = new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME10005", "産地別銘柄"),
                                行番号 = ukeireOkList[i].行番号
                            };
                            ukeireErrorList.Add(entity);
                            continue;
                        }
                        // 組合員等コードのチェック（農業者情報管理システムに存在するか）
                        if (0 == GetNogyoshaCount(db, kumiaitoCd, ukeireOkList[i].組合員等コード))
                        {
                            var entity = new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME10001", "組合員等コード", "農業者情報管理システム"),
                                行番号 = ukeireOkList[i].行番号
                            };
                            ukeireErrorList.Add(entity);
                            continue;
                        }
                        // 組合等コードの一致チェック
                        if (!kumiaitoCd.Equals(ukeireOkList[i].組合等コード))
                        {
                            var entity = new ErrorList
                            {
                                //TODO: メッセージ情報必要
                                //"DUMMY_MESSAGE_ID"：{0}が他県のコードとなっています。
                                エラー内容 = MessageUtil.Get("ME10001", "組合員等コード", "農業者情報管理システム"),
                                行番号 = ukeireOkList[i].行番号
                            };
                            ukeireErrorList.Add(entity);
                            continue;
                        }
                        // 年産の一致チェック
                        if (short.TryParse(nensan, out short parsedValue))
                        {
                            if (parsedValue != ukeireOkList[i].年産)
                            {
                                var entity = new ErrorList
                                {
                                    //"DUMMY_MESSAGE_ID"：{0}が他県のコードとなっています。
                                    エラー内容 = MessageUtil.Get("ME90015", "年産"),
                                    行番号 = ukeireOkList[i].行番号
                                };
                                ukeireErrorList.Add(entity);
                                continue;
                            }
                        }
                        
                        
                        // 基準収穫量のチェック
                        int kijunShukakuryoCount = GetKijunShukakuryoCount(
                            db
                            , kumiaitoCd
                            , ukeireOkList[i].年産
                            , ukeireOkList[i].共済目的コード
                            , ukeireOkList[i].組合員等コード
                            , ukeireOkList[i].類区分
                            , ukeireOkList[i].産地別銘柄コード
                            , ukeireOkList[i].営農対象外フラグ);
                        // [変数：受入OKデータ.処理区分]=1（削除）の場合
                        if (0 == kijunShukakuryoCount && "1".Equals(ukeireOkList[i].処理区分))
                        {
                            var entity = new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME10068", "基準収穫量設定"),
                                行番号 = ukeireOkList[i].行番号
                            };
                            ukeireErrorList.Add(entity);
                            continue;
                        }
                        // [変数：受入OKデータ.処理区分]=2（新規）の場合
                        if (1 <= kijunShukakuryoCount && "2".Equals(ukeireOkList[i].処理区分))
                        {
                            var entity = new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME91002", "基準収穫量設定"),
                                行番号 = ukeireOkList[i].行番号
                            };
                            ukeireErrorList.Add(entity);
                            continue;
                        }
                        // [変数：受入OKデータ.処理区分]=3（修正）の場合
                        if (0 == kijunShukakuryoCount && "3".Equals(ukeireOkList[i].処理区分))
                        {
                            var entity = new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME10068", "基準収穫量設定"),
                                行番号 = ukeireOkList[i].行番号
                            };
                            ukeireErrorList.Add(entity);
                            continue;
                        }
                        // 平均単収のチェック：0.0未満または9999.99超の場合
                        if (ukeireOkList[i].平均単収 < 0.0m || ukeireOkList[i].平均単収 > 9999.99m)
                        {
                            var entity = new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME10004", "平均単収"),
                                行番号 = ukeireOkList[i].行番号
                            };
                            ukeireErrorList.Add(entity);
                            continue;
                        }
                        var breakFlg = false;
                        // 受入OKデータの「規格別割合_規格（全角数字付き）」チェック（1～40）
                        for (int j = 1; j <= 40; j++)
                        {
                            // 全角数字に変換してプロパティ名を生成
                            string propertyName = $"規格別割合_規格{ToFullWidth(j)}";
                            var property = ukeireOkList[i].GetType().GetProperty(propertyName);
                            if (property == null)
                            {
                                continue;
                            }

                            // プロパティの値を decimal? 型として取得
                            var value = property.GetValue(ukeireOkList[i]) as decimal?;
                            if (value.HasValue && (value.Value < 0.0m || value.Value > 1.0m))
                            {
                                var entity = new ErrorList
                                {
                                    エラー内容 = MessageUtil.Get("ME10034", propertyName),
                                    行番号 = ukeireOkList[i].行番号
                                };
                                ukeireErrorList.Add(entity);
                                breakFlg = true;
                                break;
                            }
                        }
                        if (breakFlg)
                        {
                            continue;
                        }
                        // 規格別割合_規格１～４０の合計値が1.0超の場合
                        decimal totalRatio = 0.0m;
                        for (int j = 1; j <= 40; j++)
                        {
                            string propertyName = $"規格別割合_規格{ToFullWidth(j)}";
                            var property = ukeireOkList[i].GetType().GetProperty(propertyName);
                            if (property != null)
                            {
                                var value = property.GetValue(ukeireOkList[i]) as decimal?;
                                if (value.HasValue)
                                {
                                    totalRatio += value.Value;
                                }
                            }
                        }
                        if (totalRatio > 1.0m)
                        {
                            var entity = new ErrorList
                            {
                                エラー内容 = MessageUtil.Get("ME10004", "規格別割合_規格の合計"),
                                行番号 = ukeireOkList[i].行番号
                            };
                            ukeireErrorList.Add(entity);
                            continue;
                        }

                    }

                    /*
                     * TODO:ファイル格納先と処理のロジックが決まってないため、
                     * 必要に応じて修正
                     */
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
                        // 形式: [取込前ファイル名]-ERR-[取込履歴ID].csv
                        string errorFileName = $"{resultTairyoData.取込ファイル_変更前ファイル名}-ERR-{torikomiRirekiId}.csv";
                        string tempFilePath = Path.Combine(tempFolderPath, errorFileName);

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
                                履歴id = long.Parse(torikomiRirekiId),
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
                        // ファイル出力（UTF-8、改行はCRLF）
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

                        UpdateUkeireErrorRireki(
                            db
                            , ukeireErrorList.Count
                            , errorFileName
                            , errorListPath
                            , errorListHashValue
                            , sysDate
                            , torikomiRirekiId);

                        // 10.1.5 一時領域に作成したフォルダとファイルを削除する
                        Directory.Delete(tempFolderPath, true);
                        logger.Error(MessageUtil.Get("ME10026", "取込対象"));
                        goto forcedShutdown;
                    }

                    // 11.取込処理
                    for (int i = 0; i < ukeireOkList.Count; i++)
                    {
                        // [変数：受入OKデータ.処理区分]=1（削除）の場合
                        if ("1".Equals(ukeireOkList[i].処理区分))
                        {
                            Delete11070Records(db, ukeireOkList[i].組合等コード, ukeireOkList[i].年産, ukeireOkList[i].共済目的コード
                                , ukeireOkList[i].類区分, ukeireOkList[i].産地別銘柄コード, ukeireOkList[i].営農対象外フラグ);
                            Delete11060Records(db, ukeireOkList[i].組合等コード, ukeireOkList[i].年産, ukeireOkList[i].共済目的コード
                                , ukeireOkList[i].類区分, ukeireOkList[i].産地別銘柄コード, ukeireOkList[i].営農対象外フラグ);
                        }
                        // [変数：受入OKデータ.処理区分]=2（新規）の場合
                        if ("2".Equals(ukeireOkList[i].処理区分))
                        {
                            var dataNew1 = new T11060基準収穫量設定
                            {
                                組合等コード = ukeireOkList[i].組合等コード,
                                年産 = ukeireOkList[i].年産 ?? 0,
                                共済目的コード = ukeireOkList[i].共済目的コード,
                                組合員等コード = ukeireOkList[i].組合員等コード,
                                類区分 = ukeireOkList[i].類区分,
                                産地別銘柄コード = ukeireOkList[i].産地別銘柄コード,
                                営農対象外フラグ = ukeireOkList[i].営農対象外フラグ,
                                平均単収 = ukeireOkList[i].平均単収,
                                登録日時 = sysDate,
                                登録ユーザid = BATCH_USER_NAME,
                                更新日時 = sysDate,
                                更新ユーザid = BATCH_USER_NAME
                            };
                            // DbSet に追加
                            db.T11060基準収穫量設定s.Add(dataNew1);

                            for (int j = 1; j <= 40; j++)
                            {

                                string propertyName = $"規格別割合_規格{ToFullWidth(j)}";
                                var property = ukeireOkList[i].GetType().GetProperty(propertyName);
                                if (property != null)
                                {
                                    var value = property.GetValue(ukeireOkList[i]) as decimal?;
                                    var dataNew2 = new T11070基準収穫量設定規格別
                                    {
                                        組合等コード = ukeireOkList[i].組合等コード,
                                        年産 = ukeireOkList[i].年産 ?? 0,
                                        共済目的コード = ukeireOkList[i].共済目的コード,
                                        組合員等コード = ukeireOkList[i].組合員等コード,
                                        類区分 = ukeireOkList[i].類区分,
                                        産地別銘柄コード = ukeireOkList[i].産地別銘柄コード,
                                        営農対象外フラグ = ukeireOkList[i].営農対象外フラグ,
                                        規格 = j.ToString(),
                                        規格別割合 = value.Value,
                                        登録日時 = sysDate,
                                        登録ユーザid = BATCH_USER_NAME,
                                        更新日時 = sysDate,
                                        更新ユーザid = BATCH_USER_NAME
                                    };
                                    // DbSet に追加
                                    db.T11070基準収穫量設定規格別s.Add(dataNew2);
                                } 
                                else
                                {
                                    var dataNew2 = new T11070基準収穫量設定規格別
                                    {
                                        組合等コード = ukeireOkList[i].組合等コード,
                                        年産 = ukeireOkList[i].年産 ?? 0,
                                        共済目的コード = ukeireOkList[i].共済目的コード,
                                        組合員等コード = ukeireOkList[i].組合員等コード,
                                        類区分 = ukeireOkList[i].類区分,
                                        産地別銘柄コード = ukeireOkList[i].産地別銘柄コード,
                                        営農対象外フラグ = ukeireOkList[i].営農対象外フラグ,
                                        規格 = j.ToString(),
                                        規格別割合 = 0,
                                        登録日時 = sysDate,
                                        登録ユーザid = BATCH_USER_NAME,
                                        更新日時 = sysDate,
                                        更新ユーザid = BATCH_USER_NAME
                                    };
                                    // DbSet に追加
                                    db.T11070基準収穫量設定規格別s.Add(dataNew2);
                                }
                            }
                        }
                        // [変数：受入OKデータ.処理区分]=3（修正）の場合
                        if ("3".Equals(ukeireOkList[i].処理区分))
                        {
                            Delete11070Records(db, ukeireOkList[i].組合等コード, ukeireOkList[i].年産, ukeireOkList[i].共済目的コード
                                , ukeireOkList[i].類区分, ukeireOkList[i].産地別銘柄コード, ukeireOkList[i].営農対象外フラグ);
                            Delete11060Records(db, ukeireOkList[i].組合等コード, ukeireOkList[i].年産, ukeireOkList[i].共済目的コード
                                , ukeireOkList[i].類区分, ukeireOkList[i].産地別銘柄コード, ukeireOkList[i].営農対象外フラグ);
                            var dataNew1 = new T11060基準収穫量設定
                            {
                                組合等コード = ukeireOkList[i].組合等コード,
                                年産 = ukeireOkList[i].年産 ?? 0,
                                共済目的コード = ukeireOkList[i].共済目的コード,
                                組合員等コード = ukeireOkList[i].組合員等コード,
                                類区分 = ukeireOkList[i].類区分,
                                産地別銘柄コード = ukeireOkList[i].産地別銘柄コード,
                                営農対象外フラグ = ukeireOkList[i].営農対象外フラグ,
                                平均単収 = ukeireOkList[i].平均単収,
                                登録日時 = sysDate,
                                登録ユーザid = BATCH_USER_NAME,
                                更新日時 = sysDate,
                                更新ユーザid = BATCH_USER_NAME
                            };
                            // DbSet に追加
                            db.T11060基準収穫量設定s.Add(dataNew1);

                            for (int j = 1; j <= 40; j++)
                            {

                                string propertyName = $"規格別割合_規格{ToFullWidth(j)}";
                                var property = ukeireOkList[i].GetType().GetProperty(propertyName);
                                if (property != null)
                                {
                                    var value = property.GetValue(ukeireOkList[i]) as decimal?;
                                    var dataNew2 = new T11070基準収穫量設定規格別
                                    {
                                        組合等コード = ukeireOkList[i].組合等コード,
                                        年産 = ukeireOkList[i].年産 ?? 0,
                                        共済目的コード = ukeireOkList[i].共済目的コード,
                                        組合員等コード = ukeireOkList[i].組合員等コード,
                                        類区分 = ukeireOkList[i].類区分,
                                        産地別銘柄コード = ukeireOkList[i].産地別銘柄コード,
                                        営農対象外フラグ = ukeireOkList[i].営農対象外フラグ,
                                        規格 = j.ToString(),
                                        規格別割合 = value.Value,
                                        登録日時 = sysDate,
                                        登録ユーザid = BATCH_USER_NAME,
                                        更新日時 = sysDate,
                                        更新ユーザid = BATCH_USER_NAME
                                    };
                                    // DbSet に追加
                                    db.T11070基準収穫量設定規格別s.Add(dataNew2);
                                }
                                else
                                {
                                    var dataNew2 = new T11070基準収穫量設定規格別
                                    {
                                        組合等コード = ukeireOkList[i].組合等コード,
                                        年産 = ukeireOkList[i].年産 ?? 0,
                                        共済目的コード = ukeireOkList[i].共済目的コード,
                                        組合員等コード = ukeireOkList[i].組合員等コード,
                                        類区分 = ukeireOkList[i].類区分,
                                        産地別銘柄コード = ukeireOkList[i].産地別銘柄コード,
                                        営農対象外フラグ = ukeireOkList[i].営農対象外フラグ,
                                        規格 = j.ToString(),
                                        規格別割合 = 0,
                                        登録日時 = sysDate,
                                        登録ユーザid = BATCH_USER_NAME,
                                        更新日時 = sysDate,
                                        更新ユーザid = BATCH_USER_NAME
                                    };
                                    // DbSet に追加
                                    db.T11070基準収穫量設定規格別s.Add(dataNew2);
                                }
                            }
                        }
                    }

                    // 12 最終処理
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
                }
                catch (Exception e)
                {
                    logger.Error(e.StackTrace);
                    Console.Error.WriteLine(MessageUtil.Get("MF00001"));
                    logger.Error(MessageUtil.Get("MF00001"));
                    logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                    shoriSts = "99";
                    errorMsg = MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION);
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
        private static TairyoDataDto GetTairyoData(NskAppContext db, string torikomiRirekiId)
        {

            var result = (
                from t1 in db.Set<T01070大量データ取込履歴>()
                join t2 in db.Set<T01060大量データ受入履歴>()
                    on t1.受入履歴id equals t2.受入履歴id
                where t1.取込履歴id == torikomiRirekiId
                select new TairyoDataDto
                {
                    受入履歴id = t1.受入履歴id,
                    組合等コード = t1.組合等コード,
                    ステータス = t1.ステータス,
                    対象データ区分 = t1.対象データ区分,
                    取込ファイル_変更前ファイル名 = t2.取込ファイル_変更前ファイル名
                }
            ).FirstOrDefault();

            return result;
        }

        /// <summary>
        ///　大量データ取込履歴の更新
        /// </summary>
        private static int UpdateTorikomiRireki(NskAppContext db, string torikomiRirekiId, string runningSts, DateTime systemDate)
        {
            var entity = db.Set<T01070大量データ取込履歴>()
                           .FirstOrDefault(x => x.取込履歴id == torikomiRirekiId);
            if (entity == null)
            {
                return 0;
            }

            entity.ステータス = runningSts;
            entity.開始日時 = systemDate;
            entity.更新日時 = systemDate;
            entity.更新ユーザid = "NSK_112041B";

            return db.SaveChanges();
        }


        /// <summary>
        ///　エラーリスト履歴更新処理
        /// </summary>
        private static int UpdateUkeireErrorRireki(
            NskAppContext db,
            int errorCount,
            string errorListName,
            string errorListPath,
            string errorListHashValue,
            DateTime sysDate,
            string torikomiRirekiId)
        {
            var entity = db.Set<T01070大量データ取込履歴>()
                           .FirstOrDefault(x => x.取込履歴id == torikomiRirekiId);
            if (entity == null)
            {
                return 0;
            }

            entity.ステータス = "99";
            entity.エラー件数 = errorCount;
            entity.エラーリスト名 = errorListName;
            entity.エラーリストパス = errorListPath;
            entity.エラーリストハッシュ値 = errorListHashValue;
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
        /// <summary>
        /// 大量データ受入基準収穫量okリスト取得
        /// </summary>
        private static List<T19020大量データ受入基準収穫量ok> GetOkListByUkeireRirekiId(NskAppContext db, string ukeireRirekiId)
        {
            logger.Info($"履歴ID {ukeireRirekiId} のデータを取得します。");
            return db.Set<T19020大量データ受入基準収穫量ok>()
                     .Where(x => x.受入履歴id == long.Parse(ukeireRirekiId))
                     .OrderBy(x => x.行番号)
                     .ToList();
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
        /// T11060基準収穫量設定取得
        /// </summary>
        private static int GetKijunShukakuryoCount(
            NskAppContext db
            , string kumiaitoCd
            , short? nensan
            , string kyosaimokutekiCd
            , string kumiaiintoCd
            , string ruiKbn
            , string sanchibetuMeigaraCd
            , string einouTaishoFlg)
        {
            return db.Set<T11060基準収穫量設定>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == nensan &&
                                 x.共済目的コード == kyosaimokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd &&
                                 x.類区分 == ruiKbn &&
                                 x.産地別銘柄コード == sanchibetuMeigaraCd &&
                                 x.営農対象外フラグ == einouTaishoFlg);
        }

        /// <summary>
        /// T11070基準収穫量設定規格別削除処理
        /// </summary>
        private static void Delete11070Records(
                NskAppContext db,
                string kumiaiCd,                // 組合等コード
                short? nensan,                       // 年産
                string kyosaiMokutekiCode,        // 共済目的コード
                string ruiKbn,                    // 類区分
                string sanchiBetsuMeigaraCode,     // 産地別銘柄コード
                string einouTaishogaiFlg)           // 営農対象外フラグ
        {

            // 規格の条件 "1"～"40" の文字列リストを作成
            var allowedKikaku = Enumerable.Range(1, 40)
                                          .Select(i => i.ToString())
                                          .ToList();

            // LINQクエリで条件に合致するレコードを取得
            var entities = db.Set<T11070基準収穫量設定規格別>()
                             .Where(x => x.組合等コード == kumiaiCd &&
                                         x.年産 == nensan &&
                                         x.共済目的コード == kyosaiMokutekiCode &&
                                         x.類区分 == ruiKbn &&
                                         x.産地別銘柄コード == sanchiBetsuMeigaraCode &&
                                         x.営農対象外フラグ == einouTaishogaiFlg &&
                                         allowedKikaku.Contains(x.規格))
                             .ToList();

            if (entities.Any())
            {
                db.Set<T11070基準収穫量設定規格別>().RemoveRange(entities);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// T11060基準収穫量設定削除処理
        /// </summary>
        private static void Delete11060Records(
                NskAppContext db,
                string kumiaiCd,                // 組合等コード
                short? nensan,                       // 年産
                string kyosaiMokutekiCode,        // 共済目的コード
                string ruiKbn,                    // 類区分
                string sanchiBetsuMeigaraCode,     // 産地別銘柄コード
                string einouTaishogaiFlg)           // 営農対象外フラグ
        {
            // LINQクエリで条件に合致するレコードを取得
            var entities = db.Set<T11060基準収穫量設定>()
                             .Where(x => x.組合等コード == kumiaiCd &&
                                         x.年産 == nensan &&
                                         x.共済目的コード == kyosaiMokutekiCode &&
                                         x.類区分 == ruiKbn &&
                                         x.産地別銘柄コード == sanchiBetsuMeigaraCode &&
                                         x.営農対象外フラグ == einouTaishogaiFlg)
                             .ToList();

            if (entities.Any())
            {
                db.Set<T11060基準収穫量設定>().RemoveRange(entities);
                db.SaveChanges();
            }
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
        /// 全角変換メソッド
        /// </summary>
        private static string ToFullWidth(int number)
        {
            // 数字を文字列に変換し、各桁を全角に変換
            return string.Concat(number.ToString().Select(c => (char)('０' + (c - '0'))));
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
            public string 取込ファイル_変更前ファイル名 { get; set; }
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
