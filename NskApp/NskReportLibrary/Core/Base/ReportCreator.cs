using GrapeCity.ActiveReports;
using NLog;

namespace NskReportLibrary.Core.Base
{
    /// <summary>
    /// Creatorベースクラス
    /// 
    /// 作成日：2017/12/11
    /// 作成者：KAN RI
    /// </summary>
    public class ReportCreator
    {
        #region クラス変数
        /// <summary>
        /// ロガー
        /// </summary>
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 帳票インスタンス
        /// </summary>
        protected SectionReport report = null;
        #endregion

        #region 帳票インスタンスのメモリを解放するメソッド
        /// <summary>
        /// 帳票インスタンスのメモリを解放するメソッド
        /// </summary>
        public void DisposeReportMemory()
        {
            // レポートインスタンスをDisposeする
            if (report != null)
            {
                report.Document.Dispose();
                report.Dispose();
                report = null;
            }
        }
        #endregion
    }
}
