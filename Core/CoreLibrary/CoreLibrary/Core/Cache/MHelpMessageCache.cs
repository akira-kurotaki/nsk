using CoreLibrary.Core.Consts;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Cache
{
    /// <summary>
    /// ヘルプメッセージマスタ用のキャッシュクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2020/12/15
    /// 作成者：Nakamura Koichi
    /// </remarks>
    public class MHelpMessageCache : CacheBase
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンストラクタメソッド
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        public MHelpMessageCache(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// 全件別の取得処理の実装メソッド。
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> FindAll()
        {
            return cacheManager.Get(CoreConst.M_HELP_MESSAGE_CACHE, () => GetList());
        }

        /// <summary>
        /// 件別取得処理ロジックの実装メソッド（コールバックメソッド）。
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> GetList()
        {
            logger.Info("ヘルプメッセージマスタデータを取得する。");
            return db.MHelpMessages.AsEnumerable().Select(m => new MHelpMessage
            {
                TekiyoStartYmd = m.TekiyoStartYmd,
                TekiyoEndYmd = m.TekiyoEndYmd,
                ScreenId = m.ScreenId,
                ItemNo = m.ItemNo,
                ItemNm = m.ItemNm,
                Message = m.Message,
                DisplayFlg = m.DisplayFlg,
            }).ToList();
        }
    }
}