using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore.Storage;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskWeb.Areas.F105.Consts;
using NskWeb.Areas.F105.Models.D105073;
using System.Data;
using System.Text.RegularExpressions;

namespace NskWeb.Areas.F105.Controllers
{
    [AllowAnonymous]
    [ExcludeAuthCheck]
    [Area("F105")]
    public class D105073Controller : CoreController
    {
        /// <summary>
        /// セッションキー(D105073)
        /// </summary>
        private const string SESS_D105073 = $"{F105Const.SCREEN_ID_D105073}_SCREEN";



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public D105073Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        // GET: F105/D105073
        public ActionResult Index()
        {
            if (ConfigUtil.Get(CoreConst.D0000_DISPLAY_FLAG) == "true")
            {
                // 画面表示モードを設定
                SetScreenModeFromQueryString();

            }

            return RedirectToAction("Init", F105Const.SCREEN_ID_D105073, new { area = "F105" });
        }

        /// <summary>
        /// 初期表示
        /// </summary>
        /// <returns>加入申込書入力（麦）危険段階区分設定画面表示結果</returns>
        public ActionResult Init()
        {
            // １．セッション情報をクリアする。
            SessionUtil.Remove(SESS_D105073, HttpContext);

            D105073Model model = new();

            // １．１．権限チェック
            // (1)	ログインユーザの権限が「参照」
            // 「更新権限」いずれも許可されていない場合、メッセージを設定し業務エラー画面を表示する。
            bool dispKengen = ScreenSosaUtil.CanReference(F105Const.SCREEN_ID_D105073, HttpContext);
            bool updKengen = ScreenSosaUtil.CanUpdate(F105Const.SCREEN_ID_D105073, HttpContext);
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


            // ２．画面表示用データを取得する。	
            // ２．１．セッションから「組合等コード」「都道府県コード」「年産」「共済目的」「組合員等コード」「氏名」「電話番号」「支所コード」「支所名」	
            // 「市町村コード」「市町村名」「大地区コード」「大地区名」「小地区コード」「小地区名」「合併時識別コード」
            // 「引受方式」「引受方式名称」「類区分」「類区分名称」「一筆半損特約」「補償割合コード」を取得する。

            D105073SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            NskAppContext dbContext = getJigyoDb<NskAppContext>();

            // ２．２．統計単位地域危険段階区分設定を取得する。
            model.KikenDankaiKbn.GetPageDataList(dbContext, sessionInfo, F105Const.PAGE_1);

            // ※引受方式がインデックス以外の場合でデータ未登録の場合、空データを1件入れる
            if (sessionInfo.HikiukeHoushikiCd != $"{(int)F105Const.HikiukeHoushikiCd.ChiikiIndex}" &&
                model.KikenDankaiKbn.DispRecords.Count == 0)
            {
                model.KikenDankaiKbn.DispRecords.Add(new() { IsNewRec = true, TokeiTaniChiikiCd = "0" });
            }


            // ２．３．[危険段階区分（料率）]ドロップダウンリスト項目を取得する。
            model.KikenDankaiKbn.InitializeDropdonwList(dbContext, sessionInfo);


            // ３．画面項目設定
            // ３．１．[２．１．][２．２．][２．３．] で取得した値を設定する。
            model.Nensan = $"{sessionInfo.Nensan}";
            model.KyosaiMokutekiCd = sessionInfo.KyosaiMokutekiCd;
            model.KyosaiMokuteki = dbContext.M00010共済目的名称s.SingleOrDefault(x =>
                (x.共済目的コード == model.KyosaiMokutekiCd))?.共済目的名称 ?? string.Empty;
            model.KumiaiinToCd = sessionInfo.KumiaiintoCd;
            model.FullNm = sessionInfo.FullNm;
            model.Tel = sessionInfo.Tel;
            model.ShishoCd = sessionInfo.ShishoCd;
            model.ShishoNm = sessionInfo.ShishoNm;
            model.ShichosonCd = sessionInfo.ShichosonCd;
            model.ShichosonNm = sessionInfo.ShichosonNm;
            model.DaichikuCd = sessionInfo.DaichikuCd;
            model.DaichikuNm = sessionInfo.DaichikuNm;
            model.ShochikuCd = sessionInfo.ShochikuCd;
            model.ShochikuNm = sessionInfo.ShochikuNm;
            model.GappeijiShikibetu = sessionInfo.GappeiShikibetsuCd;
            model.HikiukeHoushikiNm = sessionInfo.HikiukeHoushikiNm;
            model.HikiukeHoushikiCd = sessionInfo.HikiukeHoushikiCd;
            model.RuiKbnNm = sessionInfo.RuiKbnNm;
            model.RuiKbn = sessionInfo.RuiKbn;

            // 結果をセッションに保存する
            SessionUtil.Set(SESS_D105073, model, HttpContext);

            ModelState.Clear();

            // 加入申込書入力（麦）危険段階区分設定画面を表示する
            return View(F105Const.SCREEN_ID_D105073, model);
        }


