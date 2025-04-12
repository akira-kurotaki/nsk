using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskWeb.Areas.F105.Consts;
using NskWeb.Areas.F105.Models.D105150;
using System.Data;
using System.Text.RegularExpressions;

namespace NskWeb.Areas.F105.Controllers
{
    [AllowAnonymous]
    [ExcludeAuthCheck]
    [Area("F105")]
    public class D105150Controller : CoreController
    {
        /// <summary>
        /// セッションキー(D105150)
        /// </summary>
        private const string SESS_D105150 = $"{F105Const.SCREEN_ID_D105150}_SCREEN";



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public D105150Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        // GET: F105/D105150
        public ActionResult Index()
        {
            if (ConfigUtil.Get(CoreConst.D0000_DISPLAY_FLAG) == "true")
            {
                // 画面表示モードを設定
                SetScreenModeFromQueryString();

            }

            return RedirectToAction("Init", F105Const.SCREEN_ID_D105150, new { area = "F105" });
        }

        /// <summary>
        /// 初期表示
        /// </summary>
        /// <returns>基準収穫量設定（災害収入、品質）画面表示結果</returns>
        public ActionResult Init()
        {
            // １．ログインユーザの参照・更新可否判定
            D105150Model model = SessionUtil.Get<D105150Model>(SESS_D105150, HttpContext);

            if (model is not null)
            {
                // セッション検索条件 あり
                SessionUtil.Remove(SESS_D105150, HttpContext);
            }

            // １．１．権限チェック
            // (1)	ログインユーザの権限が「参照」
            // 「更新権限」いずれも許可されていない場合、メッセージを設定し業務エラー画面を表示する。
            bool dispKengen = ScreenSosaUtil.CanReference(F105Const.SCREEN_ID_D105150, HttpContext);
            bool updKengen = ScreenSosaUtil.CanUpdate(F105Const.SCREEN_ID_D105150, HttpContext);
            F105Const.Authority dispAuth = F105Const.Authority.None;
            if (updKengen)
            {
                dispAuth = F105Const.Authority.Update;// "更新権限";
            }
            else if (dispKengen)
            {
                dispAuth = F105Const.Authority.ReadOnly;// "参照権限";
            }
            else
            {
                throw new AppException("ME10075", MessageUtil.Get("ME10075"));
            }


            // ２．画面表示情報をDBから取得
            D105150SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            NskAppContext dbContext = getJigyoDb<NskAppContext>();

            // ２．１．「共済目的名称」を取得する。
            string kyosaiMokutekiNm = dbContext.M00010共済目的名称s.SingleOrDefault(x =>
                (x.共済目的コード == sessionInfo.KyosaiMokutekiCd))?.共済目的名称 ?? string.Empty;

            // ２．２．「支所情報リスト」を取得する。
            List<Shisho> shishoList = new();
            shishoList.AddRange(dbContext.VShishoNms.Where(x =>
                (x.KumiaitoCd == sessionInfo.KumiaitoCd) &&
                (x.TodofukenCd == sessionInfo.TodofukenCd))?.
                OrderBy(x => x.ShishoCd).
                Select(x => new Shisho()
                {
                    TodofukenCd = x.TodofukenCd,
                    KumiaitoCd = x.KumiaitoCd,
                    ShishoCd = x.ShishoCd,
                    ShishoNm = x.ShishoNm
                }));

            model = new (Syokuin, shishoList);

            // ２．３．「類区分情報リスト」を取得する。
            // ２．４．「営農対象外フラグ情報リスト」を取得する。
            // ２．５．「産地別銘柄等コード情報リスト」を取得する。
            model.SearchCondition.InitializeDropdonwList(dbContext, sessionInfo);
            model.KijunSyukakuryoSettei.InitializeDropdonwList(dbContext, sessionInfo);

            // ３．画面項目設定
            // ３．１．「２．」で取得した値を設定する。
            model.DispKengen = dispAuth;
            model.Nensan = $"{sessionInfo.Nensan}";
            model.KyosaiMokutekiCd = sessionInfo.KyosaiMokutekiCd;
            model.KyosaiMokuteki = kyosaiMokutekiNm;


            //// ４．セッションから検索条件を取得する。
            //// ４．１．セッションに検索条件がない場合

            //// ４．２．セッションに検索条件がある場合、処理を続行する。

            //// ５．画面項目再設定
            //// ５．１．「４．」で取得した値を設定する。

            //// ６．「検索ボタン」イベントを実施する。
            //model.KijunSyukakuryoSettei.SearchCondition = model.SearchCondition;
            //model.KijunSyukakuryoSettei.GetPageDataList(dbContext, sessionInfo, F105Const.PAGE_1);

            // 結果をセッションに保存する
            SessionUtil.Set(SESS_D105150, model, HttpContext);

            ModelState.Clear();

            // 基準収穫量設定（災害収入、品質）画面を表示する
            return View(F105Const.SCREEN_ID_D105150, model);
        }

