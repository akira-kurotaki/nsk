using CoreLibrary.Core.Pager;

namespace NskWeb.Areas.F000.Models.D000021
{
    /// <summary>
    /// 品種コード検索(子画面)（検索結果表示）
    /// </summary>
    [Serializable]
    public class D000021SearchResult
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D000021SearchResult()
        {
            TableRecords = new List<D000021TableRecord>();
        }

        /// <summary>
        /// 品種コード一覧
        /// </summary>
        public List<D000021TableRecord> TableRecords { get; set; }

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
