using NskAppModelLibrary.Context;
using NskWeb.Areas.F000.Consts;
using NskWeb.Areas.F000.Models.D000000;
using NskWeb.Common.Consts;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.HelpMenu;
//using CoreLibrary.Core.Menu;
using NskWeb.Common.Menu;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Text;
using System.Configuration;
using CoreLibrary.Core.Dto;
using System.ComponentModel.DataAnnotations;
using CoreLibrary.Core.Validator;

namespace NskWeb.Areas.F000.Controllers
{
    /// <summary>
    /// ポータル
    /// </summary>
    /// <remarks>
    /// 作成日：2018/03/07
    /// 作成者：Gon Etuun
    /// </remarks>
    [SessionOutCheck]
    [ExcludeSystemLockCheck]
    [Area("F000")]
    public class D000000Controller : CoreController
    {
        #region メンバー定数
        /// <summary>
        /// 画面ID(D000000)
        /// </summary>
        private static readonly string SCREEN_ID_D000000 = "D000000";

        /// <summary>
        /// セッションキー(D0001)
        /// </summary>
        private static readonly string SESS_D000000 = "D000000_SCREEN";

        /// <summary>
        /// 180日(判断用)
        /// </summary>
        private static readonly int DAYS_180 = 180;

        /// <summary>
        /// システム選択画面ID(D8031)
        /// </summary>
        private static readonly string SCREEN_ID_D8031 = "D8031";

        public D000000Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }
        #endregion

        #region 初期表示イベント
        /// <summary>
        /// イベント：初期表示
        /// 画面を初期表示する
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Init()
        {
            // 画面表示モードを設定
            //SetScreenModeFromQueryString();
            //if (ScreenMode == CoreConst.ScreenMode.None)
            //{
            //    throw new AppException("MF00004", MessageUtil.Get("MF00004"));
            //}

            // 遷移元引数screenを取得する
            var pagefrom = HttpContext.Request.Query[CoreConst.SCREEN_PAGE_FROM];
            if (!string.IsNullOrEmpty(pagefrom) &&
                !Regex.IsMatch(pagefrom, CoreConst.SCREEN_PAGE_FROM_REGEX_MATCH))
            {
                throw new SystemException(MessageUtil.Get("MF00005", "遷移URLが不正です"));
            }

            // パンくずリストを修正する
            SessionUtil.Set(CoreConst.SESS_BREADCRUMB, new List<string>(), HttpContext);

            // 業務アプリ使用のセッションを削除
            SessionUtil.RemoveAll(HttpContext);

            // ヘルプメニューを設定
            if (SessionUtil.Get<IEnumerable<HelpMenuItem>>(CoreConst.SESS_HELP_MENU_LIST, HttpContext) == null)
            {
                SessionUtil.Set(CoreConst.SESS_HELP_MENU_LIST, HelpMenuUtil.GetHelpMenuItems(HttpContext), HttpContext);
            }
            // Baseアプリでは以下を使用してメニュー、ヘルプメニューを設定しているが、NSKのメニューは別の作りなので使用しない
            //SetMenuAndHelpMenu();

            // モデル初期化
            D000000Model model = new ()
            {
                ShishoCd = Syokuin.ShishoCd,
                ShishoGroupId = Syokuin.ShishoGroupId,
                ShishoList = SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, HttpContext)
            };

            // $$$$$$$$$$$$$$$$$$
            NSKPortalInfoModel md = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            if (md != null)
            {
                model.D000000Info.SKyosaiMokutekiCd = md.SKyosaiMokutekiCd;
                model.D000000Info.SNensanHikiuke = md.SNensanHikiuke;
                model.D000000Info.SNensanHyoka = md.SNensanHyoka;
                model.D000000Info.SHikiukeJikkoTanniKbnHikiuke = md.SHikiukeJikkoTanniKbnHikiuke;
                model.D000000Info.SHikiukeJikkoTanniKbnHyoka = md.SHikiukeJikkoTanniKbnHyoka;

                model.D000000Info2.SKyosaiMokutekiCd = md.SKyosaiMokutekiCd;
                model.D000000Info2.SNensanHikiuke = md.SNensanHikiuke;
                model.D000000Info2.SNensanHyoka = md.SNensanHyoka;
            }
            // $$$$$$$$$$$$$$$$$$

            // 初期表示情報をセッションに保存する
            SessionUtil.Set(SESS_D000000, model, HttpContext);

