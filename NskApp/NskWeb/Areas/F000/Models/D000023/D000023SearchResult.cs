using CoreLibrary.Core.Pager;

namespace NskWeb.Areas.F000.Models.D000023
{
    /// <summary>
    /// 統計単位地域コード検索(子画面)（検索結果表示）
    /// </summary>
    [Serializable]
    public class D000023SearchResult
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D000023SearchResult()
        {
            TableRecords = new List<D000023TableRecord>();
        }

        /// <summary>
        /// 統計単位地域コード一覧
        /// </summary>
        public List<D000023TableRecord> TableRecords { get; set; }

        /// <summary>
        /// 検索結果件数
        /// </summary>
        public string TotalCount { get; set; }

        /// <summary>
        /// 表部品1
        /// </summary>
        public string TablePart1 { get; set; }

        /// <summary>
        /// 表部品2
        /// </summary>
        public string TablePart2 { get; set; }

        /// <summary>
        /// 表部品3
        /// </summary>
        public string TablePart3 { get; set; }
    }
}
