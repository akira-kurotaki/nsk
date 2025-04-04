using System.Data;
using System.Text.RegularExpressions;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Utility;
using CoreLibrary.Core.Validator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ModelLibrary.Models;
using NskAppModelLibrary.Context;
using NskWeb.Areas.F105.Consts;
using NskWeb.Areas.F105.Models.D105190;

namespace NskWeb.Areas.F105.Controllers
{
    [AllowAnonymous]
    [ExcludeAuthCheck]
    [Area("F105")]
    public class D105190Controller : CoreController
    {
        /// <summary>
        /// セッションキー(D105190)
        /// </summary>
        private const string SESS_D105190 = $"{F105Const.SCREEN_ID_D105190}_SCREEN";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public D105190Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        // GET: F105/D105190
        public ActionResult Index()
        {
            if (ConfigUtil.Get(CoreConst.D0000_DISPLAY_FLAG) == "true")
            {
                // 画面表示モードを設定
                SetScreenModeFromQueryString();

            }

            return RedirectToAction("Init", F105Const.SCREEN_ID_D105190, new { area = "F105" });
        }

        /// <summary>
        /// 初期表示
        /// </summary>
        /// <returns>加入申込書入力（水稲）画面表示結果</returns>
        public ActionResult Init()
        {

            // １．ログインユーザの参照・更新可否判定
            D105190Model model = SessionUtil.Get<D105190Model>(SESS_D105190, HttpContext);

            if (model is not null)
            {
                // セッション検索条件 あり
                // 検索結果をセッションから削除
                SessionUtil.Remove(SESS_D105190, HttpContext);
            }

            D105190SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            model = new(Syokuin, sessionInfo.ShishoList);

            // １．１．権限チェック
            // (1)	ログインユーザの権限が「参照」「更新権限」いずれも許可されていない場合、メッセージを設定し業務エラー画面を表示する。
            bool dispKengen = ScreenSosaUtil.CanReference(F105Const.SCREEN_ID_D105190, HttpContext);
            bool updKengen = ScreenSosaUtil.CanUpdate(F105Const.SCREEN_ID_D105190, HttpContext);

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

            NskAppContext dbContext = getJigyoDb<NskAppContext>();

            // ２．画面表示情報をDBから取得
            // ２．１．「共済目的名称」を取得する。
            model.KyosaiMokuteki = dbContext.M00010共済目的名称s.SingleOrDefault(x =>
                (x.共済目的コード == sessionInfo.KyosaiMokutekiCd))?.共済目的名称 ?? string.Empty;

            // ２．２．「類区分情報リスト」を取得する。
            model.SearchCondition.RuiKbnList.AddRange(dbContext.M00020類名称s.Where(m =>
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd))?.
                OrderBy(m => m.類区分).
                Select(m => new SelectListItem($"{m.類区分} {m.類名称}", $"{m.類区分}")));
            SelectListItem selected = model.SearchCondition.RuiKbnList.First();
            model.SearchCondition.RuiKbn = selected.Value;
            
            // ２．３．「支所情報リスト」を取得する。]
            // 共通部品にて支所は取得されるため取得不要

            // ３．画面項目設定
            // ３．１．「２．」で取得した値を設定する。
            // 「２．」で取得した値は取得時にそのまま設定
            // ポータルから連係された値を設定。
            model.KyosaiMokutekiCd = sessionInfo.KyosaiMokutekiCd;
            model.Nensan = $"{sessionInfo.Nensan}";

            // ３．２．[画面：表示数]、[画面：表示順１]、[画面：表示順２]、[画面：表示順３]を設定する。
            // 共通部品にて設定

            // ４～６は設計誤りのためコメントアウト

            // ４．セッションから検索条件を取得する。
            // ４．１．セッションに検索条件がない場合
            // ４．１．1．検索結果を表示する「■共済金額設定」カテゴリの画面項目を非表示にし、処理を終了する。
            // ※■共済金額設定」カテゴリの画面項目を非表示にすると戻るボタンも非表示することとなる。

