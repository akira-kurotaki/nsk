using NLog;

namespace NskWeb.Core.Middleware
{
    public class CustomMiddlewareRequest
    {
        protected Logger logger = LogManager.GetCurrentClassLogger();

        private readonly RequestDelegate _next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public CustomMiddlewareRequest(RequestDelegate next)
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
            BeginInvoke(context);
            await _next.Invoke(context);
            EndInvoke(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private void BeginInvoke(HttpContext context)
        {
            // 接続元IPアドレス
            var hostAddress = context.Connection.RemoteIpAddress;
            var xForwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            context.Items["clientip"] = string.Format("{0} {1}", hostAddress, string.IsNullOrEmpty(xForwardedFor) ? "\"\"" : xForwardedFor.Split(',')[0].Trim());
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
    public static class CustomMiddlewareRequestExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomMiddlewareRequest(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddlewareRequest>();
        }
    }
}