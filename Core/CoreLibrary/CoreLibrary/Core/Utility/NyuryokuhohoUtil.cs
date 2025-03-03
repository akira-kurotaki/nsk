using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using ModelLibrary.Models;
using NLog;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 入力方法ユーティリティ
    /// </summary>
    /// <remarks>
    /// 作成日：2020/12/09
    /// 作成者：Cho
    /// </remarks>
    public static class NyuryokuhohoUtil
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 入力方法PDFファイルパス取得するメソッド
        /// </summary>
        /// <param name="screenId">画面ID</param>
        /// <param name="tekiyoYmd">適用年月日</param>
        /// <returns>入力方法PDFファイルパス</returns>
        public static FileStream GetNyuryokuhohoPDF(string screenId, DateTime tekiyoYmd)
        {
            try
            {
                MNyuryokuhohoPdfCache mNyuryokuhohoPdfCache = new MNyuryokuhohoPdfCache(CacheManager.GetInstance());
                IEnumerable<MNyuryokuhohoPdf> nyuryokuhohoPdf = CacheUtil.Get<IEnumerable<MNyuryokuhohoPdf>>(CacheManager.GetInstance()
                                            , CoreConst.M_NYURYOKUHOHO_PDF_CACHE, () => (IEnumerable<MNyuryokuhohoPdf>)mNyuryokuhohoPdfCache.GetList())
                                            .Where(a => a.ScreenId == screenId && a.TekiyoStartYmd <= tekiyoYmd);

                List<MNyuryokuhohoPdf> nyuryokuhohoPdfs = new List<MNyuryokuhohoPdf>();
                foreach (var item in nyuryokuhohoPdf)
                {
                    if((item.TekiyoEndYmd != null && item.TekiyoEndYmd >= tekiyoYmd) || 
                       (item.TekiyoEndYmd == null && DateTime.Parse("2099/12/31") >= tekiyoYmd))
                    {
                        nyuryokuhohoPdfs.Add(item);
                    }
                    
                }

                // 例外処理
                if (nyuryokuhohoPdfs.Count() == 0)
                {
                    logger.Error(SystemMessageUtil.Get("ME01438"));
                    return null;
                }

                if (nyuryokuhohoPdfs.Count() > 1)
                {
                    logger.Error(SystemMessageUtil.Get("ME01439"));
                    return null;
                }

                if (File.Exists(nyuryokuhohoPdfs.ElementAtOrDefault(0).PdfFilePath))
                {
                    return File.OpenRead(nyuryokuhohoPdfs.ElementAtOrDefault(0).PdfFilePath);
                }
                else
                {
                    logger.Error(SystemMessageUtil.Get("ME01440"));
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 入力方法PDFマスタのリフレッシュメソッド。
        /// </summary>
        /// <returns>なし</returns>
        public static void Refresh()
        {
            MNyuryokuhohoPdfCache mNyuryokuhohoPdfCache = new MNyuryokuhohoPdfCache(CacheManager.GetInstance());
            CacheUtil.Refresh<IEnumerable<MNyuryokuhohoPdf>>(CacheManager.GetInstance(), CoreConst.M_NYURYOKUHOHO_PDF_CACHE, () => (IEnumerable<MNyuryokuhohoPdf>)mNyuryokuhohoPdfCache.GetList());
        }
    }
}