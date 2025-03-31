using NskWeb.Common.Models;

namespace NskWeb.Areas.F105.Models.D105036
{
    public class D105036ResultRecord : BasePagerRecord
    {
        /// <summary>組合員等コード</summary>
        public string KumiaiintoCd { get; set; } = string.Empty;
        /// <summary>氏名</summary>
        public string FullNm { get; set; } = string.Empty;

        /// <summary>支所</summary>
        public string ShishoCd { get; set; } = string.Empty;
        public string ShishoNm { get; set; } = string.Empty;
        /// <summary>大地区</summary>
        public string DaichikuCd {  get; set; } = string.Empty;
        public string DaichikuNm { get; set; } = string.Empty;
        /// <summary>小地区</summary>
        public string ShochikuCd { get; set; } = string.Empty;
        public string ShochikuNm { get; set; } = string.Empty;
        /// <summary>市町村</summary>
        public string ShichosonCd { get; set; } = string.Empty;
        public string ShichosonNm { get; set; } = string.Empty;
        /// <summary>未加入</summary>
        public string HikiukeTeishi { get; set; } = string.Empty;
        /// <summary>解除</summary>
        public string Kaijo { get; set; } = string.Empty;
        /// <summary>耕地情報有無</summary>
        public string KouchiUmu { get; set; } = string.Empty;

        /// <summary>合併時識別</summary>
        public string GappeijiShikibetsuCd { get; set; } = string.Empty;
    }
}