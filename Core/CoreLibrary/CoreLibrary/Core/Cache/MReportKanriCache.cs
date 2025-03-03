using CoreLibrary.Core.Consts;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Cache
{
    /// <summary>
    /// 帳票処理管理マスタ用キャッシュクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/1/29
    /// 作成者：Rou I
    /// </remarks>
    public class MReportKanriCache : CacheBase
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンストラクタメソッド
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        public MReportKanriCache(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// 全件別の取得処理の実装メソッド。
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> FindAll()
        {
            return cacheManager.Get(CoreConst.M_REPORT_KANRI_CACHE, () => GetList());
        }

        /// <summary>
        /// 件別取得処理ロジックの実装メソッド（コールバックメソッド）。
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> GetList()
        {
            logger.Info("帳票処理管理マスタデータを取得する。");
            return db.MReportKanris
                    .AsEnumerable()
                    .Select(m => new MReportKanri
                    {
                        ReportControlId = m.ReportControlId,
                        SerialNumber = m.SerialNumber,
                        BatchShoriKensu = m.BatchShoriKensu,
                        ReportControlNm = m.ReportControlNm,
                        FileNm = m.FileNm,
                        YoyakuNm = m.YoyakuNm
                    })
                    .OrderBy(a => a.ReportControlId)
                    .ThenBy(a => a.SerialNumber)
                    .ToList();
        }
    }
}