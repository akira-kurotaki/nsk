using CoreLibrary.Core.Consts;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Cache
{
    /// <summary>
    /// 元号マスタ用のキャッシュクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/10/30
    /// 作成者：Rou I
    /// </remarks>
    public class MGengoCache : CacheBase
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンストラクタメソッド
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        public MGengoCache(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// 全件別の取得処理の実装メソッド。
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> FindAll()
        {
            return cacheManager.Get(CoreConst.M_GENGO_CACHE, () => GetList());
        }

        /// <summary>
        /// 件別取得処理ロジックの実装メソッド（コールバックメソッド）。
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> GetList()
        {
            logger.Info("元号マスタデータを取得する。");
            return db.MGengos
                    .AsEnumerable().Select(m => new MGengo
                    {
                        Gengo = m.Gengo,
                        Ryakugo = m.Ryakugo,
                        RyakugoEn = m.RyakugoEn,
                        TekiyoStartYmd = m.TekiyoStartYmd,
                        TekiyoEndYmd = m.TekiyoEndYmd
                    }).ToList();
        }
    }
}