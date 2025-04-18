
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Validator;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F000.Models.D000022
{
    /// <summary>
    /// 産地別銘柄コード検索(子画面)画面項目モデル
    /// </summary>
    [Serializable]
    public class D000022Model: CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D000022Model()
        {
            this.SearchCondition = new D000022SearchCondition();
            this.SearchResult = new D000022SearchResult();
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
        public D000022SearchCondition SearchCondition { get; set; }

        /// <summary>
        /// 検索結果
        /// </summary>
        [Display(Name = "検索結果")]
        public D000022SearchResult SearchResult { get; set; }

        /// <summary>
        /// 名称取得用コード（要求）
        /// </summary>
        public string TargetCd { get; set; }
        /// <summary>
        /// 名称取得用名称（戻り値）
        /// </summary>
        public string ReturnNm { get; set; }
    }
}
