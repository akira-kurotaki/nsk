namespace NSK_B110030.Models
{
    /// <summary>
    /// 危険段階連携データ
    /// </summary>
    public class KikenDankaiRenkeiData
    {
        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string 共済目的コード { get; set; } = string.Empty;

        /// <summary>
        /// 年産
        /// </summary>
        public int 年産 { get; set; }

        /// <summary>
        /// 合併時識別コード
        /// </summary>
        public string 合併時識別コード { get; set; } = string.Empty;

        /// <summary>
        /// 統計単位地域コード
        /// </summary>
        public string 統計単位地域コード { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string 組合員等コード { get; set; } = string.Empty;

        /// <summary>
        /// 類区分
        /// </summary>
        public string 類区分 { get; set; } = string.Empty;

        /// <summary>
        /// 共済掛金区分
        /// </summary>
        public string 共済掛金区分 { get; set; } = string.Empty;

        /// <summary>
        /// 共済金額
        /// </summary>
        public decimal 共済金額 { get; set; }

        /// <summary>
        /// 共済掛金標準率
        /// </summary>
        public decimal 共済掛金標準率 { get; set; }
    }
}
