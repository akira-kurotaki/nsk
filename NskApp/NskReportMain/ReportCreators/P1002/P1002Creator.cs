using NskReportMain.Models.P1002;
using NskReportMain.Reports;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;

namespace NskReportMain.ReportCreators.P1002
{
    /// <summary>
    /// P1002_加入者情報（単票）
    /// </summary>
    /// <remarks>
    /// 作成日：2024/10/23
    /// 作成者：YOU EN
    /// </remarks>
    public class P1002Creator : ReportCreator
    {
        #region P1002_加入者情報（単票）
        /// <summary>
        /// P1002_加入者情報（単票）
        /// </summary>
        /// <param name="joukenId">条件ID</param>
        /// <param name="model">出力対象データ</param>
        /// <returns>実行結果</returns>
        public CreatorResult CreateReport(int joukenId, List<P1002Model> model)
        {
            logger.Info(string.Format(
                ReportConst.METHOD_BEGIN_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P1002_加入者情報（単票）",
                string.Empty));

            // 実行結果
            CreatorResult result = new CreatorResult();

            // 出力対象データがないの場合、エラーとし、エラーメッセージを返す
            if (model == null || model.Count == 0)
            {
                return result.CreateResultError("ME90009", ReportConst.PARAM_NAME_OUTPUT_DATA);
            }

            // 帳票を作成する
            report = new BaseSectionReport(@"Reports\P1002\P1002Report.rpx");
            // 帳票のデータ設定
            report.DataSource = model;
            // 帳票を呼び出す
            report.Run();

            logger.Info(string.Format(
                ReportConst.METHOD_END_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P1002_加入者情報（単票）"));

            // 作成された帳票を返却する
            result.Result = ReportConst.RESULT_SUCCESS;
            result.SectionReport = report;
            return result;
        }
        #endregion
    }
}
