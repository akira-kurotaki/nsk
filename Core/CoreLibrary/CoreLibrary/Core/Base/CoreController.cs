using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.HelpMenu;
using CoreLibrary.Core.Menu;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Context;
using NLog;
using Npgsql;
using ReportService.Core;
using System.Text.Json;

namespace CoreLibrary.Core.Base
{
    /// <summary>
    /// コントローラー親クラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/1
    /// 作成者：MATSUBAYSHI Atsushi
    /// </remarks>
    public abstract class CoreController : Controller
    {
        #region クラス変数
        /// <summary>
        /// ユーザ情報(職員モデル)
        /// </summary>
        protected Syokuin Syokuin { get { return SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, HttpContext); } }

        /// <summary>
        /// 画面表示モード
        /// </summary>
        protected CoreConst.ScreenMode ScreenMode
        {
            get
            {
                object screenMode = SessionUtil.Get<CoreConst.ScreenMode>(CoreConst.SESS_SCREEN_MODE, HttpContext);
                return screenMode == null ? CoreConst.ScreenMode.None : (CoreConst.ScreenMode)screenMode;
            }
        }

        /// <summary>
        /// 事業共通DBに接続するDBコンテキストクラス
        /// </summary>
        private JigyoCommonContext jigyoCommonDb;

        /// <summary>
        /// システム共通DBに接続するDBコンテキストクラス
        /// </summary>
        private SystemCommonContext systemCommonDb;

        /// <summary>
        /// 農業者情報管理スキーマに接続するDBコンテキスト
        /// </summary>
        private FimContext fimDb;

        /// <summary>
        /// 都道府県別事業スキーマに接続するDBコンテキスト
        /// </summary>
        private JigyoContext jigyoDb;

        /// <summary>
        /// ロガー
        /// </summary>
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// SQL出力フラグ
        /// </summary>
        protected bool logOutputFlag = true;

        /// <summary>
        /// ICompositeViewEngine
        /// </summary>
        protected readonly ICompositeViewEngine _viewEngine;

