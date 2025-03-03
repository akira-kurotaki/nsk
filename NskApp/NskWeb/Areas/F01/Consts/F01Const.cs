namespace NskWeb.Areas.F01.Consts
{
    /// <summary>
    /// D01機能共通定数クラス
    /// </summary>
    public class F01Const
    {
        /// <summary>
        /// 加入者情報の遷移元画面のセッションキー
        /// </summary>
        public const string D0102_SCREEN_FROM = "D0102_SCREEN_FROM";

        /// <summary>
        /// 耕地形態(0:未選択, 1:畑, 2:田, 3:その他)
        /// </summary>
        public enum KochiKeitai
        {
            /// <summary>
            /// 未選択
            /// </summary>
            NotSelected = 0,
            /// <summary>
            /// 畑
            /// </summary>
            Hata = 1,
            /// <summary>
            /// 田
            /// </summary>
            Ta = 2,
            /// <summary>
            /// その他
            /// </summary>
            Sonohoka = 3
        }
    }

}
