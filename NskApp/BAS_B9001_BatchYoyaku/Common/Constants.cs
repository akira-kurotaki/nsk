namespace BAS_B9001_BatchYoyaku.Common
{
    /// <summary>
    /// 定数定義
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// バッチプログラムが処理成功：0
        /// </summary>
        public const short BATCH_EXECUT_SUCCESS = 0;

        /// <summary>
        /// バッチプログラムが処理失敗：1
        /// </summary>
        public const short BATCH_EXECUT_FAILED = 1;

        /// <summary>
        /// システム設定ファイルにある「システム区分：共通」のキー名
        /// </summary>
        public const string SYSTEM_KBN_COMMON = "SystemKbnCommon";

        /// <summary>
        /// システム設定ファイルにある「定時実行予約設定ファイル」のキー名
        /// </summary>
        public const string SCHE_BATCH_RUN_SETTING = "sche_batch_run_setting";

        /// <summary>
        /// 定時実行予約設定ファイルの列数
        /// </summary>
        public const int SETTING_FILE_ROW_NUMS = 15;
        
        /// <summary>
        /// 星：*
        /// </summary>
        public const string SYMBOL_STAR = "*";
    }
}
