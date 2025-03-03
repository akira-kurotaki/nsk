using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using NLog;

namespace BaseDmpApi.Core.Config
{
    /// <summary>
    /// アプリ起動時にキャッシュデータを取得するクラス
    /// </summary>
    public class CacheConfig
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// キャッシュデータを取得する。
        /// </summary>
        public static void RegisterMasterTable()
        {
            logger.Info("RegisterMasterTable Start.");

            // キャッシュマネージャーのインスタンス取得
            CacheManager cacheManager = CacheManager.GetInstance();

            // メッセージのマスタデータをキャッシュする
            CacheBase mMessageCache = new MMessageCache(cacheManager);
            cacheManager.Set(CoreConst.M_MESSAGE_CACHE, mMessageCache.FindAll(), CacheUtil.cacheExpirationTime);

            logger.Info("RegisterMasterTable Completed.");
        }
    }
}