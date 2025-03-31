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
using NskWeb.Areas.F110.Consts;
using NskWeb.Areas.F110.Models.D110010;
using NskWeb.Common.Consts;
using System.Data;

namespace NskWeb.Areas.F110.Controllers
{
    /// <summary>
    /// 再引受前処理コントローラ
    /// </summary>
    [AllowAnonymous]
    [ExcludeAuthCheck]
    [Area("F110")]
    public class D110010Controller : CoreController
    {
        /// <summary>
        /// セッションキー(D110010)
        /// </summary>
        private const string SESS_D110010 = $"{F110Const.SCREEN_ID_D110010}_SCREEN";

        /// <summary>
        /// 引受回上限
        /// </summary>
        private const int HIKIUKEKAI_MAX = 99;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public D110010Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        // GET: F110/D110010
        public ActionResult Index()
        {
            if (ConfigUtil.Get(CoreConst.D0000_DISPLAY_FLAG) == "true")
            {
                // 画面表示モードを設定
                SetScreenModeFromQueryString();

            }

            return RedirectToAction("Init", F110Const.SCREEN_ID_D110010, new { area = "F110" });
        }

        /// <summary>
        /// 初期表示
        /// </summary>
        /// <returns>再引受前処理画面表示結果</returns>
        public ActionResult Init()
        {
            // １．セッション情報をクリアする。
            SessionUtil.Remove(SESS_D110010, HttpContext);

            // ２．権限チェック
            // (1)	ログインユーザの権限が「参照」「更新権限」いずれも許可されていない場合、メッセージを設定し業務エラー画面を表示する。
            bool dispKengen = ScreenSosaUtil.CanReference(F110Const.SCREEN_ID_D110010, HttpContext);
            bool updKengen = ScreenSosaUtil.CanUpdate(F110Const.SCREEN_ID_D110010, HttpContext);

            F110Const.DispAuthority dispAuthority = F110Const.DispAuthority.None;
            F110Const.UserAuthority userAuthority = F110Const.UserAuthority.None;
            if (updKengen)
            {
                dispAuthority = F110Const.DispAuthority.Update;// "更新権限";
            }
            else if (dispKengen)
            {
                dispAuthority = F110Const.DispAuthority.ReadOnly;// "参照権限";
            }
            else
            {
                throw new AppException("ME10075", MessageUtil.Get("ME10075"));
            }

            // ログインユーザの支所コードが「00：本所」の場合、「本所権限」、それ以外を「支所権限」とする
            userAuthority = Syokuin.ShishoCd == AppConst.HONSHO_CD ? F110Const.UserAuthority.Honsho : F110Const.UserAuthority.Shisho;


            // ３．画面表示用データを取得する。
            // ３．１．セッションから「都道府県コード」「組合等コード」「支所コード][支所グループコード]「年産」「共済目的」を取得する。
            D110010SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            D110010Model model = new();
            model.DispKengen = dispAuthority;
            model.UserKengen = userAuthority;


            NskAppContext dbContext = getJigyoDb<NskAppContext>();


            // ４．[本所・支所]ドロップダウンリスト項目を取得する。
            model.InitializeDropdonwList(dbContext, sessionInfo);


            // ５．画面項目設定
            // ５．１．[２．１．]で取得した値を設定する。
            model.Nensan = $"{sessionInfo.Nensan}";
            model.KyosaiMokutekiCd = sessionInfo.KyosaiMokutekiCd;
            model.KyosaiMokuteki = dbContext.M00010共済目的名称s.SingleOrDefault(x =>
                (x.共済目的コード == model.KyosaiMokutekiCd))?.共済目的名称 ?? string.Empty;

            // 結果をセッションに保存する
            SessionUtil.Set(SESS_D110010, model, HttpContext);

            // ６．引受回一覧更新
            // ６．１．引受回表示メソッドの呼び出し
            model = DispUnderwritingCycle(model.HonshoShishoCd);

            ModelState.Clear();

            // ７．画面制御設定
            // ７．１．画面コントロール表示制御メソッドの呼び出し
            model.DispControl();


            // 再引受前処理画面を表示する
            return View(F110Const.SCREEN_ID_D110010, model);
        }

