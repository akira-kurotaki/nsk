using BAS_B0000_Management.Common;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using ModelLibrary.Context;
using NLog;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Timers;

namespace BAS_B0000_Management
{
    /// <summary>
    /// 巡回プログラム
    /// </summary>
    class Program
    {
        /// <summary>
        /// appsettings.jsonに設定あるDB参照時間間隔(単位：秒)
        /// </summary>
        private static int databaseRefTime = 0;

        /// <summary>
        /// appsettings.jsonに設定あるシステム時間フラグ
        /// </summary>
        private static bool sysDateTimeFlag = false;

        /// <summary>
        /// appsettings.jsonに設定あるシステム時間ファイルパス
        /// </summary>
        private static string sysDateTimePath = string.Empty;

        /// <summary>
        /// 実行中フラグ
        /// </summary>
        private static bool executingFlg = false;

        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 処理時間計測用ストップウォッチ
        /// </summary>
        //private static Dictionary<long, Stopwatch> stopwatchs = new();

        /// <summary>
        /// 
        /// </summary>
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
            try
            {
                logger.Info("巡回プログラム開始");

                // 設定ファイルから値を取得

                // DB参照時間間隔(単位：秒)
                bool isDatabaseRefTime = int.TryParse(ConfigUtil.Get(Constants.DATABASE_REF_TIME_TAG_NAME), out databaseRefTime);
                if (!isDatabaseRefTime)
                {
                    logger.Error(MessageUtil.Get("ME90015", "DB参照時間間隔(単位：秒)"));
                    return;
                }

                // システム時間フラグ
                bool isSysDateTimeFlag = bool.TryParse(ConfigUtil.Get(Constants.SYS_DATE_TIME_FLAG), out sysDateTimeFlag);
                if (!isSysDateTimeFlag)
                {
                    logger.Error(MessageUtil.Get("ME90015", "システム時間フラグ"));
                    return;
                }

                if (sysDateTimeFlag)
                {
                    // システム時間ファイルパス
                    sysDateTimePath = ConfigUtil.Get(Constants.SYS_DATE_TIME_PATH);
                    if (string.IsNullOrEmpty(sysDateTimePath))
                    {
                        logger.Error(MessageUtil.Get("ME90015", "システム時間ファイルパス"));
                        return;
                    }
                }

                // DB参照時間間隔のタイマ
                var timer = new System.Timers.Timer();
                // Intervalの設定単位はミリ秒（1秒 = 1000ミリ秒）
                timer.Interval = databaseRefTime * 1000;
                // タイマイベント処理(時間経過後の処理)を登録する
                timer.Elapsed += Timer_Elapsed;
                // イベントElapsedを発生させる
                timer.Enabled = true;
            }
            catch (Exception ex)
            {
                var message = ex.Message + string.Join(string.Empty, new string[]{
                    ex.InnerException == null ? string.Empty : Constants.NEW_LINE_SEPARATOR + ex.InnerException.ToString(),
                    string.IsNullOrEmpty(ex.StackTrace) ? string.Empty : Constants.NEW_LINE_SEPARATOR + ex.StackTrace
                });
                logger.Error(message);
            }

            Console.ReadKey();
            logger.Info("巡回プログラム終了");
        }

        /// <summary>
        /// DB参照時間間隔のタイマが時間間隔を経過すると発生するイベントハンドラ
        /// </summary>
        /// <param name="sender">タイマへの参照</param>
        /// <param name="e">Elapsedイベントのデータ</param>
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool isSetExecutingFlg = false;
            try
            {
                if (!executingFlg)
                {
                    // [静的変数：実行中フラグ]をTrueにする。
                    executingFlg = true;
                    isSetExecutingFlg = true;

                    // 処理時間計測用ストップウォッチをクリアする
                    //stopwatchs.Clear();

                    // 『実行可能バッチ予約取得』インターフェースに引数を渡す。
                    string message = string.Empty;
                    var batchYoyaku = BatchUtil.GetRunBatchYoyaku(ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
                                                                    null,
                                                                    Dns.GetHostName(),
                                                                    Constants.BATCH_USER_ID,
                                                                    ref message);

                    // エラーメッセージが設定されている場合
                    if (!string.IsNullOrEmpty(message))
                    {
                        logger.Info("実行可能バッチ予約取得：" + message);
                    }
                    else if (batchYoyaku != null)
                    {
                        // バッチ予約データが設定されている場合

                        // "バッチプログラム開始 バッチID：{0} 予約ID：{1}"　引数{0}：戻り値のバッチID、引数{1}：戻り値のバッチ条件のキー情報）
                        logger.Info("バッチプログラム開始 " + string.Format(Constants.LOG_MSG, batchYoyaku.BatchId, batchYoyaku.BatchJoken));

                        // 「バッチプログラム」を非同期で実行する。
                        ExcuteBatchYoyaku(batchYoyaku);
                    }
                }
                else
                {
                    // バッチプログラム実行中の場合、次の処理をしない
                    //logger.Info("DB参照時間間隔になったが、バッチプログラムが実行中のため、バッチ予約管理テーブルのデータを処理しない");
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message + string.Join(string.Empty, new string[]{
                    ex.InnerException == null ? string.Empty : Constants.NEW_LINE_SEPARATOR + ex.InnerException.ToString(),
                    string.IsNullOrEmpty(ex.StackTrace) ? string.Empty : Constants.NEW_LINE_SEPARATOR + ex.StackTrace
                });
                logger.Error(message);
            }

            if (isSetExecutingFlg)
            {
                // [静的変数：実行中フラグ]をFalseにする。
                executingFlg = false;
            }
        }

