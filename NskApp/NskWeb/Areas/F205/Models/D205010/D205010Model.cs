using CoreLibrary.Core.Base;
using ModelLibrary.Models;
using NskWeb.Areas.F000.Models.D000000;
using System.ComponentModel;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.DropDown;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F205.Models.D205010
{
    [Serializable]
    public class D205010Model : CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D205010Model()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D205010Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.VSyokuinRecords = new VSyokuin();
            this.D205010Info = new NSKPortalInfoModel();
            this.TodofukenDropDownList = new TodofukenDropDownList("D205010", syokuin, shishoList);
        }

        /// <summary>
        /// 職員マスタの検索結果
        /// </summary>
        public VSyokuin VSyokuinRecords { get; set; }
        public NSKPortalInfoModel D205010Info { get; set; }

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
    }
}