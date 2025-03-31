using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore.Storage;
using NskAppModelLibrary.Context;
using NskWeb.Areas.F105.Consts;
using NskWeb.Areas.F105.Models.D105074;
using System.Data;
using System.Text.RegularExpressions;

namespace NskWeb.Areas.F105.Controllers
{
    [AllowAnonymous]
    [ExcludeAuthCheck]
    [Area("F105")]
    public class D105074Controller : CoreController
    {
        /// <summary>
        /// セッションキー(D105074)
        /// </summary>
        private const string SESS_D105074 = $"{F105Const.SCREEN_ID_D105074}_SCREEN";



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public D105074Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        // GET: F105/D105074
        public ActionResult Index()
        {
            if (ConfigUtil.Get(CoreConst.D0000_DISPLAY_FLAG) == "true")
            {
                // 画面表示モードを設定
                SetScreenModeFromQueryString();

            }

            return RedirectToAction("Init", F105Const.SCREEN_ID_D105074, new { area = "F105" });
        }

        /// <summary>
        /// 初期表示
        /// </summary>
        /// <returns>加入申込書入力（麦）用途別単価設定画面表示結果</returns>
        public ActionResult Init()
        {
            // １．セッション情報をクリアする。
            SessionUtil.Remove(SESS_D105074, HttpContext);

            D105074Model model = new();

            // １．１．権限チェック
            // (1)	ログインユーザの権限が「参照」
            // 「更新権限」いずれも許可されていない場合、メッセージを設定し業務エラー画面を表示する。
            bool dispKengen = ScreenSosaUtil.CanReference(F105Const.SCREEN_ID_D105074, HttpContext);
            bool updKengen = ScreenSosaUtil.CanUpdate(F105Const.SCREEN_ID_D105074, HttpContext);
            model.DispKengen = F105Const.Authority.None;
            if (updKengen)
            {
                model.DispKengen = F105Const.Authority.Update;
            }
            else if (dispKengen)
            {
                model.DispKengen = F105Const.Authority.ReadOnly;
            }
            else
            {
                throw new AppException("ME10075", MessageUtil.Get("ME10075"));
            }


            // ２．画面表示用データを取得する。	
            // ２．１．セッションから「組合等コード」「都道府県コード」「年産」「共済目的」「組合員等コード」「氏名」「電話番号」「支所コード」「支所名」	
            // 「市町村コード」「市町村名」「大地区コード」「大地区名」「小地区コード」「小地区名」「合併時識別コード」
            // 「引受方式」「引受方式名称」「類区分」「類区分名称」「一筆半損特約」「補償割合コード」を取得する。

            D105074SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            NskAppContext dbContext = getJigyoDb<NskAppContext>();

            // ２．２．用途別単価設定を取得する。
            // (1)	t_11050_個人用途別より[２．１．]で取得した値に該当する用途別単価設定を取得する。
            // (2) 取得した用途別単価設定リストをセッションに保持する。
            model.YotobetsuTanka.GetPageDataList(dbContext, sessionInfo, F105Const.PAGE_1);



            // ２．３．[作付時期]ドロップダウンリスト項目を取得する。
            // ２．４．[用途区分]ドロップダウンリスト項目を取得する。
            // ２．５．[適用単価]ドロップダウンリスト項目を取得する。
            model.YotobetsuTanka.InitializeDropdonwList(dbContext, sessionInfo);


            // ３．画面項目設定
            // ３．１．[２．１．][２．２．][２．３．][２．４．][２．５．]で取得した値を設定する。
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
            SessionUtil.Set(SESS_D105074, model, HttpContext);

            ModelState.Clear();

            // 加入申込書入力（麦）用途別単価設定画面を表示する
            return View(F105Const.SCREEN_ID_D105074, model);
        }


