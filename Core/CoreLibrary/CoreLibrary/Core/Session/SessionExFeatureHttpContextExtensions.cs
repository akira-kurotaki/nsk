namespace CoreLibrary.Core.Session
{
    /// <summary>
    /// IsNewSession代替機能
    /// </summary>
    public static class SessionExFeatureHttpContextExtensions
    {
        /// <summary>
        /// IsNewSession代替機能
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>新しいセッションを確立する場合：true、セッションを再開する場合：false</returns>
        public static bool HasNewSession(this HttpContext context)
        {
            return context.Features.Get<ISessionExFeature>()?.IsNewSession ?? false;
        }
    }
}
