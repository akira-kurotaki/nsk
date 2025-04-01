using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore.Storage;
using NskAppModelLibrary.Context;
using NskWeb.Areas.F109.Consts;
using NskWeb.Areas.F109.Models.D109020;
using System.Data;

namespace NskWeb.Areas.F109.Controllers
{
    /// <summary>
    /// 規模別分布状況データ作成設定コントローラ
    /// </summary>
    [AllowAnonymous]
    [ExcludeAuthCheck]
    [Area("F109")]
    public class D109020Controller : CoreController
    {
        /// <summary>
        /// セッションキー(D109020)
        /// </summary>
        private const string SESS_D109020 = $"{F109Const.SCREEN_ID_D109020}_SCREEN";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public D109020Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        // GET: F109/D109020
        public ActionResult Index()
        {
            if (ConfigUtil.Get(CoreConst.D0000_DISPLAY_FLAG) == "true")
            {
                // 画面表示モードを設定
                SetScreenModeFromQueryString();

            }

            return RedirectToAction("Init", F109Const.SCREEN_ID_D109020, new { area = "F109" });
        }

        /// <summary>
        /// 初期表示
        /// </summary>
        /// <returns>規模別分布状況データ作成設定画面表示結果</returns>
        public ActionResult Init()
        {
            // セッション情報をクリアする。
            SessionUtil.Remove(SESS_D109020, HttpContext);

            // １．権限チェック
            // １．１．ログインユーザの画面単位・機能単位の権限を取得する。
            // ログインユーザの権限が「参照」「更新権限」いずれも許可されていない場合、メッセージを設定し業務エラー画面を表示する。
            bool dispKengen = ScreenSosaUtil.CanReference(F109Const.SCREEN_ID_D109020, HttpContext);
            bool updKengen = ScreenSosaUtil.CanUpdate(F109Const.SCREEN_ID_D109020, HttpContext);

            F109Const.DispAuthority dispAuthority = F109Const.DispAuthority.None;
            if (updKengen)
            {
                dispAuthority = F109Const.DispAuthority.Update;// "更新権限";
            }
            else if (dispKengen)
            {
                dispAuthority = F109Const.DispAuthority.ReadOnly;// "参照権限";
            }
            else
            {
                throw new AppException("ME10075", MessageUtil.Get("ME10075"));
            }

            // ２．画面表示用データを取得する。
            // ２．１．セッションから「都道府県コード」「組合等コード」「支所コード」「利用可能支所コード」「ユーザーID」
            // 「画面操作権限区分コード」「共済目的コード」「引受年産」を取得する。
            D109020SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            D109020Model model = new();
            model.DispKengen = dispAuthority;


            NskAppContext dbContext = getJigyoDb<NskAppContext>();

            // ２．２．共済目的名称を取得する。
            model.KyosaiMokutekiCd = sessionInfo.KyosaiMokutekiCd;
            model.KyosaiMokuteki = dbContext.M00010共済目的名称s.SingleOrDefault(x =>
                (x.共済目的コード == model.KyosaiMokutekiCd))?.共済目的名称 ?? string.Empty;


            // ２．３．組合等設定を取得する。
            // ※ 引受計算支所実行単位区分はセッション情報で取得するので省略

            // ２．４．～２．６．
            // [支所]ドロップダウンリスト項目を取得する。
            model.InitializeDropdonwList(dbContext, sessionInfo);


            // ３．データの取得後の画面設定
            // ３．１．年産に[セッション：引受年産]を表示する。
            model.Nensan = $"{sessionInfo.Nensan}";

            // ３．２．共済目的に「２．２．」で取得した共済目的名称を表示する
            // ※ ２．２．で設定済み

            // ３．３．[組合等設定.引受計算支所実行単位区分]＝「2：本所支所」の場合、[本所・支所]ドロップダウンリストの空白行を選択する。
            if (sessionInfo.HikiukeJikkoTanniKbnHikiuke == "2")
            {
                model.SearchCondition.ShishoList.Insert(0, new());
            }

            // ３．４．本所・支所ドロップダウンリストで選択している支所コードを[変数：選択支所コード]に保持
            model.SearchCondition.ShishoCd = sessionInfo.ShishoCd;

            // ３．５．規模別分布状況データ設定エリアの設定
            // ３．５．１．見出し「面積区分」「面積上限」のみを表示する。
            // ３．５．２．内容は非表示とする。
            model.KibobetsuBunpu = new();

            // ３．６．「１．１．」で取得した画面権限と「２．３．」で取得した[組合等設定.引受計算支所実行単位区分]で活性・非活性を設定する。
            model.DispControl(sessionInfo);

            // 結果をセッションに保存する
            SessionUtil.Set(SESS_D109020, model, HttpContext);

            ModelState.Clear();

            // 規模別分布状況データ作成設定画面を表示する
            return View(F109Const.SCREEN_ID_D109020, model);
        }