        #region ページャーイベント
        /// <summary>
        /// 用途別単価設定ページャー
        /// </summary>
        /// <param name="id">ページID</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult YotobetsuTankaPager(string id)
        {
            // ページIDは数値以外のデータの場合
            if (!Regex.IsMatch(id, @"^[0-9]+$") || F105Const.PAGE_0 == id)
            {
                return BadRequest();
            }

            // セッションから加入申込書入力（麦）用途別単価設定モデルを取得する
            D105074Model model = SessionUtil.Get<D105074Model>(SESS_D105074, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }
            // メッセージをクリアする
            model.MessageArea1 = string.Empty;

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // 用途別単価設定を取得する
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            D105074SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            model.YotobetsuTanka.GetPageDataList(dbContext, sessionInfo, int.Parse(id));

            // ドロップダウンリスト項目を取得する。
            model.YotobetsuTanka.InitializeDropdonwList(dbContext, sessionInfo);


            // 結果をセッションに保存する
            SessionUtil.Set(SESS_D105074, model, HttpContext);

            return PartialViewAsJson("_D105074YotobetsuTankaResullt", model);
        }
        #endregion

        /// <summary>
        /// 用途別単価設定行挿入
        /// 用途別単価設定行を挿入する。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SystemException"></exception>
        public ActionResult AddYotobetsuTankaRow(D105074Model dispModel)
        {
            // セッションから加入申込書入力（麦）用途別単価設定モデルを取得する
            D105074Model model = SessionUtil.Get<D105074Model>(SESS_D105074, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            model.YotobetsuTanka.ApplyInput(dispModel.YotobetsuTanka);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // ２．１．用途別単価設定入力情報リストにレコード（行）を挿入する
            model.YotobetsuTanka.AddPageData();

            // ２．２．ドロップダウンリスト項目を取得する。
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            D105074SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            model.YotobetsuTanka.InitializeDropdonwList(dbContext, sessionInfo);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105074, model, HttpContext);

            // ２．３．部分ビューを構築する
            return PartialViewAsJson("_D105074YotobetsuTankaResullt", model);
        }

        /// <summary>
        /// 用途別単価設定選択行削除
        /// 用途別単価設定でチェックされた行を削除する。
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DelYotobetsuTankaRows(D105074Model dispModel)
        {
            // セッションから加入申込書入力（麦）用途別単価設定モデルを取得する
            D105074Model model = SessionUtil.Get<D105074Model>(SESS_D105074, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            model.YotobetsuTanka.ApplyInput(dispModel.YotobetsuTanka);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // ２．１．用途別単価設定リストの選択レコード（行）を削除する
            model.YotobetsuTanka.DelPageData();

            // ２．２．ドロップダウンリスト項目を取得する。
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            D105074SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            model.YotobetsuTanka.InitializeDropdonwList(dbContext, sessionInfo);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105074, model, HttpContext);

            // ２．３．部分ビューを構築する
            return PartialViewAsJson("_D105074YotobetsuTankaResullt", model);
        }

