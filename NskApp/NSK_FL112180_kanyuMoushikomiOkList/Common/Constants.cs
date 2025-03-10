namespace NSK_FL112180_KikiDankaiDataToriKokuji.Common
{
    /// <summary>
    /// 定数定義
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// 処理結果：成功
        /// </summary>
        public const int RESULT_SUCCESS = 0;

        /// <summary>
        /// 処理結果：失敗
        /// </summary>
        public const int RESULT_FAILED = 1;

        /// <summary>
        /// システム区分99（共通）
        /// </summary>
        public const string SYSTEM_KBN_COMMON = "99";

        /// <summary>
        /// バッチ条件（JOUKEN_UKEIRE_RIREKI_ID）
        /// </summary>
        public const string JOUKEN_UKEIRE_RIREKI_ID = "受入履歴ID";

        /// <summary>
        /// バッチ条件（JOUKEN_UKEIRE_RIREKI_ID）
        /// </summary>
        public const string JOUKEN_FILE_NAME = "ファイル名";

        /// <summary>
        /// バッチ条件（JOUKEN_UKEIRE_RIREKI_ID）
        /// </summary>
        public const string JOUKEN_MOJI_CD = "文字コード";

        /// <summary>
        /// パラメータのセパレータ
        /// </summary>
        public const string PARAM_SEPARATOR = ", ";

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

        public const string FILE_TEMP_FOLDER_PATH = "FileTempFolderPath";

        /// <summary>
        /// appsettings.jsonの「ZIPファイル格納先パス」のキー名
        /// </summary>
        public const string CSV_OUTPUT_FOLDER = "CsvOutputFolder";

        /// <summary>
        /// ファイルクラスの和名
        /// </summary>
        public const string CLASS_NM_CONTROLLER = "ファイル制御";

        /// <summary>
        /// メソッドの開始ログ文字列
        /// {0}: ファイル出力 OR ファイル制御 OR ファイル作成
        /// {1}: 条件ID
        /// {2}: 機能名
        /// {3}: パラメータ
        /// </summary>
        public const string METHOD_BEGIN_LOG = "BEGIN {0} {1}。{2}{3}";

        /// <summary>
        /// メソッドの終了ログ文字列
        /// {0}: ファイル出力 OR ファイル制御 OR ファイル作成
        /// {1}: 条件ID
        /// {2}: 機能名 OR 処理時間
        /// </summary>
        public const string METHOD_END_LOG = "END   {0} {1}。{2}";
    }
}
