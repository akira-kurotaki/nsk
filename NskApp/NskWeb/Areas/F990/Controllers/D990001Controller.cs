using NskWeb.Areas.F990.Models.D990001;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NskWeb.Areas.F990.Controllers
{
    /// <summary>
    /// 加入者一覧
    /// </summary>
    [Authorize(Roles = "bas")]
    [Area("F990")]
    public class D990001Controller : CoreController
    {
        #region メンバー定数
        /// <summary>
        /// 画面ID(D9901)
        /// </summary>
        private static readonly string SCREEN_ID_D990001 = "D990001";

        /// <summary>
        /// 画面ID(D9901)
        /// </summary>
        private static readonly string SCREEN_ID_D990001_PARTERN1 = "D990001Partern1";

        /// <summary>
        /// 画面ID(D9901)
        /// </summary>
        private static readonly string SCREEN_ID_D990001_PARTERN2 = "D990001Partern2";

        /// <summary>
        /// セッションキー(D9901)
        /// </summary>
        private static readonly string SESS_D990001 = SCREEN_ID_D990001 + "_" + "SCREEN";
        #endregion

        #region 初期表示イベント
        /// <summary>
        /// イベント：初期化
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Init()
        {
            // 利用可能な支所一覧
            var shishoList = SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, HttpContext);

            var model = new D990001Model(Syokuin, shishoList);
            // モデル状態ディクショナリからすべての項目を削除します。       
            ModelState.Clear();

            // 検索条件をセッションに保存する
            SessionUtil.Set(SESS_D990001, model, HttpContext);

            // パンくずリストを変更する
            SessionUtil.Set(CoreConst.SESS_BREADCRUMB, new List<string>() { "D000000" }, HttpContext);

            // 画面を表示する
            return View(SCREEN_ID_D990001_PARTERN1, model);
        }
        #endregion

        #region 初期表示イベント
        /// <summary>
        /// イベント：初期化
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult InitPartarn2()
        {
            // 利用可能な支所一覧
            var shishoList = SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, HttpContext);

            var model = new D990001Model(Syokuin, shishoList);
            // モデル状態ディクショナリからすべての項目を削除します。       
            ModelState.Clear();

            // 検索条件をセッションに保存する
            SessionUtil.Set(SESS_D990001, model, HttpContext);

            // パンくずリストを変更する
            SessionUtil.Set(CoreConst.SESS_BREADCRUMB, new List<string>() { "D000000" }, HttpContext);

            // 画面を表示する
            return View(SCREEN_ID_D990001_PARTERN2, model);
        }
        #endregion

        #region 戻るイベント
        /// <summary>
        /// イベント名：戻る 
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Back()
        {
            // セッション情報から検索条件、検索結果件数をクリアする
            SessionUtil.Remove(SESS_D990001, HttpContext);

            return Json(new { result = "success" });
        }
        #endregion
    }
}
