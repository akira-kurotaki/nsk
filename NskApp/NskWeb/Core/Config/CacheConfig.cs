using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Utility;
using NLog;

namespace NskWeb.Core.Config
{
    /// <summary>
    /// アプリ起動時にキャッシュデータを取得するクラス
    /// </summary>
    public class CacheConfig
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// キャッシュデータを取得する。
        /// </summary>
        public static void RegisterMasterTable()
        {
            try
            {
                logger.Info("RegisterMasterTable Start.");

                // キャッシュマネージャーのインスタンス取得
                CacheManager cacheManager = CacheManager.GetInstance();

                // メッセージのマスタデータをキャッシュする
                CacheBase mMessageCache = new MMessageCache(cacheManager);
                cacheManager.Set(CoreConst.M_MESSAGE_CACHE, mMessageCache.FindAll(), CacheUtil.cacheExpirationTime);

                // ヘルプメッセージのマスタデータをキャッシュする
                //CacheBase mHelpMessageCache = new MHelpMessageCache(cacheManager);
                //cacheManager.Set(CoreConst.M_HELP_MESSAGE_CACHE, mHelpMessageCache.FindAll(), CacheUtil.cacheExpirationTime);

                // 画面のマスタデータをキャッシュする
                CacheBase mScreenCache = new MScreenCache(cacheManager);
                cacheManager.Set(CoreConst.M_SCREEN_CACHE, mScreenCache.FindAll(), CacheUtil.cacheExpirationTime);

                // 元号のマスタデータをキャッシュする
                //CacheBase mGengoCache = new MGengoCache(cacheManager);
                //cacheManager.Set(CoreConst.M_GENGO_CACHE, mGengoCache.FindAll(), CacheUtil.cacheExpirationTime);

                // 汎用区分のマスタデータをキャッシュする
                CacheBase mHanyokubunCache = new MHanyokubunCache(cacheManager);
                cacheManager.Set(CoreConst.M_HANYOKUBUN_CACHE, mHanyokubunCache.FindAll(), CacheUtil.cacheExpirationTime);

                // 名称のマスタデータをキャッシュする
                //CacheBase mMeishoCache = new MMeishoCache(cacheManager);
                //cacheManager.Set(CoreConst.M_MEISHO_CACHE, mMeishoCache.FindAll(), CacheUtil.cacheExpirationTime);

                // 都道府県のマスタデータをキャッシュする
                CacheBase vTodofukenCache = new MTodofukenCache(cacheManager);
                cacheManager.Set(CoreConst.M_TODOFUKEN_CACHE, vTodofukenCache.FindAll(), CacheUtil.cacheExpirationTime);

                // システム設定値のマスタデータをキャッシュする
                CacheBase mCoreConfigCache = new MCoreConfigCache(cacheManager);
                cacheManager.Set(CoreConst.M_CORE_CONFIG_CACHE, mCoreConfigCache.FindAll(), CacheUtil.cacheExpirationTime);

                // 帳票処理管理のマスタデータをキャッシュする
                //CacheBase mReportKanriCache = new MReportKanriCache(cacheManager);
                //cacheManager.Set(CoreConst.M_REPORT_KANRI_CACHE, mReportKanriCache.FindAll(), CacheUtil.cacheExpirationTime);

                // メニューのマスタデータをキャッシュする
                CacheBase mMenuCache = new MMenuCache(cacheManager);
                cacheManager.Set(CoreConst.M_MENU_CACHE, mMenuCache.FindAll(), CacheUtil.cacheExpirationTime);

                // ヘルプメニューのマスタデータをキャッシュする
                CacheBase mHelpMenuCache = new MHelpMenuCache(cacheManager);
                cacheManager.Set(CoreConst.M_HELP_MENU_CACHE, mHelpMenuCache.FindAll(), CacheUtil.cacheExpirationTime);

                // 年度のマスタデータをキャッシュする
                CacheBase mNendoCache = new MNendoCache(cacheManager);
                cacheManager.Set(CoreConst.M_NENDO_CACHE, mNendoCache.FindAll(), CacheUtil.cacheExpirationTime);

                // 入力方法PDFのマスタデータをキャッシュする
                //CacheBase mNyuryokuhohoPdfCache = new MNyuryokuhohoPdfCache(cacheManager);
                //cacheManager.Set(CoreConst.M_NYURYOKUHOHO_PDF_CACHE, mNyuryokuhohoPdfCache.FindAll(), CacheUtil.cacheExpirationTime);

                // メッセージのマスタデータをキャッシュする（システム共通）
                SystemCacheBase mSystemMessageCache = new MSystemMessageCache(cacheManager);
                cacheManager.Set(CoreConst.M_SYSTEM_MESSAGE_CACHE, mSystemMessageCache.FindAll(), CacheUtil.cacheExpirationTime);

                logger.Info("RegisterMasterTable Completed.");
            }
            catch (Exception e)
            {
                logger.Fatal("キャッシュデータ取得失敗");
                logger.Fatal(MessageUtil.GetErrorMessage(e, CoreConst.LOG_MAX_INNER_EXCEPTION));
            }
        }
    }
}