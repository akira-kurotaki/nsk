using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace CoreLibrary.Core.Filters
{
    /// <summary>
    /// 例外フィルター
    /// </summary>
    public class CoreExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 例外発生時に呼び出し
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            // ログ出力
            logger.Error(SystemMessageUtil.GetErrorMessage(context.Exception, CoreConst.LOG_MAX_INNER_EXCEPTION));

            // TempDataのストア（格納先）がセッションの場合、リダイレクトが上手くいかない。
            // TempDataではなく、直接セッションに格納する。
            SessionUtil.Set(CoreConst.SESS_COMMON_ERROR_TIME, DateUtil.GetSysDateTime(), context.HttpContext);
            SessionUtil.Set(CoreConst.SESS_COMMON_EXCEPTION, context.Exception, context.HttpContext);

            // 業務エラー/システムエラーにリダイレクト
            context.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "D9002" },
                    { "action", "Init" },
                    { "area", "F90" }
                });
            context.ExceptionHandled = true;
        }
    }
}
