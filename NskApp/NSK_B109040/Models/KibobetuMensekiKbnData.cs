namespace NSK_B109040.Models
{
    /// <summary>
    /// 規模別面積区分情報
    /// </summary>
    public class KibobetuMensekiKbnData
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
        /// 組合等コード
        /// </summary>
        public string 組合等コード { get; set; } = string.Empty;

        /// <summary>
        /// 支所コード
        /// </summary>
        public string 支所コード { get; set; } = string.Empty;

        /// <summary>
        /// 規模別面積区分
        /// </summary>
        public string 規模別面積区分 { get; set; } = string.Empty;

        /// <summary>
        /// 対象面積上限
        /// </summary>
        public string 対象面積上限 { get; set; } = string.Empty;
    }
}
