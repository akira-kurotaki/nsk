namespace NSK_B111040.Models
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
        /// 交付回
        /// </summary>
        public int 交付回 { get; set; }

        /// <summary>
        /// 組合等別国庫負担金
        /// </summary>
        public decimal 組合等交付金の金額 { get; set; }

        /// <summary>
        /// 組合等別連合会保険料
        /// </summary>
        public decimal 既受領交付金の金額 { get; set; }

        /// <summary>
        /// 組合等別連合会負担金交付対象額
        /// </summary>
        public decimal 今回交付申請額 { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        public int 年産 { get; set; }

    }
}
