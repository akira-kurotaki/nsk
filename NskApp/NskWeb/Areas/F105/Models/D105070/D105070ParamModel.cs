namespace NskWeb.Areas.F105.Models.D105070
{
    /// <summary>
    /// 画面間パラメタ（セッション情報）
    /// </summary>
    [Serializable]
    public class D105070ParamModel
    {
        /// <summary>組合等コード</summary>
        public string KumiaitoCd { get; set; } = string.Empty;
        /// <summary>都道府県コード</summary>
        public string TodofukenCd { get; set; } = string.Empty;
        /// <summary>年産</summary>
        public int Nensan { get; set; }
        /// <summary>共済目的</summary>
        public string KyosaiMokutekiCd { get; set; } = string.Empty;
        /// <summary>組合員等コード</summary>
        public string KumiaiintoCd { get; set; } = string.Empty;
        /// <summary>氏名</summary>
        public string FullNm { get; set; } = string.Empty;
        /// <summary>電話番号</summary>
        public string Tel { get; set; } = string.Empty;
        /// <summary>支所コード</summary>
        public string ShishoCd { get; set; } = string.Empty;
        /// <summary>支所名</summary>
        public string ShishoNm { get; set; } = string.Empty;
        /// <summary>市町村コード</summary>
        public string ShichosonCd { get; set; } = string.Empty;
        /// <summary>市町村名</summary>
        public string ShichosonNm { get; set; } = string.Empty;
        /// <summary>大地区コード</summary>
        public string DaichikuCd { get; set; } = string.Empty;
        /// <summary>大地区名</summary>
        public string DaichikuNm { get; set; } = string.Empty;
        /// <summary>小地区コード</summary>
        public string ShochikuCd { get; set; } = string.Empty;
        /// <summary>小地区名</summary>
        public string ShochikuNm { get; set; } = string.Empty;
        /// <summary>合併時識別コード</summary>
        public string GappeiShikibetsuCd { get; set; } = string.Empty;

        /// <summary>類区分</summary>
        public string RuiKbn { get; set; } = string.Empty;
        /// <summary>類区分名称</summary>
        public string RuiKbnNm { get; set; } = string.Empty;
        /// <summary>引受方式</summary>
        public string HikiukeHoushikiCd { get; set; } = string.Empty;
        /// <summary>引受方式名称</summary>
        public string HikiukeHoushikiNm { get; set; } = string.Empty;

        /// <summary>補償割合</summary>
        public string HoshoWariai { get; set; } = string.Empty;
        /// <summary>付保割合</summary>
        public decimal? FuhoWariai { get; set; }
        /// <summary>一筆半損特約</summary>
        public string IppitsuHansonTokuyaku { get; set; } = string.Empty;
        /// <summary>担い手</summary>
        public string NinaiteKbn { get; set; } = string.Empty;
        /// <summary>営農支払以外</summary>
        public bool EinoShiharaiIgai { get; set; } = false;
        /// <summary>収穫量確認方法</summary>
        public string SyukakuryoKakuninHouhou { get; set; } = string.Empty;
        /// <summary>全相殺基準単収</summary>
        public decimal? ZensousaiKijunTansyu { get; set; }
    }
}
