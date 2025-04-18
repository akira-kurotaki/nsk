
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Validator;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F000.Models.D000020
{
    /// <summary>
    /// 組合員等コード検索(子画面)画面項目モデル
    /// </summary>
    [Serializable]
    public class D000020Model: CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D000020Model()
        {
            this.SearchCondition = new D000020SearchCondition();
            this.SearchResult = new D000020SearchResult();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D000020Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.SearchCondition = new D000020SearchCondition(syokuin, shishoList);
            this.SearchResult = new D000020SearchResult();
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
        public D000020SearchCondition SearchCondition { get; set; }

        /// <summary>
        /// 検索結果
        /// </summary>
        [Display(Name = "検索結果")]
        public D000020SearchResult SearchResult { get; set; }

        /// <summary>
        /// 名称取得用コード（要求）
        /// </summary>
        public string TargetCd {  get; set; }
        /// <summary>
        /// 名称取得用名称（戻り値）
        /// </summary>
        public string ReturnNm { get; set; }
    }
}
