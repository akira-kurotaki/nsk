using System.Data;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore.Storage;
using NskAppModelLibrary.Context;
using NskWeb.Areas.F111.Consts;
using NskWeb.Areas.F111.Models.D111050;

namespace NskWeb.Areas.F111.Controllers
{
    [AllowAnonymous]
    [ExcludeAuthCheck]
    [Area("F111")]
    public class D111050Controller : CoreController
    {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public D111050Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        // GET: F111/D111050
        public ActionResult Index()
        {
            if (ConfigUtil.Get(CoreConst.D0000_DISPLAY_FLAG) == "true")
            {
                // 画面表示モードを設定
                SetScreenModeFromQueryString();

            }

            return RedirectToAction("Init", F111Const.SCREEN_ID_D111050, new { area = "F111" });
        }

        /// <summary>
        /// 画面 初期表示
        /// </summary>
        /// <remarks>画面を初期表示する。</remarks>
        /// <returns>加入申込書入力（麦）危険段階区分設定画面表示結果</returns>
        public ActionResult Init()
        {
            // セッション情報をクリアする。
            SessionUtil.Remove(F111Const.SCREEN_ID_D111050, HttpContext);

            D111050Model model = new();

            // １．画面表示用データを取得する。
            // １．１．セッションから「都道府県コード」「組合等コード」「負担金交付区分」「年産」「交付回」を取得する。
            D111050SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            NskAppContext dbContext = getJigyoDb<NskAppContext>();

            // ２．３．交付徴収額データを取得する。
            model.KakekinChoshugaku.GetResult(dbContext, sessionInfo);

            // ３．画面項目設定
            // ３．１．[２．１．][２．２．][２．３．]で取得した値を設定する。
            model.Nensan = $"{sessionInfo.Nensan}";
            model.FutankinKofuKbnCd = $"{sessionInfo.FutankinKofuKbnCd}";
            model.FutankinKofuKbn = $"{sessionInfo.FutankinKofuKbn}";
            model.Koufukai = $"{sessionInfo.Koufukai}";

            // 結果をセッションに保存する
            SessionUtil.Set(F111Const.SESS_D111050, model, HttpContext);

            ModelState.Clear();

            // 交付金計算処理画面を表示する。
            return View(F111Const.SCREEN_ID_D111050, model);
        }

        #region 入金額入力ボタンイベント
        /// <summary>
        /// 入金額入力
        /// </summary>
        /// <remarks>徴収済み額を入力する。</remarks>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult InputNyukingaku()
        {
            // セッションを取得
            D111050Model model = SessionUtil.Get<D111050Model>(F111Const.SESS_D111050, HttpContext);

            // ３．入金額の取得
            // ３．１．t_12090_組合員等別徴収情報から [負担金交付区分]に沿った徴収金額の合計値を取得する
            D111050SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            NskAppContext dbContext = getJigyoDb<NskAppContext>();

            model.ChoshuzumiGaku.GetResult(dbContext, sessionInfo);

            foreach (D111050KakekinChoshugakuRecord kakekinChoshugakuRecord in model.KakekinChoshugaku.DispRecords)
            {

                foreach (D111050ChoshuzumiGakuRecord choshuzumiGakuRecord in model.ChoshuzumiGaku.DispRecords)
                {

                    if (kakekinChoshugakuRecord.KyosaiMokutekiCd == choshuzumiGakuRecord.KyosaiMokutekiCd)
                    {
                        kakekinChoshugakuRecord.ChoshuzumiGaku = choshuzumiGakuRecord.ChoshuzumiGaku;

                        if (kakekinChoshugakuRecord.Futankin > 0)
                        {
                            kakekinChoshugakuRecord.ChoshuWariai = Math.Floor((decimal)kakekinChoshugakuRecord.ChoshuzumiGaku / (decimal)kakekinChoshugakuRecord.Futankin * 10000) / 100;
                        }
                    }

                }

            }

            // 結果をセッションに保存する
            SessionUtil.Set(F111Const.SESS_D111050, model, HttpContext);

            // ３．２．部分ビューを構築する
            return PartialViewAsJson("_D111050KakekinChoshugaku", model);
        }
        #endregion

        #region 戻るボタンイベント
        /// <summary>
        /// 戻る
        /// </summary>
        /// <remarks>遷移元画面に遷移する。</remarks>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Back()
        {
            // １．ポータル画面へ戻る。
            return Json(new { result = "success" });
        }
        #endregion

        #region 登録ボタンイベント
        /// <summary>
        /// 戻る
        /// </summary>
        /// <remarks>遷移元画面に遷移する。</remarks>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult Regist(D111050Model dispModel)
        {

            IDbContextTransaction? transaction = null;
            string errMessage = string.Empty;

            try
            {
                D111050SessionInfo sessionInfo = new();
                sessionInfo.GetInfo(HttpContext);

                // セッションを取得
                D111050Model model = SessionUtil.Get<D111050Model>(F111Const.SESS_D111050, HttpContext);

                // セッションに自画面のデータが存在しない場合
                if (model is null)
                {
                    throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
                }

                // ４．トランザクションの開始
                NskAppContext dbContext = getJigyoDb<NskAppContext>();
                transaction = dbContext.Database.BeginTransaction();

                // 画面入力値をセッションモデルに反映
                model.KakekinChoshugaku.ApplyInput(dispModel.KakekinChoshugaku);

                // ユーザID
                var userId = Syokuin.UserId;

                // ５．交付徴収テーブル登録
                // ５．１．[セッション：負担金交付区分]から画面表示された共済目的に紐づくレコードを登録する。
                foreach (D111050KakekinChoshugakuRecord target in model.KakekinChoshugaku.DispRecords)
                {
                    // 交付徴収テーブル
                    model.ChoshuzumiGaku.GetRecordCount(dbContext, sessionInfo, userId, target);
                    // 交付回テーブル
                    model.ChoshuzumiGaku.UpdKoufukai(dbContext, sessionInfo, userId, target);

                }

                errMessage = MessageUtil.Get("MI00004", "登録");

                // ６．トランザクションのコミット
                transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                // ５．２．エラーが発生した場合は、ロールバックを行う。
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

            return Json(new { message = errMessage });
        }
        #endregion

    }
}
