using System.Data;
using System.Text;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore.Storage;
using ModelLibrary.Models;
using NskAppModelLibrary.Context;
using NskWeb.Areas.F111.Consts;
using NskWeb.Areas.F111.Models.D111010;
using NskWeb.Common.Consts;

namespace NskWeb.Areas.F111.Controllers
{
    [AllowAnonymous]
    [ExcludeAuthCheck]
    [Area("F111")]
    public class D111010Controller : CoreController
    {

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public D111010Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        // GET: F111/D111010
        public ActionResult Index()
        {
            if (ConfigUtil.Get(CoreConst.D0000_DISPLAY_FLAG) == "true")
            {
                // 画面表示モードを設定
                SetScreenModeFromQueryString();

            }

            return RedirectToAction("Init", F111Const.SCREEN_ID_D111010, new { area = "F111" });
        }

        /// <summary>
        /// 画面 初期表示
        /// </summary>
        /// <remarks>画面を初期表示する。</remarks>
        /// <returns>加入申込書入力（麦）危険段階区分設定画面表示結果</returns>
        public ActionResult Init()
        {
            // １．セッション情報をクリアする。
            SessionUtil.Remove(F111Const.SESS_D111010, HttpContext);

            D111010Model model = new();

            // １．１．権限チェック
            // (1)	ログインユーザの権限が「参照」
            // 「更新権限」いずれも許可されていない場合、メッセージを設定し業務エラー画面を表示する。
            bool dispKengen = ScreenSosaUtil.CanReference(F111Const.SCREEN_ID_D111010, HttpContext);
            bool updKengen = ScreenSosaUtil.CanUpdate(F111Const.SCREEN_ID_D111010, HttpContext);
            model.DispKengen = F111Const.Authority.None;
            if (updKengen)
            {
                model.DispKengen = F111Const.Authority.Update;// "更新権限";
            }
            else if (dispKengen)
            {
                model.DispKengen = F111Const.Authority.ReadOnly;// "参照権限";
            }
            else
            {
                throw new SystemException(MessageUtil.Get("ME10075"));
            }


            // ２．画面表示用データを取得する。	
            // ２．１．セッションから「都道府県コード」「組合等コード」「年産」「共済目的コード」「支所コード」を取得する。
            D111010SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            NskAppContext dbContext = getJigyoDb<NskAppContext>();

            // ２．３．交付徴収額データを取得する。
            model.KoufukinKeisan.GetResult(dbContext, sessionInfo);

            // ３．画面項目設定
            // ３．１．[２．１．][２．２．][２．３．]で取得した値を設定する。
            model.Nensan = $"{sessionInfo.Nensan}";
            model.FutankinKofuKbnCd = $"{sessionInfo.FutankinKofuKbnCd}";
            model.FutankinKofuKbn = $"{sessionInfo.FutankinKofuKbn}";


            // 画面制御用パラメータ取得
            GetConrtollParam(ref model);

            // 結果をセッションに保存する
            SessionUtil.Set(F111Const.SESS_D111010, model, HttpContext);

            ModelState.Clear();

            // 交付金計算処理画面を表示する。
            return View(F111Const.SCREEN_ID_D111010, model);
        }

        #region 表示更新ボタンイベント

        /// <summary>
        /// 表示更新ボタン
        /// </summary>
        /// <remarks>交付金計算処理の一覧を再表示する。</remarks>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult UpdateDisplay()
        {
            
            // １．最新データの取得
            // １．１．No.1の[２．][３．]の処理を実施する
            D111010SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            // セッションを取得
            D111010Model model = SessionUtil.Get<D111010Model>(F111Const.SESS_D111010, HttpContext);

            // 交付徴収額データをクリア
            model.KoufukinKeisan = new();

            NskAppContext dbContext = getJigyoDb<NskAppContext>();

            model.KoufukinKeisan.GetResult(dbContext, sessionInfo);

            // 画面制御用パラメータ取得
            GetConrtollParam(ref model);

            // 結果をセッションに保存する
            SessionUtil.Set(F111Const.SESS_D111010, model, HttpContext);

            // １．２．部分ビューを構築する
            JsonResult resultArea = PartialViewAsJson("_D111010KoufukinKeisan", model);

            return Json(new { resultArea = resultArea.Value, model.noneKoufukaiFlg, model.batchYoyakuFlg, model.syokaiKoufukaiFlg, model.saishinChoshuGakuNyuryokuzumiFlg });

        }
        #endregion

