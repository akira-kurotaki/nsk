using CoreLibrary.Core.Utility;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Document.Section;
using NskReportLibrary.Core.Consts;
using System.Drawing;

namespace NskReportLibrary.Core.Utility
{
    /// <summary>
    /// 帳票ページャのユーティリティクラス
    /// 
    /// 作成日：2017/12/12
    /// 作成者：KAN RI
    /// </summary>
    public static class ReportPagerUtil
    {
        /// <summary>
        /// レポートのページ番号を描画する
        /// </summary>
        /// <param name="report">レポート本体</param>
        /// <param name="reportBottomMargin">帳票下部の余白</param>
        /// <returns>レポート本体</returns>
        public static SectionReport DrawReportPageNumber(SectionReport report, float reportBottomMargin = ReportConst.REPORT_BOTTOM_MARGIN_STANDARD)
        {
            // 全体的に、レポートの下部にページ番号を描画する
            for (var i = 0; i < report.Document.Pages.Count; i++)
            {
                //report.Document.Pages[i].Font = new Font(ReportConst.REPORT_PAGE_NUM_FONT_TYPE, ReportConst.REPORT_PAGE_NUM_FONT_SIZE);
                report.Document.Pages[i].Font = new GrapeCity.ActiveReports.Document.Drawing.Font(ReportConst.REPORT_PAGE_NUM_FONT_TYPE, ReportConst.REPORT_PAGE_NUM_FONT_SIZE);
                report.Document.Pages[i].ForeColor = Color.Black;
                report.Document.Pages[i].BackColor = Color.Transparent;
                report.Document.Pages[i].VerticalTextAlignment = VerticalTextAlignment.Middle;
                report.Document.Pages[i].TextAlignment = TextAlignment.Center;

                // ページ番号をページの上端の座標
                float pageNumberTopLocation = report.Document.Pages[i].Height - reportBottomMargin;

                // ページ番号を描画する
                report.Document.Pages[i].DrawText(
                    (i + 1).ToString() + " / " + report.Document.Pages.Count.ToString(),
                    0,                                   // テキストの左端の座標
                    pageNumberTopLocation,               // テキストの上端の座標
                    report.Document.Pages[i].Width,      // テキスト領域の幅
                    ReportConst.TEXT_AREA_HEIGHT_FIVE);  // テキスト領域の高さ
            }

            return report;
        }

        /// <summary>
        /// 印刷業者一意コードを描画する
        /// </summary>
        /// <param name="report">レポート本体</param>
        /// <param name="printText">描画文字</param>
        /// <returns>レポート本体</returns>
        //public static SectionReport DrawReportPrimaryKeyCode(SectionReport report, string printText)
        //{
        //    // 全体的に、レポートの上部に印刷業者一意コードを描画する
        //    for (var i = 0; i < report.Document.Pages.Count; i++)
        //    {
        //        //report.Document.Pages[i].Font = new Font(ConfigUtil.Get(ReportConst.REPORT_PRIMARY_KEY_FONT_TYPE_TAG_NAME), ReportConst.REPORT_PRIMARY_KEY_FONT_SIZE);
        //        report.Document.Pages[i].Font = new GrapeCity.ActiveReports.Document.Drawing.Font(ConfigUtil.Get(ReportConst.REPORT_PRIMARY_KEY_FONT_TYPE_TAG_NAME), ReportConst.REPORT_PRIMARY_KEY_FONT_SIZE);
        //        report.Document.Pages[i].ForeColor = Color.Black;
        //        report.Document.Pages[i].BackColor = Color.Transparent;
        //        report.Document.Pages[i].VerticalTextAlignment = VerticalTextAlignment.Middle;
        //        report.Document.Pages[i].TextAlignment = TextAlignment.Right;

        //        // 印刷業者一意コードを描画するページ右端の座標
        //        float primaryKeyCodeWidthLocation = report.Document.Pages[i].Width - ReportConst.REPORT_RIGHT_MARGIN_FIVE;

        //        // 印刷業者一意コードを描画する
        //        report.Document.Pages[i].DrawText(
        //            printText,
        //            0,                                   // テキストの左端の座標
        //            ReportConst.REPORT_TOP_MARGIN_FIVE,  // テキストの上端の座標
        //            primaryKeyCodeWidthLocation,         // テキスト領域の幅
        //            ReportConst.TEXT_AREA_HEIGHT_FIVE);  // テキスト領域の高さ
        //    }

        //    return report;
        //}
    }
}