using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Validator;
using ModelLibrary.Models;
using NskWeb.Areas.F000.Models.D000000;
using NskWeb.Areas.F112.Models.D112010;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F112.Models.D112010
{
    [Serializable]
    public class D112010Model : CoreViewModel
    {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D112010Model()
        {
            this.VSyokuinRecords = new VSyokuin();
            this.D112010Info = new NSKPortalInfoModel();
            this.SearchCondition = new D112010SearchCondition();
            this.SearchResult = new D112010SearchResult();
            this.SearchConditionTorikomi = new D112010SearchConditionTorikomi();
            this.SearchResultTorikomi = new D112010SearchResultTorikomi();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin">ユーザー情報（セッション）</param>
        /// <param name="shishoList">利用可能な支所一覧（セッション）</param>
        public D112010Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.VSyokuinRecords = new VSyokuin();
            this.D112010Info = new NSKPortalInfoModel(); 
            this.SearchCondition = new D112010SearchCondition(syokuin, shishoList);
            this.SearchResult = new D112010SearchResult();
            this.SearchConditionTorikomi = new D112010SearchConditionTorikomi(syokuin, shishoList);
            this.SearchResultTorikomi = new D112010SearchResultTorikomi();
        }

        /// <summary>
        /// 職員マスタの検索結果
        /// </summary>
        public VSyokuin VSyokuinRecords { get; set; }

        public string ActiveTab { get; set; } = "UkeireTab";
        public NSKPortalInfoModel D112010Info { get; set; }
        /// <summary>
        /// 検索結果
        /// </summary>
        [Display(Name = "検索結果")]
        public D112010SearchResult SearchResult { get; set; }

        /// <summary>
        /// 検索結果取込
        /// </summary>
        [Display(Name = "検索結果取込")]
        public D112010SearchResultTorikomi SearchResultTorikomi { get; set; }

        /// <summary>
        /// 検索条件
        /// </summary>
        [Display(Name = "検索条件")]
        public D112010SearchCondition SearchCondition { get; set; }

        /// <summary>
        /// 検索条件取込
        /// </summary>
        [Display(Name = "検索条件取込")]
        public D112010SearchConditionTorikomi SearchConditionTorikomi { get; set; }

        [DisplayName("ユーザＩＤ")]
        public string UserId { get; set; }
        [DisplayName("パスワード")]
        public string Password { get; set; }
        [DisplayName("メッセージエリア1")]
        public string MessageArea1 { get; set; }
        [DisplayName("メッセージエリア2")]
        public string MessageArea2 { get; set; }
        [DisplayName("メッセージエリア3")]
        public string MessageArea3 { get; set; }
        [DisplayName("メッセージエリア4")]
        public string MessageArea4 { get; set; }
        [DisplayName("メッセージエリア5")]
        public string MessageArea5 { get; set; }


        /// <summary>
        /// 受入対象
        /// </summary>
        [Display(Name = "受入対象")]
        [Required]
        public string UkeireTaisho { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Display(Name = "支所コード")]
        [Required]
        public string SishoCd { get; set; }

        /// <summary>
        /// 受入ファイルパス
        /// </summary>
        [Display(Name = "支所コード")]
        [Required]
        public string UkeireFilePath { get; set; }

        /// <summary>
        /// コメント
        /// </summary>
        [Display(Name = "コメント")]
        public string Comment { get; set; }

        /// <summary>
        /// 表示数
        /// </summary>
        [Display(Name = "表示数")]
        public int? DisplayCount { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Display(Name = "都道府県コード")]
        [WithinStringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 更新権限フラグ
        /// </summary>
        public bool UpdateKengenFlg { get; set; }
    }
}