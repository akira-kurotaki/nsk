using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Filters;
using CoreLibrary.Core.Session;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace NskWeb.Core.Filter
{
    /// <summary>
    /// SysSelectのリクエストの有効性を検証するフィルター
    /// </summary>
    /// <remarks>
    /// 作成日：2018/04/04
    /// 作成者：MATSUBAYSHI Atsushi
    /// </remarks>
    public class ValidRequestNskWebAttribute : ActionFilterAttribute
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 業務のメソッドが呼ばれる前の処理
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            logger.Debug("---");
            logger.Debug("IsNewSession:" + filterContext.HttpContext.HasNewSession());
            logger.Debug("SessionId:" + filterContext.HttpContext.Session.Id);
            filterContext.HttpContext.Request.Headers.TryGetValue(CoreConst.X_REQ_SESSION_ID, out var xReqSessionId);
            logger.Debug("XReqSessionId:" + xReqSessionId);
            logger.Debug("RawUrl:" + UriHelper.GetEncodedUrl(filterContext.HttpContext.Request));

            if (filterContext.HttpContext.HasNewSession())
            {
                // セッションのチェック
                if (ConfigUtil.Get(CoreConst.APP_ENV) != "debug")
                {
                    // 下記の場合、セッション切れ画面へ遷移
                    // ・クライアントからセッションIDが送信されている。
                    if (!string.IsNullOrEmpty(xReqSessionId))
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary
                            {
                                { "controller", "D900001" },
                                { "action", "Init" },
                                { "area", "F900" }
                            }
                        );
                        return;
                    }
                }

                // セッションチェックから除外する画面がある場合は、チェック処理を通らないようにする
            }

            // 権限チェック
            FilterUtil.CheckAuth(filterContext);
        }
    }
}
