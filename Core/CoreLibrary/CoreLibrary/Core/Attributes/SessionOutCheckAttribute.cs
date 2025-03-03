using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreLibrary.Core.Attributes
{
    /// <summary>
    /// セッション有効チェックカスタムアトリビュート
    /// </summary>
    public class SessionOutCheckAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 初回アクセス時以外、かつ、セッションからユーザ情報が取得できない場合は、セッションタイムアウト画面に遷移する。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.HasNewSession())
            {
                byte[] value;
                if (!filterContext.HttpContext.Session.TryGetValue(CoreConst.SESS_LOGIN_USER, out value))
                {
                    // セッションタイムアウト画面にリダイレクト
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            { "controller", "D9001" },
                            { "action", "Init" },
                            { "area", "F90" }
                        }
                    );
                    return;
                }
            }
        }
    }
}
