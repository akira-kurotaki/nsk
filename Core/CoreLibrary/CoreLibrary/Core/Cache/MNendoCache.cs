using CoreLibrary.Core.Consts;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Cache
{
    /// <summary>
    /// 年度マスタテーブルのキャッシュクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/02/13
    /// 作成者：Rou I
    /// </remarks>
    public class MNendoCache : CacheBase
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンストラクタメソッド
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        public MNendoCache(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// 全件別の取得処理の実装メソッド
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> FindAll()
        {
            return cacheManager.Get(CoreConst.M_NENDO_CACHE, () => GetList());
        }

        /// <summary>
        /// 件別取得処理ロジックの実装メソッド（コールバックメソッド）
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> GetList()
        {
            logger.Info("年度マスタデータを取得する。");
            return db.MNendos
                .AsEnumerable()
                .Select(m => new MNendo
                {
                    Nendo = m.Nendo,
                    NendoDisp1 = m.NendoDisp1,
                    NendoDisp2 = m.NendoDisp2,
                    NendoDisp3 = m.NendoDisp3
                })
                .OrderBy(a => a.Nendo)
                .ToList();
        }
    }
}