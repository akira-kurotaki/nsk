using NskReportMain.Models.P102090;
using NskReportMain.Reports;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.SectionReportModel;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;
using Microsoft.IdentityModel.Tokens;

namespace NskReportMain.ReportCreators.P102090
{
    /// <summary>
    /// P102090_危険段階地域区分一覧表
    /// </summary>
    /// <remarks>
    /// 作成日：2025/3/6
    /// 作成者：NEXT
    /// </remarks>
    public class P102090Creator : ReportCreator
    {
        #region P102090_危険段階地域区分一覧表 レポート出力処理
        /// <summary>
        /// P102090_危険段階地域区分一覧表
        /// </summary>
        /// <param name="joukenId">条件ID</param>
        /// <param name="model">出力対象データ</param>
        /// <returns>実行結果</returns>
        public CreatorResult CreateReport(string joukenId, P102090Model model)
        {
            logger.Info(string.Format(
                ReportConst.METHOD_BEGIN_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P102090_危険段階地域区分一覧表",
                string.Empty));

            // 実行結果
            CreatorResult result = new CreatorResult();

            // 出力対象データがない場合、エラーメッセージを返す
            if (model.P102090TableRecords.Count == 0)
            {
                // エラーメッセージ：「処理件数が0件のため処理を終了します」
                return result.CreateResultError(string.Empty, "ME10076", "0");
            }

            // 帳票を作成する
            report = new BaseSectionReport(@"Reports\P102090\P102090Report.rpx");
            // データ設定を行う
            SetData(model, ref report);
            // 帳票を呼び出す
            report.Run();

            logger.Info(string.Format(
                ReportConst.METHOD_END_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P102090_危険段階地域区分一覧表"));

            // 作成された帳票を返却する
            result.Result = ReportConst.RESULT_SUCCESS;
            result.SectionReport = report;
            return result;
        }
        #endregion

        #region P102090_危険段階地域区分一覧表レポートデータ設定処理
        /// <summary>
        /// セクションレポートのコンポーネントに値を設定する
        /// </summary>
        /// <param name="model"></param>
        /// <param name="rpt"></param>
        private void SetData(P102090Model model, ref SectionReport rpt)
        {
            // ヘッダー年産
            ((TextBox)rpt.Sections["PageHeader"].Controls["kyousaimokuteki"]).Text = model.P102090TableRecords[0].kyousaimokuteki;

            // ヘッダー共済目的
            ((TextBox)rpt.Sections["PageHeader"].Controls["ruikbn"]).Text = model.P102090TableRecords[0].ruikbn;

            // ヘッダー出力日
            ((TextBox)rpt.Sections["PageHeader"].Controls["ruiName"]).Text = model.P102090TableRecords[0].ruiName;

            // 明細
            rpt.DataSource = model.P102090TableRecords;
        }
        #endregion
    }
}
