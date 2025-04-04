using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using CoreLibrary.Core.Validator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static NskWeb.Areas.F107.Models.D107060.D107060SearchCondition;
using static NskWeb.Areas.F107.Models.D107060.D107060SearchResult;
using NskWeb.Areas.F107.Models.D107060;
using Microsoft.AspNetCore.Mvc.Rendering;
using NskAppModelLibrary.Context;
using NskWeb.Areas.F000.Models.D000000;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F107.Models.D107060
{
    /// <summary>
    /// 消込み処理（手動）画面項目モデル（検索条件部分）
    /// </summary>
    [Serializable]
    public class D107060SearchCondition
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D107060SearchCondition()
        {
            this.TodofukenDropDownList = new TodofukenDropDownList("SearchCondition");

            // 本所・支所ドロップダウンリスト用
            HonshoshishoList = new List<SelectListItem>();
            HonshoshishoModelList = new List<D107000HonshoshishoList>();

            // 徴収予定区分ドロップダウンリスト用
            ChoshuYoteiList = new List<SelectListItem>();
            ChoshuYoteiModelList = new List<D107060ChoshuYoteiList>();

            // 徴収区分ドロップダウンリスト用
            ChoshuKbnList = new List<SelectListItem>();
            ChoshuKbnModelList = new List<D107060ChoshuKbnList>();

            // 徴収理由ドロップダウンリスト用
            ChoshuRiyuList = new List<SelectListItem>();
            ChoshuRiyuModelList = new List<D107060ChoshuRiyuList>();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin">ユーザー情報（セッション）</param>
        /// <param name="shishoList">利用可能な支所一覧（セッション）</param>
        public D107060SearchCondition(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.TodofukenDropDownList = new TodofukenDropDownList("SearchCondition", syokuin, shishoList);
        }

        #region セッション情報表示項目
        /// <summary>
        /// 年産
        /// </summary>
        [Display(Name = "年産")]
        [FullStringLength(4)]
        public string Nensan { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Display(Name = "共済目的コード")]
        [FullStringLength(2)]
        public string KyosaiMokutekiCd { get; set; }

        /// <summary>
        /// 共済目的名称
        /// </summary>
        [Display(Name = "共済目的名称")]
        public string KyosaiMokutekiNm { get; set; }

        /// <summary>
        /// 引受計算支所実行単位区分_引受
        /// </summary>
        [Display(Name = "引受計算支所実行単位区分_引受")]
        public string ShishoJikkoHikiukeKbn { get; set; }
        #endregion

        #region 検索条件項目
        /// <summary>
        /// 都道府県マルチドロップダウンリスト
        /// </summary>
        [Display(Name = "都道府県マルチドロップダウンリスト")]
        public TodofukenDropDownList TodofukenDropDownList { get; set; }

        /// <summary>
        /// 本所・支所リスト
        /// </summary>
        public List<SelectListItem> HonshoshishoList { get; set; }
        /// <summary>
        /// 本所・支所モデルリスト
        /// </summary>
        public List<D107000HonshoshishoList> HonshoshishoModelList { get; set; }

        /// <summary>
        /// 選択支所コード
        /// </summary>
        [Required]
        [Display(Name = "選択支所コード")]
        public string SelectShishoCd { get; set; }

        /// <summary>
        /// 選択大地区コード
        /// </summary>
        [Required]
        [Display(Name = "選択大地区コード")]
        public string SelectDaichikuCd { get; set; }

        /// <summary>
        /// 選択小地区開始コード
        /// </summary>
        [Required]
        [Display(Name = "選択小地区開始コード")]
        public string ShochikuCdFrom { get; set; }

        /// <summary>
        /// 選択小地区終了コード
        /// </summary>
        [Required]
        [Display(Name = "選択小地区終了コード")]
        public string ShochikuCdTo { get; set; }

        /// <summary>
        /// 選択市町村コード
        /// </summary>
        [Required]
        [Display(Name = "選択市町村コード")]
        public string ShichosonCd { get; set; }

        /// <summary>
        /// 引受回
        /// </summary>
        [Display(Name = "引受回")]
        [Required]
        [Numeric]
        [WithinDigitLength(2)]
        public int? HikiukeCnt { get; set; }

        /// <summary>
        /// 引受回退避
        /// </summary>
        [Display(Name = "引受回退避")]
        [Numeric]
        public int? HikiukeCntHidden { get; set; }

        /// <summary>
        /// 組合員等コード（開始）
        /// </summary>
        [Display(Name = "組合員等コード（開始）")]
        [HalfWidthAlphaNum]
        [WithinStringLength(13)]
        public string KumiaiintoCdFrom { get; set; }
        
        /// <summary>
        /// 組合員等コード（終了）
        /// </summary>
        [Display(Name = "組合員等コード（終了）")]
        [HalfWidthAlphaNum]
        [WithinStringLength(13)]
        public string KumiaiintoCdTo { get; set; }

        /// <summary>
        /// 徴収予定区分
        /// </summary>
        public string ChoshuYoteiKbn { get; set; }
        /// <summary>
        /// 徴収予定区分ドロップダウンリスト
        /// </summary>
        public List<SelectListItem> ChoshuYoteiList { get; set; } = new();
        /// <summary>
        /// 徴収予定区分モデルリスト
        /// </summary>
        public List<D107060ChoshuYoteiList> ChoshuYoteiModelList { get; set; } = new();

        /// <summary>
        /// 自動振替予定
        /// </summary>
        public string JidoFurikaeYotei { get; set; }
        /// <summary>
        /// 自動振替予定ドロップダウンリスト
        /// </summary>
        public JidoFurikaeYoteiType? JidoFurikaeYoteiList { get; set; }

        /// <summary>
        /// 未納者のみチェックボックス
        /// </summary>
        [Display(Name = "未納者のみ")]
        public bool MinosyaOnly { get; set; }
        #endregion

        #region ソート順項目
        /// <summary>
        /// 表示数
        /// </summary>
        [Required]
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
        #endregion

        #region アコーディオン項目
        /// <summary>
        /// 徴収区分
        /// </summary>
        [Display(Name = "徴収区分(一括設定用)")]
        public decimal? ChoshuKbn { get; set; }
        /// <summary>
        /// 徴収区分ドロップダウンリスト
        /// </summary>
        public List<SelectListItem> ChoshuKbnList { get; set; }
        /// <summary>
        /// 徴収区分モデルリスト
        /// </summary>
        public List<D107060ChoshuKbnList> ChoshuKbnModelList { get; set; }

        /// <summary>
        /// 徴収年月日
        /// </summary>
        [Display(Name = "徴収年月日(一括設定用)")]
        public DateTime? ChoshuYmd { get; set; }

        /// <summary>
        /// 徴収理由
        /// </summary>
        [Display(Name = "徴収理由(一括設定用)")]
        public decimal? ChoshuRiyu { get; set; }
        /// <summary>
        /// 徴収理由ドロップダウンリスト
        /// </summary>
        public List<SelectListItem> ChoshuRiyuList { get; set; }
        /// <summary>
        /// 徴収理由モデルリスト
        /// </summary>
        public List<D107060ChoshuRiyuList> ChoshuRiyuModelList { get; set; }

        /// <summary>
        /// 徴収者
        /// </summary>
        [Display(Name = "徴収者(一括設定用)")]
        public string Choshusya { get; set; }

        #endregion

        #region 検索結果項目
        /// <summary>
        /// 検索結果表示フラグ
        /// </summary>
        [Display(Name = "検索結果表示フラグ")]
        public bool IsResultDisplay { get; set; }
        #endregion

        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src"></param>
        public void ApplyInput(D107060SearchCondition src)
        {
            this.Nensan = src.Nensan;
            this.KyosaiMokutekiCd = src.KyosaiMokutekiCd;
            this.KyosaiMokutekiNm = src.KyosaiMokutekiNm;
            this.ShishoJikkoHikiukeKbn = src.ShishoJikkoHikiukeKbn;
            this.SelectShishoCd = src.SelectShishoCd;
            this.SelectDaichikuCd = src.TodofukenDropDownList.DaichikuCd;
            this.ShochikuCdFrom = src.TodofukenDropDownList.ShochikuCdFrom;
            this.ShochikuCdTo = src.TodofukenDropDownList.ShochikuCdTo;
            this.ShichosonCd = src.TodofukenDropDownList.ShichosonCd;
            this.HikiukeCnt = src.HikiukeCnt;
            this.KumiaiintoCdFrom = src.KumiaiintoCdFrom;
            this.KumiaiintoCdTo = src.KumiaiintoCdTo;
            this.ChoshuYoteiKbn = src.ChoshuYoteiKbn;
            this.ChoshuYoteiList = src.ChoshuYoteiList;
            this.JidoFurikaeYotei = src.JidoFurikaeYotei;
            this.JidoFurikaeYoteiList = src.JidoFurikaeYoteiList;
            this.MinosyaOnly = src.MinosyaOnly;
            this.DisplayCount = src.DisplayCount;
            this.DisplaySort1 = src.DisplaySort1;
            this.DisplaySortOrder1 = src.DisplaySortOrder1;
            this.DisplaySort2 = src.DisplaySort2;
            this.DisplaySortOrder2 = src.DisplaySortOrder2;
            this.DisplaySort3 = src.DisplaySort3;
            this.DisplaySortOrder3 = src.DisplaySortOrder3;
            this.ChoshuKbn = src.ChoshuKbn;
            this.ChoshuKbnList = src.ChoshuKbnList;
            this.ChoshuYmd = src.ChoshuYmd;
            this.ChoshuRiyu = src.ChoshuRiyu;
            this.ChoshuRiyuList = src.ChoshuRiyuList;
            this.Choshusya = src.Choshusya;
            this.IsResultDisplay = src.IsResultDisplay;
        }

        #region ドロップダウンリスト要素
        /// <summary>
        /// 自動振替予定ドロップダウンリスト要素
        /// </summary>
        public enum JidoFurikaeYoteiType
        {
            /// <summary>有り</summary>
            [Description("1有り")]
            True = 1,
            //1,
            /// <summary>無し</summary>
            [Description("2無し")]
            False = 0,
            //0,
        }

        /// <summary>
        /// 表示順ドロップダウンリスト要素
        /// </summary>
        [Flags]
        public enum DisplaySortType
        {
            [Description("本所・支所")]
            ShishoCd,
            [Description("引受回")]
            HikiukeCnt,
            [Description("大地区")]
            DaichikuCd,
            [Description("小地区")]
            ShochikuCd,
            [Description("市町村")]
            ShichosonCd,
            [Description("組合員等コード")]
            KumiaitoCd,
            [Description("徴収予定区分")]
            ChoshuYoteiKbn,
            [Description("自動振替予定")]
            JidoFurikaeYotei,
            [Description("未納")]
            MinosyaOnly,
        }
        #endregion
    }
}
