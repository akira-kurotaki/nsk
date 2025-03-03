using CoreLibrary.Core.Consts;

namespace CoreLibrary.Core.Pager
{
    /// <summary>
    /// ページング用モデル
    /// </summary>
    /// <remarks>
    /// 作成日：2017/11/21
    /// 作成者：Rou I
    /// </remarks>
    [Serializable]
    public class Pagination : IPagination
    {
        /// <summary>
        /// 現在ページ
        /// </summary>
        //public int CurrentPage { get; private set; }
        public int CurrentPage { get; set; }

        /// <summary>
        /// 総件数
        /// </summary>
        //public int TotalCount { get; private set; }
        public int TotalCount { get; set; }

        /// <summary>
        /// ページごとの表示件数
        /// </summary>
        //public int PageSize { get; private set; }
        public int PageSize { get; set; }

        /// <summary>
        /// ページ数
        /// </summary>
        //public int MaxPage { get; private set; }
        public int MaxPage { get; set; }

        /// <summary>
        /// 現在ページの表示範囲(開始)
        /// </summary>
        //public int CurrentPageFrom { get; private set; }
        public int CurrentPageFrom { get; set; }

        /// <summary>
        /// 現在ページの表示範囲(終了)
        /// </int>
        //public int CurrentPageTo { get; private set; }
        public int CurrentPageTo { get; set; }


        /// <summary>
        /// 前ページ有無
        /// </summary>
        public bool HasPreviousPage
        {
            get { return CurrentPage > 1; }
        }

        /// <summary>
        /// 次ページ有無
        /// </summary>
        public bool HasNextPage
        {
            get { return (TotalCount - CurrentPage * PageSize) > 0; }
        }

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public Pagination()
        {
            // 何もしない
        }

        /// <summary>
        /// コンストラクタ(現在ページ、データ件数)
        /// </summary>
        /// <param name="currentPage">現在ページ</param>
        /// <param name="totalCount">データ件数</param>
        public Pagination(int currentPage, int totalCount)
        {
            CurrentPage = currentPage;
            TotalCount = totalCount;
            PageSize = CoreConst.PAGE_SIZE;
            MaxPage = GetMaxPage(TotalCount, PageSize);
            CurrentPageFrom = GetCurrentPageFrom(CurrentPage, PageSize);
            CurrentPageTo = GetCurrentPageTo(CurrentPage, TotalCount, PageSize);
        }

        /// <summary>
        /// コンストラクタ(現在ページ、ページごとの表示件数、データ件数)
        /// </summary>
        /// <param name="currentPage">現在ページ</param>
        /// <param name="pageSize">ページごとの表示件数</param>
        /// <param name="totalCount">データ件数</param>
        public Pagination(int currentPage, int pageSize, int totalCount)
        {
            CurrentPage = currentPage;
            TotalCount = totalCount;
            PageSize = pageSize;
            MaxPage = GetMaxPage(TotalCount, PageSize);
            CurrentPageFrom = GetCurrentPageFrom(CurrentPage, PageSize);
            CurrentPageTo = GetCurrentPageTo(CurrentPage, TotalCount, PageSize);
        }

        /// <summary>
        /// 最大ページ数取得メソッド。
        /// </summary>
        /// <param name="totalCount">データ件数</param>
        /// <param name="pageSize">ページごとの表示件数</param>
        /// <returns>最大ページ数</returns>
        private int GetMaxPage(int totalCount, int pageSize)
        {
            if (totalCount % pageSize > 0)
            {
                return totalCount / pageSize + 1;
            }
            else
            {
                return totalCount / pageSize;
            }
        }

        /// <summary>
        /// 現在ページの表示範囲(開始)の取得メソッド。
        /// </summary>
        /// <param name="currentPage">現在ページ</param>
        /// <param name="pageSize">ページごとの表示件数</param>
        /// <returns>現在ページの表示範囲(開始)</returns>
        private int GetCurrentPageFrom(int currentPage, int pageSize)
        {
            return (currentPage - 1) * pageSize + 1;
        }

        /// <summary>
        /// 現在ページの表示範囲(終了)の取得メソッド。
        /// </summary>
        /// <param name="currentPage">現在ページ</param>
        /// <param name="totalCount">データ件数</param>
        /// <param name="pageSize">ページごとの表示件数</param>
        /// <returns>現在ページの表示範囲(終了)</returns>
        private int GetCurrentPageTo(int currentPage, int totalCount, int pageSize)
        {
            if (totalCount < pageSize)
            {
                return totalCount;
            }
            else
            {
                if (currentPage * pageSize > totalCount)
                {
                    return totalCount;
                }
                else
                {
                    return currentPage * pageSize;
                }
            }

        }
    }
}