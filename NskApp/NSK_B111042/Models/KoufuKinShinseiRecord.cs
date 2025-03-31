namespace NSK_B111042.Models
{
    /// <summary>
    /// 交付申請書データ
    /// </summary>
    public class KoufuKinShinseiRecord
    {
        /// <summary>
        /// 連合会略称
        /// </summary>
        public string 連合会略称 { get; set; } = string.Empty;

        /// <summary>
        /// 負担金交付区分
        /// </summary>
        public string 負担金交付区分 { get; set; } = string.Empty;

        /// <summary>
        /// 組合等コード
        /// </summary>
        public string 組合等コード { get; set; } = string.Empty;

        /// <summary>
        /// 交付回
        /// </summary>
        public int 交付回 { get; set; }

        /// <summary>
        /// 保険料総額
        /// </summary>
        public decimal 保険料総額 { get; set; }

        /// <summary>
        /// 組合等別国庫負担金
        /// </summary>
        public decimal 組合等別国庫負担金 { get; set; }

        /// <summary>
        /// 組合等負担金交付対象金額
        /// </summary>
        public decimal 組合等負担金交付対象金額 { get; set; }

        /// <summary>
        /// 組合員等負担共済掛金
        /// </summary>
        public decimal 組合員等負担共済掛金 { get; set; }

        /// <summary>
        /// 組合員等負担共済掛金徴収済額
        /// </summary>
        public decimal 組合員等負担共済掛金徴収済額 { get; set; }

        /// <summary>
        /// 共済掛金徴収割合
        /// </summary>
        public decimal 共済掛金徴収割合 { get; set; }

        /// <summary>
        /// 組合等交付金の金額
        /// </summary>
        public decimal 組合等交付金の金額 { get; set; }

        /// <summary>
        /// 既受領交付金の金額
        /// </summary>
        public decimal 既受領交付金の金額 { get; set; }

        /// <summary>
        /// 今回交付申請額
        /// </summary>
        public decimal 今回交付申請額 { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        public int 年産 { get; set; }

    }
}
