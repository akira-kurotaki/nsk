using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Dto;
using static CoreLibrary.Core.Consts.CoreConst;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 画面操作制限ユーティリティ
    /// </summary>
    public static class ScreenSosaUtil
    {
        /// <summary>
        /// ログインユーザ（セッションに格納されたSyokuin）が指定された画面機能コードの画面機能に対して参照権限を持つか判定する。
        /// </summary>
        /// <param name="screenKinoCd">画面機能コード</param>
        /// <param name="context">HTTPコンテキスト</param>
        /// <returns>true：参照権限あり、false：参照権限なし</returns>
        public static bool CanReference(string screenKinoCd, HttpContext context)
        {
            var authMap = SessionUtil.Get<Dictionary<string, ScreenSosaKengen>>(CoreConst.SESS_SCREEN_KINO_KENGEN, context);
            if (authMap.ContainsKey(screenKinoCd))
            {
                return !((int)ReferenceUpdateKengen.True).ToString().Equals(authMap[screenKinoCd].NoReferFlg);
            }

            // 一覧に含まれない場合は、参照権限あり
            return true;
        }

        /// <summary>
        /// ログインユーザ（セッションに格納されたSyokuin）が指定された画面機能コードの画面機能に対して更新権限を持つか判定する。
        /// </summary>
        /// <param name="screenKinoCd">画面機能コード</param>
        /// <param name="context">HTTPコンテキスト</param>
        /// <returns>true：更新権限あり、false：更新権限なし</returns>
        public static bool CanUpdate(string screenKinoCd, HttpContext context)
        {
            var authMap = SessionUtil.Get<Dictionary<string, ScreenSosaKengen>>(CoreConst.SESS_SCREEN_KINO_KENGEN, context);
            if (authMap.ContainsKey(screenKinoCd))
            {
                return !((int)ReferenceUpdateKengen.True).ToString().Equals(authMap[screenKinoCd].NoUpdateFlg);
            }

            // 一覧に含まれない場合は、更新権限あり
            return true;
        }

        /// <summary>
        /// ログインユーザ（セッションに格納されたSyokuin）が参照可能な支所一覧を取得する。
        /// </summary>
        /// <param name="context">HTTPコンテキスト</param>
        /// <returns>参照可能な支所一覧</returns>
        public static List<Shisho> GetShishoList(HttpContext context)
        {
            return SessionUtil.Get<List<Shisho>>(CoreConst.SESS_SHISHO_GROUP, context);
        }
    }
}
