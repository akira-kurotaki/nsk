using System.ComponentModel;

namespace NskWeb.Areas.F103.Consts
{
    /// <summary>
    /// F103機能共通定数クラス
    /// </summary>
    public class F103Const
    {
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
