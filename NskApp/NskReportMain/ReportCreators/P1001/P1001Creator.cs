using NskReportMain.Models.P1001;
using NskReportMain.Reports;
using CoreLibrary.Core.Utility;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.SectionReportModel;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;
using System.Xml;

namespace NskReportMain.ReportCreators.P1001
{
    /// <summary>
    /// P1001_加入者情報（一覧）
    /// </summary>
    /// <remarks>
    /// 作成日：2024/10/18
    /// 作成者：YOU EN
    /// </remarks>
    public class P1001Creator : ReportCreator
    {
        #region P1001_加入者情報（一覧）レポート出力処理
        /// <summary>
        /// P1001_加入者情報（一覧）
        /// </summary>
        /// <param name="joukenId">条件ID</param>
        /// <param name="model">出力対象データ</param>
        /// <returns>実行結果</returns>
        public CreatorResult CreateReport(int joukenId, P1001Model model)
        {
            logger.Info(string.Format(
                ReportConst.METHOD_BEGIN_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P1001_加入者情報（一覧）",
                string.Empty));

            // 実行結果
            CreatorResult result = new CreatorResult();

            // 引数チェックする
            // 出力年月日がないの場合、エラーとし、エラーメッセージを返す
            if (string.IsNullOrEmpty(model.OutputYMD))
            {
                return result.CreateResultError("ME90009", ReportConst.PARAM_NAME_OUTPUT_YMD);
            }

            // 出力時分がないの場合、エラーとし、エラーメッセージを返す
            if (string.IsNullOrEmpty(model.OutputHM))
            {
                return result.CreateResultError("ME90009", ReportConst.PARAM_NAME_OUTPUT_HM);
            }

            // 出力条件がないの場合、エラーとし、エラーメッセージを返す
            if (model.P1001SearchCondition == null)
            {
                return result.CreateResultError("ME90009", ReportConst.PARAM_NAME_OUTPUT_JOUKEN);
            }

            // 出力対象データがないの場合、エラーとし、エラーメッセージを返す
            if (model.P1001TableRecordList == null || model.P1001TableRecordList.Count == 0)
            {
                return result.CreateResultError("ME90009", ReportConst.PARAM_NAME_OUTPUT_DATA);
            }

            // 帳票を作成する
            report = new BaseSectionReport(@"Reports\P1001\P1001Report.rpx");
            // データ設定を行う
            SetData(model, ref report);
            // 帳票を呼び出す
            report.Run();

            logger.Info(string.Format(
                ReportConst.METHOD_END_LOG,
                ReportConst.CLASS_NM_CREATOR,
                joukenId,
                "P1001_加入者情報（一覧）"));

            // 作成された帳票を返却する
            result.Result = ReportConst.RESULT_SUCCESS;
            result.SectionReport = report;
            return result;
        }
        #endregion

