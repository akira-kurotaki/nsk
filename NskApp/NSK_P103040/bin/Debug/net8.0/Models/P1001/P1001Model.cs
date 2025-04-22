namespace NskReportMain.Models.P1001
{
    /// <summary>
    /// P1001の帳票用モデル
    /// </summary>
    /// <remarks>
    /// 作成日：2024/10/18
    /// 作成者：YOU EN
    /// </remarks>
    public class P1001Model
    {
        /// <summary>
        /// 出力年月日
        /// </summary>
        public string OutputYMD { get; set; }

        /// <summary>
        /// 出力時分
        /// </summary>
        public string OutputHM { get; set; }

        /// <summary>
        /// P1001の帳票条件用モデル
        /// </summary>
        public P1001SearchCondition P1001SearchCondition { get; set; }

        /// <summary>
        /// P1001の帳票明細用モデルリスト
        /// </summary>
        public List<P1001TableRecord> P1001TableRecordList { get; set; }
    }
}
