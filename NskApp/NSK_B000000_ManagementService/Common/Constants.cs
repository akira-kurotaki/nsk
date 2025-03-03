namespace NSK_B000000_ManagementService.Common
{
    /// <summary>
    /// 定数定義
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// 設定ファイルの「一括帳票出力プログラムの格納先」のキー名
        /// </summary>
        public const string BATCH_MANAGEMENT_PRO_PATH_TAG_NAME = "B0000ManagementProPath";

        /// <summary>
        /// サービスの開始メッセージ
        /// </summary>
        public const string SERVICE_START_MESSAGE = "巡回プログラムのサービスを正常に開始しました。";

        /// <summary>
        /// サービスの終了メッセージ
        /// </summary>
        public const string SERVICE_STOP_MESSAGE = "巡回プログラムのサービスを正常に終了しました。";

        /// <summary>
        /// バッチ管理プログラムの正常実行メッセージ
        /// </summary>
        public const string BATCH_MANAGEMENT_PRO_START_MESSAGE = "巡回プログラムの実行が正常に開始しました。";

        /// <summary>
        /// バッチ管理プログラムの格納先メッセージ
        /// </summary>
        public const string BATCH_MANAGEMENT_PRO_LOCATION_MSG = "巡回プログラムの格納先：";
    }
}