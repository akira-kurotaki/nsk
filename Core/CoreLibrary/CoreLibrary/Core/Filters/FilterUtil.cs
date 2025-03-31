using CoreLibrary.Core.Attributes;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Exceptions;
using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Context;
using ModelLibrary.Models;
using NLog;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static CoreLibrary.Core.Consts.CoreConst;

namespace CoreLibrary.Core.Filters
{
    /// <summary>
    /// フィルターユーティリティ
    /// </summary>
    /// <remarks>
    /// 作成日：2018/07/20
    /// 作成者：MATSUBAYSHI Atsushi
    /// </remarks>
    public class FilterUtil
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ログインユーザをセッションに設定
        /// システム共通（ログイン画面、システム選択画面、農業者情報管理）向け
        /// </summary>
        /// <param name="filterContext">ActionExecutingContext</param>
        public static void SetLoginUserToSessionForCore(ActionExecutingContext filterContext)
        {
            string identityName = string.Empty;

            if (ConfigUtil.Get(CoreConst.APP_ENV) == "debug")
            {
                identityName = ConfigUtil.Get("Debug_UserId");
            }
            else
            {
                identityName = filterContext.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            logger.Debug("identityName:" + identityName);
            if (string.IsNullOrEmpty(identityName))
            {
                return;
            }

            SetLoginUserToSessionForCore(identityName, filterContext.HttpContext);
        }

        /// <summary>
        /// ログインユーザをセッションに設定
        /// システム共通（ログイン画面、システム選択画面、農業者情報管理）向け
        /// </summary>
        /// <param name="identityName">ユーザID</param>
        /// <param name="context">HttpContext</param>
        public static void SetLoginUserToSessionForCore(string identityName, HttpContext context)
        {
            // システム区分
            string systemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN);

            // 農業者情報管理スキーマから職員情報を取得する
            using (var db = new FimContext())
            {
                // システム日時
                var systemDate = DateUtil.GetSysDateTime();

                var syokuinEntity = db.MSyokuins.AsNoTracking().Where(
                                    s => s.UserId == identityName &&
                                    s.UserYukoStartYmd <= systemDate.Date &&
                                    (s.UserYukoEndYmd == null || s.UserYukoEndYmd >= systemDate.Date)).SingleOrDefault();

                var syokuin = new Syokuin
                {
                    UserId = syokuinEntity.UserId,
                    UserKanriNo = syokuinEntity.UserKanriNo,
                    TodofukenCd = syokuinEntity.TodofukenCd,
                    KumiaitoCd = syokuinEntity.KumiaitoCd,
                    ShishoCd = syokuinEntity.ShishoCd,
                    UserKanriKengen = syokuinEntity.UserKanriKengen,
                    BatchKanriKengen = syokuinEntity.BatchKanriKengen,
                    FullNm = syokuinEntity.FullNm,
                    FullKana = syokuinEntity.FullKana,
                    Tel = syokuinEntity.Tel,
                    Mail = syokuinEntity.Mail,
                    SystemRiyoKbn = syokuinEntity.SystemRiyoKbn,

                    KjkKengen = syokuinEntity.KjkKengen,
                    FimKengen = syokuinEntity.FimKengen,
                    NskKengen = syokuinEntity.NskKengen,
                    SmlKengen = syokuinEntity.SmlKengen,
                    SynKengen = syokuinEntity.SynKengen,
                    KyotsuEngKengen = syokuinEntity.KyotsuEngKengen,
                    KyotsuKtkKengen = syokuinEntity.KyotsuKtkKengen,
                    KyotsuRenkeiNskKengen = syokuinEntity.KyotsuRenkeiNskKengen,
                    KyotsuRenkeiHatKengen = syokuinEntity.KyotsuRenkeiHatKengen,
                    KyotsuRenkeiKjuKengen = syokuinEntity.KyotsuRenkeiKjuKengen,

                    Kengen = GetRiyoKengenSwitch(systemKbn, syokuinEntity),
                    ScreenSosaKengenId = GetScreenSosaKengenIdSwith(systemKbn, syokuinEntity),
                    ShishoGroupId = GetShishoGroupCdSwitch(systemKbn, syokuinEntity),

                    LoginDate = syokuinEntity.LoginDate,
                };

                // 画面操作権限一覧取得
                var authMap = GetScreenSosaKengen(db, syokuin, systemKbn);

                // 利用可能な支所一覧取得
                var shishoList = GetRiyoShishoGroup(db, syokuin);

                SessionUtil.Set(CoreConst.SESS_LOGIN_USER, syokuin, context);
                logger.Info(SystemMessageUtil.Get("MI00017", syokuin.UserId));

                // 画面機能権限
                SessionUtil.Set(CoreConst.SESS_SCREEN_KINO_KENGEN, authMap, context);
                // 利用可能な支所一覧
                SessionUtil.Set(CoreConst.SESS_SHISHO_GROUP, shishoList, context);
            }
        }

