using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using ModelLibrary.Models;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 名称マスタのユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/18
    /// 作成者：Rou I
    /// </remarks>
    public static class MeishoUtil
    {
        /// <summary>
        /// 名称マスタの取得メソッド。
        /// </summary>
        /// <returns>名称リスト</returns>
        public static IEnumerable<MMeisho> GetMeishoList()
        {
            MMeishoCache mMeishoCache = new MMeishoCache(CacheManager.GetInstance());
            return CacheUtil.Get<IEnumerable<MMeisho>>(CacheManager.GetInstance(), CoreConst.M_MEISHO_CACHE, () => (IEnumerable<MMeisho>)mMeishoCache.GetList());
        }

        /// <summary>
        /// 名称マスタのリフレッシュメソッド。
        /// </summary>
        /// <returns>なし</returns>
        public static void Refresh()
        {
            MMeishoCache mMeishoCache = new MMeishoCache(CacheManager.GetInstance());
            CacheUtil.Refresh<IEnumerable<MMeisho>>(CacheManager.GetInstance(), CoreConst.M_MEISHO_CACHE, () => (IEnumerable<MMeisho>)mMeishoCache.GetList());
        }
    }
}