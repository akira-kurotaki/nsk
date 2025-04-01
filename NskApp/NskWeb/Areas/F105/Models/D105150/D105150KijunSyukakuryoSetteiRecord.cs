using CoreLibrary.Core.Validator;
using NskWeb.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F105.Models.D105150
{
    public class D105150KijunSyukakuryoSetteiRecord : BasePagerRecord
    {
        /// <summary>組合員等コード</summary>
        [Display(Name = "組合員等コード")]
        [Required]
        [Numeric]
        [WithinDigitLength(13)]
        public string KumiaiintoCd { get; set; } = string.Empty;
        /// <summary>氏名</summary>
        public string Shimei { get; set; } = string.Empty;
        /// <summary>類区分</summary>
        [Display(Name = "類区分")]
        [Required]
        public string RuiKbn { get; set; } = string.Empty;
        /// <summary>営農対象外フラグ</summary>
        [Display(Name = "営農対象外フラグ")]
        public bool EinoTaisyogaiFlg { get; set; } = false;
        /// <summary>産地別銘柄等コード</summary>
        [Display(Name = "産地別銘柄等コード")]
        [Required]
        [Numeric]
        [WithinDigitLength(5)]
        public string SanchibetsuMeigaratoCd { get; set; } = string.Empty;
        /// <summary>産地別銘柄等名称</summary> Label
        public string SanchibetsuMeigaratoNm { get; set; } = string.Empty;
        /// <summary>平均単収</summary> TextBox
        [Display(Name = "平均単収")]
        [Required]
        [NumberDec(6, 2)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? HeikinTanshu { get; set; }
        /// <summary>規格等別割合（規格１）</summary>
        [Display(Name = "規格等別割合（規格１）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai1 { get; set; }
        /// <summary>規格等別割合（規格２）</summary>
        [Display(Name = "規格等別割合（規格２）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai2 { get; set; }
        /// <summary>規格等別割合（規格３）</summary>
        [Display(Name = "規格等別割合（規格３）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai3 { get; set; }
        /// <summary>規格等別割合（規格４）</summary>
        [Display(Name = "規格等別割合（規格４）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai4 { get; set; }
        /// <summary>規格等別割合（規格５）</summary>
        [Display(Name = "規格等別割合（規格５）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai5 { get; set; }
        /// <summary>規格等別割合（規格６）</summary>
        [Display(Name = "規格等別割合（規格６）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai6 { get; set; }
        /// <summary>規格等別割合（規格７）</summary>
        [Display(Name = "規格等別割合（規格７）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai7 { get; set; }
        /// <summary>規格等別割合（規格８）</summary>
        [Display(Name = "規格等別割合（規格８）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai8 { get; set; }
        /// <summary>規格等別割合（規格９）</summary>
        [Display(Name = "規格等別割合（規格９）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai9 { get; set; }
        /// <summary>規格等別割合（規格１０）</summary>
        [Display(Name = "規格等別割合（規格１０）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai10 { get; set; }
        /// <summary>規格等別割合（規格１１）</summary>
        [Display(Name = "規格等別割合（規格１１）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai11 { get; set; }
        /// <summary>規格等別割合（規格１２）</summary>
        [Display(Name = "規格等別割合（規格１２）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai12 { get; set; }
        /// <summary>規格等別割合（規格１３）</summary>
        [Display(Name = "規格等別割合（規格１３）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai13 { get; set; }
        /// <summary>規格等別割合（規格１４）</summary>
        [Display(Name = "規格等別割合（規格１４）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai14 { get; set; }
        /// <summary>規格等別割合（規格１５）</summary>
        [Display(Name = "規格等別割合（規格１５）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai15 { get; set; }
        /// <summary>規格等別割合（規格１６）</summary>
        [Display(Name = "規格等別割合（規格１６）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai16 { get; set; }
        /// <summary>規格等別割合（規格１７）</summary>
        [Display(Name = "規格等別割合（規格１７）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai17 { get; set; }
        /// <summary>規格等別割合（規格１８）</summary>
        [Display(Name = "規格等別割合（規格１８）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai18 { get; set; }
        /// <summary>規格等別割合（規格１９）</summary>
        [Display(Name = "規格等別割合（規格１９）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai19 { get; set; }
        /// <summary>規格等別割合（規格２０）</summary>
        [Display(Name = "規格等別割合（規格２０）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai20 { get; set; }
        /// <summary>規格等別割合（規格２１）</summary>
        [Display(Name = "規格等別割合（規格２１）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai21 { get; set; }
        /// <summary>規格等別割合（規格２２）</summary>
        [Display(Name = "規格等別割合（規格２２）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai22 { get; set; }
        /// <summary>規格等別割合（規格２３）</summary>
        [Display(Name = "規格等別割合（規格２３）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai23 { get; set; }
        /// <summary>規格等別割合（規格２４）</summary>
        [Display(Name = "規格等別割合（規格２４）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai24 { get; set; }
        /// <summary>規格等別割合（規格２５）</summary>
        [Display(Name = "規格等別割合（規格２５）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai25 { get; set; }
        /// <summary>規格等別割合（規格２６）</summary>
        [Display(Name = "規格等別割合（規格２６）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai26 { get; set; }
        /// <summary>規格等別割合（規格２７）</summary>
        [Display(Name = "規格等別割合（規格２７）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai27 { get; set; }
        /// <summary>規格等別割合（規格２８）</summary>
        [Display(Name = "規格等別割合（規格２８）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai28 { get; set; }
        /// <summary>規格等別割合（規格２９）</summary>
        [Display(Name = "規格等別割合（規格２９）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai29 { get; set; }
        /// <summary>規格等別割合（規格３０）</summary>
        [Display(Name = "規格等別割合（規格３０）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai30 { get; set; }
        /// <summary>規格等別割合（規格３１）</summary>
        [Display(Name = "規格等別割合（規格３１）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai31 { get; set; }
        /// <summary>規格等別割合（規格３２）</summary>
        [Display(Name = "規格等別割合（規格３２）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai32 { get; set; }
        /// <summary>規格等別割合（規格３３）</summary>
        [Display(Name = "規格等別割合（規格３３）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai33 { get; set; }
        /// <summary>規格等別割合（規格３４）</summary>
        [Display(Name = "規格等別割合（規格３４）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai34 { get; set; }
        /// <summary>規格等別割合（規格３５）</summary>
        [Display(Name = "規格等別割合（規格３５）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai35 { get; set; }
        /// <summary>規格等別割合（規格３６）</summary>
        [Display(Name = "規格等別割合（規格３６）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai36 { get; set; }
        /// <summary>規格等別割合（規格３７）</summary>
        [Display(Name = "規格等別割合（規格３７）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai37 { get; set; }
        /// <summary>規格等別割合（規格３８）</summary>
        [Display(Name = "規格等別割合（規格３８）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai38 { get; set; }
        /// <summary>規格等別割合（規格３９）</summary>
        [Display(Name = "規格等別割合（規格３９）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai39 { get; set; }
        /// <summary>規格等別割合（規格４０）</summary>
        [Display(Name = "規格等別割合（規格４０）")]
        [NumberDec(4, 3)]
        [DisplayFormat(DataFormatString = "{0:#,##0.0}", ApplyFormatInEditMode = true)]
        public decimal? KikakubetsuWariai40 { get; set; }

        /// <summary>xmin</summary>
        public uint? Xmin { get; set; }


        /// <summary>
        /// srcオブジェクトとの比較
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public bool Compare(D105150KijunSyukakuryoSetteiRecord src)
        {
            return
            (
                (this.HeikinTanshu.GetValueOrDefault(0) == src.HeikinTanshu.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai1.GetValueOrDefault(0) == src.KikakubetsuWariai1.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai2.GetValueOrDefault(0) == src.KikakubetsuWariai2.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai3.GetValueOrDefault(0) == src.KikakubetsuWariai3.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai4.GetValueOrDefault(0) == src.KikakubetsuWariai4.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai5.GetValueOrDefault(0) == src.KikakubetsuWariai5.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai6.GetValueOrDefault(0) == src.KikakubetsuWariai6.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai7.GetValueOrDefault(0) == src.KikakubetsuWariai7.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai8.GetValueOrDefault(0) == src.KikakubetsuWariai8.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai9.GetValueOrDefault(0) == src.KikakubetsuWariai9.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai10.GetValueOrDefault(0) == src.KikakubetsuWariai10.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai11.GetValueOrDefault(0) == src.KikakubetsuWariai11.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai12.GetValueOrDefault(0) == src.KikakubetsuWariai12.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai13.GetValueOrDefault(0) == src.KikakubetsuWariai13.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai14.GetValueOrDefault(0) == src.KikakubetsuWariai14.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai15.GetValueOrDefault(0) == src.KikakubetsuWariai15.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai16.GetValueOrDefault(0) == src.KikakubetsuWariai16.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai17.GetValueOrDefault(0) == src.KikakubetsuWariai17.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai18.GetValueOrDefault(0) == src.KikakubetsuWariai18.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai19.GetValueOrDefault(0) == src.KikakubetsuWariai19.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai20.GetValueOrDefault(0) == src.KikakubetsuWariai20.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai21.GetValueOrDefault(0) == src.KikakubetsuWariai21.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai22.GetValueOrDefault(0) == src.KikakubetsuWariai22.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai23.GetValueOrDefault(0) == src.KikakubetsuWariai23.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai24.GetValueOrDefault(0) == src.KikakubetsuWariai24.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai25.GetValueOrDefault(0) == src.KikakubetsuWariai25.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai26.GetValueOrDefault(0) == src.KikakubetsuWariai26.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai27.GetValueOrDefault(0) == src.KikakubetsuWariai27.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai28.GetValueOrDefault(0) == src.KikakubetsuWariai28.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai29.GetValueOrDefault(0) == src.KikakubetsuWariai29.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai30.GetValueOrDefault(0) == src.KikakubetsuWariai30.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai31.GetValueOrDefault(0) == src.KikakubetsuWariai31.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai32.GetValueOrDefault(0) == src.KikakubetsuWariai32.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai33.GetValueOrDefault(0) == src.KikakubetsuWariai33.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai34.GetValueOrDefault(0) == src.KikakubetsuWariai34.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai35.GetValueOrDefault(0) == src.KikakubetsuWariai35.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai36.GetValueOrDefault(0) == src.KikakubetsuWariai36.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai37.GetValueOrDefault(0) == src.KikakubetsuWariai37.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai38.GetValueOrDefault(0) == src.KikakubetsuWariai38.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai39.GetValueOrDefault(0) == src.KikakubetsuWariai39.GetValueOrDefault(0)) &&
                (this.KikakubetsuWariai40.GetValueOrDefault(0) == src.KikakubetsuWariai40.GetValueOrDefault(0))
            );
        }
    }
}