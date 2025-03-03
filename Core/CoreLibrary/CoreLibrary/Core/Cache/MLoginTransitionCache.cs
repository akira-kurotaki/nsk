using CoreLibrary.Core.Consts;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Cache
{
    /// <summary>
    /// ログイン時遷移先マスタ用のキャッシュクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2022/07/12
    /// 作成者：Nakamura Koichi
    public class MLoginTransitionCache : CacheBase
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンストラクタメソッド
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        public MLoginTransitionCache(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// 全件別の取得処理の実装メソッド
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> FindAll()
        {
            return cacheManager.Get(CoreConst.M_LOGIN_TRANSITION_CACHE, () => GetList());
        }

        /// <summary>
        /// 件別取得処理ロジックの実装メソッド（コールバックメソッド）
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> GetList()
        {
            logger.Info("ログイン時遷移先マスタデータを取得する。");
            return db.MLoginTransitions.AsEnumerable().Select(m => new MLoginTransition
            {
                TransitionNo    = m.TransitionNo,
                Keyword         = m.Keyword,
                DefaultUrl      = m.DefaultUrl,
                ScreenPc        = m.ScreenPc,
                ScreenTablet    = m.ScreenTablet
            }).OrderBy(m => m.TransitionNo).ToList();
        }
    }
}