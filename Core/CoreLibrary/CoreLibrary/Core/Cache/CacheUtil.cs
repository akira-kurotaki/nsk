using CoreLibrary.Core.Utility;

namespace CoreLibrary.Core.Cache
{
    /// <summary>
    /// キャッシュのユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/10/30
    /// 作成者：Rou I
    /// </remarks>
    public static class CacheUtil
    {
        /// <summary>
        /// キャッシュ有効期限(単位：分)
        /// </summary>
        public static readonly int cacheExpirationTime = Convert.ToInt32(ConfigUtil.Get("CacheExpirationTime"));

        /// <summary>
        /// キャッシュ対象の取得メソッド。
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        /// <param name="key">キャッシュキー</param>
        /// <param name="acquire">コードバックメソッド</param>
        /// <returns>キャッシュ対象</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, cacheExpirationTime, acquire);
        }

        /// <summary>
        /// キャッシュ対象の取得メソッド。
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        /// <param name="key">キャッシュキー</param>
        /// <param name="cacheTime">キャッシュ保存有効期限</param>
        /// <param name="acquire">コードバックメソッド</param>
        /// <returns>キャッシュ対象</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }
            else
            {
                var result = acquire();

                cacheManager.Set(key, result, cacheTime);

                return result;
            }
        }

        /// <summary>
        /// キャッシュ対象のリフレッシュメソッド。
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        /// <param name="key">キャッシュキー</param>
        /// <param name="acquire">コードバックメソッド</param>
        /// <returns>なし</returns>
        public static void Refresh<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            Refresh(cacheManager, key, cacheExpirationTime, acquire);
        }

        /// <summary>
        /// キャッシュ対象のリフレッシュメソッド。
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        /// <param name="key">キャッシュキー</param>
        /// <param name="cacheTime">キャッシュ保存有効期限</param>
        /// <param name="acquire">コードバックメソッド</param>
        /// <returns>なし</returns>
        public static void Refresh<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            var result = acquire();

            cacheManager.Refresh(key, result, cacheTime);
        }
    }
}