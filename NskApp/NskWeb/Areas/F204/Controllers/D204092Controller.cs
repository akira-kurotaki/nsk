using NskAppModelLibrary.Context;
using NskWeb.Areas.F204.Models.D204092;
using NskWeb.Common.Consts;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Pager;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using ModelLibrary.Models;
using Npgsql;
using NpgsqlTypes;
using ReportService.Core;
using System.Text;
using System.Text.RegularExpressions;
using NskWeb.Areas.F000.Models.D000000;

namespace NskWeb.Areas.F204.Controllers
{
    /// <summary>
    /// 一括帳票出力
    /// </summary>
    [Authorize(Roles = "bas")]
    [SessionOutCheck]
    [Area("F204")]
    public class D204092Controller : CoreController
    {
        #region メンバー定数
        /// <summary>
        /// 画面ID(D204092)
        /// </summary>
        private static readonly string SCREEN_ID_D204092 = "D204092";

        /// <summary>
        /// セッションキー(D204092)
        /// </summary>
        private static readonly string SESS_D204092 = SCREEN_ID_D204092 + "_" + "SCREEN";


        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        /// <param name="serviceClient"></param>
        public D204092Controller(ICompositeViewEngine viewEngine, ReportServiceClient serviceClient) : base(viewEngine, serviceClient)
        {
        }
        #endregion

        #region 初期表示イベント
        /// <summary>
        /// イベント：初期化
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Init()
        {
            NSKPortalInfoModel model_portal = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            if (model_portal == null)
            {
                throw new AppException("ME00068", MessageUtil.Get("ME00068"));
            }
            D204092Model model = SessionUtil.Get<D204092Model>(SESS_D204092, HttpContext);
            if (model == null){
                model = new D204092Model();
                // 自画面のセッション情報が無いとき、postされたデータから処理IDを取得する
                model.OpeId = (string)TempData[InfraConst.MENU_POST_OPEID];
                SessionUtil.Set(SESS_D204092, model, HttpContext);
            }

            return View(SCREEN_ID_D204092, model);
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
            SessionUtil.Remove(SESS_D204092, HttpContext);

            return Json(new { result = "success" });
        }
        #endregion

    }
}
