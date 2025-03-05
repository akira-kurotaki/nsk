using CoreLibrary.Core.Validator;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F501.Models.D501010
{
    /// <summary>
    /// 農作物共済加入状況データ取込画面項目モデル
    /// </summary>
    [Serializable]
    public class D501010TableRecord
    {
        /// <summary>
        /// 年産
        /// </summary>
        [Display(Name = "年産")]
        public int 年産 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string 共済目的コード { get; set; } = string.Empty;

        /// <summary>
        /// 共済目的
        /// </summary>
        [Display(Name = "共済目的")]
        public string 共済目的 { get; set; }
        
        /// <summary>
        /// 引受回
        /// </summary>
        [Display(Name = "引受回")]
        public int 引受回 { get; set; }

        /// <summary>
        /// 引受計算実施日
        /// </summary>
        public DateTime? 引受計算実施日 { get; set; }

        /// <summary>
        /// 引受面積
        /// </summary>
        [Display(Name = "引受面積")]
        [Numeric]
        public Decimal? 引受面積 { get; set; }

        /// <summary>
        /// 農家負担掛金
        /// </summary>
        [Display(Name = "農家負担掛金")]
        [Numeric]
        public Decimal? 農家負担掛金 { get; set; }

        /// <summary>
        /// 基準収穫量
        /// </summary>
        [Display(Name = "基準収穫量")]
        [Numeric]
        public Decimal? 基準収穫量 { get; set; }

        /// <summary>
        /// 賦課金
        /// </summary>
        [Display(Name = "賦課金")]
        [Numeric]
        public Decimal? 賦課金 { get; set; }

        /// <summary>
        /// 共済金額
        /// </summary>
        [Display(Name = "共済金額")]
        [Numeric]
        public Decimal? 共済金額 { get; set; }

        /// <summary>
        /// 掛金徴収日
        /// </summary>
        public DateTime? 掛金徴収日 { get; set; }

        /// <summary>
        /// 掛金等徴収額
        /// </summary>
        [Display(Name = "掛金等徴収額")]
        [Numeric]
        public Decimal? 掛金等徴収額 { get; set; }
    }
}
