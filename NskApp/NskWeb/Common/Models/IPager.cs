using NskAppModelLibrary.Context;

namespace NskWeb.Common.Models
{
    /// <summary>
    /// ページャー用インタフェース
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPager<T> where T : new()
    {
        /// <summary>
        /// ページ分データを取得する
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <param name="pageId">ページID</param>
        public void GetPageDataList(NskAppContext dbContext, BaseSessionInfo sessionInfo, int pageId);

        /// <summary>
        /// 検索結果を取得する
        /// </summary>
        /// <param name="dbContext">DBコンテキスト</param>
        /// <param name="sessionInfo">セッション情報</param>
        /// <returns></returns>
        public List<T> GetResult(NskAppContext dbContext, BaseSessionInfo sessionInfo);

        /// <summary>
        /// ページデータを1行分追加し、ページ分取得する
        /// </summary>
        public void AddPageData();

        /// <summary>
        /// チェックされたデータを削除し、ページ分取得する
        /// </summary>
        public void DelPageData();

        /// <summary>
        /// 追加対象レコード取得
        /// </summary>
        /// <returns></returns>
        public List<T> GetAddRecs();

        /// <summary>
        /// 削除対象レコード取得
        /// </summary>
        /// <returns></returns>
        public List<T> GetDeleteRecs();

        /// <summary>
        /// 更新対象レコード取得
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="sessionInfo"></param>
        /// <returns></returns>
        public List<T> GetUpdateRecs(ref NskAppContext dbContext, BaseSessionInfo sessionInfo);
    }
}
