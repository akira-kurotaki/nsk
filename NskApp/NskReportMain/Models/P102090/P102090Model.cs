namespace NskReportMain.Models.P102090
{
    /// <summary>
    /// P102090の帳票用モデル
    /// </summary>
    /// <remarks>
    /// 作成日：作成日：2025/3/6
    /// 作成者：NEXT
    /// </remarks>
    public class P102090Model
    {
        /// <summary>
        /// P102090の帳票出力日時
        /// </summary>
        public DateTime dateTimeToString = DateTime.Now;

        /// <summary>
        /// P102090の帳票用モデル(条件)
        /// </summary>
        public P102090JoukenModel P102090BatchJouken { get; set; }

        /// <summary>
        /// P102090の帳票明細用モデルリスト
        /// </summary>
        public List<P102090TableRecord> P102090TableRecords { get; set; }
    }
}
