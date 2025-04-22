using NLog;
using NskReportMain.Common;
using Core = CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NskAppModelLibrary.Context;
using NskCommon = NskCommonLibrary.Core.Consts;
using NSK_P111020.Controllers;
using System.Text;

namespace NSK_B111020
{
    /// <summary>
    /// 交付金申請書（別記様式第１号）
    /// </summary>
    class Program
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static Program()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// 交付金申請書（別記様式第１号）
        /// </summary>
        /// <param name="args">
        /// 引数配列要素1：バッチID
        /// 引数配列要素2：都道府県コード
        /// 引数配列要素3：組合等コード
        /// 引数配列要素4：支所コード
        /// 引数配列要素5：バッチ条件のキー情報
        /// </param>
        public static void Main(string[] args)
        {
            // 引数
            // バッチID
            string bid = string.Empty;
            // 都道府県コード
            string todofukenCd = string.Empty;
            // 組合等コード
            string kumiaitoCd = string.Empty;
            // 支所コード
            string shishoCd = string.Empty;
            // バッチ条件のキー情報
            string jid = string.Empty;

            // バッチ予約ユーザID
            string batchYoyakuId = string.Empty;
            // [変数：エラーメッセージ] string.Empty
            string errorMessage = string.Empty;
            // [変数：ステータス] "03"（成功）
            string status = NskCommon.CoreConst.STATUS_SUCCESS;
            // 処理結果（正常：0、エラー：1）
            int result = NskCommon.CoreConst.BATCH_EXECUT_SUCCESS;

            try
            {
                // １．設定ファイルから、以下の内容を取得し、グローバル変数へ保存する。
                // １．１．上記の何れか（DBデフォルトスキーマを除外）が正しく取得できなかった場合、以下のエラーメッセージをログに出力し、処理を中断する。
                // システム時間フラグ    （検索キー：SysDateTimeFlag）
                // appsetting.jsonの設定にあるシステム時間フラグ
                bool sysDateTimeFlag = false;
                if (bool.TryParse(ConfigUtil.Get(Core.CoreConst.SYS_DATE_TIME_FLAG), out sysDateTimeFlag))
                {
                    // システム時間ファイルパス （検索キー：SysDateTimePath）※本項目は「システム時間フラグ」が"true"で取得できた場合のみ対象
                    // appsetting.jsonの設定にあるシステム時間ファイルパス
                    string sysDateTimePath = string.Empty;
                    if (sysDateTimeFlag)
                    {
                        // システム時間ファイルパス
                        sysDateTimePath = ConfigUtil.Get(Core.CoreConst.SYS_DATE_TIME_PATH);

                        if (string.IsNullOrEmpty(sysDateTimePath))
                        {
                            // エラーメッセージをログに出力し、処理を中断する。
                            // （"ME90015"、{0}：システム時間ファイルパス）
                            throw new AppException("ME90015", MessageUtil.Get("ME90015", "システム時間ファイルパス"));
                        }
                    }
                }
                else
                {
                    // エラーメッセージをログに出力し、処理を中断する。
                    // （"ME90015"、{0}：システム時間フラグ）
                    throw new AppException("ME90015", MessageUtil.Get("ME90015", "システム時間フラグ"));
                }

                // ２．システム日付の設定を行う。
                // ２．１．システム日付の設定
                // （ア）[変数：システム日付] に以下の条件に従って設定を行う。
                // （１）[グローバル変数：システム時間フラグ] がtrueの場合、[グローバル変数：システム時間ファイルパス] の年月日＋マシン時間を設定する。
                // （２）上記以外、マシン時間を設定する。
                // [変数：システム日付]
                DateTime systemDate = DateUtil.GetSysDateTime(); // DateUtil.GetSysDateTime()内で全てやっている

                // args から 各変数へ展開する
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

                // ３．引数のチェック
                // ３．１．必須チェック
                // ３．１．１．[変数：バッチID] が未入力の場合
                if (string.IsNullOrEmpty(bid))
                {
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、「８.」へ進む。
                    // （"ME01645" 引数{0} ：パラメータの取得）
                    throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
                }

                // ３．１．２．[変数：バッチID]が数値変換不可の場合
                // 数値化したバッチID
                int nBid = 0;
                if (!int.TryParse(bid, out nBid))
                {
                    // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、「８.」へ進む。
                    // （"ME90012"　引数{0} ：バッチID)
                    throw new AppException("ME90012", MessageUtil.Get("ME90012", "バッチID"));
                }

                // ３．１．３．[変数：都道府県コード]が未入力の場合
                if (string.IsNullOrEmpty(todofukenCd))
                {
                    // 以下のエラーメッセージを [変数：エラーメッセージ] に設定し、「８.」へ進む。
                    // （"ME01054"　引数{0} ：都道府県コード)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "都道府県コード"));
                }

                //３．１．４．[変数：組合等コード] が未入力の場合
                if (string.IsNullOrEmpty(kumiaitoCd))
                {
                    //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、「８.」へ進む。
                    //（"ME01054" 引数{0} ：組合等コード)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "組合等コード"));
                }

                //３．１．５．[変数：支所コード] が未入力の場合
                if (string.IsNullOrEmpty(shishoCd))
                {
                    //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、「８.」へ進む。
                    //（"ME01054" 引数{0} ：支所コード)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "支所コード"));
                }

                //３．１．６．[変数：バッチ条件のキー情報] が未入力の場合
                if (string.IsNullOrEmpty(jid))
                {
                    //以下のエラーメッセージを[変数：エラーメッセージ] に設定し、「８.」へ進む。
                    //（"ME01054" 引数{0} ：バッチ条件のキー情報)
                    throw new AppException("ME01054", MessageUtil.Get("ME01054", "バッチ条件のキー情報"));
                }

                // バッチ予約状況取得
                // バッチ予約状況取得引数の設定
                BatchUtil.GetBatchYoyakuListParam batchYoyakuListparam = new()
                {
                    SystemKbn = Core.CoreConst.SystemKbn.Nsk,
                    TodofukenCd = todofukenCd,
                    KumiaitoCd = kumiaitoCd,
                    ShishoCd = shishoCd,
                    BatchId = (long)nBid
                };
                // 総件数取得フラグ
                bool boolAllCntFlg = false;
                // 件数（出力パラメータ）
                int intAllCnt = 0;
                // エラーメッセージ（出力パラメータ）
                string message = string.Empty;
                // バッチ予約状況取得登録（BatchUtil.GetBatchYoyakuList()）を呼び出し、バッチ予約状況を取得する。
                List<BatchYoyaku> batchYoyakus = BatchUtil.GetBatchYoyakuList(batchYoyakuListparam, boolAllCntFlg, ref intAllCnt, ref message);

                // バッチ予約が存在しない場合、
                if (batchYoyakus.Count == 0)
                {
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01645" 引数{0} ：パラメータの取得)
                    throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
                }

                // バッチ予約が存在する場合
                BatchYoyaku? batchYoyaku = batchYoyakus.FirstOrDefault(x => x.BatchId == nBid);
                // [引数：バッチID]に一致する場合
                if (batchYoyaku is not null)
                {
                    // 取得した「バッチ予約状況」から値を取得し変数に設定する。
                    // バッチ予約ユーザID = バッチ予約情報.予約ユーザID
                    batchYoyakuId = batchYoyaku.BatchYoyakuId;
                }
                // [引数：バッチID]に一致するバッチ予約状況が取得できない場合、
                else
                {
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01645" 引数{0} ：パラメータの取得)
                    throw new AppException("ME01645", MessageUtil.Get("ME01645", "パラメータの取得"));
                }

                // ４．DB接続
                // ※「共通機能設計_070_DB切替」の「バッチのDB接続先取得処理」を参照。
                // システム共通スキーマからログインユーザの所属に応じた都道府県別事業スキーマのDB接続先を取得する
                // DB接続情報
                DbConnectionInfo? dbInfo = null;
                dbInfo = DBUtil.GetDbConnectionInfo(Core.CoreConst.SystemKbn.Nsk, todofukenCd, kumiaitoCd, shishoCd);
                if (dbInfo == null)
                {
                    throw new AppException("ME90014", MessageUtil.Get("ME90014"));
                }

                using (NskAppContext dbContext = new(dbInfo.ConnectionString, dbInfo.DefaultSchema, ConfigUtil.GetInt(Core.CoreConst.COMMAND_TIMEOUT)))
                {
                    // ５．コードの整合性チェック
                    IsConsistency(dbContext, todofukenCd, kumiaitoCd, shishoCd);

                    // ６．帳票を作成する。
                    // ６．１．「帳票制御処理_NSK_111020P_交付金申請書（別記様式第１号）」を呼び出す。
                    // 【引数】
                    List<string> hikisu =
                    [
                        "NSK_111020P",
                        bid,
                        todofukenCd,
                        kumiaitoCd,
                        shishoCd,
                        jid
                    ];
                    var controllerResult = new ControllerResult();
                    using (var nsk111020PController = new NSK_111020PController(dbInfo))
                    {
                        controllerResult = nsk111020PController.ManageReports(dbContext, hikisu.ToArray());
                    }

                    // ６．１．２．失敗した(戻り値(0)以外）場合
                    if (controllerResult.Result != 0)
                    {
                        // 戻り値で返されたエラーメッセージを、[変数：エラーメッセージ]に設定し、「８．」へ進む。
                        throw new AppException(controllerResult.ErrorMessageId, controllerResult.ErrorMessage);
                    }

                    // ６．１．１．成功した(戻り値(0)）場合
                    // （１）[変数：処理ステータス]に"03"（成功）を設定する。
                    // （２）「７．」へ進む。
                    // 処理正常終了時
                    status = NskCommon.CoreConst.STATUS_SUCCESS;

                    // [変数：エラーメッセージ] に正常終了メッセージを設定
                    // （"MI10005"：処理が正常に終了しました。)
                    errorMessage = MessageUtil.Get("MI10005");
                    logger.Info(errorMessage);
                }

                // ７．バッチ実行状況更新
                string refMessage = string.Empty;

                // ７．１．『バッチ実行状況更新』インターフェースに引数を渡す。
                // ７．２．『バッチ実行状況更新』インターフェースから戻り値を受け取る。
                // バッチ実行状況更新（BatchUtil.UpdateBatchYoyakuSts()）を呼び出し、ステータスを更新する。
                if (BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), status, errorMessage, batchYoyakuId, ref refMessage) == BatchUtil.RET_FAIL)
                {
                    // （１）失敗の場合
                    logger.Error(string.Format(NskCommon.CoreConst.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, refMessage));
                    result = NskCommon.CoreConst.BATCH_EXECUT_FAILED;
                }
                else
                {
                    // （２）成功の場合
                    logger.Info(string.Format(NskCommon.CoreConst.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, bid, status, refMessage));
                    result = NskCommon.CoreConst.BATCH_EXECUT_SUCCESS;
                }

