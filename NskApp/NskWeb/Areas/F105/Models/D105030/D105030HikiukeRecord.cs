using CoreLibrary.Core.Validator;
using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F105.Models.D105030
{
    /// <summary>
    /// 引受情報入力明細
    /// </summary>
    public class D105030HikiukeRecord : D105030PagerRecord
    {
        /// <summary>耕地番号</summary>
        [Display(Name = "耕地番号")]
        [Numeric]
        [WithinDigitLength(5)]
        public string KouchiNo { get; set; } = string.Empty;
        /// <summary>地名地番</summary>
        [Display(Name = "地名地番")]
        [ExceptGaiji]
        [WithinDigitLength(20)]
        public string ChimeiChiban { get; set; } = string.Empty;
        /// <summary>本地面積</summary>
        [Display(Name = "本地面積")]
        [NumberDec(9, 1)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? HonchiMenseki { get; set; }
        /// <summary>引受面積</summary>
        [Display(Name = "引受面積")]
        [NumberDec(9,1)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? HikukeMenseki { get; set; }
        /// <summary>転作等の面積</summary>
        [Display(Name = "転作等の面積")]
        [NumberDec(9, 1)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? TensakutoMenseki { get; set; }
        /// <summary>種類</summary>
        public string Syurui { get; set; } = string.Empty;
        /// <summary>区分</summary>
        public string Kbn { get; set; } = string.Empty;
        /// <summary>市町村</summary>
        public string Shichoson { get; set; } = string.Empty;
        /// <summary>品種</summary>
        public string Hinsyu { get; set; } = string.Empty;
        /// <summary>産地銘柄</summary>
        public string SanchiMeigara { get; set; } = string.Empty;
        /// <summary>田畑</summary>
        public string Tahata { get; set; } = string.Empty;
        /// <summary>収量等級</summary>
        public string SyuryoTokyu { get; set; } = string.Empty;
        /// <summary>分筆番号</summary>
        [Display(Name = "分筆番号")]
        [Numeric]
        [WithinDigitLength(4)]
        public string BunpitsuNo { get; set; } = string.Empty;
        /// <summary>統計単収</summary>
        [Numeric]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? ToukeiTansyu { get; set; }
        /// <summary>実量基準単収</summary>
        [Display(Name = "実量基準単収")]
        [Numeric]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? JituryoKijunTansyu { get; set; }
        /// <summary>参酌</summary>
        [Display(Name = "参酌")]
        [Numeric]
        [WithinDigitLength(2)]
        public string Sanjaku { get; set; } = string.Empty;
        /// <summary>受委託者区分</summary>
        public string JuitakushaKbn { get; set; } = string.Empty;
        /// <summary>所有者</summary>
        [Display(Name = "所有者")]
        [Numeric]
        [WithinDigitLength(8)]
        public string Shoyusha { get; set; } = string.Empty;
        /// <summary>備考</summary>
        [Display(Name = "備考")]
        [ExceptGaiji]
        [WithinDigitLength(10)]
        public string Bikou { get; set; } = string.Empty;

        /// <summary>耕地xmin</summary>
        public uint? KouchiXmin { get; set; }

        /// <summary>局都道府県</summary>
        public string GisKyokuTodofuken { get; set; } = string.Empty;
        /// <summary>市区町村</summary>
        public string GisShichoson { get; set; } = string.Empty;
        /// <summary>大字</summary>
        public string GisOoaza { get; set; } = string.Empty;
        /// <summary>小字</summary>
        public string GisKoaza { get; set; } = string.Empty;
        /// <summary>地番</summary>
        public string GisChiban { get; set; } = string.Empty;
        /// <summary>枝番</summary>
        public string GisEdaban { get; set; } = string.Empty;
        /// <summary>子番</summary>
        public string GisKoban { get; set; } = string.Empty;
        /// <summary>孫番</summary>
        public string GisMagoban { get; set; } = string.Empty;
        /// <summary>RS区分</summary>
        public string GisRsKbn { get; set; } = string.Empty;

        /// <summary>GISxmin</summary>
        public uint? GisXmin { get; set; }

        /// <summary>
        /// srcオブジェクトとの比較
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public bool Compare(D105030HikiukeRecord src)
        {
            return (
                ($"{this.ChimeiChiban}" == $"{src.ChimeiChiban}") &&
                ($"{this.HonchiMenseki}" == $"{src.HonchiMenseki}") &&
                ($"{this.HikukeMenseki}" == $"{src.HikukeMenseki}") &&
                ($"{this.TensakutoMenseki}" == $"{src.TensakutoMenseki}") &&
                ($"{this.Syurui}" == $"{src.Syurui}") &&
                ($"{this.Kbn}" == $"{src.Kbn}") &&
                ($"{this.Shichoson}" == $"{src.Shichoson}") &&
                ($"{this.Hinsyu}" == $"{src.Hinsyu}") &&
                ($"{this.SanchiMeigara}" == $"{src.SanchiMeigara}") &&
                ($"{this.Tahata}" == $"{src.Tahata}") &&
                ($"{this.SyuryoTokyu}" == $"{src.SyuryoTokyu}") &&
                ($"{this.ToukeiTansyu}" == $"{src.ToukeiTansyu}") &&
                ($"{this.JituryoKijunTansyu}" == $"{src.JituryoKijunTansyu}") &&
                ($"{this.Sanjaku}" == $"{src.Sanjaku}") &&
                ($"{this.JuitakushaKbn}" == $"{src.JuitakushaKbn}") &&
                ($"{this.Shoyusha}" == $"{src.Shoyusha}") &&
                ($"{this.Bikou}" == $"{src.Bikou}") &&
                ($"{this.GisKyokuTodofuken}" == $"{src.GisKyokuTodofuken}") &&
                ($"{this.GisShichoson}" == $"{src.GisShichoson}") &&
                ($"{this.GisOoaza}" == $"{src.GisOoaza}") &&
                ($"{this.GisKoaza}" == $"{src.GisKoaza}") &&
                ($"{this.GisChiban}" == $"{src.GisChiban}") &&
                ($"{this.GisEdaban}" == $"{src.GisEdaban}") &&
                ($"{this.GisKoban}" == $"{src.GisKoban}") &&
                ($"{this.GisMagoban}" == $"{src.GisMagoban}") &&
                ($"{this.GisRsKbn}" == $"{src.GisRsKbn}")
            );
        }
    }
}