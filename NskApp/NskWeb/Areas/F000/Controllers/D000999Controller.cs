using NskWeb.Areas.F000.Models.D000999;
using NskWeb.Common.Consts;
using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Base;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Filters;
using CoreLibrary.Core.Menu;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using ModelLibrary.Models;
using System.Security.Claims;

namespace NskWeb.Areas.F000.Controllers
{
    /// <summary>
    /// 簡易ログイン画面
    /// </summary>
    [ExcludeAuthCheck]
    [AllowAnonymous]
    [ExcludeSystemLockCheck]
    [Area("F000")]
    public class D000999Controller : CoreController
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="viewEngine"></param>
        public D000999Controller(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        // GET: F000/D000999
        public ActionResult Index()
        {
            if (ConfigUtil.Get(CoreConst.D0000_DISPLAY_FLAG) == "true")
            {
                // 画面表示モードを設定
                SetScreenModeFromQueryString();

                // $$$$$$$$$$$$$$ NSKポータル情報の削除
                SessionUtil.Remove(AppConst.SESS_NSK_PORTAL, HttpContext);
                SessionUtil.Remove(CoreConst.SESS_MENU_LIST, HttpContext);

                // 簡易ログイン画面
                return View("D000999_Pre", GetInitModel());
            }
            else
            {
                return RedirectToAction("Init", "D900004", new { area = "F900" });
            }
        }

        public ActionResult Init()
        {
            if (ConfigUtil.Get(CoreConst.D0000_DISPLAY_FLAG) == "true")
            {
                // 画面表示モードを設定
                SetScreenModeFromQueryString();
                if (ScreenMode == CoreConst.ScreenMode.None)
                {
                    throw new AppException("MF00004", MessageUtil.Get("MF00004"));
                }

                if (SessionUtil.Get<IEnumerable<MenuItem>>(CoreConst.SESS_MENU_LIST, HttpContext) == null)
                {
                    SessionUtil.Set(CoreConst.SESS_MENU_LIST, MenuUtil.GetLeftMenuItems(HttpContext), HttpContext);
                }

                // 仮ポータル画面
                return View("D000000");
            }
            else
            {
                return RedirectToAction("Init", "D900004", new { area = "F900" });
            }
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

        /// <summary>
        /// ログイン
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> LoginAsync([Bind("UserId,Password")] D000999Model model)
        {
            if (ConfigUtil.Get(CoreConst.D0000_DISPLAY_FLAG) == "true")
            {
                if (ModelState.IsValid)
                {
                    // システム日時
                    var systemDate = DateUtil.GetSysDateTime();

                    // ユーザ読み込み
                    var mSyokuin = getFimDb().MSyokuins.Where(
                                        s => s.UserId == model.UserId &&
                                        s.UserYukoStartYmd <= systemDate.Date &&
                                        (s.UserYukoEndYmd == null || s.UserYukoEndYmd > systemDate.Date)).SingleOrDefault();
                    if (mSyokuin == null)
                    {
                        // ユーザIDが存在しない
                        ModelState.AddModelError("MessageArea", MessageUtil.Get("ME01033"));
                        D000999Model d000999Model = GetInitModel();
                        d000999Model.UserId = model.UserId;
                        return View("D000999_Pre", d000999Model);
                    }

                    // ユーザ認証
                    bool validUser = AuthUser(model, mSyokuin);

                    // DBへ格納
                    getFimDb().SaveChanges();

                    // 画面遷移
                    if (validUser)
                    {
                        // 認証クッキーの発行

                        // 認証情報作成
                        var claims = new List<Claim>();
                        AddClaim(claims, ClaimTypes.NameIdentifier, model.UserId); // ユーザID

                        // 所属情報
                        AddClaim(claims, "TodofukenCd", mSyokuin.TodofukenCd);     // 都道府県コード
                        AddClaim(claims, "KumiaitoCd", mSyokuin.KumiaitoCd);       // 組合等コード
                        AddClaim(claims, "ShishoCd", mSyokuin.ShishoCd);           // 支所コード

                        // 利用権限
                        AddClaimRole(claims, "bas");                            // ベースアプリ

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            // サーバーアクセスによる認証セッションの更新を許可するかどうか
                            AllowRefresh = true,

                            // 認証チケットの有効期限、認証cookieタイムアウト（セッションタイムアウトと合わせる）
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(ConfigUtil.GetInt(CoreConst.SESSION_STATE_TIMEOUT)),

                            //cookieの有効期間を優先かsessionを優先か
                            //true:cookieが有効であれば、ブラウザを閉じても再ログインが必要ない
                            //false:ブラウザ閉じたら再ログインが必要
                            IsPersistent = false,

                            // 認証チケットが発行された日時
                            IssuedUtc = DateTime.UtcNow
                        };

                        // ユーザーをサインイン
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        // セッションへログインユーザを格納
                        Syokuin syokuin = new Syokuin
                        {
                            UserId = mSyokuin.UserId,
                            TodofukenCd = mSyokuin.TodofukenCd,
                            KumiaitoCd = mSyokuin.KumiaitoCd,
                            ShishoCd = mSyokuin.ShishoCd,
                            UserKanriKengen = mSyokuin.UserKanriKengen,
                            FullNm = mSyokuin.FullNm,
                            FullKana = mSyokuin.FullKana,
                            Tel = mSyokuin.Tel,
                            Mail = mSyokuin.Mail,
                            SystemRiyoKbn = mSyokuin.SystemRiyoKbn,
                            Kengen = "1",
                            KjkKengen = mSyokuin.KjkKengen,
                            FimKengen = mSyokuin.FimKengen,
                            NskKengen = mSyokuin.NskKengen,
                            SmlKengen = mSyokuin.SmlKengen,
                            SynKengen = mSyokuin.SynKengen,
                            KyotsuEngKengen = mSyokuin.KyotsuEngKengen,
                            KyotsuKtkKengen = mSyokuin.KyotsuKtkKengen,
                            KyotsuRenkeiNskKengen = mSyokuin.KyotsuRenkeiNskKengen,
                            KyotsuRenkeiHatKengen = mSyokuin.KyotsuRenkeiHatKengen,
                            KyotsuRenkeiKjuKengen = mSyokuin.KyotsuRenkeiKjuKengen,
                            LoginDate = mSyokuin.LoginDate,

                            //ScreenSosaKengenId = mSyokuin.SmlScreenSosaKengenId,
                            //ShishoGroupId = mSyokuin.SmlShishoGroupId,
                            ScreenSosaKengenId = mSyokuin.NskScreenSosaKengenId,    // $$$$$$ NSKの画面操作権限をセット
                            ShishoGroupId = mSyokuin.NskShishoGroupId,          // $$$$$$ NSKの支所グループIDをセット
                        };
                        SessionUtil.Set(CoreConst.SESS_LOGIN_USER, syokuin, HttpContext);

                        // システム区分
                        string systemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN);

                        // 画面操作権限一覧取得
                        var authMap = FilterUtil.GetScreenSosaKengen(getFimDb(), syokuin, systemKbn);

                        // 利用可能な支所一覧取得
                        var shishoList = FilterUtil.GetRiyoShishoGroup(getFimDb(), syokuin);

                        // システム共通スキーマからログインユーザの所属に応じた都道府県別事業スキーマのDB接続先を取得する
                        DbConnectionInfo dbInfo = DBUtil.GetDbConnectionInfo(systemKbn, syokuin.TodofukenCd,
                                                                                    syokuin.KumiaitoCd, syokuin.ShishoCd);

                        // 画面機能権限
                        SessionUtil.Set(CoreConst.SESS_SCREEN_KINO_KENGEN, authMap, HttpContext);
                        // 利用可能な支所一覧
                        SessionUtil.Set(CoreConst.SESS_SHISHO_GROUP, shishoList, HttpContext);
                        // 都道府県別事業スキーマのDB接続先をセッションに格納する
                        SessionUtil.Set(CoreConst.SESS_DB_CONN, dbInfo, HttpContext);

                        return RedirectToAction("Init", "D000000");
                    }
                }

                SessionUtil.Set(CoreConst.SESS_SCREEN_MODE, CoreConst.ScreenMode.PC, HttpContext);
                return RedirectToAction("Init", "D000999_Pre");
            }
            else
            {
                return RedirectToAction("Init", "D900004", new { area = "F900" });
            }
        }

