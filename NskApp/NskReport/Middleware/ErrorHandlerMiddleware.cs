using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using NLog;
using NskReportLibrary.Core.Consts;
using ReportService.Core;

namespace NskReport.Middleware
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
                // エラーログ出力
                logger.Error(MessageUtil.GetErrorMessage(ex, CoreConst.LOG_MAX_INNER_EXCEPTION));
                var message = ex.Message + string.Join(string.Empty, new string[]{
                    ex.InnerException == null ? string.Empty : ReportConst.NEW_LINE_SEPARATOR + ex.InnerException.ToString(),
                    string.IsNullOrEmpty(ex.StackTrace) ? string.Empty : ReportConst.NEW_LINE_SEPARATOR + ex.StackTrace
                });

                // 戻り値を設定する
                ReportResult result = new ReportResult();
                result.Result = ReportConst.RESULT_FAILED;
                if (ex is AppException)
                {
                    result.ErrorMessageId = ((AppException)ex).ErrorCode;
                    result.ErrorMessage = ex.Message;
                }
                else
                {
                    result.ErrorMessageId = "MF00001";
                    // DBに繋がらない場合もあるので固定値にする
                    string msg = "予期せぬエラーが発生しました。システム管理者に連絡してください。(MF00001)";
                    result.ErrorMessage = msg;
                    message = msg + ReportConst.NEW_LINE_SEPARATOR + message;
                }

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(result);
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
