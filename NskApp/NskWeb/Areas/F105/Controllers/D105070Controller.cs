using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ModelLibrary.Models;
using Npgsql;
using NpgsqlTypes;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskWeb.Areas.F105.Consts;
using NskWeb.Areas.F105.Models.D105030;
using NskWeb.Areas.F105.Models.D105070;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace NskWeb.Areas.F105.Controllers
{
    [AllowAnonymous]
    [ExcludeAuthCheck]
    [Area("F105")]
    public class D105070Controller : CoreController
    {
        /// <summary>
        /// セッションキー(D105070)
        /// </summary>
        private const string SESS_D105070 = $"{F105Const.SCREEN_ID_D105070}_SCREEN";



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public D105070Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        // GET: F105/D105070
        public ActionResult Index()
        {
            if (ConfigUtil.Get(CoreConst.D0000_DISPLAY_FLAG) == "true")
            {
                // 画面表示モードを設定
                SetScreenModeFromQueryString();

            }

            return RedirectToAction("Init", F105Const.SCREEN_ID_D105070, new { area = "F105" });
        }

        /// <summary>
        /// 初期表示
        /// </summary>
        /// <returns>加入申込書入力（麦）画面表示結果</returns>
        public ActionResult Init()
        {
            // １．セッション情報をクリアする。
            SessionUtil.Remove(SESS_D105070, HttpContext);

            D105070Model model = new();
            model.HikiukeSearchResult = new(model.SearchCondition);
            model.HikiukeSearchResult.Pager.PageSize = 1;

            // １．１．権限チェック
            // (1)	ログインユーザの権限が「参照」「一部権限」「更新権限」いずれも許可されていない場合、メッセージを設定し業務エラー画面を表示する。
            bool partKengen = ScreenSosaUtil.CanUpdate($"{F105Const.SCREEN_ID_D105070}_001", HttpContext);
            bool dispKengen = ScreenSosaUtil.CanReference(F105Const.SCREEN_ID_D105070, HttpContext);
            bool updKengen = ScreenSosaUtil.CanUpdate(F105Const.SCREEN_ID_D105070, HttpContext);

            model.DispKengen = F105Const.Authority.None;
            if (updKengen)
            {
                model.DispKengen = F105Const.Authority.Update;// "更新権限";
            }
            else if (partKengen)
            {
                model.DispKengen = F105Const.Authority.Part;// "一部権限";
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
            // 「市町村コード」「市町村名」「大地区コード」「大地区名」「小地区コード」「小地区名」「合併時識別コード」を取得する。
            D105070SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            // ２．２．「表示数リスト」を取得する。		
            // ２．３．「表示順」ドロップダウンリスト項目を設定する。		

            // ドロップダウンリスト初期化
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            model.HikiukeSearchResult.InitializeDropdonwList(dbContext, sessionInfo);
            model.KumiaiintoSettei.InitializeDropdonwList(dbContext, sessionInfo);
            model.RuibetsuSettei.InitializeDropdonwList(dbContext, sessionInfo);


            // ２．３．１８．組合員等毎設定を取得する。
            model.KumiaiintoSettei.GetKumiaitoSettei(dbContext, sessionInfo);

            // ２．３．１９．類別設定を取得する。
            List<D105070RuibetsuSetteiRecord> ruibetsuSettiRecords = model.RuibetsuSettei.GetResult(dbContext, sessionInfo);
            model.RuibetsuSettei.GetPageDataList(dbContext, sessionInfo, F105Const.PAGE_1);

            // ２．３．２０．引受確定の有無を取得する。
            model.ExistsHikiukeKakutei = model.GetHikiukeKakutei(dbContext, sessionInfo);


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


            // ３．２．検索結果（入力データ（組合員等毎設定））をセッションに保持する。				
            // ３．３．検索結果（入力データ（類別設定リスト））をセッションに保持する。
            SessionUtil.Set(SESS_D105070, model, HttpContext);

            ModelState.Clear();
            // 加入申込書入力（麦）画面を表示する
            return View(F105Const.SCREEN_ID_D105070, model);
        }



        /// <summary>
        /// 引受情報検索処理
        /// 引受情報を検索し、画面に表示する。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SearchHikiuke(D105070Model dispModel)
        {
            D105070SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            // セッションから加入申込書入力（麦）モデルを取得する
            D105070Model model = SessionUtil.Get<D105070Model>(SESS_D105070, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            model.SearchCondition.ApplyInput(dispModel.SearchCondition);

            // ２．１．検索結果クリア
            // 保持している検索結果をクリアする。

            model.MessageArea1 = string.Empty;
            ModelState.Clear();

            // ２．２．検索条件チェック
            // ２．２．１．エラーの場合、エラーメッセージを設定する。	
            // (1)[画面：耕地番号（開始）]と[画面：耕地番号（終了）]に値が入力されている、かつ、[画面：耕地番号（開始）]＞[画面：耕地番号（終了）] の場合、
            // エラーと判定し、エラーメッセージを「メッセージエリア１」に設定する。
            if (!string.IsNullOrEmpty(model.SearchCondition.KouchiNoFrom) &&
                !string.IsNullOrEmpty(model.SearchCondition.KouchiNoTo) &&
                int.TryParse(model.SearchCondition.KouchiNoFrom, out int nKouchiNoFrom) &&
                int.TryParse(model.SearchCondition.KouchiNoTo, out int nKouchiNoTo) &&
                nKouchiNoFrom > nKouchiNoTo)
            {
                // エラーメッセージを「メッセージエリア１」に設定する。
                model.MessageArea1 = MessageUtil.Get("ME10022", "耕地番号(終了)", "耕地番号(開始)");

                ModelState.AddModelError("MessageArea1", model.MessageArea1);
            }
            else
            {
                // (2)[画面：表示順キー１]が空欄以外、かつ、[画面：表示順キー２] と同一の場合、
                // エラーと判定し、エラーメッセージを「メッセージエリア１」に設定する。
                if (model.SearchCondition.DisplaySort1.HasValue &&
                    model.SearchCondition.DisplaySort2.HasValue &&
                    model.SearchCondition.DisplaySort1.Value == model.SearchCondition.DisplaySort2.Value)
                {
                    // エラーメッセージを「メッセージエリア１」に設定する。
                    model.MessageArea1 = MessageUtil.Get("ME10012", "表示順キー1", "表示順キー2");

                    ModelState.AddModelError("MessageArea1", model.MessageArea1);
                }
                else
                {
                    NskAppContext dbContext = getJigyoDb<NskAppContext>();
                    model.HikiukeSearchResult.SearchCondition = model.SearchCondition;

                    if (ModelState.IsValid)
                    {
                        // ２．２．２．１．引受情報を取得する。
                        model.HikiukeSearchResult.GetPageDataList(dbContext, sessionInfo, F105Const.PAGE_1);

                        // ２．２．３．検索結果のチェック
                        // ２．２．３．１．検索結果0件の場合、エラーメッセージを設定する。
                        if (model.HikiukeSearchResult.Pager.TotalCount == 0)
                        {
                            model.MessageArea1 = MessageUtil.Get("MI00011");
                            // 画面エラーメッセージエリアにメッセージ設定
                            ModelState.AddModelError("MessageArea1", MessageUtil.Get("MI00011"));
                        }

                        // ２．２．４．計算結果の算出
                        model.CalcResult.Calc(dbContext, sessionInfo, model.HikiukeSearchResult);
                    }
                }
            }

            // ３．部分ビューを構築する
            //// (1) ドロップダウンリスト項目取得
            //// No.1[２．４．]～[２．１７．] の処理を実行する。
            //model.HikiukeSearchResult.InitializeDropdonwList(dbContext, sessionInfo);

            // (2) 検索条件と検索結果をセッションに保存する。
            SessionUtil.Set(SESS_D105070, model, HttpContext);

            // (3) [２．]で取得した検索結果、計算結果を反映したモデルを使用し、部分ビューに反映させる。	
            // 共済目的に基づき、活性・非活性制御を行う。
            // (4) 部分ビューを文字列化する。
            JsonResult messageArea = PartialViewAsJson("_D105070HikiukeSearchResultMessage", model);
            JsonResult resultArea = PartialViewAsJson("_D105070HikiukeSearchResult", model);
            JsonResult calcArea = PartialViewAsJson("_D105070CalcResult", model);

            // (5) 文字列化した部分ビューをJSON化する。
            return Json(new { messageArea = messageArea.Value, resultArea = resultArea.Value, calcArea = calcArea.Value });
        }

        #region ページャーイベント
        /// <summary>
        /// 引受検索結果ページャー
        /// </summary>
        /// <param name="id">ページID</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult HikiukeResultPager(string id)
        {
            // ページIDは数値以外のデータの場合
            if (!Regex.IsMatch(id, @"^[0-9]+$") || F105Const.PAGE_0 == id)
            {
                return BadRequest();
            }

            // セッションから加入申込書入力（麦）モデルを取得する
            D105070Model model = SessionUtil.Get<D105070Model>(SESS_D105070, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // 検索結果を取得する
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            D105070SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            model.HikiukeSearchResult.GetPageDataList(dbContext, sessionInfo, int.Parse(id));

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105070, model, HttpContext);

            return PartialViewAsJson("_D105070HikiukeSearchResult", model);
        }

        /// <summary>
        /// 類別設定ページャー
        /// </summary>
        /// <param name="id">ページID</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult RuibetsuResultPager(string id)
        {
            // ページIDは数値以外のデータの場合
            if (!Regex.IsMatch(id, @"^[0-9]+$") || F105Const.PAGE_0 == id)
            {
                return BadRequest();
            }

            // セッションから加入申込書入力（麦）モデルを取得する
            D105070Model model = SessionUtil.Get<D105070Model>(SESS_D105070, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // 類別設定を取得する
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            D105070SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            model.RuibetsuSettei.GetPageDataList(dbContext, sessionInfo, int.Parse(id));

            // 結果をセッションに保存する
            SessionUtil.Set(SESS_D105070, model, HttpContext);

            return PartialViewAsJson("_D105070RuibetsuSetteiResult", model);
        }
        #endregion

        /// <summary>
        /// 引受情報入力行挿入
        /// 引受情報入力行を挿入する。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddHikiukeRow(D105070Model dispModel)
        {
            // セッションから加入申込書入力（麦）モデルを取得する
            D105070Model model = SessionUtil.Get<D105070Model>(SESS_D105070, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            List<D105070HikiukeRecord> beforeRecs = new(dispModel.HikiukeSearchResult.DispRecords);
            model.HikiukeSearchResult.ApplyInput(dispModel.HikiukeSearchResult);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // 一行追加
            model.HikiukeSearchResult.AddPageData();

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105070, model, HttpContext);

            // 追加行のインデックスを取得
            List<D105070HikiukeRecord> diff = model.HikiukeSearchResult.DispRecords.Except(beforeRecs)?.ToList() ?? new();
            D105070HikiukeRecord? addRow = diff.LastOrDefault();
            int addRowIdx = -1;
            if (addRow is not null)
            {
                addRowIdx = model.HikiukeSearchResult.DispRecords.IndexOf(addRow);
            }

            JsonResult messageArea = PartialViewAsJson("_D105070HikiukeSearchResultMessage", model);
            JsonResult resultArea = PartialViewAsJson("_D105070HikiukeSearchResult", model);

            return Json(new { addRowIdx, messageArea = messageArea.Value, resultArea = resultArea.Value });
        }

        /// <summary>
        /// 引受情報選択行削除
        /// 引受情報入力でチェックされた行を削除する。
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DelHikiukeRows(D105070Model dispModel)
        {
            // セッションから加入申込書入力（麦）モデルを取得する
            D105070Model model = SessionUtil.Get<D105070Model>(SESS_D105070, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            List<D105070HikiukeRecord> beforeRecs = new(dispModel.HikiukeSearchResult.DispRecords);
            model.HikiukeSearchResult.ApplyInput(dispModel.HikiukeSearchResult);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            model.HikiukeSearchResult.DelPageData();

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105070, model, HttpContext);

            JsonResult messageArea = PartialViewAsJson("_D105070HikiukeSearchResultMessage", model);
            JsonResult resultArea = PartialViewAsJson("_D105070HikiukeSearchResult", model);

            D105070HikiukeRecord? firstRow = model.HikiukeSearchResult.DispRecords.FirstOrDefault(x => !x.IsDelRec);
            int firstRowIdx = -1;
            if (firstRow is not null)
            {
                firstRowIdx = model.HikiukeSearchResult.DispRecords.IndexOf(firstRow);
            }
            return Json(new { firstRowIdx, messageArea = messageArea.Value, resultArea = resultArea.Value });
        }

        /// <summary>
        /// 引受情報登録
        /// 引受情報の入力内容を登録する。
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RegistHikiuke(D105070Model dispModel)
        {
            IDbContextTransaction? transaction = null;
            string errMessage = string.Empty;
            try
            {
                D105070SessionInfo sessionInfo = new();
                sessionInfo.GetInfo(HttpContext);

                // セッションから加入申込書入力（麦）モデルを取得する
                D105070Model model = SessionUtil.Get<D105070Model>(SESS_D105070, HttpContext);

                // セッションに自画面のデータが存在しない場合
                if (model is null)
                {
                    throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
                }

                // 画面入力値をセッションモデルに反映
                model.HikiukeSearchResult.ApplyInput(dispModel.HikiukeSearchResult);

                NskAppContext dbContext = getJigyoDb<NskAppContext>();
                transaction = dbContext.Database.BeginTransaction();

                int execCount = 0;

                // ３．引受情報登録処理
                // ３．１．引受情報入力の一覧から行削除されたデータがある場合、保持した耕地番号、分筆番号から削除対象レコードを取得する。
                List<D105070HikiukeRecord> hikiukeDelRecords = model.HikiukeSearchResult.GetDeleteRecs();

                if (hikiukeDelRecords.Count > 0)
                {
                    // ３．１．１．保持した耕地番号、分筆番号からt_11090_引受耕地とt_11100_引受gisの対象レコードを削除する。
                    execCount += model.HikiukeSearchResult.DeleteHikiukeGis(ref dbContext, sessionInfo, ref hikiukeDelRecords);

                    // ３．１．２．[３．１．１．]で取得した削除対象レコードの情報をt_12140_引受耕地削除データ保持テーブルに登録する。
                    model.HikiukeSearchResult.RegistHikiukeDelRec(
                        ref dbContext,
                        sessionInfo,
                        GetUserId(),
                        DateUtil.GetSysDateTime(),
                        ref hikiukeDelRecords);

                }

                // 更新対象レコード取得
                List<D105070HikiukeRecord> hikiukeUpdRecords = model.HikiukeSearchResult.GetUpdateRecs(ref dbContext, sessionInfo);

                // ３．２．引受情報入力の一覧で変更されたデータがある場合、t_11090_引受耕地とt_11100_引受gisの対象レコードを更新する。
                if (hikiukeUpdRecords.Count > 0)
                {
                    execCount += model.HikiukeSearchResult.UpdateHikiukeGis(
                        ref dbContext,
                        sessionInfo,
                        GetUserId(),
                        DateUtil.GetSysDateTime(),
                        ref hikiukeUpdRecords);

                }

                // ３．３．引受情報入力の一覧に行追加されたデータがある場合、t_11090_引受耕地とt_11100_引受gisの対象レコードを登録する。
                List<D105070HikiukeRecord> hikiukeAddRecords = model.HikiukeSearchResult.GetAddRecs();
                if (hikiukeAddRecords.Count > 0)
                {
                    execCount += model.HikiukeSearchResult.AppendHikiukeGis(
                        ref dbContext,
                        sessionInfo,
                        GetUserId(),
                        DateUtil.GetSysDateTime(),
                        ref hikiukeAddRecords);

                }


                // ３．４．削除、更新、登録のいずれかが行われたデータが1件でもある場合、[メッセージエリア２]に以下のメッセージを表示する。
                if (execCount > 0)
                {
                    errMessage = MessageUtil.Get("MI00004", "登録");
                }

                transaction.CommitAsync();

                // 検索条件と検索結果をセッションに保存する
                SessionUtil.Set(SESS_D105070, model, HttpContext);
            }
            catch (Exception ex)
            {
                transaction?.RollbackAsync();

                if (string.IsNullOrEmpty(errMessage))
                {
                    if (ex is DBConcurrencyException)
                    {
                        // ３．５．排他エラーが含まれる場合は、[メッセージエリア２]に以下のメッセージを表示する。
                        //  [変数：エラーメッセージ] にエラーメッセージを設定
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

        /// <summary>
        /// 引受情報計算
        /// 引受情報の入力内容から計算結果を算出する。
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CalcHikiuke(D105070Model dispModel)
        {
            // セッションから加入申込書入力（麦）モデルを取得する
            D105070Model model = SessionUtil.Get<D105070Model>(SESS_D105070, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // ２．画面入力値の反映
            // ２．１．セッションから入力データ（引受情報リスト）を取得する。	

            // ２．２．「２．１．」で取得した結果に画面入力値を反映する。
            model.HikiukeSearchResult.ApplyInput(dispModel.HikiukeSearchResult);

            // ３．計算処理
            // ３．１．[２．２．] で値反映したデータを元に各計算を行う。
            D105070SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            model.CalcResult.Calc(dbContext, sessionInfo, model.HikiukeSearchResult);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105070, model, HttpContext);

            return PartialViewAsJson("_D105070CalcResult", model);
        }

        /// <summary>
        /// 引受情報チェック
        /// 入力内容が妥当かチェック処理を行う。
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckHikiuke()
        {
            // １．エラーチェック処理を実行する。
            // １．１．初期処理。	
            //  [変数：エラーメッセージ] に空文字を設定する。
            string errMessage = string.Empty;
            int result = 0;
            int errCd = 0;

            D105070SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            // セッションから加入申込書入力（麦）モデルを取得する
            D105070Model model = SessionUtil.Get<D105070Model>(SESS_D105070, HttpContext);

            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            StringBuilder query = new();

            // １．２．「NSK_106032B_引受計算処理（麦）エラーチェック処理」を実行する。
            // 引数           IN/ OUT          値
            // 組合等コード     IN             [セッション：組合等コード]
            // 年産             IN             [セッション：年産]
            // 共済目的コード   IN             [セッション：共済目的コード]
            // ユーザーID       IN             [セッション：ユーザID]
            // チェック対象     IN             0：全組合員、または対象支所組合員
            // チェック対象(サブ) IN           [セッション：支所コード]
            // 処理結果         OUT            [変数：処理結果]
            // エラーコード     OUT            [変数：エラーコード]
            // エラーメッセージ OUT            [変数：エラーメッセージ]
            List<NpgsqlParameter> storedParams = [
                new("組合員等コード", sessionInfo.KumiaiintoCd),
                new("年産", sessionInfo.Nensan),
                new("共済目的コード", sessionInfo.KyosaiMokutekiCd),
                new("ユーザーID", GetUserId()),
                new("チェック対象", "0"), // 0：全組合員、または対象支所組合員
                new("チェック対象サブ", sessionInfo.ShishoCd)
            ];
            NpgsqlParameter outRet = new("ret", NpgsqlDbType.Integer) { Direction = ParameterDirection.InputOutput, Value = result };
            storedParams.Add(outRet);
            NpgsqlParameter outErrCd = new("message_id", NpgsqlDbType.Integer) { Direction = ParameterDirection.InputOutput, Value = errCd };
            storedParams.Add(outErrCd);
            NpgsqlParameter outErrMsg = new("message", NpgsqlDbType.Text) { Direction = ParameterDirection.InputOutput, Value = errMessage };
            storedParams.Add(outErrMsg);

            query.Append("CALL f_Hiki_Check_Mug000(@組合員等コード, @年産, @共済目的コード, @ユーザーID, @チェック対象, @チェック対象サブ, @ret, @message_id, @message)");
            dbContext.Database.ExecuteSqlRaw(query.ToString(), storedParams.ToArray());

            result = (int)(storedParams.SingleOrDefault(p => p.ParameterName == "ret")?.NpgsqlValue ?? 0);
            errCd = (int)(storedParams.SingleOrDefault(p => p.ParameterName == "message_id")?.NpgsqlValue ?? 0);
            errMessage = (string)(storedParams.SingleOrDefault(p => p.ParameterName == "message")?.NpgsqlValue ?? string.Empty);

            // １．３．「処理結果」「エラーメッセージ」をJSON化して返送する。
            return Json(new { result, message = errMessage });
        }

        #region 戻るイベント
        /// <summary>
        /// 戻る
        /// 加入申込書入力（水稲）組合員等検索（共通）画面へ遷移する。
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Back()
        {
            // 加入申込書入力（水稲）組合員等検索（共通）

            return Json(new { result = "success" });
        }
        #endregion

        /// <summary>
        /// 加入状況、加入形態、自動継続特約有無登録
        /// 組合員等毎の加入状況、加入形態、自動継続特約有無を登録する。
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RegistKanyuJokyo(D105070Model dispModel)
        {
            IDbContextTransaction? transaction = null;
            string errMessage = string.Empty;
            try
            {
                D105070SessionInfo sessionInfo = new();
                sessionInfo.GetInfo(HttpContext);

                // セッションから加入申込書入力（麦）モデルを取得する
                D105070Model model = SessionUtil.Get<D105070Model>(SESS_D105070, HttpContext);

                // セッションに自画面のデータが存在しない場合
                if (model is null)
                {
                    throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
                }

                // 画面入力値をセッションモデルに反映
                model.KumiaiintoSettei.ApplyInput(dispModel.KumiaiintoSettei);

                NskAppContext dbContext = getJigyoDb<NskAppContext>();
                transaction = dbContext.Database.BeginTransaction();

                int execCount = 0;

                // ３．組合員等毎の設定登録・更新
                if (!model.KumiaiintoSettei.Exists)
                {
                    // ３．１．個人設定の登録

                    // ３．１．１．組合員等毎設定でt_11010_個人設定の登録を行う。
                    execCount += model.KumiaiintoSettei.RegistKojinSettei(ref dbContext, sessionInfo, GetUserId(), DateUtil.GetSysDateTime());

                    //if (model.KumiaiintoSettei.KanyuState == F105Const.KanyuStateType.NO_MEMBER)
                    //{
                    //    // ３．１．１．１．加入状況で未加入を選択されている場合、t_11020_個人設定解除の登録を行う。
                    //    execCount += model.KumiaiintoSettei.RegistKanyuKaijo(ref dbContext, sessionInfo, GetUserId(), DateUtil.GetSysDateTime());
                    //}

                    // ３．１．２．登録が行われたデータが1件でもある場合、[メッセージエリア４]に以下のメッセージを表示する。
                    if (execCount > 0)
                    {
                        errMessage = MessageUtil.Get("MI00004", "登録");
                    }
                }
                else
                {
                    // ３．２．個人設定の更新

                    // ３．２．１．組合員等毎設定でt_11010_個人設定の更新を行う。
                    execCount += model.KumiaiintoSettei.UpdateKojinSettei(ref dbContext, sessionInfo, GetUserId(), DateUtil.GetSysDateTime());

                    //if (model.KumiaiintoSettei.KanyuState == F105Const.KanyuStateType.NO_MEMBER)
                    //{
                    //    // ３．２．１．１．加入状況で停止を選択されている場合、t_11020_個人設定解除の登録を行う。
                    //    execCount += model.KumiaiintoSettei.RegistKanyuKaijo(ref dbContext, sessionInfo, GetUserId(), DateUtil.GetSysDateTime());

                    //}

                    // ３．２．２．更新が行われたデータが1件でもある場合、[メッセージエリア４]に以下のメッセージを表示する。
                    if (execCount > 0)
                    {
                        errMessage = MessageUtil.Get("MI00004", "更新");
                    }
                }


                transaction.CommitAsync();

                // 検索条件と検索結果をセッションに保存する
                SessionUtil.Set(SESS_D105070, model, HttpContext);
            }
            catch (Exception ex)
            {
                transaction?.RollbackAsync();

                // 例外エラー
                if (string.IsNullOrEmpty(errMessage))
                {
                    if (ex is DBConcurrencyException)
                    {
                        // 排他エラーが含まれる場合は、[メッセージエリア４]に以下のメッセージを表示する。
                        //  [変数：エラーメッセージ] にエラーメッセージを設定
                        errMessage = MessageUtil.Get("ME10081");
                    }
                    else
                    {
                        //  [変数：エラーメッセージ] にエラーメッセージを設定
                        errMessage = MessageUtil.Get("MF00001");
                    }
                }
            }

            // ３．３．処理結果をJSON化して返送する。
            return Json(new { message = errMessage });
        }

        /// <summary>
        ///  類別設定情報入力行挿入
        ///  類別設定入力行を挿入する。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRuibetsuRow(D105070Model dispModel)
        {
            // セッションから加入申込書入力（麦）モデルを取得する
            D105070Model model = SessionUtil.Get<D105070Model>(SESS_D105070, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            List<D105070RuibetsuSetteiRecord> beforeRecs = new(dispModel.RuibetsuSettei.DispRecords);
            model.RuibetsuSettei.ApplyInput(dispModel.RuibetsuSettei);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();


            // 一行追加
            model.RuibetsuSettei.AddPageData();

            // [選択共済金額]ドロップダウンリスト項目を取得する。
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            D105070SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            model.RuibetsuSettei.UpdateDropdownListAll(dbContext, sessionInfo);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105070, model, HttpContext);

            // 追加行のインデックスを取得
            List<D105070RuibetsuSetteiRecord> diff = model.RuibetsuSettei.DispRecords.Except(beforeRecs)?.ToList() ?? new();
            D105070RuibetsuSetteiRecord? addRow = diff.LastOrDefault();
            int addRowIdx = -1;
            if (addRow is not null)
            {
                addRowIdx = model.RuibetsuSettei.DispRecords.IndexOf(addRow);
            }
            return Json(new { addRowIdx, view = PartialViewAsJson("_D105070RuibetsuSetteiResult", model).Value });
        }

        /// <summary>
        /// 類別設定情報選択行削除
        /// 類別設定入力でチェックされた行を削除する。
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DelRuibetsuRows(D105070Model dispModel)
        {
            // セッションから加入申込書入力（麦）モデルを取得する
            D105070Model model = SessionUtil.Get<D105070Model>(SESS_D105070, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            List<D105070RuibetsuSetteiRecord> beforeRecs = new(dispModel.RuibetsuSettei.DispRecords);
            model.RuibetsuSettei.ApplyInput(dispModel.RuibetsuSettei);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // 選択行を削除する
            model.RuibetsuSettei.DelPageData();

            // [選択共済金額]ドロップダウンリスト項目を取得する。
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            D105070SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            model.RuibetsuSettei.UpdateDropdownListAll(dbContext, sessionInfo);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105070, model, HttpContext);

            //return PartialViewAsJson("_D105070RuibetsuSetteiResult", model);
            D105070RuibetsuSetteiRecord? firstRow = model.RuibetsuSettei.DispRecords.FirstOrDefault(x => !x.IsDelRec);
            int firstRowIdx = -1;
            if (firstRow is not null)
            {
                firstRowIdx = model.RuibetsuSettei.DispRecords.IndexOf(firstRow);
            }
            return Json(new { firstRowIdx, view = PartialViewAsJson("_D105070RuibetsuSetteiResult", model).Value });
        }

        /// <summary>
        /// 類別設定情報登録
        /// 類別設定入力の入力内容を登録する。
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RegistRuibetsu(D105070Model dispModel)
        {
            IDbContextTransaction? transaction = null;
            string errMessage = string.Empty;
            try
            {
                D105070SessionInfo sessionInfo = new();
                sessionInfo.GetInfo(HttpContext);

                // セッションから加入申込書入力（麦）モデルを取得する
                D105070Model model = SessionUtil.Get<D105070Model>(SESS_D105070, HttpContext);

                // セッションに自画面のデータが存在しない場合
                if (model is null)
                {
                    throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
                }

                // 画面入力値をセッションモデルに反映
                model.RuibetsuSettei.ApplyInput(dispModel.RuibetsuSettei);

                NskAppContext dbContext = getJigyoDb<NskAppContext>();
                transaction = dbContext.Database.BeginTransaction();

                int execCount = 0;

                // ３．１．１．類別設定の一覧から行削除されたデータがある場合、t_11030_個人設定類の対象レコードを削除する。
                List<D105070RuibetsuSetteiRecord> delRecords = model.RuibetsuSettei.GetDeleteRecs();

                if (delRecords.Count > 0)
                {
                    execCount += model.RuibetsuSettei.DeleteRuibetsu(ref dbContext, sessionInfo, ref delRecords);

                }

                // ３．１．２．類別設定の一覧で変更されたデータがある場合、t_11030_個人設定類の対象レコードを更新する。
                // 更新対象レコード取得
                List<D105070RuibetsuSetteiRecord> ruibetsuUpdRecords = model.RuibetsuSettei.GetUpdateRecs(ref dbContext, sessionInfo);
                if (ruibetsuUpdRecords.Count > 0)
                {
                    execCount += model.RuibetsuSettei.UpdateRuibetsu(
                        ref dbContext,
                        sessionInfo,
                        GetUserId(),
                        DateUtil.GetSysDateTime(),
                        ref ruibetsuUpdRecords);

                }

                // ３．１．３．類別設定の一覧に行追加されたデータがある場合、t_11030_個人設定類の対象レコードを登録する。
                List<D105070RuibetsuSetteiRecord> ruibetsuAddRecords = model.RuibetsuSettei.GetAddRecs();
                if (ruibetsuAddRecords.Count > 0)
                {
                    execCount += model.RuibetsuSettei.AppendRuibetsu(
                        ref dbContext,
                        sessionInfo,
                        GetUserId(),
                        DateUtil.GetSysDateTime(),
                        ref ruibetsuAddRecords);

                }


                // ３．２．削除、更新、登録のいずれかが行われたデータが1件でもある場合、[メッセージエリア５]に以下のメッセージを表示する。
                if (execCount > 0)
                {
                    errMessage = MessageUtil.Get("MI00004", "登録");
                }


                transaction.CommitAsync();

                // 検索条件と検索結果をセッションに保存する
                SessionUtil.Set(SESS_D105070, model, HttpContext);
            }
            catch (Exception ex)
            {
                transaction?.RollbackAsync();

                if (string.IsNullOrEmpty(errMessage))
                {
                    if (ex is DBConcurrencyException)
                    {
                        // 排他エラーが含まれる場合は、[メッセージエリア２]に以下のメッセージを表示する。
                        //  [変数：エラーメッセージ] にエラーメッセージを設定
                        errMessage = MessageUtil.Get("ME10081");
                    }
                    else
                    {
                        //  [変数：エラーメッセージ] にエラーメッセージを設定
                        errMessage = MessageUtil.Get("MF00001");
                    }
                }
            }

            // ３．３．処理結果をJSON化して返送する。
            return Json(new { message = errMessage });
        }

        /// <summary>
        /// 地域集団名更新
        /// 地域集団コードに該当する氏名または法人名を表示する。
        /// </summary>
        /// <param name="chiikiSyudanCd"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateChiikiSyudanName(string chiikiSyudanCd)
        {
            // ２．氏名または法人名を取得
            // ２．１．「地域集団コード」に該当する農業者情報テーブルから「氏名または法人名」を取得する。
            D105070SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            VNogyosha? nogyosha = dbContext.VNogyoshas.SingleOrDefault(x => x.KumiaiintoCd == chiikiSyudanCd);
            string chiikiSyudanNm = nogyosha?.HojinFullNm ?? string.Empty;

            // ２．２．「地域集団名」をJSON化して返送する。
            return Json(new { chiikiSyudanNm });
        }

        /// <summary>
        /// 統計地域危険段階区分設定
        /// 加入申込書入力（麦）統計地域危険段階区分設定に遷移する。													
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MoveTokeiChiikiKikenDankaiKubunSettei(string guid)
        {
            //２．選択行データをセッションに登録
            //	・類区分
            //	・引受方式
            //	・補償割合
            //	・付保割合
            //	・一筆半損特約
            //	・担い手
            //	・営農支払以外
            //	・収穫量確認方法
            //	・全相殺基準単収
            // セッションから加入申込書入力（麦）モデルを取得する
            D105070Model model = SessionUtil.Get<D105070Model>(SESS_D105070, HttpContext);
            D105070SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            D105070RuibetsuSetteiRecord? rec = model.RuibetsuSettei.DispRecords.SingleOrDefault(x => x.GUID == guid);
            string ruiKbnNm = model.RuibetsuSettei.GetRuiKbnNm(rec.RuiKbn);
            string hikiukeHoushikiNm = model.RuibetsuSettei.GetHikiukeHoushikiNm(rec.HikiukeHoushiki);

            D105070ParamModel paramModel = new()
            {
                // 「組合等コード」
                // 「都道府県コード」
                // 「年産」
                // 「共済目的」
                // 「組合員等コード」
                // 「氏名」
                // 「電話番号」
                // 「支所コード」
                // 「支所名」
                // 「市町村コード」
                // 「市町村名」
                // 「大地区コード」
                // 「大地区名」
                // 「小地区コード」
                // 「小地区名」
                // 「合併時識別コード」
                KumiaitoCd = sessionInfo.KumiaitoCd,
                TodofukenCd = sessionInfo.TodofukenCd,
                Nensan = sessionInfo.Nensan,
                KyosaiMokutekiCd = sessionInfo.KyosaiMokutekiCd,
                KumiaiintoCd = sessionInfo.KumiaiintoCd,
                FullNm = sessionInfo.FullNm,
                Tel = sessionInfo.Tel,
                ShishoCd = sessionInfo.ShishoCd,
                ShishoNm = sessionInfo.ShishoNm,
                ShichosonCd = sessionInfo.ShichosonCd,
                ShichosonNm = sessionInfo.ShichosonNm,
                DaichikuCd = sessionInfo.DaichikuCd,
                DaichikuNm = sessionInfo.DaichikuNm,
                ShochikuCd = sessionInfo.ShochikuCd,
                ShochikuNm = sessionInfo.ShochikuNm,
                GappeiShikibetsuCd = sessionInfo.GappeiShikibetsuCd,

                RuiKbn = rec?.RuiKbn,
                RuiKbnNm = ruiKbnNm,
                HikiukeHoushikiCd = rec?.HikiukeHoushiki,
                HikiukeHoushikiNm = hikiukeHoushikiNm,
                HoshoWariai = rec?.HoshoWariai,
                FuhoWariai = rec?.FuhoWariai,
                IppitsuHansonTokuyaku = rec?.IppitsuHansonTokuyaku,
                NinaiteKbn = rec?.NinaiteKbn,
                EinoShiharaiIgai = rec?.EinoShiharaiIgai ?? false,
                SyukakuryoKakuninHouhou = rec?.SyukakuryoKakuninHouhou,
                ZensousaiKijunTansyu = rec?.ZensousaiKijunTansyu
            };
            SessionUtil.Set(F105Const.SESS_D105070_PARAMS, paramModel, HttpContext);

            //３．加入申込書入力（麦）統計地域危険段階区分設定画面へリダイレクトする。	
            string redirectLink = Url.Action("Init", F105Const.SCREEN_ID_D105073, new { area = "F105" });
            return Json(new { redirectLink });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MoveYotobetsuTankaSettei(string guid)
        {
            //２．選択行データをセッションに登録
            //	・類区分
            //	・引受方式
            //	・補償割合
            //	・付保割合
            //	・一筆半損特約
            //	・担い手
            //	・営農支払以外
            //	・収穫量確認方法
            //	・全相殺基準単収
            // セッションから加入申込書入力（麦）モデルを取得する
            D105070Model model = SessionUtil.Get<D105070Model>(SESS_D105070, HttpContext);
            D105070SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            D105070RuibetsuSetteiRecord? rec = model.RuibetsuSettei.DispRecords.SingleOrDefault(x => x.GUID == guid);
            string ruiKbnNm = model.RuibetsuSettei.GetRuiKbnNm(rec.RuiKbn);
            string hikiukeHoushikiNm = model.RuibetsuSettei.GetHikiukeHoushikiNm(rec.HikiukeHoushiki);

            D105070ParamModel paramModel = new()
            {
                KumiaitoCd = sessionInfo.KumiaitoCd,
                TodofukenCd = sessionInfo.TodofukenCd,
                Nensan = sessionInfo.Nensan,
                KyosaiMokutekiCd = sessionInfo.KyosaiMokutekiCd,
                KumiaiintoCd = sessionInfo.KumiaiintoCd,
                FullNm = sessionInfo.FullNm,
                Tel = sessionInfo.Tel,
                ShishoCd = sessionInfo.ShishoCd,
                ShishoNm = sessionInfo.ShishoNm,
                ShichosonCd = sessionInfo.ShichosonCd,
                ShichosonNm = sessionInfo.ShichosonNm,
                DaichikuCd = sessionInfo.DaichikuCd,
                DaichikuNm = sessionInfo.DaichikuNm,
                ShochikuCd = sessionInfo.ShochikuCd,
                ShochikuNm = sessionInfo.ShochikuNm,
                GappeiShikibetsuCd = sessionInfo.GappeiShikibetsuCd,

                RuiKbn = rec?.RuiKbn,
                RuiKbnNm = ruiKbnNm,
                HikiukeHoushikiCd = rec?.HikiukeHoushiki,
                HikiukeHoushikiNm = hikiukeHoushikiNm,
                HoshoWariai = rec?.HoshoWariai,
                FuhoWariai = rec?.FuhoWariai,
                IppitsuHansonTokuyaku = rec?.IppitsuHansonTokuyaku,
                NinaiteKbn = rec?.NinaiteKbn,
                EinoShiharaiIgai = rec?.EinoShiharaiIgai ?? false,
                SyukakuryoKakuninHouhou = rec?.SyukakuryoKakuninHouhou,
                ZensousaiKijunTansyu = rec?.ZensousaiKijunTansyu
            };
            SessionUtil.Set(F105Const.SESS_D105070_PARAMS, paramModel, HttpContext);

            //３．加入申込書入力（麦）用途別単価設定画面へリダイレクトする。	
            string redirectLink = Url.Action("Init", F105Const.SCREEN_ID_D105074, new { area = "F105" });
            return Json(new { redirectLink });
        }

        /// <summary>
        /// 産地銘柄名更新
        /// 産地銘柄コードに該当する産地別銘柄名を表示する。													
        /// </summary>
        /// <param name="sanchiMeigaraCd"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateSanchiMeigaraName(string sanchiMeigaraCd)
        {
            // ２．産地別銘柄名称を取得
            // ２．１．m_00130_産地別銘柄名称設定テーブルから「産地別銘柄コード」に該当する「産地別銘柄名称」を取得する。
            D105070SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            M00130産地別銘柄名称設定? sanchiMeigara =
                dbContext.M00130産地別銘柄名称設定s.FirstOrDefault(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.年産 == sessionInfo.Nensan) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
                (m.産地別銘柄コード == sanchiMeigaraCd));
            string sanchiMeigaraNm = sanchiMeigara?.産地別銘柄名称 ?? string.Empty;

            // ２．２．「変数：産地別銘柄名称」をJSON化して返送する。
            return Json(new { sanchiMeigaraNm });
        }

        /// <summary>
        /// 品種名更新
        /// 品種コードに該当する品種名を表示する。													
        /// </summary>
        /// <param name="hinshuCd"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateHinshuName(string hinshuCd)
        {
            // ２．品種名を取得
            // ２．１．m_00110_品種係数テーブルから「品種コード」に該当する「品種名等」を取得する。
            D105070SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            M00110品種係数? hinshu =
                dbContext.M00110品種係数s.FirstOrDefault(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.年産 == sessionInfo.Nensan) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
                (m.品種コード == hinshuCd));
            string hinsyuNm = hinshu?.品種名等 ?? string.Empty;

            // ２．２．「変数：品種名」をJSON化して返送する。
            return Json(new { hinsyuNm });
        }

        /// <summary>
        /// 市町村名更新
        /// 市町村コードに該当する市町村名を表示する。													
        /// </summary>
        /// <param name="shichosonCd"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateShichosonName(string shichosonCd)
        {
            // ２．市町村名を取得
            // ２．１．名称M市町村テーブルから「市町村コード」に該当する「市町村名」を取得する。
            D105070SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            VShichosonNm? shichoson =
                dbContext.VShichosonNms.FirstOrDefault(m =>
                (m.KumiaitoCd == sessionInfo.KumiaitoCd) &&
                (m.TodofukenCd == sessionInfo.TodofukenCd) &&
                (m.ShichosonCd == shichosonCd));
            string shichosonNm = shichoson?.ShichosonNm ?? string.Empty;

            // ２．２．「変数：市町村名」をJSON化して返送する。
            return Json(new { shichosonNm });
        }

        /// <summary>
        /// 共通申請割引方法更新
        /// 共通申請割引方法に応じた割引、割増割合、金額を表示する。
        /// </summary>
        /// <param name="kyotuShinseiWaribikiHouhoCd">共通申請割引方法コード</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateWaribikiHouhou(string kyotuShinseiWaribikiHouhoCd)
        {
            // セッションから加入申込書入力（麦）モデルを取得する
            D105070Model model = SessionUtil.Get<D105070Model>(SESS_D105070, HttpContext);

            D105070SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            NskAppContext dbContext = getJigyoDb<NskAppContext>();

            model.KumiaiintoSettei.KyotuShinseiWaribikiHouho = kyotuShinseiWaribikiHouhoCd;
            model.KumiaiintoSettei.GetWaribikiHouhou(dbContext, sessionInfo);

            return Json(new
            {
                waribikiWariai = $"{model.KumiaiintoSettei.WaribikiWariai}",
                waribikiKingaku = $"{model.KumiaiintoSettei.WaribikiKingaku}",
                warimashiWariai = $"{model.KumiaiintoSettei.WarimashiWariai}",
                warimashiKingaku = $"{model.KumiaiintoSettei.WarimashiKingaku}",
                kyotuShinseitoWaribikiRiyu = $"{model.KumiaiintoSettei.KyotuShinseitoWaribikiRiyu}"
            });
        }
    }
}