        /// <summary>
        /// 検索
        /// 検索を行う。
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Search(D105150Model dispModel)
        {
            string errMsg = string.Empty;

            // ２．入力チェック
            // ２．１．属性チェック
            // ２．２．独自チェック
            // ２．２．１．[画面：小地区（終了）]<[画面：小地区（開始）]の場合、
            // エラーと判定し「メッセージリスト」にメッセージを設定する。
            if (!string.IsNullOrEmpty(dispModel.SearchCondition.TodofukenDropDownList.ShochikuCdFrom) &&
                !string.IsNullOrEmpty(dispModel.SearchCondition.TodofukenDropDownList.ShochikuCdTo) &&
                (dispModel.SearchCondition.TodofukenDropDownList.ShochikuCdFrom.
                CompareTo(dispModel.SearchCondition.TodofukenDropDownList.ShochikuCdTo) > 0))
            {
                errMsg = MessageUtil.Get("ME10020", "小地区");
                ModelState.AddModelError("MessageArea1", errMsg);
            }
            // ２．２．２．[画面：組合員等コード（終了）]<[画面：組合員等コード（開始）]の場合、
            // エラーと判定し「メッセージリスト」にメッセージを設定する。
            if (!string.IsNullOrEmpty(dispModel.SearchCondition.KumiaiinToCdFrom) &&
                !string.IsNullOrEmpty(dispModel.SearchCondition.KumiaiinToCdTo) &&
                (dispModel.SearchCondition.KumiaiinToCdFrom.
                CompareTo(dispModel.SearchCondition.KumiaiinToCdTo) > 0))
            {
                errMsg = MessageUtil.Get("ME10020", "組合員等コード");
                ModelState.AddModelError("MessageArea1", errMsg);
            }
            // ２．２．３．[画面：産地別銘柄等コード（終了）]<[画面：産地別銘柄等コード（開始）]の場合、
            // エラーと判定し「メッセージリスト」にメッセージを設定する。
            if (!string.IsNullOrEmpty(dispModel.SearchCondition.SanchibetsuMeigaratoCdFrom) &&
                !string.IsNullOrEmpty(dispModel.SearchCondition.SanchibetsuMeigaratoCdTo) &&
                (dispModel.SearchCondition.SanchibetsuMeigaratoCdFrom.
                CompareTo(dispModel.SearchCondition.SanchibetsuMeigaratoCdTo) > 0))
            {
                errMsg = MessageUtil.Get("ME10020", "産地別銘柄等コード");
                ModelState.AddModelError("MessageArea1", errMsg);
            }
            // ２．２．４．［画面：表示順］の選択値に重複がある場合（以下のいずれかの条件に該当する場合）、
            // エラーと判定し「メッセージリスト」にメッセージを設定する。
            if (((dispModel.SearchCondition.DisplaySort1 is not null) &&
                 (dispModel.SearchCondition.DisplaySort2 is not null) &&
                 (dispModel.SearchCondition.DisplaySort3 is not null)) &&
                (dispModel.SearchCondition.DisplaySort1 == 
                 dispModel.SearchCondition.DisplaySort2 &&
                 dispModel.SearchCondition.DisplaySort1 ==
                 dispModel.SearchCondition.DisplaySort3))
            {
                errMsg = MessageUtil.Get("ME90018", "表示順");
                ModelState.AddModelError("MessageArea1", errMsg);
            }
            if (((dispModel.SearchCondition.DisplaySort1 is not null) &&
                 (dispModel.SearchCondition.DisplaySort2 is not null)) &&
                (dispModel.SearchCondition.DisplaySort1 ==
                 dispModel.SearchCondition.DisplaySort2))
            {
                errMsg = MessageUtil.Get("ME90018", "表示順");
                ModelState.AddModelError("MessageArea1", errMsg);
            }
            if (((dispModel.SearchCondition.DisplaySort1 is not null) &&
                 (dispModel.SearchCondition.DisplaySort3 is not null)) &&
                (dispModel.SearchCondition.DisplaySort1 ==
                 dispModel.SearchCondition.DisplaySort3))
            {
                errMsg = MessageUtil.Get("ME90018", "表示順");
                ModelState.AddModelError("MessageArea1", errMsg);
            }
            if (((dispModel.SearchCondition.DisplaySort2 is not null) &&
                 (dispModel.SearchCondition.DisplaySort3 is not null)) &&
                (dispModel.SearchCondition.DisplaySort2 ==
                 dispModel.SearchCondition.DisplaySort3))
            {
                errMsg = MessageUtil.Get("ME90018", "表示順");
                ModelState.AddModelError("MessageArea1", errMsg);
            }


            // セッションから基準収穫量設定（災害収入、品質）モデルを取得する
            D105150Model model = SessionUtil.Get<D105150Model>(SESS_D105150, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }
            model.KijunSyukakuryoSettei.SearchCondition.ApplyInput(dispModel.SearchCondition);
            model.KijunSyukakuryoSettei.DispRecords.Clear();
            model.KijunSyukakuryoSettei.Pager = new();
            model.KijunSyukakuryoSettei.MessageArea2 = string.Empty;

            // ２．３．「メッセージリスト」のメッセージの有無で下記の処理を行う。
            if (ModelState.IsValid)
            {
                D105150SessionInfo sessionInfo = new();
                sessionInfo.GetInfo(HttpContext);

                NskAppContext dbContext = getJigyoDb<NskAppContext>();

                // ３．データ検索SQLを実行（ログ出力：あり）
                // ３．１．「検索結果情報リスト」を取得する。
                model.KijunSyukakuryoSettei.GetPageDataList(dbContext, sessionInfo, F105Const.PAGE_1);

                // ４．検索結果の表示
                // ４．１．検索結果が0件だった場合
                if (model.KijunSyukakuryoSettei.Pager.TotalCount == 0)
                {
                    errMsg = MessageUtil.Get("MI00011");
                    // 画面エラーメッセージエリアにメッセージ設定
                    ModelState.AddModelError("MessageArea2", errMsg);
                }

                // ５．検索条件、検索結果件数の保持
                // ５．１．検索条件、検索結果件数をセッションに保存する。
                SessionUtil.Set(SESS_D105150, model, HttpContext);
            }

            return Json(new { message = errMsg, resultArea = PartialViewAsJson("_D105150KijunSyukakuryoSetteiResult", model).Value });
        }


