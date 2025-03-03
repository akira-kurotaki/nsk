using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authentication;
using NLog;
using System.Security.Claims;

namespace NskWeb.Core.Middleware
{
    public class CustomMiddlewareAuthenticateRequest
    {
        protected Logger logger = LogManager.GetCurrentClassLogger();

        private readonly RequestDelegate _next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public CustomMiddlewareAuthenticateRequest(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
#if false
            // 以下調査用ログ出力
            // 認証情報の結果を取り出す
            var authResult = await context.AuthenticateAsync();
            logger.Info("authResult:" + authResult);

            // 認証チケットから認証情報を取り出す
            var principal = authResult.Principal;
            logger.Info("principal:" + principal);

            // 認証情報
            logger.Info("name:" + principal?.FindFirst(ClaimTypes.Name)?.Value);
            logger.Info("NameIdentifier:" + principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            logger.Info("TodofukenCd:" + principal?.FindFirst("TodofukenCd")?.Value);

            // HttpContext.Userプロパティから認証情報を取り出す
            logger.Info("HttpContext.User(Name):" + context.User?.FindFirst(ClaimTypes.Name)?.Value);
            logger.Info("HttpContext.User(NameIdentifier):" + context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            logger.Info("HttpContext.User(TodofukenCd):" + context.User?.FindFirst("TodofukenCd")?.Value);

            logger.Info("Identity.IsAuthenticated:" + context.User?.Identity.IsAuthenticated);
#endif
            BeginInvoke(context);
            await _next.Invoke(context);
            EndInvoke(context);
        }

        /// <summary>
        /// Global.asax.csのApplication_AuthenticateRequest相当の想定
        /// </summary>
        /// <param name="context"></param>
        private void BeginInvoke(HttpContext context)
        {
            // リクエスト時のセッションIDを退避
            if (context.Request.Cookies[ConfigUtil.Get("HttpCookies_Session_Id")] != null)
            {
                context.Request.Headers.Append(CoreConst.X_REQ_SESSION_ID, context.Request.Cookies[ConfigUtil.Get("HttpCookies_Session_Id")]);
            }
            else
            {
                context.Request.Headers.Append(CoreConst.X_REQ_SESSION_ID, string.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void EndInvoke(HttpContext context)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class CustomMiddlewareAuthenticateRequestExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomMiddlewareAuthenticateRequest(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddlewareAuthenticateRequest>();
        }
    }
}
