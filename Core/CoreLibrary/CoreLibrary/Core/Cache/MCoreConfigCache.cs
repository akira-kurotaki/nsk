using CoreLibrary.Core.Consts;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Cache
{
    /// <summary>
    /// システム設定値マスタテーブル用のキャッシュクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/19
    /// 作成者：Nakamura Koichi
    /// </remarks>
    public class MCoreConfigCache : CacheBase
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンストラクタメソッド
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        public MCoreConfigCache(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// 全件別の取得処理の実装メソッド
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> FindAll()
        {
            return cacheManager.Get(CoreConst.M_CORE_CONFIG_CACHE, () => GetList());
        }

        /// <summary>
        /// 件別取得処理ロジックの実装メソッド（コールバックメソッド）
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> GetList()
        {
            logger.Info("メッセージ設定値マスタデータを取得する。");
            return db.MCoreConfigs
                .AsEnumerable()
                .Select(m => new MCoreConfig
                {
                    SearchKey = m.SearchKey,
                    ConfigValue = m.ConfigValue
                }).ToList();
        }
    }
}