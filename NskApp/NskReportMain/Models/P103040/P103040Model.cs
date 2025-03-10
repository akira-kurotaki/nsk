namespace NskReportMain.Models.P103040
{
    /// <summary>
    /// P10304の帳票用モデル
    /// </summary>
    /// <remarks>
    /// 作成日：作成日：2025/3/6
    /// 作成者：NEXT
    /// </remarks>
    public class P103040Model
    {
        /// <summary>
        /// P10304の帳票出力日時
        /// </summary>
        public DateTime dateTimeToString = DateTime.Now;

        /// <summary>
        /// P10304の帳票用モデル(条件)
        /// </summary>
        public P103040JoukenModel P103040BatchJouken { get; set; }

        /// <summary>
        /// P10304の帳票用モデル(ヘッダー部)
        /// </summary>
        public P103040HeaderModel P103040HeaderModel { get; set; }

        /// <summary>
        /// P10304の帳票明細用モデルリスト
        /// </summary>
        public List<P103040TableRecord> P103040TableRecords { get; set; }
    }
}