        #region ページャーイベント
        /// <summary>
        /// 危険段階区分ページャー
        /// </summary>
        /// <param name="id">ページID</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult KikenDankaiKbnPager(string id)
        {
            // ページIDは数値以外のデータの場合
            if (!Regex.IsMatch(id, @"^[0-9]+$") || F105Const.PAGE_0 == id)
            {
                return BadRequest();
            }

            // セッションから加入申込書入力（麦）危険段階区分設定モデルを取得する
            D105073Model model = SessionUtil.Get<D105073Model>(SESS_D105073, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }
            // メッセージをクリアする
            model.MessageArea1 = string.Empty;
            model.MessageArea2 = string.Empty;

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // 危険段階区分を取得する
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            D105073SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            model.KikenDankaiKbn.GetPageDataList(dbContext, sessionInfo, int.Parse(id));

            // [危険段階区分（料率）]ドロップダウンリスト項目を取得する。
            model.KikenDankaiKbn.InitializeDropdonwList(dbContext, sessionInfo);


            // 結果をセッションに保存する
            SessionUtil.Set(SESS_D105073, model, HttpContext);

            return PartialViewAsJson("_D105073KikenDankaiKbnResult", model);
        }
        #endregion

        /// <summary>
        /// 統計単位地域コード、危険段階区分入力行挿入
        /// 統計単位地域コード、危険段階区分入力行を挿入する。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SystemException"></exception>
        public ActionResult AddTokeiTaniRow(D105073Model dispModel)
        {
            // セッションから加入申込書入力（麦）危険段階区分設定モデルを取得する
            D105073Model model = SessionUtil.Get<D105073Model>(SESS_D105073, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            model.KikenDankaiKbn.ApplyInput(dispModel.KikenDankaiKbn);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // ２．１．統計単位地域コード、危険段階区分入力情報リストにレコード（行）を挿入する
            model.KikenDankaiKbn.AddPageData();

            // ２．２．[危険段階区分（料率）]ドロップダウンリスト項目を取得する。
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            D105073SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            model.KikenDankaiKbn.InitializeDropdonwList(dbContext, sessionInfo);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105073, model, HttpContext);

            // ２．３．部分ビューを構築する
            return PartialViewAsJson("_D105073KikenDankaiKbnResult", model);
        }

        /// <summary>
        /// 統計単位地域コード、危険段階区分選択行削除
        /// 統計単位地域コード、危険段階区分入力でチェックされた行を削除する。
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DelTokeiTaniRows(D105073Model dispModel)
        {
            // セッションから加入申込書入力（麦）危険段階区分設定モデルを取得する
            D105073Model model = SessionUtil.Get<D105073Model>(SESS_D105073, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            model.KikenDankaiKbn.ApplyInput(dispModel.KikenDankaiKbn);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // ２．１．統計単位地域コード、危険段階区分情報リストの選択レコード（行）を削除する
            model.KikenDankaiKbn.DelPageData();

            // ２．２．[危険段階区分（料率）]ドロップダウンリスト項目を取得する。
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            D105073SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            model.KikenDankaiKbn.InitializeDropdonwList(dbContext, sessionInfo);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105073, model, HttpContext);

            // ２．３．部分ビューを構築する
            return PartialViewAsJson("_D105073KikenDankaiKbnResult", model);
        }

