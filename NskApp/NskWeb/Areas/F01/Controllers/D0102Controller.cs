using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskWeb.Areas.F01.Consts;
using NskWeb.Areas.F01.Models.D0102;
using NskWeb.Common.Consts;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.DropDown;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Extensions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using Npgsql;
using NpgsqlTypes;
using ReportService.Core;
using System.Text;

namespace NskWeb.Areas.F01.Controllers
{
    /// <summary>
    /// 加入者情報
    /// </summary>
    [SessionOutCheck]
    [Area("F01")]
    [Authorize(Roles = "bas")]
    public class D0102Controller : CoreController
    {
        #region メンバー定数
        /// <summary>
        /// 画面ID(D0102)
        /// </summary>
        private static readonly string SCREEN_ID_D0102 = "D0102";

        /// <summary>
        /// 画面ID(D0101)
        /// </summary>
        private static readonly string SCREEN_ID_D0101 = "D0101";

        /// <summary>
        /// 画面ID(_D0102Partial1)
        /// </summary>
        private static readonly string SCREEN_ID_D0102_PARTIAL1 = "_D0102Partial1";

        /// <summary>
        /// 機能ID(D0102_001)
        /// </summary>
        private static readonly string REPORT_ID_D0102_001 = SCREEN_ID_D0102 + "_" + "001";

        /// <summary>
        /// セッションキー(D0102)
        /// </summary>
        private static readonly string SESS_D0102 = SCREEN_ID_D0102 + "_" + "SCREEN";

        /// <summary>
        /// セッションキー(加入者情報)
        /// </summary>
        private static readonly string SESS_TKANYUSHA = SESS_D0102 + "_" + "TKANYUSHA";

        /// <summary>
        /// セッションキー(PAGE_TOKEN）
        /// </summary>
        public static readonly string SESS_D0102_PAGE_TOKEN = SCREEN_ID_D0102 + "_" + "PAGE_TOKEN";

        /// <summary>
        /// 帳票制御ID
        /// </summary>
        public static readonly string REPORT_CONTROL_ID_H002 = "H002";

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        /// <param name="serviceClient"></param>
        public D0102Controller(ICompositeViewEngine viewEngine, ReportServiceClient serviceClient) : base(viewEngine, serviceClient)
        {
        }
        #endregion

        #region 初期表示イベント
        /// <summary>
        /// イベント：初期表示
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Init()
        {
            // モデル初期化
            D0102Model model = new D0102Model();
            ModelState.Clear();

            // セッションから農業者ID、対象年度を取得する
            var nogyoshaId = SessionUtil.Get<int?>(AppConst.SESS_NOGYOSHA_ID, HttpContext);
            var nendo = SessionUtil.Get<short?>(AppConst.SESS_NENDO, HttpContext);

            if (!nogyoshaId.HasValue)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できない"));
            }
            model.NogyoshaId = nogyoshaId;

            // PAGE_TOKEN
            // 初期表示時、生成しセッション及びHIDDENに格納
            model.PageToken = Guid.NewGuid().ToString();
            SessionUtil.Set(SESS_D0102_PAGE_TOKEN, model.PageToken, HttpContext);
            logger.Info("D0102_PAGE_TOKEN設定：" + model.PageToken);

            // ログインユーザの参照・更新可否判定
            // 画面IDをキーとして、画面マスタ、画面機能権限マスタを参照し、ログインユーザに本画面の権限がない場合は業務エラー画面を表示する。
            if (!ScreenSosaUtil.CanReference(SCREEN_ID_D0102, HttpContext))
            {
                throw new AppException("ME90003", MessageUtil.Get("ME90003"));
            }

            // 画面に対するユーザの更新可否を取得する。
            model.CanUpdate = ScreenSosaUtil.CanUpdate(SCREEN_ID_D0102, HttpContext);
            if (!model.CanUpdate)
            {
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("MI00019"));
            }

            // 申請書類等作成ボタンの参照可否
            model.CanReportOutput = ScreenSosaUtil.CanReference(REPORT_ID_D0102_001, HttpContext);

            // 利用可能な支所一覧
            var shishoList = ScreenSosaUtil.GetShishoList(HttpContext);
            // 「農業者情報に対するユーザ権限有無」情報が取得できない（＝権限がない）場合は、業務エラー画面を表示する。
            if (!HaveNogyoshaKengen(nogyoshaId.Value, Syokuin.TodofukenCd, Syokuin.KumiaitoCd, Syokuin.ShishoCd, shishoList))
            {
                throw new AppException("ME90004", MessageUtil.Get("ME90004"));
            }

