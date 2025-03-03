using CoreLibrary.Core.Validator;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F105.Models.D105030
{
    public class D105030RuibetsuSetteiRecord : D105030PagerRecord
    {
        /// <summary>引受区分</summary>
        [Display(Name = "引受区分")]
        [Required]
        public string HikiukeKbn { get; set; } = string.Empty;
        /// <summary>引受方式</summary>
        [Display(Name = "引受方式")]
        [Required]
        public string HikiukeHoushiki { get; set; } = string.Empty;
        /// <summary>補償割合</summary>
        [Display(Name = "補償割合")]
        [Required]
        public string HoshoWariai { get; set; } = string.Empty;
        /// <summary>付保割合</summary>
        [Display(Name = "付保割合")]
        [Numeric]
        [WithinDigitLength(2)]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? FuhoWariai { get; set; }
        /// <summary>一筆半損特約</summary>
        public string IppitsuHansonTokuyaku { get; set; } = string.Empty;
        /// <summary>選択共済金額</summary>
        public decimal? SelectKyosaiKingaku { get; set; }
        /// <summary>危険段階区分（料率）</summary>
        public string KikenDankaiKbn { get; set; } = string.Empty;
        /// <summary>収穫量確認方法</summary>
        public string SyukakuryoKakuninHouhou { get; set; } = string.Empty;
        /// <summary>全相殺基準単収</summary>
        [Display(Name = "全相殺基準単収")]
        [Numeric]
        [WithinDigitLength(3)]
        public string ZensousaiKijunTansyu { get; set; } = string.Empty;

        /// <summary>個人設定類xmin</summary>
        public uint? Xmin { get; set; }

        /// <summary>
        /// srcオブジェクトとの比較
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public bool Compare(D105030RuibetsuSetteiRecord src)
        {
            return (
                ($"{this.HikiukeKbn}" == $"{src.HikiukeKbn}") &&
                ($"{this.HikiukeHoushiki}" == $"{src.HikiukeHoushiki}") &&
                ($"{this.HoshoWariai}" == $"{src.HoshoWariai}") &&
                ($"{this.FuhoWariai}" == $"{src.FuhoWariai}") &&
                ($"{this.IppitsuHansonTokuyaku}" == $"{src.IppitsuHansonTokuyaku}") &&
                ($"{this.SelectKyosaiKingaku}" == $"{src.SelectKyosaiKingaku}") &&
                ($"{this.KikenDankaiKbn}" == $"{src.KikenDankaiKbn}") &&
                ($"{this.SyukakuryoKakuninHouhou}" == $"{src.SyukakuryoKakuninHouhou}") &&
                ($"{this.ZensousaiKijunTansyu}" == $"{src.ZensousaiKijunTansyu}")
            );
        }
    }
}