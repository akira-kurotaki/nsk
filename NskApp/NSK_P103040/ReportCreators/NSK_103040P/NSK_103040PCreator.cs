using CoreLibrary.Core.Utility;
using DocumentFormat.OpenXml.Presentation;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Document.Section;
using GrapeCity.ActiveReports.SectionReportModel;
using NSK_P103040.Models.P103040;
using NskReportLibrary.Core.Base;
using NskReportLibrary.Core.Consts;
using NskReportMain.Common;
using NskReportMain.Reports;
using System.Drawing;
using System.Globalization;

namespace NSK_P103040.ReportCreators.P103040
{
    /// <summary>
    /// P103040_基準統計単収一覧
    /// </summary>
    /// <remarks>
    /// 作成日：2025/3/6
    /// 作成者：NEXT
    /// </remarks>
    public class NSK_103040PCreator : ReportCreator
    {
        /// <summary>
        /// 「P103040_危険段階一覧表」を作成するメソッド
        /// </summary>
        /// <param name="outputDatas">出力データ</param>
        /// <returns>実行結果</returns>
        public static CreatorResult CreateReport(NSK_103040PModel outputDatas, BatchJouken jouken)
        {
            // 変数
            // Creatorの実行結果
            CreatorResult creatorResult = new()
            {
                Result = ReportConst.RESULT_FAILED,
                SectionReport = null
            };

            // １．引数チェックする。
            IsRequired(outputDatas, jouken, ref creatorResult);

            // 帳票を作成する。
            // 帳票オブジェクト
            BaseSectionReport tyouhyouInstance = new(@"Reports\P103040\P103040Report.rpx");

            // 帳票のデータ設定
            SetDatas(outputDatas, jouken, ref tyouhyouInstance);

            // Run()を呼び、設定したデータで帳票を作成する。
            tyouhyouInstance.Run();

            // ResultにReportConst.RESULT_SUCCESS（成功：0）を設定する。
            creatorResult.Result = ReportConst.RESULT_SUCCESS;

            // SectionReportに帳票インスタンスを設定する。
            creatorResult.SectionReport = tyouhyouInstance;

            // 実行結果を返す。
            return creatorResult;
        }


        #region 引数のチェック
        /// <summary>
        /// 引数のチェック
        /// </summary>
        /// <param name="jouken">出力条件</param>
        /// <param name="outputDatas">出力対象データ</param>
        private static void IsRequired(NSK_103040PModel outputDatas, BatchJouken jouken, ref CreatorResult createResult)
        {
            // 出力対象データがnullまたはデータの件数が0件の場合、
            if (outputDatas.headerModel == null || outputDatas.tableRecords.Count == 0 || jouken == null)
            {
                // エラーとし、エラーメッセージを返却、処理を終了する。
                // （"ME01054"、引数{0} ： "出力対象データ")
                createResult.CreateResultError("ME01054", string.Empty);
            }
        }
        #endregion

        #region  帳票のデータ設定
        private static void SetDatas(NSK_103040PModel data, BatchJouken jouken, ref BaseSectionReport tyouhyouInstance)
        {
            string nendo = NendoUtil.GetNendoDisp2(jouken.joukenNensan);
            string convertNendo = nendo.Substring(0, nendo.Length - 1) + "産";

            CultureInfo ci = new CultureInfo("ja-JP", false);
            ci.DateTimeFormat.Calendar = new JapaneseCalendar();
            string wareki = DateTime.Now.ToString("ggyy年 MM月 dd日", ci);

            // 帳票をA4縦向きに設定
            tyouhyouInstance.PageSettings.PaperKind = GrapeCity.ActiveReports.Printing.PaperKind.A4;
            tyouhyouInstance.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Portrait;

            // 年産（令和〇〇年産）
            ((Label)tyouhyouInstance.Sections["PageHeader"].Controls["nensan"]).Text = convertNendo;

            // 共済目的
            ((Label)tyouhyouInstance.Sections["PageHeader"].Controls["kyosaimokuteki"]).Text = data.headerModel.KyosaiMokutekiNm;

            // 類区分
            ((Label)tyouhyouInstance.Sections["PageHeader"].Controls["ruikbn"]).Text = jouken.joukenRuikbn;
            ((Label)tyouhyouInstance.Sections["PageHeader"].Controls["ruikbnName"]).Text = data.headerModel.ruiKbnNm;

            // 出力年月日
            // "[年号]yy年　MM月　dd日"
            ((TextBox)tyouhyouInstance.Sections["PageHeader"].Controls["outputData"]).Text = wareki;
           

            // 詳細データ
            tyouhyouInstance.DataSource = data.tableRecords;
        }
        #endregion
    }
}