        private bool AuthUser(D000999Model model, MSyokuin mShokuin)
        {
            bool result = false;
            // ユーザIDが存在する
            // アカウントロックチェック
            if (mShokuin.LockFlg == "1")
            {
                ModelState.AddModelError("MessageArea", MessageUtil.Get("ME01037"));
                return false;
            }

            // 削除済みユーザチェック
            if (mShokuin.DeleteFlg == "1")
            {
                ModelState.AddModelError("MessageArea", MessageUtil.Get("ME01038"));
                return false;
            }

            // パスワードチェック
            if (!PasswordUtil.VerifyHashedPassword(mShokuin, mShokuin.Pwd, model.Password))
            {
                // ログイン失敗
                result = false;
                mShokuin.LoginFailCnt = (short?)((mShokuin.LoginFailCnt ?? 0) + 1);
                if (mShokuin.LoginFailCnt < int.Parse(ConfigUtil.Get(InfraConst.LOGIN_TRAIAL_MAX_CNT)))
                {
                    // ログイン失敗5回未満
                    ModelState.AddModelError("MessageArea", MessageUtil.Get("ME01033"));
                    mShokuin.LockFlg = "0";
                }
                else
                {
                    // ログイン失敗5回以上
                    ModelState.AddModelError("MessageArea", MessageUtil.Get("ME01034"));
                    mShokuin.LockFlg = "1";
                }
            }
            else
            {
                // ログイン成功
                result = true;
                mShokuin.LastLoginDate = mShokuin.LoginDate;
                mShokuin.LoginDate = DateUtil.GetSysDateTime();
                mShokuin.LockFlg = "0";
                mShokuin.LoginFailCnt = 0;
            }
            mShokuin.UpdateUserId = mShokuin.UserId;
            mShokuin.UpdateDate = DateUtil.GetSysDateTime();

            return result;
        }

