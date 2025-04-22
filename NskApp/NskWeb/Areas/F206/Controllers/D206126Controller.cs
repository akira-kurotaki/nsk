using NskWeb.Common.Consts;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.CodeAnalysis;
using Pipelines.Sockets.Unofficial.Arenas;
using NskWeb.Areas.F206.Models.D206126;
using NskWeb.Areas.F000.Models.D000000;
using ModelLibrary.Models;
using NskAppModelLibrary.Models;
using NskAppModelLibrary.Context;
using NskWeb.Areas.F000.Models.D000999;
using NskWeb.Areas.F206.Consts;
using NskCommonLibrary.Core.Consts;
using Microsoft.AspNetCore.Mvc.Rendering;
using NskConsts = NskCommonLibrary.Core.Consts;

namespace NskWeb.Areas.F206.Controllers
{
    /// <summary>
    /// NSK_206126D_平均単収差計算処理
    /// </summary>
    /// <remarks>
    /// 作成日：2025/04/15
    /// 作成者：ネクスト松嶋
    /// </remarks>
    [ExcludeAuthCheck]
    [AllowAnonymous]
    [Area("F206")]
    public class D206126Controller : CoreController
    {
        public D206126Controller(ICompositeViewEngine viewEngine, IWebHostEnvironment webHostEnvironment) : base(viewEngine)
        {
        }


        public ActionResult Init()

        {
            // ログインユーザの参照・更新可否判定
            // 画面IDをキーとして、画面マスタ、画面機能権限マスタを参照し、ログインユーザに本画面の権限がない場合は業務エラー画面を表示する。
            if (!ScreenSosaUtil.CanReference(F206Const.SCREEN_ID_D206126, HttpContext))
            {
                throw new AppException("ME90003", MessageUtil.Get("ME90003"));
            }

            //var pagefrom = HttpContext.Request.Query[CoreConst.SCREEN_PAGE_FROM];
            var syokuin = SessionUtil.Get<Syokuin>("_D9000_LOGIN_USER", HttpContext);
            if (syokuin == null)
            {
                ModelState.AddModelError("MessageArea", MessageUtil.Get("ME01033"));
                D000999Model d000999Model = GetInitModel();
                d000999Model.UserId = "";
                return View("D000999_Pre", d000999Model);
            }

            //// モデル初期化
            D206126Model model = new D206126Model
            {
                // 「ログイン情報」を取得する
                VSyokuinRecords = getJigyoDb<NskAppContext>().VSyokuins.Where(t => t.UserId == Syokuin.UserId).Single()
            };

            var updateKengen = ScreenSosaUtil.CanUpdate(F206Const.SCREEN_ID_D206126, HttpContext);
            model.UpdateKengenFlg = updateKengen;

            NSKPortalInfoModel md = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);
            if (md != null)
            {
                model.D206126Info.SKyosaiMokutekiCd = md.SKyosaiMokutekiCd;
                model.D206126Info.SNensanHyoka = md.SNensanHyoka;

            }

            logger.Debug(md.SKyosaiMokutekiCd);

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
            SessionUtil.Set(F206Const.SESS_D206126, model, HttpContext);
            return View(F206Const.SCREEN_ID_D206126, model);
        }

