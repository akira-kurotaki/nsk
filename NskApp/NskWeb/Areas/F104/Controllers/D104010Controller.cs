using NskWeb.Common.Consts;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.HelpMenu;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.CodeAnalysis;
using Pipelines.Sockets.Unofficial.Arenas;
using NskAppModelLibrary.Context;
using NskWeb.Areas.F104.Models.D104010;
using NskWeb.Areas.F000.Models.D000000;
using static NskWeb.Areas.F104.Models.D104010.D104010SearchCondition;
using CoreLibrary.Core.Pager;
using Microsoft.EntityFrameworkCore;
using CoreLibrary.Core.Extensions;
using NskAppModelLibrary.Models;
using ModelLibrary.Models;
using System.Text.RegularExpressions;
using NskWeb.Areas.F000.Models.D000999;

namespace NskWeb.Areas.F104.Controllers
{
    /// <summary>
    /// NSK_104010D_引受解除設定
    /// </summary>
    /// <remarks>
    /// 作成日：2025/01/08
    /// 作成者：
    /// </remarks>
    [ExcludeAuthCheck]
    [AllowAnonymous]
    [Area("F104")]
    public class D104010Controller : CoreController
    {
        #region メンバー定数
        /// <summary>
        /// 画面ID(D2010)
        /// </summary>
        private static readonly string SCREEN_ID_D104010 = "D104010";

        /// <summary>
        /// セッションキー(D102010)
        /// </summary>
        private static readonly string SESS_D104010 = "D104010_SCREEN";

        /// <summary>
        /// 部分ビュー名（検索結果）
        /// </summary>
        private static readonly string RESULT_VIEW_NAME = "_D104010SearchResult";

        /// <summary>
        /// ページ0
        /// </summary>
        private static readonly string PAGE_0 = "0";

        public D104010Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }
        #endregion

        // GET: F00/D2010/Init
        public ActionResult Init()
        {
            SessionUtil.Remove(SESS_D104010, HttpContext);
            ModelState.Clear();
            // ログインユーザの参照・更新可否判定
            // 画面IDをキーとして、画面マスタ、画面機能権限マスタを参照し、ログインユーザに本画面の権限がない場合は業務エラー画面を表示する。
            if (!ScreenSosaUtil.CanReference(SCREEN_ID_D104010, HttpContext))
            {
                throw new AppException("ME90003", MessageUtil.Get("ME90003"));
            }
            var pagefrom = HttpContext.Request.Query[CoreConst.SCREEN_PAGE_FROM];
            var syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, HttpContext);
            if (syokuin == null)
            {
                ModelState.AddModelError("MessageArea", MessageUtil.Get("ME01033"));
                D000999Model d000999Model = GetInitModel();
                d000999Model.UserId = "";
                return View("D000999_Pre", d000999Model);
            }

            // ログインユーザの参照・更新可否判定
            // 画面IDをキーとして、画面マスタ、画面機能権限マスタを参照し、ログインユーザに本画面の権限がない場合は業務エラー画面を表示する。
            if (!ScreenSosaUtil.CanReference(SCREEN_ID_D104010, HttpContext))
            {
                throw new AppException("ME90003", MessageUtil.Get("ME90003"));
            }

            // 利用可能な支所一覧
            var shishoList = ScreenSosaUtil.GetShishoList(HttpContext);

            //// モデル初期化
            D104010Model model = new D104010Model(Syokuin, shishoList)
            {
                // 「ログイン情報」を取得する
                VSyokuinRecords = getJigyoDb<NskAppContext>().VSyokuins.Where(t => t.UserId == Syokuin.UserId).Single()
            };

            NSKPortalInfoModel md = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            if (md != null)
            {
                model.D104010Info.SKyosaiMokutekiCd = md.SKyosaiMokutekiCd;
                model.D104010Info.SNensanHikiuke = md.SNensanHikiuke;
                model.D104010Info.SNensanHyoka = md.SNensanHyoka;
            }
            try
            {
                var kyosai = getJigyoDb<NskAppContext>().M00010共済目的名称s
                              .Where(t => t.共済目的コード == md.SKyosaiMokutekiCd)
                              .Single();

                model.KyosaiMokutekiMeisho = kyosai.共済目的名称;
            }
            catch (Exception ex)
            {
                logger.Debug(ex.StackTrace);
                throw new AppException("MF80002", MessageUtil.Get("MF80002", "共済目的"));
            }
            // 初期表示情報をセッションに保存する
            SessionUtil.Set(SESS_D104010, model, HttpContext);
            return View(SCREEN_ID_D104010, model);
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
            SessionUtil.Remove(SESS_D104010, HttpContext);