        /// <summary>
        /// バッチプログラムを非同期で実行
        /// </summary>
        /// <param name="batchYoyaku">バッチ予約状況</param>
        private static void ExcuteBatchYoyaku(BatchYoyaku batchYoyaku)
        {
            Task.Run(() =>
            {
                //var stopwatch = new Stopwatch();
                //stopwatch.Start();
                //stopwatchs.Add(batchYoyaku.BatchId, stopwatch);

                // 「バッチプログラム」を取得する。
                var batchPath = GetBatchPath(batchYoyaku.BatchNm);

                // 取得した件数が1件以外の場合
                if (string.IsNullOrEmpty(batchPath))
                {
                    // 『バッチ実行状況更新』インターフェースに引数を渡す。

                    var message = MessageUtil.Get("ME90015", "予約処理マスタ");

                    // バッチ実行状況更新
                    string refMessage = string.Empty;
                    if (BatchUtil.UpdateBatchYoyakuSts(batchYoyaku.BatchId, Constants.STATUS_ERROR, message, Constants.BATCH_USER_ID, ref refMessage) == 0)
                    {
                        // 更新に失敗した場合
                        logger.Error(refMessage);
                        logger.Error(string.Format(Constants.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, batchYoyaku.BatchId, Constants.STATUS_ERROR, message));
                    }
                    else
                    {
                        // 更新に成功した場合
                        logger.Info(string.Format(Constants.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, batchYoyaku.BatchId, Constants.STATUS_ERROR, message));
                    }
                }
                else
                {
                    // 「バッチプログラム」を非同期で実行する。
                    CallBatchProgram(batchPath, batchYoyaku);
                }
            });
        }

        /// <summary>
        /// バッチプログラムを実行
        /// </summary>
        /// <param name="batchPath">実行するバッチプログラム</param>
        /// <param name="batchYoyaku">バッチ予約状況</param>
        private static void CallBatchProgram(string batchPath, BatchYoyaku batchYoyaku)
        {
            try
            {
                Process p = new();

                // 実行するファイル
                p.StartInfo.FileName = batchPath;

                // 引数
                var argsSb = new StringBuilder();
                AddArguments(argsSb, batchYoyaku.BatchId.ToString());
                AddArguments(argsSb, batchYoyaku.TodofukenCd);
                AddArguments(argsSb, batchYoyaku.KumiaitoCd);
                AddArguments(argsSb, batchYoyaku.ShishoCd);
                AddArguments(argsSb, batchYoyaku.BatchJoken);

                p.StartInfo.Arguments = argsSb.ToString();

                // コンソールを開かない
                p.StartInfo.CreateNoWindow = true;
                // シェル機能を使用しない
                p.StartInfo.UseShellExecute = false;
                // 標準出力をリダイレクト
                p.StartInfo.RedirectStandardOutput = true;
                // エラー情報標準出力をリダイレクト
                p.StartInfo.RedirectStandardError = true;

                //イベントハンドラの追加
                p.Exited += new EventHandler(ProcessExited);
                //プロセスが終了したときに Exited イベントを発生させる
                p.EnableRaisingEvents = true;

                // アプリの実行開始
                logger.Debug(string.Format("バッチプログラム実行 バッチプログラム：{0}、引数：{1}", batchPath, argsSb));
                p.Start();
            }
            catch (Exception ex)
            {
                var message = ex.Message + string.Join(string.Empty, new string[]{
                    ex.InnerException == null ? string.Empty : Constants.NEW_LINE_SEPARATOR + ex.InnerException.ToString(),
                    string.IsNullOrEmpty(ex.StackTrace) ? string.Empty : Constants.NEW_LINE_SEPARATOR + ex.StackTrace
                });
                logger.Error(string.Format(Constants.ERROR_LOG, batchYoyaku.BatchId, message));
            }
        }