            // 「農業者情報」を取得する
            VNogyosha nogyoshaInfo = GetNogyoshaInfo(nogyoshaId.Value);

            // 農業者情報がある場合
            if (nogyoshaInfo != null)
            {
                // 都道府県マルチドロップダウンリスト
                model.TodofukenDropDownList = new TodofukenDropDownList("D0102EntryContent", Syokuin, shishoList);
                // 都道府県
                model.TodofukenDropDownList.TodofukenCd = nogyoshaInfo.TodofukenCd;
                // 組合等
                model.TodofukenDropDownList.KumiaitoCd = nogyoshaInfo.KumiaitoCd;
                // 支所
                model.TodofukenDropDownList.ShishoCd = nogyoshaInfo.ShishoCd;
                // 市町村
                model.TodofukenDropDownList.ShichosonCd = nogyoshaInfo.ShichosonCd;
                // 大地区
                model.TodofukenDropDownList.DaichikuCd = nogyoshaInfo.DaichikuCd;
                // 小地区
                model.TodofukenDropDownList.ShochikuCd = nogyoshaInfo.ShochikuCd;
                // 氏名又は法人名(フリガナ）
                model.HojinFullKana = nogyoshaInfo.HojinFullKana;
                // 氏名又は法人名
                model.HojinFullNm = nogyoshaInfo.HojinFullNm;

                // 「加入者情報」を取得する
                TKanyusha kanyushaInfo = GetKanyushaInfo(nogyoshaId.Value, nendo);
                // 「加入者情報」排他制御のため、データを取得する。
                var kanyushaInfoSes = GetKanyushaList(nogyoshaId.Value);
                // DB更新用レコードをセッションに格納する。
                SessionUtil.Set(SESS_TKANYUSHA + nogyoshaId, kanyushaInfoSes, HttpContext);

                if (kanyushaInfo != null)
                {
                    model.EntryContent = new D0102EntryContent(kanyushaInfo);
                }
                else
                {
                    //「加入者情報」が取得できない場合、[画面：申請書類等作成] ボタンを非活性にする。
                    model.CanReportOutput = false;
                    model.EntryContent.KouchiKeitaiCd = F01Const.KochiKeitai.NotSelected.ToString("d");
                }
                // 対象年度
                model.EntryContent.Nendo = nendo;
            }

            SessionUtil.Set(SESS_D0102, model, HttpContext);

