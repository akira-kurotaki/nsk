using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using NskAppModelLibrary.Context;
using NskWeb.Areas.F105.Consts;
using NskWeb.Areas.F105.Models.D105036;
using System.Text.RegularExpressions;

namespace NskWeb.Areas.F105.Controllers
{
    [AllowAnonymous]
    [ExcludeAuthCheck]
    [Area("F105")]
    public class D105036Controller : CoreController
    {
        /// <summary>
        /// セッションキー(D105036)
        /// </summary>
        private const string SESS_D105036 = $"{F105Const.SCREEN_ID_D105036}_SCREEN";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public D105036Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        // GET: F105/D105036
        public ActionResult Index()
        {
            if (ConfigUtil.Get(CoreConst.D0000_DISPLAY_FLAG) == "true")
            {
                // 画面表示モードを設定
                SetScreenModeFromQueryString();

            }

            return RedirectToAction("Init", F105Const.SCREEN_ID_D105036, new { area = "F105" });
        }

        /// <summary>
        /// 初期表示
        /// </summary>
        /// <returns>加入申込書入力（水稲）画面表示結果</returns>
        public ActionResult Init()
        {
            // １．セッション情報をクリアする。
            SessionUtil.Remove(SESS_D105036, HttpContext);

            // １．１．権限チェック
            // (1)	ログインユーザの権限が「参照」「更新権限」いずれも許可されていない場合、メッセージを設定し業務エラー画面を表示する。
            bool dispKengen = ScreenSosaUtil.CanReference(F105Const.SCREEN_ID_D105036, HttpContext);
            bool updKengen = ScreenSosaUtil.CanUpdate(F105Const.SCREEN_ID_D105036, HttpContext);

            // ２．画面表示用データを取得する。

            // ２．１．セッションから「都道府県コード」「組合等コード」「共済目的」「年産」「利用可能な支所一覧」を取得する。
            D105036SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            D105036Model model = new(Syokuin, sessionInfo.ShishoList);

            model.DispKengen = F105Const.Authority.None;
            if (updKengen)
            {
                model.DispKengen = F105Const.Authority.Update;// "更新権限";
            }
            else if (dispKengen)
            {
                model.DispKengen = F105Const.Authority.ReadOnly;// "参照権限";
            }
            else
            {
                throw new AppException("ME10075", MessageUtil.Get("ME10075"));
            }

            // ３．画面項目設定
            NskAppContext dbContext = getJigyoDb<NskAppContext>();

            model.KyosaiMokutekiCd = sessionInfo.KyosaiMokutekiCd;
            model.KyosaiMokuteki = dbContext.M00010共済目的名称s.SingleOrDefault(x =>
                (x.共済目的コード == model.KyosaiMokutekiCd))?.共済目的名称 ?? string.Empty;
            model.Nensan = $"{sessionInfo.Nensan}";

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105036, model, HttpContext);

            // 加入申込書入力組合員等検索（共通）画面を表示する
            return View(F105Const.SCREEN_ID_D105036, model);
        }

        #region ページャーイベント
        /// <summary>
        /// 検索結果ページャー
        /// </summary>
        /// <param name="id">ページID</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult ResultPager(string id)
        {
            // ページIDは数値以外のデータの場合
            if (!Regex.IsMatch(id, @"^[0-9]+$") || F105Const.PAGE_0 == id)
            {
                return BadRequest();
            }

            // セッションから加入申込書入力組合員等検索（共通）モデルを取得する
            D105036Model model = SessionUtil.Get<D105036Model>(SESS_D105036, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // 検索結果を取得する
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            D105036SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            model.SearchResult.GetPageDataList(dbContext, sessionInfo, int.Parse(id));

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105036, model, HttpContext);

            return PartialViewAsJson("_D105036SearchResult", model);
        }
        #endregion

        /// <summary>
        /// 加入申込書入力組合員等を検索する。													
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Search(D105036Model dispModel)
        {
            D105036SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            // セッションから加入申込書入力（水稲）モデルを取得する
            D105036Model model = SessionUtil.Get<D105036Model>(SESS_D105036, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            model.SearchCondition.ApplyInput(dispModel.SearchCondition);

            // ２．加入申込書入力組合員等検索処理
            // ２．１．検索結果クリア
            model.MessageArea1 = string.Empty;
            model.SearchResult = new(model.SearchCondition);

            // ２．２．検索条件チェックしエラーの場合、エラーメッセージを設定する。
            // (1)[画面：小地区（開始）]と[画面：小地区（終了）]に値が入力されている、かつ、[画面：小地区（開始）]＞[画面：小地区（終了）] の場合、
            // エラーと判定し、エラーメッセージを「メッセージエリア１」に設定する。
            if ((!string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom) &&
                (!string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShochikuCdTo))) &&
                (model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom.CompareTo(
                 model.SearchCondition.TodofukenDropDownList.ShochikuCdTo) > 0))
            {
                // エラーメッセージを「メッセージエリア１」に設定する。
                model.MessageArea1 = MessageUtil.Get("ME10022", "小地区（終了）", "小地区（開始）");

                ModelState.AddModelError("SearchCondition_TodofukenDropDownList_ShochikuCdFrom", model.MessageArea1);
            }

