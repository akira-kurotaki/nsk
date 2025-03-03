using CoreLibrary.Core.Consts;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Cache
{
    /// <summary>
    /// 画面マスタテーブル用のキャッシュクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/11
    /// 作成者：Nakamura Koichi
    /// </remarks>
    public class MScreenCache : CacheBase
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンストラクタメソッド
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        public MScreenCache(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// 全件別の取得処理の実装メソッド
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> FindAll()
        {
            return cacheManager.Get(CoreConst.M_SCREEN_CACHE, () => GetList());
        }

        /// <summary>
        /// 件別取得処理ロジックの実装メソッド（コールバックメソッド）
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> GetList()
        {
            logger.Info("画面マスタデータを取得する。");
            return db.MScreens
                .AsEnumerable()
                .Select(m => new MScreen
                {
                    ScreenId = m.ScreenId,
                    ScreenNm = m.ScreenNm,
                    AreaId = m.AreaId,
                    FirstMenuGroup = m.FirstMenuGroup,
                    SecondMenuDisplayFlg = m.SecondMenuDisplayFlg,
                    SecondMenuDisplayOrder = m.SecondMenuDisplayOrder,
                    SecondMenuDisplayNm = m.SecondMenuDisplayNm,
                    HyojiKbn = m.HyojiKbn,
                    UserKanriKengen = m.UserKanriKengen
                }).ToList();
        }
    }
}