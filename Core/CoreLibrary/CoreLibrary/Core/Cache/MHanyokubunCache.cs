using CoreLibrary.Core.Consts;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Cache
{
    /// <summary>
    /// 汎用区分マスタ用のキャッシュクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2017/10/30
    /// 作成者：Rou I
    /// </remarks>
    public class MHanyokubunCache : CacheBase
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンストラクタメソッド
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        public MHanyokubunCache(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// 全件別の取得処理の実装メソッド。
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> FindAll()
        {
            return cacheManager.Get(CoreConst.M_HANYOKUBUN_CACHE, () => GetList());
        }

        /// <summary>
        /// 件別取得処理ロジックの実装メソッド（コールバックメソッド）。
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> GetList()
        {
            logger.Info("汎用区分マスタデータを取得する。");
            return db.MHanyokubuns
                     .Where(a => a.DeleteFlg == "0")
                     .AsEnumerable()
                     .Select(m => new MHanyokubun
                     {
                         KbnSbt = m.KbnSbt,
                         KbnCd = m.KbnCd,
                         Sort = m.Sort,
                         KbnNm = m.KbnNm,
                         KbnRnm = m.KbnRnm
                     })
                     .OrderBy(a => a.KbnSbt)
                     .ThenBy(a => a.Sort).ToList();
        }
    }
}