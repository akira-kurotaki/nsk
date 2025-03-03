using CoreLibrary.Core.Filters;
using CoreLibrary.Core.Session;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NskWeb.Core.Filter
{
    /// <summary>
    /// ログインユーザをセッションに設定するフィルター
    /// </summary>
    /// <remarks>
    /// 作成日：2018/07/20
    /// 作成者：MATSUBAYSHI Atsushi
    /// </remarks>
    public class LoginUserSettingNskWebAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 業務のメソッドが呼ばれる前の処理
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.HttpContext.HasNewSession())
            {
                // セッションにユーザ情報を設定
                FilterUtil.SetLoginUserToSession(filterContext);
            }
        }
    }
}