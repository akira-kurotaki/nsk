using CoreLibrary.Core.Pager;

namespace NskWeb.Areas.F000.Models.D000020
{
    /// <summary>
    /// 組合員等コード検索(子画面)（検索結果表示）
    /// </summary>
    [Serializable]
    public class D000020SearchResult
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D000020SearchResult()
        {
            TableRecords = new List<D000020TableRecord>();
        }

        /// <summary>
        /// 組合員等一覧
        /// </summary>
        public List<D000020TableRecord> TableRecords { get; set; }

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
