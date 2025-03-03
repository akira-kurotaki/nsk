namespace CoreLibrary.Core.DropDown
{
    /// <summary>
    /// 品目ドロップダウンDB取得結果モデルクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/02/23
    /// 作成者：Rou I
    /// </remarks>
    [Serializable]
    public class HimmokuSearchResult
    {
        /// <summary>
        /// 農産物等品目コード
        /// </summary>
        public string HimmokuCd { get; set; }

        /// <summary>
        /// 品目名
        /// </summary>
        public string HimmokuNm { get; set; }

        /// <summary>
        /// 農畜産区分
        /// </summary>
        public string NochikusanKbn { get; set; }

        /// <summary>
        /// 農産物等種類コード
        /// </summary>
        public string ShuruiCd { get; set; }

        /// <summary>
        /// 種類名
        /// </summary>
        public string ShuruiNm { get; set; }

        /// <summary>
        /// 農産物等用途コード
        /// </summary>
        public string YotoCd { get; set; }

        /// <summary>
        /// 用途名
        /// </summary>
        public string YotoNm { get; set; }

        /// <summary>
        /// 支所別品目コード
        /// </summary>
        public string ShishoHimmokuCd { get; set; }

        /// <summary>
        /// 支所別種類コード
        /// </summary>
        public string ShishoShuruiCd { get; set; }

        /// <summary>
        /// 支所別用途コード
        /// </summary>
        public string ShishoYotoCd { get; set; }

    }
}