        /// <summary>
        /// バッチプログラムのプロセス終了のイベントハンドラ
        /// </summary>
        /// <param name="sender">プロセスへの参照</param>
        /// <param name="e">Exitedイベントのデータ</param>
        private static void ProcessExited(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    // プロセスから引数を取得
                    var arguments = ((Process)sender).StartInfo.Arguments.Split(new string[] { Constants.HALF_WIDTH_SPACE }, StringSplitOptions.RemoveEmptyEntries);

                    // バッチID
                    var batchId = long.Parse(arguments[0]);
                    // バッチプログラムのプルセスの終了コード
                    var result = ((Process)sender).ExitCode;
                    // エラー情報
                    string errorInfo = ((Process)sender).StandardError.ReadToEnd();

                    // プロセスを破棄する 
                    if ((Process)sender != null)
                    {
                        ((Process)sender).Close();
                        ((Process)sender).Dispose();
                    }

                    // バッチプログラムの実行が正常終了する場合
                    if (result == Constants.BATCH_EXECUT_SUCCESS)
                    {
                        logger.Info(string.Format(
                                        Constants.LOG_MSG,
                                        batchId,
                                        "バッチ実行終了",
                                        string.Empty,
                                        string.Empty));
                    }
                    else
                    {
                        // バッチプログラムの実行中にエラーが発生する場合
                        logger.Error(string.Format(
                                        Constants.ERROR_LOG,
                                        batchId,
                                        "バッチプログラムの実行中にエラーが発生した。" +
                                        Constants.NEW_LINE_SEPARATOR +
                                        errorInfo.Replace("\r", "").Replace("\n", "")));
                    }

                    //var stopwatch = stopwatchs[batchId];
                    //stopwatch.Stop();
                    //var excutingTime = Constants.LOG_TIMER_START_MESSAGE + Constants.HALF_WIDTH_SPACE + stopwatch.ElapsedMilliseconds.ToString() + Constants.HALF_WIDTH_SPACE + Constants.LOG_TIMER_END_MESSAGE;
                    //logger.Info(string.Format(
                    //                Constants.LOG_MSG,
                    //                batchId,
                    //                "バッチ処理終了。",
                    //                excutingTime,
                    //                string.Empty));
                }
                catch (Exception ex)
                {
                    var message = ex.Message + string.Join(string.Empty, new string[]{
                            ex.InnerException == null ? string.Empty : Constants.NEW_LINE_SEPARATOR + ex.InnerException.ToString(),
                            string.IsNullOrEmpty(ex.StackTrace) ? string.Empty : Constants.NEW_LINE_SEPARATOR + ex.StackTrace
                        });
                    logger.Error(message);
                }
            });
        }

        /// <summary>
        /// 端末のIPを取得
        /// </summary>
        /// <returns>端末のIP</returns>
        private static string GetIPAddress()
        {
            var ipAddress = string.Empty;

            // ホスト名を取得する
            string hostname = Dns.GetHostName();

            // ホスト名からIPアドレスを取得する
            IPAddress[] adrList = Dns.GetHostAddresses(hostname);

            foreach (IPAddress address in adrList)
            {
                // IPv4 のみを追加する
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAddress = address.ToString();
                }
            }

            return ipAddress;
        }

        /// <summary>
        /// プログラム引数を作成する
        /// パラメータ値が未設定（nullまたは空文字）の場合は、""とする。
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="value">パラメータ値</param>
        private static void AddArguments(StringBuilder builder, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = "\"\"";
            }
            builder.Append(value + Constants.HALF_WIDTH_SPACE);
        }

        /// <summary>
        /// 「実行対象バッチパス」を取得する。
        /// </summary>
        /// <param name="batchNm">バッチ名</param>
        /// <returns>実行対象バッチパス</returns>
        private static string GetBatchPath(string batchNm)
        {
            using (var db = new SystemCommonContext())
            {
                return db.MYoyakus.Where(a => a.SystemKbn == ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN) &&
                                                a.BatchNm == batchNm).Select(a => a.BatchPath).SingleOrDefault();
            }
        }
    }
}
