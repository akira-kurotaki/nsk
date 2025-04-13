
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Pager;

namespace NskWeb.Areas.F104.Models.D104010
{
    /// <summary>
    /// 一括帳票出力画面項目モデル（検索結果部分）
    /// </summary>
    [Serializable]
    public class D104010SearchResult
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D104010SearchResult()
        {
            TableRecords = new List<D104010TableRecord>();
            TableAddRecords = new List<D104010TableAddRecord>();
        }

        /// <summary>
        /// 検索結果一覧
        /// </summary>
        public List<D104010TableRecord> TableRecords { get; set; }

        /// <summary>
        /// 追加
        /// </summary>
        public List<D104010TableAddRecord> TableAddRecords { get; set; }
        

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