        #region 掛金徴収額入力ボタンイベント
        /// <summary>
        /// 掛金徴収額入力ボタン
        /// </summary>
        /// <remarks>「交付金計算掛金徴収額入力」画面に遷移する。</remarks>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ActionResult MoveKofukinNyuryoku(string guid)
        {

            D111010Model model = SessionUtil.Get<D111010Model>(F111Const.SESS_D111010, HttpContext);
            D111010SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }


            // バッチ予約状況取得引数の設定
            BatchUtil.GetBatchYoyakuListParam param = new()
            {
                SystemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
                TodofukenCd = Syokuin.TodofukenCd,
                KumiaitoCd = Syokuin.KumiaitoCd,
                ShishoCd = Syokuin.ShishoCd,
                BatchNm = F111Const.BATCH_ID_NSK_111011B
            };
            // 総件数取得フラグ
            bool boolAllCntFlg = false;
            // 件数（出力パラメータ）
            int intAllCnt = 0;
            // エラーメッセージ（出力パラメータ）
            string message = string.Empty;

            // １．バッチ予約状況確認
            // １．１．バッチ予約状況取得
            // バッチ予約状況取得登録（BatchUtil.GetBatchYoyakuList()）を呼び出し、バッチ予約状況を取得する。
            List<BatchYoyaku> batchYoyakus = BatchUtil.GetBatchYoyakuList(param, boolAllCntFlg, ref intAllCnt, ref message);

            // バッチ予約が存在する場合、
            if (intAllCnt >= 1)
            {

                // バッチ予約が存在する場合
                BatchYoyaku? batchYoyaku = batchYoyakus.FirstOrDefault(x => x.BatchStatus == BatchUtil.BATCH_STATUS_WAITING);
                if (batchYoyaku is not null)
                {
                    // １．２．未実行のバッチが予約されていた場合、[メッセージエリア１]に以下のメッセージを表示して処理を中止する。
                    model.MessageArea1 = MessageUtil.Get("ME10019", "交付金計算");

                    ModelState.AddModelError("MessageArea1", model.MessageArea1);

                    // 画面制御用パラメータ取得
                    GetConrtollParam(ref model);

                    // メッセージ反映のため部分ビューを構築する
                    // 返却用パラメータ
                    JsonResult messageArea1 = PartialViewAsJson("_D111010KoufukinKeisan", model, model.MessageArea1);

                    return Json(new { messageArea1 = messageArea1.Value, model.noneKoufukaiFlg, model.batchYoyakuFlg, model.syokaiKoufukaiFlg, model.saishinChoshuGakuNyuryokuzumiFlg });

                }
            }


            // 交付金計算処理の一覧から最新交付回を取得
            D111010KoufukinKeisanRecord? rec = model.KoufukinKeisan.DispRecords.MaxBy(x => x.Koufukai);

            // ２．入力対象行のデータをセッションに登録する。
            D111010ParamModel paramModel = new()
            {
                FutankinKofuKbnCd = sessionInfo.FutankinKofuKbnCd,
                // 暫定対応で交付回+1
                Koufukai = rec.Koufukai
            };
            SessionUtil.Set(F111Const.SESS_D111010_PARAMS, paramModel, HttpContext);

            //３．「交付金計算掛金徴収額入力」画面へリダイレクトする。
            string redirectLink = Url.Action("Init", F111Const.SCREEN_ID_D111050, new { area = "F111" });
            return Json(new { redirectLink });
        }
        #endregion

        #region 交付金計算ボタンイベント

        /// <summary>
        /// 交付金計算ボタン
        /// </summary>
        /// <remarks>交付金計算処理バッチを予約登録する。</remarks>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult KofukinKeisanYoyaku()
        {
            // 返却用パラメータ
            JsonResult messageArea1;

            D111010Model model = SessionUtil.Get<D111010Model>(F111Const.SESS_D111010, HttpContext);
            D111010SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }

