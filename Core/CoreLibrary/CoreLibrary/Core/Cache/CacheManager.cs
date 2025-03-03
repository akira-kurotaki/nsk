using CoreLibrary.Core.Utility;
using System.Runtime.Caching;

namespace CoreLibrary.Core.Cache
{
    /// <summary>
    /// キャッシュマネージャー実装クラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/10/30
    /// 作成者：Rou I
    /// </remarks>
    public class CacheManager : ICacheManager
    {
        /// <summary>
        /// キャッシュマネジャーのインスタンス変数
        /// </summary>
        private static CacheManager instance = null;

        /// <summary>
        /// コンストラクタを privateに定義（初期化できない）
        /// </summary>
        private CacheManager() { }

        /// <summary>
        /// キャッシュマネジャーのインスタンス取得メソッド（シングルトン・パターン）。
        /// </summary>
        /// <returns>キャッシュマネジャーインスタンス</returns>
        public static CacheManager GetInstance()
        {
            if (instance == null)
                instance = new CacheManager();
            return instance;
        }

        /// <summary>
        /// システムメモリキャッシュの取得メソッド。
        /// </summary>
        /// <returns>システムメモリキャッシュ</returns>
        private ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        /// <summary>
        /// キャッシュ対象の取得メソッド。
        /// </summary>
        /// <param name="key">キャッシュキー</param>
        /// <returns>キャッシュ対象</returns>
        public T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        /// <summary>
        /// キャッシュ対象の設定メソッド。
        /// </summary>
        /// <param name="key">キャッシュキー</param>
        /// <param name="data">キャッシュデータ</param>
        /// <param name="cacheTime">キャッシュ保存有効期限</param>
        /// <returns>なし</returns>
        public void Set(string key, object data, int cacheTime)
        {
            if (data == null)
            {
                return;
            }

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);

            Cache.Add(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// キャッシュ対象設定ありの判定メソッド。
        /// </summary>
        /// <param name="key">キャッシュキー</param>
        /// <returns>設定ありの場合、trueを返す</returns>
        public bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        /// <summary>
        /// キャッシュ対象の削除メソッド。
        /// </summary>
        /// <param name="key">キャッシュキー</param>
        /// <returns>なし</returns>
        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// キャッシュ対象のリフレッシュメソッド。
        /// </summary>
        /// <param name="key">キャッシュキー</param>
        /// <param name="data">キャッシュデータ</param>
        /// <param name="cacheTime">キャッシュ保存有効期限</param>
        /// <returns>なし</returns>
        public void Refresh(string key, object data, int cacheTime)
        {
            if (data == null)
            {
                return;
            }

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateUtil.GetSysDateTime() + TimeSpan.FromMinutes(cacheTime);

            if (IsSet(key))
            {
                Cache.Set(key, data, policy);
            }
            else
            {
                Cache.Add(new CacheItem(key, data), policy);
            }
        }

        /// <summary>
        /// キャッシュ全対象のクリアメソッド。
        /// </summary>
        /// <returns>なし</returns>
        public void Clear()
        {
            foreach (var item in Cache)
            {
                Remove(item.Key);
            }
        }
    }
}