
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F204.Models.D204092
{
    /// <summary>
    /// 一括帳票出力画面項目モデル
    /// </summary>
    [Serializable]
    public class D204092Model: CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D204092Model()
        {
        }

        /// <summary>
        /// メッセージエリア1
        /// </summary>
        [Display(Name = "メッセージエリア1")]
        public string MessageArea1 { get; set; }

        [Display(Name = "処理Id")]
        public string OpeId { get; set; }

    }
}
