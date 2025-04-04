
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Validator;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F107.Models.D107060
{
    /// <summary>
    /// 消込み処理（手動）画面項目モデル
    /// </summary>
    [Serializable]
    public class D107060Model: CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D107060Model()
        {
            this.SearchCondition = new D107060SearchCondition();
            this.SearchResult = new D107060SearchResult();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin">ユーザー情報（セッション）</param>
        /// <param name="shishoList">利用可能な支所一覧（セッション）</param>
        public D107060Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.SearchCondition = new D107060SearchCondition(syokuin, shishoList);
            this.SearchResult = new D107060SearchResult();
        }

        #region 画面権限
        /// <summary>
        /// 画面権限
        /// </summary>
        public int? DispKengen { get; set; }

        /// <summary>
        /// 自動振替済みの権限
        /// </summary>
        public int? JidoFurikaeKengen { get; set; }

        /// <summary>
        /// 引受確定の有無
        /// </summary>
        public bool ExistsHikiukeKakutei { get; set; } = false;
        #endregion

        #region その他画面項目
        /// メッセージエリア1
        /// </summary>
        [Display(Name = "メッセージエリア1")]
        public string MessageArea1 { get; set; }
        
        /// メッセージエリア2
        /// </summary>
        [Display(Name = "メッセージエリア2")]
        public string MessageArea2 { get; set; }

        /// <summary>
        /// 検索条件
        /// </summary>
        [Display(Name = "検索条件")]
        public D107060SearchCondition SearchCondition { get; set; }

        /// <summary>
        /// 検索結果
        /// </summary>
        [Display(Name = "検索結果")]
        public D107060SearchResult SearchResult { get; set; }
        #endregion
    }
}
