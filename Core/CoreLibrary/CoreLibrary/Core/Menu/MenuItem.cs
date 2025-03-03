namespace CoreLibrary.Core.Menu
{
    /// <summary>
    /// メニュー項目モデルクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/01/16
    /// 作成者：Rou I
    /// </remarks>
    [Serializable]
    public class MenuItem
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MenuItem()
        {
            // サブメニュー
            ChildItems = new List<MenuItem>();
        }

        /// <summary>
        /// 表示名
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 画面ID
        /// </summary>
        public string ScreenId { get; set; }

        /// <summary>
        /// サブメニュー
        /// </summary>
        public IEnumerable<MenuItem> ChildItems { get; set; }

    }
}