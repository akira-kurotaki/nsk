using NskWeb.Areas.F990.Models.D990002;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NskWeb.Areas.F990.Controllers
{
    /// <summary>
    /// システムロック
    /// </summary>
    [Authorize(Roles = "bas")]
    [Area("F990")]
    [ExcludeSystemLockCheck]
    public class D990002Controller : CoreController
    {
        #region メンバー定数
        /// <summary>
        /// 画面ID(D990002)
        /// </summary>
        private static readonly string SCREEN_ID_D990002 = "D990002";

        /// <summary>
        /// セッションキー(D990002)
        /// </summary>
        private static readonly string SESS_D990002 = SCREEN_ID_D990002 + "_" + "SCREEN";

        /// <summary>
        /// セッションキー(PAGE_TOKEN）
        /// </summary>
        public static readonly string SESS_D990002_PAGE_TOKEN = SCREEN_ID_D990002 + "_" + "PAGE_TOKEN";

        #endregion

        #region 初期表示イベント
        /// <summary>
        /// イベント：初期化
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Init()
        {
            var model = InitializeModel();

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // PAGE_TOKEN
            // 初期表示時、生成しセッション及びHIDDENに格納
            model.PageToken = Guid.NewGuid().ToString();
            SessionUtil.Set(SESS_D990002_PAGE_TOKEN, model.PageToken, HttpContext);
            logger.Info("D0102_PAGE_TOKEN設定：" + model.PageToken);

            // モデルをセッションに保存する
            SessionUtil.Set(SESS_D990002, model, HttpContext);

            // 画面を表示する
            return View(SCREEN_ID_D990002, Init(model));
        }
        #endregion

        private D990002Model InitializeModel()
        {
            // 利用可能な支所一覧
            var shishoList = SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, HttpContext);

            var model = new D990002Model(Syokuin, shishoList);

            return model;
        }

        #region 戻るイベント
        /// <summary>
        /// イベント名：戻る 
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Back()
        {
            // セッション情報から検索条件、検索結果件数をクリアする
            SessionUtil.Remove(SESS_D990002, HttpContext);

            return Json(new { result = "success" });
        }
        #endregion

        #region クリアイベント
        /// <summary>
        /// イベント名：クリア
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Clear()
        {
            // 画面を表示する
            return RedirectToAction("Init");
        }
        #endregion

        #region 初期設定メソッド
        /// <summary>
        /// メソッド：初期設定
        /// </summary>
        ///<param name="model">ビューモデル</param>
        /// <returns>モデル</returns>
        private D990002Model Init(D990002Model model)
        {
            var db = getSystemCommonDb();

            // データロック一覧
            db.TDataLocks.ToList().ForEach(x => model.DataLocks.Locks.Add(new()
            {
                SystemKbn = x.SystemKbn,
                SystemKbnNm = x.SystemKbn + CoreConst.SEPARATOR + HanyokubunUtil.GetKbnNm("system_kbn", x.SystemKbn),
                TodofukenCd = x.TodofukenCd,
                KumiaitoCd = x.KumiaitoCd,
                KumiaitoNm = KumiaitoUtil.GetKumiaitoCdNm(x.TodofukenCd, x.KumiaitoCd),
                ShishoCd = x.ShishoCd,
                ShishoNm = ShishoUtil.GetShishoCdNm(x.TodofukenCd, x.KumiaitoCd, x.ShishoCd),
                LockDate = x.LockDate,
                LockEndDate = x.LockEndDate,
                LockUserId = x.LockUserId,
                LockShori = x.LockShori,
            }));
            
            // システムロック一覧
            db.TSysLocks.ToList().ForEach(x => model.SysLocks.Locks.Add(new()
            {
                SystemKbn = x.SystemKbn,
                SystemKbnNm = x.SystemKbn + CoreConst.SEPARATOR + HanyokubunUtil.GetKbnNm("system_kbn", x.SystemKbn),
                TodofukenCd = x.TodofukenCd,
                LockDate = x.LockDate,
                LockEndDate = x.LockEndDate,
                LockUserId = x.LockUserId,
                LockShori = x.LockShori,
            }));

            return model;
        }
        #endregion

        #region システムロック登録メソッド
        /// <summary>
        /// システムロック登録
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Insert([Bind("TodofukenDropDownList, SystemKbn, LockStart, LockEnd, LockShori, UserId")] D990002Model form)
        {
            // システム区分
            string systemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN);
            // エラーメッセージ
            string message = string.Empty;

            var result = LockUtil.InsertSysLock(systemKbn, form.TodofukenDropDownList.TodofukenCd, form.LockStart, form.LockEnd, form.LockShori, form.UserId, ref message);
            logger.Debug("InsertSysLock result:" + result);

            // 利用可能な支所一覧
            var shishoList = SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, HttpContext);

            // 都道府県マルチドロップダウンリスト
            form.TodofukenDropDownList = new TodofukenDropDownList("", Syokuin, shishoList);

            if (LockUtil.RET_FAIL == result)
            {
                // 処理成否（失敗）
                form.MessageArea1 = "システムロックの登録に失敗しました。";
            }
            else if (LockUtil.RET_SUCCESS == result)
            {
                // 処理成否（成功）
                form.MessageArea1 = "システムロックの登録に成功しました。";
            }
            else if (LockUtil.RET_ALREADY_LOCKED == result)
            {
                // 処理成否（既にロック済）
                form.MessageArea1 = "システムは既にロック済みです。";
            }

            // 画面を表示する
            return View(SCREEN_ID_D990002, Init(form));
        }
        #endregion
    }
}