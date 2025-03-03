using NskWeb.Common.Consts;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.HelpMenu;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.CodeAnalysis;

namespace NskWeb.Areas.F000.Controllers
{
    [AllowAnonymous]
    [ExcludeAuthCheck]
    [ExcludeSystemLockCheck]
    [Area("F000")]
    public class C000000Controller : CoreController
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public C000000Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        /// <summary>
        /// イベント：都道府県
        /// </summary>
        /// <param name="model">都道府県ドロップダウンリスト用画面モデル</param>
        /// <returns>ActionResult</returns>
        /// POST: F00/C0000/Todofuken
        [HttpPost]
        public ActionResult Todofuken([Bind("TodofukenCd"), FromBody] TodofukenDropDownList model)
        {
            // 組合等ドロップダウンリスト取得
            var kumiaitoSelectList = TodofukenDropDownListUtil.GetSelectList(TodofukenDropDownListUtil.KbnSbt.Kumiaito, model);

            return Json(kumiaitoSelectList);
        }

        /// <summary>
        /// イベント：組合等
        /// </summary>
        /// <param name="model">都道府県ドロップダウンリスト用画面モデル</param>
        /// <returns>ActionResult</returns>
        /// POST: F00/C0000/Kumiaito
        [HttpPost]
        public ActionResult Kumiaito([Bind("TodofukenCd,KumiaitoCd"), FromBody] TodofukenDropDownList model)
        {
            // 組合等サブドロップダウンリスト（支所、市町村、大地区）
            var kumiaitoSubSelectList = new KumiaitoSubSelectList();

            // セッションから利用可能支所一覧情報取得
            var shishoList = SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, HttpContext);
            // 利用可能支所一覧設定
            model.ShishoList = shishoList;
            // 支所ドロップダウンリスト取得
            kumiaitoSubSelectList.Shisho = TodofukenDropDownListUtil.GetSelectList(TodofukenDropDownListUtil.KbnSbt.Shisho, model);
            // 市町村ドロップダウンリスト取得
            kumiaitoSubSelectList.Shichoson = TodofukenDropDownListUtil.GetSelectList(TodofukenDropDownListUtil.KbnSbt.Shichoson, model);
            // 大地区ドロップダウンリスト取得
            kumiaitoSubSelectList.Daichiku = TodofukenDropDownListUtil.GetSelectList(TodofukenDropDownListUtil.KbnSbt.Daichiku, model);

            return Json(kumiaitoSubSelectList);
        }

        /// <summary>
        /// イベント：大地区
        /// </summary>
        /// <param name="model">都道府県ドロップダウンリスト用画面モデル</param>
        /// <returns>ActionResult</returns>
        /// POST: F00/C0000/Daichiku
        [HttpPost]
        public ActionResult Daichiku([Bind("TodofukenCd,KumiaitoCd,DaichikuCd"), FromBody] TodofukenDropDownList model)
        {
            // 小地区ドロップダウンリスト取得
            var shochikuSelectList = TodofukenDropDownListUtil.GetSelectList(TodofukenDropDownListUtil.KbnSbt.Shochiku, model);

            return Json(shochikuSelectList);
        }

        /// <summary>
        /// イベント：ログアウト
        /// </summary>
        /// <returns>ActionResult</returns>
        // GET: F00/C0000/Logout
        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            // セッション破棄
            //HttpContext.Session.Clear();
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //var sessionId = HttpContext.Request.Cookies[ConfigUtil.Get("HttpCookies_Session_Id")];
            //if (sessionId != null)
            //{
            //    var cOptions = new CookieOptions()
            //    {
            //        Expires = DateTime.Now.AddDays(-1d)
            //    };
            //    HttpContext.Response.Cookies.Append(ConfigUtil.Get("HttpCookies_Session_Id"), sessionId, cOptions);
            //    HttpContext.Response.Cookies.Delete(ConfigUtil.Get("HttpCookies_Session_Id"));
            //}

            return View("C000000");
        }

        /// <summary>
        /// イベント：メニューリンク
        /// </summary>
        /// <returns>ActionResult</returns>
        // GET: F00/C0000/Link
        public ActionResult Link()
        {
            string screenId = Request.Query[InfraConst.MENU_REQUEST_QUERYSTRING];
            if (string.IsNullOrEmpty(screenId))
            {
                throw new AppException("ME01056", MessageUtil.Get("ME01056"));
            }
            else
            {
                if (TempData.ContainsKey(CoreConst.IS_LEFT_SUB_MENU))
                {
                    TempData.Remove(CoreConst.IS_LEFT_SUB_MENU);
                }
                TempData.Add(CoreConst.IS_LEFT_SUB_MENU, true);
                if (TempData.ContainsKey(InfraConst.MENU_POST_OPEID))
                {
                    TempData.Remove(InfraConst.MENU_POST_OPEID);
                }
                // 処理IDを取得
                string OpeId = Request.Query[InfraConst.MENU_OPE_REQUEST_QUERYSTRING];
                if (!string.IsNullOrEmpty(OpeId))
                {
                    TempData.Add(InfraConst.MENU_POST_OPEID, OpeId);
                }
                string area = ScreenUtil.GetArea(screenId);
                if (string.IsNullOrEmpty(area))
                {
                    return BadRequest();
                }

                // 業務アプリ使用のセッションを削除
                SessionUtil.RemoveAll(HttpContext);

                return RedirectToAction("Init", screenId, new { area = area });
            }
        }

        /// <summary>
        /// イベント：ヘルプメニューリンク
        /// </summary>
        /// <param name="hh"></param>
        /// <returns>File</returns>
        // GET: F00/C0000/HelpLink
        public ActionResult HelpLink(string hh)
        {
            if (String.IsNullOrEmpty(hh))
            {
                return BadRequest();
            }

            // ヘルプの特定
            var helpMenuDto = HelpMenuUtil.GetHelpMenuList().Where(h => h.Hash == hh).Single();

            // ファイル読み込み
            byte[] helpFile = null;
            using (var fileStream = new FileStream(helpMenuDto.HelpFilePath, FileMode.Open, FileAccess.Read))
            {
                helpFile = new byte[fileStream.Length];
                fileStream.Read(helpFile, 0, helpFile.Length);
            }

            return File(helpFile, GetMimeTypeForFileExtension(helpMenuDto.HelpFilePath));
        }
    }
}