                // ７．３．処理を終了する。
                Environment.ExitCode = result;
            }
            catch (Exception ex)
            {
                if (ex is AppException aEx)
                {
                    // エラーメッセージをERRORログに出力して「８．」へ進む。
                    logger.Error(aEx.Message);
                }
                // ８．エラー処理
                string refMessage = string.Empty;

                // ８．１．例外（エクセプション）の場合
                // [変数：ステータス]を「99：エラー」に更新する。
                status = NskCommon.CoreConst.STATUS_ERROR;

                // [変数：エラーメッセージ] にエラーメッセージを設定
                //（"MF00001"：予期せぬエラーが発生しました。システム管理者に連絡してください。)
                errorMessage = MessageUtil.Get("MF00001");

                // ８．２．共通機能の「バッチ実行状況更新」を呼び出し、バッチ予約テーブルを更新する。
                BatchUtil.UpdateBatchYoyakuSts(long.Parse(bid), status, errorMessage, batchYoyakuId, ref refMessage);
            }
        }

        /// <summary>
        /// コードの整合性チェック
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="todofukenCd">都道府県コード</param>
        /// <param name="kumiaitoCd">組合等コード</param>
        /// <param name="shishoCd">支所コード</param>
        /// <exception cref="AppException"></exception>
        public static void IsConsistency(NskAppContext dbContext, string todofukenCd, string kumiaitoCd, string shishoCd)
        {
            // ５．１．「都道府県コード存在情報」を取得する。
            // 都道府県コード存在情報を取得する
            int todofukenCdCounter = dbContext.VTodofukens
                .Where(x => x.TodofukenCd == todofukenCd)
                .Select(x => x.TodofukenCd)
                .Count();
            // ５．２．データが取得できない場合（該当データがマスタデータに登録されていない場合）、
            if (todofukenCdCounter == 0)
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「８.」へ進む。
                // （"ME10005"　引数{0} ：都道府県コード)
                throw new AppException("ME10005", MessageUtil.Get("ME10005", "都道府県コード"));
            }

            // ５．３．[変数：組合等コード] が入力されている場合、「組合等コード存在情報」を取得する。
            // 組合等コード存在情報を取得する
            int kumiaitoCdCounter = dbContext.VKumiaitos
                .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd)
                .Select(x => x.KumiaitoCd)
                .Count();
            // ５．４．データが取得できない場合（該当データがマスタデータに登録されていない場合）、
            if (kumiaitoCdCounter == 0)
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「８.」へ進む。
                // （"ME10005"　引数{0} ：組合等コード)
                throw new AppException("ME10005", MessageUtil.Get("ME10005", "組合等コード"));
            }

            // ５．５．[変数：支所コード]が入力されている場合、「支所コード存在情報」を取得する。
            // 支所コード存在情報存在情報を取得する
            int shishoCdCounter = dbContext.VShishoNms
                .Where(x => x.TodofukenCd == todofukenCd && x.KumiaitoCd == kumiaitoCd && x.ShishoCd == shishoCd)
                .Select(x => x.ShishoCd)
                .Count();
            // ５．６．データが取得できない場合（該当データがマスタデータに登録されていない場合）、
            if (shishoCdCounter == 0)
            {
                // 以下のエラーメッセージを設定し、ERRORログに出力して「８.」へ進む。
                // （"ME10005"　引数{0} ：支所コード)
                throw new AppException("ME10005", MessageUtil.Get("ME10005", "支所コード"));
            }
        }
    }
}
