using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using CoreLibrary.Core.Menu;
using CoreLibrary.Core.Utility;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using Npgsql;
using NskWeb.Common.NskMenu;
using System;
using System.Linq;
using System.Text;

namespace NskWeb.Common.Menu
{
    /// <summary>
    /// メニューユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/02/09
    /// 作成者：Rou I
    /// </remarks>
    public static class NskMenuUtil
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
        public static IEnumerable<NskMenuItem> GetLeftMenuItems(HttpContext context, DbContext db, string strKyosaiMokutekiCd)
        {
            List<NskMenuItem> menuItems = new List<NskMenuItem>();

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

            var menuStructList = GetMenuStructList(db, strKyosaiMokutekiCd);
            foreach (var menu in menuList)
            {
                // 対象の第1階層より下のメニュー構成情報の取得
                var childMenuList = menuStructList.Where(a => a.第1階層メニューグループ == menu.FirstMenuGroup)
                                                              .OrderBy(a => a.SortKey);
                if (childMenuList.Count() == 0)
                {
                    continue;
                }

                // 第1階層の情報をセット
                NskMenuItem menuItem = new NskMenuItem();
                menuItem.Text = menu.FirstMenuDisplayKinoNm;
                menuItem.MenuId = "MID" + menu.FirstMenuGroup;
                menuItem.ScreenId = "#";
                menuItem.MenuLevel = 0;
                menuItem.OpeId = "";
                List<NskMenuItem> childMenuItems = new List<NskMenuItem>();

                // 第2階層以下の情報をセット
                foreach (var childMenu in childMenuList)
                {
                    NskMenuItem childMenuItem = new NskMenuItem();
                    childMenuItem.Text = childMenu.メニュー名称;
                    childMenuItem.MenuId = menuItem.MenuId + "-" + childMenu.メニューID;
                    childMenuItem.MenuLevel = childMenu.MenuLevel;
                    if (!string.IsNullOrEmpty(childMenu.画面id))
                    {
                        // 画面IDが設定されている場合
                        // 画面別遷移権限チェック
                        if (AuthorityUtil.HasTransitionAuthority(childMenu.画面id, context))
                        {
                            childMenuItem.ScreenId = childMenu.画面id;
                            childMenuItem.OpeId = childMenu.処理id;
                        }
                        else
                        {
                            // 遷移権限が無いときは遷移先IDを設定しない
                            childMenuItem.ScreenId = "";
                            childMenuItem.OpeId = "";
                        }
                    }
                    else
                    {
                        // 画面IDが設定されていない場合
                        childMenuItem.ScreenId = "#";
                        childMenuItem.OpeId = "";
                    }
                    childMenuItems.Add(childMenuItem);
                }
                menuItem.ChildItems = childMenuItems;
                menuItems.Add(menuItem);
            }

            return menuItems;
        }

        /// <summary>
        /// メニュー構成情報の取得
        /// </summary>
        /// <returns>メニュー構成情報リスト</returns>
        private static List<NskMenuStruct> GetMenuStructList(DbContext db, string strKyosaiMokutekiCd)
        {
            List<NskMenuStruct> msList = new List<NskMenuStruct>();

            var sql = new StringBuilder();
            var parameters = new List<NpgsqlParameter>();
            sql.Append("with RECURSIVE xmenu as ( ");
            sql.Append("select ");
            sql.Append("第1階層メニューグループ ");
            sql.Append(",1 as lvl ");
            sql.Append(", lpad(第1階層メニューグループ,2,'0') || '-' || lpad(cast(並び順 as character varying(3)),3,'0') as sortkey ");
            sql.Append(",メニューID ");
            sql.Append(",メニュー名称 ");
            sql.Append(",親メニューID ");
            sql.Append(",共済目的コード ");
            sql.Append(",画面id ");
            sql.Append(",処理id ");
            sql.Append("from m_00240_メニュー  ");
            sql.Append("where 親メニューID ='' ");
            sql.Append("union all ");
            sql.Append("select  ");
            sql.Append("b.第1階層メニューグループ ");
            sql.Append(",b.lvl +1 as lvl ");
            sql.Append(",b.sortkey || '-' || lpad(cast(a.並び順 as character varying(3)),3,'0') ");
            sql.Append(",a.メニューID ");
            sql.Append(",a.メニュー名称 ");
            sql.Append(",a.親メニューID ");
            sql.Append(",a.共済目的コード ");
            sql.Append(",a.画面id ");
            sql.Append(",a.処理id ");
            sql.Append("from m_00240_メニュー a,xmenu b  ");
            sql.Append("where a.親メニューID=b.メニューID  ");
            sql.Append(") ");
            sql.Append("select * from xmenu ");
            sql.Append("where 共済目的コード is null or 共済目的コード = '' or 共済目的コード = @KyosaiMokutekiCd ");
            sql.Append("order by sortkey ");
            parameters.Add(new NpgsqlParameter("@KyosaiMokutekiCd", strKyosaiMokutekiCd));

            msList = db.Database.SqlQueryRaw<NskMenuStruct>(sql.ToString(),parameters.ToArray()).ToList();

            return msList;
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