        /// <summary>
        /// ログインユーザをセッションに設定
        /// 事業システム向け
        /// </summary>
        /// <param name="filterContext"></param>
        public static void SetLoginUserToSession(ActionExecutingContext filterContext)
        {
            string identityName = string.Empty;

            // 都道府県コード
            string todofukenCd;
            // 組合等コード
            string kumiaitoCd;
            // 支所コード
            string shishoCd;
            // システム区分
            string systemKbn = ConfigUtil.Get(CoreConst.APP_ENV_SYSTEM_KBN);

            // デバッグ用
            int? debugScreenSosaKengenId = null;
            int? debugShishoGroupId = null;

            if (ConfigUtil.Get(CoreConst.APP_ENV) == "debug")
            {
                identityName = ConfigUtil.Get("Debug_UserId");
                todofukenCd = ConfigUtil.Get("Debug_TodofukenCd");
                kumiaitoCd = ConfigUtil.Get("Debug_KumiaitoCd");
                shishoCd = ConfigUtil.Get("Debug_ShishoCd");
                if (!string.IsNullOrEmpty(ConfigUtil.Get("Debug_ScreenSosaKengenId")))
                {
                    debugScreenSosaKengenId = ConfigUtil.GetInt("Debug_ScreenSosaKengenId");
                }
                if (!string.IsNullOrEmpty(ConfigUtil.Get("Debug_ShishoGroupId")))
                {
                    debugShishoGroupId = ConfigUtil.GetInt("Debug_ShishoGroupId");
                }
                logger.Debug("DebugScreenSosaKengenId : " + debugScreenSosaKengenId);
                logger.Debug("DebugShishoGroupId : " + debugShishoGroupId);
            }
            else
            {
                identityName = filterContext.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                todofukenCd = filterContext.HttpContext.User?.FindFirst("TodofukenCd")?.Value;
                kumiaitoCd = filterContext.HttpContext.User?.FindFirst("KumiaitoCd")?.Value;
                shishoCd = filterContext.HttpContext.User?.FindFirst("ShishoCd")?.Value;
            }

            logger.Debug("identityName : " + identityName);
            logger.Debug("HttpContext.User[TodofukenCd] : " + todofukenCd);
            logger.Debug("HttpContext.User[KumiaitoCd] : " + kumiaitoCd);
            logger.Debug("HttpContext.User[ShishoCd] :" + shishoCd);
            logger.Debug("SystemKbn : " + systemKbn);

            if (string.IsNullOrEmpty(identityName))
            {
                return;
            }

            // システム共通スキーマからログインユーザの所属に応じた都道府県別事業スキーマのDB接続先を取得する
            DbConnectionInfo dbInfo = DBUtil.GetDbConnectionInfo(systemKbn, todofukenCd, kumiaitoCd, shishoCd);
            if (dbInfo == null)
            {
                // 取得できない場合のエラー処理
                throw new AppException("MF00001", SystemMessageUtil.Get("MF00001"), CoreConst.HEADER_PATTERN_ID_2);
            }

            using (var jigyoDb = new JigyoContext(dbInfo.ConnectionString, dbInfo.DefaultSchema))
            {
                // システム日時
                var systemDate = DateUtil.GetSysDateTime();

                // 都道府県別事業スキーマから職員情報を取得する
                var syokuinEntity = jigyoDb.VSyokuins.AsNoTracking().Where(
                                    s => s.UserId == identityName &&
                                    s.UserYukoStartYmd <= systemDate.Date &&
                                    (s.UserYukoEndYmd == null || s.UserYukoEndYmd >= systemDate.Date)).SingleOrDefault();

                // 取得できない場合のエラー処理
                if (syokuinEntity == null)
                {
                    throw new AppException("MF00001", SystemMessageUtil.Get("MF00001"), CoreConst.HEADER_PATTERN_ID_2);
                }

                var syokuin = new Syokuin
                {
                    UserId = syokuinEntity.UserId,
                    UserKanriNo = syokuinEntity.UserKanriNo,
                    TodofukenCd = syokuinEntity.TodofukenCd,
                    KumiaitoCd = syokuinEntity.KumiaitoCd,
                    ShishoCd = syokuinEntity.ShishoCd,
                    UserKanriKengen = syokuinEntity.UserKanriKengen,
                    FullNm = syokuinEntity.FullNm,
                    FullKana = syokuinEntity.FullKana,
                    Tel = syokuinEntity.Tel,
                    Mail = syokuinEntity.Mail,
                    SystemRiyoKbn = syokuinEntity.SystemRiyoKbn,
                    LoginDate = syokuinEntity.LoginDate,

                    Kengen = GetRiyoKengenSwitch(systemKbn, syokuinEntity),
                    ScreenSosaKengenId = GetScreenSosaKengenIdSwith(systemKbn, syokuinEntity),
                    ShishoGroupId = GetShishoGroupCdSwitch(systemKbn, syokuinEntity),
                };

                // デバッグ用
                if (ConfigUtil.Get(CoreConst.APP_ENV) == "debug")
                {
                    syokuin.ScreenSosaKengenId = debugScreenSosaKengenId;
                    syokuin.ShishoGroupId = debugShishoGroupId;
                }

                // 画面操作権限一覧取得
                var authMap = GetScreenSosaKengen(jigyoDb, syokuin, systemKbn);

                // 利用可能な支所一覧取得
                var shishoList = GetRiyoShishoGroup(jigyoDb, syokuin);

                SessionUtil.Set(CoreConst.SESS_LOGIN_USER, syokuin, filterContext.HttpContext);
                logger.Info(SystemMessageUtil.Get("MI00017", syokuin.UserId));

                // 画面機能権限
                SessionUtil.Set(CoreConst.SESS_SCREEN_KINO_KENGEN, authMap, filterContext.HttpContext);
                // 利用可能な支所一覧
                SessionUtil.Set(CoreConst.SESS_SHISHO_GROUP, shishoList, filterContext.HttpContext);
                // 都道府県別事業スキーマのDB接続先をセッションに格納する
                SessionUtil.Set(CoreConst.SESS_DB_CONN, dbInfo, filterContext.HttpContext);
            }
        }