            // ４．２．セッションに検索条件がある場合、処理を続行する。
            // ５．画面項目再設定
            // ５．１．「４．」で取得した値を設定する。

            // ６．「検索ボタン」イベントを実施する。
            //model.SearchResult.SearchCondition = model.SearchCondition;
            //model.SearchResult.GetPageDataList(dbContext, sessionInfo, F105Const.PAGE_1);

            // 検索結果からメッセージ設定
            //if (model.SearchResult.AllRecCount == 0)
            //{
            //    // エラーメッセージを「メッセージエリア２」に設定する。
            //    model.MessageArea2 = MessageUtil.Get("MI00011");
            //    ModelState.AddModelError("MessageArea2", model.MessageArea2);
            //}

            // セッションに保存する
            SessionUtil.Set(SESS_D105190, model, HttpContext);

            ModelState.Clear();

            // 加入申込書入力組合員等検索（共通）画面を表示する
            return View(F105Const.SCREEN_ID_D105190, model);
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

            // セッションから共済金額設定モデルを取得する
            D105190Model model = SessionUtil.Get<D105190Model>(SESS_D105190, HttpContext);

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

            // 検索結果を取得する
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            D105190SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            model.SearchResult.GetPageDataList(dbContext, sessionInfo, int.Parse(id));

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105190, model, HttpContext);

