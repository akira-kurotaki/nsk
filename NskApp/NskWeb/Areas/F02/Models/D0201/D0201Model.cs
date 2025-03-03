
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F02.Models.D0201
{
    /// <summary>
    /// 一括帳票出力画面項目モデル
    /// </summary>
    [Serializable]
    public class D0201Model: CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D0201Model()
        {
            this.SearchCondition = new D0201SearchCondition();
            this.SearchResult = new D0201SearchResult();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin">ユーザー情報（セッション）</param>
        /// <param name="shishoList">利用可能な支所一覧（セッション）</param>
        public D0201Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.SearchCondition = new D0201SearchCondition(syokuin, shishoList);
            this.SearchResult = new D0201SearchResult();
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
        public D0201SearchCondition SearchCondition { get; set; }

        /// <summary>
        /// メッセージエリア2
        /// </summary>
        [Display(Name = "メッセージエリア2")]
        public string MessageArea2 { get; set; }

        /// <summary>
        /// メッセージエリア3
        /// </summary>
        [Display(Name = "メッセージエリア3")]
        public string MessageArea3 { get; set; }

        /// <summary>
        /// 検索結果
        /// </summary>
        [Display(Name = "検索結果")]
        public D0201SearchResult SearchResult { get; set; }

        /// <summary>
        /// 選択作成ボタンを押下フラグ
        /// </summary>
        public bool isSelectBtnClick { get; set; }
    }
}
