using CoreLibrary.Core.Consts;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Cache
{
    /// <summary>
    /// 都道府県マスタ用キャッシュクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/18
    /// 作成者：Rou I
    /// </remarks>
    public class MTodofukenCache : CacheBase
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンストラクタメソッド
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        public MTodofukenCache(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// 全件別の取得処理の実装メソッド。
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> FindAll()
        {
            return cacheManager.Get(CoreConst.M_TODOFUKEN_CACHE, () => GetList());
        }

        /// <summary>
        /// 件別取得処理ロジックの実装メソッド（コールバックメソッド）。
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> GetList()
        {
            logger.Info("都道府県マスタからデータを全件取得する。");
            return db.MTodofukens
                    .AsEnumerable()
                    .Select(m => new MTodofuken
                    {
                        TodofukenCd = m.TodofukenCd,
                        TodofukenNm = m.TodofukenNm
                    })
                    .OrderBy(a => a.TodofukenCd).ToList();
        }
    }
}