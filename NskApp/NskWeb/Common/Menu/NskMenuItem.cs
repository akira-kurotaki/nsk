namespace NskWeb.Common.Menu
{
    /// <summary>
    /// メニュー項目モデルクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/01/16
    /// 作成者：Rou I
    /// </remarks>
    [Serializable]
    public class NskMenuItem
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NskMenuItem()
        {
            // サブメニュー
            ChildItems = new List<NskMenuItem>();
        }

        /// <summary>
        /// メニューID
        /// </summary>
        public string MenuId { get; set; }

        /// <summary>
        /// 表示名
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 画面ID
        /// </summary>
        public string ScreenId { get; set; }

        /// <summary>
        /// 処理ID
        /// </summary>
        public string OpeId { get; set; }

        /// <summary>
        /// メニューレベル
        /// </summary>
        public int MenuLevel { get; set; }

        /// <summary>
        /// サブメニュー
        /// </summary>
        public IEnumerable<NskMenuItem> ChildItems { get; set; }

    }
}