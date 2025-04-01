using CoreLibrary.Core.Base;
using ModelLibrary.Models;
using NskWeb.Areas.F000.Models.D000000;
using System.ComponentModel;

namespace NskWeb.Areas.F102.Models.D102010
{
    [Serializable]
    public class D102010Model : CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D102010Model()
        {
            this.VSyokuinRecords = new VSyokuin();
            this.D102010Info = new NSKPortalInfoModel();  // $$$$$$$$$$$$$$$$$$$
        }

        /// <summary>
        /// 職員マスタの検索結果
        /// </summary>
        public VSyokuin VSyokuinRecords { get; set; }
        public NSKPortalInfoModel D102010Info { get; set; }

        [DisplayName("メッセージエリア")]
        public string MessageArea { get; set; }

        [DisplayName("共済目的名称")]
        public string KyosaiMokutekiMeisho { get; set; }

        /// <summary>
        /// 更新権限フラグ
        /// </summary>
        public bool UpdateKengenFlg { get; set; }

        public string TorikomiFilePath { get; set; }
    }
}