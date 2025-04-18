using NskAppModelLibrary.Context;
using NskWeb.Areas.F106.Models.D106010;
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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NskWeb.Areas.F106.Controllers
{
    /// <summary>
    /// 一括帳票出力
    /// </summary>
    [Authorize(Roles = "bas")]
    [SessionOutCheck]
    [Area("F106")]
    public class D106010Controller : D106000BaseController
    {
        #region メンバー定数
        /// <summary>
        /// 画面ID(D106010)
        /// </summary>
        private static readonly string SCREEN_ID_D106010 = "D106010";

        /// <summary>
        /// セッションキー(D106010)
        /// </summary>
        private static readonly string SESS_D106010 = SCREEN_ID_D106010 + "_" + "SCREEN";

        /// <summary>
        /// 引受回一覧ビュー名（検索結果）
        /// </summary>
        private static readonly string RESULT_VIEW_NAME = "_D106010SearchResult";

        /// <summary>
        /// 画面名
        /// </summary>
        private static readonly string D106010_SCREEN_NM = "NSK_106010D";

        /// <summary>
        /// バッチ名
        /// </summary>
        private static readonly string D106010_BATCH_NM = "NSK_106011B";
        
        /// <summary>
        /// 複数実行禁止ID
        /// </summary>
        private static readonly string D106010_MULTI_ID = "NSK_106010B";

        /// <summary>
        /// 実行単位区分本所本所
        /// </summary>
        private static readonly string D106010_HIKIUKE_JIKKO_TANNI_KBN_HONSHO = "1";
        
        /// <summary>
        /// 実行単位区分本所支所
        /// </summary>
        private static readonly string D106010_HIKIUKE_JIKKO_TANNI_KBN_HONSYOSHISHO = "2";
        
        /// <summary>
        /// 実行単位区分支所支所
        /// </summary>
        private static readonly string D106010_HIKIUKE_JIKKO_TANNI_KBN_SHISHO = "3";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        /// <param name="serviceClient"></param>
        public D106010Controller(ICompositeViewEngine viewEngine, ReportServiceClient serviceClient) : base(viewEngine, serviceClient)
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

            // モデル状態ディクショナリからすべての項目を削除します。       
            ModelState.Clear();
            // セッション情報から検索条件、検索結果件数をクリアする
            SessionUtil.Remove(SESS_D106010, HttpContext);

            // ログインユーザの参照・更新可否判定
            // 画面IDをキーとして、画面マスタ、画面機能権限マスタを参照し、ログインユーザに本画面の権限がない場合は業務エラー画面を表示する。
            if (!ScreenSosaUtil.CanReference(SCREEN_ID_D106010, HttpContext))
            {
                //「参照権限」が許可されていない場合、業務エラー画面を表示する。
                throw new AppException("ME90003", MessageUtil.Get("ME90003"));
            }

            // 利用可能な支所一覧
            var shishoList = SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, HttpContext);

            // 画面モデルの生成
            D106010Model model = new D106010Model(Syokuin, shishoList);

            if (!ScreenSosaUtil.CanUpdate(SCREEN_ID_D106010, HttpContext))
            {
                // 更新権限がない場合は登録ボタン非活性フラグをtrueにする
                model.KengenFlg = "1";
            }
            else
            {
                // 参照権限も更新権限もある場合は登録ボタン非活性フラグをfalseにする
                model.KengenFlg = "0";
            }

            // 検索条件をセッションに保存する
            SessionUtil.Set(SESS_D106010, model, HttpContext);

            // パンくずリストを変更する
            SessionUtil.Set(CoreConst.SESS_BREADCRUMB, new List<string>() { "D000000" }, HttpContext);

            //共済目的コードおよび年産をセッションから取得
            NSKPortalInfoModel m = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);

            //セッション情報変数定義
            String KyosaiMokutekiCd;
            String NensanHikiuke;
            String ShishoJikkoHikiukeKbn;

            //セッションがnullの場合、空白を設定
            if (m != null) 
            {
                KyosaiMokutekiCd = m.SKyosaiMokutekiCd;
                NensanHikiuke = m.SNensanHikiuke;
                ShishoJikkoHikiukeKbn = m.SHikiukeJikkoTanniKbnHikiuke;
            }
            else 
            {
                KyosaiMokutekiCd = string.Empty;
                NensanHikiuke = string.Empty;
                ShishoJikkoHikiukeKbn = string.Empty;
            }

            // 組合等コードを職員情報から取得
            String KumiaitoCd = Syokuin.KumiaitoCd;

            //共済目的・年産・組合等コードが取得できなければメッセージエリア１を表示する
            if (string.IsNullOrEmpty(KyosaiMokutekiCd) || string.IsNullOrEmpty(NensanHikiuke) || string.IsNullOrEmpty(KumiaitoCd)) 
            {
                ModelState.AddModelError("MessageArea1", MessageUtil.Get("ME10098"));
            }

            //共済目的または年産を取得できれば表示する
            if (!string.IsNullOrEmpty(KyosaiMokutekiCd) || !string.IsNullOrEmpty(NensanHikiuke))
            {
                model.SearchCondition.Nensan = NensanHikiuke;
                model.SearchCondition.KyosaiMokutekiCd = KyosaiMokutekiCd;
                //共済目的コードが空白以外の時に名称を取得する
                if (!string.IsNullOrEmpty(KyosaiMokutekiCd))
                {
                    model.SearchCondition.KyosaiMokutekiNm = GetKyosaiMokutekiNM(KyosaiMokutekiCd);
                }
            }

            // 組合等コードを設定する
            model.SearchCondition.KumiaitoCd = KumiaitoCd;
            // 引受計算実行単位区分_引受を設定する
            model.SearchCondition.ShishoJikkoHikiukeKbn = ShishoJikkoHikiukeKbn;

            // 本所・支所ドロップダウンリストの取得
            if (ShishoJikkoHikiukeKbn.Equals(D106010_HIKIUKE_JIKKO_TANNI_KBN_HONSHO))
            {
                // 本所本所の場合は本所のみドロップダウンリストに表示
                model.SearchCondition.HonshoshishoList = setHonshoList(model);
            }
            else
            {
                // 本所本所(1)以外の場合は支所のドロップダウンリストを表示
                model.SearchCondition.HonshoshishoList = setShishoList(model);
            }

            // ドロップダウンリストの先頭データを取得
            var firstlist = model.SearchCondition.HonshoshishoList.FirstOrDefault();
            string selectvalue = firstlist.Value;

            // 引受計算処理（水稲）画面を表示する
            return View(SCREEN_ID_D106010, GetInitUnderwritingCycle(model, m, selectvalue));
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
            SessionUtil.Remove(SESS_D106010, HttpContext);

            return Json(new { result = "success" });
        }
        #endregion

        #region 本所・支所ドロップダウンリスト取得
        /// <summary>
        /// 本所のみのドロップダウンリストを設定
        /// </summary>
        /// <param name="model">画面viewモデル</param>
        /// <returns>List<SelectListItem></returns>
        protected List<SelectListItem> setHonshoList(D106010Model model)
        {
            model.SearchCondition.HonshoshishoModelList = getHonshoList();
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var honshoshisho in model.SearchCondition.HonshoshishoModelList)
            {
                SelectListItem item = new SelectListItem();
                item.Value = honshoshisho.HonshoshishoCd;
                item.Text = honshoshisho.HonshoshishoNm;
                list.Add(item);
            }
            return list;
        }

        /// <summary>
        /// 支所のドロップダウンリストを設定
        /// </summary>
        /// <param name="model">画面viewモデル</param>
        /// <returns>List<SelectListItem></returns>
        protected List<SelectListItem> setShishoList(D106010Model model)
        {
            model.SearchCondition.HonshoshishoModelList = getShishoList();
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var honshoshisho in model.SearchCondition.HonshoshishoModelList)
            {
                SelectListItem item = new SelectListItem();
                item.Value = honshoshisho.HonshoshishoCd;
                item.Text = honshoshisho.HonshoshishoNm;
                list.Add(item);
            }
            return list;
        }
        #endregion

        #region 引受回一覧取得メソッド(初期表示時)
        /// <summary>
        /// メソッド：引受回一覧を取得する(初期表示時)
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>引受回テーブルモデル</returns>
        private D106010Model GetInitUnderwritingCycle(D106010Model model, NSKPortalInfoModel portalmodel, string selectvalue)
        {
            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();
            // 引受回テーブルをクリアする
            model.SearchResult = new D106010SearchResult();

            Syokuin syokuin = Syokuin;

            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // セッションの引受計算支所実行単位区分を取得
            string ShishoJikkoHikiukeKbn = portalmodel.SHikiukeJikkoTanniKbnHikiuke;
            // 職員情報の本所支所コードを取得
            string ShishoCd = syokuin.ShishoCd;

            /* 引受回情報取得用SQLをセットする */
            // 実行単位区分が本所・本所の場合
            if (ShishoJikkoHikiukeKbn.Equals(D106010_HIKIUKE_JIKKO_TANNI_KBN_HONSHO))
            {
                // 実行単位区分が本所・本所の場合
                GetHonshoHikiukeList(sql, parameters, portalmodel);
            }
            else
            {
                // 実行単位区分が本所・本所以外の場合
                if (selectvalue.Equals(AppConst.HONSHO_CD))
                {
                    // 本所配下の支所引受回を取得するためShishoCdは空白を渡す
                    GetHonshoHaikaHikiukeList(sql, parameters, portalmodel, ShishoCd);
                }
                else 
                {
                    // ドロップダウンリストが本所以外の場合は選択されている支所の引受回を表示する
                    GetShishoHikiukeList(sql, parameters, portalmodel, selectvalue);
                }
            }

            // 引受回表示件数を取得する
            var totalCount = GetHikiukeDispCount(sql, parameters);
            // 引受回表示件数を画面表示用モデルに設定する
            model.SearchResult.TotalCount = totalCount;
            // 引受回表示件数が0件の場合
            if (totalCount == 0)
            {
                // 検索条件をセッションに保存する
                SessionUtil.Set(SESS_D106010, model, HttpContext);
                return model;
            }
            // 引受回表示結果ページの取得
            model.SearchResult.TableRecords = GetResultList(sql, parameters);

            // 実行ボタン制御値取得
            model.SearchResult.EnterCtrlFlg = GetEnterCntrlFlag(model.SearchResult.TableRecords);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D106010, model, HttpContext);

            return model;
        }
        #endregion

        #region 引受回一覧取得メソッド(ドロップダウン選択時)
        /// <summary>
        /// メソッド：引受回一覧を取得する(ドロップダウン選択時)
        /// </summary>
        /// <param name="model">ビューモデル</param>
        /// <returns>引受回テーブルモデル</returns>
        [HttpPost]
        public ActionResult DispUnderwritingCycle(D106010Model form)
        {
            // セッションから画面モデルを取得する
            var model = SessionUtil.Get<D106010Model>(SESS_D106010, HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                throw new SystemException(MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();
            // 引受回テーブルをクリアする
            form.SearchResult = new D106010SearchResult();

            // sql用定数定義
            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();

            // ポータルのセッションを取得
            NSKPortalInfoModel pmodel = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            // 画面の引受計算支所実行単位区分を取得
            string ShishoJikkoHikiukeKbn = pmodel.SHikiukeJikkoTanniKbnHikiuke;
            // 本所支所ドロップダウンリストで選択されている支所コードを取得
            string DropDownShishoCd = form.SearchCondition.SelectShishoCd;

            /* 引受回情報取得用SQLをセットする */
            // 実行単位区分が本所・本所の場合
            if (ShishoJikkoHikiukeKbn.Equals(D106010_HIKIUKE_JIKKO_TANNI_KBN_HONSHO))
            {
                // 実行単位区分が本所・本所の場合
                GetHonshoHikiukeList(sql, parameters, pmodel);
            }
            else
            {
                // 実行単位区分が本所・本所(1)以外の場合
                // ドロップダウンリストで本所を選択した場合
                if (DropDownShishoCd.Equals(AppConst.HONSHO_CD))
                {
                    GetHonshoHaikaHikiukeList(sql, parameters, pmodel, DropDownShishoCd);
                }
                else
                {
                    // ドロップダウンリストで本所以外を選択した場合
                    GetShishoHikiukeList(sql, parameters, pmodel, DropDownShishoCd);
                }
            }

            // 引受回表示件数を取得する
            var totalCount = GetHikiukeDispCount(sql, parameters);
            // 引受回表示件数を画面表示用モデルに設定する
            model.SearchResult.TotalCount = totalCount;
            // 引受回表示件数が0件の場合
            if (totalCount == 0)
            {
                // 検索条件をセッションに保存する
                SessionUtil.Set(SESS_D106010, model, HttpContext);

                return PartialView(RESULT_VIEW_NAME, model);
            }

            // 引受回表示結果ページの取得
            model.SearchResult.TableRecords = GetResultList(sql, parameters);


            // 実行ボタン制御値取得
            model.SearchResult.EnterCtrlFlg = GetEnterCntrlFlag(model.SearchResult.TableRecords);

            // 検索条件と検索結果をセッションに保存する
            SessionUtil.Set(SESS_D106010, model, HttpContext);

            return PartialView(RESULT_VIEW_NAME, model);
        }
        #endregion

        #region 引受回数の検索結果取得メソッド
        /// <summary>
        /// 引受回数の検索結果取得メソッド
        /// <summary>
        /// <param name="Sql">検索用SQL</param>
        /// <param name="parameters">検索用パラメータ</param>
        /// <returns>引受回数の検索結果</returns>
        private List<D106010TableRecord> GetResultList(StringBuilder Sql, List<NpgsqlParameter> parameters)
        {
            logger.Info("引受回数表示結果情報取得処理を実行します。");

            // 引受回表示検索結果データを取得
            var sqlResults = getJigyoDb<NskAppContext>().Database.SqlQueryRaw<D106010TableRecord>(Sql.ToString(), parameters.ToArray()).ToList();

            return sqlResults;
        }
        #endregion

        #region 実行ボタン制御値取得メソッド
        /// <summary>
        /// 実行ボタン制御値取得メソッド
        /// <summary>
        /// <param name="TableRecords">List<D106010TableRecord></param>
        /// <returns>実行ボタン制御値</returns>
        private string GetEnterCntrlFlag(List<D106010TableRecord> TableRecords)
        {
            string result = "1";
            foreach (var record in TableRecords)
            {
                if (record.KakuteiHikiukeCnt != null && record.KakuteiHikiukeCnt > 0)
                {
                    result = "0";   // 使用不可
                    break;
                }
                if (record.HoukokuCnt != null && record.HoukokuCnt > 0)
                {
                    result = "0";   // 使用不可
                    break;
                }
            }
            return result;
        }
        #endregion

        #region バッチ予約登録イベント
        /// <summary>
        /// イベント名：バッチ予約登録
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatBatchHikiukeCalc(D106010Model model)
        {
            // システム日時
            var systemDate = DateUtil.GetSysDateTime();

            //条件テーブルへの登録を実行
            // バッチ条件IDを取得する
            string BatchJokenId = GetBatchJoken();

            // バッチ条件（表示用）
            //var batchJokenDispSb = new StringBuilder();
            //batchJokenDispSb.Append("[年産：" + model.SearchCondition.Nensan + "]");
            //batchJokenDispSb.Append("[共済目的：" + model.SearchCondition.KyosaiMokutekiCd + "]");
            //batchJokenDispSb.Append("[支所コード：" + model.SearchCondition.SelectShishoCd + "]");

            // バッチ予約を登録する。
            string message = string.Empty;
            long batchId = 0L;
            int? insertResult = null;

            // 複数実行禁止IDを設定する
            String multiNotId = D106010_MULTI_ID;
            // 支所コードは付加しない（支所が異なっていても重複不可）
            //if (model.SearchCondition.ShishoJikkoHikiukeKbn == "3")
            //{
            //    multiNotId += model.SearchCondition.SelectShishoCd;
            //}

            // メッセージ用の支所名称を設定
            string msgShisho = model.SearchCondition.SelectShishoNm + " の引受計算処理(水稲)のバッチ登録";
            // レスポンスメッセージ用変数定義
            var responseMsg = string.Empty;

            //予約処理の実行
            try
            {
                var jigyodb = getJigyoDb<NskAppContext>();
                using (var tx = jigyodb.Database.BeginTransaction())
                {
                    try
                    {
                        logger.Info(string.Format("引受計算（水稲）のバッチ条件を登録します。"));
                        //引受年産を条件テーブルに登録
                        CreatBatchJoken(CreatBatchJokenNensan(model.SearchCondition.Nensan, Syokuin, BatchJokenId, systemDate, 1), jigyodb);

                        //共済目的コードを条件テーブルに登録
                        CreatBatchJoken(CreatBatchJokenKyosaiMokuteki(model.SearchCondition.KyosaiMokutekiCd, Syokuin, BatchJokenId, systemDate, 2), jigyodb);

                        // 支所コードを条件テーブルに登録
                        CreatBatchJoken(CreatBatchJokenShishoCd(model.SearchCondition.SelectShishoCd, Syokuin, BatchJokenId, systemDate, 3), jigyodb);

                        // 全てinsertが完了したらコミット
                        tx.Commit();
                    }
                    catch
                    {
                        // 失敗の場合ロールバックする
                        tx.Rollback();
                        throw;
                    }
                }

                insertResult = BatchUtil.InsertBatchYoyaku(AppConst.BatchBunrui.BATCH_BUNRUI_90_OTHER,
                                                            ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
                                                            Syokuin.TodofukenCd,
                                                            Syokuin.KumiaitoCd,
                                                            Syokuin.ShishoCd,
                                                            systemDate,
                                                            Syokuin.UserId,
                                                            D106010_SCREEN_NM,
                                                            D106010_BATCH_NM,
                                                            BatchJokenId,
                                                            BatchJokenId,
                                                            //batchJokenDispSb.ToString(),
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
                // （出力メッセージ：（メッセージID：ME01645、引数{0}："本所・支所コード+ " " + 本所・支所名 + の引受計算処理(水稲)のバッチ登録"））
                logger.Error("バッチ予約登録失敗");
                logger.Error(MessageUtil.Get("ME01645", msgShisho));
                logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                return Json(new { message = MessageUtil.Get("ME01645", msgShisho) });
            }

            // モデル状態ディクショナリからすべての項目を削除します。
            ModelState.Clear();

            // 処理結果がエラーだった場合
            if (insertResult == 0)
            {
                // （出力メッセージ：バッチ予約登録失敗）
                // （出力メッセージ：（メッセージID：ME90008、引数{0}："本所・支所コード+ " " + 本所・支所名 + の引受計算処理(水稲)のバッチ登録）"））
                logger.Error("バッチ予約登録失敗");
                logger.Error(MessageUtil.Get("ME01645", msgShisho));
                responseMsg = MessageUtil.Get("ME01645", msgShisho);
            }
            else
            {
                // バッチ予約完了メッセージ
                logger.Info("バッチ予約登録成功");
                logger.Info(MessageUtil.Get("MI00004", msgShisho));
                responseMsg = MessageUtil.Get("MI00004", msgShisho);
            }

            return Json(new { message = responseMsg });
        }
        #endregion
    }
}
