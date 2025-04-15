using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using CoreLibrary.Core.Validator;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Models;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskWeb.Areas.F105.Models.D105030;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static NskWeb.Areas.F104.Models.D104010.D104010SearchCondition;


namespace NskWeb.Areas.F104.Models.D104010
{
    /// <summary>
    /// 一括帳票出力画面項目モデル（検索条件部分）
    /// </summary>
    [Serializable]
    public class D104010SearchCondition
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D104010SearchCondition()
        {
            this.TodofukenDropDownList = new TodofukenDropDownList("SearchCondition");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin">ユーザー情報（セッション）</param>
        /// <param name="shishoList">利用可能な支所一覧（セッション）</param>
        public D104010SearchCondition(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.TodofukenDropDownList = new TodofukenDropDownList("SearchCondition", syokuin, shishoList);
        }

        /// <summary>
        /// 検索結果表示フラグ
        /// </summary>
        [Display(Name = "検索結果表示フラグ")]
        public bool IsResultDisplay { get; set; }

        /// <summary>
        /// メッセージエリア1
        /// </summary>
        [Display(Name = "メッセージエリア1")]
        public string MessageArea1 { get; set; }

        /// <summary>
        /// 組合番号コードFrom
        /// </summary>
        [Display(Name = "組合員等コードFrom")]
        public string kumiaiintoCdFrom { get; set; }

        /// <summary>
        /// 組合番号コードTo
        /// </summary>
        [Display(Name = "組合員等コードTo")]
        public string kumiaiintoCdTo { get; set; }

        /// <summary>
        /// 解除理由
        /// </summary>
        [Display(Name = "解除理由")]
        public string kaijyoRiyu { get; set; }

        /// <summary>
        /// 解除理由の選択肢リスト
        /// </summary>
        public List<SelectListItem> KaijyoRiyuItems { get; set; }

        /// <summary>
        /// 都道府県マルチドロップダウンリスト
        /// </summary>
        [Display(Name = "都道府県マルチドロップダウンリスト")]
        public TodofukenDropDownList TodofukenDropDownList { get; set; }

        /// <summary>
        /// 表示数
        /// </summary>
        [Display(Name = "表示数")]
        public int? DisplayCount { get; set; }

        /// <summary>
        /// 表示順1
        /// </summary>
        public DisplaySortType? DisplaySort1 { get; set; }

        /// <summary>
        /// ソート順1
        /// </summary>
        public CoreConst.SortOrder DisplaySortOrder1 { get; set; }

        /// <summary>
        /// 表示順2
        /// </summary>
        public DisplaySortType? DisplaySort2 { get; set; }

        /// <summary>
        /// ソート順2
        /// </summary>
        public CoreConst.SortOrder DisplaySortOrder2 { get; set; }

        /// <summary>
        /// 表示順3
        /// </summary>
        public DisplaySortType? DisplaySort3 { get; set; }

        /// <summary>
        /// ソート順3
        /// </summary>
        public CoreConst.SortOrder DisplaySortOrder3 { get; set; }

        /// <summary>
        /// 表示順ドロップダウンリスト要素
        /// </summary>
        [Flags]
        public enum DisplaySortType
        {
            [Description("大地区")]
            DaichikuCd,
            [Description("小地区")]
            ShochikuCd,
            [Description("組合員等コード")]
            KumiaiintoCd,
        }
    }
}
