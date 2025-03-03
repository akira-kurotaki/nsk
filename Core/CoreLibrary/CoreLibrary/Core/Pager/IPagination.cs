namespace CoreLibrary.Core.Pager
{
    /// <summary>
    /// ページング用モデルのインタフェース
    /// </summary>
    /// <remarks>
    /// 作成日：2017/11/21
    /// 作成者：Rou I
    /// </remarks>
    public interface IPagination
    {
        /// <summary>
        /// 現在ページ
        /// </summary>
        int CurrentPage { get; }

        /// <summary>
        /// 総件数
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// ページごとの表示件数
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// ページ数
        /// </summary>
        int MaxPage { get; }

        /// <summary>
        /// 現在ページの表示範囲(開始)
        /// </summary>
        int CurrentPageFrom { get; }

        /// <summary>
        /// 現在ページの表示範囲(終了)
        /// </summary>
        int CurrentPageTo { get; }

        /// <summary>
        /// 前ページ有無
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// 次ページ有無
        /// </summary>
        bool HasNextPage { get; }
    }
}