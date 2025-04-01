using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Validator;
using Microsoft.AspNetCore.Mvc.Rendering;
using NskAppModelLibrary.Context;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F105.Models.D105150
{
    /// <summary>
    /// 検索条件
    /// </summary>
    public class D105150SearchCondition
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


        /// <summary>類区分</summary>
        [Display(Name = "類区分")]
        public string RuiKbn { get; set; } = string.Empty;
        /// <summary>営農対象外フラグ</summary>
        [Display(Name = "営農対象外フラグ")]
        public string EinoTaishogaiFlg { get; set; } = string.Empty;
        /// <summary>産地別銘柄等コード(From)</summary>
        [Display(Name = "産地別銘柄等コード(開始)")]
        public string SanchibetsuMeigaratoCdFrom { get; set; } = string.Empty;
        /// <summary>産地別銘柄等コード(To)</summary>
        [Display(Name = "産地別銘柄等コード(終了)")]
        public string SanchibetsuMeigaratoCdTo { get; set; } = string.Empty;


        /// <summary>類区分リスト</summary>
        public List<SelectListItem> RuiKbnList { get; set; } = new();
        /// <summary>営農対象外フラグリスト</summary>
        public List<SelectListItem> EinoTaishogaiFlgList { get; set; } = new();
        /// <summary>産地別銘柄等コードリスト</summary>
        public List<SelectListItem> SanchibetsuMeigaratoCdList { get; set; } = new();
        
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
        public D105150SearchCondition()
        {
            // 都道府県マルチドロップダウンリスト
            TodofukenDropDownList = new TodofukenDropDownList("SearchCondition");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin"></param>
        /// <param name="shishoList"></param>
        public D105150SearchCondition(Syokuin syokuin, List<Shisho> shishoList)
        {
            // 都道府県マルチドロップダウンリスト
            TodofukenDropDownList = new TodofukenDropDownList("SearchCondition", syokuin, shishoList);
        }

        /// <summary>
        /// ドロップダウンリスト初期化
        /// </summary>
        /// <param name="dbContext"></param>
        public void InitializeDropdonwList(NskAppContext dbContext, D105150SessionInfo sessionInfo)
        {
            // ２．３．「類区分情報リスト」を取得する。
            RuiKbnList = new();
            RuiKbnList.AddRange(dbContext.M00020類名称s.Where(x =>
                (x.共済目的コード == sessionInfo.KyosaiMokutekiCd))?.
                OrderBy(x => x.類区分).
                Select(x => new SelectListItem($"{x.類区分} {x.類短縮名称}", $"{x.類区分}")));

            // ２．４．「営農対象外フラグ情報リスト」を取得する。
            EinoTaishogaiFlgList = new();
            EinoTaishogaiFlgList.AddRange(dbContext.M00230営農対象名称s.
                OrderBy(x => x.営農対象外フラグ).
                Select(x => new SelectListItem($"{x.営農対象外フラグ} {x.営農対象フラグ名称}", $"{x.営農対象外フラグ}")));

            // ２．５．「産地別銘柄等コード情報リスト」を取得する。
            SanchibetsuMeigaratoCdList = new();
            SanchibetsuMeigaratoCdList.AddRange(dbContext.M00130産地別銘柄名称設定s.Where(x =>
                (x.組合等コード == sessionInfo.KumiaitoCd) &&
                (x.年産 == sessionInfo.Nensan) &&
                (x.共済目的コード == sessionInfo.KyosaiMokutekiCd))?.
                OrderBy(x => x.産地別銘柄コード).
                Select(x => new SelectListItem($"{x.産地別銘柄コード} {x.産地別銘柄名称}", $"{x.産地別銘柄コード}")));
        }

        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src"></param>
        public void ApplyInput(D105150SearchCondition src)
        {
            this.TodofukenDropDownList = src.TodofukenDropDownList;
            this.KumiaiinToCdFrom = src.KumiaiinToCdFrom;
            this.KumiaiinToCdTo = src.KumiaiinToCdTo;
            this.RuiKbn = src.RuiKbn;
            this.EinoTaishogaiFlg = src.EinoTaishogaiFlg;
            this.SanchibetsuMeigaratoCdFrom = src.SanchibetsuMeigaratoCdFrom;
            this.SanchibetsuMeigaratoCdTo = src.SanchibetsuMeigaratoCdTo;

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
            this.RuiKbn = src.RuiKbn;
            this.EinoTaishogaiFlg = src.EinoTaishogaiFlg;
            this.SanchibetsuMeigaratoCdFrom = src.SanchibetsuMeigaratoCdFrom;
            this.SanchibetsuMeigaratoCdTo = src.SanchibetsuMeigaratoCdTo;
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
            KumiaiintoCd,
            [Description("類区分")]
            RuiKbn,
            [Description("産地別銘柄等コード")]
            SanchibetsuMeigaratoCd,
        }
    }
}
