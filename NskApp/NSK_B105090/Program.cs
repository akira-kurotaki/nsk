using System.Text;
using NLog;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NskAppModelLibrary.Context;
using NskCommonLibrary.Core.Consts;
using NSK_B105090.Models;

namespace NSK_B105090
{
    /// <summary>
    /// 組合員等各種条件設定エラーリスト
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
        /// 組合員等各種条件設定エラーリスト
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
            Arguments arg = new (args);

            // [変数：エラーメッセージ] string.Empty
            string errorMessage = string.Empty;
            // [変数：ステータス]       STATUS_SUCCESS = "03"（成功）
            string status = CoreConst.STATUS_SUCCESS;
            // 処理結果（正常：0、エラー：1）
            int result = CoreConst.BATCH_EXECUT_SUCCESS;
            // バッチ予約ユーザID
            string batchYoyakuUid = string.Empty;

            try
            {
                // １．設定ファイルから、以下の内容を取得し、グローバル変数へ保存する。
                bool sysDateTimeFlag = false;
                if (bool.TryParse(ConfigUtil.Get(CoreConst.SYS_DATE_TIME_FLAG), out sysDateTimeFlag))
                {
                    // システム時間ファイルパス （検索キー：SysDateTimePath）※本項目は「システム時間フラグ」が"true"で取得できた場合のみ対象
                    // appsetting.jsonの設定にあるシステム時間ファイルパス
                    string sysDateTimePath = string.Empty;
                    if (sysDateTimeFlag)
                    {
                        // システム時間ファイルパス
                        sysDateTimePath = ConfigUtil.Get(CoreConst.SYS_DATE_TIME_PATH);

                        if (string.IsNullOrEmpty(sysDateTimePath))
                        {
                            // エラーメッセージをログに出力し、処理を中断する。
                            // （"ME90015"、{0}：システム時間ファイルパス）
                            errorMessage = MessageUtil.Get("ME90015", "システム時間ファイルパス");
                            throw new AppException("ME90015", errorMessage);
                        }
                    }
                }
                else
                {
                    // エラーメッセージをログに出力し、処理を中断する。
                    // （"ME90015"、{0}：システム時間フラグ）
                    errorMessage = MessageUtil.Get("ME90015", "システム時間フラグ");
                    throw new AppException("ME90015", errorMessage);
                }

                // ２．システム日付の設定を行う。
                // ２．１．システム日付の設定
                DateTime systemDate = DateUtil.GetSysDateTime();
                string sysDateTime = systemDate.ToString("yyyyMMddHHmmss");

                // ３．引数のチェック
                // ３．１．必須チェック
                // ３．１．１．以下の引数が未入力の場合、[変数：エラーメッセージ]を設定し、「１０.」へ進む。
                arg.IsRequired(ref errorMessage);

                // バッチ予約状況取得引数の設定
                BatchUtil.GetBatchYoyakuListParam yoyakuParam = new()
                {
                    SystemKbn = CoreConst.SystemKbn.Nsk,
                    TodofukenCd = arg.TodofukenCd,
                    KumiaitoCd = arg.KumiaitoCd,
                    ShishoCd = arg.ShishoCd,
                    BatchId = arg.BatchIdNum,
                };
                // 件数（出力パラメータ）
                int allCnt = 0;
                // エラーメッセージ（出力パラメータ）
                string message = string.Empty;
                // バッチ予約状況取得登録（BatchUtil.GetBatchYoyakuList()）を呼び出し、バッチ予約状況を取得する。
                BatchYoyaku? batchYoyaku = BatchUtil.GetBatchYoyakuList(yoyakuParam, false, ref allCnt, ref message)?.FirstOrDefault();

                // バッチ予約が存在しない場合、
                if (batchYoyaku is null)
                {
                    // 以下のエラーメッセージを[変数：エラーメッセージ] に設定し、ERRORログに出力して「１０．」へ進む。
                    //（"ME01645" 引数{0} ：パラメータの取得)
                    errorMessage = MessageUtil.Get("ME01645", "パラメータの取得");
                    throw new AppException("ME01645", errorMessage);
                }
                else
                {
                    // バッチ予約が存在する場合
                    // 取得した「バッチ予約状況」から値を取得し変数に設定する。
                    // バッチ予約ユーザID = バッチ予約情報.予約ユーザID
                    batchYoyakuUid = batchYoyaku.BatchYoyakuId;
                }

                // ４．DB接続
                // ※「共通機能設計_070_DB切替」の「バッチのDB接続先取得処理」を参照。
                // システム共通スキーマからログインユーザの所属に応じた都道府県別事業スキーマのDB接続先を取得する
                // DB接続情報
                DbConnectionInfo? dbInfo = DBUtil.GetDbConnectionInfo(CoreConst.SystemKbn.Nsk, arg.TodofukenCd, arg.KumiaitoCd, arg.ShishoCd);
                if (dbInfo is null)
                {
                    errorMessage = MessageUtil.Get("ME90014");
                    throw new AppException("ME90014", errorMessage);
                }

                using (NskAppContext dbContext = new(dbInfo.ConnectionString, dbInfo.DefaultSchema, ConfigUtil.GetInt(CoreConst.COMMAND_TIMEOUT)))
                {
                    // ５．バッチ条件を取得
                    BatchJoken batchJoken = new();

                    // ５．１．バッチ条件情報の取得
                    batchJoken.GetBatchJokens(dbContext, arg.JokenId, ref errorMessage);

                    // ５．２．バッチ条件情報のチェック
                    // 必須入力チェック
                    batchJoken.IsRequired(ref errorMessage);

                    // ６．コードの整合性チェック
                    arg.IsConsistency(dbContext, ref errorMessage);
                    batchJoken.IsConsistency(dbContext, arg, ref errorMessage);

                    // ７．データ検索SQLを実行（ログ出力：あり）
                    NSK_FL105090 csvFile = new(arg, batchJoken);

                    // ７．１．対象データの取得
                    csvFile.Search(dbContext);

                    // ７．２．取得した件数が0件の場合
                    if (csvFile.IsEmpty())
                    {
                        //  [変数：エラーメッセージ] に以下のメッセージを設定し、ERRORログに出力して「１０．」へ進む。
                        // （"ME10076" 引数{0}：0)
                        errorMessage = MessageUtil.Get("ME10076", "0");
                        throw new AppException("ME10076", errorMessage);
                    }

                    // ７．３．取得した件数が0件以外の場合

                    // ７．３．１．ZIPファイル格納先パスを作成して変数に設定する
                    // [変数：ZIPファイル格納先パス]　←　[設定ファイル：CsvOutputFolder]/[変数：バッチID]_yyyyMMddHHmmss
                    string zipFolderPath = Path.Combine(
                        ConfigUtil.Get(CoreConst.CSV_OUTPUT_FOLDER),
                        $"{arg.BatchId}{CoreConst.SYMBOL_UNDERSCORE}{sysDateTime}");

                    // ７．３．２．作成したZIPファイル格納先パスにZIPファイル格納フォルダを作成する
                    Directory.CreateDirectory(zipFolderPath);


                    // ８．加入申込書チェックリストデータ出力処理
                    csvFile.OutputDataFile(systemDate, sysDateTime);


                    // ８．３．Zip暗号化を行う。
                    csvFile.EncryptFile(zipFolderPath, batchYoyakuUid);


                    // ８．３．２．「８．１．」のフォルダを削除する。
                    csvFile.DeleteTempFolder();

                    // ８．４．処理正常終了時
                    // [変数：処理ステータス] に"03"（成功）を設定
                    status = CoreConst.STATUS_SUCCESS;

                    // [変数：エラーメッセージ] に正常終了メッセージを設定
                    // （"MI10005"：処理が正常に終了しました。)
                    errorMessage = MessageUtil.Get("MI10005");
                    logger.Info(errorMessage);
                }
            }
            catch (Exception ex)
            {
                // １０．エラー処理
                // １０．１．例外（エクセプション）の場合

                //  [変数：処理ステータス] に"99"（エラー）を設定
                status = CoreConst.STATUS_ERROR;

                //   [変数：エラーメッセージ] にエラーメッセージを設定
                if (string.IsNullOrEmpty(errorMessage))
                {
                    //  [変数：エラーメッセージ] にエラーメッセージを設定
                    // （"MF00001")
                    errorMessage = MessageUtil.Get("MF00001");
                }

                // ログにエラーメッセージを出力する。
                logger.Error(ex, errorMessage);
            }

            // １０．２．共通機能の「バッチ実行状況更新」を呼び出し、バッチ予約テーブルを更新する。
            string refMessage = string.Empty;
            int ret = BatchUtil.UpdateBatchYoyakuSts(arg.BatchIdNum, status, errorMessage, batchYoyakuUid, ref refMessage);

            if (ret == BatchUtil.RET_FAIL)
            {
                // （１）失敗の場合
                logger.Error(string.Format(CoreConst.ERROR_LOG_UPDATE_BATCH_YOYAKU_STS, arg.BatchId, status, refMessage));
                result = CoreConst.BATCH_EXECUT_FAILED;
            }
            else
            {
                // （２）成功の場合
                logger.Info(string.Format(CoreConst.SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS, arg.BatchId, status, refMessage));
                result = CoreConst.BATCH_EXECUT_SUCCESS;
            }

            // 処理を終了する。
            Environment.ExitCode = result;
        }

    }
}
