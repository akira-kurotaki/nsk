namespace NskReportMain.Common
{
    /// <summary>
    /// ReportBase の概要の説明です。
    /// </summary>
    partial class ReportBase
    {
        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }

        #region ActiveReport Designer generated code
        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ReportBase));
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // ReportBase
            // 
            this.MasterReport = false;
            this.PageSettings.PaperHeight = 11F;
            this.PageSettings.PaperWidth = 8.5F;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
    }
}
