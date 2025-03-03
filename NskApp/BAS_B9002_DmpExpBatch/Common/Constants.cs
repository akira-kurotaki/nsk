namespace BAS_B9002_DmpExpBatch.Common
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
        /// システム区分99（共通）
        /// </summary>
        public const string SYSTEM_KBN_COMMON = "99";

        /// <summary>
        /// 設定ファイルにあるバックアップ用のDBユーザ
        /// </summary>
        public const string CONFIG_BACK_UP_USER = "BackUpUser";

        /// <summary>
        /// 設定ファイルにあるバックアップ用のDBパスワード
        /// </summary>
        public const string CONFIG_BACK_UP_PASS = "BackUpPass";

        /// <summary>
        /// 設定ファイルにあるダンプファイルを出力する一次フォルダ
        /// </summary>
        public const string CONFIG_TEMP_BACK_UP_FOLDER = "TempBackUpFolder";

        /// <summary>
        /// 設定ファイルにあるダンプファイルを分割する容量（MB)
        /// </summary>
        public const string CONFIG_DUMP_FILE_DIV_MB = "DumpFileDivMB";

        /// <summary>
        /// 設定ファイルにあるDBサーバとの通信時のタイムアウト時間(単位：秒)
        /// </summary>
        public const string CONFIG_COMMAND_TIMEOUT = "CommandTimeout";

        /// <summary>
        /// 設定ファイルにあるdb_dumpのパス
        /// </summary>
        public const string CONFIG_PG_DUMP_PATH = "PgDumpPath";

        /// <summary>
        /// ファイル拡張子:".dump"
        /// </summary>
        public const string FILE_EXTENSION_DUMP = ".dump";

        /// <summary>
        /// 半角スペース
        /// </summary>
        public const string HALF_WIDTH_SPACE = " ";

        /// <summary>
        /// 星：*
        /// </summary>
        public const string SYMBOL_STAR = "*";
    }
}