        #region ページャーイベント
        /// <summary>
        /// ページャー
        /// </summary>
        /// <param name="id">ページID</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult KijunSyukakuryoSetteiPager(string id)
        {
            // ページIDは数値以外のデータの場合
            if (!Regex.IsMatch(id, @"^[0-9]+$") || F105Const.PAGE_0 == id)
            {
                return BadRequest();
            }

            // セッションから基準収穫量設定（災害収入、品質）モデルを取得する
            D105150Model model = SessionUtil.Get<D105150Model>(SESS_D105150, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }
            // メッセージをクリアする
            model.MessageArea1 = string.Empty;

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // 基準収穫量設定を取得する
            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            D105150SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);
            model.KijunSyukakuryoSettei.GetPageDataList(dbContext, sessionInfo, int.Parse(id));


            // 結果をセッションに保存する
            SessionUtil.Set(SESS_D105150, model, HttpContext);

            return PartialViewAsJson("_D105150KijunSyukakuryoSetteiResult", model);
        }
        #endregion

        /// <summary>
        /// 行挿入
        /// 行挿入を行う。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertLine(D105150Model dispModel)
        {
            // セッションから基準収穫量設定（災害収入、品質）モデルを取得する
            D105150Model model = SessionUtil.Get<D105150Model>(SESS_D105150, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            List<D105150KijunSyukakuryoSetteiRecord> beforeRecs = new(dispModel.KijunSyukakuryoSettei.DispRecords);
            model.KijunSyukakuryoSettei.ApplyInput(dispModel.KijunSyukakuryoSettei);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // １．[画面：明細：行選択]チェックボックスがオンになっている明細行の次行に空白行を追加する。
            // オン状態の[画面：明細：行選択]チェックボックスがない場合は、明細行の最後に空白行を追加する。空白行の[画面：明細：行追加フラグ（非表示）]をtrueとする。
            // オン状態の[画面：明細：行選択]チェックボックス複数存在する場合は、なにもしない。
            model.KijunSyukakuryoSettei.AddPageData();

            // 追加行のインデックスを取得
            List<D105150KijunSyukakuryoSetteiRecord> diff = model.KijunSyukakuryoSettei.DispRecords.Except(beforeRecs)?.ToList() ?? new();
            D105150KijunSyukakuryoSetteiRecord? addRow = diff.LastOrDefault();
            int addRowIdx = -1;
            if (addRow is not null)
            {
                addRowIdx = model.KijunSyukakuryoSettei.DispRecords.IndexOf(addRow);
            }

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105150, model, HttpContext);

            // 部分ビューを構築する
            return Json(new { addRowIdx, view = PartialViewAsJson("_D105150KijunSyukakuryoSetteiResult", model) });
        }

