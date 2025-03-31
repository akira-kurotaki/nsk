using CoreLibrary.Core.Validator;
using NskWeb.Common.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F105.Models.D105070
{
    /// <summary>
    /// 引受情報入力明細
    /// </summary>
    public class D105070HikiukeRecord : BasePagerRecord
    {
        /// <summary>耕地番号</summary>
        [Display(Name = "耕地番号")]
        [Numeric]
        [Required]
        [WithinDigitLength(5)]
        public string KouchiNo { get; set; } = string.Empty;
        /// <summary>地名地番</summary>
        [Display(Name = "地名地番")]
        [ExceptGaiji]
        [WithinDigitLength(20)]
        public string ChimeiChiban { get; set; } = string.Empty;
        /// <summary>耕地面積</summary>
        [Display(Name = "耕地面積")]
        [NumberDec(9, 1)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KouchiMenseki { get; set; }
        /// <summary>引受面積</summary>
        [Display(Name = "引受面積")]
        [NumberDec(9,1)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? HikukeMenseki { get; set; }
        /// <summary>除外等の面積</summary>
        [Display(Name = "除外等の面積")]
        [NumberDec(9, 1)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? JogaitoMenseki { get; set; }
        /// <summary>種類</summary>
        [Display(Name = "種類")]
        [Required]
        public string Syurui { get; set; } = string.Empty;
        /// <summary>区分</summary>
        public string Kbn { get; set; } = string.Empty;
        /// <summary>市町村</summary>
        [Display(Name = "市町村")]
        [Numeric]
        [WithinDigitLength(5)]
        public string Shichoson { get; set; } = string.Empty;
        /// <summary>市町村名</summary>
        public string ShichosonNm { get; set; } = string.Empty;
        /// <summary>品種</summary>
        [Display(Name = "品種")]
        [Numeric]
        [WithinDigitLength(3)]
        public string Hinsyu { get; set; } = string.Empty;
        /// <summary>品種名</summary>
        public string HinsyuNm { get; set; } = string.Empty;
        /// <summary>産地銘柄</summary>
        [Display(Name = "産地銘柄")]
        [Numeric]
        [WithinDigitLength(5)]
        public string SanchiMeigara { get; set; } = string.Empty;
        /// <summary>産地銘柄名</summary>
        public string SanchiMeigaraNm { get; set; } = string.Empty;
        /// <summary>田畑</summary>
        public string Tahata { get; set; } = string.Empty;
        /// <summary>収量等級</summary>
        public string SyuryoTokyu { get; set; } = string.Empty;
        /// <summary>分筆番号</summary>
        [Display(Name = "分筆番号")]
        [Numeric]
        [Required]
        [WithinDigitLength(4)]
        public string BunpitsuNo { get; set; } = string.Empty;
        /// <summary>統計単収</summary>
        [Numeric]
        [ReadOnly(true)]
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
        public bool Compare(D105070HikiukeRecord src)
        {
            return (
                ($"{this.ChimeiChiban}" == $"{src.ChimeiChiban}") &&
                ($"{this.KouchiMenseki}" == $"{src.KouchiMenseki}") &&
                ($"{this.HikukeMenseki}" == $"{src.HikukeMenseki}") &&
                ($"{this.JogaitoMenseki}" == $"{src.JogaitoMenseki}") &&
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