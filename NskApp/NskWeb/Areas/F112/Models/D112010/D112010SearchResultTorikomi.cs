using CoreLibrary.Core.Pager;
using NskWeb.Areas.F112.Models.D112010;

namespace NskWeb.Areas.F112.Models.D112010
{
    /// <summary>
    /// 一括帳票出力画面項目モデル（検索結果部分）
    /// </summary>
    [Serializable]
    public class D112010SearchResultTorikomi
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D112010SearchResultTorikomi()
        {
            TableRecords = new List<D112010TableRecordTorikomi>();
        }

        /// <summary>
        /// 検索結果一覧
        /// </summary>
        public List<D112010TableRecordTorikomi> TableRecords { get; set; }

        /// <summary>
        /// ページャー
        /// </summary>
        public Pagination Pager { get; set; }

        /// <summary>
        /// 検索結果全件数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// チェックオンにした明細データのindex
        /// </summary>
        public List<string> SelectCheckBoxes { get; set; }
    }
}
