using CoreLibrary.Core.Base;
using ModelLibrary.Models;
using NskWeb.Areas.F000.Models.D000000;
using System.ComponentModel;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.DropDown;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F205.Models.D205080
{
    [Serializable]
    public class D205080Model : CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D205080Model()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D205080Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.VSyokuinRecords = new VSyokuin();
            this.D205080Info = new NSKPortalInfoModel();
            this.TodofukenDropDownList = new TodofukenDropDownList("D205080", syokuin, shishoList);
        }

        /// <summary>
        /// 職員マスタの検索結果
        /// </summary>
        public VSyokuin VSyokuinRecords { get; set; }
        public NSKPortalInfoModel D205080Info { get; set; }

        [DisplayName("メッセージエリア")]
        public string MessageArea { get; set; }

        [DisplayName("共済目的名称")]
        public string KyosaiMokutekiMeisho { get; set; }

        /// <summary>
        /// 更新権限フラグ
        /// </summary>
        public bool UpdateKengenFlg { get; set; }

        /// <summary>
        /// 支所ドロップダウンリスト用
        /// </summary>
        [DisplayName("支所コード")]
        public string ShishoCd { get; set; }

        /// <summary>
        /// 都道府県マルチドロップダウンリスト
        /// </summary>
        [Display(Name = "都道府県マルチドロップダウンリスト")]
        public TodofukenDropDownList TodofukenDropDownList { get; set; }

        /// <summary>
        /// 対象データ振替日
        /// </summary>
        [Display(Name = "対象データ振替日")]
        public DateTime? TaishoFurikaeDate { get; set; }
    }
}