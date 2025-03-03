using NskReportMain.Models.P0041;
using NskReportMain.Reports.P0041;
using CoreLibrary.Core.Utility;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;

namespace NskReportMain.ReportCreators.P0041
{
    /// <summary>
    /// 帳票P0041作成クラス
    /// </summary>
    /// <remarks>
    /// 作成日：2023/05/06
    /// 作成者：KAN RI
    /// </remarks>
    public class P0041Creator : ReportCreator
    {
        #region 帳票作成メソッド
        /// <summary>
        /// 帳票作成メソッド
        /// </summary>
        /// <param name="joukenId">条件ID</param>
        /// <param name="dataList">出力対象データ</param>
        /// <param name="printPattern">印字パターン</param>
        /// <returns>実行結果</returns>
        public CreatorResult CreateReport(int joukenId, List<P0041Model> dataList, string printPattern)
        {
            logger.Info(string.Format(
                ReportConst.METHOD_BEGIN_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P0041_加入申請書※R6年度以降用",
                string.Empty));

            // 実行結果
            CreatorResult result = new CreatorResult();

            // 引数チェックする
            // 出力対象データがないの場合、エラーとし、エラーメッセージを返す
            if (dataList == null || dataList.Count == 0)
            {
                result.Result = ReportConst.RESULT_FAILED;
                result.ErrorMessageId = "ME01054";
                result.ErrorMessage = MessageUtil.Get("ME01054", ReportConst.PARAM_NAME_OUTPUT_DATA);
                return result;
            }

            // 印字パターンがないの場合、エラーとし、エラーメッセージを返す
            if (string.IsNullOrEmpty(printPattern))
            {
                result.Result = ReportConst.RESULT_FAILED;
                result.ErrorMessageId = "ME01054";
                result.ErrorMessage = MessageUtil.Get("ME01054", ReportConst.PARAM_NAME_PRINT_PATTERN);
                return result;
            }

            // 帳票を作成する
            report = new P0041Report(printPattern)
            {
                DataSource = dataList
            };
            report.Run();

            logger.Info(string.Format(
                ReportConst.METHOD_END_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P0041_加入申請書※R6年度以降用"));

            // 作成された帳票を返却する
            result.Result = ReportConst.RESULT_SUCCESS;
            result.SectionReport = report;
            return result;
        }
#endregion
    }
}