        /// <summary>
        /// 画面操作権限一覧取得
        /// システム共通（ログイン画面、システム選択画面、農業者情報管理）向け
        /// ログインユーザの現在の事業システムの画面操作権限一覧を取得する。
        /// </summary>
        /// <param name="db">事業システムDBコンテキスト</param>
        /// <param name="syokuin">ログインユーザ</param>
        /// <param name="systemKbn">システム区分</param>
        /// <returns>画面操作権限一覧</returns>
        public static Dictionary<string, ScreenSosaKengen> GetScreenSosaKengen(FimContext db, Syokuin syokuin, string systemKbn)
        {
            var authMap = new Dictionary<string, ScreenSosaKengen>();

            if (syokuin.ScreenSosaKengenId == null)
            {
                return authMap;
            }

            var list = (
                    from o in db.MScreenKinoKengens
                    join k in db.MScreenSosaKengens
                    on o.ScreenSosaKengenId equals k.ScreenSosaKengenId 
                    where
                    k.TodofukenCd == syokuin.TodofukenCd &&
                    k.KumiaitoCd == syokuin.KumiaitoCd &&
                    k.SystemKbn == systemKbn &&
                    k.ScreenSosaKengenId == syokuin.ScreenSosaKengenId
                    select new ScreenSosaKengen()
                    {
                        // 画面機能コード
                        ScreenKinoCd = o.ScreenKinoCd,
                        // 参照不可フラグ
                        NoReferFlg = o.NoReferFlg,
                        // 更新不可フラグ
                        NoUpdateFlg = o.NoUpdateFlg,
                    }
                ).ToList();

            list.ForEach(x =>
            {
                authMap.Add(x.ScreenKinoCd, x);
            });

            return authMap;
        }

