using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using ModelLibrary.Models;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using static CoreLibrary.Core.Consts.CoreConst;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// ログイン時遷移先ユーティリティ
    /// </summary>
    /// <remarks>
    /// 作成日：2022/07/12
    /// 作成者：Nakamura Koichi
    /// </remarks>
    public static class LoginTransitionUtil
    {
        /// <summary>
        /// 画面表示モード取得
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>画面表示モード</returns>
        public static ScreenMode GetScreenMode(string url)
        {
            // キャッシュから全件取得
            MLoginTransitionCache mLoginTransitionCache = new MLoginTransitionCache(CacheManager.GetInstance());
            List<MLoginTransition> mLoginTransitions = 
                CacheUtil.Get(CacheManager.GetInstance(), 
                CoreConst.M_LOGIN_TRANSITION_CACHE, 
                () => (IEnumerable<MLoginTransition>)mLoginTransitionCache.GetList()).ToList();

            foreach(var mLoginTransition in mLoginTransitions)
            {
                var match = new Regex(mLoginTransition.Keyword, RegexOptions.IgnoreCase).Match(url);
                if (match.Success)
                {
                    var screenMode = match.Groups["sm"].Value;
                    if (!string.IsNullOrEmpty(screenMode))
                    {
                        if (screenMode.Equals(mLoginTransition.ScreenPc))
                        {
                            return ScreenMode.PC;
                        }
                        else if (screenMode.Equals(mLoginTransition.ScreenTablet))
                        {
                            return ScreenMode.Tablet;
                        }
                    }
                    break;
                }
            }
            return ScreenMode.None;
        }

        /// <summary>
        /// デフォルト遷移先URL取得
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="screenMode">画面表示モード</param>
        /// <returns>デフォルト遷移先URL</returns>
        public static string GetReturnUrl(string url, ScreenMode screenMode)
        {
            MLoginTransitionCache mLoginTransitionCache = new MLoginTransitionCache(CacheManager.GetInstance());
            List<MLoginTransition> mLoginTransitions =
                CacheUtil.Get(CacheManager.GetInstance(),
                CoreConst.M_LOGIN_TRANSITION_CACHE,
                () => (IEnumerable<MLoginTransition>)mLoginTransitionCache.GetList()).ToList();

            foreach (var mLoginTransition in mLoginTransitions)
            {
                var match = new Regex(mLoginTransition.Keyword, RegexOptions.IgnoreCase).Match(url);
                if (match.Success)
                {
                    NameValueCollection queryStrings = HttpUtility.ParseQueryString(string.Empty);
                    if (ScreenMode.PC.Equals(screenMode))
                    {
                        queryStrings.Add("sm", mLoginTransition.ScreenPc);
                        return mLoginTransition.DefaultUrl + "?" + queryStrings.ToString();
                    }
                    else if (ScreenMode.Tablet.Equals(screenMode))
                    {
                        queryStrings.Add("sm", mLoginTransition.ScreenTablet);
                        return mLoginTransition.DefaultUrl + "?" + queryStrings.ToString();
                    }
                    break;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// ログイン時遷移先マスタのリフレッシュメソッド。
        /// </summary>
        /// <returns>なし</returns>
        public static void Refresh()
        {
            MLoginTransitionCache mLoginTransitionCache = new MLoginTransitionCache(CacheManager.GetInstance());
            CacheUtil.Refresh<IEnumerable<MLoginTransition>>(CacheManager.GetInstance(), CoreConst.M_LOGIN_TRANSITION_CACHE, () => (IEnumerable<MLoginTransition>)mLoginTransitionCache.GetList());
        }
    }
}