        /// <summary>
        /// リアルタイム帳票呼び出しクライアント
        /// </summary>
        protected readonly ReportServiceClient _serviceClient;

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected CoreController()
        {
            ;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine">ビューエンジン</param>
        protected CoreController(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine">ビューエンジン</param>
        /// <param name="serviceClient">リアルタイム帳票呼び出しクライアント</param>
        protected CoreController(ICompositeViewEngine viewEngine, ReportServiceClient serviceClient)
        {
            _viewEngine = viewEngine;
            _serviceClient = serviceClient;
        }
        #endregion


        /// <summary>
        /// 事業共通DBに接続するDBコンテキストを取得する
        /// </summary>
        /// <returns>事業共通DBに接続するDBコンテキスト</returns>
        protected JigyoCommonContext getJigyoCommonDb()
        {
            if (jigyoCommonDb == null)
            {
                jigyoCommonDb = new JigyoCommonContext();
            }
            return jigyoCommonDb;
        }

        /// <summary>
        /// システム共通DBに接続するDBコンテキストを取得する
        /// </summary>
        /// <returns>システム共通DBに接続するDBコンテキスト</returns>
        protected SystemCommonContext getSystemCommonDb()
        {
            if (systemCommonDb == null)
            {
                systemCommonDb = new SystemCommonContext();
            }
            return systemCommonDb;
        }

        /// <summary>
        /// 都道府県別事業スキーマに接続するDBコンテキストを取得する
        /// セッションに格納されたログインユーザのDB接続先からDBコンテキストを作成する
        /// 事業システム向け
        /// </summary>
        /// <typeparam name="T">JigyoContext継承クラス</typeparam>
        /// <returns>都道府県別事業スキーマに接続するDBコンテキスト</returns>
        protected T getJigyoDb<T>() where T : JigyoContext
        {
            if (jigyoDb == null)
            {
                // セッションに格納されたログインユーザのDB接続先からDBコンテキストを作成する
                var info = SessionUtil.Get<DbConnectionInfo>(CoreConst.SESS_DB_CONN, HttpContext);
                jigyoDb = (JigyoContext)Activator.CreateInstance(typeof(T), info.ConnectionString, info.DefaultSchema, ConfigUtil.GetInt(CoreConst.COMMAND_TIMEOUT));
            }

            return (T)jigyoDb;
        }

        /// <summary>
        /// 農業者情報管理スキーマに接続するDBコンテキストを取得する
        /// システム共通（ログイン画面、システム選択画面、農業者情報管理）向け
        /// 事業システムの通常運用時には使用しない
        /// </summary>
        /// <returns>農業者情報管理スキーマに接続するDBコンテキスト</returns>
        protected FimContext getFimDb()
        {
            if (fimDb == null)
            {
                fimDb = new FimContext();
            }
            return fimDb;
        }

        #region ページャー
        /// <summary>
        /// ページID取得
        /// <param name="pageId">ページリンクID</param>
        /// <param name="totalCount">検索結果件数</param>
        /// <param name="displayCount">ページ表示件数</param>
        /// <returns>ページID</returns>
        /// </summary>
        protected int GetPageId(int pageId, int totalCount, int displayCount)
        {
            int tempPageId = pageId;
            if ((displayCount * pageId) > totalCount)
            {
                if (totalCount % displayCount > 0)
                {
                    tempPageId = totalCount / displayCount + 1;
                }
                else
                {
                    tempPageId = totalCount / displayCount;
                }
            }
            return tempPageId;
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (jigyoCommonDb != null)
                {
                    jigyoCommonDb.Dispose();
                }

                if (systemCommonDb != null)
                {
                    systemCommonDb.Dispose();
                }

                if (jigyoDb != null)
                {
                    jigyoDb.Dispose();
                }

                if (fimDb != null)
                {
                    fimDb.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion

        #region 部分ビュー
        /// <summary>
        /// 部分ビュー文字列をJSONデータへの変換メソッド
        /// <param name="viewName">ビュー名</param>
        /// <returns>JSONデータ</returns>
        /// </summary>
        protected JsonResult PartialViewAsJson(string viewName)
        {
            return PartialViewAsJson(viewName, null, null);
        }

        /// <summary>
        /// 部分ビュー文字列をJSONデータへの変換メソッド
        /// <param name="viewName">ビュー名</param>
        /// <param name="exceptionMessage">異常メッセージ</param>
        /// <returns>JSONデータ</returns>
        /// </summary>
        protected JsonResult PartialViewAsJson(string viewName, string exceptionMessage)
        {
            return PartialViewAsJson(viewName, null, exceptionMessage);
        }

        /// <summary>
        /// 部分ビュー文字列をJSONデータへの変換メソッド
        /// <param name="viewName">ビュー名</param>
        /// <param name="model">モデル</param>
        /// <returns>JSONデータ</returns>
        /// </summary>
        protected JsonResult PartialViewAsJson(string viewName, object model)
        {
            return PartialViewAsJson(viewName, model, (string)null);
        }

        /// <summary>
        /// 部分ビュー文字列をJSONデータへの変換メソッド
        /// <param name="viewName">ビュー名</param>
        /// <param name="model">モデル</param>
        /// <param name="exceptionMessage">異常メッセージ</param>
        /// <returns>JSONデータ</returns>
        /// </summary>
        protected JsonResult PartialViewAsJson(string viewName, object model, string exceptionMessage)
        {
            ViewData.Model = model;
            var viewAsString = "";
            IEnumerable<string> errorItems = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => x.Key).ToList();
            List<ModelErrorCollection> errorMessage = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => x.Value.Errors).ToList();
            List<string> errorMessages = errorItems.Count() > 0 ? new List<string>() : null;

            for (int index = 0; index < errorItems.Count(); index++)
            {
                ModelErrorCollection error = errorMessage[index];

                if (error.Count > 0)
                {
                    var editedErrorMessages = String.Empty;
                    foreach (var err in error)
                    {
                        if (!String.IsNullOrEmpty(editedErrorMessages))
                        {
                            editedErrorMessages += "<br/>";
                        }

                        editedErrorMessages += err.ErrorMessage;
                    }
                    errorMessages.Add(editedErrorMessages);
                }
            }

            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw, new HtmlHelperOptions());
                var t = viewResult.View.RenderAsync(viewContext);
                t.Wait();
                viewAsString = sw.GetStringBuilder().ToString();
            }
            var jsonResult = Json(new { partialView = viewAsString, message = exceptionMessage, errorItems = errorItems, errorMessages = errorMessages });

            return jsonResult;
        }

