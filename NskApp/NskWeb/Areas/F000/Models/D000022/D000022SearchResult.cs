using CoreLibrary.Core.Pager;

namespace NskWeb.Areas.F000.Models.D000022
{
    /// <summary>
    /// 産地別銘柄コード検索(子画面)（検索結果表示）
    /// </summary>
    [Serializable]
    public class D000022SearchResult
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D000022SearchResult()
        {
            TableRecords = new List<D000022TableRecord>();
        }

        /// <summary>
        /// 産地別銘柄コード一覧
        /// </summary>
        public List<D000022TableRecord> TableRecords { get; set; }

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
