namespace BAS_B0001_BatchReport.Common
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
        /// appsettings.jsonの「一括帳票出力本体プログラムの格納先」のキー名
        /// </summary>
        public const string BATCH_PATH = "B0001BatchReportPath";

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
        public const string SUCCESS_LOG_UPDATE_BATCH_YOYAKU_STS = "バッチ実行状況更新成功 バッチID：{0}、ステータス：{1}、エラー情報：{2}";

        /// <summary>
        /// リアルタイム帳票、又はバッチ帳票プロジェクトのクラスの和名
        /// </summary>
        public const string CLASS_NM_REPORT_OUTPUT = "帳票出力";

        /// <summary>
        /// 設定不正時のエラーメッセージ
        /// </summary>
        public const string CONFIG_ERR_MSG = "{0}の設定が不正です";

        /// <summary>
        /// 共済事業コード（ベースアプリ）
        /// </summary>
        public const string KYOSAI_JIGYO_CD = "88";

        /// <summary>
        /// バッチとしてのユーザID
        /// </summary>
        public const string BATCH_USER_ID = "Batch";

        /// <summary>
        /// バッチ種類（巡回バッチ）
        /// </summary>
        public const string BATCH_TYPE_PATROL = "1";

        /// <summary>
        /// バッチ種類（定時バッチ）
        /// </summary>
        public const string BATCH_TYPE_SCHEDULED = "2";

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
        /// {0}: 帳票出力 OR 帳票制御 OR 帳票作成
        /// {1}: 条件ID
        /// {2}: 機能名
        /// {3}: パラメータ
        /// </summary>
        public const string METHOD_BEGIN_LOG = "BEGIN {0} {1}。{2}{3}";

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