            // バッチ予約状況取得引数の設定
            BatchUtil.GetBatchYoyakuListParam param = new()
            {
                SystemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
                TodofukenCd = Syokuin.TodofukenCd,
                KumiaitoCd = Syokuin.KumiaitoCd,
                ShishoCd = Syokuin.ShishoCd,
                BatchNm = F111Const.BATCH_ID_NSK_111011B
            };
            // 総件数取得フラグ
            bool boolAllCntFlg = false;
            // 件数（出力パラメータ）
            int intAllCnt = 0;
            // エラーメッセージ（出力パラメータ）
            string message = string.Empty;

            // ２．バッチ予約状況確認
            // ２．１．バッチ予約状況取得
            // バッチ予約状況取得登録（BatchUtil.GetBatchYoyakuList()）を呼び出し、バッチ予約状況を取得する。
            List<BatchYoyaku> batchYoyakus = BatchUtil.GetBatchYoyakuList(param, boolAllCntFlg, ref intAllCnt, ref message);

            // バッチ予約が存在する場合、
            if (intAllCnt >= 1)
            {

                // バッチ予約が存在する場合
                BatchYoyaku? batchYoyaku = batchYoyakus.FirstOrDefault(x => x.BatchStatus == BatchUtil.BATCH_STATUS_WAITING);
                if (batchYoyaku is not null)
                {
                    // １．２．未実行のバッチが予約されていた場合、[メッセージエリア１]に以下のメッセージを表示して処理を中止する。
                    model.MessageArea1 = MessageUtil.Get("ME10019", "交付金計算");

                    ModelState.AddModelError("MessageArea1", model.MessageArea1);

                    // 画面制御用パラメータ取得
                    GetConrtollParam(ref model);

                    // メッセージ反映のため部分ビューを構築する
                    messageArea1 = PartialViewAsJson("_D111010KoufukinKeisan", model, model.MessageArea1);

                    return Json(new { messageArea1 = messageArea1.Value, model.noneKoufukaiFlg, model.batchYoyakuFlg, model.syokaiKoufukaiFlg, model.saishinChoshuGakuNyuryokuzumiFlg });

                }

            }

            // ３．バッチ予約登録
            // ３．１．バッチ条件を登録する。
            // (1) バッチ条件IDを取得する。
            string jid = Guid.NewGuid().ToString("D");

            // 交付金計算処理の一覧から最新交付回を取得
            D111010KoufukinKeisanRecord? rec = model.KoufukinKeisan.DispRecords.MaxBy(x => x.Koufukai);

            // (2) バッチ条件値を生成する。
            var batchJokenDispSb = new StringBuilder();
            batchJokenDispSb.Append("負担金交付区分：" + $"{sessionInfo.FutankinKofuKbnCd}" + Environment.NewLine);
            batchJokenDispSb.Append("交付回：" + rec.Koufukai + Environment.NewLine);
            batchJokenDispSb.Append("年産：" + $"{sessionInfo.Nensan}" + Environment.NewLine);

            // ３．２．バッチ予約を登録する。
            long batchId = 0L;
            var insertResult = BatchUtil.InsertBatchYoyaku(AppConst.BatchBunrui.BATCH_BUNRUI_90_OTHER,
                                                            ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
                                                            Syokuin.TodofukenCd,
                                                            Syokuin.KumiaitoCd,
                                                            Syokuin.ShishoCd,
                                                            DateUtil.GetSysDateTime(),
                                                            Syokuin.UserId,
                                                            F111Const.SCREEN_ID_NSK_111010D,
                                                            F111Const.BATCH_ID_NSK_111011B,
                                                            jid,
                                                            batchJokenDispSb.ToString(),
                                                            "0",
                                                            AppConst.BatchType.BATCH_TYPE_PATROL,
                                                            DateUtil.GetSysDateTime(),
                                                            "1",
                                                            ref message,
                                                            ref batchId);

            // バッチの予約登録結果取得
            if (insertResult == 0)
            {
                model.MessageArea1 = MessageUtil.Get("ME01645", "バッチの予約登録");
                logger.Error(message);
                logger.Error(model.MessageArea1);
                ModelState.AddModelError("MessageArea1", model.MessageArea1);

            }
            else
            {

                model.MessageArea1 = MessageUtil.Get("MI00004", "バッチの予約登録");
                logger.Info(model.MessageArea1);
                ModelState.AddModelError("MessageArea1", model.MessageArea1);

            }