        /// <summary>
        /// 部分ビュー文字列をJSONデータへの変換メソッド(GET要求許可)
        /// <param name="viewName">ビュー名</param>
        /// <param name="serializerSettings">HTTP GET要求可否フラグ</param>
        /// <returns>JSONデータ</returns>
        /// </summary>
        protected JsonResult PartialViewAsJson(string viewName, JsonSerializerOptions serializerSettings)
        {
            return PartialViewAsJson(viewName, null, null, serializerSettings);
        }

        /// <summary>
        /// 部分ビュー文字列をJSONデータへの変換メソッド(GET要求許可)
        /// <param name="viewName">ビュー名</param>
        /// <param name="exceptionMessage">異常メッセージ</param>
        /// <param name="serializerSettings">HTTP GET要求可否フラグ</param>
        /// <returns>JSONデータ</returns>
        /// </summary>
        protected JsonResult PartialViewAsJson(string viewName, string exceptionMessage, JsonSerializerOptions serializerSettings)
        {
            return PartialViewAsJson(viewName, null, exceptionMessage, serializerSettings);
        }

        /// <summary>
        /// 部分ビュー文字列をJSONデータへの変換メソッド(GET要求許可)
        /// <param name="viewName">ビュー名</param>
        /// <param name="model">モデル</param>
        /// <param name="serializerSettings">HTTP GET要求可否フラグ</param>
        /// <returns>JSONデータ</returns>
        /// </summary>
        protected JsonResult PartialViewAsJson(string viewName, object model, JsonSerializerOptions serializerSettings)
        {
            return PartialViewAsJson(viewName, model, null, serializerSettings);
        }

