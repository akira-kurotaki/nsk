namespace CoreLibrary.Core.Session
{
    /// <summary>
    /// IsNewSession代替機能
    /// </summary>
    public class SessionExFeature : ISessionExFeature
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="isNewSession">新しいセッションを確立する場合：true、セッションを再開する場合：false</param>
        public SessionExFeature(bool isNewSession)
        {
            IsNewSession = isNewSession;
        }

        /// <summary>
        /// 新しいセッションを確立する場合：true、セッションを再開する場合：false
        /// </summary>
        public bool IsNewSession { get; }
    }
}