        /// <summary>
        /// 画面操作権限一覧取得
        /// 事業システム向け
        /// ログインユーザの現在の事業システムの画面操作権限一覧を取得する。
        /// </summary>
        /// <param name="db">事業システムDBコンテキスト</param>
        /// <param name="syokuin">ログインユーザ</param>
        /// <param name="systemKbn">システム区分</param>
        /// <returns>画面操作権限一覧</returns>
        public static Dictionary<string, ScreenSosaKengen> GetScreenSosaKengen(JigyoContext db, Syokuin syokuin, string systemKbn)
        {
            var authMap = new Dictionary<string, ScreenSosaKengen>();

            if (syokuin.ScreenSosaKengenId == null)
            {
                return authMap;
            }

            var list = (
                    from o in db.VScreenKinoKengens
                    join k in db.VScreenSosaKengens
                    on o.ScreenSosaKengenId equals k.ScreenSosaKengenId
                    where
                    k.TodofukenCd == syokuin.TodofukenCd &&
                    k.KumiaitoCd == syokuin.KumiaitoCd &&
                    k.SystemKbn == systemKbn &&
                    k.ScreenSosaKengenId == syokuin.ScreenSosaKengenId
                    select new ScreenSosaKengen()
                    {
                        // 画面機能コード
                        ScreenKinoCd = o.ScreenKinoCd,
                        // 参照不可フラグ
                        NoReferFlg = o.NoReferFlg,
                        // 更新不可フラグ
                        NoUpdateFlg = o.NoUpdateFlg,
                    }
                ).ToList();

            list.ForEach(x =>
            {
                authMap.Add(x.ScreenKinoCd, x);
            });

            return authMap;
        }

        /// <summary>
        /// 利用可能な支所一覧取得
        /// システム共通（ログイン画面、システム選択画面、農業者情報管理）向け
        /// </summary>
        /// <param name="db"></param>
        /// <param name="syokuin"></param>
        /// <returns></returns>
        public static List<Shisho> GetRiyoShishoGroup(FimContext db, Syokuin syokuin)
        {
            var shishoList = new List<Shisho>();

            if (syokuin.ShishoGroupId == null)
            {
                // 支所グループコードが設定されていない場合

                if (string.IsNullOrEmpty(syokuin.ShishoCd))
                {
                    // システム利用者の支所が設定されていない場合、組合の全支所を取得する。
                    shishoList = db.MShishoNms
                                    .Where(a => a.TodofukenCd == syokuin.TodofukenCd &&
                                                a.KumiaitoCd == syokuin.KumiaitoCd)
                                    .OrderBy(a => a.ShishoCd)
                                    .AsEnumerable().Select(m => new Shisho
                                    {
                                        TodofukenCd = m.TodofukenCd,
                                        KumiaitoCd = m.KumiaitoCd,
                                        ShishoCd = m.ShishoCd,
                                        ShishoNm = m.ShishoNm
                                    })
                                    .ToList();
                }
                else
                {
                    // システム利用者の支所が設定されている場合、システム利用者に設定されている支所を取得する。
                    shishoList = db.MShishoNms
                                    .Where(a => a.TodofukenCd == syokuin.TodofukenCd &&
                                                a.KumiaitoCd == syokuin.KumiaitoCd &&
                                                a.ShishoCd == syokuin.ShishoCd)
                                    .OrderBy(a => a.ShishoCd)
                                    .AsEnumerable().Select(m => new Shisho
                                    {
                                        TodofukenCd = m.TodofukenCd,
                                        KumiaitoCd = m.KumiaitoCd,
                                        ShishoCd = m.ShishoCd,
                                        ShishoNm = m.ShishoNm
                                    })
                                    .ToList();
                }
            }
            else
            {
                // 支所グループコードが設定されている場合
                // 利用支所グループの支所一覧を取得する。
                shishoList = (
                                from o in db.MShishoGroups
                                join k in db.MShishoGroupShosais
                                on o.ShishoGroupId equals k.ShishoGroupId
                                join m in db.MShishoNms
                                on
                                new { o.TodofukenCd, o.KumiaitoCd, k.ShishoCd }
                                equals
                                new { m.TodofukenCd, m.KumiaitoCd, m.ShishoCd }
                                where
                                o.TodofukenCd == syokuin.TodofukenCd &&
                                o.KumiaitoCd == syokuin.KumiaitoCd &&
                                o.ShishoGroupId == syokuin.ShishoGroupId
                                select new Shisho()
                                {
                                    TodofukenCd = o.TodofukenCd,
                                    KumiaitoCd = o.KumiaitoCd,
                                    ShishoCd = k.ShishoCd,
                                    ShishoNm = m.ShishoNm
                                }
                            ).OrderBy(a => a.ShishoCd)
                            .ToList();
            }

            return shishoList;
        }