        /// <summary>
        /// 部分ビュー文字列をJSONデータへの変換メソッド(GET要求許可)
        /// <param name="viewName">ビュー名</param>
        /// <param name="model">モデル</param>
        /// <param name="exceptionMessage">異常メッセージ</param>
        /// <param name="serializerSettings">HTTP GET要求可否フラグ</param>
        /// <returns>JSONデータ</returns>
        /// </summary>
        protected JsonResult PartialViewAsJson(string viewName, object model, string exceptionMessage, JsonSerializerOptions serializerSettings)
        {
            ViewData.Model = model;
            var viewAsString = "";
            IEnumerable<string> errorItems = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => x.Key).ToList();
            List<ModelErrorCollection> errorMessage = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => x.Value.Errors).ToList();
            List<string> errorMessages = errorItems.Count() > 0 ? new List<string>() : null;

            for (int index = 0; index < errorItems.Count(); index++)
            {
                ModelErrorCollection error = errorMessage[index];

                if (error.Count > 0)
                {
                    var editedErrorMessages = String.Empty;
                    foreach (var err in error)
                    {
                        if (!String.IsNullOrEmpty(editedErrorMessages))
                        {
                            editedErrorMessages += "<br/>";
                        }

                        editedErrorMessages += err.ErrorMessage;
                    }
                    errorMessages.Add(editedErrorMessages);
                }
            }

            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw, new HtmlHelperOptions());
                var t = viewResult.View.RenderAsync(viewContext);
                t.Wait();
                viewAsString = sw.GetStringBuilder().ToString();
            }
            var jsonResult = Json(new { partialView = viewAsString, message = exceptionMessage, errorItems = errorItems, errorMessages = errorMessages }, serializerSettings);

            return jsonResult;
        }
        #endregion

        #region 帳票作成
        /// <summary>
        /// 帳票パスを取得する
        /// </summary>
        /// <param name="fileNm">帳票ファイル名</param>
        /// <param name="sysdateTime">システム時間</param>
        protected string GetReportPath(string fileNm, DateTime sysdateTime)
        {
            // 帳票出力フォルダを取得する
            var reportOutputFolder = (ConfigUtil.Get(CoreConst.REPORT_OUTPUT_FOLDER) ?? string.Empty);
            // ユーザID
            var userId = Syokuin.UserId;
            // システム日時
            var systemDate = DateUtil.GetSysDateTime();

            // 帳票パス
            var reportPath = Path.Combine(
                reportOutputFolder,
                userId + "_" + systemDate.ToString("yyyyMMddHHmmssfff"),
                fileNm + systemDate.ToString("yyyyMMddHHmmssfff") + ".zip");

            return reportPath;
        }

        /// <summary>
        /// 帳票作成内容をバッチ予約管理テーブルに登録する
        /// </summary>
        /// <param name="joukenId">条件ID</param>
        /// <param name="reportControlId">帳票制御ID</param>
        /// <param name="serial_number">連番</param>
        //protected void InsertTBatchYoyaku(int joukenId, string reportControlId, int serial_number)
        //{
        //    var db = getSystemCommonDb();

        //    // 帳票出力フォルダを取得する
        //    var reportOutputFolder = ConfigUtil.Get(CoreConst.REPORT_OUTPUT_FOLDER);

        //    // ユーザID
        //    var userId = Syokuin.UserId;
        //    // システム日時
        //    var systemDate = DateUtil.GetSysDateTime();
        //    //「帳票処理情報」の取得
        //    var mReportKanris = ReportKanriUtil.GetReportKanri(reportControlId, serial_number);

        //    TBatchYoyaku yoyaku = new TBatchYoyaku();
        //    // 予約ユーザID
        //    yoyaku.YoyakuUserId = userId;
        //    // 予約日時
        //    yoyaku.YoyakuDate = systemDate;
        //    // 予約処理コード
        //    yoyaku.YoyakuCode = 1;
        //    // 予約処理名
        //    yoyaku.YoyakuNm = mReportKanris.YoyakuNm;
        //    // 帳票制御ID
        //    yoyaku.ReportControlId = mReportKanris.ReportControlId;
        //    // 条件ID
        //    yoyaku.JoukenId = joukenId;
        //    // 帳票パス
        //    yoyaku.ReportPath = Path.Combine(
        //        reportOutputFolder,
        //        userId,
        //        "_" + systemDate.ToString("yyyyMMddHHmmssfff"),
        //        mReportKanris.FileNm + systemDate.ToString("yyyyMMddHHmmssfff") + ".zip");
        //    // 開始日時
        //    yoyaku.StartDate = null;
        //    // 終了日時
        //    yoyaku.EndDate = null;
        //    // ステータス
        //    yoyaku.Status = "1";
        //    // バッチサーバIP
        //    yoyaku.ServerIp = "";
        //    // エラー情報
        //    yoyaku.ErrorInfo = "";
        //    // 登録ユーザID
        //    yoyaku.InsertUserId = userId;
        //    // 登録日時
        //    yoyaku.InsertDate = systemDate;
        //    // 更新ユーザID
        //    yoyaku.UpdateUserId = userId;
        //    // 更新日時
        //    yoyaku.UpdateDate = systemDate;

        //    db.TBatchYoyakus.Add(yoyaku);

        //    logger.Info(string.Format("予約ID：{0}を登録します。", DBUtil.CurrSeqVal(db, "t_batch_yoyaku_yoyaku_id_seq") + 1));
        //    db.SaveChanges();
        //}

        /// <summary>
        /// 条件テーブルの登録内容を削除する
        /// </summary>
        /// <param name="joukenId">条件ID</param>
        protected void DeletetReportJouken(int joukenId)
        {
            var db = getSystemCommonDb();

            var sql = "DELETE FROM " + ConfigUtil.Get(CoreConst.DEFAULT_SCHEMA) + ".t_report_jouken WHERE jouken_id = @jouken_id";

            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();
            parameters.Add(new NpgsqlParameter("@jouken_id", joukenId));

            db.Database.ExecuteSqlRaw(sql, parameters.ToArray());
        }
        #endregion

        #region 画面表示モード

        protected void SetScreenModeFromQueryString()
        {
            if (ScreenMode == CoreConst.ScreenMode.None)
            {
                // クエリーストリングからスクリーンモードを取得
                string screenMode = Request.Query["sm"];
                if (screenMode == CoreConst.SCREEN_MODE_PC_HASH)
                {
                    SessionUtil.Set(CoreConst.SESS_SCREEN_MODE, CoreConst.ScreenMode.PC, HttpContext);
                }
                else if (screenMode == CoreConst.SCREEN_MODE_TABLET_HASH)
                {
                    SessionUtil.Set(CoreConst.SESS_SCREEN_MODE, CoreConst.ScreenMode.Tablet, HttpContext);
                }
                else
                {
                    if (ConfigUtil.Get(CoreConst.APP_ENV) == "debug")
                    {
                        SessionUtil.Set(CoreConst.SESS_SCREEN_MODE, CoreConst.ScreenMode.PC, HttpContext);
                    }
                }
            }
        }
        #endregion

        #region 操作履歴登録
        /// <summary>
        /// 操作履歴登録
        /// </summary>
        /// <param name="db">データベース</param>
        /// <param name="nogyoshaId">農業者ID</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="screenId">画面ID</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="sosaNaiyo">操作内容</param>
        /// <param name="displayColor">表示色</param>
        /// <returns>True：成功　False：失敗</returns>
        //protected bool RegisterHistory(JigyoContext db, int nogyoshaId, string userId, string screenId,
        //                                string shishoCd, string sosaNaiyo, string displayColor)
        //{

        //    TSosaRireki sosaRireki = new TSosaRireki()
        //    {
        //        NogyoshaId = nogyoshaId,
        //        SosaDate = DateUtil.GetSysDateTime(),
        //        UserId = GetUserId(),
        //        SystemRiyoKbn = Syokuin.SystemRiyoKbn,
        //        ScreenId = screenId,
        //        ShishoCd = shishoCd,
        //        SosaNaiyo = sosaNaiyo,
        //        DisplayColor = displayColor
        //    };

        //    db.TSosaRirekis.Add(sosaRireki);

        //    using var tx = db.Database.BeginTransaction();

        //    try
        //    {
        //        db.SaveChanges();
        //        tx.Commit();
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e);
        //        tx.Rollback();
        //        return false;
        //    }

        //    return true;
        //}
        #endregion

        #region お知らせ登録
        /// <summary>
        /// お知らせ登録
        /// </summary>
        /// <param name="db">データベース</param>
        /// <param name="nogyoshaId">農業者ID</param>
        /// <param name="oshiraseDate">お知らせ日付</param>
        /// <param name="shinseiOhiraseNaiyo">お知らせ内容</param>
        /// <returns>True：成功　False：失敗</returns>
        //protected bool RegisterOshirase(JigyoContext db, int nogyoshaId, DateTime oshiraseDate, string shinseiOhiraseNaiyo)
        //{
        //    TShinseiOshirase oshirase = new TShinseiOshirase()
        //    {
        //        NogyoshaId = nogyoshaId,
        //        OshiraseDate = oshiraseDate,
        //        ShinseiOhiraseNaiyo = shinseiOhiraseNaiyo
        //    };

        //    db.TShinseiOshirases.Add(oshirase);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Error(e);
        //        return false;
        //    }

        //    return true;
        //}
        #endregion


        /// <summary>
        /// UserId取得
        /// </summary>
        /// <returns>UserId</returns>
        protected string GetUserId()
        {
            return (Syokuin == null ? string.Empty : Syokuin.UserId);
        }

        /// <summary>
        /// ファイル拡張子からコンテンツタイプを判定する。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        protected string GetMimeTypeForFileExtension(string filePath)
        {
            const string DefaultContentType = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(filePath, out string contentType))
            {
                contentType = DefaultContentType;
            }

            return contentType;
        }

        /// <summary>
        /// リストを分割する
        /// </summary>
        /// <typeparam name="T">分割モデル</typeparam>
        /// <param name="source">分割したリスト</param>
        /// <param name="linecount">分割件数</param>
        /// <returns>分割後のリスト</returns>
        public static List<List<T>> SplitList<T>(List<T> source, int linecount)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / linecount)
                .Select(g => g.Select(x => x.Value).ToList())
                .ToList();
        }

        #region メニュー、ヘルプメニュー
        /// <summary>
        /// セッションにメニュー情報、ヘルプメニュー情報を設定する
        /// </summary>
        protected void SetMenuAndHelpMenu()
        {
            // メニューを設定
            if (SessionUtil.Get<IEnumerable<MenuItem>>(CoreConst.SESS_MENU_LIST, HttpContext) == null)
            {
                SessionUtil.Set(CoreConst.SESS_MENU_LIST, MenuUtil.GetLeftMenuItems(HttpContext), HttpContext);
            }

            // ヘルプメニューを設定
            if (SessionUtil.Get<IEnumerable<HelpMenuItem>>(CoreConst.SESS_HELP_MENU_LIST, HttpContext) == null)
            {
                SessionUtil.Set(CoreConst.SESS_HELP_MENU_LIST, HelpMenuUtil.GetHelpMenuItems(HttpContext), HttpContext);
            }
        }
        #endregion
    }
}