        /// <summary>
        /// 本所支所変更
        /// 本所・支所を選択した際に呼び出される。
        /// </summary>
        /// <param name="shishoCd"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Change(string shishoCd)
        {
            // １．引受回一覧更新

            // １．１．引受回表示メソッドの呼び出し
            D110010Model model = DispUnderwritingCycle(shishoCd);


            // ２．画面制御設定
            // ２．１．画面コントロール表示制御メソッドの呼び出し
            model.DispControl();

            // 部分ビューを構築する																																																						
            return PartialViewAsJson("_D110010HikiukeKaiResult", model, model.MessageArea1);
        }



        #region 戻るイベント
        /// <summary>
        /// 戻る
        /// 農作物ポータルへ遷移する。
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Back()
        {
            // １．農作物ポータルへ戻る。
            return Json(new { result = "success" });
        }
        #endregion

        /// <summary>
        /// 再引受前処理実行前チェック
        /// 再引受前処理実行前チェックを行う。
        /// </summary>
        /// <param name="shishoCd"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckExecusion(string shishoCd)
        {
            string errMessage = string.Empty;
            // 確定済みの引受回有無
            bool isExistsKakuteiZumiHikiukeKai = false;
            // 支所コードが本所か否か
            bool isShishoCdHonsho = false;
            // 引受回数が99回以上の有無
            bool isExistsHikiukeKaiLimit = false;

            try
            {
                // １．実行前排他チェック
                // セッションから再引受前処理モデルを取得する
                D110010Model model = SessionUtil.Get<D110010Model>(SESS_D110010, HttpContext);
                D110010SessionInfo sessionInfo = new();
                sessionInfo.GetInfo(HttpContext);

                NskAppContext dbContext = getJigyoDb<NskAppContext>();

                // １．１．引受回取得メソッドの呼び出し
                List<D110010HikiukeKaiRecord> dbRecs = model.HikiukeKai.GetResult(dbContext, sessionInfo, shishoCd);

                // １．２．引受回取得メソッドで取得した結果とセッション「検索結果（引受回情報）」を比較する。
                bool existsDiff = model.IsExistsDiff(dbContext, sessionInfo);

                // 比較結果 一致
                if (!existsDiff)
                {
                    // ２．再引受前処理対象取得
                    List<D110010HikiukeKaiRecord> targetRecs = model.HikiukeKai.GetTargets();

                    // ２．２．セッション「処理対象（引受回情報）」に格納されたレコード判定
                    isExistsKakuteiZumiHikiukeKai = !(targetRecs.Where(x => !x.KakuteiHikiukeKai.HasValue).Count() == targetRecs.Count);

                    // ２．３．支所の場合、引受回数で再引受可能か判定を行う。
                    if (shishoCd == AppConst.HONSHO_CD)
                    {
                        // 本所の場合
                        isShishoCdHonsho = true;
                    }
                    else
                    {
                        // 支所の場合

                        // 引受回数が99回以上の有無
                        isExistsHikiukeKaiLimit = targetRecs.Any(x => x.HikiukeKai.GetValueOrDefault(0) >= HIKIUKEKAI_MAX);

                        if (isExistsHikiukeKaiLimit)
                        {
                            errMessage = MessageUtil.Get("ME10067", $"{HIKIUKEKAI_MAX}");
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(errMessage))
                {
                    //  [変数：エラーメッセージ] にエラーメッセージを設定
                    errMessage = MessageUtil.Get("MF00001");
                }
            }

            return Json(new {
                isExistsKakuteiZumiHikiukeKai,
                isShishoCdHonsho,
                isExistsHikiukeKaiLimit,
                message = errMessage });
        }

        /// <summary>
        /// 再引受前処理実行
        /// 選択した本所・支所で再引受前処理を実行する。													
        /// </summary>
        /// <param name="shishoCd"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Execusion(string shishoCd)
        {
            IDbContextTransaction? transaction = null;
            string errMessage = string.Empty;

            // セッションから再引受前処理モデルを取得する
            D110010Model model = SessionUtil.Get<D110010Model>(SESS_D110010, HttpContext);

            try
            {

                D110010SessionInfo sessionInfo = new();
                sessionInfo.GetInfo(HttpContext);

                NskAppContext dbContext = getJigyoDb<NskAppContext>();

                // １．トランザクションの開始
                transaction = dbContext.Database.BeginTransaction();

                // ２．支所再引受前処理
                DateTime systemDate = DateUtil.GetSysDateTime();
                int registCnt = model.HikiukeKai.RegistHikiukeKai(ref dbContext, sessionInfo, systemDate, Syokuin.UserId);


                // ３．トランザクションのコミット
                transaction.Commit();

                // ４．削除、更新、登録のいずれかが行われたデータが1件でもある場合、[メッセージエリア１]に以下のメッセージを表示する。
                if (registCnt > 0)
                {
                    errMessage = MessageUtil.Get("MI00004", "登録");
                }

            }
            catch (Exception ex)
            {
                // ５．１．エラーが発生した場合は、ロールバックを行う。
                transaction?.Rollback();

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

            // ６．引受回一覧更新
            // ６．１．引受回表示メソッドの呼び出し
            model = DispUnderwritingCycle(shishoCd);

            // 部分ビューを構築する																																																						
            return PartialViewAsJson("_D110010HikiukeKaiResult", model, errMessage);
        }


        /// <summary>
        /// 引受回表示								
        /// 選択した支所・本所の引受回情報を画面に表示する。													
        /// </summary>
        private D110010Model DispUnderwritingCycle(string shishoCd)
        {
            // セッションから再引受前処理モデルを取得する
            D110010Model model = SessionUtil.Get<D110010Model>(SESS_D110010, HttpContext);

            D110010SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            // １．表示中の引受回情報をクリアする。
            model.HikiukeKai.DispRecords = new();


            // 画面選択値（本所・支所コード）をモデルにセット
            model.HonshoShishoCd = shishoCd;


            // ２．引受回情報取得
            // ２．１．引受回セッション保存メソッドの呼び出し
            SessionSave(ref model, sessionInfo);

            return model;
        }

        /// <summary>
        /// 引受回セッション保存
        /// 引受回情報のセッション保存をする。
        /// </summary>
        /// <param name="model"></param>
        private void SessionSave(ref D110010Model model, D110010SessionInfo sessionInfo)
        {
            // １．引受回情報取得前処理

            // ２．引受回情報の取得
            GetUnderwritingCycle(ref model, sessionInfo);

            // ２．２．検索結果のチェック
            if (model.HikiukeKai.DispRecords.Count == 0)
            {
                // 検索結果 0件
                model.MessageArea1 = MessageUtil.Get("MI10007", "0");
            }
            else
            {
                // 検索結果 1件以上

                // 結果をセッションに保存する
                SessionUtil.Set(SESS_D110010, model, HttpContext);
            }
        }

        /// <summary>
        /// 引受回取得
        /// 引受回情報の取得をする。
        /// </summary>
        private void GetUnderwritingCycle(ref D110010Model model, D110010SessionInfo sessionInfo)
        {
            // １．[本所・支所]ドロップダウンリストで選択された支所毎に引受回情報の取得
            NskAppContext dbContext = getJigyoDb<NskAppContext>();

            model.HikiukeKai.DispRecords =
                model.HikiukeKai.GetResult(dbContext, sessionInfo, model.HonshoShishoCd);
        }

    }
}
