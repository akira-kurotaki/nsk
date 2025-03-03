using BaseDmpApi.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Utility;
using NLog;

namespace BaseDmpApi.Middleware
{
    /// <summary>
    /// エラーハンドリングミドルウエア
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private Logger logger = LogManager.GetCurrentClassLogger();

        private readonly RequestDelegate _next;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="next"></param>
        public ErrorHandlerMiddleware(RequestDelegate next)
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
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // エラー情報をログに出力する
                logger.Error(MessageUtil.GetErrorMessage(ex, CoreConst.LOG_MAX_INNER_EXCEPTION));

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                // FATALメッセージをappsettings.jsonから取得し、ログ出力
                var fatalMessage = ConfigUtil.Get("MF00001");
                logger.Error(fatalMessage);

                ApiErrorResponseBase response = new ApiErrorResponseBase
                {
                    messages = new List<Message> { new Message { message = fatalMessage } }
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

    /// <summary>
    /// ErrorHandlerMiddlewareを使用するための拡張クラス
    /// </summary>
    public static class ErrorHandlerMiddlewareExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseErrorHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