            // D0001 ポータルを表示する。
            return View(SCREEN_ID_D000000, model);
        }
        #endregion

        #region 決定イベント
        /// <summary>
        /// イベント名：決定
        /// </summary>
        /// <param name="model">加入者一覧モデル</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Decision([Bind("D000000Info")] D000000Model model)
        {
            string SKyosaiMokutekiCdOld = "";
            string SNensanHikiukeOld = "";
            string SNensanHyokaOld = "";

            Type tp = typeof(NSKPortalInfoModel);

            FullDigitLengthAttribute lenAttr = (FullDigitLengthAttribute)Attribute.GetCustomAttribute(tp.GetProperty("SKyosaiMokutekiCd"), typeof(FullDigitLengthAttribute));

            if (string.IsNullOrEmpty(model.D000000Info.SKyosaiMokutekiCd) || string.IsNullOrEmpty(model.D000000Info.SNensanHikiuke) || string.IsNullOrEmpty(model.D000000Info.SNensanHyoka)){
                    // ひとつでも未入力のときは何もせずに終了
                return View(SCREEN_ID_D000000, model);
            }

            // セッションからNSKポータル情報を取得
            NSKPortalInfoModel m = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            if (m != null)
            {
                // 以前の状態を保存
                SKyosaiMokutekiCdOld = m.SKyosaiMokutekiCd;
                SNensanHikiukeOld = m.SNensanHikiuke;
                SNensanHyokaOld = m.SNensanHyoka;
                // 画面入力の無い項目をセッションから復元
                model.D000000Info.SHikiukeJikkoTanniKbnHikiuke = m.SHikiukeJikkoTanniKbnHikiuke;
                model.D000000Info.SHikiukeJikkoTanniKbnHyoka = m.SHikiukeJikkoTanniKbnHyoka;

                model.D000000Info2.SKyosaiMokutekiCd = m.SKyosaiMokutekiCd;
                model.D000000Info2.SNensanHikiuke = m.SNensanHikiuke;
                model.D000000Info2.SNensanHyoka = m.SNensanHyoka;
            }
            model.ShishoCd = Syokuin.ShishoCd;
            model.ShishoGroupId = Syokuin.ShishoGroupId;
            model.ShishoList = SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, HttpContext);

            if (!SKyosaiMokutekiCdOld.Equals(model.D000000Info.SKyosaiMokutekiCd) || !SNensanHikiukeOld.Equals(model.D000000Info.SNensanHikiuke) || !SNensanHyokaOld.Equals(model.D000000Info.SNensanHyoka))
            {
                // セッションからNSKポータル情報が取得できなかった時、または共済目的、引受年産、評価年ののいずれかが入力値と異なっていた場合は、引受計算支所実行単位区分を取得する
                // 組合等コードを職員情報から取得
                String KumiaitoCd = Syokuin.KumiaitoCd;

                var SHikiukeJikkoTanniKbnHikiuke = getJigyoDb<NskAppContext>().M10010組合等設定s.Where(t => t.組合等コード == KumiaitoCd && t.年産 == int.Parse(model.D000000Info.SNensanHikiuke) && t.共済目的コード == model.D000000Info.SKyosaiMokutekiCd).SingleOrDefault();
                var SHikiukeJikkoTanniKbnHyoka = getJigyoDb<NskAppContext>().M10010組合等設定s.Where(t => t.組合等コード == KumiaitoCd && t.年産 == int.Parse(model.D000000Info.SNensanHyoka) && t.共済目的コード == model.D000000Info.SKyosaiMokutekiCd).SingleOrDefault();

                // $$$$$$$$$$$$ デバッグ用 データが取得できなかった場合は1にする
                model.D000000Info.SHikiukeJikkoTanniKbnHikiuke = "1";
                model.D000000Info.SHikiukeJikkoTanniKbnHyoka = "1";
                // $$$$$$$$$$$$ デバッグ用 データが取得できなかった場合は1にする

                if (SHikiukeJikkoTanniKbnHikiuke != null)
                {
                    model.D000000Info.SHikiukeJikkoTanniKbnHikiuke = SHikiukeJikkoTanniKbnHikiuke.引受計算支所実行単位区分.ToString();
                }

                if (SHikiukeJikkoTanniKbnHyoka != null)
                {
                    model.D000000Info.SHikiukeJikkoTanniKbnHyoka = SHikiukeJikkoTanniKbnHyoka.引受計算支所実行単位区分.ToString();
                }

                // 共済目的が変更された場合、またはメニュー情報がセッションに存在しない場合
                if (!SKyosaiMokutekiCdOld.Equals(model.D000000Info.SKyosaiMokutekiCd) || SessionUtil.Get<IEnumerable<NskMenuItem>>(CoreConst.SESS_MENU_LIST, HttpContext) == null)
                {
                    DbContext dbCom = getJigyoCommonDb();
                    SessionUtil.Set(CoreConst.SESS_MENU_LIST, NskMenuUtil.GetLeftMenuItems(HttpContext, dbCom, model.D000000Info.SKyosaiMokutekiCd), HttpContext);
                }
                // NSKポータル情報をセッション保存
                SessionUtil.Set(AppConst.SESS_NSK_PORTAL, model.D000000Info, HttpContext);

                // NSKポータル情報が存在している時は、欄外表示のために値をセット（デバッグ用）
                model.D000000Info2.SKyosaiMokutekiCd = model.D000000Info.SKyosaiMokutekiCd;
                model.D000000Info2.SNensanHikiuke = model.D000000Info.SNensanHikiuke;
                model.D000000Info2.SNensanHyoka = model.D000000Info.SNensanHyoka;
            }

            return View(SCREEN_ID_D000000, model);
        }
        #endregion
    }

}