namespace NskWeb.Areas.F000.Consts
{
    /// <summary>
    /// D000機能共通定数クラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/03/01
    /// 作成者：Tin Kungen
    /// </remarks>
    public class F000Const
    {
        /// <summary>
        /// 最終ログイン日時
        /// （セッション）
        /// </summary>
        public const string SESS_LAST_LOGIN_DATETIME = "D000000_LAST_LOGIN_DATETIME";

        /// <summary>
        /// 日付フォーマット（yyyy/MM/dd HH:mm ）
        /// </summary>
        public const string YYYYMMDD_HHmm_WITH_SLASH = "yyyy/MM/dd HH:mm";

        /// <summary>
        /// 別ウィンドウ開けため、D000020画面サイズ(Pc)
        /// <summary>
        public enum PcOpenSizeD000020
        {
            /// <summary>
            /// 高さ
            /// </summary>
            PcHeight = 535,
            /// <summary>
            /// 広さ
            /// </summary>
            PcWidth = 990
        }

        /// <summary>
        /// 別ウィンドウ開けため、D000020画面サイズ(Tablet)
        /// <summary>
        public enum TabletOpenSizeD000020
        {
            /// <summary>
            /// 高さ
            /// </summary>
            TabletHeight = 803,
            /// <summary>
            /// 広さ
            /// </summary>
            TabletWidth = 1485
        }
    }

}
