using CoreLibrary.Core.Base;
using ModelLibrary.Models;
using NskWeb.Areas.F000.Models.D000000;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using CoreLibrary.Core.Dto;

namespace NskWeb.Areas.F206.Models.D206126
{
    [Serializable]
    public class D206126Model : CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D206126Model()
        {
            this.VSyokuinRecords = new VSyokuin();
            this.D206126Info = new NSKPortalInfoModel(); 
        }

        /// <summary>
        /// 職員マスタの検索結果
        /// </summary>
        public VSyokuin VSyokuinRecords { get; set; }
        public NSKPortalInfoModel D206126Info { get; set; }

        [DisplayName("メッセージエリア")]
        public string MessageArea { get; set; }

        [DisplayName("共済目的名称")]
        public string KyosaiMokutekiMeisho { get; set; }

        /// <summary>
        /// 更新権限フラグ
        /// </summary>
        public bool UpdateKengenFlg { get; set; }
    }
}