using NSK_B112121.Common;
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
using Microsoft.AspNetCore.Diagnostics;
using System.Text.RegularExpressions;
using System.IO.Compression;
using Microsoft.AspNetCore.Http.HttpResults;

namespace NSK_B112121
{
    /// <summary>
    /// 定時実行予約登録
    /// </summary>
    class Program
    {
        /// <summary>
        /// バッチ名
        /// </summary>
        private static string BATCH_NAME = "組合員等類別平均単収単価大量受入データ取込";
        private static string BATCH_USER_NAME = "NSK_112121B";

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
            logger.Info(string.Concat(CoreConst.LOG_START_KEYWORD, " " + BATCH_NAME));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //ConfigUtil.Get(

            var bid = string.Empty;
            var todofukenCd = string.Empty;
            var kumiaitoCd = string.Empty;
            var shishoCd = string.Empty;
            var jid = string.Empty;

            var batchSts = "";
            var torikomiRirekiId = "";

            int result = Constants.BATCH_EXECUT_SUCCESS;
            var errorMsg = string.Empty;
            var shoriSts = "99";
            bool errFlg = false;
            int okCount = 0;
            int errCount = 0;
            int total = 0;



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

            
            var torikomiErrorList = new List<ErrorList>();
            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema, ConfigUtil.GetInt(Constants.CONFIG_COMMAND_TIMEOUT)))
            {
                var transaction = db.Database.BeginTransaction();
                try
                {
                    // バッチ条件情報取得
                    T01050バッチ条件 joukenList = GetBatchJoukenList(db, jid);
                    if (joukenList == null)
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        logger.Error(errorMsg);
                        goto forcedShutdown;
                    }
                    if (Constants.JOUKEN_TORIKOMI_ID.Equals(joukenList.条件名称))
                    {
                        torikomiRirekiId = joukenList.条件値;
                    }

                    if (string.IsNullOrEmpty(torikomiRirekiId))
                    {
                        errorMsg = MessageUtil.Get("ME01645", "バッチ条件の取得");
                        logger.Error(errorMsg);
                        goto forcedShutdown;
                    }

                    // 取込履歴取得
                    T01070大量データ取込履歴 tairyoData = GetTairyoDataTorikomi(db, joukenList.条件値);

                    if (tairyoData == null)
                    {
                        errorMsg = MessageUtil.Get("ME01645", "パラメータの取得");
                        logger.Error(errorMsg);
                        goto forcedShutdown;
                    }

                    torikomiRirekiId = tairyoData.取込履歴id;

                    //取込ステータス、組合等コード
                    var torikomiSts = tairyoData.ステータス;
                    var torikomiCd = tairyoData.組合等コード;
                    if (!"01".Equals(torikomiSts))
                    {
                        errorMsg = MessageUtil.Get("ME10042", "取込処理");
                        logger.Error(errorMsg);
                        goto forcedShutdown;
                    }
                    else if ("01".Equals(torikomiSts))
                    {
                        torikomiSts = "02";
                    }
                    UpdateT01070TorikomiRireki(db, tairyoData.取込履歴id, torikomiSts, sysDate, BATCH_NAME);

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

                    // 受入OKテーブルからデータを取得
                    List<T19080大量データ受入組合員等類別平均単収ok> tairyoOkList = GetT19080Data(db);
                    if (tairyoOkList.IsNullOrEmpty())
                    {
                        errorMsg = MessageUtil.Get("ME10025", "取込対象");
                        goto forcedShutdown;
                    }
                    int errListSeq = 0;
                    
                    for (int i = 0; i < tairyoOkList.Count; i++)
                    {
                        total++;
                        errFlg = false;
                        // １０．３．属性チェックを行う
                        // ----- 組合等コード（必須、数字3桁） -----
                        if (string.IsNullOrWhiteSpace(tairyoOkList[i].組合等コード))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "組合等コード", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        if (tairyoOkList[i].組合等コード.Length > 3)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "組合等コード", "3", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        if (!Regex.IsMatch(tairyoOkList[i].組合等コード, @"^\d{1,3}$"))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "組合等コード", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }

                        // ----- 年産（必須、数字4桁） -----
                        if (tairyoOkList[i].年産 == null)
                        {
                            errorMsg = MessageUtil.Get("ME00001", "年産", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        if (tairyoOkList[i].年産 > 9999)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "年産", "4", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        if (!Regex.IsMatch(tairyoOkList[i].年産.ToString(), @"^\d{1,4}$"))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "年産", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }

                        // ----- 共済目的コード（必須、数字2桁） -----
                        if (string.IsNullOrWhiteSpace(tairyoOkList[i].共済目的コード))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "共済目的コード", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        if (tairyoOkList[i].共済目的コード.Length > 2)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "共済目的コード", "2", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        if (!Regex.IsMatch(tairyoOkList[i].共済目的コード, @"^\d{1,2}$"))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "共済目的コード", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }

                        // ----- 組合員等コード（必須、数字13桁） -----
                        if (string.IsNullOrWhiteSpace(tairyoOkList[i].組合員等コード))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "組合員等コード", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        if (tairyoOkList[i].組合員等コード.Length > 13)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "組合員等コード", "13", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        if (!Regex.IsMatch(tairyoOkList[i].組合員等コード, @"^\d{1,13}$"))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "組合員等コード", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }

                        // ----- 類区分（必須、数字2桁） -----
                        if (string.IsNullOrWhiteSpace(tairyoOkList[i].類区分))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "類区分", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        if (tairyoOkList[i].類区分.Length > 2)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "類区分", "2", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        if (!Regex.IsMatch(tairyoOkList[i].類区分, @"^\d{1,2}$"))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "類区分", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }

                        // ----- 引受区分（必須、数字2桁） -----
                        if (string.IsNullOrWhiteSpace(tairyoOkList[i].引受区分))
                        {
                            errorMsg = MessageUtil.Get("ME00001", "引受区分", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        if (tairyoOkList[i].引受区分.Length > 2)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "引受区分", "2", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        if (!Regex.IsMatch(tairyoOkList[i].引受区分, @"^\d{1,2}$"))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "引受区分", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }

                        // ----- 全相殺基準単収（必須、数字4桁） -----
                        if (tairyoOkList[i].全相殺基準単収 == null)
                        {
                            errorMsg = MessageUtil.Get("ME00001", "全相殺基準単収", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        if (tairyoOkList[i].全相殺基準単収 > 9999)
                        {
                            errorMsg = MessageUtil.Get("ME00020", "全相殺基準単収", "4", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        if (!Regex.IsMatch(tairyoOkList[i].全相殺基準単収.ToString(), @"^\d{1,4}$"))
                        {
                            errorMsg = MessageUtil.Get("ME00003", "全相殺基準単収", "(" + tairyoOkList[i].行番号 + "行目)");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }

                        if (!tairyoOkList[i].組合等コード.Equals(kumiaitoCd))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "共済目的コード");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        

                        if (0 == GetKyosaiMokutekiCount(db, tairyoOkList[i].共済目的コード))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "共済目的コード");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }

                        if (0 == GetRuiKbnCount(db, tairyoOkList[i].類区分))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "類区分");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }

                        if (0 == GetHikiukeKbnCount(db, tairyoOkList[i].共済目的コード, tairyoOkList[i].引受区分))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "引受区分");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }

                        if (0 == GetNogyoshaCount(db, tairyoOkList[i].組合等コード, tairyoOkList[i].組合員等コード))
                        {
                            errorMsg = MessageUtil.Get("ME10016", "農業者情報管理システム", "組合員等コード");

                            var entityForFile = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entityForFile);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }

                        if (0 == GetKojinSettingTaguiCount(db, tairyoOkList[i].組合等コード
                            , tairyoOkList[i].年産, tairyoOkList[i].共済目的コード
                            , tairyoOkList[i].組合員等コード, tairyoOkList[i].類区分
                            , tairyoOkList[i].引受区分))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "引受区分");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }
                        
                        if (0 == GetKojinSettingCount(db, tairyoOkList[i].組合等コード
                            , tairyoOkList[i].年産, tairyoOkList[i].共済目的コード, tairyoOkList[i].組合員等コード))
                        {
                            errorMsg = MessageUtil.Get("ME10005", "引受区分");
                            var entity = new ErrorList
                            {
                                エラー内容 = errorMsg,
                                行番号 = tairyoOkList[i].行番号
                            };
                            torikomiErrorList.Add(entity);
                            errCount++;
                            errFlg = true;
                            goto skipChk;
                        }

                    skipChk:;

                        logger.Debug("T01080大量データ受入エラーリスト ; INSERST INS");
                        if (errFlg)
                        {
                            var dataErrorList = new T01080大量データ受入エラーリスト
                            {
                                処理区分 = "2",
                                履歴id = long.Parse(joukenList.条件値),
                                枝番 = errListSeq++,
                                行番号 = tairyoOkList[i].行番号.ToString(),
                                エラー内容 = errorMsg,
                                登録日時 = sysDate,
                                登録ユーザid = BATCH_USER_NAME,
                            };
                            // DbSet に追加
                            db.T01080大量データ受入エラーリストs.Add(dataErrorList);
                            
                            errCount++;
                        }
                        else
                        {
                            
                            okCount++;
                        }
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
                        string errFileName = $"{preNm}-ERR-{tairyoData.取込履歴id}.csv";
                        string tempFilePath = Path.Combine(tempFolderPath, errFileName);

                        // 10.1.2 エラーリストの内容をCSV形式で作成
                        // ヘッダ： "エラー対象行数","エラー内容"
                        // 各データ行もすべての項目をダブルクォーテーションで囲む
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("\"エラー対象行数\",\"エラー内容\"");
                        foreach (var error in torikomiErrorList)
                        {
                            // 行番号は数値→文字列変換、エラー内容はそのまま
                            sb.AppendLine(
                                $"\"{error.行番号}\"," +
                                $"\"{error.エラー内容}\""
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
                        byte[] encryptedData = CryptoUtil.Encrypt(csvBytes, errFileName);

                        // 出力先フォルダは例："C:\NSK\112041Real\[yyyyMMddHHmmss]\"
                        string outputRootFolder = @"C:\NSK\112041Real";
                        string outputFolder = Path.Combine(outputRootFolder, timestamp);
                        if (!Directory.Exists(outputFolder))
                        {
                            Directory.CreateDirectory(outputFolder);
                        }
                        // 出力先ファイル名は、エラーリストファイル名に ".zip" を付加（※内部に元のファイル名で格納）
                        string destinationZipFilePath = Path.Combine(outputFolder, errFileName + ".zip");
                        using (var zipStream = new FileStream(destinationZipFilePath, FileMode.Create))
                        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
                        {
                            // ZIP内のエントリ名は元のCSVファイル名とする
                            var zipEntry = archive.CreateEntry(errFileName);
                            using (var entryStream = zipEntry.Open())
                            {
                                entryStream.Write(encryptedData, 0, encryptedData.Length);
                            }
                        }
                        // 保存先のパスを変数に設定する
                        var errorListPath = destinationZipFilePath;
                        UpdateTorikomiHistory(db, torikomiRirekiId, torikomiSts, total, errCount, errFileName, errorListPath, errorListHashValue,sysDate, sysDate);
                        // 10.1.5 一時領域に作成したフォルダとファイルを削除する
                        Directory.Delete(tempFolderPath, true);
                        logger.Error(MessageUtil.Get("ME10026", "取込対象"));
                        goto forcedShutdown;
                    }


                    batchSts = "03";
                forcedShutdown:;
                    int updateResult = UpdateTorikomiStartDate(db, torikomiRirekiId, batchSts, sysDate, sysDate, BATCH_USER_NAME);
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
                    int resultYoyaku = BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), batchSts, errorMsg, BATCH_USER_NAME, ref refMessage);
                    if (0 == resultYoyaku)
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
        private static T01050バッチ条件 GetBatchJoukenList(NskAppContext db, string batchJoukenId)
        {

            // LINQクエリで該当エンティティを取得
            var result = db.Set<T01050バッチ条件>()
                .Where(b => b.バッチ条件id == batchJoukenId
                         && b.条件名称 == Constants.JOUKEN_TORIKOMI_ID)
                .FirstOrDefault();

            return result;
        }

        // 大量データ取込履歴の取得
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

            preNm = record.取込ファイル_変更前ファイル名;

            return record.x;
        }

        // 大量データ取込履歴の更新（ステータス、更新日時、更新ユーザID）
        private static int UpdateT01070TorikomiRireki(NskAppContext db, string torikomiRerekiId, string torikomiSts, DateTime updateDate, string updateUserId)
        {
            logger.Info("「大量データ取込履歴更新登録」の処理を実行します。");
            var entity = db.Set<T01070大量データ取込履歴>()
                           .FirstOrDefault(x => x.取込履歴id == torikomiRerekiId);
            if (entity == null)
            {
                return 0;
            }

            entity.ステータス = torikomiSts;
            entity.更新日時 = updateDate;
            entity.更新ユーザid = updateUserId;

            return db.SaveChanges();
        }

        private static int UpdateT11030KojinSetting(NskAppContext db, DateTime updateDate, string updateUserId)
        {
            logger.Info("「個人設定類」テーブルの更新処理を実行します。");
            var query = db.Set<T11030個人設定類>()
                            .Where(t => t.引受方式 == "3")
                            .Select(t => new
                            {
                                t,
                                全相殺基準単収 = db.Set<T19080大量データ受入組合員等類別平均単収ok>()
                                    .Where(ok =>
                                        ok.組合等コード == t.組合等コード &&
                                        ok.年産 == t.年産 &&
                                        ok.共済目的コード == t.共済目的コード &&
                                        ok.組合員等コード == t.組合員等コード &&
                                        ok.類区分 == t.類区分 &&
                                        ok.引受区分 == t.引受区分)
                                    .Select(ok => ok.全相殺基準単収)
                                    .FirstOrDefault()
                            });

            var list = query.ToList();

            foreach (var item in list)
            {
                item.t.全相殺基準単収 = item.全相殺基準単収;
                item.t.更新日時 = updateDate;
                item.t.更新ユーザid = updateUserId;
            }

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

        // T19030 大量データ受入 組合員等別補償割合等OK の全件取得
        private static List<T19080大量データ受入組合員等類別平均単収ok> GetT19080Data(NskAppContext db)
        {
            logger.Info("T19080大量データ受入組合員等類別平均単収ok の全データを取得します。");
            return db.Set<T19080大量データ受入組合員等類別平均単収ok>().ToList();
        }

        // 共済目的名称の件数取得
        private static int GetKyosaiMokutekiCount(NskAppContext db, string kyosaiMokutekiCd)
        {
            logger.Info($"共済目的コード {kyosaiMokutekiCd} の件数を取得します。");
            // 仮に m_00010_共済目的名称 に対応するエンティティを KyosaiMokuteki とする
            return db.Set<M00010共済目的名称>()
                     .Count(x => x.共済目的コード == kyosaiMokutekiCd);
        }

        // 類名称の件数取得
        private static int GetRuiKbnCount(NskAppContext db, string ruiKbn)
        {
            logger.Info($"類区分 {ruiKbn} の件数を取得します。");
            // 仮に m_00020_類名称 に対応するエンティティを RuiMei とする
            return db.Set<M00020類名称>()
                     .Count(x => x.類区分 == ruiKbn);
        }

        // 引受区分名称の件数取得
        private static int GetHikiukeKbnCount(NskAppContext db, string kyosaiMokutekiCd, string hikiukeKbn)
        {
            logger.Info($"共済目的コード {kyosaiMokutekiCd} と引受区分 {hikiukeKbn} の件数を取得します。");
            // 仮に m_10090_引受区分名称 に対応するエンティティを HikiukeKbn とする
            return db.Set<M10090引受区分名称>()
                     .Count(x => x.共済目的コード == kyosaiMokutekiCd && x.引受区分 == hikiukeKbn);
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

        // 個人設定類（引受方式3）の件数取得
        private static int GetKojinSettingTaguiCount(
            NskAppContext db,
            string kumiaitoCd,
            short? nensan,
            string kyosaiMokutekiCd,
            string kumiaiintoCd,
            string ruiKbn,
            string hikiukeKbn)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {kyosaiMokutekiCd}, " +
                        $"組合員等コード {kumiaiintoCd}, 類区分 {ruiKbn}, 引受区分 {hikiukeKbn}, 引受方式 3 の件数を取得します。");
            // 仮に t_11030_個人設定類 に対応するエンティティを KojinSettingRui とする
            return db.Set<T11030個人設定類>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == nensan &&
                                 x.共済目的コード == kyosaiMokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd &&
                                 x.類区分 == ruiKbn &&
                                 x.引受区分 == hikiukeKbn &&
                                 x.引受方式 == "3");
        }

        // 個人設定の件数取得
        private static int GetKojinSettingCount(
            NskAppContext db,
            string kumiaitoCd,
            short? nensan,
            string kyosaiMokutekiCd,
            string kumiaiintoCd)
        {
            logger.Info($"組合等コード {kumiaitoCd}, 年産 {nensan}, 共済目的コード {kyosaiMokutekiCd}, 組合員等コード {kumiaiintoCd} の件数を取得します。");
            // 仮に t_11010_個人設定 に対応するエンティティを KojinSetting とする
            return db.Set<T11010個人設定>()
                     .Count(x => x.組合等コード == kumiaitoCd &&
                                 x.年産 == nensan &&
                                 x.共済目的コード == kyosaiMokutekiCd &&
                                 x.組合員等コード == kumiaiintoCd);
        }

        // エラーリストの取得（処理区分が '2' かつ履歴ID）
        private static List<T01080大量データ受入エラーリスト> GetErrorListByTorikomiId(NskAppContext db, string torikomiId)
        {
            logger.Info($"処理区分 2, 履歴ID {torikomiId} のデータを取得します。");
            return db.Set<T01080大量データ受入エラーリスト>()
                     .Where(x => x.処理区分 == "2" && x.履歴id == long.Parse(torikomiId))
                     .ToList();
        }

        // 取込履歴の更新（複数項目）
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

        // 取込履歴の開始日時更新
        private static int UpdateTorikomiStartDate(
            NskAppContext db,
            string torikomiRirekiId,
            string torikomiSts,
            DateTime startDate,
            DateTime updateDate,
            string updateUser)
        {
            logger.Info($"取込履歴ID {torikomiRirekiId} のデータを更新します。");

            const int maxRetry = 3;
            int retryCount = 0;
            bool saveFailed;

            do
            {
                saveFailed = false;
                // エンティティを取得（再試行のたびに最新の状態を取得）
                var entity = db.Set<T01070大量データ取込履歴>()
                               .FirstOrDefault(x => x.取込履歴id == torikomiRirekiId);
                if (entity == null)
                {
                    logger.Error($"取込履歴ID {torikomiRirekiId} のデータが存在しません。");
                    return 0;
                }

                // 更新内容の設定
                entity.ステータス = torikomiSts;
                entity.開始日時 = startDate;
                entity.更新日時 = updateDate;
                entity.更新ユーザid = updateUser;

                logger.Debug("torikomiSts ; " + torikomiSts);
                logger.Debug("startDate ; " + startDate);
                logger.Debug("updateDate ; " + updateDate);
                logger.Debug("updateUser ; " + updateUser);

                try
                {
                    // 更新を試行
                    return db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    retryCount++;
                    logger.Warn($"同時実行例外が発生しました。再試行 {retryCount}/{maxRetry}");
                    // エラーが発生したエントリを最新の状態にリロードする
                    foreach (var entry in ex.Entries)
                    {
                        entry.Reload();
                    }
                }
            } while (saveFailed && retryCount < maxRetry);

            if (saveFailed)
            {
                logger.Error("最大再試行回数に達しました。更新に失敗します。");
                return 0;
            }

            return 1;
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
            public decimal 行番号 { get; set; }
        }
    }
}
