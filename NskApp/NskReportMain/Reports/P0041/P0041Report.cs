using NskReportMain.Common;

namespace NskReportMain.Reports.P0041
{
    /// <summary>
    /// 帳票P0041本体
    /// </summary>
    /// <remarks>
    /// 作成日：2018/03/05
    /// 作成者：KAN RI
    /// </remarks>
    public partial class P0041Report : ReportBase
    {
        /// <summary>
        /// コントラクター
        /// </summary>
        public P0041Report()
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();
        }

        /// <summary>
        /// コントラクター
        /// </summary>
        /// <param name="printPattern">印字パターン</param>
        public P0041Report(string printPattern)
        {
            //
            // デザイナー サポートに必要なメソッドです。
            //
            InitializeComponent();

            // 登録確認・写し用の場合、加入者控えを印字する
            //if (printPattern == ReportConst.PRINT_PATTERN_REGIST_COPY)
            //{
            //    txtNote.Visible = true;
            //}
            //else
            //{
            //    txtNote.Visible = false;
            //}
            txtNote.Visible = false;
        }
    }
}