using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace SynMain.Areas.F900.Controllers
{
    /// <summary>
    /// セッションタイムアウト
    /// </summary>
    /// <remarks>
    /// 作成日：2018/01/15
    /// 作成者：MATSUBAYASHI Atsushi
    /// </remarks>
    [ExcludeAuthCheck]
    [AllowAnonymous]
    [ExcludeSystemLockCheck]
    [Area("F900")]
    public class D900001Controller : CoreController
    {
        public D900001Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        // GET: F900/D900001/Init
        public ActionResult Init()
        {
            HttpContext.Session.Clear();

            var sessionId = Request.Cookies[ConfigUtil.Get("HttpCookies_Session_Id")];
            if (sessionId != null)
            {
                var cOptions = new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(-1d)
                };
                HttpContext.Response.Cookies.Append(ConfigUtil.Get("HttpCookies_Session_Id"), sessionId, cOptions);
            }
            return View("D900001");
        }
    }
}