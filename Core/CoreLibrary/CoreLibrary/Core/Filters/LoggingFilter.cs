using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System.Diagnostics;

namespace CoreLibrary.Core.Filters
{
    /// <summary>
    /// ロギング用フィルター
    /// </summary>
    public class LoggingFilter : ActionFilterAttribute
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// アクションメソッド実行前に呼び出し
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (null == context)
            {
                throw new ArgumentNullException("filterContext");
            }

            // ログインユーザ
            var syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, context.HttpContext) ?? new Syokuin();
            //var nogyoshaId = SessionUtil.Get<int?>(CoreConst.SESS_NOGYOSHA_ID, context.HttpContext);
            //var loginuser
            //    = bool.Parse(ConfigUtil.Get(CoreConst.SHINSEI_FLAG))
            //    ? (nogyoshaId is null ? null : "農" + nogyoshaId)
            //    : syokuin.UserId;
            //context.HttpContext.Items["loginuser"] = loginuser;
            context.HttpContext.Items["loginuser"] = syokuin.UserId;

            // メソッド情報
            var methodName = context.Controller.GetType().Name + "." + ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            var parameters = "";
            int i = 0;
            foreach (var param in context.ActionArguments)
            {
                if (0 < i++)
                {
                    parameters += " , ";
                }
                var paramValue = null == param.Value ? "null" : param.Value.ToString();
                parameters += param.Key + " = " + paramValue;
            }
            context.HttpContext.Items["methodInfo"] = methodName + "(" + parameters + ")";

            // ログ出力
            var message = string.Format("{0} {1}", CoreConst.LOG_START_KEYWORD, context.HttpContext.Items["methodInfo"]);
            logger.Info(message);

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            context.HttpContext.Items["timer"] = stopwatch;
        }

        /// <summary>
        /// ActionResult実行後に呼び出し
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (null == context)
            {
                throw new ArgumentNullException("filterContext");
            }

            // 処理時間
            var stopwatch = context.HttpContext.Items["timer"] as Stopwatch;

            if (stopwatch != null)
            {
                stopwatch.Stop();
                var timer = CoreConst.LOG_TIMER_START_MESSAGE + " " + stopwatch.ElapsedMilliseconds.ToString() + " " + CoreConst.LOG_TIMER_END_MESSAGE;

                // ログ出力
                var message = string.Format("{0} {1} : {2}", CoreConst.LOG_END_KEYWORD, context.HttpContext.Items["methodInfo"], timer);
                logger.Info(message);
            }
        }
    }
}