            // D0102 加入者情報を表示する
            return View(SCREEN_ID_D0102, model);
        }

        /// <summary>
        /// 「農業者情報に対するユーザ権限有無」を取得する。
        /// </summary>
        /// <param name="nogyoshaId">農業者ID</param>
        /// <param name="todofukenCd">都道府県</param>
        /// <param name="kumiaitoCd">組合等</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="shishoList">利用可能支所一覧</param>
        private bool HaveNogyoshaKengen(int nogyoshaId, string todofukenCd, string kumiaitoCd, string shishoCd, List<Shisho> shishoList)
        {
            // sql用定数定義
            StringBuilder sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            sql.Append("SELECT ");
            sql.Append("    1  AS \"Value\" ");
            sql.Append("FROM ");
            sql.Append("    v_nogyosha ");
            sql.Append("WHERE ");

            //農業者ID
            sql.Append("    nogyosha_id = @NogyoshaId ");
            parameters.Add(new NpgsqlParameter("@NogyoshaId", nogyoshaId));

            //都道府県
            sql.Append("    AND todofuken_cd = @TodofukenCd ");
            parameters.Add(new NpgsqlParameter("@TodofukenCd", todofukenCd));

            //組合等
            sql.Append("    AND kumiaito_cd = @KumiaitoCd ");
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", kumiaitoCd));

            // 支所
            if (!shishoList.IsNullOrEmpty())
            {
                // [セッション：利用可能支所一覧]がある場合
                sql.Append("AND shisho_cd = ANY (@ShishoCdList) ");
                parameters.Add(new NpgsqlParameter("@ShishoCdList", NpgsqlDbType.Array | NpgsqlDbType.Varchar)
                {
                    Value = shishoList.Select(i => i.ShishoCd).ToList()
                });
            }
            else if (!string.IsNullOrEmpty(shishoCd))
            {
                // [セッション：利用可能支所一覧]がない、かつ、[セッション：支所コード]が空でない場合
                sql.Append("AND shisho_cd = @ShishoCd ");
                parameters.Add(new NpgsqlParameter("@ShishoCd", shishoCd));
            }

            logger.Info("「農業者情報に対するユーザ権限有無」の取得処理を実行します。");
            var result = getJigyoDb<NskAppContext>().Database.SqlQueryRaw<int>(sql.ToString(), parameters.ToArray()).ToList();

            return !result.IsNullOrEmpty();
        }

        /// <summary>
        /// 「農業者情報」を取得する
        /// </summary>
        /// <param name="nogyoshaId">農業者ID</param>
        private VNogyosha GetNogyoshaInfo(int nogyoshaId)
        {
            logger.Info("「農業者情報」取得処理を実行します。");
            return getJigyoDb<NskAppContext>().VNogyoshas
                .Where(t => t.NogyoshaId == nogyoshaId)
                .SingleOrDefault();
        }

        /// <summary>
        /// 「加入者情報」を取得する
        /// </summary>
        /// <param name="nogyoshaId">農業者ID</param>
        /// <param name="nendo">対象年度</param>
        private TKanyusha GetKanyushaInfo(int nogyoshaId, short? nendo)
        {
            if (!nendo.HasValue)
            {
                return null;
            }
            logger.Info("「加入者情報」取得処理を実行します。");
            return getJigyoDb<NskAppContext>().TKanyushas
                .Where(t => t.NogyoshaId == nogyoshaId && t.Nendo == nendo)
                .SingleOrDefault();
        }

        /// <summary>
        /// 加入者情報を取得する
        /// <returns>加入者情報</returns>
        /// </summary>
        /// <param name="nogyoshaId">農業者ID</param>
        private List<TKanyusha> GetKanyushaList(int nogyoshaId)
        {
            logger.Info("「加入者情報」排他制御データ取得処理を実行します。");
            return getJigyoDb<NskAppContext>().TKanyushas.Where(t => t.NogyoshaId == nogyoshaId).ToList();
        }

        #endregion     

        #region 登録イベント
        /// <summary>
        /// イベント：登録
        /// エラーチェックをおこないDBに登録する。
        /// </summary>
        /// <param name="form">画面の入力値</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind("EntryContent, PageToken")] D0102Model form)
        {
            // セッションから画面モデルを取得する
            var model = SessionUtil.Get<D0102Model>(SESS_D0102, HttpContext);
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面モデルを取得できない"));
            }

            // PAGE_TOKEN
            // 登録時、セッションとHIDDENの比較チェック
            var pageToken = SessionUtil.Get<string>(SESS_D0102_PAGE_TOKEN, HttpContext);
            if (pageToken != form.PageToken)
            {
                logger.Info("PAGE_TOKEN不一致。セッション:" + pageToken + ", モデル:" + form.PageToken);
                throw new AppException("ME00068", MessageUtil.Get("ME00068"));
            }

            // 値を保持するためにの処理
            model.EntryContent = form.EntryContent;

            // システムロック、データロック確認
            if (IsLocked(model))
            {
                // ロック有り
                return View(SCREEN_ID_D0102, model);
            }

            var nogyoshaId = model.NogyoshaId;
            var nendo = model.EntryContent.Nendo;
            // 属性チェック
            // 独自チェック（サーバ側）
            // 加入者管理コードの重複チェック
            if (!ModelState.IsValid || !IsInputValid(model, nogyoshaId) ||
                (!string.IsNullOrEmpty(model.EntryContent.KanyushaCd) && !KanyushaCdChk(nogyoshaId, model.EntryContent.KanyushaCd)))
            {
                // チェックNG
                return View(SCREEN_ID_D0102, model);
            }

            // 利用可能な支所一覧
            var shishoList = ScreenSosaUtil.GetShishoList(HttpContext);
            // 「農業者情報に対するユーザ権限有無」情報が取得できない（＝権限がない）場合
            if (!HaveNogyoshaKengen(nogyoshaId.Value, Syokuin.TodofukenCd, Syokuin.KumiaitoCd, Syokuin.ShishoCd, shishoList))
            {
                // チェックNG
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME90004"));
                return View(SCREEN_ID_D0102, model);
            }

            var sysDate = DateUtil.GetSysDateTime();
            bool result = false;

            // セッションから加入者情報を取得する
            var kanyushaInfo = SessionUtil.Get<List<TKanyusha>>(SESS_TKANYUSHA + nogyoshaId, HttpContext);
            if (!kanyushaInfo.IsNullOrEmpty() && !(kanyushaInfo.Where(x => x.NogyoshaId == nogyoshaId && x.Nendo == nendo).ToList().IsNullOrEmpty()))
            {
                var list = kanyushaInfo.Where(x => x.NogyoshaId == nogyoshaId && x.Nendo == nendo).ToList();
                var beforeData = list[0];
                // 更新判定
                if (!CheckNeedUpdateOrInsert(model, beforeData))
                {
                    // 本画面用のセッション情報をクリアする
                    SessionClear();

                    // 処理を終了し加入者情報へ遷移する
                    return RedirectToAction("Init", SCREEN_ID_D0101, new { area = "F01" });
                }

                // 更新登録処理
                result = Update(model, beforeData, sysDate);
            }
            else
            {
                // 新規登録処理
                result = Insert(nogyoshaId, model, sysDate);
            }

            if (!result)
            {
                // 失敗した場合
                return View(SCREEN_ID_D0102, model);
            }
            // 本画面用のセッション情報をクリアする
            SessionClear();
            // D0101 加入者一覧へ遷移する。
            return RedirectToAction("Init", SCREEN_ID_D0101, new { area = "F01" });
        }
        #endregion

        #region 申請書類等作成
        /// <summary>
        /// 申請書類等作成
        /// 対象の加入者情報が印字された加入者情報書類のPDFファイルを作成をする。
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            // セッションに自画面のデータが存在しない場合
            var model = SessionUtil.Get<D0102Model>(SESS_D0102, HttpContext);

            if (model == null || !model.NogyoshaId.HasValue || model.EntryContent == null || !model.EntryContent.Nendo.HasValue)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // 農業者ID、対象年度
            var nogyoshaId = model.NogyoshaId.Value;
            var nendo = model.EntryContent.Nendo.Value;
            // 帳票作成条件を登録する
            int joukenId = InsertTReportJouken(nogyoshaId.ToString(), nendo.ToString());

            // 利用可能な支所一覧
            var shishoList = ScreenSosaUtil.GetShishoList(HttpContext);

            // リアルタイム帳票出力サービスを呼び出す
            var result = new ReportResult();
            var shishoCdList = new List<string>();
            if (!shishoList.IsNullOrEmpty())
            {
                shishoCdList = shishoList.Select(t => t.ShishoCd).Order().ToList();
            }
            logger.Info("リアルタイム帳票を呼び出す。（帳票制御ID:" + REPORT_CONTROL_ID_H002 + "）");
            result = await _serviceClient.CreateReport(REPORT_CONTROL_ID_H002, Syokuin.UserId, joukenId, Syokuin.TodofukenCd, Syokuin.KumiaitoCd, Syokuin.ShishoCd, shishoCdList);

            // 帳票出力処理エラーの場合
            if (result == null)
            {
                throw new SystemException("リアル帳票出力処理に予期以外のエラーが発生しました。");
            }
            if (result.Result != 0)
            {
                throw new AppException(result.ErrorMessageId, result.ErrorMessage, CoreConst.HEADER_PATTERN_ID_2);
            }

            logger.Info("申請書類等作成 帳票制御ID：" + REPORT_CONTROL_ID_H002 + " 条件ID：" + joukenId);

            var cd = new System.Net.Mime.ContentDisposition
            {
                Inline = true,
            };
            Response.Headers.Append(AppConst.RESPONSE_HEADER, cd.ToString());
            return File(result.ReportData, AppConst.RESPONSE_CONTENT_TYPE_APPLICATION_PDF, cd.FileName);
        }
        #endregion

        #region 帳票作成条件登録メッソド
        /// <summary>
        /// メッソド:帳票作成条件登録
        /// </summary>
        /// <param name="nogyoshaId">農業者ID</param>
        /// <param name="nendo">対象年度</param>
        private int InsertTReportJouken(string nogyoshaId, string nendo)
        {
            // 条件IDを取得する
            logger.Info("条件ID取得処理を実行します。");
            int joukenId = (int)DBUtil.NextSeqVal(getJigyoDb<NskAppContext>(), "seq_jouken_id");
            logger.Info("条件ID取得処理が実行完了（条件ID:" + joukenId + "）");

            // ユーザID
            var userId = Syokuin.UserId;
            // システム日時
            var systemDate = DateUtil.GetSysDateTime();
            // 連番
            int serialNumber = 0;

            using (var tx = getJigyoDb<NskAppContext>().Database.BeginTransaction())
            {
                // 農業者ID
                var jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_NOGYOSHA_ID,
                    JoukenDisplayValue = nogyoshaId,
                    JoukenValue = nogyoshaId,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_NOGYOSHA_ID + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 対象年度
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_TARGET_NENDO,
                    JoukenDisplayValue = nendo,
                    JoukenValue = nendo,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_TARGET_NENDO + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 農業者ID、対象年度
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_NOGYOSHA_ID_TARGET_NENDO,
                    JoukenDisplayValue = string.Concat("(", nogyoshaId, ",", nendo, ")"),
                    JoukenValue = string.Concat("(", nogyoshaId, ",", nendo, ")"),
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_NOGYOSHA_ID_TARGET_NENDO + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                // 帳票制御ID
                jouken = new TReportJouken
                {
                    JoukenId = joukenId,
                    SerialNumber = ++serialNumber,
                    JoukenNm = JoukenNameConst.JOUKEN_REPORT_CONTROL_ID,
                    JoukenDisplayValue = REPORT_CONTROL_ID_H002,
                    JoukenValue = REPORT_CONTROL_ID_H002,
                    InsertUserId = userId,
                    InsertDate = systemDate,
                    UpdateUserId = userId,
                    UpdateDate = systemDate
                };
                getJigyoDb<NskAppContext>().TReportJoukens.Add(jouken);
                logger.Info("帳票作成条件を条件テーブルに登録する(条件名：" + JoukenNameConst.JOUKEN_REPORT_CONTROL_ID + ")");
                getJigyoDb<NskAppContext>().SaveChanges();

                tx.Commit();
            }
            return joukenId;
        }
        #endregion

        #region 戻る
        /// <summary>
        /// 戻る
        /// セッションをクリアする
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Back()
        {
            SessionClear();
            return Json(new { result = "success" });
        }

        /// <summary>
        /// セッションをクリアする
        /// </summary>
        private void SessionClear()
        {
            // セッションから画面モデルを取得する
            var model = SessionUtil.Get<D0102Model>(SESS_D0102, HttpContext);
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面モデルを取得できない"));
            }

            // セッションをクリアする
            SessionUtil.Remove(SESS_D0102, HttpContext);
            SessionUtil.Remove(SESS_D0102_PAGE_TOKEN, HttpContext);
            SessionUtil.Remove(AppConst.SESS_NOGYOSHA_ID, HttpContext);
            SessionUtil.Remove(AppConst.SESS_NENDO, HttpContext);
            SessionUtil.Remove(SESS_TKANYUSHA + model.NogyoshaId, HttpContext);

        }
        #endregion

        #region チェック
        #region 加入者管理コードの重複チェック
        /// <summary>
        /// 加入者管理コードの重複チェック
        /// </summary>
        /// <param name="nogyoshaId">農業者ID</param>
        /// <param name="kanyushaCd">加入者管理コード</param>
        private bool KanyushaCdChk(int? nogyoshaId, string kanyushaCd)
        {
            // sql用定数定義
            StringBuilder sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            sql.Append("SELECT ");
            sql.Append("    COUNT(1)  AS \"Value\" ");
            sql.Append("FROM ");
            sql.Append("    t_kanyusha ");
            sql.Append("WHERE ");

            //農業者ID
            sql.Append("    nogyosha_id != @NogyoshaId ");
            parameters.Add(new NpgsqlParameter("@NogyoshaId", nogyoshaId));

            //加入者管理コード
            sql.Append("    AND kanyusha_cd = @KanyushaCd ");
            parameters.Add(new NpgsqlParameter("@KanyushaCd", kanyushaCd));

            logger.Info("「加入者管理コード登録件数」の取得処理を実行します。");
            var cnt = getJigyoDb<NskAppContext>().Database.SqlQueryRaw<int>(sql.ToString(), parameters.ToArray()).Single();
            if (1 <= cnt)
            {
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME91002", "指定した加入者管理コード"));
                ModelState.AddModelError("EntryContent.KanyushaCd", " ");
                return false;
            }
            return true;
        }

        #endregion

        #region 独自チェック（サーバ側）
        /// <summary>
        /// 独自チェック（サーバ側）
        /// </summary>
        /// <param name="form">画面の入力値</param>     
        /// <param name="nogyoshaId">農業者ID</param>
        /// <returns>true:エラー無し/ false:エラー有り</returns>
        private bool IsInputValid(D0102Model form, int? nogyoshaId)
        {
            // システム日付
            var sysDateTime = DateUtil.GetSysDateTime();
            var sysDateYmd = new DateTime(sysDateTime.Year, sysDateTime.Month, sysDateTime.Day);
            var errFlg = true;
            // 耕地郵便番号の関連チェック
            if (string.IsNullOrEmpty(form.EntryContent.KouchiPostalCd1) || string.IsNullOrEmpty(form.EntryContent.KouchiPostalCd2))
            {
                if (!string.IsNullOrEmpty(form.EntryContent.KouchiPostalCd1) || !string.IsNullOrEmpty(form.EntryContent.KouchiPostalCd2))
                {
                    ModelState.AddModelError("MessageArea2", MessageUtil.Get("ME00020", "耕地郵便番号", "7", ""));
                    ModelState.AddModelError("EntryContent.KouchiPostalCd1", " ");
                    ModelState.AddModelError("EntryContent.KouchiPostalCd2", " ");
                    errFlg = false;
                }
            }
            //耕地形態のチェック
            if (F01Const.KochiKeitai.NotSelected.ToString("d").Equals(form.EntryContent.KouchiKeitaiCd))
            {
                ModelState.AddModelError("MessageArea2", MessageUtil.Get("ME91001"));
                ModelState.AddModelError("EntryContent.KouchiKeitaiCd", " ");
                errFlg = false;
            }

            //個人情報の取扱いのチェック
            if (!form.EntryContent.KojinjohoToriatsukaiFlg)
            {
                ModelState.AddModelError("MessageArea2", MessageUtil.Get("ME00005", "個人情報の取扱いについて", ""));
                ModelState.AddModelError("EntryContent.KojinjohoToriatsukaiFlg", " ");
                errFlg = false;
            }

            // 「１．２．」で実施した独自チェックと同等のチェックを実施結果がfalseの場合、
            // 加入者情報を表示し処理を終了する
            if (!errFlg)
            {
                return errFlg;
            }

            return true;
        }
        #endregion

        #region 更新判定チェック
        /// <summary>
        /// 更新判定チェック
        /// </summary>
        /// <param name="form">画面の入力値</param>
        /// <param name="kanyusha">加入者情報</param>
        /// <returns>True：更新の必要がある/Flase：更新の必要がない</returns>
        private bool CheckNeedUpdateOrInsert(D0102Model form, TKanyusha kanyusha)
        {
            if (form == null || kanyusha == null)
            {
                return false;
            }

            D0102EntryContent entryContent = form.EntryContent;

            if (form.NogyoshaId != kanyusha.NogyoshaId ||
                NullableStringToString(form.EntryContent.Nendo.ToString()) != NullableStringToString(kanyusha.Nendo.ToString()) ||
                NullableStringToString(entryContent.KanyushaCd) != NullableStringToString(kanyusha.KanyushaCd) ||
                NullableStringToString(entryContent.KouchiPostalCd1) != (string.IsNullOrEmpty(kanyusha.KouchiPostalCd) ? string.Empty : kanyusha.KouchiPostalCd.Substring(0, 3)) ||
                NullableStringToString(entryContent.KouchiPostalCd2) != (string.IsNullOrEmpty(kanyusha.KouchiPostalCd) ? string.Empty : kanyusha.KouchiPostalCd.Substring(3, 4)) ||
                NullableStringToString(entryContent.KouchiAddressKana) != NullableStringToString(kanyusha.KouchiAddressKana) ||
                NullableStringToString(entryContent.KouchiAddress) != NullableStringToString(kanyusha.KouchiAddress) ||
                NullableStringToString(entryContent.KouchiMenseki) != NullableStringToString(kanyusha.KouchiMenseki.HasValue ? kanyusha.KouchiMenseki.Value.ToString() : "") ||
                NullableStringToString(entryContent.KouchiKeitaiCd) != NullableStringToString(kanyusha.KouchiKeitaiCd) ||
                entryContent.KojinjohoToriatsukaiFlg != AppConst.FLG_ON.Equals(kanyusha.KojinjohoToriatsukaiFlg) ||
                entryContent.KanyuShinseiYmd != kanyusha.KanyuShinseiYmd ||
                NullableStringToString(entryContent.Biko) != NullableStringToString(kanyusha.Biko))
            {
                return true;
            }


            return false;
        }
        #endregion

        #region システムロック、データロック確認
        /// <summary>
        /// システムロック、データロック確認
        /// </summary>
        /// <param name="form">画面の入力値</param>
        /// <returns>true：ロック有り、false：ロック無し</returns>
        private bool IsLocked(D0102Model form)
        {
            // システム区分
            var systemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN);
            // システム日時
            var sysDateTime = DateUtil.GetSysDateTime();
            // ロック実行ユーザID
            var lockUserId = string.Empty;

            // システムロック取得
            var lockStatus = LockUtil.GetSysLock(systemKbn, Syokuin.TodofukenCd, sysDateTime, ref lockUserId);

            logger.Debug("LockStatus : " + lockStatus);
            logger.Debug("LockUserId : " + lockUserId);

            if (lockStatus == LockUtil.LOCKED_STATE_LOCKED)
            {
                // ロック有り
                ModelState.AddModelError("MessageArea2", MessageUtil.Get("ME91013", "システム", lockUserId));
                return true;
            }
            else if (lockStatus == LockUtil.LOCKED_STATE_ERROR)
            {
                // エラー
                logger.Fatal("システムロック取得でエラーが発生しました。");
                throw new SystemException(MessageUtil.Get("MF00001"));
            }

            // 農業者ID
            var nogyoshaId = form.NogyoshaId;

            // 「農業者情報」を取得する
            VNogyosha nogyoshaInfo = GetNogyoshaInfo(nogyoshaId.Value);

            // 農業者情報がある場合
            if (nogyoshaInfo != null)
            {
                // データロック取得
                lockStatus = LockUtil.GetDataLock(systemKbn, nogyoshaInfo.TodofukenCd, nogyoshaInfo.KumiaitoCd, nogyoshaInfo.ShishoCd, sysDateTime, ref lockUserId);

                logger.Debug("LockStatus : " + lockStatus);
                logger.Debug("LockUserId : " + lockUserId);

                if (lockStatus == LockUtil.LOCKED_STATE_LOCKED)
                {
                    // ロック有り
                    ModelState.AddModelError("MessageArea2", MessageUtil.Get("ME91013", "データ", lockUserId));
                    return true;
                }
                else if (lockStatus == LockUtil.LOCKED_STATE_ERROR)
                {
                    // エラー
                    logger.Fatal("データロック取得でエラーが発生しました。");
                    throw new SystemException(MessageUtil.Get("MF00001"));
                }
            }

            return false;
        }
        #endregion
        #endregion

        #region 新規登録処理
        /// <summary>
        /// 新規登録処理
        /// </summary>
        /// <param name="nogyoshaId">農業者ID</param>
        /// <param name="form">画面の入力値</param>
        /// <param name="sysDate">システム日時</param>
        /// <returns>True：登録成功　False：登録失敗</returns>
        private bool Insert(int? nogyoshaId, D0102Model form, DateTime sysDate)
        {
            // ユーザID
            var userId = Syokuin.UserId;

            using (var tx = getJigyoDb<NskAppContext>().Database.BeginTransaction())
            {
                var kanyusha = new TKanyusha
                {
                    NogyoshaId = nogyoshaId.Value,
                    KanyushaCd = form.EntryContent.KanyushaCd,
                    Nendo = form.EntryContent.Nendo.Value,
                    KouchiPostalCd = string.Concat(form.EntryContent.KouchiPostalCd1, form.EntryContent.KouchiPostalCd2),
                    KouchiAddressKana = form.EntryContent.KouchiAddressKana,
                    KouchiAddress = form.EntryContent.KouchiAddress,
                    KouchiMenseki = string.IsNullOrEmpty(form.EntryContent.KouchiMenseki) ? null : decimal.Parse(form.EntryContent.KouchiMenseki),
                    KouchiKeitaiCd = form.EntryContent.KouchiKeitaiCd,
                    KojinjohoToriatsukaiFlg = form.EntryContent.KojinjohoToriatsukaiFlg ? AppConst.FLG_ON : AppConst.FLG_OFF,
                    KanyuShinseiYmd = form.EntryContent.KanyuShinseiYmd,
                    Biko = form.EntryContent.Biko,
                    InsertDate = sysDate,
                    InsertUserId = userId,
                    UpdateDate = sysDate,
                    UpdateUserId = userId
                };
                getJigyoDb<NskAppContext>().TKanyushas.Add(kanyusha);

                try
                {
                    logger.Info("加入者情報登録処理を実行します。");
                    getJigyoDb<NskAppContext>().SaveChanges();
                    tx.Commit();
                }
                catch (DbUpdateException e)
                {
                    // 失敗の場合
                    logger.Error(MessageUtil.Get("MW00002", "登録"));
                    logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                    ModelState.AddModelError("MessageArea1", MessageUtil.Get("MW00002", "登録"));
                    tx.Rollback();
                    return false;
                }
                catch (Exception e)
                {
                    logger.Error(MessageUtil.Get("MF80001"));
                    logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                    ModelState.AddModelError("MessageArea1", MessageUtil.Get("MF80001"));
                    tx.Rollback();
                    return false;
                }
            }
            logger.Info("加入者情報登録成功");
            return true;
        }
        #endregion

        #region 更新登録処理
        /// <summary>
        /// 更新登録処理
        /// </summary>
        /// <param name="form">画面の入力値</param>
        /// <param name="kanyusha">加入者情報</param>
        /// <param name="sysDate">システム日時</param>
        /// <returns>True：登録成功　False：登録失敗</returns>
        private bool Update(D0102Model form, TKanyusha kanyusha, DateTime sysDate)
        {
            using (var tx = getJigyoDb<NskAppContext>().Database.BeginTransaction())
            {
                // 加入者情報を更新する
                kanyusha.KanyushaCd = form.EntryContent.KanyushaCd;
                kanyusha.Nendo = form.EntryContent.Nendo.Value;
                kanyusha.KouchiPostalCd = string.Concat(form.EntryContent.KouchiPostalCd1, form.EntryContent.KouchiPostalCd2);
                kanyusha.KouchiAddressKana = form.EntryContent.KouchiAddressKana;
                kanyusha.KouchiAddress = form.EntryContent.KouchiAddress;
                kanyusha.KouchiMenseki = string.IsNullOrEmpty(form.EntryContent.KouchiMenseki) ? null : decimal.Parse(form.EntryContent.KouchiMenseki);
                kanyusha.KouchiKeitaiCd = form.EntryContent.KouchiKeitaiCd;
                kanyusha.KojinjohoToriatsukaiFlg = form.EntryContent.KojinjohoToriatsukaiFlg ? AppConst.FLG_ON : AppConst.FLG_OFF;
                kanyusha.KanyuShinseiYmd = form.EntryContent.KanyuShinseiYmd;
                kanyusha.Biko = form.EntryContent.Biko;
                kanyusha.UpdateDate = sysDate;
                kanyusha.UpdateUserId = Syokuin.UserId;
                getJigyoDb<NskAppContext>().Entry(kanyusha).State = EntityState.Modified;

                try
                {
                    logger.Info("加入者情報更新処理を実行します。");
                    getJigyoDb<NskAppContext>().SaveChanges();
                    tx.Commit();
                }
                catch (DbUpdateException e)
                {
                    // 失敗の場合
                    logger.Error(MessageUtil.Get("MW00002", "更新"));
                    logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                    ModelState.AddModelError("MessageArea1", MessageUtil.Get("MW00002", "更新"));
                    tx.Rollback();
                    return false;
                }
                catch (Exception e)
                {
                    logger.Error(MessageUtil.Get("MF80001"));
                    logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                    ModelState.AddModelError("MessageArea1", MessageUtil.Get("MF80001"));
                    tx.Rollback();
                    return false;
                }
            }
            logger.Info("加入者情報更新成功");
            return true;
        }
        #endregion

        #region string? ⇒ string変換
        /// <summary>
        /// string? ⇒ string変換
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>string</returns>
        private string NullableStringToString(string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }
        #endregion

    }
}