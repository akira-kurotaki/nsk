using CoreLibrary.Core.Consts;
using ModelLibrary.Models;

namespace CoreLibrary.Core.Cache
{
    /// <summary>
    /// メッセージマスタ用のキャッシュクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/11/13
    /// 作成者：Hiratake Minori
    /// </remarks>
    public class MMessageCache : CacheBase
    {

        /// <summary>
        /// コンストラクタメソッド
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        public MMessageCache(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// 全件別の取得処理の実装メソッド。
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> FindAll()
        {
            return cacheManager.Get(CoreConst.M_MESSAGE_CACHE, () => GetList());
        }

        /// <summary>
        /// 件別取得処理ロジックの実装メソッド（コールバックメソッド）。
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> GetList()
        {
            return db.MMessages.AsEnumerable().Select(m => new MMessage
            {
                MessageId = m.MessageId,
                Message = m.Message
            }).ToList();
        }
    }
}