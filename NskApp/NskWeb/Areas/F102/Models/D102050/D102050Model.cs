using CoreLibrary.Core.Base;
using ModelLibrary.Models;
using NskWeb.Areas.F000.Models.D000000;
using System.ComponentModel;

namespace NskWeb.Areas.F102.Models.D102050
{
    [Serializable]
    public class D102050Model : CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D102050Model()
        {
            this.VSyokuinRecords = new VSyokuin();
            this.D102050Info = new NSKPortalInfoModel();  // $$$$$$$$$$$$$$$$$$$
        }

        /// <summary>
        /// 職員マスタの検索結果
        /// </summary>
        public VSyokuin VSyokuinRecords { get; set; }
        public NSKPortalInfoModel D102050Info { get; set; }

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