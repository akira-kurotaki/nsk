using CoreLibrary.Core.Pager;

namespace NskWeb.Areas.F02.Models.D0201
{
    /// <summary>
    /// 一括帳票出力画面項目モデル（検索結果部分）
    /// </summary>
    [Serializable]
    public class D0201SearchResult
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D0201SearchResult()
        {
            TableRecords = new List<D0201TableRecord>();
        }

        /// <summary>
        /// 検索結果一覧
        /// </summary>
        public List<D0201TableRecord> TableRecords { get; set; }

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
