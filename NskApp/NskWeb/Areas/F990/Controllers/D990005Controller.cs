using NskWeb.Areas.F990.Models.D990005;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace NskWeb.Areas.F990.Controllers
{
    [Authorize(Roles = "bas")]
    [Area("F990")]
    [ExcludeSystemLockCheck]
    public class D990005Controller : CoreController
    {
        public D990005Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        // GET: F00/CheckInput
        public ActionResult Init()
        {
            return View("D990005");
        }

        // POST: F00/CheckInput/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Numeric,NumberDec,NumberSign,NumberSignDec,HalfWidthAlphaNum,HalfWidthAlphaNumSymbol,Hiragana,FullWidthKatakana,HalfWidthKatakana,ExceptGaiji,WithinStringLength,WithinNumericLength,FullStringLength,FullNumericLength,NumberPositive,EmailAddress,DateGYMD")] D990005Model checkInputModel)
        {
            if (ModelState.IsValid)
            {

                ViewBag.Message = "CheckInput is OK!";
                return View("D990005", checkInputModel);
            }

            return View("D990005", checkInputModel);
        }
    }
}
