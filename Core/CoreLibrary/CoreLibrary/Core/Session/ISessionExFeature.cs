namespace CoreLibrary.Core.Session
{
    /// <summary>
    /// IsNewSession代替機能
    /// </summary>
    public interface ISessionExFeature
    {
        /// <summary>
        /// 新しいセッションを確立する場合：true、セッションを再開する場合：false
        /// </summary>
        bool IsNewSession { get; }
    }
}