        /// <summary>
        /// 選択行削除
        /// 選択された行の削除を行う。
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteLine(D105150Model dispModel)
        {
            // セッションから基準収穫量設定（災害収入、品質）モデルを取得する
            D105150Model model = SessionUtil.Get<D105150Model>(SESS_D105150, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 画面入力値をセッションモデルに反映
            model.KijunSyukakuryoSettei.ApplyInput(dispModel.KijunSyukakuryoSettei);

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // １．[明細：行選択]チェックボックスが1件以上、オンの場合
            // １．１．産地別銘柄名称設定（災害収入、品質）を取得する。
            // １．１．１．「産地別銘柄名称設定（災害収入、品質）情報リスト」を取得する。

            // １．２．「削除対象産地別銘柄コードリスト」に選択された行の産地別銘柄コードを追加する。
            // １．３．選択された行を削除する。
            // １．４．行削除対象行以降にデータ入力済の明細行が存在する場合、明細行の前詰めを行う。
            model.KijunSyukakuryoSettei.DelPageData();

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D105150, model, HttpContext);

            // 部分ビューを構築する
            D105150KijunSyukakuryoSetteiRecord? firstRow = model.KijunSyukakuryoSettei.DispRecords.FirstOrDefault(x => !x.IsDelRec);
            int firstRowIdx = -1;
            if (firstRow is not null)
            {
                firstRowIdx = model.KijunSyukakuryoSettei.DispRecords.IndexOf(firstRow);
            }
            return Json(new { firstRowIdx, view = PartialViewAsJson("_D105150KijunSyukakuryoSetteiResult", model) });
        }

