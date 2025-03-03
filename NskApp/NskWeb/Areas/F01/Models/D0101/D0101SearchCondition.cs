using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using CoreLibrary.Core.Validator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F01.Models.D0101
{
    /// <summary>
    /// 加入者一覧(検索条件)
    /// </summary>
    [Serializable]
    public class D0101SearchCondition
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D0101SearchCondition()
        {
            // 対象年度
            Nendo = NendoUtil.GetStandardNendo();
            // 都道府県マルチドロップダウンリスト
            TodofukenDropDownList = new TodofukenDropDownList("SearchCondition");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin">職員マスタ</param>
        public D0101SearchCondition(Syokuin syokuin, List<Shisho> shishoList)
        {
            // 対象年度
            Nendo = NendoUtil.GetStandardNendo();
            // 都道府県マルチドロップダウンリスト
            TodofukenDropDownList = new TodofukenDropDownList("SearchCondition", syokuin, shishoList);
            // 検索結果の表示
            IsDisplayFlag = false;
            // 表示数
            DisplayCount = CoreConst.PAGE_SIZE;
            // 表示順
            DisplaySortOrder1 = CoreConst.SortOrder.DESC;
            DisplaySortOrder2 = CoreConst.SortOrder.DESC;
            DisplaySortOrder3 = CoreConst.SortOrder.DESC;
        }

        /// <summary>
        /// 検索結果表示フラグ
        /// </summary>
        [Display(Name = "検索結果表示フラグ")]
        public bool IsDisplayFlag { get; set; }

        /// <summary>
        /// 対象年度
        /// </summary>
        [Display(Name = "対象年度")]
        public string Nendo { get; set; }

        /// <summary>
        /// 都道府県マルチドロップダウンリスト
        /// </summary>
        [Display(Name = "都道府県マルチドロップダウンリスト")]
        public TodofukenDropDownList TodofukenDropDownList { get; set; }

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
        [Display(Name = "表示順")]
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
            [Description("対象年度")]
            Nendo,
            [Description("都道府県")]
            TodofukenCd,
            [Description("組合等")]
            KumiaitoCd,
            [Description("支所")]
            ShishoCd,
            [Description("市町村")]
            ShichosonCd,
            [Description("大地区")]
            DaichikuCd,
            [Description("小地区")]
            ShochikuCd,
            [Description("氏名又は法人名")]
            HojinFullNm
        }
    }
}
