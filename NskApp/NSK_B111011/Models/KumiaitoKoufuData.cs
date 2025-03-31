namespace NSK_B111011.Models
{
    /// <summary>
    /// 組合等交付データ
    /// </summary>
    public class KumiaitoKoufuData
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        public string 組合等コード { get; set; } = string.Empty;

        /// <summary>
        /// 引受面積
        /// </summary>
        public decimal 引受面積 { get; set; }

        /// <summary>
        /// 引受収量
        /// </summary>
        public int 引受収量 { get; set; }

        /// <summary>
        /// 共済金額
        /// </summary>
        public int 共済金額 { get; set; }

        /// <summary>
        /// 保険料総額
        /// </summary>
        public int 保険料総額 { get; set; }

        /// <summary>
        /// 組合等別国庫負担金
        /// </summary>
        public int 組合等別国庫負担金 { get; set; }

        /// <summary>
        /// 組合員等負担共済掛金
        /// </summary>
        public int 組合員等負担共済掛金 { get; set; }

        /// <summary>
        /// 組合員等負担共済掛金徴収済額
        /// </summary>
        public int 組合員等負担共済掛金徴収済額 { get; set; }

        /// <summary>
        /// 共済掛金徴収割合
        /// </summary>
        public decimal 共済掛金徴収割合 { get; set; }

        /// <summary>
        /// 組合等交付対象金額
        /// </summary>
        public int 組合等交付対象金額 { get; set; } 
    }
}
