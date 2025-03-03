namespace BAS_B0002_CsvExp.Common
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
        /// ステータス：成功
        /// </summary>
        public const string STATUS_SUCCESS = "03";

        /// <summary>
        /// ステータス：エラー
        /// </summary>
        public const string STATUS_ERROR = "99";

        /// <summary>
        /// appsettings.jsonの「システム時間フラグ」のキー名
        /// </summary>
        public const string SYS_DATE_TIME_FLAG = "SysDateTimeFlag";

        /// <summary>
        /// appsettings.jsonの「システム時間ファイルパス」のキー名
        /// </summary>
        public const string SYS_DATE_TIME_PATH = "SysDateTimePath";

        /// <summary>
        /// appsettings.jsonの「CSV一時出力フォルダ」のキー名
        /// </summary>
        public const string FILE_TEMP_FOLDER_PATH = "FileTempFolder";

        /// <summary>
        /// appsettings.jsonの「CSV出力先（ZIP保管フォルダ）」のキー名
        /// </summary>
        public const string CSV_OUTPUT_FOLDER_PATH = "CSVOutputFolder";

        /// <summary>
        /// エラーログ文字列
        /// {0}: 条件ID
        /// {1}: エラーメッセージ
        /// </summary>
        public const string ERROR_LOG = "{0}。{1}";

        /// <summary>
        /// エラーログ文字列
        /// </summary>
        public const string ERROR_LOG_UPDATE_BATCH_YOYAKU_STS = "バッチ実行状況更新失敗 バッチID：{0}、ステータス：{1}、エラー情報：{2}";

        /// <summary>
        /// ログ文字列
        /// </summary>
        public const string SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS = "バッチ実行状況更新成功 バッチID：{0}、ステータス：{1}";

        /// <summary>
        /// CSV出力
        /// </summary>
        public const string CLASS_NM_CSV_OUTPUT = "CSV出力";

        /// <summary>
        /// システム区分（ベースアプリ）
        /// </summary>
        public const string SYSTEM_KBN_CD = "88";

        /// <summary>
        /// バッチとしてのユーザID
        /// </summary>
        public const string BATCH_USER_ID = "BAS_B0002";

        /// <summary>
        /// パラメータの名前と値のセパレータ
        /// </summary>
        public const string PARAM_NAME_VALUE_SEPARATOR = ":";

        /// <summary>
        /// パラメータのセパレータ
        /// </summary>
        public const string PARAM_SEPARATOR = "; ";

        /// <summary>
        /// メソッドの開始ログ文字列
        /// {0}: 帳票出力 OR 帳票制御 OR 帳票作成 OR CSV出力
        /// {1}: 条件ID OR バッチID
        /// {2}: 機能名
        /// {3}: パラメータ
        /// </summary>
        public const string METHOD_BEGIN_LOG = "BEGIN {0} {1}。{2}{3}";

        /// <summary>
        /// フラグ（0）
        /// </summary>
        public const string FLG_0 = "0";

        /// <summary>
        /// フラグ（1）
        /// </summary>
        public const string FLG_1 = "1";

        /// <summary>
        /// パラメータ和名：バッチID
        /// </summary>
        public const string PARAM_NAME_BATCH_ID = "バッチID";

        /// <summary>
        /// パラメータ和名：都道府県コード
        /// </summary>
        public const string PARAM_NAME_TODOFUKEN_CD = "都道府県コード";

        /// <summary>
        /// パラメータ和名：組合等コード
        /// </summary>
        public const string PARAM_NAME_KUMIAITO_CD = "組合等コード";

        /// <summary>
        /// パラメータ和名：支所コード
        /// </summary>
        public const string PARAM_NAME_SHISHO_CD = "支所コード";

        /// <summary>
        /// パラメータ和名：バッチ条件
        /// </summary>
        public const string PARAM_NAME_BATCH_JOKEN = "バッチ条件";
    }
}
