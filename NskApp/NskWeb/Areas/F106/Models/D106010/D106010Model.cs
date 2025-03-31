
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F106.Models.D106010
{
    /// <summary>
    /// 引受計算処理（水稲）画面項目モデル
    /// </summary>
    [Serializable]
    public class D106010Model: CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D106010Model()
        {
            this.SearchCondition = new D106010SearchCondition();
            this.SearchResult = new D106010SearchResult();
            this.KengenFlg = "0";
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="syokuin">ユーザー情報（セッション）</param>
        /// <param name="shishoList">利用可能な支所一覧（セッション）</param>
        public D106010Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.SearchCondition = new D106010SearchCondition(syokuin, shishoList);
            this.SearchResult = new D106010SearchResult();
            this.KengenFlg = "0";
        }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Display(Name = "共済目的コード")]
        public string KyosaiMokutekiCd { get; set; }/// <summary>

        /// <summary>
        /// 年産
        /// </summary>
        [Display(Name = "年産")]
        public string Nensan { get; set; }/// <summary>

        /// メッセージエリア1
        /// </summary>
        [Display(Name = "メッセージエリア1")]
        public string MessageArea1 { get; set; }

        /// <summary>
        /// 検索条件
        /// </summary>
        [Display(Name = "検索条件")]
        public D106010SearchCondition SearchCondition { get; set; }

        /// <summary>
        /// 検索結果
        /// </summary>
        [Display(Name = "検索結果")]
        public D106010SearchResult SearchResult { get; set; }

        /// <summary>
        /// 権限フラグ
        /// </summary>
        [Display(Name = "権限フラグ")]
        public string KengenFlg { get; set; }

    }
}
