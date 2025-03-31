using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Validator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F105.Models.D105036
{
    /// <summary>
    /// 検索条件
    /// </summary>
    public class D105036SearchCondition
    {
        /// <summary>
        /// 都道府県マルチドロップダウンリスト
        /// </summary>
        [Display(Name = "都道府県マルチドロップダウンリスト")]
        public TodofukenDropDownList TodofukenDropDownList { get; set; }

        /// <summary>組合員等コード(From)</summary>
        [Display(Name = "組合員等コード(開始)")]
        [Numeric]
        [WithinDigitLength(8)]
        public string KumiaiinToCdFrom { get; set; } = string.Empty;
        /// <summary>組合員等コード(To)</summary>
        [Display(Name = "組合員等コード(終了)")]
        [Numeric]
        [WithinDigitLength(8)]
        public string KumiaiinToCdTo { get; set; } = string.Empty;

        /// <summary>加入状況</summary>
        [Display(Name = "未加入")]
        public KanyuJokyo KanyuState { get; set; } = KanyuJokyo.ALL;

        /// <summary>解除状況</summary>
        [Display(Name = "解除")]
        public KaijoJokyo KaijoState { get; set; } = KaijoJokyo.ALL;

        /// <summary>新規（耕地情報有無）</summary>
        [Display(Name = "新規（耕地情報有無）")]
        public KouchiJokyo KouchiUmu { get; set; } = KouchiJokyo.ALL;

        
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
        public D105036SearchCondition()
        {
            // 都道府県マルチドロップダウンリスト
            TodofukenDropDownList = new TodofukenDropDownList("SearchCondition");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin"></param>
        /// <param name="shishoList"></param>
        public D105036SearchCondition(Syokuin syokuin, List<Shisho> shishoList)
        {
            // 都道府県マルチドロップダウンリスト
            TodofukenDropDownList = new TodofukenDropDownList("SearchCondition", syokuin, shishoList);
        }

        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src"></param>
        public void ApplyInput(D105036SearchCondition src)
        {
            this.DisplayCount = src.DisplayCount;
            this.DisplaySort1 = src.DisplaySort1;
            this.DisplaySortOrder1 = src.DisplaySortOrder1;
            this.DisplaySort2 = src.DisplaySort2;
            this.DisplaySortOrder2 = src.DisplaySortOrder2;
            this.DisplaySort3 = src.DisplaySort3;
            this.DisplaySortOrder3 = src.DisplaySortOrder3;

            this.TodofukenDropDownList = src.TodofukenDropDownList;
            this.KumiaiinToCdFrom = src.KumiaiinToCdFrom;
            this.KumiaiinToCdTo = src.KumiaiinToCdTo;

            this.KanyuState = src.KanyuState;
            this.KaijoState = src.KaijoState;
            this.KouchiUmu = src.KouchiUmu;
        }

        /// <summary>
        /// 表示順ドロップダウンリスト要素
        /// </summary>
        [Flags]
        public enum DisplaySortType
        {
            [Description("支所")]
            Shisyo,
            [Description("市町村")]
            Shichoson,
            [Description("大地区")]
            Daichiku,
            [Description("小地区")]
            Shochiku,
            [Description("組合員等コード")]
            KumiaiintoCd
        }

        /// <summary>
        /// 加入状況
        /// </summary>
        [Flags]
        public enum KanyuJokyo
        {
            [Description("加入")]
            KANYU,
            [Description("未加入")]
            MIKANYU,
            [Description("全て")]
            ALL,
        }

        /// <summary>
        /// 解除状況
        /// </summary>
        [Flags]
        public enum KaijoJokyo
        {
            [Description("解除")]
            KAIJO,
            [Description("解除以外")]
            OTHER,
            [Description("全て")]
            ALL,
        }
        /// <summary>
        /// 解除状況
        /// </summary>
        [Flags]
        public enum KouchiJokyo
        {
            [Description("新規")]
            NEW,
            [Description("新規以外")]
            OTHER,
            [Description("全て")]
            ALL,
        }
        
    }
}
