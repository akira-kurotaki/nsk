
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Validator;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F000.Models.D000023
{
    /// <summary>
    /// 統計単位地域コード検索(子画面)画面項目モデル
    /// </summary>
    [Serializable]
    public class D000023Model: CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D000023Model()
        {
            this.SearchCondition = new D000023SearchCondition();
            this.SearchResult = new D000023SearchResult();
        }

        /// <summary>
        /// メッセージエリア1
        /// </summary>
        [Display(Name = "メッセージエリア1")]
        public string MessageArea1 { get; set; }

        /// <summary>
        /// 検索条件
        /// </summary>
        [Display(Name = "検索条件")]
        public D000023SearchCondition SearchCondition { get; set; }

        /// <summary>
        /// 検索結果
        /// </summary>
        [Display(Name = "検索結果")]
        public D000023SearchResult SearchResult { get; set; }
    }
}