        /// <summary>
        /// 利用可能な支所一覧取得
        /// </summary>
        /// <param name="db"></param>
        /// <param name="syokuin"></param>
        /// <returns></returns>
        public static List<Shisho> GetRiyoShishoGroup(JigyoContext db, Syokuin syokuin)
        {
            var shishoList = new List<Shisho>();

            if (syokuin.ShishoGroupId == null)
            {
                // 支所グループコードが設定されていない場合

                if (string.IsNullOrEmpty(syokuin.ShishoCd))
                {
                    // システム利用者の支所が設定されていない場合、組合の全支所を取得する。
                    shishoList = db.VShishoNms
                                    .Where(a => a.TodofukenCd == syokuin.TodofukenCd &&
                                                a.KumiaitoCd == syokuin.KumiaitoCd)
                                    .OrderBy(a => a.ShishoCd)
                                    .AsEnumerable().Select(m => new Shisho
                                    {
                                        TodofukenCd = m.TodofukenCd,
                                        KumiaitoCd = m.KumiaitoCd,
                                        ShishoCd = m.ShishoCd,
                                        ShishoNm = m.ShishoNm
                                    })
                                    .ToList();
                }
                else
                {
                    // システム利用者の支所が設定されている場合、システム利用者に設定されている支所を取得する。
                    shishoList = db.VShishoNms
                                    .Where(a => a.TodofukenCd == syokuin.TodofukenCd &&
                                                a.KumiaitoCd == syokuin.KumiaitoCd &&
                                                a.ShishoCd == syokuin.ShishoCd)
                                    .OrderBy(a => a.ShishoCd)
                                    .AsEnumerable().Select(m => new Shisho
                                    {
                                        TodofukenCd = m.TodofukenCd,
                                        KumiaitoCd = m.KumiaitoCd,
                                        ShishoCd = m.ShishoCd,
                                        ShishoNm = m.ShishoNm
                                    })
                                    .ToList();
                }
            }
            else
            {
                // 支所グループコードが設定されている場合
                // 利用支所グループの支所一覧を取得する。
                shishoList = (
                                from o in db.VShishoGroups
                                join k in db.VShishoGroupShosais
                                on o.ShishoGroupId equals k.ShishoGroupId 
                                join m in db.VShishoNms
                                on
                                new { o.TodofukenCd, o.KumiaitoCd, k.ShishoCd }
                                equals
                                new { m.TodofukenCd, m.KumiaitoCd, m.ShishoCd }
                                where
                                o.TodofukenCd == syokuin.TodofukenCd &&
                                o.KumiaitoCd == syokuin.KumiaitoCd &&
                                o.ShishoGroupId == syokuin.ShishoGroupId
                                select new Shisho()
                                {
                                    TodofukenCd = o.TodofukenCd,
                                    KumiaitoCd = o.KumiaitoCd,
                                    ShishoCd = k.ShishoCd,
                                    ShishoNm = m.ShishoNm
                                }
                            ).OrderBy(a => a.ShishoCd)
                            .ToList();
            }

            return shishoList;
        }

        /// <summary>
        /// 画面操作権限ID Swith式
        /// </summary>
        /// <param name="systemKbn">システム区分</param>
        /// <param name="syokuin">職員情報</param>
        /// <returns>指定されたシステム区分の画面操作権限ID</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static int? GetScreenSosaKengenIdSwith(string systemKbn, ISyokuin syokuin) => systemKbn switch
        {
            // 画面操作権限ID_加入状況管理
            (SystemKbn.Kjk) => syokuin.KjkScreenSosaKengenId,

            // 画面操作権限ID_農作物共済
            (SystemKbn.Nsk) => syokuin.NskScreenSosaKengenId,

            // 画面操作権限ID_建物共済
            (SystemKbn.Sml) => syokuin.SmlScreenSosaKengenId,

            // 画面操作権限ID_農業者情報管理
            (SystemKbn.Fim) => syokuin.FimScreenSosaKengenId,

            // 画面操作権限ID_ベースアプリケーション
            (SystemKbn.BAS) => syokuin.SmlScreenSosaKengenId,

            // 上記以外
            _ =>throw new AppException("MF00001", SystemMessageUtil.Get("MF00001"), CoreConst.HEADER_PATTERN_ID_2)
        };

