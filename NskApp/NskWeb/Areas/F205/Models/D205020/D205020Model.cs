using CoreLibrary.Core.Base;
using ModelLibrary.Models;
using NskWeb.Areas.F000.Models.D000000;
using System.ComponentModel;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.DropDown;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F205.Models.D205020
{
    [Serializable]
    public class D205020Model : CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D205020Model()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D205020Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.VSyokuinRecords = new VSyokuin();
            this.D205020Info = new NSKPortalInfoModel();
            this.TodofukenDropDownList = new TodofukenDropDownList("D205020", syokuin, shishoList);
        }

        /// <summary>
        /// 職員マスタの検索結果
        /// </summary>
        public VSyokuin VSyokuinRecords { get; set; }
        public NSKPortalInfoModel D205020Info { get; set; }

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

        /// 都道府県マルチドロップダウンリスト	/// <summary>
        /// </summary>	/// 対象者ラジオボタン用
        [Display(Name = "都道府県マルチドロップダウンリスト")]   /// </summary>
        public TodofukenDropDownList TodofukenDropDownList { get; set; }

        /// <summary>
        /// 対象者ラジオボタン用
        /// </summary>
        public string Taishosha { get; set; } = "0";
    }
}