namespace NSK_P103040.Models.P103040
{
    /// <summary>
    /// P10304の帳票用モデル
    /// </summary>
    /// <remarks>
    /// 作成日：作成日：2025/3/6
    /// 作成者：NEXT
    /// </remarks>
    public class NSK_103040PModel
    {
        /// <summary>
        /// P10304の帳票用モデル(ヘッダー部)
        /// </summary>
        public required HeaderModel headerModel { get; set; }

        /// <summary>
        /// P10304の帳票明細用モデルリスト
        /// </summary>
        public required List<TableRecord> tableRecords { get; set; }
    }
}