        #region P1001_加入者情報（一覧）レポートデータ設定処理
        /// <summary>
        /// セクションレポートのコンポーネントに値を設定する
        /// </summary>
        /// <param name="model"></param>
        /// <param name="rpt"></param>
        private void SetData(P1001Model model, ref SectionReport rpt)
        {
            // 出力年月日
            ((TextBox)rpt.Sections["PageHeader"].Controls["txtOutputYMD"]).Text = "出力年月日時分：" + model.OutputYMD + model.OutputHM;

            // 検索条件部の値を設定する
            // 都道府県
            if (!string.IsNullOrEmpty(model.P1001SearchCondition.JoukenTodofukenCd))
            {
                if (!string.IsNullOrEmpty(model.P1001SearchCondition.JoukenTodofukenNm))
                {
                    ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenTodofuken"]).Text = model.P1001SearchCondition.JoukenTodofukenCd + "(" + model.P1001SearchCondition.JoukenTodofukenNm + ")";
                }
                else
                {
                    ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenTodofuken"]).Text = model.P1001SearchCondition.JoukenTodofukenCd;
                }
            }

            // 組合等
            if (!string.IsNullOrEmpty(model.P1001SearchCondition.JoukenKumiaitoCd))
            {
                if (!string.IsNullOrEmpty(model.P1001SearchCondition.JoukenKumiaitoNm))
                {
                    ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenKumiaito"]).Text = model.P1001SearchCondition.JoukenKumiaitoCd + "(" + model.P1001SearchCondition.JoukenKumiaitoNm + ")";
                }
                else
                {
                    ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenKumiaito"]).Text = model.P1001SearchCondition.JoukenKumiaitoCd;
                }
            }

            // 支所
            if (!string.IsNullOrEmpty(model.P1001SearchCondition.JoukenShishoCd))
            {
                if (!string.IsNullOrEmpty(model.P1001SearchCondition.JoukenShishoNm))
                {
                    ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenShisho"]).Text = model.P1001SearchCondition.JoukenShishoCd + "(" + model.P1001SearchCondition.JoukenShishoNm + ")";
                }
                else
                {
                    ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenShisho"]).Text = model.P1001SearchCondition.JoukenShishoCd;
                }
            }

            // 市町村
            if (!string.IsNullOrEmpty(model.P1001SearchCondition.JoukenShichosonCd))
            {
                if (!string.IsNullOrEmpty(model.P1001SearchCondition.JoukenShichosonNm))
                {
                    ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenShichouson"]).Text = model.P1001SearchCondition.JoukenShichosonCd + "(" + model.P1001SearchCondition.JoukenShichosonNm + ")";
                }
                else
                {
                    ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenShichouson"]).Text = model.P1001SearchCondition.JoukenShichosonCd;
                }
            }

            // 大地区
            if (!string.IsNullOrEmpty(model.P1001SearchCondition.JoukenDaichikuCd))
            {
                if (!string.IsNullOrEmpty(model.P1001SearchCondition.JoukenDaichikuNm))
                {
                    ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenDaichiku"]).Text = model.P1001SearchCondition.JoukenDaichikuCd + "(" + model.P1001SearchCondition.JoukenDaichikuNm + ")";
                }
                else
                {
                    ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenDaichiku"]).Text = model.P1001SearchCondition.JoukenDaichikuCd;
                }
            }

            // 小地区（開始）
            if (!string.IsNullOrEmpty(model.P1001SearchCondition.JoukenShochikuCdStart))
            {
                if (!string.IsNullOrEmpty(model.P1001SearchCondition.JoukenShochikuNmStart))
                {
                    ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenShochikuStart"]).Text = model.P1001SearchCondition.JoukenShochikuCdStart + "(" + model.P1001SearchCondition.JoukenShochikuNmStart + ")";
                }
                else
                {
                    ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenShochikuStart"]).Text = model.P1001SearchCondition.JoukenShochikuCdStart;
                }
            }
            // 小地区（終了）
            if (!string.IsNullOrEmpty(model.P1001SearchCondition.JoukenShochikuCdEnd))
            {
                if (!string.IsNullOrEmpty(model.P1001SearchCondition.JoukenShochikuNmEnd))
                {
                    ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenShochikuEnd"]).Text = model.P1001SearchCondition.JoukenShochikuCdEnd + "(" + model.P1001SearchCondition.JoukenShochikuNmEnd + ")";
                }
                else
                {
                    ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenShochikuEnd"]).Text = model.P1001SearchCondition.JoukenShochikuCdEnd;
                }
            }
            // 加入者管理コード（開始）
            ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenKanyuShaKanriCdStart"]).Text = model.P1001SearchCondition.JoukenKanyushaCdStart;
            // 加入者管理コード（終了）
            ((TextBox)rpt.Sections["GroupHeader1"].Controls["txtJoukenKanyuShaKanriCdEnd"]).Text = model.P1001SearchCondition.JoukenKanyushaCdEnd;

            // 明細
            rpt.DataSource = model.P1001TableRecordList;
        }
        #endregion
    }
}
