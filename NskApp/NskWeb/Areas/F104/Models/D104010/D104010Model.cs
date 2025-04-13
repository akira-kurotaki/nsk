using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Validator;
using ModelLibrary.Models;
using NskWeb.Areas.F000.Models.D000000;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F104.Models.D104010
{
    [Serializable]
    public class D104010Model : CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D104010Model()
        {
            this.VSyokuinRecords = new VSyokuin();
            this.D104010Info = new NSKPortalInfoModel();
            this.SearchCondition = new D104010SearchCondition();
            this.SearchResult = new D104010SearchResult();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin">ユーザー情報（セッション）</param>
        /// <param name="shishoList">利用可能な支所一覧（セッション）</param>
        public D104010Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.VSyokuinRecords = new VSyokuin();
            this.D104010Info = new NSKPortalInfoModel();
            this.SearchCondition = new D104010SearchCondition(syokuin, shishoList);
            this.SearchResult = new D104010SearchResult();
        }

        /// <summary>
        /// 職員マスタの検索結果
        /// </summary>
        public VSyokuin VSyokuinRecords { get; set; }
        public NSKPortalInfoModel D104010Info { get; set; }

        /// <summary>
        /// メッセージエリア1
        /// </summary>
        [Display(Name = "メッセージエリア1")]
        public string MessageArea1 { get; set; }

        /// <summary>
        /// 検索条件
        /// </summary>
        [Display(Name = "検索条件")]
        public D104010SearchCondition SearchCondition { get; set; }

        /// <summary>
        /// メッセージエリア2
        /// </summary>
        [Display(Name = "メッセージエリア2")]
        public string MessageArea2 { get; set; }

        /// <summary>
        /// メッセージエリア3
        /// </summary>
        [Display(Name = "メッセージエリア3")]
        public string MessageArea3 { get; set; }

        /// <summary>
        /// 検索結果
        /// </summary>
        [Display(Name = "検索結果")]
        public D104010SearchResult SearchResult { get; set; }

        /// <summary>
        /// 共済目的名称
        /// </summary>
        [DisplayName("共済目的名称")]
        public string KyosaiMokutekiMeisho { get; set; }

        /// <summary>
        /// 更新権限フラグ
        /// </summary>
        public bool UpdateKengenFlg { get; set; }

    }
}