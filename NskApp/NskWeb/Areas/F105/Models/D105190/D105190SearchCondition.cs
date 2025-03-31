using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Validator;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F105.Models.D105190
{
    /// <summary>
    /// 検索条件
    /// </summary>
    public class D105190SearchCondition
    {

        /// <summary>加入形態</summary>
        [Display(Name = "類区分")]
        [Required]
        public string RuiKbn { get; set; } = string.Empty;

        /// <summary>類区分ドロップダウンリスト</summary>
        [Display(Name = "類区分ドロップダウンリスト")]
        public List<SelectListItem> RuiKbnList { get; set; } = new();

        /// <summary>都道府県マルチドロップダウンリスト</summary>
        [Display(Name = "都道府県マルチドロップダウンリスト")]
        public TodofukenDropDownList TodofukenDropDownList { get; set; }

        /// <summary>組合員等コード(From)</summary>
        [Display(Name = "組合員等コード（開始）")]
        [Numeric]
        [WithinDigitLength(13)]
        public string KumiaiinToCdFrom { get; set; } = string.Empty;
        [Display(Name = "組合員等コード（終了）")]
        [Numeric]
        [WithinDigitLength(13)]
        /// <summary>組合員等コード(To)</summary>
        public string KumiaiinToCdTo { get; set; } = string.Empty;
        
        #region "表示数・表示順"
        /// <summary>表示数</summary>
        [Display(Name = "表示数")]
        public int? DisplayCount { get; set; } = CoreConst.PAGE_SIZE;

        /// <summary>表示順キー１</summary>
        [Display(Name = "表示順")]
        public DisplaySortType? DisplaySort1 { get; set; }
        /// <summary>表示順１</summary>
        public CoreConst.SortOrder DisplaySortOrder1 { get; set; }
        /// <summary>表示順キー２</summary>
        [Display(Name = "表示順")]
        public DisplaySortType? DisplaySort2 { get; set; }
        /// <summary>表示順２</summary>
        public CoreConst.SortOrder DisplaySortOrder2 { get; set; }
        /// <summary>表示順キー３</summary>
        [Display(Name = "表示順")]
        public DisplaySortType? DisplaySort3 { get; set; }
        /// <summary>表示順３</summary>
        public CoreConst.SortOrder DisplaySortOrder3 { get; set; }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D105190SearchCondition()
        {
            // 都道府県マルチドロップダウンリスト
            TodofukenDropDownList = new TodofukenDropDownList("SearchCondition");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin"></param>
        /// <param name="shishoList"></param>
        public D105190SearchCondition(Syokuin syokuin, List<Shisho> shishoList)
        {
            // 都道府県マルチドロップダウンリスト
            TodofukenDropDownList = new TodofukenDropDownList("SearchCondition", syokuin, shishoList);
        }

        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src"></param>
        public void ApplyInput(D105190SearchCondition src)
        {
            this.DisplayCount = src.DisplayCount;
            this.DisplaySort1 = src.DisplaySort1;
            this.DisplaySortOrder1 = src.DisplaySortOrder1;
            this.DisplaySort2 = src.DisplaySort2;
            this.DisplaySortOrder2 = src.DisplaySortOrder2;
            this.DisplaySort3 = src.DisplaySort3;
            this.DisplaySortOrder3 = src.DisplaySortOrder3;

            this.RuiKbn = src.RuiKbn;
            this.TodofukenDropDownList = src.TodofukenDropDownList;
            this.KumiaiinToCdFrom = src.KumiaiinToCdFrom;
            this.KumiaiinToCdTo = src.KumiaiinToCdTo;
        }

        /// <summary>
        /// 表示順ドロップダウンリスト要素
        /// </summary>
        [Flags]
        public enum DisplaySortType
        {
            [Description("類区分")]
            Ruikbn,
            [Description("支所")]
            Shisyo,
            [Description("大地区")]
            Daichiku,
            [Description("小地区")]
            Shochiku,
            [Description("組合員等コード")]
            KumiaiintoCd
        }

    }
}
