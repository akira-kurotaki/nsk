using Microsoft.AspNetCore.Mvc;

namespace NskReportDriver.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
