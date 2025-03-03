using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F01.Models.D0101
{
    /// <summary>
    /// 加入者一覧モデル
    /// </summary>
    [Serializable]
    public class D0101Model : CoreViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D0101Model()
        {
            this.SearchCondition = new D0101SearchCondition();
            this.SearchResult = new D0101SearchResult();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D0101Model(Syokuin syokuin, List<Shisho> shishoList)
        {
            this.SearchCondition = new D0101SearchCondition(syokuin, shishoList);
            this.SearchResult = new D0101SearchResult();
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
        public D0101SearchCondition SearchCondition { get; set; }

        /// <summary>
        /// メッセージエリア2
        /// </summary>
        [Display(Name = "メッセージエリア2")]
        public string MessageArea2 { get; set; }

        /// <summary>
        /// 検索結果
        /// </summary>
        [Display(Name = "検索結果")]
        public D0101SearchResult SearchResult { get; set; }

        /// <summary>
        /// 加入者情報参照可否フラグ
        /// </summary>
        [Display(Name = "加入者情報参照可否フラグ")]
        public string KanyushaReferenceFlag { get; set; }

        /// <summary>
        /// 一覧表作成ボタン参照可否フラグ
        /// </summary>
        [Display(Name = "一覧表作成ボタン参照可否フラグ")]
        public string CreateCSVReferenceFlag { get; set; }
    }

    /// <summary>
    /// 加入者一覧CSV出力用（検索条件、ソート順、パラメータ、検索テーブル部分）
    /// </summary>
    [Serializable]
    public class D0101SqlInfoForCsv
    {
        /// <summary>
        /// 検索条件のSQL文
        /// </summary>
        public string ConditionSqlStr { get; set; }

        /// <summary>
        /// 検索条件の論理名称と値
        /// </summary>
        public string WhereName { get; set; }

        /// <summary>
        /// パラメータリスト
        /// </summary>
        public List<SqlParam> SqlParams { get; set; }

        /// <summary>
        /// ソート順のSQL文
        /// </summary>
        public string OrderBySqlStr { get; set; }

        /// <summary>
        /// ソート順の論理名称と値
        /// </summary>
        public string OrderByName { get; set; }
    }
}