        /// <summary>
        /// 統計単位地域コード、危険段階区分登録
        /// 統計単位地域コード、危険段階区分の入力内容を登録する。
        /// </summary>
        /// <returns></returns>
        public ActionResult RegistTokeiTani(D105073Model dispModel)
        {
            IDbContextTransaction? transaction = null;
            string errMessage = string.Empty;
            try
            {
                D105073SessionInfo sessionInfo = new();
                sessionInfo.GetInfo(HttpContext);

                // セッションから加入申込書入力（麦）危険段階区分設定モデルを取得する
                D105073Model model = SessionUtil.Get<D105073Model>(SESS_D105073, HttpContext);

                // セッションに自画面のデータが存在しない場合
                if (model is null)
                {
                    throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
                }

                // 画面入力値をセッションモデルに反映
                model.KikenDankaiKbn.ApplyInput(dispModel.KikenDankaiKbn);

                NskAppContext dbContext = getJigyoDb<NskAppContext>();
                transaction = dbContext.Database.BeginTransaction();

                int execCount = 0;

                // ３．統計単位地域コード危険段階区分登録処理
                // ３．１．統計単位地域コード危険段階区分入力の一覧から行削除されたデータがある場合、削除対象レコードを取得する。
                List<D105073KikenDankaiKbnRecord> delRecords = model.KikenDankaiKbn.GetDeleteRecs();

                // ３．１．１．統計単位地域コード危険段階区分入力の一覧から行削除されたデータがある場合、t_11040_個人料率の対象レコードを削除する。
                if (delRecords.Count > 0)
                {
                    execCount += model.KikenDankaiKbn.DeleteKikenDankaiKbn(ref dbContext, sessionInfo, ref delRecords);

                }

                // ３．１．２．統計単位地域コード危険段階区分入力の一覧で変更されたデータがある場合、t_11040_個人料率の対象レコードを更新する。
                List<D105073KikenDankaiKbnRecord> updRecords = model.KikenDankaiKbn.GetUpdateRecs(ref dbContext, sessionInfo);
                if (updRecords.Count > 0)
                {
                    execCount += model.KikenDankaiKbn.UpdateKikenDankaiKbn(
                        ref dbContext,
                        sessionInfo,
                        GetUserId(),
                        DateUtil.GetSysDateTime(),
                        ref updRecords);

                }

                // ３．１．３．統計単位地域コード危険段階区分入力の一覧に行追加されたデータがある場合、t_11040_個人料率の対象レコードを登録する。
                List<D105073KikenDankaiKbnRecord> kikenDankaiKbnAddRecords = model.KikenDankaiKbn.GetAddRecs();
                if (kikenDankaiKbnAddRecords.Count > 0)
                {
                    execCount += model.KikenDankaiKbn.AppendKikenDankaiKbn(
                        ref dbContext,
                        sessionInfo,
                        GetUserId(),
                        DateUtil.GetSysDateTime(),
                        ref kikenDankaiKbnAddRecords);

                }


                // ３．２．削除、更新、登録のいずれかが行われたデータが1件でもある場合、[メッセージエリア1]に以下のメッセージを表示する。
                if (execCount > 0)
                {
                    errMessage = MessageUtil.Get("MI00004", "登録");
                }

                transaction.CommitAsync();

                // 検索条件と検索結果をセッションに保存する
                SessionUtil.Set(SESS_D105073, model, HttpContext);
            }
            catch (Exception ex)
            {
                transaction?.RollbackAsync();

                if (string.IsNullOrEmpty(errMessage))
                {
                    if (ex is DBConcurrencyException)
                    {
                        // 排他エラーが含まれる場合は、以下のメッセージを表示する。
                        // [変数：エラーメッセージ] にエラーメッセージを設定
                        errMessage = MessageUtil.Get("ME10081");
                    }
                    else
                    {
                        //  [変数：エラーメッセージ] にエラーメッセージを設定
                        errMessage = MessageUtil.Get("MF00001");
                    }
                }
            }

            // 処理結果をJSON化して返送する。
            return Json(new { message = errMessage });
        }


        #region 戻るイベント
        /// <summary>
        /// 戻る
        /// 加入申込書入力（麦）画面へ遷移する。
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Back()
        {
            // １．加入申込書入力（麦）画面へ戻る。
            return Json(new { result = "success" });
        }
        #endregion



        /// <summary>
        /// 統計単位地域コードに紐づく統計単位地域名、危険段階区分取得
        /// 統計単位地域コードに紐づく統計単位地域名、危険段階区分候補値を取得し画面表示する。
        /// </summary>
        /// <param name="tokeiTanniChiikiCd"></param>
        /// <returns></returns>
        public ActionResult UpdateTokeiTanniChiikiName(string tokeiTanniChiikiCd)
        {
            // ２．１．統計単位地域名を取得する。
            D105073SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            M00170統計単位地域? tokeiTaniChiiki = dbContext.M00170統計単位地域s.SingleOrDefault(x =>
                (x.組合等コード == sessionInfo.KumiaitoCd) &&
                (x.年産 == sessionInfo.Nensan) &&
                (x.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
                (x.統計単位地域コード == tokeiTanniChiikiCd)
                );
            string tokeiTanniChiikiNm = tokeiTaniChiiki?.統計単位地域名称 ?? string.Empty;

            // ２．２．[危険段階区分（料率）]ドロップダウンリスト項目を取得する。
            List<SelectListItem> kikenDankaiKbnList = new();
            kikenDankaiKbnList.AddRange(dbContext.M10230危険段階s.Where(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.年産 == sessionInfo.Nensan) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
                (m.合併時識別コード == (sessionInfo.GappeiShikibetsuCd ?? string.Empty)) &&
                (m.類区分 == (sessionInfo.RuiKbn ?? string.Empty)) &&
                (m.地域単位区分 == tokeiTanniChiikiCd) &&
                (m.引受方式 == (sessionInfo.HikiukeHoushikiCd ?? string.Empty)) &&
                (m.特約区分 == (sessionInfo.IppitsuHansonTokuyaku ?? string.Empty)) &&
                (m.補償割合コード == (sessionInfo.HoshoWariai ?? string.Empty))
                )?.
                OrderBy(m => m.危険段階区分).
                Select(m => new SelectListItem($"{m.危険段階区分}", $"{m.危険段階区分}")));

            List<string> options = new();
            options.Add("<option value=\"\"></option>");
            for (int i = 0; i < kikenDankaiKbnList.Count; i++)
            {
                options.Add($"<option value=\"{kikenDankaiKbnList[i].Value}\">{kikenDankaiKbnList[i].Text}</option>");
            }

            // ２．３．部分ビューを構築する																																																						
            return Json(new { tokeiTanniChiikiNm, options });
        }
    }
}
