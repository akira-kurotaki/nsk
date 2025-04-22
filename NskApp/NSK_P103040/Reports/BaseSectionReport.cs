using System.Xml;
namespace NSK_P103040.Reports
{
    /// <summary>
    /// レポートの起動設定クラス
    /// </summary>
    public class BaseSectionReport : GrapeCity.ActiveReports.SectionReport
    {
        /// <summary>
        /// デフォルトコントローラー
        /// </summary>
        public BaseSectionReport()
        {
            InitializeReport();
        }

        /// <summary>
        /// レポートファイルを読み込んで、レポートを初期設定する
        /// </summary>
        /// <param name="reportFilePath">レポートファイルパス</param>
        public BaseSectionReport(string reportFilePath)
        {
            LoadReportLayout(reportFilePath);
            InitializeReport();
        }

        /// <summary>
        /// レポートを初期設定する
        /// </summary>
        private void InitializeReport()
        {
            ReportStart += BaseSectionReport_ReportStart;
        }

        /// <summary>
        /// レポートファイルを読み込む
        /// </summary>
        /// <param name="reportFilePath">レポートファイルパス</param>
        private void LoadReportLayout(string reportFilePath)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(baseDirectory, reportFilePath);

            using (var reader = new XmlTextReader(fullPath))
            {
                this.LoadLayout(reader);
            }
        }

        /// <summary>
        /// ReportStartイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BaseSectionReport_ReportStart(object sender, EventArgs e)
        {
            // 帳票デザインに影響（プレビューの赤線が出る）しないように、一旦コメントアウトする
            // 仮想プリンタを設定する（帳票出力の高速化のため）
            Document.Printer.PrinterName = string.Empty;
        }
    }
}
