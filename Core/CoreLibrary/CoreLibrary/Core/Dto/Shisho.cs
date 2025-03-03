namespace CoreLibrary.Core.Dto
{
    /// <summary>
    /// 支所グループ詳細
    /// </summary>
    [Serializable]
    public class Shisho
    {
        /// <summary>
        /// 都道府県コード
        /// </summary>
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        public string ShishoCd { get; set; }

        /// <summary>
        /// 支所名
        /// </summary>
        public string ShishoNm { get; set; }
    }
}
