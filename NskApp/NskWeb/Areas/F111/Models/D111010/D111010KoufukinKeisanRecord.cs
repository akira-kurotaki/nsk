using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F111.Models.D111010
{

    public class D111010KoufukinKeisanRecord
    {

        /// <summary>計算対象交付回</summary>
        [Display(Name = "計算対象交付回")]
        public int Koufukai { get; set; }

        /// <summary>交付金計算実施日</summary>
        [Display(Name = "交付金計算実施日")]
        [DisplayFormat(DataFormatString = "{0:GEE/MM/DD}")]
        public DateTime? KoufukinKeisanJisshibi { get; set; }

        /// <summary>農家負担掛金</summary>
        [Display(Name = "農家負担掛金")]
        [DisplayFormat(DataFormatString = "{0:#,##0}")]
        public decimal? Futankin { get; set; }

        /// <summary>徴収済み額</summary>
        [Display(Name = "徴収済み額")]
        [DisplayFormat(DataFormatString = "{0:#,##0}")]
        public decimal? ChoshuzumiGaku { get; set; }

        /// <summary>徴収割合</summary>
        [Display(Name = "徴収割合")]
        [DisplayFormat(DataFormatString = "{0:0.#0}")]
        public decimal? ChoshuWariai { get; set; }

        /// <summary>掛金徴収額入力済み</summary>
        [Display(Name = "掛金徴収額入力済み")]
        public bool ChoshuGakuNyuryokuzumi { get; set; }

    }
}
