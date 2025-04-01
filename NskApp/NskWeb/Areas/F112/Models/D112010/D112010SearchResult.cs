using CoreLibrary.Core.Pager;
using NskWeb.Areas.F112.Models.D112010;

namespace NskWeb.Areas.F112.Models.D112010
{
    /// <summary>
    /// 一括帳票出力画面項目モデル（検索結果部分）
    /// </summary>
    [Serializable]
    public class D112010SearchResult
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D112010SearchResult()
        {
            TableRecords = new List<D112010TableRecord>();
        }

        /// <summary>
        /// 検索結果一覧
        /// </summary>
        public List<D112010TableRecord> TableRecords { get; set; }

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
