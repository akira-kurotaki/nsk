using NskAppModelLibrary.Context;
using NskWeb.Areas.F107.Models.D107050;
using NskWeb.Common.Consts;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReportService.Core;
using System.Text;
using NskWeb.Areas.F000.Models.D000000;
using static CoreLibrary.Core.Utility.BatchUtil;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NskWeb.Areas.F107.Controllers
{
    /// <summary>
    /// 一括帳票出力
    /// </summary>
    [Authorize(Roles = "bas")]
    [SessionOutCheck]
    [Area("F107")]
    public class D107050Controller : D107000BaseController
    {
        #region メンバー定数
        /// <summary>
        /// 画面ID(D107050)
        /// </summary>
        private static readonly string SCREEN_ID_D107050 = "D107050";

        /// <summary>
        /// セッションキー(D107050)
        /// </summary>
        private static readonly string SESS_D107050 = SCREEN_ID_D107050 + "_" + "SCREEN";

        /// <summary>
        /// バッチ名
        /// </summary>
        private static readonly string D107050_BATCH_NM = "NSK_107051B";

        /// <summary>
        /// 複数実行禁止ID
        /// </summary>
        private static readonly string D107050_MULTI_ID = "NSK_107050D";

        /// <summary>
        /// 実行単位区分本所本所
        /// </summary>
        private static readonly string D107050_HIKIUKE_JIKKO_TANNI_KBN_HONSHO = "1";
        
        /// <summary>
        /// 実行単位区分本所支所
        /// </summary>
        private static readonly string D107050_HIKIUKE_JIKKO_TANNI_KBN_HONSYOSHISHO = "2";
        
        /// <summary>
        /// 実行単位区分支所支所
        /// </summary>
        private static readonly string D107050_HIKIUKE_JIKKO_TANNI_KBN_SHISHO = "3";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        /// <param name="serviceClient"></param>
        public D107050Controller(ICompositeViewEngine viewEngine, ReportServiceClient serviceClient) : base(viewEngine, serviceClient)
        {
        }
        #endregion

        #region 初期表示イベント
        /// <summary>
        /// イベント：初期化
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Init()
        {

            //モデルを取得
            D107050Model model = new D107050Model();

            // ログインユーザの参照・更新可否判定
            // 画面IDをキーとして、画面マスタ、画面機能権限マスタを参照し、ログインユーザに本画面の権限がない場合は業務エラー画面を表示する。
            //「参照」「更新権限」いずれも許可されていない場合、業務エラー画面を表示する。
            if (!ScreenSosaUtil.CanReference(SCREEN_ID_D107050, HttpContext) && !ScreenSosaUtil.CanUpdate(SCREEN_ID_D107050, HttpContext))
            {
                throw new AppException("ME10075", MessageUtil.Get("ME10075"));
            }

            // 参照権限はあるが、更新権限がない場合は登録ボタン非活性フラグをtrueにする
            if (ScreenSosaUtil.CanReference(SCREEN_ID_D107050, HttpContext) && !ScreenSosaUtil.CanUpdate(SCREEN_ID_D107050, HttpContext))
            {
                ViewBag.UpdateFlg = 1;
            }
            // 参照権限も更新権限もある場合は登録ボタン非活性フラグをfalseにする
            if (ScreenSosaUtil.CanReference(SCREEN_ID_D107050, HttpContext) && ScreenSosaUtil.CanUpdate(SCREEN_ID_D107050, HttpContext))
            {
                ViewBag.UpdateFlg = 0;
            }

            // モデル状態ディクショナリからすべての項目を削除します。       
            ModelState.Clear();
            // セッション情報から検索条件、検索結果件数をクリアする
            SessionUtil.Remove(SESS_D107050, HttpContext);

            // 利用可能な支所一覧
            var shishoList = SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, HttpContext);

            // 画面項目の初期化
            model = new D107050Model(Syokuin, shishoList);

            // 検索条件をセッションに保存する
            SessionUtil.Set(SESS_D107050, model, HttpContext);

            // パンくずリストを変更する
            SessionUtil.Set(CoreConst.SESS_BREADCRUMB, new List<string>() { "D000000" }, HttpContext);

            //共済目的コード、年産、引受計算実行単位区分をセッションから取得
            NSKPortalInfoModel m = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);

            //セッション情報変数定義
            String KyosaiMokutekiCd;
            String NensanHikiuke;
            String ShishoJikkoHikiukeKbn;

            //セッションがnullの場合、業務エラー画面を表示
            if (m != null) 
            {
                KyosaiMokutekiCd = m.SKyosaiMokutekiCd;
                NensanHikiuke = m.SNensanHikiuke;
                ShishoJikkoHikiukeKbn = m.SHikiukeJikkoTanniKbnHikiuke;

                // 本所または支所ユーザーか判断する
                if (Syokuin.ShishoCd.Equals(""))
                {
                    // ログイン者のシステム利用区分が3の場合は本所ユーザー
                    @ViewBag.HonsyoUserFlg = 1;
                }
                else if (!Syokuin.ShishoCd.Equals(""))
                {
                    // ログイン者のシステム利用区分が4の場合は支所ユーザー
                    @ViewBag.HonsyoUserFlg = 0;
                }
            }
            else 
            {
                throw new AppException("ME90015", MessageUtil.Get("ME90015", "引受年産・共済目的・引受計算実行単位区分"));
            }

            // 取得したパラメータに一つでも空データがあればエラーにするそれ以外は表示する
            if (KyosaiMokutekiCd != "" && NensanHikiuke != "" && ShishoJikkoHikiukeKbn != "")
            {
                model.EntryCondition.Nensan = NensanHikiuke;
                model.EntryCondition.KyosaiMokutekiCd = KyosaiMokutekiCd;
                
                //共済目的コードが空白以外の時に名称を取得する
                if (KyosaiMokutekiCd != "")
                {
                    model.EntryCondition.KyosaiMokutekiNm = GetKyosaiMokutekiNM(KyosaiMokutekiCd);
                }

                // 引受計算実行単位区分_引受を画面パラメータへ設定する
                model.EntryCondition.ShishoJikkoHikiukeKbn = ShishoJikkoHikiukeKbn;
            }
            else
            {
                throw new AppException("ME90015", MessageUtil.Get("ME90015", "引受年産・共済目的・引受計算実行単位区分"));
            }

            //対象データ振替日にv_30020_口座振替結果から取得した振替日を設定する
            DateTime? FurikaeDate = GetTaisyoFurikaeYmd(Syokuin, m);
            model.EntryCondition.TaisyoFurikaeDate = FurikaeDate;

            // 本所・支所ドロップダウンリストをセットする
            model.EntryCondition.HonshoshishoList = setHonshoShishoList(model, m);

            // 消込み処理（還付・自動）（インタフェース）画面を表示する
            return View(SCREEN_ID_D107050, model);
        }
        #endregion

        #region 戻るイベント
        /// <summary>
        /// イベント名：戻る 
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Back()
        {
            // セッション情報から検索条件、検索結果件数をクリアする
            SessionUtil.Remove(SESS_D107050, HttpContext);

            return Json(new { result = "success" });
        }
        #endregion

        #region 本所・支所ドロップダウンリスト取得
        /// <summary>
        /// 本所・支所ドロップダウンリストを設定
        /// </summary>
        /// <param name="model">画面viewモデル</param>
        /// <returns>List<SelectListItem></returns>
        protected List<SelectListItem> setHonshoShishoList(D107050Model model, NSKPortalInfoModel pmodel)
        {
            model.EntryCondition.HonshoshishoModelList = getHonshoShishoList(pmodel);
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var honshoshisho in model.EntryCondition.HonshoshishoModelList)
            {
                SelectListItem item = new SelectListItem();
                item.Value = honshoshisho.HonshoshishoCd;
                item.Text = honshoshisho.HonshoshishoNm;
                list.Add(item);
            }
            return list;
        }
        #endregion

        #region 選択支所のカレント引受回を取得
        /// <summary>
        /// カレント引受回を取得する
        /// </summary>
        /// <param name="sql">検索sql</param>
        /// <param name="shishoCd">支所コード</param>
        /// <param name="parameters">パラメーター</param>
        /// <param name="syokuin">職員情報</param>
        /// <param syokuin="sql">職員情報</param>
        private int? GetUnderwritingCycle(StringBuilder sql, string shishoCd, List<NpgsqlParameter> parameters, Syokuin syokuin)
        {
            sql.Append("SELECT ");
            sql.Append("     MAX(引受回) AS  \"Value\" ");
            sql.Append("FROM ");
            sql.Append("    t_00010_引受回 ");

            sql.Append("WHERE '1' = '1' ");

            // [セッション：組合等]
            sql.Append("AND 組合等コード = @KumiaitoCd ");
            parameters.Add(new NpgsqlParameter("@KumiaitoCd", syokuin.KumiaitoCd));

            //共済目的コードおよび年産をセッションから取得
            NSKPortalInfoModel m = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);

            if (m != null)
            {
                // セッションがnull以外の時に条件とセッションの値をパラメータにセットする
                // [セッション：共済目的]
                sql.Append("AND 共済目的コード = @KyosaiMokutekiCd ");
                parameters.Add(new NpgsqlParameter("@KyosaiMokutekiCd", m.SKyosaiMokutekiCd));
                // [セッション：年産]
                sql.Append("AND 年産 = @Nensan ");
                parameters.Add(new NpgsqlParameter("@Nensan", short.Parse(m.SNensanHikiuke)));
            }

            // [画面：支所コード]
            sql.Append("AND 支所コード = @ShishoCd ");
            parameters.Add(new NpgsqlParameter("@ShishoCd", shishoCd));

            sql.Append("GROUP BY ");
            sql.Append("    組合等コード ");
            sql.Append("    ,共済目的コード ");
            sql.Append("    ,年産 ");
            sql.Append("    ,支所コード ");

            // 引受回表示検索結果データを取得
            logger.Info("カレント引受回数取得処理を実行します。");
            logger.Info(sql);

            return getJigyoDb<NskAppContext>().Database.SqlQueryRaw<int?>(sql.ToString(), parameters.ToArray()).SingleOrDefault() ?? null;
        }
        #endregion

        #region カレント引受回表示メソッド
        /// <summary>
        /// カレント引受回表示メソッド
        /// </summary>
        /// <param name="model">ビューモデル</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DispUnderwritingCycle(D107050Model form)
        {
            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();
            Syokuin syokuin = Syokuin;

            // 表示用引受回引数定義
            int? DispCnt = null;

            // 選択された支所の最大引受回を取得
            DispCnt = GetUnderwritingCycle(sql, form.EntryCondition.SelectShishoCd, parameters, syokuin);

            // 引受回数が取得できなかった場合はエラーにより処理を中断する
            if (DispCnt == null) 
            {
                logger.Info("カレント引受回取得エラー");
                logger.Error(MessageUtil.Get("MI10007", "0"));
                return Json(new { message = MessageUtil.Get("MI10007", "0") });
            }

            // カレント引受回を画面に返す
            logger.Info("カレント引受回を表示する");
            return Json(new { CurentHikiukeCnt = DispCnt });
        }
        #endregion

        #region バッチ予約登録イベント
        /// <summary>
        /// イベント名：バッチ予約登録
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Insert(D107050Model form)
        {
            // セッションから画面モデルを取得する
            var model = SessionUtil.Get<D107050Model>(SESS_D107050, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // ユーザIDの取得
            var userId = Syokuin.UserId;
            // システム日時
            var systemDate = DateUtil.GetSysDateTime();

            Syokuin syokuin = Syokuin;

            string ShishoCd = string.Empty;
            // 職員が本所の場合、セッションは空白設定のため"00"を設定する必要がある
            //if (Syokuin.ShishoCd.Equals(""))
            //{
            //    ShishoCd = "00";
            //}
            //else if (!Syokuin.ShishoCd.Equals(""))
            //{
            //    ShishoCd = syokuin.ShishoCd;
            //}

            // セッションの支所コードを取得(本所の場合は空)
            ShishoCd = syokuin.ShishoCd;

            // バッチ条件取得
            string batchJoken = GetBatchJoken();

            // バッチ条件（表示用）取得
            var batchJokenDispSb = CreateBatchJokenDsp(form.EntryCondition.Nensan,
                                                        form.EntryCondition.KyosaiMokutekiCd,
                                                        form.EntryCondition.SelectShishoCd,
                                                        form.EntryCondition.CurentHikiukeCnt.ToString(),
                                                        form.EntryCondition.TaisyoFurikaeDate.ToString(),
                                                        syokuin.UserId);

            // バッチ予約を登録する。
            int intAllCnt = 0;
            string message = string.Empty;
            long batchId = 0L;
            int? insertResult = null;
            List<BatchYoyaku> yoyakuResult = null;

            // 複数実行禁止IDを設定する(都道府県と選択された支所を付加)
            String multiNotId = D107050_MULTI_ID;
            //multiNotId += syokuin.TodofukenCd;
            multiNotId += syokuin.TodofukenCd + form.EntryCondition.SelectShishoCd;

            // レスポンスメッセージ用変数定義
            var responseMsg = "";

            try
            {
                // バッチ予約状況を取得
                var BatchYoyakuparam = BatchYoyakuJokyoParam(syokuin, ShishoCd, D107050_BATCH_NM);

                yoyakuResult = GetBatchYoyakuList(BatchYoyakuparam, false, ref intAllCnt, ref message);

            }
            catch (Exception e)
            {
                // （出力メッセージ：バッチ予約状況取得失敗）
                // （出力メッセージ：（メッセージID：ME10019、引数{0}："消込み処理(還付・自動)(インタフェース)"））
                logger.Error("バッチ予約状況取得失敗");
                responseMsg = MessageUtil.Get("ME10019", "消込み処理(還付・自動)(インタフェース)");
                logger.Error(responseMsg);
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));

                // エラーメッセージをレスポンス
                return Json(new { message = responseMsg });
            }

            // 未実行のバッチが予約されている場合、ここで処理を中断する
            if (yoyakuResult != null && yoyakuResult.Count > 0) 
            {
                responseMsg = MessageUtil.Get("ME10019", "消込み処理（還付・自動）（インタフェース）");
                logger.Error(responseMsg);

                // エラーメッセージをレスポンス
                return Json(new { message = responseMsg });
            }

            //予約処理の実行
            try {

                var jigyodb = getJigyoDb<NskAppContext>();
                using (var tx = jigyodb.Database.BeginTransaction())
                {
                    try
                    {
                        logger.Info(string.Format("消込処理(還付・自動)(インタフェース)のバッチ条件を登録します。"));
                        // 先に条件テーブルへの登録を実行
                        // 引受年産を条件テーブルに登録
                        CreatBatchJoken(CreatBatchJokenNensan(form.EntryCondition.Nensan, syokuin, batchJoken, systemDate, 1), jigyodb);
                        // 共済目的コードを条件テーブルに登録
                        CreatBatchJoken(CreatBatchJokenKyosaiMokuteki(form.EntryCondition.KyosaiMokutekiCd, syokuin, batchJoken, systemDate, 2), jigyodb);
                        // 支所コードを条件テーブルに登録
                        CreatBatchJoken(CreatBatchJokenShishoCd(form.EntryCondition.SelectShishoCd, syokuin, batchJoken, systemDate, 3), jigyodb);
                        // 引受回を条件テーブルに登録
                        CreatBatchJoken(CreatBatchJokenHikiukeCnt(form.EntryCondition.CurentHikiukeCnt, syokuin, batchJoken, systemDate, 4), jigyodb);
                        // 対象データ振替日を条件テーブルに登録
                        CreatBatchJoken(CreatBatchJokenTaisyoFurikaeDate(form.EntryCondition.TaisyoFurikaeDate, syokuin, batchJoken, systemDate, 5), jigyodb);
                        // ユーザーIDを条件テーブルに登録
                        CreatBatchJoken(CreatBatchJokenUseiId(syokuin, batchJoken, systemDate, 6), jigyodb);

                        // 全てinsertが完了したらコミット
                        tx.Commit();
                    }
                    catch (DbUpdateException e)
                    {
                        // 失敗の場合ロールバックする
                        tx.Rollback();
                        throw;
                    }
                }

                // バッチ予約テーブルへの登録
                insertResult = BatchUtil.InsertBatchYoyaku(AppConst.BatchBunrui.BATCH_BUNRUI_90_OTHER, //後でその他に変更する必要あり
                                                        ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
                                                        Syokuin.TodofukenCd,
                                                        Syokuin.KumiaitoCd,
                                                        ShishoCd,
                                                        systemDate,
                                                        Syokuin.UserId,
                                                        "NSK_107050D",
                                                        D107050_BATCH_NM,
                                                        batchJoken,
                                                        batchJokenDispSb.ToString(),
                                                        "1",
                                                        AppConst.BatchType.BATCH_TYPE_PATROL,
                                                        systemDate,
                                                        "1",
                                                        ref message,
                                                        ref batchId,
                                                        multiNotId);
            }
            catch (Exception e)
            {
                // （出力メッセージ：バッチ予約登録失敗）
                // （出力メッセージ：（メッセージID：ME01645、引数{0}："バッチの予約登録"））
                logger.Error("バッチ予約登録失敗");
                responseMsg = MessageUtil.Get("ME01645", "バッチの予約登録");
                logger.Error(responseMsg);
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));

                // エラーメッセージをレスポンス
                return Json(new {message = responseMsg });
            }

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // 処理結果がエラーだった場合
            if (insertResult == 0)
            {
                logger.Info("バッチ予約登録失敗");
                responseMsg = MessageUtil.Get("ME01645", "バッチの予約登録");
                logger.Error(responseMsg);
            }
            else
            {
                // バッチ予約完了メッセージ
                responseMsg = MessageUtil.Get("MI00004", "バッチの予約登録");
            }

            // 画面情報をセッションに保存する
            SessionUtil.Set(SESS_D107050, form, HttpContext);

            return Json(new { message = responseMsg });
        }
        #endregion
    }
}
