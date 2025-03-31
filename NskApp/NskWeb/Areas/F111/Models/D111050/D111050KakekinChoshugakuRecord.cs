using System.ComponentModel.DataAnnotations;
using CoreLibrary.Core.Validator;

namespace NskWeb.Areas.F111.Models.D111050
{

    public class D111050KakekinChoshugakuRecord
    {

        /// <summary>共済目的コード</summary>
        [Display(Name = "共済目的コード")]
        public string KyosaiMokutekiCd { get; set; }

        /// <summary>共済目的</summary>
        [Display(Name = "共済目的")]
        public string KyosaiMokuteki { get; set; }

        /// <summary>計算対象交付回</summary>
        [Display(Name = "引受報告回")]
        public int HikiukeHoukokukai { get; set; }

        /// <summary>農家負担掛金</summary>
        [Display(Name = "農家負担掛金")]
        [DisplayFormat(DataFormatString = "{0:#,##0}")]
        public decimal? Futankin { get; set; }

        /// <summary>徴収済み額</summary>
        [Display(Name = "徴収済み額")]
        [NumberSign(12)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal? ChoshuzumiGaku { get; set; } = 0;

        /// <summary>徴収割合</summary>
        [Display(Name = "徴収割合")]
        [DisplayFormat(DataFormatString = "{0:0.#0}")]
        public decimal? ChoshuWariai { get; set; } = 0;

        /// <summary>KofuChoshuXmin</summary>
        public uint? KofuChoshuXmin { get; set; }

        /// <summary>Xmin</summary>
        public uint? KofukaiXmin { get; set; }


    }
}
