using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using ModelLibrary.Models;

namespace CoreLibrary.Core.Utility
{
    public class CoreConfigUtil
    {
        /// <summary>
        /// キャッシュから設定値を取得する。
        /// </summary>
        /// <param name="key">検索キー</param>
        /// <returns>システム設定値マスタ．設定値</returns>
        public static string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            MCoreConfigCache mCoreConfigCache = new MCoreConfigCache(CacheManager.GetInstance());
            var mCoreConfig = CacheUtil.Get(CacheManager.GetInstance(), CoreConst.M_CORE_CONFIG_CACHE,
                () => mCoreConfigCache.GetList() as IEnumerable<MCoreConfig>)
                .SingleOrDefault(a => a.SearchKey == key);
            return (null == mCoreConfig || string.IsNullOrEmpty(mCoreConfig.ConfigValue)) ? string.Empty : mCoreConfig.ConfigValue;
        }

        /// <summary>
        /// システム設定値マスタのリフレッシュメソッド。
        /// </summary>
        /// <returns>なし</returns>
        public static void Refresh()
        {
            MCoreConfigCache mCoreConfigCache = new MCoreConfigCache(CacheManager.GetInstance());
            CacheUtil.Refresh<IEnumerable<MCoreConfig>>(CacheManager.GetInstance(), CoreConst.M_CORE_CONFIG_CACHE, () => (IEnumerable<MCoreConfig>)mCoreConfigCache.GetList());
        }
    }
}