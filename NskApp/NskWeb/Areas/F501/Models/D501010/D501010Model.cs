using CoreLibrary.Core.Base;
using CoreLibrary.Core.Validator;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelLibrary.Models;
using NskWeb.Areas.F000.Models.D000000;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F501.Models.D501010
{
    [Serializable]
    public class D501010Model : CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D501010Model()
        {
            D501010Info = new NSKPortalInfoModel();
        }

        /// <summary>
        /// ポータル画面情報
        /// </summary>
        public NSKPortalInfoModel D501010Info { get; set; }

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
        /// 加入状況
        /// </summary>
        [Display(Name = "加入状況")]
        public List<D501010TableRecord> tableRecords { get; set; }
    }
}