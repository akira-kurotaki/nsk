using System.ComponentModel.DataAnnotations;
using CoreLibrary.Core.Validator;

namespace NskWeb.Areas.F111.Models.D111050
{

    public class D111050ChoshuzumiGakuRecord
    {

        /// <summary>共済目的</summary>
        [Display(Name = "共済目的コード")]
        public string KyosaiMokutekiCd { get; set; }

        /// <summary>共済目的</summary>
        [Display(Name = "共済目的")]
        public string KyosaiMokuteki { get; set; }

        /// <summary>徴収済み額</summary>
        [Display(Name = "徴収済み額")]
        [NumberSign(12)]
        [DisplayFormat(DataFormatString = "{0:#,##0}", ApplyFormatInEditMode = true)]
        public decimal? ChoshuzumiGaku { get; set; } = 0;

    }
}
