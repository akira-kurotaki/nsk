using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;

namespace CoreLibrary.Core.HelpMenu
{
    /// <summary>
    /// ヘルプメニューユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/04/13
    /// 作成者：MATSUBAYASHI Atsushi
    /// </remarks>
    public class HelpMenuUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<HelpMenuItem> GetHelpMenuItems(HttpContext context)
        {
            List<HelpMenuItem> helpMenuItems = new List<HelpMenuItem>();

            Syokuin syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, context);

            var systemRiyoKbn = string.Empty;
            var userKanriKengen = string.Empty;

            if (syokuin != null)
            {
                systemRiyoKbn = syokuin.SystemRiyoKbn;
                userKanriKengen = syokuin.UserKanriKengen;

                if (string.IsNullOrEmpty(systemRiyoKbn) ||
                    string.IsNullOrEmpty(userKanriKengen))
                {
                    return helpMenuItems;
                }
            }

            // ヘルプメニューリストの取得
            var helpMenuList = GetHelpMenuDisplayList(GetHelpMenuList(), systemRiyoKbn, userKanriKengen);

            foreach (var helpMenu in helpMenuList)
            {
                HelpMenuItem helpMenuItem = new HelpMenuItem();
                helpMenuItem.Hash = helpMenu.Hash;
                helpMenuItem.Text = helpMenu.HelpMenuDisplayKinoNm;
                helpMenuItems.Add(helpMenuItem);
            }

            return helpMenuItems;
        }

        /// <summary>
        /// ヘルプメニューマスタのデータ取得メソッド。
        /// </summary>
        /// <returns>ヘルプメニューリスト</returns>
        public static IEnumerable<HelpMenuDto> GetHelpMenuList()
        {
            MHelpMenuCache mHelpMenuCache = new MHelpMenuCache(CacheManager.GetInstance());
            return CacheUtil.Get<IEnumerable<HelpMenuDto>>(CacheManager.GetInstance(), CoreConst.M_HELP_MENU_CACHE, () => (IEnumerable<HelpMenuDto>)mHelpMenuCache.GetList());
        }

        /// <summary>
        /// ヘルプメニューマスタのリフレッシュメソッド。
        /// </summary>
        /// <returns>なし</returns>
        public static void Refresh()
        {
            MHelpMenuCache mHelpMenuCache = new MHelpMenuCache(CacheManager.GetInstance());
            CacheUtil.Refresh<IEnumerable<HelpMenuDto>>(CacheManager.GetInstance(), CoreConst.M_HELP_MENU_CACHE, () => (IEnumerable<HelpMenuDto>)mHelpMenuCache.GetList());
        }


        /// <summary>
        /// ヘルプメニュー表示リスト取得メソッド。
        /// </summary>
        /// <param name="helpMenuList">ヘルプメニューリスト</param>
        /// <param name="systemRiyoKbn">システム利用者区分</param>
        /// <param name="userKanriKengen">ユーザ管理権限</param>
        /// <returns>ヘルプメニュー表示リスト</returns>
        private static IEnumerable<HelpMenuDto> GetHelpMenuDisplayList(IEnumerable<HelpMenuDto> helpMenuList, string systemRiyoKbn,
                                                                        string userKanriKengen)
        {
            // メニュー表示フラグ = 1:表示
            helpMenuList = helpMenuList.Where(a => a.HelpMenuDisplayFlg == "1");

            // 引数.ユーザ管理権限が「0:なし」の場合、ユーザ管理権限が「0」のレコードを取得
            if (userKanriKengen == AuthorityUtil.USER_KANRI_KENGEN_NASHI)
            {
                helpMenuList = helpMenuList.Where(a => a.UserKanriKengen == AuthorityUtil.USER_KANRI_KENGEN_NASHI);
            }

            // 表示区分
            helpMenuList = helpMenuList.Where(a => a.HyojiKbn.Contains(systemRiyoKbn));

            return helpMenuList.OrderBy(a => a.HelpMenuDisplayOrder);
        }
    }
}