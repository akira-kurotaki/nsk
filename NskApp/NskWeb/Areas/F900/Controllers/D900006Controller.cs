using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace SynMain.Areas.F900.Controllers
{
    /// <summary>
    /// 閉じる
    /// </summary>
    /// <remarks>
    /// 作成日：2020/02/10
    /// 作成者：Nakamura Koichi
    /// </remarks>
    [ExcludeAuthCheck]
    [AllowAnonymous]
    [ExcludeSystemLockCheck]
    [Area("F900")]
    public class D900006Controller : CoreController
    {
        public D900006Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        // GET: F900/D900006/Init
        public ActionResult Init()
        {
            return View("D900006");
        }
    }
}