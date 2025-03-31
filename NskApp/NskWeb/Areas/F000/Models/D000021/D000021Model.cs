
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Validator;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F000.Models.D000021
{
    /// <summary>
    /// 品種コード検索(子画面)画面項目モデル
    /// </summary>
    [Serializable]
    public class D000021Model: CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D000021Model()
        {
            this.SearchCondition = new D000021SearchCondition();
            this.SearchResult = new D000021SearchResult();
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
        public D000021SearchCondition SearchCondition { get; set; }

        /// <summary>
        /// 検索結果
        /// </summary>
        [Display(Name = "検索結果")]
        public D000021SearchResult SearchResult { get; set; }
    }
}
