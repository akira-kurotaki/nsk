using NskWeb.Areas.F02.Models.D0201;
using CoreLibrary.Core.Pager;

namespace NskWeb.Areas.F04.Models.D0401
{
    /// <summary>
    /// ファイル取込（検索結果）
    /// </summary>
    [Serializable]
    public class D0401SearchResult
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D0401SearchResult()
        {
            TableRecords = new List<D0401TableRecord>();
        }

        /// <summary>
        /// 検索結果一覧
        /// </summary>
        public List<D0401TableRecord> TableRecords { get; set; }

        /// <summary>
        /// ページャー
        /// </summary>
        public Pagination Pager { get; set; }

        /// <summary>
        /// 検索結果全件数
        /// </summary>
        public int TotalCount { get; set; }

    }
}
