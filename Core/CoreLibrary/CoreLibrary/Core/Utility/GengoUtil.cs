using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using ModelLibrary.Models;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 元号マスタのユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/10/30
    /// 作成者：Rou I
    /// </remarks>
    public static class GengoUtil
    {

        /// <summary>
        /// 元号マスタの取得メソッド。
        /// </summary>
        /// <returns>元号リスト</returns>
        public static IEnumerable<MGengo> GetGengoList()
        {
            MGengoCache mGengoCache = new MGengoCache(CacheManager.GetInstance());
            return CacheUtil.Get<IEnumerable<MGengo>>(CacheManager.GetInstance(), CoreConst.M_GENGO_CACHE, () => (IEnumerable<MGengo>)mGengoCache.GetList());
        }

        /// <summary>
        /// 元号マスタのリフレッシュメソッド。
        /// </summary>
        /// <returns>なし</returns>
        public static void Refresh()
        {
            MGengoCache mGengoCache = new MGengoCache(CacheManager.GetInstance());
            CacheUtil.Refresh<IEnumerable<MGengo>>(CacheManager.GetInstance(), CoreConst.M_GENGO_CACHE, () => (IEnumerable<MGengo>)mGengoCache.GetList());
        }

    }
}