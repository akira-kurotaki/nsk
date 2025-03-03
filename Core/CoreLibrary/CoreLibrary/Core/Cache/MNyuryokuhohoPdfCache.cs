using CoreLibrary.Core.Consts;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Cache
{

    /// <summary>
    /// 入力方法PDFマスタテーブルのキャッシュクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2020/12/09
    /// 作成者：Cho
    /// </remarks>
    public class MNyuryokuhohoPdfCache : CacheBase
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンストラクタメソッド
        /// </summary>
        /// <param name="cacheManager">キャッシュマネージャー</param>
        public MNyuryokuhohoPdfCache(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// 全件別の取得処理の実装メソッド
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> FindAll()
        {
            return cacheManager.Get(CoreConst.M_NYURYOKUHOHO_PDF_CACHE, () => GetList());
        }

        /// <summary>
        /// 件別取得処理ロジックの実装メソッド（コールバックメソッド）
        /// </summary>
        /// <returns>全件取得の結果</returns>
        public override IEnumerable<ModelBase> GetList()
        {
            logger.Info("入力方法PDFマスタデータを取得する。");
            return db.MNyuryokuhohoPdfs
                .AsEnumerable()
                .Select(m => new MNyuryokuhohoPdf
                {
                    ScreenId = m.ScreenId,
                    TekiyoStartYmd = m.TekiyoStartYmd,
                    TekiyoEndYmd = m.TekiyoEndYmd,
                    PdfFilePath = m.PdfFilePath
                })
                .ToList();
        }
    }
}