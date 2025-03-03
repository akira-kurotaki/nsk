namespace NskWeb.Common.Consts
{
    /// <summary>
    /// 業務用定数定義
    /// </summary>
    public class AppConst
    {
        /// <summary>
        /// フラグ　ON
        /// </summary>
        public const string FLG_ON = "1";

        /// <summary>
        /// フラグ　OFF
        /// </summary>
        public const string FLG_OFF = "0";

        /// <summary>
        /// ページトークンチェック用
        /// </summary>
        public const string PAGE_TOKEN_REGEX_MATCH = @"[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}";

        /// <summary>
        /// 農業者IDのセッションキー
        /// </summary>
        public const string SESS_NOGYOSHA_ID = "D0101_NOGYOSHA_ID";

        /// <summary>
        /// 対象年度のセッションキー
        /// </summary>
        public const string SESS_NENDO = "D0101_NENDO";

        // $$$$$$$$$$$$$$$$$$$$$$$$$$$$
        /// <summary>
        /// ポータル画面でセットする情報のセッションキー
        /// </summary>
        public const string SESS_NSK_PORTAL = "_D000000_INFO";
        // $$$$$$$$$$$$$$$$$$$$$$$$$$$$

        /// <summary>
        /// HTTPレスポンスヘッダーフィールド：Content-Disposition
        /// </summary>
        public const string RESPONSE_HEADER = "Content-Disposition";

        /// <summary>
        /// HTTPレスポンスファイルの種類:pdf
        /// </summary>
        public const string RESPONSE_CONTENT_TYPE_APPLICATION_PDF = "application/pdf";

        /// <summary>
        /// HTTPレスポンスファイルの種類:csv
        /// </summary>
        public const string RESPONSE_CONTENT_TYPE_APPLICATION_CSV = "application/csv";

        /// <summary>
        /// 本所コード
        /// </summary>
        public const string HONSHO_CD = "00";

        /// <summary>
        /// バッチ分類
        /// </summary>
        public class BatchBunrui
        {
            /// <summary>
            /// 01：帳票出力
            /// </summary>
            public const string BATCH_BUNRUI_01_REPORT = "01";

            /// <summary>
            /// 02：CSV等出力
            /// </summary>
            public const string BATCH_BUNRUI_02_CSV = "02";

            /// <summary>
            /// 03：高負荷処理
            /// </summary>
            public const string BATCH_BUNRUI_03_KOHUKA = "03";

            /// <summary>
            /// 04：ファイル取込
            /// </summary>
            public const string BATCH_BUNRUI_04_FILE_IMPORT = "04";

            /// <summary>
            /// 90：その他
            /// </summary>
            public const string BATCH_BUNRUI_90_OTHER = "90";
        }

        /// <summary>
        /// バッチ種類
        /// </summary>
        public class BatchType
        {
            /// <summary>
            /// バッチ種類（巡回バッチ）
            /// </summary>
            public const string BATCH_TYPE_PATROL = "1";

            /// <summary>
            /// バッチ種類（定時バッチ）
            /// </summary>
            public const string BATCH_TYPE_SCHEDULED = "2";
        }

        /// <summary>
        /// 取込ステータス
        /// </summary>
        public class TorikomiSts
        {
            /// <summary>
            /// 01 ファイル取込待ち
            /// </summary>
            public const string WAITING_IMPORT = "01";

            /// <summary>
            /// 02 一時テーブルへの取込済み
            /// </summary>
            public const string TEMPORARY_TABLE_IMPORTED = "02";

            /// <summary>
            /// 03 データロック済み
            /// </summary>
            public const string DATA_LOCKED = "03";

            /// <summary>
            /// 04 バックアップデータ出力済み
            /// </summary>
            public const string BACKUP_DATA_OUTPUT = "04";

            /// <summary>
            /// 05 データ反映済み
            /// </summary>
            public const string DATA_REFLECTED = "05";

            /// <summary>
            /// 06 取込処理完了
            /// </summary>
            public const string IMPORT_COMPLETED = "06";

            /// <summary>
            /// 11 取込キャンセル（データ切り戻し済み）
            /// </summary>
            public const string IMPORT_CANCEL = "11";

            /// <summary>
            /// 99 取込エラー
            /// </summary>
            public const string IMPORT_ERROR = "99";
        }
    }
}
