namespace NSK_B107041.Models
{
    /// <summary>
    /// 細目データ
    /// </summary>
    public class MainSaimokuDataRecord
    {
        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string 組合員等コード { get; set; } = string.Empty;

        /// <summary>
        /// 引受回
        /// </summary>
        public int 引受回 { get; set; }

        /// <summary>
        /// 組合員等負担共済掛金
        /// </summary>
        public decimal 組合員等負担共済掛金 { get; set; }

        /// <summary>
        /// 賦課金計
        /// </summary>
        public decimal 賦課金計 { get; set; }

        /// <summary>
        /// 引受解除返還賦課金額
        /// </summary>
        public decimal 引受解除返還賦課金額 { get; set; }

        /// <summary>
        /// 納入額
        /// </summary>
        public decimal 納入額 { get; set; }

        /// <summary>
        /// 徴収組合員等コード
        /// </summary>
        public string 徴収組合員等コード { get; set; } = string.Empty;

        /// <summary>
        /// 徴収金額
        /// </summary>
        public decimal 徴収金額 { get; set; }

        /// <summary>
        /// 内農家負担掛金
        /// </summary>
        public decimal 内農家負担掛金 { get; set; }

        /// <summary>
        /// 内賦課金
        /// </summary>
        public decimal 内賦課金 { get; set; }

        /// <summary>
        /// 引受解除徴収賦課金額
        /// </summary>
        public decimal 引受解除徴収賦課金額 { get; set; }
    }
}
