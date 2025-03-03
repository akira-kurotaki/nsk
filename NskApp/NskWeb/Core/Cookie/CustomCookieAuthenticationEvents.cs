using NskWeb.Common.Consts;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Extensions;
using System.Web;

namespace NskWeb.Core.Cookie
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        /// <summary>
        /// 認証エラー時にログインURLにリダイレクトする
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            var url = ConfigUtil.Get(InfraConst.LOGIN_URL);
            url += "?ReturnUrl=";
            url += HttpUtility.UrlEncode(context.Request.GetEncodedUrl());

            context.RedirectUri = url;
            return base.RedirectToLogin(context);
        }
    }
}
