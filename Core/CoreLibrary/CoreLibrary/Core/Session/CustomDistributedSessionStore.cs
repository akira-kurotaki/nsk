using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.Distributed;

namespace CoreLibrary.Core.Session
{
    /// <summary>
    /// IsNewSession代替機能を持ったセッションを作成するセッションストレージ
    /// </summary>
    public class CustomDistributedSessionStore : DistributedSessionStore, ISessionStore
    {
        readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="httpContextAccessor"></param>
        public CustomDistributedSessionStore(IDistributedCache cache,
            ILoggerFactory loggerFactory,
            IHttpContextAccessor httpContextAccessor) : base(cache, loggerFactory)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 新しいセッションを作成するか、再開する。
        /// </summary>
        /// <param name="sessionKey">セッションキー</param>
        /// <param name="idleTimeout">セッションの有効期限</param>
        /// <param name="ioTimeout">許容される最大時間</param>
        /// <param name="tryEstablishSession">セッションの変更が現在有効か確認するコールバック</param>
        /// <param name="isNewSessionKey">新しいセッションを確立する場合：true、セッションを再開する場合：false</param>
        /// <returns></returns>
        ISession ISessionStore.Create(string sessionKey, TimeSpan idleTimeout, TimeSpan ioTimeout, Func<bool> tryEstablishSession, bool isNewSessionKey)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var sessionExFeature = new SessionExFeature(isNewSessionKey);
                httpContext.Features.Set<ISessionExFeature>(sessionExFeature);
            }
            return Create(sessionKey, idleTimeout, ioTimeout, tryEstablishSession, isNewSessionKey);
        }
    }
}