        #region バッチ登録イベント
        /// <summary>
        /// イベント名：バッチ登録
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatBatchReport(D206126Model form)
        {
            logger.Debug("START CreatBatchReport");

            var model = SessionUtil.Get<D206126Model>(F206Const.SESS_D206126, HttpContext);
            logger.Debug(model == null);
            DbConnectionInfo dbConnectionInfo =
                DBUtil.GetDbConnectionInfo(ConfigUtil.Get("SystemKbn")
                , Syokuin.TodofukenCd
                , Syokuin.KumiaitoCd
                , Syokuin.ShishoCd);

            // セッションに自画面のデータが存在しない場合
            if (model == null)
            {
                return Json(new { success = false, message = MessageUtil.Get("ME01645", "セッション情報の取得") });
            }

            // バッチ予約状況取得引数の設定
            BatchUtil.GetBatchYoyakuListParam param = new()
            {
                SystemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
                TodofukenCd = Syokuin.TodofukenCd,
                KumiaitoCd = Syokuin.KumiaitoCd,
                ShishoCd = Syokuin.ShishoCd,
                BatchNm = F206Const.BATCH_ID_NSK_206111B
            };
            // 総件数取得フラグ
            bool boolAllCntFlg = false;
            // 件数（出力パラメータ）
            int intAllCnt = 0;
            // エラーメッセージ（出力パラメータ）
            string message = string.Empty;

            // バッチ予約状況取得（BatchUtil.GetBatchYoyakuList()）を呼び出し、バッチ予約状況を取得する。
            List<BatchYoyaku> batchYoyakuList = BatchUtil.GetBatchYoyakuList(param, boolAllCntFlg, ref intAllCnt, ref message);

            // NSKポータル情報の取得
            NSKPortalInfoModel md = SessionUtil.Get<NSKPortalInfoModel>(AppConst.SESS_NSK_PORTAL, HttpContext);

            if (intAllCnt >= 1 &&
                    batchYoyakuList
                        .Where(b => b.BatchStatus == BatchUtil.BATCH_STATUS_WAITING)
                        .ToList() is var waitingList && waitingList.Any())
            {
                using (var db1 = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
                {
                    var jokenIds = waitingList
                        .Select(w => w.BatchJoken)
                        .Where(id => id != null)
                        .Distinct()
                        .ToList();

                    bool hasMatching = db1.T01050バッチ条件s
                        .Any(t =>
                            jokenIds.Contains(t.バッチ条件id) &&
                            t.条件名称 == JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD &&
                            t.条件値 == md.SKyosaiMokutekiCd &&
                            db1.T01050バッチ条件s.Any(t2 =>
                                t2.バッチ条件id == t.バッチ条件id &&
                                t2.条件名称 == JoukenNameConst.JOUKEN_NENSAN &&
                                t2.条件値 == md.SNensanHyoka));

                    if (hasMatching)
                    {
                        return Json(new
                        {
                            success = false,
                            message = MessageUtil.Get("ME10019", "平均単収差計算処理")
                        });
                    }
                }
            }


            // ユーザIDの取得
            var userId = Syokuin.UserId;
            // システム日時
            var systemDate = DateUtil.GetSysDateTime();

            // 条件IDを取得する
            string strJoukenId = Guid.NewGuid().ToString();
            string strJoukenErrorMsg = string.Empty;

            // 条件を登録する
            strJoukenErrorMsg = InsertTJouken(dbConnectionInfo, model, strJoukenId, md);
            if (!string.IsNullOrEmpty(strJoukenErrorMsg))
            {
                return Json(new { success = false, message = strJoukenErrorMsg });
            }

            // バッチ予約登録
            var refMsg = string.Empty;
            long batchId = 0;
            // バッチ条件（表示用）作成
            var displayJouken = NskConsts.JoukenNameConst.JOUKEN_NENSAN + "、"  + NskConsts.JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD; 
            //ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
            // バッチ予約登録
            int? result = null;
            try
            {
                logger.Info("バッチ予約登録処理を実行する。");
                result = BatchUtil.InsertBatchYoyaku(AppConst.BatchBunrui.BATCH_BUNRUI_90_OTHER,
                ConfigUtil.Get(CoreLibrary.Core.Consts.CoreConst.APP_ENV_SYSTEM_KBN),
                Syokuin.TodofukenCd,
                Syokuin.KumiaitoCd,
                Syokuin.ShishoCd,
                DateUtil.GetSysDateTime(),
                Syokuin.UserId,
                F206Const.SCREEN_ID_NSK_D206126,
                F206Const.BATCH_ID_NSK_206111B,
                strJoukenId,
                displayJouken,
                AppConst.FLG_OFF,
                AppConst.BatchType.BATCH_TYPE_PATROL,
                null,
                AppConst.FLG_OFF,
                ref refMsg,
                ref batchId,
                F206Const.SCREEN_ID_NSK_D206126 + Syokuin.TodofukenCd
                );
            }
            catch (Exception e)
            {
                // （出力メッセージ：バッチ予約登録失敗）
                // （出力メッセージ：（メッセージID：ME90008、引数{0}："（"+｢４．２．３．｣で返却されたメッセージ + "）"））
                logger.Error("バッチ予約登録失敗1");
                logger.Error(MessageUtil.Get("ME90008", "（" + refMsg + "）"));
                //logger.Error(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
                logger.Error(MessageUtil.GetErrorMessage(e, 10));
                return Json(new { success = false, message = MessageUtil.Get("ME90005") });
            }

            // 処理結果がエラーだった場合
            if (result == 0)
            {
                // （出力メッセージ：バッチ予約登録失敗）
                // （出力メッセージ：（メッセージID：ME90008、引数{0}："（"+｢４．２．３．｣で返却されたメッセージ + "）"））
                logger.Error("バッチ予約登録失敗2");
                logger.Error(MessageUtil.Get("ME90008", "（" + refMsg + "）"));
                return Json(new { success = false, message = MessageUtil.Get("ME90005") });
            }


            // （出力メッセージ：バッチ予約登録成功）
            logger.Info("バッチ予約登録成功");
            return Json(new { success = true, message = MessageUtil.Get("MI00004", "平均単収差計算処理") });
        }
        #endregion



        #region Macの濁点や半濁点を変換する
        /// <summary>
        /// Macの濁点や半濁点を変換する
        /// </summary>
        /// <param name="str">変更対象</param>
        /// <returns>返還結果</returns>
        private string ChangeMacDakuten(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            string result = str;
            // NFCかどうかを判定
            if (!result.IsNormalized())
            {
                result = result.Normalize();
            }
            return result;
        }
        #endregion


        #region 条件登録メッソド
        /// <summary>
        /// メッソド:条件登録
        /// </summary>
        /// <param name="model">画面モデル</param>
        /// <param name="joukenId">条件ID</param>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>登録結果</returns>
        private string InsertTJouken(DbConnectionInfo dbConnectionInfo, D206126Model model, string joukenId, NSKPortalInfoModel md)
        {
            // ユーザID
            var userId = Syokuin.UserId;
            // システム日時
            var systemDate = DateUtil.GetSysDateTime();

            // 連番を手動で初期化
            int serialNumber = 0;

            logger.Debug("md.SNensanHyoka : " + md.SNensanHyoka);


            // DbContext を一度だけ使用する
            using (var db1 = new NskAppContext(dbConnectionInfo.ConnectionString, dbConnectionInfo.DefaultSchema))
            {
                // トランザクションを開始
                var transaction = db1.Database.BeginTransaction();

                try
                {
                    // 条件情報を追加する共通処理
                    var nensan = new T01050バッチ条件
                    {
                        バッチ条件id = joukenId,
                        連番 = ++serialNumber,
                        条件名称 = JoukenNameConst.JOUKEN_NENSAN,
                        表示用条件値 = JoukenNameConst.JOUKEN_NENSAN,
                        条件値 = md.SNensanHyoka,
                        登録日時 = systemDate,
                        登録ユーザid = userId,
                        更新日時 = systemDate,
                        更新ユーザid = userId
                    };
                    db1.T01050バッチ条件s.Add(nensan);


                    // 共済の条件をそのままINSERT
                    var kyosai = new T01050バッチ条件
                    {
                        バッチ条件id = joukenId,
                        連番 = ++serialNumber,
                        条件名称 = JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD,
                        表示用条件値 = JoukenNameConst.JOUKEN_KYOSAI_MOKUTEKI_CD,
                        条件値 = md.SKyosaiMokutekiCd,
                        登録日時 = systemDate,
                        登録ユーザid = userId,
                        更新日時 = systemDate,
                        更新ユーザid = userId
                    };
                    db1.T01050バッチ条件s.Add(kyosai);

                    // すべてのエンティティを一括保存
                    db1.SaveChanges();

                    // トランザクションコミット
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    logger.Debug($"Exception: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        logger.Debug($"Inner Exception: {ex.InnerException.Message}");
                        if (ex.InnerException.InnerException != null)
                        {
                            logger.Debug($"Inner Exception (Level 2): {ex.InnerException.InnerException.Message}");
                        }
                    }
                    logger.Debug($"StackTrace: {ex.StackTrace}");
                    logger.Error("バッチ条件登録失敗");
                    transaction.Rollback();
                    return MessageUtil.Get("ME90005");
                }
            }
            return string.Empty;
        }

        #endregion

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