        /// <summary>
        /// 用途別単価設定登録
        /// 用途別単価設定の入力内容を登録する。
        /// </summary>
        /// <returns></returns>
        public ActionResult RegistYotobetsuTanka(D105074Model dispModel)
        {
            IDbContextTransaction? transaction = null;
            string errMessage = string.Empty;

            try
            {
                // セッションから加入申込書入力（麦）用途別単価設定モデルを取得する
                D105074Model model = SessionUtil.Get<D105074Model>(SESS_D105074, HttpContext);

                // セッションに自画面のデータが存在しない場合
                if (model is null)
                {
                    throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
                }

                D105074SessionInfo sessionInfo = new();
                sessionInfo.GetInfo(HttpContext);

                NskAppContext dbContext = getJigyoDb<NskAppContext>();


                // 画面入力値をセッションモデルに反映
                model.YotobetsuTanka.ApplyInput(dispModel.YotobetsuTanka);

                transaction = dbContext.Database.BeginTransaction();

                int execCount = 0;

                // ３．用途別単価設定登録処理
                // ３．１．用途別単価設定入力の一覧から行削除されたデータがある場合、削除対象レコードを取得する。
                List<D105074YotobetsuTankaRecord> delRecords = model.YotobetsuTanka.GetDeleteRecs();

                // ３．１．１．用途別単価設定入力の一覧から行削除されたデータがある場合、t_11050_個人用途別の対象レコードを削除する。
                if (delRecords.Count > 0)
                {
                    execCount += model.YotobetsuTanka.DeleteYotobetsuTanka(ref dbContext, sessionInfo, ref delRecords);

                }

                // ３．１．２．用途別単価設定入力の一覧で変更されたデータがある場合、t_11050_個人用途別の対象レコードを更新する。
                List<D105074YotobetsuTankaRecord> updRecords = model.YotobetsuTanka.GetUpdateRecs(ref dbContext, sessionInfo);
                if (updRecords.Count > 0)
                {
                    execCount += model.YotobetsuTanka.UpdateYotobetsuTanka(
                        ref dbContext,
                        sessionInfo,
                        GetUserId(),
                        DateUtil.GetSysDateTime(),
                        ref updRecords);

                }

                // ３．１．３．用途別単価設定入力の一覧に行追加されたデータがある場合、t_11050_個人用途別の対象レコードを登録する。
                List<D105074YotobetsuTankaRecord> yotobetsuTankaAddRecords = model.YotobetsuTanka.GetAddRecs();
                if (yotobetsuTankaAddRecords.Count > 0)
                {
                    execCount += model.YotobetsuTanka.AppendYotobetsuTanka(
                        ref dbContext,
                        sessionInfo,
                        GetUserId(),
                        DateUtil.GetSysDateTime(),
                        ref yotobetsuTankaAddRecords);

                }


                // ３．２．削除、更新、登録のいずれかが行われたデータが1件でもある場合、[メッセージエリア1]に以下のメッセージを表示する。
                if (execCount > 0)
                {
                    errMessage = MessageUtil.Get("MI00004", "登録");
                }

                transaction.Commit();
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
        /// 作付時期に紐づく用途区分取得
        /// 作付時期に紐づく用途区分候補値を取得し画面表示する。
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        public ActionResult UpdateYotokubun(D105074Model dispModel)
        {
            // セッションから加入申込書入力（麦）用途別単価設定モデルを取得する
            D105074Model model = SessionUtil.Get<D105074Model>(SESS_D105074, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            model.YotobetsuTanka.ApplyInput(dispModel.YotobetsuTanka);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // ２．１．No.1　[２．４．][２．５．]の処理を実行する。
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            D105074SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            // ２．４．[用途区分] ドロップダウンリスト項目を取得する。		
            model.YotobetsuTanka.UpdateYotoKbn(dbContext, sessionInfo);

            // ２．５．[適用単価] ドロップダウンリスト項目を取得する。		
            model.YotobetsuTanka.UpdateTekiyoTanka(dbContext, sessionInfo);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105074, model, HttpContext);

            // ２．３．部分ビューを構築する
            return PartialViewAsJson("_D105074YotobetsuTankaResullt", model);
        }

        /// <summary>
        /// 用途区分に紐づく適用単価取得
        /// 用途区分に紐づく適用単価候補値を取得し画面表示する。
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        public ActionResult UpdateTekiyoTanka(D105074Model dispModel)
        {
            // セッションから加入申込書入力（麦）用途別単価設定モデルを取得する
            D105074Model model = SessionUtil.Get<D105074Model>(SESS_D105074, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            model.YotobetsuTanka.ApplyInput(dispModel.YotobetsuTanka);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // ２．１．No.1　[２．４．][２．５．]の処理を実行する。
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            D105074SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            // ２．４．[用途区分] ドロップダウンリスト項目を取得する。		
            model.YotobetsuTanka.UpdateYotoKbn(dbContext, sessionInfo);

            // ２．５．[適用単価] ドロップダウンリスト項目を取得する。		
            model.YotobetsuTanka.UpdateTekiyoTanka(dbContext, sessionInfo);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105074, model, HttpContext);

            // ２．３．部分ビューを構築する
            return PartialViewAsJson("_D105074YotobetsuTankaResullt", model);
        }
    }
}