        /// <summary>
        /// 支所グループID Swith式
        /// </summary>
        /// <param name="systemKbn">システム区分</param>
        /// <param name="syokuin">職員情報</param>
        /// <returns>指定されたシステム区分の支所グループID</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static int? GetShishoGroupCdSwitch(string systemKbn, ISyokuin syokuin) => systemKbn switch
        {
            // 支所グループID_加入状況管理
            (SystemKbn.Kjk) => syokuin.KjkShishoGroupId,

            // 支所グループID_農作物共済
            (SystemKbn.Nsk) => syokuin.NskShishoGroupId,

            // 支所グループID_建物共済
            (SystemKbn.Sml) => syokuin.SmlShishoGroupId,

            // 支所グループID_農業者情報管理
            (SystemKbn.Fim) => syokuin.FimShishoGroupId,

            // 支所グループID_収入保険
            (SystemKbn.Syn) => syokuin.SynShishoGroupId,

            // 支所グループID_ベースアプリケーション
            (SystemKbn.BAS) => syokuin.SmlShishoGroupId,

            // 上記以外
            _ => throw new AppException("MF00001", SystemMessageUtil.Get("MF00001"), CoreConst.HEADER_PATTERN_ID_2)
        };

        /// <summary>
        /// 利用権限 Swith式
        /// </summary>
        /// <param name="systemKbn">システム区分</param>
        /// <param name="syokuin">職員情報</param>
        /// <returns>指定されたシステム区分の利用権限</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static string GetRiyoKengenSwitch(string systemKbn, ISyokuin syokuin) => systemKbn switch
        {
            // 利用権限_加入状況管理
            (SystemKbn.Kjk) => syokuin.KjkKengen,

            // 利用権限_農作物共済
            (SystemKbn.Nsk) => syokuin.NskKengen,

            // 利用権限_建物共済
            (SystemKbn.Sml) => syokuin.SmlKengen,

            // 利用権限_農業者情報管理
            (SystemKbn.Fim) => syokuin.FimKengen,

            // 利用権限_収入保険
            (SystemKbn.Syn) => syokuin.SynKengen,

            // 利用権限_家畜共済（共通申請）
            (SystemKbn.KyotsuKtk) => syokuin.KyotsuKtkKengen,

            // 利用権限_園芸施設共済（共通申請）
            (SystemKbn.KyotsuEng) => syokuin.KyotsuEngKengen,

            // 利用権限_農作物共済（共通申請連携）
            (SystemKbn.KyotsuRenkeiNsk) => syokuin.KyotsuRenkeiNskKengen,

            // 利用権限_畑作物共済（共通申請連携）
            (SystemKbn.KyotsuRenkeiHat) => syokuin.KyotsuRenkeiHatKengen,

            // 利用権限_果樹共済（共通申請連携）
            (SystemKbn.KyotsuRenkeiKju) => syokuin.KyotsuRenkeiKjuKengen,

            // 利用権限_ベースアプリケーション
            (SystemKbn.BAS) => syokuin.SmlKengen,

            // 上記以外
            _ => throw new AppException("MF00001", SystemMessageUtil.Get("MF00001"), CoreConst.HEADER_PATTERN_ID_2)
        };


        /// <summary>
        /// 権限チェック
        /// </summary>
        /// <param name="filterContext"></param>
        public static void CheckAuth(ActionExecutingContext filterContext)
        {
            // Controller のExcludeAuthCheck 属性を取得
            var exclude = (ExcludeAuthCheckAttribute[])filterContext.Controller.GetType().GetCustomAttributes(typeof(ExcludeAuthCheckAttribute), false);
            if (exclude.Length == 0)
            {
                try
                {
                    string screenId = GetScreenId(filterContext.Controller.ToString());

                    var syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, filterContext.HttpContext);

                    if (!"1".Equals(syokuin?.Kengen))
                    {
                        throw new AppException("MI00010", SystemMessageUtil.Get("MI00010"));
                    }

                    if (!AuthorityUtil.HasTransitionAuthority(screenId, filterContext.HttpContext))
                    {
                        // 業務エラーへ遷移
                        throw new AppException("MI00010", SystemMessageUtil.Get("MI00010"));
                    }

                    // 画面操作権限
                    if (!ScreenSosaUtil.CanReference(screenId, filterContext.HttpContext))
                    {
                        // 業務エラーへ遷移
                        throw new AppException("MI00010", SystemMessageUtil.Get("MI00010"));
                    }
                }
                catch (Exception e)
                {
                    SessionUtil.Set(CoreConst.SESS_COMMON_EXCEPTION, e, filterContext.HttpContext);
                    throw;
                }
            }
        }

        /// <summary>
        /// 画面IDを返す
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static string GetScreenId(string controller)
        {
            string screenId = String.Empty;
            var regex = new Regex(@"^.*F\d{1,4}\.Controllers\.(?<controller>(D|C)\d{1,8})Controller$");
            var match = regex.Match(controller);
            if (match.Success)
            {
                screenId = match.Groups["controller"].Value;
            }
            return screenId;
        }
    }
}