        /// <summary>
        /// 検索処理
        /// 検索条件で情報を取得し、表示する。													
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        public ActionResult Search(D109020Model dispModel)
        {
            // ２．規模別分布状況設定検索処理
            ModelState.Clear();

            // セッションから規模別分布状況データ作成設定モデルを取得する
            D109020Model model = SessionUtil.Get<D109020Model>(SESS_D109020, HttpContext);

            model.SearchCondition.ApplyInput(dispModel.SearchCondition);

            // ２．１．検索結果クリア
            model.KibobetsuBunpu.DispRecords?.Clear();


            // ２．２．検索処理実行
            // ２．２．１．規模別面積区分情報を取得する。
            D109020SessionInfo sessionInfo = new();
            sessionInfo.GetInfo(HttpContext);

            NskAppContext dbContext = getJigyoDb<NskAppContext>();
            model.KibobetsuBunpu.Search(dbContext, sessionInfo, model.SearchCondition);

            // ２．３．検索結果のチェック
            if (model.KibobetsuBunpu.DispRecords.Count == 0)
            {
                // 検索結果 0件
                // ２．４．規模別分布状況データ設定一覧設定
                // ２．４．１．[面積区分]の設定
                // ２．４．２．[面積上限]の設定
                for (int i = 0; i < 21; i++)
                {
                    model.KibobetsuBunpu.DispRecords.Add(
                        new()
                        {
                            MensekiKbn = $"{(i + 1)}", // [面積区分]に1から21まで連番を設定する。
                            MensekiJogen = null,       // [面積上限]に全て空欄を設定する。
                            Xmin = null
                        });
                }
            }
            else
            {
                // 検索結果 1件以上
                // ２．５．規模別分布状況データ設定一覧設定
                // ２．５．１．[面積区分]の設定
                // ２．５．２．[面積上限]の設定
                // t_14070_規模別面積区分情報の[規模別面積区分]と一致する画面の[面積区分]の行の[面積上限]に[対象面積上限]を設定する。
                // ※ model.KibobetsuBunpu.Search()でセット済み
            }

            // ２．４．１．検索結果をセッションに保存する。
            SessionUtil.Set(SESS_D109020, model, HttpContext);

            // ２．４．３．規模別分布状況データ設定一覧を画面に表示する。
            // ２．５．３．規模別分布状況データ設定一覧を画面に表示する。
            return PartialViewAsJson("_D109020KibobetsuBunpuResult", model);
        }

        #region 戻るイベント
        /// <summary>
        /// 戻る
        /// 農作物ポータルへ遷移する。
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Back()
        {
            // ２．１．セッション情報から検索条件、検索結果件数をクリアする。
            SessionUtil.Remove(SESS_D109020, HttpContext);

            // ２．２．１．ポータルへ遷移する。
            return Json(new { result = "success" });
        }
        #endregion

        /// <summary>
        /// 登録処理
        /// エラーチェックを行い、選択した本所・支所に該当する規模別分布状況データ設定をＤＢに登録する。													
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        public ActionResult Insert(D109020Model dispModel)
        {
            // ３．１．登録処理を実行する。
            IDbContextTransaction? transaction = null;
            string errMessage = string.Empty;
            try
            {
                // セッションから規模別分布状況データ作成設定モデルを取得する
                D109020Model model = SessionUtil.Get<D109020Model>(SESS_D109020, HttpContext);
                model.KibobetsuBunpu = dispModel.KibobetsuBunpu;

                D109020SessionInfo sessionInfo = new();
                sessionInfo.GetInfo(HttpContext);

                // ３．１．１．トランザクションの開始
                NskAppContext dbContext = getJigyoDb<NskAppContext>();
                transaction = dbContext.Database.BeginTransaction();

                // ３．１．２．データ削除処理（ログ出力：あり）
                model.KibobetsuBunpu.Delete(ref dbContext, sessionInfo, model.SearchCondition.ShishoCd, ref errMessage);

                // ３．１．３．データ登録処理（ログ出力：あり）
                model.KibobetsuBunpu.Insert(ref dbContext, sessionInfo, model.SearchCondition.ShishoCd, DateUtil.GetSysDateTime(), ref errMessage);

                // ３．１．４．トランザクションのコミット
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction?.RollbackAsync();

                if (string.IsNullOrEmpty(errMessage))
                {
                    if (ex is DBConcurrencyException)
                    {
                        // 排他エラーが含まれる場合は、以下のメッセージを表示する。
                        // メッセージセットは以下の処理内で行う
                        // model.KibobetsuBunpu.Delete()
                        // model.KibobetsuBunpu.Insert()
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

    }
}
