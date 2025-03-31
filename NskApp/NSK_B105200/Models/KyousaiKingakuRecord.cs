namespace NSK_B105200.Models
{
    /// <summary>
    /// 共済金額一覧表データ
    /// </summary>
    public class KyousaiKingakuRecord
    {

        /// <summary>
        /// 組合等コード
        /// </summary>
        public string 組合等コード { get; set; } = string.Empty;

        /// <summary>
        /// 組合等名
        /// </summary>
        public string 組合等名 { get; set; } = string.Empty;

        /// <summary>
        /// 年産
        /// </summary>
        public int 年産 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string 共済目的コード { get; set; } = string.Empty;

        /// <summary>
        /// 共済目的名
        /// </summary>
        public string 共済目的名 { get; set; } = string.Empty;

        /// <summary>
        /// 引受方式名称
        /// </summary>
        public string 引受方式名称 { get; set; } = string.Empty;

        /// <summary>
        /// 類区分
        /// </summary>
        public string 類区分 { get; set; } = string.Empty;

        /// <summary>
        /// 類区分名
        /// </summary>
        public string 類区分名 { get; set; } = string.Empty;

        /// <summary>
        /// 支所コード
        /// </summary>
        public string 支所コード { get; set; } = string.Empty;

        /// <summary>
        /// 支所名
        /// </summary>
        public string 支所名 { get; set; } = string.Empty;

        /// <summary>
        /// 大地区コード
        /// </summary>
        public string 大地区コード { get; set; } = string.Empty;

        /// <summary>
        /// 大地区名
        /// </summary>
        public string 大地区名 { get; set; } = string.Empty;

        /// <summary>
        /// 小地区コード
        /// </summary>
        public string 小地区コード { get; set; } = string.Empty;

        /// <summary>
        /// 小地区名
        /// </summary>
        public string 小地区名 { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string 組合員等コード { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等氏名
        /// </summary>
        public string 組合員等氏名 { get; set; } = string.Empty;

        /// <summary>
        /// 共済金額
        /// </summary>
        public decimal 共済金額 { get; set; }

    }
}
