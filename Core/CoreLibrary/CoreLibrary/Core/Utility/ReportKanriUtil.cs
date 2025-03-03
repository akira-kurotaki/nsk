using CoreLibrary.Core.Cache;
using CoreLibrary.Core.Consts;
using ModelLibrary.Models;

namespace CoreLibrary.Core.Utility
{
    /// <summary>
    /// 帳票処理管理のユーティリティクラス
    /// </summary>
    /// <remarks>
    /// 作成日：2018/1/29
    /// 作成者：Rou I
    /// </remarks>
    public class ReportKanriUtil
    {
        /// <summary>
        /// 帳票処理管理情報の取得メソッド。
        /// </summary>
        /// <returns>帳票処理管理リスト</returns>
        public static IEnumerable<MReportKanri> GetReportKanriList()
        {
            MReportKanriCache mReportKanriCache = new MReportKanriCache(CacheManager.GetInstance());
            return CacheUtil.Get<IEnumerable<MReportKanri>>(CacheManager.GetInstance(), CoreConst.M_REPORT_KANRI_CACHE, () => (IEnumerable<MReportKanri>)mReportKanriCache.GetList());
        }

        /// <summary>
        /// 帳票処理管理情報の取得メソッド。
        /// </summary>
        /// <param name="reportControlId">帳票制御ID</param>
        /// <returns>帳票処理管理リスト</returns>
        public static IEnumerable<MReportKanri> GetReportKanriList(string reportControlId)
        {
            if (string.IsNullOrEmpty(reportControlId))
            {
                return new List<MReportKanri>();
            }

            return GetReportKanriList().Where(t => t.ReportControlId == reportControlId);
        }

        /// <summary>
        /// 帳票処理管理情報の取得メソッド。
        /// </summary>
        /// <param name="reportControlId">帳票制御ID</param>
        /// <param name="serialNumber">連番</param>
        /// <returns>帳票処理管理情報</returns>
        public static MReportKanri GetReportKanri(string reportControlId, int serialNumber)
        {
            if (string.IsNullOrEmpty(reportControlId))
            {
                return new MReportKanri();
            }

            return GetReportKanriList().Where(t => t.ReportControlId == reportControlId &&
                                              t.SerialNumber == serialNumber).FirstOrDefault();
        }

        /// <summary>
        /// バッチ処理対象件数の取得メソッド。
        /// </summary>
        /// <param name="reportControlId">帳票制御ID</param>
        /// <param name="serialNumber">連番</param>
        /// <returns>バッチ処理対象件数</returns>
        public static short GetBatchShoriKensu(string reportControlId, int serialNumber)
        {
            if (string.IsNullOrEmpty(reportControlId))
            {
                return 0;
            }
            MReportKanri mReportKanri = GetReportKanriList().Where(t => t.ReportControlId == reportControlId &&
                                              t.SerialNumber == serialNumber).FirstOrDefault();
            return mReportKanri == null ? (short)0 : mReportKanri.BatchShoriKensu;
        }

        /// <summary>
        /// 帳票制御名の取得メソッド。
        /// </summary>
        /// <param name="reportControlId">帳票制御ID</param>
        /// <param name="serialNumber">連番</param>
        /// <returns>帳票制御名</returns>
        public static string GetReportControlNm(string reportControlId, int serialNumber)
        {
            if (string.IsNullOrEmpty(reportControlId))
            {
                return string.Empty;
            }
            MReportKanri mReportKanri = GetReportKanriList().Where(t => t.ReportControlId == reportControlId &&
                                              t.SerialNumber == serialNumber).FirstOrDefault();
            return mReportKanri == null ? string.Empty : mReportKanri.ReportControlNm;
        }

        /// <summary>
        /// ファイル名の取得メソッド。
        /// </summary>
        /// <param name="reportControlId">帳票制御ID</param>
        /// <param name="serialNumber">連番</param>
        /// <returns>ファイル名</returns>
        public static string GetFileNm(string reportControlId, int serialNumber)
        {
            if (string.IsNullOrEmpty(reportControlId))
            {
                return string.Empty;
            }
            MReportKanri mReportKanri = GetReportKanriList().Where(t => t.ReportControlId == reportControlId &&
                                              t.SerialNumber == serialNumber).FirstOrDefault();
            return mReportKanri == null ? string.Empty : mReportKanri.FileNm;
        }

        /// <summary>
        /// 予約処理名の取得メソッド。
        /// </summary>
        /// <param name="reportControlId">帳票制御ID</param>
        /// <param name="serialNumber">連番</param>
        /// <returns>予約処理名</returns>
        public static string GetYoyakuNm(string reportControlId, int serialNumber)
        {
            if (string.IsNullOrEmpty(reportControlId))
            {
                return string.Empty;
            }
            MReportKanri mReportKanri = GetReportKanriList().Where(t => t.ReportControlId == reportControlId &&
                                              t.SerialNumber == serialNumber).FirstOrDefault();
            return mReportKanri == null ? string.Empty : mReportKanri.YoyakuNm;
        }

        /// <summary>
        /// 帳票処理管理マスタのリフレッシュメソッド。
        /// </summary>
        /// <returns>なし</returns>
        public static void Refresh()
        {
            MReportKanriCache mReportKanriCache = new MReportKanriCache(CacheManager.GetInstance());
            CacheUtil.Refresh<IEnumerable<MReportKanri>>(CacheManager.GetInstance(), CoreConst.M_REPORT_KANRI_CACHE, () => (IEnumerable<MReportKanri>)mReportKanriCache.GetList());
        }
    }
}