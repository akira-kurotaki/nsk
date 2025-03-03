using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using CoreLibrary.Core.Validator;
using ModelLibrary.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static NskWeb.Areas.F02.Models.D0201.D0201SearchCondition;

namespace NskWeb.Areas.F02.Models.D0201
{
    /// <summary>
    /// 一括帳票出力画面項目モデル（検索条件部分）
    /// </summary>
    [Serializable]
    public class D0201SearchCondition
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D0201SearchCondition()
        {
            // 対象年度
            this.NendoFrom = NendoUtil.GetStandardNendo();
            this.NendoTo = NendoUtil.GetStandardNendo();
            this.TodofukenDropDownList = new TodofukenDropDownList("SearchCondition");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin">ユーザー情報（セッション）</param>
        /// <param name="shishoList">利用可能な支所一覧（セッション）</param>
        public D0201SearchCondition(Syokuin syokuin, List<Shisho> shishoList)
        {
            // 対象年度
            this.NendoFrom = NendoUtil.GetStandardNendo();
            this.NendoTo = NendoUtil.GetStandardNendo();
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
        /// 対象書類
        /// </summary>
        [Display(Name = "対象書類")]
        [Required]
        public TargetReports? TargetReport { get; set; }

        /// <summary>
        /// 対象年度（開始）
        /// </summary>
        [Display(Name = "対象年度（開始）")]
        [Required]
        public string NendoFrom { get; set; }

        /// <summary>
        /// 対象年度（終了）
        /// </summary>
        [Display(Name = "対象年度（終了）")]
        [Required]
        public string NendoTo { get; set; }

        /// <summary>
        /// 都道府県マルチドロップダウンリスト
        /// </summary>
        [Display(Name = "都道府県マルチドロップダウンリスト")]
        public TodofukenDropDownList TodofukenDropDownList { get; set; }

        /// <summary>
        /// 加入者管理コード（開始）
        /// </summary>
        [Display(Name = "加入者管理コード（開始）")]
        [Numeric]
        [FullStringLength(13)]
        public string KanyushaCdFrom { get; set; }

        /// <summary>
        /// 加入者管理コード（終了）
        /// </summary>
        [Display(Name = "加入者管理コード（終了）")]
        [Numeric]
        [FullStringLength(13)]
        public string KanyushaCdTo { get; set; }

        /// <summary>
        /// 氏名又は法人名
        /// </summary>
        [Display(Name = "氏名又は法人名")]
        [WithinStringLength(30)]
        public string HojinFullNm { get; set; }

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
        /// 対象書類ドロップダウンリスト要素
        /// </summary>
        [Flags]
        public enum TargetReports
        {
            [Description("加入者情報（一覧）")]
            KanyushaInfoList,
            [Description("加入者情報（単票）")]
            KanyushaInfoSingle,
        }

        /// <summary>
        /// 表示順ドロップダウンリスト要素
        /// </summary>
        [Flags]
        public enum DisplaySortType
        {
            [Description("対象年度")]
            Nendo,
            [Description("都道府県")]
            TodofukenCd,
            [Description("組合等")]
            KumiaitoCd,
            [Description("支所")]
            ShishoCd,
            [Description("加入者管理コード")]
            KanyushaCd,
            [Description("市町村")]
            ShichosonCd,
            [Description("大地区")]
            DaichikuCd,
            [Description("小地区")]
            ShochikuCd,
            [Description("氏名又は法人名")]
            HojinFullNm,
        }
    }
}
