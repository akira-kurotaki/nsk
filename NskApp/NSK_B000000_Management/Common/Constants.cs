namespace NSK_B000000_Management.Common
{
    /// <summary>
    /// 定数定義
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// システム設定ファイルの「DB参照時間間隔」のキー名
        /// </summary>
        public const string DATABASE_REF_TIME_TAG_NAME = "databaseRefTime";

        /// <summary>
        /// システム設定ファイルの「システム時間フラグ」のキー名
        /// </summary>
        public const string SYS_DATE_TIME_FLAG = "SysDateTimeFlag";

        /// <summary>
        /// システム設定ファイルの「システム時間ファイルパス」のキー名
        /// </summary>
        public const string SYS_DATE_TIME_PATH = "SysDateTimePath";

        /// <summary>
        /// バッチとしてのユーザID
        /// </summary>
        public const string BATCH_USER_ID = "NSK_B0000";

        /// <summary>
        /// バッチ種類（巡回バッチ）
        /// </summary>
        public const string BATCH_TYPE_PATROL = "1";

        /// <summary>
        /// バッチ種類（定時バッチ）
        /// </summary>
        public const string BATCH_TYPE_SCHEDULED = "2";

        /// <summary>
        /// バッチプログラムが処理成功：0
        /// </summary>
        public const short BATCH_EXECUT_SUCCESS = 0;

        /// <summary>
        /// バッチプログラムが処理失敗：1
        /// </summary>
        public const short BATCH_EXECUT_FAILED = 1;

        /// <summary>
        /// 半角スペース
        /// </summary>
        public const string HALF_WIDTH_SPACE = " ";

        /// <summary>
        /// 改行文字列
        /// </summary>
        public const string NEW_LINE_SEPARATOR = "\r\n";

        /// <summary>
        /// パラメータのセパレータ
        /// </summary>
        public const string PARAM_SEPARATOR = "; ";

        /// <summary>
        /// ログ文字列
        /// {0}: バッチID
        /// {1}: バッチ条件
        /// </summary>
        public const string LOG_MSG = "バッチID：{0} 予約ID：{1}";

        /// <summary>
        /// エラーログ文字列
        /// {0}: 予約ID
        /// {1}: エラーメッセージ
        /// </summary>
        public const string ERROR_LOG = "  バッチID:{0}。{1}";

        /// <summary>
        /// エラーログ文字列
        /// </summary>
        public const string ERROR_LOG_UPDATE_BATCH_YOYAKU_STS = "バッチ実行状況更新失敗 バッチID：{0}、ステータス：{1}、エラー情報：{2}";

        /// <summary>
        /// ログ文字列
        /// </summary>
        public const string SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS = "バッチ実行状況更新成功 バッチID：{0}、ステータス：{1}、エラー情報：{2}";

        /// <summary>
        /// ロガー：処理時間 開始文字列
        /// </summary>
        public const string LOG_TIMER_START_MESSAGE = "処理時間:";

        /// <summary>
        /// ロガー：処理時間 終了文字列
        /// </summary>
        public const string LOG_TIMER_END_MESSAGE = "(ミリ秒)";

        /// <summary>
        /// ステータス：エラー
        /// </summary>
        public const string STATUS_ERROR = "99";

        // $$$$$$$$$$$$$$$$$$$$$$$
        /// <summary>
        /// タイマー実行回数設定
        /// </summary>
        public const string BATCH_TIMER_COUNT_MAX = "BatchTimerCountMax";
        // $$$$$$$$$$$$$$$$$$$$$$$
    }
}
