using NskReportMain.Models.P103040;
using NskReportMain.Reports;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.SectionReportModel;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;
using Microsoft.IdentityModel.Tokens;

namespace NskReportMain.ReportCreators.P103040
{
    /// <summary>
    /// P103040_基準統計単収一覧
    /// </summary>
    /// <remarks>
    /// 作成日：2025/3/6
    /// 作成者：NEXT
    /// </remarks>
    public class P103040Creator : ReportCreator
    {
        #region P103040_基準統計単収一覧 レポート出力処理
        /// <summary>
        /// P103040_基準統計単収一覧
        /// </summary>
        /// <param name="joukenId">条件ID</param>
        /// <param name="model">出力対象データ</param>
        /// <returns>実行結果</returns>
        public CreatorResult CreateReport(string joukenId, P103040Model model)
        {
            logger.Info(string.Format(
                ReportConst.METHOD_BEGIN_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P103040_基準統計単収一覧",
                string.Empty));

            // 実行結果
            CreatorResult result = new CreatorResult();

            // ヘッダー情報がすべてそろっていなければエラー
            if (model.P103040HeaderModel.年産.IsNullOrEmpty() || model.P103040HeaderModel.共済目的名称.IsNullOrEmpty() ||
                    model.P103040HeaderModel.類区分.IsNullOrEmpty() || model.P103040HeaderModel.類名称.IsNullOrEmpty())
            {
                // エラーメッセージ：「ヘッダー情報の取得に失敗しました。」
                return result.CreateResultError(string.Empty, "ME01645", "ヘッダー情報の取得");
            }

            // 出力対象データがない場合、エラーメッセージを返す
            if (model.P103040TableRecords.Count == 0)
            {
                // エラーメッセージ：「処理件数が0件のため処理を終了します」
                return result.CreateResultError(string.Empty, "ME10076", "0");
            }

            // 帳票を作成する
            report = new BaseSectionReport(@"Reports\P103040\P103040Report.rpx");
            // データ設定を行う
            SetData(model, ref report);
            // 帳票を呼び出す
            report.Run();

            logger.Info(string.Format(
                ReportConst.METHOD_END_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P103040_基準統計単収一覧"));

            // 作成された帳票を返却する
            result.Result = ReportConst.RESULT_SUCCESS;
            result.SectionReport = report;
            return result;
        }
        #endregion

        #region P103040_基準統計単収一覧レポートデータ設定処理
        /// <summary>
        /// セクションレポートのコンポーネントに値を設定する
        /// </summary>
        /// <param name="model"></param>
        /// <param name="rpt"></param>
        private void SetData(P103040Model model, ref SectionReport rpt)
        {
            // ヘッダー年産
            ((TextBox)rpt.Sections["PageHeader"].Controls["nensan"]).Text = model.P103040HeaderModel.年産 + "産";

            // ヘッダー共済目的
            ((TextBox)rpt.Sections["PageHeader"].Controls["kyosaimokuteki"]).Text = model.P103040HeaderModel.共済目的名称;

            // ヘッダー出力日
            ((TextBox)rpt.Sections["PageHeader"].Controls["outputDate"]).Text = model.dateTimeToString.ToString("yyyy年MM月HH日");

            // ヘッダー類区分 / 類名称
            if (!model.P103040HeaderModel.類区分.IsNullOrEmpty() && !model.P103040HeaderModel.類名称.IsNullOrEmpty())
            {
                ((TextBox)rpt.Sections["PageHeader"].Controls["ruikgn"]).Text = model.P103040HeaderModel.類区分;
                ((TextBox)rpt.Sections["PageHeader"].Controls["ruiName"]).Text = model.P103040HeaderModel.類名称;
            }
            
            // 明細
            rpt.DataSource = model.P103040TableRecords;
        }
        #endregion
    }
}
