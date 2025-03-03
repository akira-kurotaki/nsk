namespace CoreLibrary.Core.HelpMenu
{
    /// <summary>
    /// ヘルプメニュー項目モデルクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/04/13
    /// 作成者：MATSUBAYASHI Atsushi
    /// </remarks>
    [Serializable]
    public class HelpMenuItem
    {
        /// <summary>
        /// ハッシュ値
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// 表示名
        /// </summary>
        public string Text { get; set; }
    }
}