            return PartialViewAsJson("_D105190SearchResult", model);
        }
        #endregion

        /// <summary>
        /// 共済金額設定を検索する。													
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Search(D105190Model dispModel)
        {
            D105190SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            // セッションから加入申込書入力（水稲）モデルを取得する
            D105190Model model = SessionUtil.Get<D105190Model>(SESS_D105190, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 検索結果クリア
            model.SearchResult = new(model.SearchCondition);
            // メッセージクリア
            model.MessageArea1 = string.Empty;
            model.MessageArea2 = string.Empty;
            ModelState.Clear();

            // ２．入力チェック
            // ２．１．属性チェック
            // 画面入力値をセッションモデルに反映
            model.SearchCondition.ApplyInput(dispModel.SearchCondition);
            // 必須チェック用クラス
            InputRequiredAttribute isInputRequired = new InputRequiredAttribute();

            if (!isInputRequired.IsValid(model.SearchCondition.RuiKbn))
            {
                // エラーメッセージを「メッセージエリア１」に設定する。
                model.MessageArea1 = isInputRequired.FormatErrorMessage("類区分");
                ModelState.AddModelError("MessageArea1", model.MessageArea1);
            }
            // 組合員等コード（開始）
            if (!string.IsNullOrEmpty(model.SearchCondition.KumiaiinToCdFrom))
            {
                // 入力ありの場合属性チェック
                // 半角数値チェック用クラス
                NumericAttribute isNumeric = new NumericAttribute();
                // 桁数チェック用クラス
                WithinDigitLengthAttribute isDigitLength = new WithinDigitLengthAttribute(13);

                if (!isNumeric.IsValid(model.SearchCondition.KumiaiinToCdFrom))
                {
                    // 半角数値チェック
                    // エラーメッセージを「メッセージエリア１」に設定する。
                    model.MessageArea1 = isNumeric.FormatErrorMessage("組合員等コード（開始）");
                    ModelState.AddModelError("MessageArea1", model.MessageArea1);
                } 
                else if (!isDigitLength.IsValid(model.SearchCondition.KumiaiinToCdFrom))
                {
                    // 桁数チェック
                    // エラーメッセージを「メッセージエリア１」に設定する。
                    model.MessageArea1 = isDigitLength.FormatErrorMessage("組合員等コード（開始）");
                    ModelState.AddModelError("MessageArea1", model.MessageArea1);
                }
            }
            // 組合員等コード（終了）
            if (!string.IsNullOrEmpty(model.SearchCondition.KumiaiinToCdTo))
            {
                // 入力ありの場合属性チェック
                // 半角数値チェック用クラス
                NumericAttribute isNumeric = new NumericAttribute();
                // 桁数チェック用クラス
                WithinDigitLengthAttribute isDigitLength = new WithinDigitLengthAttribute(13);

                if (!isNumeric.IsValid(model.SearchCondition.KumiaiinToCdTo))
                {
                    // 半角数値チェック
                    // エラーメッセージを「メッセージエリア１」に設定する。
                    model.MessageArea1 = isNumeric.FormatErrorMessage("組合員等コード（終了）");
                    ModelState.AddModelError("MessageArea1", model.MessageArea1);
                }
                else if (!isDigitLength.IsValid(model.SearchCondition.KumiaiinToCdTo))
                {
                    // 桁数チェック
                    // エラーメッセージを「メッセージエリア１」に設定する。
                    model.MessageArea1 = isDigitLength.FormatErrorMessage("組合員等コード（終了）");
                    ModelState.AddModelError("MessageArea1", model.MessageArea1);
                }
            }

            // ２．２．独自チェック
            // ２．２．１．[画面：小地区（終了）]<[画面：小地区（開始）]の場合、
            // エラーと判定し「メッセージリスト」にメッセージを設定する。
            if ((!string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom) &&
                (!string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShochikuCdTo))) &&
                (model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom.CompareTo(
                 model.SearchCondition.TodofukenDropDownList.ShochikuCdTo) > 0))
            {
                // エラーメッセージを「メッセージエリア１」に設定する。
                model.MessageArea1 = MessageUtil.Get("ME10020", "小地区");

                ModelState.AddModelError("MessageArea1", model.MessageArea1);
            }

            // (２．２．２．[画面：組合員等コード（終了）]<[画面：組合員等コード（開始）]の場合、
            // エラーと判定し「メッセージリスト」にメッセージを設定する。
            if ((!string.IsNullOrEmpty(model.SearchCondition.KumiaiinToCdFrom) &&
                (!string.IsNullOrEmpty(model.SearchCondition.KumiaiinToCdTo))) &&
                (model.SearchCondition.KumiaiinToCdFrom.CompareTo(
                 model.SearchCondition.KumiaiinToCdTo) > 0))
            {
                // エラーメッセージを「メッセージエリア１」に設定する。
                model.MessageArea1 = MessageUtil.Get("ME10020", "組合員等コード");

                ModelState.AddModelError("MessageArea1", model.MessageArea1);
            }


            // ２．２．３．［画面：表示順］の選択値に重複がある場合（以下のいずれかの条件に該当する場合）、
            // エラーと判定し「メッセージリスト」にメッセージを設定する。
            List<D105190SearchCondition.DisplaySortType> sortList = new List<D105190SearchCondition.DisplaySortType>();

            // ［画面：表示順］が選択されている場合、配列に格納
            if (model.SearchCondition.DisplaySort1.HasValue)
            {
                sortList.Add(model.SearchCondition.DisplaySort1.Value);
            }

            if (model.SearchCondition.DisplaySort2.HasValue)
            {
                sortList.Add(model.SearchCondition.DisplaySort2.Value);
            }

            if (model.SearchCondition.DisplaySort3.HasValue)
            {
                sortList.Add(model.SearchCondition.DisplaySort3.Value);
            }

            // 配列に格納した表示順の件数と重複除外した件数が一致するか判定し
            // 判定しない場合は重複と判断しエラーと判定する
            bool hasDuplicates = sortList.Count != sortList.Distinct().Count();

            if (hasDuplicates)
            {
                // エラーメッセージを「メッセージエリア１」に設定する。
                model.MessageArea1 = MessageUtil.Get("ME90018", "表示順");
                ModelState.AddModelError("MessageArea1", model.MessageArea1);
            }

            // 表示数の設定
            model.SearchResult.DisplayCount = model.SearchCondition.DisplayCount ?? CoreConst.PAGE_SIZE; ;

            // ２．３．「メッセージリスト」のメッセージの有無で下記の処理を行う。
            if (ModelState.IsValid)
            {
                // ３．データ検索SQLを実行（ログ出力：あり）
                // ３．１．「検索結果情報リスト」を取得する。
                NskAppContext dbContext = getJigyoDb<NskAppContext>();
                model.SearchResult.GetPageDataList(dbContext, sessionInfo, F105Const.PAGE_1);

                // ４．検索結果の表示
                if (model.SearchResult.AllRecCount == 0)
                {
                    // エラーメッセージを「メッセージエリア２」に設定する。
                    model.MessageArea2 = MessageUtil.Get("MI00011");
                    ModelState.AddModelError("MessageArea2", model.MessageArea2);
                }

            }

            // ５．検索条件、検索結果件数の保持
            // ５．１．検索条件、検索結果件数をセッションに保存する。
            SessionUtil.Set(SESS_D105190, model, HttpContext);

            JsonResult resultArea = PartialViewAsJson("_D105190SearchResult", model);
            // ６．画面操作制御区分による表示
            // ６．１．画面操作制御区分により、下表の画面項目の活性制御を行う。

            return Json(new { resultArea = resultArea.Value, messageArea1 = ModelState["MessageArea1"], messageArea2 = ModelState["MessageArea2"] });
        }


        /// <summary>
        /// 共済金額設定行挿入
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SystemException"></exception>
        public ActionResult AddKyousaiKingakuRow(D105190Model dispModel)
        {
            // セッションから共済金額設定モデルを取得する
            D105190Model model = SessionUtil.Get<D105190Model>(SESS_D105190, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            List<D105190ResultRecord> beforeRecs = new(dispModel.SearchResult.DispRecords);
            model.SearchResult.ApplyInput(dispModel.SearchResult);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // １．[画面：明細：行選択]チェックボックスがオンになっている明細行の次行に空白行を追加する。
            // オン状態の[画面：明細：行選択]チェックボックスがない場合は、明細行の最後に空白行を追加する。空白行の[画面：明細：行追加フラグ（非表示）]をtrueとする。
            // オン状態の[画面：明細：行選択]チェックボックス複数存在する場合は、なにもしない。
            model.SearchResult.AddPageData();

            // 追加行のインデックスを取得
            List<D105190ResultRecord> diff = model.SearchResult.DispRecords.Except(beforeRecs)?.ToList() ?? new();
            D105190ResultRecord addRow = diff.FirstOrDefault();
            int addRowIdx = model.SearchResult.DispRecords.IndexOf(addRow);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105190, model, HttpContext);

            // ２．フォーカスは追加行の先頭項目に当てる。
            return Json(new { addRowIdx, resultArea = PartialViewAsJson("_D105190SearchResult", model) });
        }

        /// <summary>
        /// 共済金額設定行削除
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DelKyousaiKingakuRows(D105190Model dispModel)
        {
            // セッションから共済金額設定モデルを取得する
            D105190Model model = SessionUtil.Get<D105190Model>(SESS_D105190, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            model.SearchResult.ApplyInput(dispModel.SearchResult);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // 共済金額設定の選択レコード（行）を削除する
            model.SearchResult.DelPageData();

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105190, model, HttpContext);

            // 先頭行のインデックス取得
            D105190ResultRecord? firstRow = model.SearchResult.DispRecords.FirstOrDefault(x => !x.IsDelRec);
            int firstRowIdx = -1;
            if (firstRow is not null)
            {
                firstRowIdx = model.SearchResult.DispRecords.IndexOf(firstRow);
            }

            // フォーカスは先頭行の先頭項目に当てる
            return Json(new { firstRowIdx, resultArea = PartialViewAsJson("_D105190SearchResult", model) });
        }


        #region 登録イベント

        /// <summary>
        /// 共済金額設定を登録
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert(D105190Model dispModel)
        {
            IDbContextTransaction? transaction = null;
            string errMessage = string.Empty;
            try
            {
                D105190SessionInfo sessionInfo = new();
                sessionInfo.GetInfo(HttpContext);

                // セッションから加入申込書入力（麦）危険段階区分設定モデルを取得する
                D105190Model model = SessionUtil.Get<D105190Model>(SESS_D105190, HttpContext);

                // セッションに自画面のデータが存在しない場合
                if (model is null)
                {
                    throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
                }

                // 画面入力値をセッションモデルに反映
                model.SearchResult.ApplyInput(dispModel.SearchResult);

                // ４．１．属性チェック
                // 必須チェック用クラス
                InputRequiredAttribute isInputRequired = new InputRequiredAttribute();

                foreach (D105190ResultRecord dispRec in dispModel.SearchResult.DispRecords)
                {
                    if (dispRec.IsDelRec)
                    {
                        continue;
                    }

                    // 半角数値チェック用クラス
                    NumericAttribute isNumeric = new NumericAttribute();
                    // 桁数チェック用クラス
                    WithinDigitLengthAttribute isDigitLength = new WithinDigitLengthAttribute(13);

                    // 組合員等コード
                    if (!isInputRequired.IsValid(dispRec.KumiaiintoCd))
                    {
                        // 必須チェック
                        // エラーメッセージを「メッセージエリア２」に設定する。                        
                        errMessage = isInputRequired.FormatErrorMessage("組合員等コード");
                        model.MessageArea2 = errMessage;
                        ModelState.AddModelError("MessageArea2", errMessage);
                        throw new AppException("ME00001", errMessage);
                    }
                    else if (!isNumeric.IsValid(dispRec.KumiaiintoCd))
                    {
                        // 半角数値チェック
                        // エラーメッセージを「メッセージエリア２」に設定する。
                        errMessage = isNumeric.FormatErrorMessage("組合員等コード");
                        model.MessageArea2 = errMessage;
                        ModelState.AddModelError("MessageArea2", errMessage);
                        throw new AppException("ME00003", errMessage);
                    }
                    else if (!isDigitLength.IsValid(dispRec.KumiaiintoCd))
                    {
                        // 桁数チェック
                        // エラーメッセージを「メッセージエリア２」に設定する。
                        errMessage = isDigitLength.FormatErrorMessage("組合員等コード");
                        model.MessageArea2 = errMessage;
                        ModelState.AddModelError("MessageArea2", errMessage);
                        throw new AppException("ME00016", errMessage);
                    }


                    // 共済金額
                    if (!isInputRequired.IsValid(dispRec.KyousaiKingaku))
                    {
                        // エラーメッセージを「メッセージエリア２」に設定する。
                        errMessage = isInputRequired.FormatErrorMessage("共済金額");
                        model.MessageArea2 = errMessage;
                        ModelState.AddModelError("MessageArea2", errMessage);
                        throw new AppException("ME00001", errMessage);
                    }
                    else if (!isNumeric.IsValid(dispRec.KyousaiKingaku))
                    {
                        // 半角数値チェック
                        // エラーメッセージを「メッセージエリア２」に設定する。
                        errMessage = isNumeric.FormatErrorMessage("共済金額");
                        model.MessageArea2 = errMessage;
                        ModelState.AddModelError("MessageArea2", errMessage);
                        throw new AppException("ME00003", errMessage);
                    }
                    else if (!isDigitLength.IsValid(dispRec.KyousaiKingaku))
                    {
                        // 桁数チェック
                        // エラーメッセージを「メッセージエリア２」に設定する。
                        errMessage = isDigitLength.FormatErrorMessage("共済金額");
                        model.MessageArea2 = errMessage;
                        ModelState.AddModelError("MessageArea2", errMessage);
                        throw new AppException("ME00016", errMessage);
                    }

                }

                // ５．トランザクションの開始
                NskAppContext dbContext = getJigyoDb<NskAppContext>();
                transaction = dbContext.Database.BeginTransaction();

                int delCount = 0;

                // ６．削除処理（ログ出力：あり）
                // 削除対象レコードが0件以上の場合、
                List<D105190ResultRecord> delRecords = model.SearchResult.GetDeleteRecs();
                if (delRecords.Count > 0)
                {
                    // ６．１．共済金額設定削除処理
                    delCount += model.SearchResult.DeleteKyousaiKingaku(ref dbContext, sessionInfo, ref delRecords);
                }


                // ７．登録更新処理
                // 入力されているすべての明細に対して以下をおこなう

                // ７．２．更新登録の場合
                int updCount = 0;
                List<D105190ResultRecord> updRecords = model.SearchResult.GetUpdateRecs(ref dbContext, sessionInfo);
                if (updRecords.Count > 0)
                {
                    updCount += model.SearchResult.UpdateKyousaiKingaku(
                        ref dbContext,
                        sessionInfo,
                        GetUserId(),
                        DateUtil.GetSysDateTime(),
                        ref updRecords);

                }

                // ７．１．新規登録の場合
                int insCount = 0;
                List<D105190ResultRecord> kikenDankaiKbnAddRecords = model.SearchResult.GetAddRecs();
                if (kikenDankaiKbnAddRecords.Count > 0)
                {
                    insCount += model.SearchResult.AppendKyousaiKingaku(
                        ref dbContext,
                        sessionInfo,
                        GetUserId(),
                        DateUtil.GetSysDateTime(),
                        ref kikenDankaiKbnAddRecords);

                }

                // １０．結果表示
                // 削除、更新、登録のいずれかが行われたデータが1件でもある場合、[メッセージエリア2]に以下のメッセージを表示する。
                if (delCount > 0)
                {
                    errMessage = MessageUtil.Get("MI00004", "削除");
                }
                if (updCount > 0)
                {
                    errMessage = MessageUtil.Get("MI00004", "更新");
                }
                if (insCount > 0)
                {
                    errMessage = MessageUtil.Get("MI00004", "登録");
                }
                if (delCount == 0 && updCount == 0 && insCount == 0)
                {
                    errMessage = MessageUtil.Get("MI00012");
                }

                transaction.CommitAsync();

                // 検索条件と検索結果をセッションに保存する
                SessionUtil.Set(SESS_D105190, model, HttpContext);
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
                        if (!string.IsNullOrEmpty(ex.Message))
                        {
                            errMessage = MessageUtil.Get(ex.Message);
                        }
                        else
                        {
                            errMessage = MessageUtil.Get("ME10081");
                        }

                    }
                    else if(ex is DbUpdateException)
                    {
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

        #endregion


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


        /// <summary>
        /// 組合員等名更新
        /// </summary>
        /// <param name="kumiaiintoCd">組合員等コード</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateKumiaiintoName(string kumiaiintoCd)
        {
            // 組合員等名を取得 
            // t_農業者情報から「組合員等コード」に該当する「氏名又は法人名」を取得する。
            D105190SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            VNogyosha? kumiaiinto = dbContext.VNogyoshas.SingleOrDefault(x =>
                (x.TodofukenCd == sessionInfo.TodofukenCd) &&
                (x.KumiaitoCd == sessionInfo.KumiaitoCd) &&
                (x.KumiaiintoCd == kumiaiintoCd)
                );
            string kumiaiintoNm = kumiaiinto?.HojinFullNm ?? string.Empty;

            // 「組合員等名」をJSON化して返送する。
            return Json(new { kumiaiintoNm });
        }
    }
}
