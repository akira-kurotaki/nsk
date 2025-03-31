namespace NskWeb.Areas.F105.Models.D105036
{
    /// <summary>
    /// 画面間パラメタ（セッション情報）
    /// </summary>
    [Serializable]
    public class D105190ParamModel
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        public string KumiaitoCd { get; set; } = string.Empty;
        /// <summary>
        /// 都道府県コード
        /// </summary>
        public string TodofukenCd { get; set; } = string.Empty;
        /// <summary>
        /// 年産
        /// </summary>
        public int Nensan { get; set; }
        /// <summary>
        /// 共済目的
        /// </summary>
        public string KyosaiMokutekiCd { get; set; } = string.Empty;
        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string KumiaiintoCd { get; set; } = string.Empty;
        /// <summary>
        /// 氏名
        /// </summary>
        public string FullNm { get; set; } = string.Empty;
        /// <summary>
        /// 電話番号
        /// </summary>
        public string Tel { get; set; } = string.Empty;
        /// <summary>
        /// 支所コード
        /// </summary>
        public string ShishoCd { get; set; } = string.Empty;
        /// <summary>
        /// 支所名
        /// </summary>
        public string ShishoNm { get; set; } = string.Empty;
        /// <summary>
        /// 市町村コード
        /// </summary>
        public string ShichosonCd { get; set; } = string.Empty;
        /// <summary>
        /// 市町村名
        /// </summary>
        public string ShichosonNm { get; set; } = string.Empty;
        /// <summary>
        /// 大地区コード
        /// </summary>
        public string DaichikuCd { get; set; } = string.Empty;
        /// <summary>
        /// 大地区名
        /// </summary>
        public string DaichikuNm { get; set; } = string.Empty;
        /// <summary>
        /// 小地区コード
        /// </summary>
        public string ShochikuCd { get; set; } = string.Empty;
        /// <summary>
        /// 小地区名
        /// </summary>
        public string ShochikuNm { get; set; } = string.Empty;
        /// <summary>
        /// 合併時識別コード
        /// </summary>
        public string GappeiShikibetsuCd { get; set; } = string.Empty;
    }
}
