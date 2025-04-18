namespace NSK_B107041.Models
{
    /// <summary>
    /// 口座振替細目データ
    /// </summary>
    public class SubSaimokuDataRecord
    {
        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string 組合員等コード { get; set; } = string.Empty;

        /// <summary>
        /// 振込金額
        /// </summary>
        public decimal 振込金額 { get; set; }
    }
}
