namespace BAS_B1001_TempFolderDelete.Common
{
    /// <summary>
    /// 定数定義
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// デリミタ「;」
        /// </summary>
        public const string DELIMITER_SEMICOLON = ";";

        /// <summary>
        /// バッチプログラムが処理成功：0
        /// </summary>
        public const short BATCH_EXECUT_SUCCESS = 0;

        /// <summary>
        /// バッチプログラムが処理失敗：1
        /// </summary>
        public const short BATCH_EXECUT_FAILED = 1;

        /// <summary>
        /// appsettings.jsonの「一時フォルダのパス」のキー名
        /// </summary>
        public const string TEMP_FOLDER_LIST = "temp_folder_list";

    }
}
