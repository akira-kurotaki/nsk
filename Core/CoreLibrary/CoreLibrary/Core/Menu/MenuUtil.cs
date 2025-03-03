using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Utility;
using ModelLibrary.Models;

namespace CoreLibrary.Core.Menu
{
    /// <summary>
    /// メニューユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/02/09
    /// 作成者：Rou I
    /// </remarks>
    public static class MenuUtil
    {
        /// <summary>
        /// メニュー表示フラグ「1:表示」
        /// </summary>
        public const string MENU_DISPLAY = "1";

        /// <summary>
        /// メニュー情報の取得メソッド。
        /// </summary>
        /// <returns>メニュー情報</returns>
        //public static IEnumerable<MenuItem> GetLeftMenuItems()
        public static IEnumerable<MenuItem> GetLeftMenuItems(HttpContext context)
        {
            List<MenuItem> menuItems = new List<MenuItem>();

            Syokuin syokuin = SessionUtil.Get<Syokuin>(CoreConst.SESS_LOGIN_USER, context);

            if (syokuin == null)
            {
                return menuItems;
            }

            // 第1階層メニューリストの取得
            var menuList = GetMenuDisplayList(GetMenuList(), syokuin.SystemRiyoKbn, syokuin.UserKanriKengen);

            if (menuList.Count() == 0)
            {
                return menuItems;
            }

            foreach (var menu in menuList)
            {
                // 第2階層メニューリストの取得
                var childMenuList = ScreenUtil.GetScreenList().Where(a => a.FirstMenuGroup == menu.FirstMenuGroup)
                                                              .Where(a => a.SecondMenuDisplayFlg == MENU_DISPLAY)
                                                              .OrderBy(a => a.SecondMenuDisplayOrder);
                if (childMenuList.Count() == 0)
                {
                    continue;
                }

                MenuItem menuItem = new MenuItem();
                menuItem.Text = menu.FirstMenuDisplayKinoNm;
                menuItem.ScreenId = "#";

                List<MenuItem> childMenuItems = new List<MenuItem>();

                foreach (var childMenu in childMenuList)
                {
                    // 画面別遷移権限チェック
                    if (AuthorityUtil.HasTransitionAuthority(childMenu.ScreenId, context))
                    {
                        MenuItem childMenuItem = new MenuItem();
                        childMenuItem.Text = childMenu.SecondMenuDisplayNm;
                        childMenuItem.ScreenId = childMenu.ScreenId;
                        childMenuItems.Add(childMenuItem);
                    }
                }

                if (childMenuItems.Count() == 0)
                {
                    continue;
                }

                menuItem.ChildItems = childMenuItems;

                menuItems.Add(menuItem);
            }

            return menuItems;
        }

        /// <summary>
        /// メニューマスタのデータ取得メソッド。
        /// </summary>
        /// <param name="firstMenuGroup">第1階層メニューグループ</param>
        /// <returns>メニューマスタデータ</returns>
        public static MMenu GetMenu(string firstMenuGroup)
        {
            return GetMenuList().Where(a => a.FirstMenuGroup == firstMenuGroup).FirstOrDefault();
        }

        /// <summary>
        /// メニューマスタのデータ取得メソッド。
        /// </summary>
        /// <returns>メニューリスト</returns>
        public static IEnumerable<MMenu> GetMenuList()
        {
            MMenuCache mMenuCache = new MMenuCache(CacheManager.GetInstance());
            return CacheUtil.Get<IEnumerable<MMenu>>(CacheManager.GetInstance(), CoreConst.M_MENU_CACHE, () => (IEnumerable<MMenu>)mMenuCache.GetList());
        }

        /// <summary>
        /// メニューマスタのリフレッシュメソッド。
        /// </summary>
        /// <returns>なし</returns>
        public static void Refresh()
        {
            MMenuCache mMenuCache = new MMenuCache(CacheManager.GetInstance());
            CacheUtil.Refresh<IEnumerable<MMenu>>(CacheManager.GetInstance(), CoreConst.M_MENU_CACHE, () => (IEnumerable<MMenu>)mMenuCache.GetList());
        }

        /// <summary>
        /// メニュー表示リスト取得メソッド。
        /// </summary>
        /// <param name="menuList">メニューリスト</param>
        /// <param name="systemRiyoKbn">システム利用者区分</param>
        /// <param name="userKanriKengen">ユーザ管理権限</param>
        /// <returns>メニュー表示リスト</returns>
        private static IEnumerable<MMenu> GetMenuDisplayList(IEnumerable<MMenu> menuList, string systemRiyoKbn, string userKanriKengen)
        {
            // メニュー表示フラグ = 1:表示
            menuList = menuList.Where(a => a.FirstMenuDisplayFlg == MENU_DISPLAY);

            // 引数.ユーザ管理権限が「0:なし」の場合、ユーザ管理権限が「0」のレコードを取得
            if (userKanriKengen == AuthorityUtil.USER_KANRI_KENGEN_NASHI)
            {
                menuList = menuList.Where(a => a.UserKanriKengen == AuthorityUtil.USER_KANRI_KENGEN_NASHI);
            }

            // 表示区分
            menuList = menuList.Where(a => a.HyojiKbn.Contains(systemRiyoKbn));

            return menuList.OrderBy(a => a.FirstMenuDisplayOrder);
        }
    }
}