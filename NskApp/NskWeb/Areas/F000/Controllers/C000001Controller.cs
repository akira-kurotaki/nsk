using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace NskWeb.Areas.F000.Controllers
{
    [ExcludeAuthCheck]
    [ExcludeSystemLockCheck]
    [Area("F000")]
    public class C000001Controller : CoreController
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public C000001Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        /// <summary>
        /// 空白ページを返す
        /// PDFページを閉じるJavaScriptで、第一引数がサイトに対する相対パスでないと「アクセスが拒否されました」が出力される。
        /// JavaScript内のチェックなので実際にこのページが呼ばれることはない。
        ///   修正前）var win = window.open('', 'report');
        ///   修正後）var win = window.open('/SynWeb/F00/C0001', 'report');
        /// </summary>
        /// <returns></returns>
        // GET: F00/C0001
        public ActionResult Init()
        {
            return View("C0001");
        }
    }
}