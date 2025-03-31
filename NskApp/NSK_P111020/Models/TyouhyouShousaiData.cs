namespace NSK_P111020.Models
{
    /// <summary>
    /// 帳票_詳細データ
    /// </summary>
    public class TyouhyouShousaiData
    {
        /// <summary>
        /// タイトル
        /// </summary>
        public string タイトル { get; set; } = string.Empty;

        /// <summary>
        /// 組合等正式名称
        /// </summary>
        public string 組合等正式名称 { get; set; } = string.Empty;

        /// <summary>
        /// 組合等長名
        /// </summary>
        public string 組合等長名 { get; set; } = string.Empty;

        /// <summary>
        /// 年産
        /// </summary>
        public int 年産 { get; set; }

        /// <summary>
        /// 今回申請額
        /// </summary>
        public int 今回申請額 { get; set; }

        /// <summary>
        /// 文言
        /// </summary>
        public string? 文言 { get; set; } = string.Empty;

        /// <summary>
        /// 負担金交付区分
        /// </summary>
        public string 負担金交付区分 { get; set; } = string.Empty;

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
        /// （組合等別連合会）保険料
        /// </summary>
        public int 組合等別連合会保険料 { get; set; }

        /// <summary>
        /// 組合等別国庫負担金
        /// </summary>
        public int 組合等別国庫負担金 { get; set; }

        /// <summary>
        /// 特定組合等（組合等）負担金交付対象金額
        /// </summary>
        public decimal 特定組合等組合等負担金交付対象金額 { get; set; }

        /// <summary>
        /// 徴収すべき共済掛金
        /// </summary>
        public int 徴収すべき共済掛金 { get; set; }

        /// <summary>
        /// 左のうち徴収済額
        /// </summary>
        public int 左のうち徴収済額 { get; set; }

        /// <summary>
        /// 共済掛金徴収割合
        /// </summary>
        public decimal 共済掛金徴収割合 { get; set; }

        /// <summary>
        /// 特定組合等（組合等）交付金の金額
        /// </summary>
        public decimal 特定組合等組合等交付金の金額 { get; set; }

        /// <summary>
        /// 既受領交付金の金額
        /// </summary>
        public int 既受領交付金の金額 { get; set; }

        /// <summary>
        /// 今回交付申請額（又は今回返還申請額）
        /// </summary>
        public int 今回交付申請額又は今回返還申請額 { get; set; }
    }
}
