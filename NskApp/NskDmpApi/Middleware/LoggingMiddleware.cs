using CoreLibrary.Core.Consts;
using NLog;
using System.Diagnostics;

namespace BaseDmpApi.Middleware
{
    /// <summary>
    /// ロギングミドルウエア
    /// </summary>
    public class LoggingMiddleware
    {
        protected Logger logger = LogManager.GetCurrentClassLogger();

        private readonly RequestDelegate _next;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="next"></param>
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// InvokeAsync
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // 処理開始
            var stopwatch = Stopwatch.StartNew();

            // 接続元IPアドレス
            var hostAddress = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var xForwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            context.Items["clientip"] = string.Format("{0} {1}", hostAddress, string.IsNullOrEmpty(xForwardedFor) ? "\"\"" : xForwardedFor.Split(',')[0].Trim());

            // ログ出力
            logger.Info(string.Format("{0} {1}", CoreConst.LOG_START_KEYWORD, context.GetEndpoint()?.DisplayName));

            await this._next.Invoke(context);

            // 処理時間
            stopwatch.Stop();
            var timer = CoreConst.LOG_TIMER_START_MESSAGE + " " + stopwatch.ElapsedMilliseconds.ToString() + " " + CoreConst.LOG_TIMER_END_MESSAGE;

            // ログ出力
            logger.Info(string.Format("{0} {1} : {2}", CoreConst.LOG_END_KEYWORD, context.GetEndpoint()?.DisplayName, timer));
        }
    }

    /// <summary>
    /// LoggingMiddlewareを使用するため拡張クラス
    /// </summary>
    public static class LoggingMiddlewareExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}