            // (2)[画面：組合員等コード（開始）]と[画面：組合員等コード（終了）]に値が入力されている、かつ、[画面：組合員等コード（開始）]＞[画面：組合員等コード（終了）] の場合、
            // エラーと判定し、エラーメッセージを「メッセージエリア１」に設定する。
            if ((!string.IsNullOrEmpty(model.SearchCondition.KumiaiinToCdFrom) &&
                (!string.IsNullOrEmpty(model.SearchCondition.KumiaiinToCdTo))) &&
                (model.SearchCondition.KumiaiinToCdFrom.CompareTo(
                 model.SearchCondition.KumiaiinToCdTo) > 0))
            {
                // エラーメッセージを「メッセージエリア１」に設定する。
                string msg = MessageUtil.Get("ME10022", "組合員等コード（終了）", "組合員等コード（開始）");
                if (string.IsNullOrEmpty(model.MessageArea1))
                    model.MessageArea1 = msg;

                ModelState.AddModelError("SearchCondition_KumiaiinToCdFrom", msg);
            }

            if (ModelState.IsValid)
            {
                // ２．３．検索処理実行
                // ２．３．１．検索結果を取得する。
                NskAppContext dbContext = getJigyoDb<NskAppContext>();
                model.SearchResult.GetPageDataList(dbContext, sessionInfo, F105Const.PAGE_1);

                // ２．４．検索結果のチェック
                // ２．４．１．検索結果0件の場合、エラーメッセージを設定する。
                if (model.SearchResult.Pager.TotalCount == 0)
                {
                    model.MessageArea1 = MessageUtil.Get("MI00011");
                    // 画面エラーメッセージエリアにメッセージ設定
                    ModelState.AddModelError("MessageArea1", MessageUtil.Get("MI00011"));
                }
            }

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105036, model, HttpContext);


            // ３．部分ビューを構築する
            return Json(new { message = model.MessageArea1, resultArea = PartialViewAsJson("_D105036SearchResult", model).Value });
        }

        /// <summary>
        /// 加入申込書入力
        /// 選択行の値をセッションに登録し、共済目的に応じた画面へ遷移する。													
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MoveKanyuMoushikomi(string guid)
        {
            D105036SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            // ２．選択行データをセッションに登録

            // セッションから加入申込書入力組合員等検索（共通）モデルを取得する
            D105036Model model = SessionUtil.Get<D105036Model>(SESS_D105036, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }


            D105036ResultRecord selRec = model.SearchResult.DispRecords.SingleOrDefault(m => m.GUID == guid);
            if (selRec is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            D105036ParamModel paramModel = new()
            {
                // 「組合等コード」
                KumiaitoCd = sessionInfo.KumiaitoCd,
                // 「都道府県コード」
                TodofukenCd = sessionInfo.TodofukenCd,
                // 「年産」
                Nensan = sessionInfo.Nensan,
                // 「共済目的」
                KyosaiMokutekiCd = sessionInfo.KyosaiMokutekiCd,
                // 「組合員等コード」
                KumiaiintoCd = selRec.KumiaiintoCd,
                // 「氏名」
                FullNm = selRec.FullNm,

                // 「電話番号」
                Tel = Syokuin.Tel,

                // 「支所コード」
                ShishoCd = selRec.ShishoCd,
                // 「支所名」
                ShishoNm = selRec.ShishoNm,
                // 「市町村コード」
                ShichosonCd = selRec.ShichosonCd,
                // 「市町村名」
                ShichosonNm = selRec.ShichosonNm,
                // 「大地区コード」
                DaichikuCd = selRec.DaichikuCd,
                // 「大地区名」
                DaichikuNm = selRec.DaichikuNm,
                // 「小地区コード」
                ShochikuCd = selRec.ShochikuCd,
                // 「小地区名」
                ShochikuNm = selRec.ShochikuNm,
                // 「合併時識別コード」
                GappeiShikibetsuCd = selRec.GappeijiShikibetsuCd
            };
            SessionUtil.Set(F105Const.SESS_D105036_PARAMS, paramModel, HttpContext);


            // ３．共済目的に応じた画面遷移
            string gamenId = string.Empty;
            if (sessionInfo.KyosaiMokutekiCd == $"{(int)NskCommonLibrary.Core.Consts.CoreConst.KyosaiMokutekiCdNumber.Suitou}" ||
                sessionInfo.KyosaiMokutekiCd == $"{(int)NskCommonLibrary.Core.Consts.CoreConst.KyosaiMokutekiCdNumber.Rikutou}")
            {
                gamenId = F105Const.SCREEN_ID_D105030;

            }
            else if (sessionInfo.KyosaiMokutekiCd == $"{(int)NskCommonLibrary.Core.Consts.CoreConst.KyosaiMokutekiCdNumber.Mugi}")
            {
                gamenId = F105Const.SCREEN_ID_D105070;

            }

            return RedirectToAction("Init", gamenId, new { area = "F105" });
        }

        #region 戻るイベント
        /// <summary>
        /// 戻る
        /// ポータルへ遷移する。
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Back()
        {
            // ポータル

            return Json(new { result = "success" });
        }
        #endregion
    }
}
