using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using ModelLibrary.Models;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 画面テーブルのユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/11
    /// 作成者：Nakamura Koichi
    /// </remarks>
    public static class ScreenUtil
    {
        /// <summary>
        /// キャッシュから画面名を取得する。
        /// </summary>
        /// <param name="screenId">画面ID</param>
        /// <returns>画面名</returns>
        public static string Get(string screenId)
        {
            if (string.IsNullOrEmpty(screenId))
            {
                return null;
            }

            var mScreen = GetScreen(screenId);
            return (null == mScreen || string.IsNullOrEmpty(mScreen.ScreenNm)) ? string.Empty : mScreen.ScreenNm;
        }

        /// <summary>
        /// キャッシュからエリアIDを取得する。
        /// </summary>
        /// <param name="screenId">画面ID</param>
        /// <returns>エリアID</returns>
        public static string GetArea(string screenId)
        {
            if (string.IsNullOrEmpty(screenId))
            {
                return null;
            }

            var mScreen = GetScreen(screenId);
            return (null == mScreen || string.IsNullOrEmpty(mScreen.AreaId)) ? string.Empty : mScreen.AreaId;
        }

        /// <summary>
        /// 画面マスタのデータ取得メソッド。
        /// </summary>
        /// <param name="screenId">画面ID</param>
        /// <returns>画面データ</returns>
        public static MScreen GetScreen(string screenId)
        {
            if (string.IsNullOrEmpty(screenId))
            {
                return null;
            }

            return GetScreenList().Where(a => a.ScreenId == screenId).SingleOrDefault();
        }

        /// <summary>
        /// 画面マスタのデータ取得メソッド。
        /// </summary>
        /// <returns>画面リスト</returns>
        public static IEnumerable<MScreen> GetScreenList()
        {
            MScreenCache mScreenCache = new MScreenCache(CacheManager.GetInstance());
            return CacheUtil.Get<IEnumerable<MScreen>>(CacheManager.GetInstance(), CoreConst.M_SCREEN_CACHE, () => (IEnumerable<MScreen>)mScreenCache.GetList());
        }

        /// <summary>
        /// 画面マスタのリフレッシュメソッド。
        /// </summary>
        /// <returns>なし</returns>
        public static void Refresh()
        {
            MScreenCache mScreenCache = new MScreenCache(CacheManager.GetInstance());
            CacheUtil.Refresh<IEnumerable<MScreen>>(CacheManager.GetInstance(), CoreConst.M_SCREEN_CACHE, () => (IEnumerable<MScreen>)mScreenCache.GetList());
        }
    }
}