        /// <summary>
        /// イベント名：デバッグログイン
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> DebugLoginAsync([Bind("UserId,UserKanriNo,TodofukenCd,KumiaitoCd,ShishoCd,UserKanriKengen,Name,NameKana,Tel,Mail,SystemRiyoKbn,ScreenSosaKengenId,ShishoGroupId,UserKanriKengen,ScreenMode")] D000999Model model)
        {
            // 認証クッキーの発行

            // 認証情報作成
            var claims = new List<Claim>();
            AddClaim(claims, ClaimTypes.NameIdentifier, model.UserId); // ユーザID

            // 所属情報
            AddClaim(claims, "TodofukenCd", model.TodofukenCd);     // 都道府県コード
            AddClaim(claims, "KumiaitoCd", model.KumiaitoCd);       // 組合等コード
            AddClaim(claims, "ShishoCd", model.ShishoCd);           // 支所コード

            // 利用権限
            AddClaimRole(claims, "bas");                            // ベースアプリ

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                // サーバーアクセスによる認証セッションの更新を許可するかどうか
                AllowRefresh = true,

                // 認証チケットの有効期限、認証cookieタイムアウト（セッションタイムアウトと合わせる）
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(ConfigUtil.GetInt(CoreConst.SESSION_STATE_TIMEOUT)),

                //cookieの有効期間を優先かsessionを優先か
                //true:cookieが有効であれば、ブラウザを閉じても再ログインが必要ない
                //false:ブラウザ閉じたら再ログインが必要
                IsPersistent = false,

                // 認証チケットが発行された日時
                IssuedUtc = DateTime.UtcNow
            };

            // ユーザーをサインイン
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // セッションへログインユーザを格納
            Syokuin syokuin = new Syokuin();
            syokuin.UserId = model.UserId;

            int userKanriNo;
            if(int.TryParse(model.UserKanriNo, out userKanriNo))
            {
                syokuin.UserKanriNo = userKanriNo;
            }

            syokuin.TodofukenCd = model.TodofukenCd;
            syokuin.KumiaitoCd = model.KumiaitoCd;
            syokuin.ShishoCd = model.ShishoCd;
            syokuin.UserKanriKengen = ((int)model.UserKanriKengen).ToString();
            syokuin.FullNm = model.Name;
            syokuin.FullKana = model.NameKana;
            syokuin.Tel = model.Tel;
            syokuin.Mail = model.Mail;
            syokuin.SystemRiyoKbn = ((int)model.SystemRiyoKbn).ToString();
            syokuin.Kengen = "1";
            syokuin.ScreenSosaKengenId = model.ScreenSosaKengenId;
            syokuin.ShishoGroupId = model.ShishoGroupId;

            SessionUtil.Set(CoreConst.SESS_LOGIN_USER, syokuin, HttpContext);

            SessionUtil.Set(CoreConst.SESS_SCREEN_MODE, Enum.ToObject(typeof(CoreConst.ScreenMode), int.Parse(model.ScreenMode)), HttpContext);

