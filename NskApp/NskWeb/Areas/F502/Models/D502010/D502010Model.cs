using CoreLibrary.Core.Base;
using ModelLibrary.Models;
using NskWeb.Areas.F000.Models.D000000;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F502.Models.D502010
{
    [Serializable]
    public class D502010Model : CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D502010Model()
        {
            D502010Info = new NSKPortalInfoModel();
        }

        /// <summary>
        /// ポータル画面情報
        /// </summary>
        public NSKPortalInfoModel D502010Info { get; set; }

        /// <summary>
        /// 農業者ID
        /// </summary>
        [Display(Name = "農業者ID")]
        public string NogyoshaId { get; set; }

        /// <summary>
        /// 農業者情報
        /// </summary>
        [Display(Name = "農業者情報")]
        public VNogyosha nogyosha { get; set; }

        /// <summary>
        /// 権限フラグ
        /// </summary>
        public bool UpdateKengenFlg { get; set; }

        /// <summary>
        /// メッセージエリア1
        /// </summary>
        [Display(Name = "メッセージエリア1")]
        public string MessageArea1 { get; set; }

        /// <summary>
        /// 支所名
        /// </summary>
        [Display(Name = "支所名")]
        public string ShishoNm { get; set; }

        /// <summary>
        /// 大地区名
        /// </summary>
        [Display(Name = "大地区名")]
        public string DaichikuNm { get; set; }

        /// <summary>
        /// 小地区名
        /// </summary>
        [Display(Name = "小地区名")]
        public string ShochikuNm { get; set; }

        /// <summary>
        /// 市町村名
        /// </summary>
        [Display(Name = "市町村名")]
        public string ShichosonNm { get; set; }

        /// <summary>
        /// 設定状況
        /// </summary>
        [Display(Name = "設定状況")]
        public List<D502010TableRecord> tableRecords { get; set; }
    }
}