            // 画面制御用パラメータ取得
            GetConrtollParam(ref model);

            // メッセージ反映のため部分ビューを構築する
            messageArea1 = PartialViewAsJson("_D111010KoufukinKeisan", model, model.MessageArea1);

            return Json(new { messageArea1 = messageArea1.Value, model.noneKoufukaiFlg, model.batchYoyakuFlg, model.syokaiKoufukaiFlg, model.saishinChoshuGakuNyuryokuzumiFlg });

        }
        #endregion

        #region 解除ボタンイベント

        /// <summary>
        /// 解除ボタン
        /// </summary>
        /// <remarks>最新交付回の交付徴収額データを解除（削除）する。</remarks>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult DelKofukin()
        {

            D111010Model model = SessionUtil.Get<D111010Model>(F111Const.SESS_D111010, HttpContext);
            D111010SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            IDbContextTransaction? transaction = null;

            string errMessage = string.Empty;

            // セッションに自画面のデータが存在しない場合
            if (model is null)
            {
                throw new AppException("MF00005", MessageUtil.Get("MF00005", "セッションから画面情報を取得できませんでした"));
            }


            // バッチ予約状況取得引数の設定
            BatchUtil.GetBatchYoyakuListParam param = new()
            {
                SystemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
                TodofukenCd = Syokuin.TodofukenCd,
                KumiaitoCd = Syokuin.KumiaitoCd,
                ShishoCd = Syokuin.ShishoCd,
                BatchNm = F111Const.BATCH_ID_NSK_111011B
            };
            // 総件数取得フラグ
            bool boolAllCntFlg = false;
            // 件数（出力パラメータ）
            int intAllCnt = 0;
            // エラーメッセージ（出力パラメータ）
            string message = string.Empty;

            // ２．バッチ予約状況確認
            // ２．１．バッチ予約状況取得
            // バッチ予約状況取得登録（BatchUtil.GetBatchYoyakuList()）を呼び出し、バッチ予約状況を取得する。
            List<BatchYoyaku> batchYoyakus = BatchUtil.GetBatchYoyakuList(param, boolAllCntFlg, ref intAllCnt, ref message);

            // バッチ予約が存在する場合、
            if (intAllCnt >= 1)
            {

                // バッチ予約が存在する場合
                BatchYoyaku? batchYoyaku = batchYoyakus.FirstOrDefault(x => x.BatchStatus == BatchUtil.BATCH_STATUS_WAITING);
                if (batchYoyaku is not null)
                {
                    // １．２．未実行のバッチが予約されていた場合、[メッセージエリア１]に以下のメッセージを表示して処理を中止する。
                    model.MessageArea1 = MessageUtil.Get("ME10019", "交付金計算");

                    ModelState.AddModelError("MessageArea1", model.MessageArea1);

                    // 画面制御用パラメータ取得
                    GetConrtollParam(ref model);

                    // メッセージ反映のため部分ビューを構築する
                    // 返却用パラメータ
                    JsonResult messageArea1 = PartialViewAsJson("_D111010KoufukinKeisan", model, model.MessageArea1);

                    return Json(new { messageArea1 = messageArea1.Value, model.noneKoufukaiFlg, model.batchYoyakuFlg, model.syokaiKoufukaiFlg, model.saishinChoshuGakuNyuryokuzumiFlg });
                }

            }


            try
            {

                NskAppContext dbContext = getJigyoDb<NskAppContext>();

                // ３．トランザクションの開始
                transaction = dbContext.Database.BeginTransaction();

                // ４．画面表示されている交付金計算処理の一覧から最新の交付回と１つ前の交付回を取得する。
                D111010KoufukinKeisanRecord? rec = model.KoufukinKeisan.DispRecords.MaxBy(x => x.Koufukai);

                // ４．１．最新交付回が0だった場合、エラーとする。
                if (rec.Koufukai <= 0)
                {
                    // 最新交付回が0以下の場合、
                    // 存在するはずの交付回がないためエラーとする。
                    errMessage = MessageUtil.Get("ME10004", "交付回");
                    throw new AppException("ME10004", errMessage);
                }

                int saishinKoufukai = rec.Koufukai;
                int zenkaiKoufukai = rec.Koufukai - 1;

                int delCount = 0;
                int updCount = 0;

                // ５．取得した交付回を元にデータを削除する。
                // ５．１． テーブルデータの削除
                // ６．取得した交付回を元にデータを更新する。
                // ６．1. t_00050_交付回のレコード更新
                switch (saishinKoufukai)
                {
                    case 1:

                        // 最新交付回 = 1のパターン

                        // 最新交付回の掛金徴収額入力判定
                        if (rec.ChoshuGakuNyuryokuzumi)
                        {
                            // 入力済みの場合
                            // 交付徴収テーブルの削除　対象：最新交付回
                            delCount = model.KoufukinKeisan.DelKoufuChoshu(dbContext, sessionInfo, saishinKoufukai);

                            // 実行判定
                            if (delCount == 0)
                            {
                                errMessage = MessageUtil.Get("ME10081");
                                break;
                            }

                        }

                        // 交付回テーブルの更新　対象：最新交付回
                        updCount = model.KoufukinKeisan.UpdKoufukaiKoufukinKeisanDelete(dbContext, sessionInfo, saishinKoufukai, Syokuin.UserId);

                        // 実行判定
                        if (updCount == 0)
                        {
                            errMessage = MessageUtil.Get("ME10081");
                            break;
                        }

                        break;

                    case >= 2:

                        // 最新交付回 = 2以上のパターン

                        // 組合等交付テーブルの削除　対象：前回交付回
                        delCount = model.KoufukinKeisan.DelKumiaitoKoufu(dbContext, sessionInfo, zenkaiKoufukai);

                        // 実行判定
                        if (delCount == 0)
                        {
                            errMessage = MessageUtil.Get("ME10081");
                            break;
                        }


                        // 最新交付回の掛金徴収額入力判定
                        if (rec.ChoshuGakuNyuryokuzumi)
                        {
                            // 入力済みの場合
                            // 交付徴収テーブルの削除　対象：最新交付回
                            delCount = model.KoufukinKeisan.DelKoufuChoshu(dbContext, sessionInfo, saishinKoufukai);

                            // 実行判定
                            if (delCount == 0)
                            {
                                errMessage = MessageUtil.Get("ME10081");
                                break;
                            }

                        }

                        // 交付徴収テーブルの削除　対象：前回交付回
                        delCount = model.KoufukinKeisan.DelKoufuChoshu(dbContext, sessionInfo, zenkaiKoufukai);

                        // 実行判定
                        if (delCount == 0)
                        {
                            errMessage = MessageUtil.Get("ME10081");
                            break;
                        }


                        // 交付回テーブルの削除　対象：最新交付回
                        delCount = model.KoufukinKeisan.DelKoufukai(dbContext, sessionInfo, saishinKoufukai);

                        // 実行判定
                        if (delCount == 0)
                        {
                            errMessage = MessageUtil.Get("ME10081");
                            break;
                        }


                        // 交付回テーブルの更新　対象：前回交付回
                        updCount = model.KoufukinKeisan.UpdKoufukaiKoufukinKeisanDelete(dbContext, sessionInfo, zenkaiKoufukai, Syokuin.UserId);

                        // 実行判定
                        if (updCount == 0)
                        {
                            errMessage = MessageUtil.Get("ME10081");
                            break;
                        }

                        break;

                    default:

                        errMessage = MessageUtil.Get("ME10004", "交付回");
                        break;

                }


                // ７．削除結果に応じ、[メッセージエリア１] にメッセージを表示する。
                // エラー判定
                if (errMessage == MessageUtil.Get("ME10081"))
                {
                    // 削除または更新に失敗した場合

                    // 最新交付回の交付計算がされているのに組合等交付のレコードが削除されていない場合、
                    // トランザクション取得前に更新がされたと想定し排他エラーとする。
                    throw new AppException("ME10081", errMessage);

                }
                else
                {
                    // 完了のメッセージを設定
                    errMessage = MessageUtil.Get("MI00004", "解除");
                }

                // ８．トランザクションのコミット
                transaction.Commit();

                // ９．交付金計算処理の一覧を再表示する。
                // 最新データの取得
                // ９．１．No.1の[２．][３．]の処理を実施する
                // 交付徴収額データをクリア
                model.KoufukinKeisan = new();
                model.KoufukinKeisan.GetResult(dbContext, sessionInfo);

            }
            catch (Exception ex)
            {

                // ６．１．途中でエラーが発生時はトランザクションのロールバックを行う。
                transaction?.Rollback();

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

            // 画面制御用パラメータ取得
            GetConrtollParam(ref model);

            // 結果をセッションに保存する
            SessionUtil.Set(F111Const.SESS_D111010, model, HttpContext);

            model.MessageArea1 = errMessage;

            ModelState.AddModelError("MessageArea1", model.MessageArea1);

            // ９．２．部分ビューを構築する
            // メッセージ反映のため部分ビューを構築する
            JsonResult resultArea = PartialViewAsJson("_D111010KoufukinKeisan", model, model.MessageArea1);

            return Json(new { resultArea = resultArea.Value, model.noneKoufukaiFlg, model.batchYoyakuFlg, model.syokaiKoufukaiFlg, model.saishinChoshuGakuNyuryokuzumiFlg });

        }
        #endregion

        #region 戻るボタンイベント
        /// <summary>
        /// 戻る
        /// </summary>
        /// <remarks>遷移元画面に遷移する。</remarks>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Back()
        {
            // １．ポータル画面へ戻る。
            return Json(new { result = "success" });
        }
        #endregion

        #region その他イベント

        /// <summary>
        /// 画面制御用パラメータ取得
        /// </summary>
        /// <param name="model"></param>
        private void GetConrtollParam(ref D111010Model model)
        {

            // 各種フラグ初期化
            model.syokaiKoufukaiFlg = false;
            model.saishinChoshuGakuNyuryokuzumiFlg = false;
            model.batchYoyakuFlg = false;
            model.noneKoufukaiFlg = false;

            // 画面表示の最新交付回取得
            D111010KoufukinKeisanRecord? rec = model.KoufukinKeisan.DispRecords.MaxBy(x => x.Koufukai);

            // 交付回存在チェック
            if (rec is not null)
            {

                // 最新交付回が初回
                if (rec.Koufukai == 1)
                {
                    model.syokaiKoufukaiFlg = true;
                }

                // 最新交付回の徴収額入力済み
                if (rec.ChoshuGakuNyuryokuzumi)
                {
                    model.saishinChoshuGakuNyuryokuzumiFlg = true;
                }

                // バッチ予約状況取得引数の設定
                BatchUtil.GetBatchYoyakuListParam param = new()
                {
                    SystemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN),
                    TodofukenCd = Syokuin.TodofukenCd,
                    KumiaitoCd = Syokuin.KumiaitoCd,
                    ShishoCd = Syokuin.ShishoCd,
                    BatchNm = F111Const.BATCH_ID_NSK_111011B
                };
                // 総件数取得フラグ
                bool boolAllCntFlg = false;
                // 件数（出力パラメータ）
                int intAllCnt = 0;
                // エラーメッセージ（出力パラメータ）
                string message = string.Empty;

                // バッチ予約状況取得（BatchUtil.GetBatchYoyakuList()）を呼び出し、バッチ予約状況を取得する。
                List<BatchYoyaku> batchYoyakus = BatchUtil.GetBatchYoyakuList(param, boolAllCntFlg, ref intAllCnt, ref message);

                // バッチ予約が存在する場合、
                if (intAllCnt >= 1)
                {

                    // バッチ予約が存在する場合
                    BatchYoyaku? batchYoyaku = batchYoyakus.FirstOrDefault(x => x.BatchStatus == BatchUtil.BATCH_STATUS_WAITING);
                    if (batchYoyaku is not null)
                    {
                        // 未実行のバッチ予約が存在する場合
                        model.batchYoyakuFlg = true;
                    }
                }

            }
            else
            {
                // 交付回無しフラグをtrue
                model.noneKoufukaiFlg = true;
            }

        }

        #endregion

    }
}