            // システム区分
            string systemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN);

            // 画面操作権限一覧取得
            var authMap = FilterUtil.GetScreenSosaKengen(getFimDb(), syokuin, systemKbn);

            // 利用可能な支所一覧取得
            var shishoList = FilterUtil.GetRiyoShishoGroup(getFimDb(), syokuin);

            // システム共通スキーマからログインユーザの所属に応じた都道府県別事業スキーマのDB接続先を取得する
            DbConnectionInfo dbInfo = DBUtil.GetDbConnectionInfo(systemKbn, syokuin.TodofukenCd,
                                                                        syokuin.KumiaitoCd, syokuin.ShishoCd);

            // 画面機能権限
            SessionUtil.Set(CoreConst.SESS_SCREEN_KINO_KENGEN, authMap, HttpContext);
            // 利用可能な支所一覧
            SessionUtil.Set(CoreConst.SESS_SHISHO_GROUP, shishoList, HttpContext);
            // 都道府県別事業スキーマのDB接続先をセッションに格納する
            SessionUtil.Set(CoreConst.SESS_DB_CONN, dbInfo, HttpContext);

            return RedirectToAction("Init", "D000000");
        }

        /// <summary>
        /// イベント：都道府県
        /// </summary>
        /// <returns>ActionResult</returns>
        /// GET: F00/D0000/Todofuken
        public ActionResult Todofuken([Bind("TodofukenCd,KumiaitoCd,ShishoCd"), FromBody] D000999Model model)
        {
            D000999Model returnModel = new D000999Model();
            string todofukenNm = TodofukenUtil.GetTodofukenNm(model.TodofukenCd);
            if (!string.IsNullOrEmpty(todofukenNm))
            {
                returnModel.TodofukenNm = todofukenNm;
                if (!string.IsNullOrEmpty(model.KumiaitoCd))
                {
                    string kumiaitoNm = KumiaitoUtil.GetKumiaitoNm(model.TodofukenCd, model.KumiaitoCd);
                    if (!string.IsNullOrEmpty(kumiaitoNm))
                    {
                        returnModel.KumiaitoNm = kumiaitoNm;
                        if (!string.IsNullOrEmpty(model.ShishoCd))
                        {
                            string shishoNm = ShishoUtil.GetShishoNm(model.TodofukenCd, model.KumiaitoCd, model.ShishoCd);
                            if (!string.IsNullOrEmpty(shishoNm))
                            {
                                returnModel.ShishoNm = shishoNm;
                            }
                            else
                            {
                                returnModel.ShishoNm = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        returnModel.KumiaitoNm = string.Empty;
                    }
                }
            }
            else
            {
                returnModel.TodofukenNm = string.Empty;
            }

            return Json(returnModel);
        }

        /// <summary>
        /// イベント：組合等
        /// </summary>
        /// <returns>ActionResult</returns>
        /// GET: F000/D000999/Kumiaito
        public ActionResult Kumiaito([Bind("TodofukenCd,KumiaitoCd,ShishoCd"), FromBody] D000999Model model)
        {
            D000999Model returnModel = new D000999Model();
            string kumiaitoNm = KumiaitoUtil.GetKumiaitoNm(model.TodofukenCd, model.KumiaitoCd);
            if (!string.IsNullOrEmpty(kumiaitoNm))
            {
                returnModel.KumiaitoNm = kumiaitoNm;
                if (!string.IsNullOrEmpty(model.ShishoCd))
                {
                    string shishoNm = ShishoUtil.GetShishoNm(model.TodofukenCd, model.KumiaitoCd, model.ShishoCd);
                    if (!string.IsNullOrEmpty(shishoNm))
                    {
                        returnModel.ShishoNm = shishoNm;
                    }
                    else
                    {
                        returnModel.ShishoNm = string.Empty;
                    }
                }
            }
            else
            {
                returnModel.KumiaitoNm = string.Empty;
            }

            return Json(returnModel);
        }

        /// <summary>
        /// イベント：支所
        /// </summary>
        /// <returns>ActionResult</returns>
        /// GET: F00/D0000/Shisho
        public ActionResult Shisho([Bind("TodofukenCd,KumiaitoCd,ShishoCd"), FromBody] D000999Model model)
        {
            D000999Model returnModel = new D000999Model();
            string shishoNm = ShishoUtil.GetShishoNm(model.TodofukenCd, model.KumiaitoCd, model.ShishoCd);
            if (!string.IsNullOrEmpty(shishoNm))
            {
                returnModel.ShishoNm = shishoNm;
            }
            else
            {
                returnModel.ShishoNm = string.Empty;
            }

            return Json(returnModel);
        }

        /// <summary>
        /// 引数で指定されたClaimリストに認証情報を追加する
        /// 値がnullの場合は、追加しない
        /// </summary>
        /// <param name="claims">Claimリスト</param>
        /// <param name="type">クレームタイプ</param>
        /// <param name="value">値</param>
        private void AddClaim(List<Claim> claims, string type, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                claims.Add(new Claim(type, value));
            }
        }

        /// <summary>
        /// 引数で指定されたClaimリストに認証情報（Role）を追加する
        /// </summary>
        /// <param name="claims">Claimリスト</param>
        /// <param name="role">ロール</param>

        private void AddClaimRole(List<Claim> claims, string role)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        /// <summary>
        /// 引数で指定されたClaimリストに認証情報（Role）を追加する
        /// 利用権限がありの場合、指定されたロールを追加する
        /// </summary>
        /// <param name="claims">Claimリスト</param>
        /// <param name="kengen">利用権限有無</param>
        /// <param name="role">ロール</param>
        private void AddClaimRole(List<Claim> claims, string kengen, string role)
        {
            if (((int)CoreConst.RiyoKengen.True).ToString().Equals(kengen))
            {
                AddClaimRole(claims, role);
            }
        }
    }
}