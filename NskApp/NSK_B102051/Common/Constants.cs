namespace NSK_B102051.Common
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
        /// エラーログ文字列
        /// </summary>
        public const string ERROR_LOG_UPDATE_BATCH_YOYAKU_STS = "バッチ実行状況更新失敗 バッチID：{0}、ステータス：{1}、エラー情報：{2}";

        /// <summary>
        /// ログ文字列
        /// </summary>
        public const string SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS = "バッチ実行状況更新成功 バッチID：{0}、ステータス：{1}";

        /// <summary>
        /// システム区分99（共通）
        /// </summary>
        public const string SYSTEM_KBN_COMMON = "99";


        /// <summary>
        /// 設定ファイルにあるダンプファイルを出力する一次フォルダ
        /// </summary>
        public const string CONFIG_TEMP_BACK_UP_FOLDER = "TempBackUpFolder";

        /// <summary>
        /// 設定ファイルにあるDBサーバとの通信時のタイムアウト時間(単位：秒)
        /// </summary>
        public const string CONFIG_COMMAND_TIMEOUT = "CommandTimeout";

        /// <summary>
        /// 半角スペース
        /// </summary>
        public const string HALF_WIDTH_SPACE = " ";

        /// <summary>
        /// 星：*
        /// </summary>
        public const string SYMBOL_STAR = "*";

        public const string DEFAULT_SCHEMA_SYSTEM_COMMON = "DefaultSchema_SystemCommon";

        public const string DEFAULT_SCHEMA_JIGYO_COMMON = "DefaultSchema_JigyoCommon";

        public const string SYS_DATE_TIME_FLAG = "SysDateTimeFlag";

        public const string SYS_DATE_TIME_PATH = "SysDateTimePath";

        public const string SYSTEM_KBN = "SystemKbn";

        public const string JOUKEN_UKEIRERIREKI_ID = "取込履歴ID";

        public const string JOUKEN_NENSAN = "年産";

        public const string JOUKEN_FILE_PATH = "ファイルパス";

        public const string JOUKEN_FILE_HASH = "ファイルハッシュ";

        public const string CSV_TEMP_FOLDER = "CsvTempFolder";

        public const string TASK_SUCCESS = "03";

        public const string TASK_FAILED = "99";
    }
}
