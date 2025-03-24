using System.ComponentModel;

namespace NskWeb.Areas.F102.Consts
{
    /// <summary>
    /// F102機能共通定数クラス
    /// </summary>
    public class F102Const
    {
        // D102010

        /// <summary>
        /// 画面ID(D102010)
        /// </summary>
        public const string SCREEN_ID_D102010 = "D102010";

        /// <summary>
        /// セッションキー(D102010)
        /// </summary>
        public const string SESS_D102010 = "D102010_SCREEN";

        /// <summary>
        /// 予約を実行した処理名(102010D)
        /// </summary>
        public const string SCREEN_ID_NSK_D102010 = "NSK_102010D";

        /// <summary>
        /// バッチID(102011B)
        /// </summary>
        public const string BATCH_ID_NSK_102011B = "NSK_102011B";

        /// <summary>
        /// 画面ID(D102010)
        /// </summary>
        public const string SCREEN_ID_D102050 = "D102050";

        /// <summary>
        /// セッションキー(D102010)
        /// </summary>
        public const string SESS_D102050 = "D102050_SCREEN";

        // D102050

        /// <summary>
        /// 予約を実行した処理名
        /// </summary>
        public const string SCREEN_ID_NSK_D102050 = "NSK_102050D";

        /// <summary>
        /// バッチID(102051B)
        /// </summary>
        public const string BATCH_ID_NSK_102051B = "NSK_102051B";

        // D102100

        /// <summary>
        /// 画面ID(D102100)
        /// </summary>
        public const string SCREEN_ID_D102100 = "D102100";

        /// <summary>
        /// セッションキー(D102100)
        /// </summary>
        public const string SESS_D102100 = "D102100_SCREEN";

        /// <summary>
        /// 予約を実行した処理名(102100D)
        /// </summary>
        public const string SCREEN_ID_NSK_D102100 = "NSK_102100D";

        /// <summary>
        /// バッチID(102101B)
        /// </summary>
        public const string BATCH_ID_NSK_102101B = "NSK_102101B";

        // D102120

        /// <summary>
        /// 画面ID(D102120)
        /// </summary>
        public const string SCREEN_ID_D102120 = "D102120";

        /// <summary>
        /// セッションキー(D102120)
        /// </summary>
        public const string SESS_D102120 = "D102120_SCREEN";

        /// <summary>
        /// 予約を実行した処理名(102120D)
        /// </summary>
        public const string SCREEN_ID_NSK_D102120 = "NSK_102120D";

        /// <summary>
        /// バッチID(102121B)
        /// </summary>
        public const string BATCH_ID_NSK_102121B = "NSK_102121B";

        // D103010

        /// <summary>
        /// 画面ID(D103010)
        /// </summary>
        public const string SCREEN_ID_D103010 = "D103010";

        /// <summary>
        /// セッションキー(D103010)
        /// </summary>
        public const string SESS_D103010 = "D103010_SCREEN";

        /// <summary>
        /// 予約を実行した処理名(103010D)
        /// </summary>
        public const string SCREEN_ID_NSK_D103010 = "NSK_103010D";

        /// <summary>
        /// バッチID(103011B)
        /// </summary>
        public const string BATCH_ID_NSK_103011B = "NSK_103011B";

        // 許容する拡張子
        public const string allowedExtension = "csv";

        // 許容するMIMEタイプ
        public const string allowedMimeType = "text/csv";

        // ファイル名の最大文字数
        public const int fileNameMaxLength = 100;

        // 許容ファイルサイズ
        public const int uploadFileMaxSize = 10485760; // TODO: aasettings.json にファイルサイズが追加されたら置き換える

        // 許容ファイルサイズ（表示用）
        public const string uploadFileMaxDispSize = "10MB"; // TODO: aasettings.json にファイルサイズ（表示用）が追加されたら置き換える
    }
}