            return Json(new { result = "success" });
        }
        #endregion


        #region 検索イベント
        /// <summary>
        /// イベント名：検索
        /// </summary>
        /// <param name="model">一括帳票出力モデル</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search([Bind("SearchCondition")] D104010Model model)
        {

            NSKPortalInfoModel md = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            if (md != null)
            {
                model.D104010Info.SKyosaiMokutekiCd = md.SKyosaiMokutekiCd;
                model.D104010Info.SNensanHikiuke = md.SNensanHikiuke;
                model.D104010Info.SNensanHyoka = md.SNensanHyoka;
            }
            try
            {
                var kyosai = getJigyoDb<NskAppContext>().M00010共済目的名称s
                              .Where(t => t.共済目的コード == md.SKyosaiMokutekiCd)
                              .Single();

                model.KyosaiMokutekiMeisho = kyosai.共済目的名称;
            }
            catch (Exception ex)
            {
                logger.Debug(ex.StackTrace);
                throw new AppException("MF80002", MessageUtil.Get("MF80002", "共済目的"));
            }
            var updateKengen = ScreenSosaUtil.CanUpdate(SCREEN_ID_D104010, HttpContext);
            model.UpdateKengenFlg = updateKengen;
            var syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, HttpContext);
            model.VSyokuinRecords.ShishoCd = syokuin.ShishoCd;
            model.VSyokuinRecords.TodofukenCd = syokuin.TodofukenCd;
            model.VSyokuinRecords.KumiaitoCd = syokuin.KumiaitoCd;

            // 属性チェックまたは独自チェックエラーの場合
            if (CheckSearchCondition(model))
            {
                // 検索結果は非表示
                model.SearchCondition.IsResultDisplay = false;
                // 検索条件をセッションに保存する
                SessionUtil.Set(SESS_D104010, model, HttpContext);
                return View(SCREEN_ID_D104010, model);
            }

            // 検索して、画面に返す
            return View(SCREEN_ID_D104010, GetPageDataList(1, model));
        }
        #endregion

        #region ページ分データ取得メソッド
        /// <summary>
        /// メソッド：ページ分データを取得する
        /// </summary>
        /// <param name="pageId">ページID</param>
        /// <param name="model">ビューモデル</param>
        /// <returns>検索結果モデル</returns>
        private D104010Model GetPageDataList(int? pageId, D104010Model model)
        {
            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();
            // 検索結果をクリアする
            model.SearchResult = new D104010SearchResult();
            // 検索フラグ設定
            model.SearchCondition.IsResultDisplay = true;

            // 検索結果件数を取得する
            var totalCount = GetSearchResultCount(model);

            // 検索件数を画面表示用モデルに設定する
            model.SearchResult.TotalCount = totalCount;
            // 検索結果は0件の場合
            if (totalCount == 0)
            {
                model.MessageArea2 = MessageUtil.Get("MI00011");
                // 画面エラーメッセージエリアにメッセージ設定
                ModelState.AddModelError("MessageArea2", MessageUtil.Get("MI00011"));
                // 検索条件をセッションに保存する
                SessionUtil.Set(SESS_D104010, model, HttpContext);
                return model;
            }
            // 検索結果表示数の取得
            var displayCount = GetDisplayCount(model);
            // ページIDの取得
            var intPageId = GetPageId((pageId ?? 1), totalCount, displayCount);
            // 検索結果ページ分の取得
            model.SearchResult.TableRecords = GetPageData(model, displayCount * (intPageId - 1), displayCount);
            // ページャーの初期化
            model.SearchResult.Pager = new Pagination(intPageId, displayCount, totalCount);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D104010, model, HttpContext);

            return model;
        }
        #endregion

        #region ページャーイベント
        /// <summary>
        /// イベント名：ページャー
        /// </summary>
        /// <param name="id">ページID</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Pager(string id)
        {
            // ページIDは数値以外のデータの場合
            if (!Regex.IsMatch(id, @"^[0-9]+$") || PAGE_0 == id)
            {
                return BadRequest();
            }

            D104010Model model = SessionUtil.Get<D104010Model>(SESS_D104010, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 検索結果を取得する
            model = GetPageDataList(int.Parse(id), model);
            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D104010, model, HttpContext);
            return PartialViewAsJson(RESULT_VIEW_NAME, model);
        }
        #endregion

        #region 行追加イベント
        /// <summary>
        /// 行追加イベント
        /// </summary>
        /// <param name="dispModel">ビューモデル</param>
        /// <returns>部分ビュー</returns>
        public ActionResult InsertRow(D104010Model dispModel)
        {
            // セッションから画面モデルを取得
            D104010Model model = SessionUtil.Get<D104010Model>(SESS_D104010, HttpContext);
            if (model is null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }
            // モデル状態をクリア
            ModelState.Clear();
            var resultMessage = string.Empty;

            foreach (var record in dispModel.SearchResult.TableRecords)
            {
                if (record.rowAddFlg == "1" && string.IsNullOrEmpty(record.kumiaiintoCd))
                {
                    resultMessage = MessageUtil.Get("ME00001", "組合員等コード", "");
                    return Json(new { message = resultMessage });
                }
            }

            foreach (var dispRecord in dispModel.SearchResult.TableRecords)
            {
                D104010TableRecord sessionRecord = null;
                
                // ユーザ入力された組合員コードがあれば、これをキーとして探す
                if (dispRecord.rowAddFlg == "1" && !string.IsNullOrEmpty(dispRecord.kumiaiintoCd))
                {
                    sessionRecord = model.SearchResult.TableRecords
                                    .FirstOrDefault(r => r.kumiaiintoCd == string.Empty ||
                                                         r.kumiaiintoCd == dispRecord.kumiaiintoCd);
                }

                if (sessionRecord != null)
                {
                    // dispRecord の更新内容を sessionRecord に反映
                    sessionRecord.SelectCheck = dispRecord.SelectCheck;
                    sessionRecord.kumiaiintoCd = dispRecord.kumiaiintoCd;
                    sessionRecord.HojinFullNm = dispRecord.HojinFullNm;
                    sessionRecord.hukakinkei = dispRecord.hukakinkei;
                    sessionRecord.kaijyoRiyuCd = dispRecord.kaijyoRiyuCd;
                    sessionRecord.hikiukeKaijyoDate = dispRecord.hikiukeKaijyoDate;
                    sessionRecord.hikiukeKaijyoHenkanHukakingaku = dispRecord.hikiukeKaijyoHenkanHukakingaku;
                    sessionRecord.kanjyoMousideDate = dispRecord.kanjyoMousideDate;
                    sessionRecord.rowAddFlg = dispRecord.rowAddFlg;
                    sessionRecord.rowDeleteFlg = dispRecord.rowDeleteFlg;
                }
            }
            // 新規の空行（新規行と判別するため rowAddFlg = "1" を設定）を作成
            D104010TableRecord newRow = new D104010TableRecord
            {
                // 必要に応じて初期値を設定
                rowAddFlg = "1",
                rowDeleteFlg = "0",
                // その他のプロパティは空文字や null で初期化（必要に応じて）
                kumiaiintoCd = string.Empty,
                HojinFullNm = string.Empty,
                hukakinkei = null,
                hikiukeKaijyoDate = null,
                kanjyoMousideDate = null,
                kaijyoRiyuCd = string.Empty,
                kaijyoRiyuMeisho = string.Empty,
                hikiukeKaijyoHenkanHukakingaku = null,
                kaijyoKakuteiFlg = "0"
            };

            // リストの末尾に新規行を追加
            model.SearchResult.TableRecords.Add(newRow);

            // 変更したモデルをセッションに再設定（必要な場合）
            SessionUtil.Set(SESS_D104010, model, HttpContext);

            // 変更後の部分ビューを返す
            return PartialViewAsJson(RESULT_VIEW_NAME, model);
        }
        #endregion

        #region 選択された行削除メソッド
        /// <summary>
        /// 選択された行削除イベント
        /// </summary>
        /// <param name="dispModel">ビューモデル</param>
        /// <returns>部分ビュー</returns>
        public ActionResult DeleteSelectedRow(D104010Model dispModel)
        {
            // セッションから画面モデルを取得
            D104010Model model = SessionUtil.Get<D104010Model>(SESS_D104010, HttpContext);
            if (model is null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }
            // モデル状態をクリア
            ModelState.Clear();
            var resultMessage = string.Empty;

            // 削除対象のレコードを保持する一時リストを用意
            List<D104010TableRecord> recordsToRemove = new List<D104010TableRecord>();
            
            // dispModel の各レコードについて、選択状態が true の場合に処理する
            foreach (var dispRecord in dispModel.SearchResult.TableRecords)
            {
                if (dispRecord.SelectCheck)
                {
                    // ユーザ入力された組合員コード（または空の場合、新規追加行）でセッション側のレコードを探す
                    var sessionRecord = model.SearchResult.TableRecords
                        .FirstOrDefault(r => string.IsNullOrEmpty(r.kumiaiintoCd) || r.kumiaiintoCd == dispRecord.kumiaiintoCd);

                    if (sessionRecord != null)
                    {
                        if (dispRecord.rowAddFlg == "1")
                        {
                            // 新規追加行の場合はセッション側から削除する
                            recordsToRemove.Add(sessionRecord);
                        }
                        else
                        {
                            // 既存行の場合は削除フラグを設定する
                            sessionRecord.SelectCheck = dispRecord.SelectCheck;
                            sessionRecord.rowAddFlg = "0";
                            sessionRecord.rowDeleteFlg = "1";
                        }
                    }
                }
            }

            // 一時リストに入ったレコードをセッション側のリストから削除する
            foreach (var record in recordsToRemove)
            {
                model.SearchResult.TableRecords.Remove(record);
            }

            SessionUtil.Set(SESS_D104010, model, HttpContext);

            // 変更後の部分ビューを返す
            return PartialViewAsJson(RESULT_VIEW_NAME, model);
        }
        #endregion


        #region 検索結果表示数取得メソッド
        /// <summary>
        /// 検索結果表示数取得メソッド
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>検索結果表示数</returns>
        private int GetDisplayCount(D104010Model model)
        {
            return model.SearchCondition.DisplayCount ?? CoreConst.PAGE_SIZE;
        }
        #endregion

        #region 検索結果件数取得メソッド
        /// <summary>
        /// 検索結果件数取得メソッド
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>検索結果件数</returns>
        private int GetSearchResultCount(D104010Model model)
        {
            // 画面からの値（数値の文字列表現）が必ず半角数字として格納されている前提
            // 年産は、モデル内の文字列を事前に数値に変換しておく（EF Core による翻訳エラー回避のため）
            short nNensan = short.Parse(model.D104010Info.SNensanHikiuke);

            DbConnectionInfo dbConnectionInfo =
                DBUtil.GetDbConnectionInfo(
                    ConfigUtil.Get("SystemKbn"),
                    Syokuin.TodofukenCd,
                    Syokuin.KumiaitoCd,
                    Syokuin.ShishoCd);

            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                // WITH 句に相当するサブクエリ max_00010
                var max_00010 = db.T00010引受回s
                    .GroupBy(t => new { t.組合等コード, t.年産, t.共済目的コード, t.支所コード })
                    .Select(g => new
                    {
                        g.Key.組合等コード,
                        g.Key.年産,
                        g.Key.共済目的コード,
                        g.Key.支所コード,
                        max_引受回 = g.Max(x => x.引受回)
                    });

                // メインクエリ
                var query =
                from t12040 in db.T12040組合員等別引受情報s
                join v in db.VNogyoshas
                    on new { t12040.組合等コード, t12040.組合員等コード }
                    equals new { 組合等コード = v.KumiaitoCd, 組合員等コード = v.KumiaiintoCd }
                join t12090 in db.T12090組合員等別徴収情報s
                    on new { t12040.組合等コード, t12040.年産, t12040.共済目的コード, t12040.組合員等コード, t12040.引受回 }
                    equals new { t12090.組合等コード, t12090.年産, t12090.共済目的コード, t12090.組合員等コード, t12090.引受回 }
                join t00010 in db.T00010引受回s
                    on new { t12040.組合等コード, t12040.共済目的コード, t12040.年産, t12040.支所コード, t12040.引受回 }
                    equals new { t00010.組合等コード, t00010.共済目的コード, t00010.年産, t00010.支所コード, t00010.引受回 }
                join t11020Left in db.T11020個人設定解除s
                    on new { t12040.組合等コード, t12040.年産, t12040.共済目的コード, t12040.組合員等コード }
                    equals new { t11020Left.組合等コード, t11020Left.年産, t11020Left.共済目的コード, t11020Left.組合員等コード }
                    into leftJoin
                from t11020 in leftJoin.DefaultIfEmpty()
                join m10150 in db.M10150解除理由名称s
                    on t11020.解除理由コード equals m10150.解除理由コード
                join max in max_00010
                    on new { t12040.組合等コード, t12040.年産, t12040.共済目的コード, t12040.支所コード }
                    equals new { max.組合等コード, max.年産, max.共済目的コード, max.支所コード }

                    // ▼ WHERE句に置き換え
                where
                    t12040.組合等コード == Syokuin.KumiaitoCd &&
                    t12040.年産 == nNensan &&
                    t12040.共済目的コード == model.D104010Info.SKyosaiMokutekiCd &&

                    (string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShishoCd)
                        || t12040.支所コード == model.SearchCondition.TodofukenDropDownList.ShishoCd) &&

                    (string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.DaichikuCd)
                        || t12040.大地区コード == model.SearchCondition.TodofukenDropDownList.DaichikuCd) &&

                    (string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom)
                        || string.Compare(t12040.小地区コード, model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom) >= 0) &&
                    (string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShochikuCdTo)
                        || string.Compare(t12040.小地区コード, model.SearchCondition.TodofukenDropDownList.ShochikuCdTo) <= 0) &&

                    (string.IsNullOrEmpty(model.SearchCondition.kumiaiintoCdFrom)
                        || string.Compare(t12040.組合員等コード, model.SearchCondition.kumiaiintoCdFrom) >= 0) &&
                    (string.IsNullOrEmpty(model.SearchCondition.kumiaiintoCdTo)
                        || string.Compare(t12040.組合員等コード, model.SearchCondition.kumiaiintoCdTo) <= 0) &&

                    (model.SearchCondition.kaijyoRiyu == null
                        || t11020.解除理由コード == model.SearchCondition.kaijyoRiyu) &&

                    t12040.類区分 == "0" &&
                    t12040.引受対象フラグ == "1" &&
                    t12090.徴収金額 > 0

                // ▼ GROUP BY（匿名型で）
                group t12040 by new
                {
                    t12040.組合等コード,
                    t12040.年産,
                    t12040.共済目的コード,
                    t12040.組合員等コード
                } into grp

                select grp.Key;

                // グループ化したキー（各グループは 1 件としてカウントされる）を返す
                return query.Count();
            }
        }
        #endregion

        #region 検索結果のページ分取得メソッド
        /// <summary>
        /// 検索結果のページ分取得メソッド
        /// <summary>
        /// <param name="model">ビューモデル</param>
        /// <param name="offset">範囲指定</param>
        /// <param name="pageSize">ページ表示数</param>
        /// <returns>検索結果のページ分</returns>
        private List<D104010TableRecord> GetPageData(D104010Model model, int offset, int pageSize)
        {
            // 検索結果件数分データを取得
            var sqlResults = GetResult(model, offset, pageSize);

            return sqlResults;
        }
        #endregion

        #region 検索情報取得メソッド
        /// <summary>
        /// メソッド：検索情報を取得する
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <param name="offset">範囲指定</param>
        /// <param name="pageSize">ページ表示数</param>
        /// <returns>検索情報</returns>
        private List<D104010TableRecord> GetResult(D104010Model model, int offset = 0, int pageSize = 10)
        {
            // ※ セッション等から必要な情報を取得（例）
            DbConnectionInfo dbConnectionInfo = DBUtil.GetDbConnectionInfo(
                ConfigUtil.Get("SystemKbn"),
                Syokuin.TodofukenCd,
                Syokuin.KumiaitoCd,
                Syokuin.ShishoCd);
            short nNensan = short.Parse(model.D104010Info.SNensanHikiuke);
            string nKyosaimokutekiCd = model.D104010Info.SKyosaiMokutekiCd;
            string nKumiaitoCd = model.VSyokuinRecords.KumiaitoCd;

            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                // WITH 句に相当するサブクエリ max_00010
                var max_00010 = db.T00010引受回s
                    .GroupBy(t => new { t.組合等コード, t.年産, t.共済目的コード, t.支所コード })
                    .Select(g => new
                    {
                        g.Key.組合等コード,
                        g.Key.年産,
                        g.Key.共済目的コード,
                        g.Key.支所コード,
                        max_引受回 = g.Max(x => x.引受回)
                    });

                var query =
                from t12040 in db.T12040組合員等別引受情報s
                join v in db.VNogyoshas
                    on new { t12040.組合等コード, t12040.組合員等コード }
                    equals new { 組合等コード = v.KumiaitoCd, 組合員等コード = v.KumiaiintoCd }
                join t12090 in db.T12090組合員等別徴収情報s
                    on new { t12040.組合等コード, t12040.年産, t12040.共済目的コード, t12040.組合員等コード, t12040.引受回 }
                    equals new { t12090.組合等コード, t12090.年産, t12090.共済目的コード, t12090.組合員等コード, t12090.引受回 }
                join t00010 in db.T00010引受回s
                    on new { t12040.組合等コード, t12040.共済目的コード, t12040.年産, t12040.支所コード, t12040.引受回 }
                    equals new { t00010.組合等コード, t00010.共済目的コード, t00010.年産, t00010.支所コード, t00010.引受回 }
                join t11020Left in db.T11020個人設定解除s
                    on new { t12040.組合等コード, t12040.年産, t12040.共済目的コード, t12040.組合員等コード }
                    equals new { t11020Left.組合等コード, t11020Left.年産, t11020Left.共済目的コード, t11020Left.組合員等コード }
                    into leftJoin
                from t11020 in leftJoin.DefaultIfEmpty()
                join m10150 in db.M10150解除理由名称s
                    on t11020.解除理由コード equals m10150.解除理由コード
                join max in max_00010
                    on new { t12040.組合等コード, t12040.年産, t12040.共済目的コード, t12040.支所コード }
                    equals new { max.組合等コード, max.年産, max.共済目的コード, max.支所コード }

                where
                    t12040.組合等コード == Syokuin.KumiaitoCd &&
                    t12040.年産 == nNensan &&
                    t12040.共済目的コード == model.D104010Info.SKyosaiMokutekiCd &&
                    (string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShishoCd)
                        || t12040.支所コード == model.SearchCondition.TodofukenDropDownList.ShishoCd) &&
                    (string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.DaichikuCd)
                        || t12040.大地区コード == model.SearchCondition.TodofukenDropDownList.DaichikuCd) &&
                    (string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom)
                        || string.Compare(t12040.小地区コード, model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom) >= 0) &&
                    (string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShochikuCdTo)
                        || string.Compare(t12040.小地区コード, model.SearchCondition.TodofukenDropDownList.ShochikuCdTo) <= 0) &&
                    (string.IsNullOrEmpty(model.SearchCondition.kumiaiintoCdFrom)
                        || string.Compare(t12040.組合員等コード, model.SearchCondition.kumiaiintoCdFrom) >= 0) &&
                    (string.IsNullOrEmpty(model.SearchCondition.kumiaiintoCdTo)
                        || string.Compare(t12040.組合員等コード, model.SearchCondition.kumiaiintoCdTo) <= 0) &&
                    (model.SearchCondition.kaijyoRiyu == null
                        || t11020.解除理由コード == model.SearchCondition.kaijyoRiyu) &&
                    t12040.類区分 == "0" &&
                    t12040.引受対象フラグ == "1" &&
                    t12090.徴収金額 > 0

                // ▼ まず select で中間匿名オブジェクトを作る
                select new
                {
                    info = t12040,
                    nougyosha = v,
                    choushu = t12090,
                    kai = max,
                    reason = m10150,
                    setting = t11020,
                    hukakinkei = (t12090.徴収金額 - t12040.共済金額) < 0 ? 0 : (t12090.徴収金額 - t12040.共済金額)
                } into pre

                // ▼ GROUP BY
                group pre by new
                {
                    pre.info.組合等コード,
                    pre.info.年産,
                    pre.info.共済目的コード,
                    pre.info.組合員等コード
                } into grp

                // ▼ Dtoに変換
                select new SortableRecordDto
                {
                    Record = new D104010TableRecord
                    {
                        kumiaiintoCd = grp.Key.組合員等コード,
                        HojinFullNm = grp.FirstOrDefault().nougyosha.HojinFullNm,
                        hikiukekai = grp.FirstOrDefault().info.引受回,
                        hukakinkei = grp.FirstOrDefault().hukakinkei,
                        hikiukeKaijyoDate = grp.FirstOrDefault().info.引受解除日付,
                        kanjyoMousideDate = grp.FirstOrDefault().setting.解除申出日付,
                        kaijyoRiyuCd = grp.FirstOrDefault().setting.解除理由コード,
                        kaijyoRiyuMeisho = grp.FirstOrDefault().reason.解除理由名称,
                        hikiukeKaijyoHenkanHukakingaku = grp.FirstOrDefault().setting.引受解除返還賦課金額,
                        kaijyoKakuteiFlg =
                            (grp.FirstOrDefault().kai.max_引受回 == grp.FirstOrDefault().info.引受回 &&
                             grp.FirstOrDefault().info.解除フラグ == "1" &&
                             grp.FirstOrDefault().info.引受解除日付 != null)
                            ? "1" : "0",
                        ruikbn = grp.FirstOrDefault().info.類区分,
                        toukeiTaniChiikiCd = grp.FirstOrDefault().info.統計単位地域コード,
                        rowAddFlg = "0",
                        rowDeleteFlg = "0"
                    },
                    ShochikuCd = grp.FirstOrDefault().nougyosha.ShochikuCd,
                    VNogyosha_KumiaiintoCd = grp.FirstOrDefault().nougyosha.KumiaiintoCd,
                    DaichikuCd = grp.FirstOrDefault().nougyosha.DaichikuCd
                };


                // 動的ソートの適用
                // ※ DisplaySort1～3 のいずれかが指定されている場合、順次 OrderBy / ThenBy を適用します。
                IOrderedQueryable<SortableRecordDto> orderedQuery = null;

                // DisplaySort1 の適用
                if (model.SearchCondition.DisplaySort1.HasValue)
                {
                    switch (model.SearchCondition.DisplaySort1.Value)
                    {
                        case DisplaySortType.ShochikuCd:
                            if (model.SearchCondition.DisplaySortOrder1 == CoreConst.SortOrder.ASC)
                                orderedQuery = query.OrderBy(x => x.ShochikuCd);
                            else
                                orderedQuery = query.OrderByDescending(x => x.ShochikuCd);
                            break;
                        case DisplaySortType.KumiaiintoCd:
                            if (model.SearchCondition.DisplaySortOrder1 == CoreConst.SortOrder.ASC)
                                orderedQuery = query.OrderBy(x => x.VNogyosha_KumiaiintoCd);
                            else
                                orderedQuery = query.OrderByDescending(x => x.VNogyosha_KumiaiintoCd);
                            break;
                        case DisplaySortType.DaichikuCd:
                            if (model.SearchCondition.DisplaySortOrder1 == CoreConst.SortOrder.ASC)
                                orderedQuery = query.OrderBy(x => x.DaichikuCd);
                            else
                                orderedQuery = query.OrderByDescending(x => x.DaichikuCd);
                            break;
                    }
                }

                // DisplaySort2 の適用
                if (model.SearchCondition.DisplaySort2.HasValue)
                {
                    if (orderedQuery == null)
                    {
                        // DisplaySort1 未指定の場合は、DisplaySort2 を初回キーとして設定
                        switch (model.SearchCondition.DisplaySort2.Value)
                        {
                            case DisplaySortType.ShochikuCd:
                                if (model.SearchCondition.DisplaySortOrder2 == CoreConst.SortOrder.ASC)
                                    orderedQuery = query.OrderBy(x => x.ShochikuCd);
                                else
                                    orderedQuery = query.OrderByDescending(x => x.ShochikuCd);
                                break;
                            case DisplaySortType.KumiaiintoCd:
                                if (model.SearchCondition.DisplaySortOrder2 == CoreConst.SortOrder.ASC)
                                    orderedQuery = query.OrderBy(x => x.VNogyosha_KumiaiintoCd);
                                else
                                    orderedQuery = query.OrderByDescending(x => x.VNogyosha_KumiaiintoCd);
                                break;
                            case DisplaySortType.DaichikuCd:
                                if (model.SearchCondition.DisplaySortOrder2 == CoreConst.SortOrder.ASC)
                                    orderedQuery = query.OrderBy(x => x.DaichikuCd);
                                else
                                    orderedQuery = query.OrderByDescending(x => x.DaichikuCd);
                                break;
                        }
                    }
                    else
                    {
                        // DisplaySort1 が指定済みの場合は ThenBy を連結
                        switch (model.SearchCondition.DisplaySort2.Value)
                        {
                            case DisplaySortType.ShochikuCd:
                                if (model.SearchCondition.DisplaySortOrder2 == CoreConst.SortOrder.ASC)
                                    orderedQuery = orderedQuery.ThenBy(x => x.ShochikuCd);
                                else
                                    orderedQuery = orderedQuery.ThenByDescending(x => x.ShochikuCd);
                                break;
                            case DisplaySortType.KumiaiintoCd:
                                if (model.SearchCondition.DisplaySortOrder2 == CoreConst.SortOrder.ASC)
                                    orderedQuery = orderedQuery.ThenBy(x => x.VNogyosha_KumiaiintoCd);
                                else
                                    orderedQuery = orderedQuery.ThenByDescending(x => x.VNogyosha_KumiaiintoCd);
                                break;
                            case DisplaySortType.DaichikuCd:
                                if (model.SearchCondition.DisplaySortOrder2 == CoreConst.SortOrder.ASC)
                                    orderedQuery = orderedQuery.ThenBy(x => x.DaichikuCd);
                                else
                                    orderedQuery = orderedQuery.ThenByDescending(x => x.DaichikuCd);
                                break;
                        }
                    }
                }

                // DisplaySort3 の適用
                if (model.SearchCondition.DisplaySort3.HasValue)
                {
                    if (orderedQuery == null)
                    {
                        // DisplaySort1,2 未指定の場合は、DisplaySort3 を初回キーとして設定
                        switch (model.SearchCondition.DisplaySort3.Value)
                        {
                            case DisplaySortType.ShochikuCd:
                                if (model.SearchCondition.DisplaySortOrder3 == CoreConst.SortOrder.ASC)
                                    orderedQuery = query.OrderBy(x => x.ShochikuCd);
                                else
                                    orderedQuery = query.OrderByDescending(x => x.ShochikuCd);
                                break;
                            case DisplaySortType.KumiaiintoCd:
                                if (model.SearchCondition.DisplaySortOrder3 == CoreConst.SortOrder.ASC)
                                    orderedQuery = query.OrderBy(x => x.VNogyosha_KumiaiintoCd);
                                else
                                    orderedQuery = query.OrderByDescending(x => x.VNogyosha_KumiaiintoCd);
                                break;
                            case DisplaySortType.DaichikuCd:
                                if (model.SearchCondition.DisplaySortOrder3 == CoreConst.SortOrder.ASC)
                                    orderedQuery = query.OrderBy(x => x.DaichikuCd);
                                else
                                    orderedQuery = query.OrderByDescending(x => x.DaichikuCd);
                                break;
                        }
                    }
                    else
                    {
                        switch (model.SearchCondition.DisplaySort3.Value)
                        {
                            case DisplaySortType.ShochikuCd:
                                if (model.SearchCondition.DisplaySortOrder3 == CoreConst.SortOrder.ASC)
                                    orderedQuery = orderedQuery.ThenBy(x => x.ShochikuCd);
                                else
                                    orderedQuery = orderedQuery.ThenByDescending(x => x.ShochikuCd);
                                break;
                            case DisplaySortType.KumiaiintoCd:
                                if (model.SearchCondition.DisplaySortOrder3 == CoreConst.SortOrder.ASC)
                                    orderedQuery = orderedQuery.ThenBy(x => x.VNogyosha_KumiaiintoCd);
                                else
                                    orderedQuery = orderedQuery.ThenByDescending(x => x.VNogyosha_KumiaiintoCd);
                                break;
                            case DisplaySortType.DaichikuCd:
                                if (model.SearchCondition.DisplaySortOrder3 == CoreConst.SortOrder.ASC)
                                    orderedQuery = orderedQuery.ThenBy(x => x.DaichikuCd);
                                else
                                    orderedQuery = orderedQuery.ThenByDescending(x => x.DaichikuCd);
                                break;
                        }
                    }
                }

                // いずれも指定されていない場合は、デフォルトの並び順を設定
                if (orderedQuery == null)
                {
                    orderedQuery = query.OrderBy(x => x.Record.kumiaiintoCd);
                }

                // 最終的に匿名型から表示用の Record 部分のみを選択してページング
                var finalQuery = orderedQuery.Select(x => x.Record)
                                             .Skip(offset)
                                             .Take(pageSize);
                var result = finalQuery.ToList();
                return result;
            }
        }

        #endregion

        #region 登録メソッド
        /// <summary>
        /// 登録イベント
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>結果メッセージ</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(D104010Model model)
        {
            var resultMessage = string.Empty;
            // ※ セッションからユーザ情報などを取得（例）
            var syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, HttpContext);
            NSKPortalInfoModel md = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            if (md != null)
            {
                model.D104010Info.SKyosaiMokutekiCd = md.SKyosaiMokutekiCd;
                model.D104010Info.SNensanHikiuke = md.SNensanHikiuke;
                model.D104010Info.SNensanHyoka = md.SNensanHyoka;
            }
            // セッション等から必要な値を取得
            string sessionKumiaitoCd = syokuin.KumiaitoCd;              // 組合等コード
            short sessionNensan = short.Parse(model.D104010Info.SNensanHikiuke); // 年産
            string sessionSKyosaiMokutekiCd = model.D104010Info.SKyosaiMokutekiCd; // 共済目的コード
            string sessionUserId = syokuin.UserId;
            DateTime sysDate = DateTime.Now;
            var selectedCount = 0;

            // DB 接続情報（各環境に合わせる）
            var dbConnectionInfo = DBUtil.GetDbConnectionInfo(
                ConfigUtil.Get("SystemKbn"), syokuin.TodofukenCd, syokuin.KumiaitoCd, syokuin.ShishoCd);

            using (var db = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                var transaction = db.Database.BeginTransaction();
                try
                {
                    foreach (var record in model.SearchResult.TableRecords)
                    {
                        // チェックが付いていない行はスキップ（更新/追加の対象外）
                        if (!record.SelectCheck)
                            continue;
                        selectedCount++;
                        // t_11020_個人設定解除に対象レコードが存在するかチェック
                        var count = (from info in db.Set<T12040組合員等別引受情報>()
                                     join setting in db.Set<T11020個人設定解除>()
                                         on new { info.組合等コード, info.年産, info.共済目的コード, info.組合員等コード }
                                         equals new { setting.組合等コード, setting.年産, setting.共済目的コード, setting.組合員等コード }
                                     where info.組合等コード == sessionKumiaitoCd &&
                                           info.年産 == sessionNensan &&
                                           info.共済目的コード == sessionSKyosaiMokutekiCd &&
                                           info.引受回 == record.hikiukekai &&
                                           info.組合員等コード == record.kumiaiintoCd &&
                                           info.類区分 == "0" &&
                                           info.引受対象フラグ == "1" &&
                                           info.共済掛金 > 0
                                     select setting).Count();

                        // 行削除の場合（行削除フラグ "1"）
                        if (record.rowDeleteFlg == "1")
                        {
                            // UPDATE: t_12040_組合員等別引受情報 の更新
                            var infoEntityForDelete = db.T12040組合員等別引受情報s.FirstOrDefault(x =>
                                x.組合等コード == sessionKumiaitoCd &&
                                x.年産 == sessionNensan &&
                                x.共済目的コード == sessionSKyosaiMokutekiCd &&
                                x.引受回 == record.hikiukekai &&
                                x.組合員等コード == record.kumiaiintoCd &&
                                x.類区分 == record.ruikbn &&
                                x.統計単位地域コード == record.toukeiTaniChiikiCd);
                            if (infoEntityForDelete != null)
                            {
                                infoEntityForDelete.引受解除返還賦課金額 = null;
                                infoEntityForDelete.引受解除日付 = null;
                                infoEntityForDelete.更新日時 = sysDate;
                                infoEntityForDelete.更新ユーザid = sessionUserId;
                            } 
                            else
                            {
                                throw new SystemException(MessageUtil.Get("MF00001"));
                            }

                            // DELETE: t_11020_個人設定解除 の対象レコードを削除
                            var t11020EntitiesForDelete = db.T11020個人設定解除s
                                .Where(x => x.組合等コード == sessionKumiaitoCd &&
                                            x.年産 == sessionNensan &&
                                            x.共済目的コード == sessionSKyosaiMokutekiCd &&
                                            x.組合員等コード == record.kumiaiintoCd)
                                .ToList();
                            if (t11020EntitiesForDelete.Count == 0)
                            {
                                throw new SystemException(MessageUtil.Get("MF00001"));
                            }
                            foreach (var entity in t11020EntitiesForDelete)
                            {
                                db.T11020個人設定解除s.Remove(entity);
                            }
                        }
                        // 新規行の場合（行追加フラグ "1"）
                        else if (record.rowAddFlg == "1")
                        {
                            if (count > 0)
                            {
                                resultMessage = MessageUtil.Get("ME90018", "登録先組合員等");
                                break;
                            }
                            if (string.IsNullOrEmpty(record.kumiaiintoCd))
                            {
                                resultMessage = MessageUtil.Get("ME00001", "組合員等コード", "");
                                break;
                            }
                            // t_11020_個人設定解除に対象レコードが存在するかチェック
                            var existingEntity = getJigyoDb<NskAppContext>().T11020個人設定解除s
                                        .AsNoTracking()
                                        .FirstOrDefault(x => x.組合等コード == sessionKumiaitoCd &&
                                                             x.年産 == sessionNensan &&
                                                             x.共済目的コード == sessionSKyosaiMokutekiCd &&
                                                             x.組合員等コード == record.kumiaiintoCd);

                            if (existingEntity == null)
                            {
                                // 存在しなければ INSERT
                                var newEntity = new T11020個人設定解除
                                {
                                    組合等コード = sessionKumiaitoCd,
                                    年産 = sessionNensan,
                                    共済目的コード = sessionSKyosaiMokutekiCd,
                                    組合員等コード = record.kumiaiintoCd,
                                    解除引受回 = record.hikiukekai,
                                    // 解除申出日付は、ここでは明細の「引受解除日付」を使用（必要に応じて変更）
                                    解除申出日付 = record.hikiukeKaijyoDate,
                                    // 引受解除日付は NULL（または必要なら設定）
                                    引受解除日付 = null,
                                    引受解除返還賦課金額 = record.hikiukeKaijyoHenkanHukakingaku,
                                    解除理由コード = record.kaijyoRiyuCd,
                                    登録日時 = sysDate,
                                    登録ユーザid = sessionUserId,
                                    更新日時 = sysDate,
                                    更新ユーザid = sessionUserId
                                };
                                db.T11020個人設定解除s.Add(newEntity);
                                // UPDATE: t_12040_組合員等別引受情報 の更新（新規行も対象）
                                var infoEntity = db.T12040組合員等別引受情報s.FirstOrDefault(x =>
                                    x.組合等コード == sessionKumiaitoCd &&
                                    x.年産 == sessionNensan &&
                                    x.共済目的コード == sessionSKyosaiMokutekiCd &&
                                    x.引受回 == record.hikiukekai &&
                                    x.組合員等コード == record.kumiaiintoCd &&
                                    x.解除フラグ == "0"); // 解除フラグ 0 のもののみ更新
                                if (infoEntity != null)
                                {
                                    infoEntity.引受解除返還賦課金額 = record.hikiukeKaijyoHenkanHukakingaku;
                                    infoEntity.引受解除日付 = record.hikiukeKaijyoDate;
                                    infoEntity.解除理由コード = record.kaijyoRiyuCd;
                                    infoEntity.更新日時 = sysDate;
                                    infoEntity.更新ユーザid = sessionUserId;
                                }
                            }
                        }
                        else
                        {
                            // 既存行の場合の更新

                            // UPDATE: t_12040_組合員等別引受情報
                            var infoEntity = db.T12040組合員等別引受情報s.FirstOrDefault(x =>
                                x.組合等コード == sessionKumiaitoCd &&
                                x.年産 == sessionNensan &&
                                x.共済目的コード == sessionSKyosaiMokutekiCd &&
                                x.引受回 == record.hikiukekai &&
                                x.組合員等コード == record.kumiaiintoCd &&
                                x.類区分 == record.ruikbn &&
                                x.統計単位地域コード == record.toukeiTaniChiikiCd);
                            if (infoEntity != null)
                            {
                                infoEntity.引受解除返還賦課金額 = record.hikiukeKaijyoHenkanHukakingaku;
                                infoEntity.引受解除日付 = record.hikiukeKaijyoDate;
                                infoEntity.解除理由コード = record.kaijyoRiyuCd;
                                infoEntity.更新日時 = sysDate;
                                infoEntity.更新ユーザid = sessionUserId;
                            }

                            // UPDATE: t_11020_個人設定解除
                            var ukeireEntity = db.T11020個人設定解除s.FirstOrDefault(x =>
                                x.組合等コード == sessionKumiaitoCd &&
                                x.年産 == sessionNensan &&
                                x.共済目的コード == sessionSKyosaiMokutekiCd &&
                                x.組合員等コード == record.kumiaiintoCd); // 解除フラグ 1（登録済み）のもの
                            if (ukeireEntity != null)
                            {
                                ukeireEntity.解除申出日付 = record.kanjyoMousideDate;
                                ukeireEntity.引受解除日付 = record.hikiukeKaijyoDate;
                                ukeireEntity.引受解除返還賦課金額 = record.hikiukeKaijyoHenkanHukakingaku;
                                ukeireEntity.解除理由コード = record.kaijyoRiyuCd;
                                ukeireEntity.更新日時 = sysDate;
                                ukeireEntity.更新ユーザid = sessionUserId;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(resultMessage))
                    {
                        transaction.Rollback();
                        return Json(new { message = resultMessage });
                    }
                    else
                    {
                        db.SaveChanges();
                        transaction.Commit();
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e.StackTrace);
                    transaction.Rollback();
                    resultMessage = MessageUtil.Get("MF00001");
                    return Json(new { message = resultMessage });
                }

            }

            if (selectedCount <= 0)
            {
                resultMessage = MessageUtil.Get("ME90007", "行");
            }
            
            return Json(new { message = resultMessage });
        }
        #endregion

        #region 検索条件チェックメソッド
        /// <summary>
        /// 検索条件チェックメソッド
        /// </summary>
        /// <param name="model">ビューモデル</param>
        private bool CheckSearchCondition(D104010Model model)
        {
            var checkFlg = false;

            // [画面：保険年度（終了）]<[画面：保険年度（開始）]の場合、エラーと判定する
            if (!string.IsNullOrEmpty(model.SearchCondition.kumiaiintoCdFrom) &&
                !string.IsNullOrEmpty(model.SearchCondition.kumiaiintoCdTo) &&
                long.Parse(model.SearchCondition.kumiaiintoCdFrom) > long.Parse(model.SearchCondition.kumiaiintoCdTo))
            {
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME10002", "組合員等コード（開始）", "組合員等コード（終了）"));
                ModelState.AddModelError("SearchCondition.kumiaiintoCdFrom", " ");
                ModelState.AddModelError("SearchCondition.kumiaiintoCdTo", " ");
                checkFlg = true;
                return checkFlg;
            }

            // [画面：保険年度（終了）]<[画面：保険年度（開始）]の場合、エラーと判定する
            if (!string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom) && !string.IsNullOrEmpty(model.SearchCondition.TodofukenDropDownList.ShochikuCdTo) &&
                int.Parse(model.SearchCondition.TodofukenDropDownList.ShochikuCdFrom) > int.Parse(model.SearchCondition.TodofukenDropDownList.ShochikuCdTo))
            {
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME10002", "小地区（開始）", "小地区（終了）"));
                ModelState.AddModelError("SearchCondition.TodofukenDropDownList.ShochikuCdFrom", " ");
                ModelState.AddModelError("SearchCondition.TodofukenDropDownList.ShochikuCdTo", " ");
                checkFlg = true;
                return checkFlg;
            }

            // ソート順選択値重複チェック
            if (model.SearchCondition.DisplaySort1.HasValue &&
                model.SearchCondition.DisplaySort1.ToString().Equals(model.SearchCondition.DisplaySort2.ToString()) &&
                model.SearchCondition.DisplaySort1.ToString().Equals(model.SearchCondition.DisplaySort3.ToString()))
            {
                // 表示順1、表示順2、表示順3の選択値が同じの場合
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME90018", "表示順"));
                ModelState.AddModelError("SearchCondition.DisplaySort1", " ");
                ModelState.AddModelError("SearchCondition.DisplaySort2", " ");
                ModelState.AddModelError("SearchCondition.DisplaySort3", " ");
                checkFlg = true;
                return checkFlg;
            }

            if (model.SearchCondition.DisplaySort1.HasValue &&
                model.SearchCondition.DisplaySort1.ToString().Equals(model.SearchCondition.DisplaySort2.ToString()))
            {
                // 表示順1、表示順2の選択値が同じの場合
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME90018", "表示順"));
                ModelState.AddModelError("SearchCondition.DisplaySort1", " ");
                ModelState.AddModelError("SearchCondition.DisplaySort2", " ");
                checkFlg = true;
                return checkFlg;
            }

            if (model.SearchCondition.DisplaySort1.HasValue &&
                model.SearchCondition.DisplaySort1.ToString().Equals(model.SearchCondition.DisplaySort3.ToString()))
            {
                // 表示順1、表示順3の選択値が同じの場合
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME90018", "表示順"));
                ModelState.AddModelError("SearchCondition.DisplaySort1", " ");
                ModelState.AddModelError("SearchCondition.DisplaySort3", " ");
                checkFlg = true;
                return checkFlg;
            }

            if (model.SearchCondition.DisplaySort2.HasValue &&
                model.SearchCondition.DisplaySort2.ToString().Equals(model.SearchCondition.DisplaySort3.ToString()))
            {
                // 表示順2、表示順3の選択値が同じの場合
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME90018", "表示順"));
                ModelState.AddModelError("SearchCondition.DisplaySort2", " ");
                ModelState.AddModelError("SearchCondition.DisplaySort3", " ");
                checkFlg = true;
                return checkFlg;
            }
            return checkFlg;
        }
        #endregion

        public class SortableRecordDto
        {
            public D104010TableRecord Record { get; set; }
            public string ShochikuCd { get; set; }
            public string VNogyosha_KumiaiintoCd { get; set; }
            public string DaichikuCd { get; set; }
        }

        /// <summary>
        /// 初期モデルの取得メソッド。
        /// </summary>
        /// <returns>初期モデル</returns>
        private D000999Model GetInitModel()
        {
            D000999Model model = new D000999Model();

            List<MTodofuken> todofukenList = TodofukenUtil.GetTodofukenList().ToList();
            if (todofukenList.Count() > 0)
            {
                model.TodofukenCd = todofukenList[0].TodofukenCd;
                model.TodofukenNm = todofukenList[0].TodofukenNm;
                List<MKumiaito> kumiaitoList = KumiaitoUtil.GetKumiaitoList(model.TodofukenCd);
                if (kumiaitoList.Count() > 0)
                {
                    model.KumiaitoCd = kumiaitoList[0].KumiaitoCd;
                    model.KumiaitoNm = kumiaitoList[0].KumiaitoNm;
                    List<MShishoNm> shishoList = ShishoUtil.GetShishoList(model.TodofukenCd, model.KumiaitoCd);
                    if (shishoList.Count() > 0)
                    {
                        model.ShishoCd = shishoList[0].ShishoCd;
                        model.ShishoNm = shishoList[0].ShishoNm;
                    }
                }
            }

            model.ScreenMode = "1";

            return model;
        }
    }
}