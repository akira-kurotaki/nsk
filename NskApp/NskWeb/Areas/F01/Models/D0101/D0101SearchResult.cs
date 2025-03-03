using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Pager;

namespace NskWeb.Areas.F01.Models.D0101
{
    /// <summary>
    /// 加入者一覧(検索結果)
    /// </summary>
    [Serializable]
    public class D0101SearchResult
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public D0101SearchResult()
        {
            TableRecords = new List<D0101TableRecord>();
            this.CharacterCode = CoreConst.CharacterCode.UTF8;
        }

        /// <summary>
        /// 検索結果一覧
        /// </summary>
        public List<D0101TableRecord> TableRecords { get; set; }

        /// <summary>
        /// ページャー
        /// </summary>
        public Pagination Pager { get; set; }

        /// <summary>
        /// 検索結果全件数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 文字コード
        /// </summary>
        public CoreConst.CharacterCode CharacterCode { get; set; }
    }
}