        /// <summary>
        /// 登録
        /// 基準収穫量設定の入力内容を登録する。
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert(D105150Model dispModel)
        {
            IDbContextTransaction? transaction = null;
            string errMessage = string.Empty;
            try
            {
                // ４．入力チェック
                // ４．１．属性チェック
                // ４．１．１．「１．１．」で実施した属性チェックと同等のチェックを実施する。
                // ４．１．２．エラーの場合、エラーが発生した各入力欄の直下にメッセージを表示し、エラーメッセージを返却、処理を終了する。
                List<int> delIdxs = new();
                foreach (D105150KijunSyukakuryoSetteiRecord dispRec in dispModel.KijunSyukakuryoSettei.DispRecords)
                {
                    if (dispRec.IsDelRec)
                    {
                        delIdxs.Add(dispModel.KijunSyukakuryoSettei.DispRecords.IndexOf(dispRec));
                    }
                }
                // 削除行のバリデーションチェックエラー
                var states = ModelState.Where(x =>
                    (x.Value.Errors.Count > 0)
                    ).Select(x => new { x.Key, x.Value.Errors }
                );
                int errCnt = states.Sum(x => x.Errors.Count);
                int delRecErrCnt = 0;
                foreach (var state in states)
                {
                    foreach (int delIdx in delIdxs)
                    {
                        string key = $"KijunSyukakuryoSettei.DispRecords[{delIdx}]";
                        if (state.Key.StartsWith(key))
                        {
                            delRecErrCnt += state.Errors.Count;
                        }
                    }
                }
                // バリデーションチェックエラーが全て削除行でのエラーの場合はエラーなしとする
                if (errCnt == delRecErrCnt)
                {
                    ModelState.Clear();
                }


                // ４．２．独自チェック
                // ４．２．１．該当レコードの規格別割合の合計値が１以外の場合は
                // [メッセージリスト２]にメッセージを設定し、処理を終了する。
                decimal sumKikakubetsuWariai = 0;
                int idx = 0;
                foreach (D105150KijunSyukakuryoSetteiRecord dispRec in dispModel.KijunSyukakuryoSettei.DispRecords)
                {
                    if (!dispRec.IsDelRec)
                    {
                        idx++;

                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai1.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai2.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai3.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai4.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai5.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai6.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai7.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai8.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai9.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai10.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai11.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai12.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai13.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai14.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai15.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai16.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai17.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai18.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai19.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai20.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai21.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai22.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai23.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai24.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai25.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai26.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai27.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai28.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai29.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai20.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai31.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai32.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai33.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai34.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai35.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai36.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai37.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai38.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai39.GetValueOrDefault(0);
                        sumKikakubetsuWariai += dispRec.KikakubetsuWariai40.GetValueOrDefault(0);
                    }

                    if (sumKikakubetsuWariai != 1)
                    {
                        errMessage = $"品質指数の規格１から規格４０までの加算値が１以外です。（{idx}行目）";// MessageUtil.Get("DUMMY_MESSAGE_ID", $"{idx}");

                        ModelState.AddModelError("MessageArea2", errMessage);
                    }
                }

                if (!ModelState.IsValid)
                {
                    errMessage = ModelState.Values.
                        FirstOrDefault(x => x.ValidationState == ModelValidationState.Invalid)?.
                        Errors.FirstOrDefault()?.ErrorMessage;
                    throw new AppException("MF00005", errMessage);
                }

                // セッションから基準収穫量設定（災害収入、品質）モデルを取得する
                D105150Model model = SessionUtil.Get<D105150Model>(SESS_D105150, HttpContext);
                // セッションに自画面のデータが存在しない場合
                if (model is null)
                {
                    errMessage = MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした");
                    throw new AppException("MF00005", errMessage);
                }

                // 画面入力値をセッションモデルに反映
                model.KijunSyukakuryoSettei.ApplyInput(dispModel.KijunSyukakuryoSettei);

                // ４．２．基準収穫量設定（災害収入、品質）に対するユーザの権限有無を確認し、権限がない場合は
                // [メッセージリスト２]にメッセージを設定し、処理を終了する。
                if (model.DispKengen != F105Const.Authority.Update)
                {
                    errMessage = MessageUtil.Get("ME90004");
                    model.KijunSyukakuryoSettei.MessageArea2 = errMessage;
                    ModelState.AddModelError("MessageArea2", errMessage);
                    throw new AppException("ME90004", errMessage);
                }


                D105150SessionInfo sessionInfo = new();
                sessionInfo.GetInfo(HttpContext);


                NskAppContext dbContext = getJigyoDb<NskAppContext>();

                // ５．トランザクションの開始
                transaction = dbContext.Database.BeginTransaction();

                int delCount = 0;
                int addCount = 0;
                int updCount = 0;

                // ６．削除処理（ログ出力：あり）
                // ※「削除対象基準収穫量設定リスト」が0件以上の場合、すべての基準収穫量設定にたいして以下を行う
                List<D105150KijunSyukakuryoSetteiRecord> delRecords = model.KijunSyukakuryoSettei.GetDeleteRecs();
                if (delRecords.Count > 0)
                {
                    // ６．１．基準収穫量設定削除処理
                    delCount += model.KijunSyukakuryoSettei.Delete(ref dbContext, sessionInfo, ref delRecords, ref errMessage);

                }

                // ７．登録更新処理（ログ出力：あり）
                // ※入力されているすべての明細に対して以下をおこなう

                // ７．１．新規登録の場合
                List<D105150KijunSyukakuryoSetteiRecord> addRecords = model.KijunSyukakuryoSettei.GetAddRecs();
                if (addRecords.Count > 0)
                {
                    // ７．１．１．基準収穫量設定登録処理
                    addCount += model.KijunSyukakuryoSettei.Insert(
                        ref dbContext,
                        sessionInfo,
                        GetUserId(),
                        DateUtil.GetSysDateTime(),
                        ref addRecords);

                }

                // ７．２．更新登録の場合
                List<D105150KijunSyukakuryoSetteiRecord> updRecords = model.KijunSyukakuryoSettei.GetUpdateRecs(ref dbContext, sessionInfo);
                if (updRecords.Count > 0)
                {
                    // ７．２．１．基準収穫量設定更新処理
                    updCount += model.KijunSyukakuryoSettei.Update(
                        ref dbContext,
                        sessionInfo,
                        GetUserId(),
                        DateUtil.GetSysDateTime(),
                        ref updRecords,
                        ref errMessage);

                }


                // ８．トランザクションのコミット
                transaction.CommitAsync();


                // １０．１．エラーの場合はエラーメッセージを[メッセージエリア２]に表示する。
                // 削除、更新、登録のいずれかが行われたデータが1件でもある場合、[メッセージエリア２]に以下のメッセージを表示する。
                if (delCount > 0)
                {
                    errMessage = MessageUtil.Get("MI00004", "削除");
                }
                if (addCount > 0)
                {
                    errMessage = MessageUtil.Get("MI00004", "登録");
                }
                if (updCount > 0)
                {
                    errMessage = MessageUtil.Get("MI00004", "更新");
                }
                if (delCount == 0 && addCount == 0 && updCount == 0)
                {
                    // 対象データなし
                    errMessage = MessageUtil.Get("MI00012");
                }

                // 検索条件と検索結果をセッションに保存する
                SessionUtil.Set(SESS_D105150, model, HttpContext);
            }
            catch (Exception ex)
            {
                // ４．３．「４．１」、「４．２．」のチェックでエラーが発生した場合
                // [画面：メッセージエリア２]に「メッセージリスト２」の値を設定し、処理を終了する。

                // ９．１．｢６．｣、または、「７．」が失敗した場合、エラーメッセージを返却し、処理を終了する。

                transaction?.RollbackAsync();

                if (string.IsNullOrEmpty(errMessage))
                {
                    if (ex is DBConcurrencyException)
                    {
                        // 排他エラーが含まれる場合は、以下のメッセージを表示する。
                        // [変数：エラーメッセージ] にエラーメッセージを設定
                        errMessage = MessageUtil.Get("ME10081");
                    }
                    else if (ex.InnerException is PostgresException pe && pe.SqlState == "23505")
                    {
                        // (1)．一意制約違反となる場合
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
            // ２．１．セッション情報から検索条件、検索結果件数をクリアする。
            SessionUtil.Remove(SESS_D105150, HttpContext);

            // ２．２．画面遷移する。
            // ２．２．１．ポータルへ遷移する。
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
            D105150SessionInfo sessionInfo = new();
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
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd) //&&
                //(m.合併時識別コード == (sessionInfo.GappeiShikibetsuCd ?? string.Empty)) &&
                //(m.類区分 == (sessionInfo.RuiKbn ?? string.Empty)) &&
                //(m.地域単位区分 == tokeiTanniChiikiCd) &&
                //(m.引受方式 == (sessionInfo.HikiukeHoushikiCd ?? string.Empty)) &&
                //(m.特約区分 == (sessionInfo.IppitsuHansonTokuyaku ?? string.Empty)) &&
                //(m.補償割合コード == (sessionInfo.HoshoWariai ?? string.Empty))
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
