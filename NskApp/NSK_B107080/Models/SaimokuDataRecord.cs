namespace NSK_B107080.Models
{
    /// <summary>
    /// 細目データ
    /// </summary>
    public class SaimokuDataRecord
    {
        /// <summary>
        /// 共済目的名称
        /// </summary>
        public string? 共済目的名称 { get; set; } = string.Empty;

        /// <summary>
        /// 組合等正式名称
        /// </summary>
        public string? 組合等正式名称 { get; set; } = string.Empty;

        /// <summary>
        /// 支所コード
        /// </summary>
        public string? 支所コード { get; set; } = string.Empty;

        /// <summary>
        /// 支所名
        /// </summary>
        public string? 支所名 { get; set; } = string.Empty;

        /// <summary>
        /// 大地区コード
        /// </summary>
        public string? 大地区コード { get; set; } = string.Empty;

        /// <summary>
        /// 大地区名
        /// </summary>
        public string? 大地区名 { get; set; } = string.Empty;

        /// <summary>
        /// 小地区コード
        /// </summary>
        public string? 小地区コード { get; set; } = string.Empty;

        /// <summary>
        /// 小地区名
        /// </summary>
        public string? 小地区名 { get; set; } = string.Empty;

        /// <summary>
        /// 引受回
        /// </summary>
        public short? 引受回 { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string? 組合員等コード { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等名
        /// </summary>
        public string? 組合員等名 { get; set; } = string.Empty;

        /// <summary>
        /// 金融機関コード
        /// </summary>
        public string? 金融機関コード { get; set; } = string.Empty;

        /// <summary>
        /// 口座番号
        /// </summary>
        public string? 口座番号 { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等負担共済掛金
        /// </summary>
        public Decimal? 組合員等負担共済掛金 { get; set; }

        /// <summary>
        /// 一般賦課金
        /// </summary>
        public Decimal? 一般賦課金 { get; set; }

        /// <summary>
        /// 組合員等割
        /// </summary>
        public Decimal? 組合員等割 { get; set; }

        /// <summary>
        /// 特別賦課金
        /// </summary>
        public Decimal? 特別賦課金 { get; set; }

        /// <summary>
        /// 防災賦課金
        /// </summary>
        public Decimal? 防災賦課金 { get; set; }

        /// <summary>
        /// 賦課金計
        /// </summary>
        public Decimal? 賦課金計 { get; set; }

        /// <summary>
        /// 納入額
        /// </summary>
        public Decimal? 納入額 { get; set; }

        /// <summary>
        /// 前回迄徴収額
        /// </summary>
        public Decimal? 前回迄徴収額 { get; set; }

        /// <summary>
        /// 今回迄徴収額
        /// </summary>
        public Decimal? 今回迄徴収額 { get; set; }

        /// <summary>
        /// 前回迄引受解除徴収賦課金額
        /// </summary>
        public Decimal? 前回迄引受解除徴収賦課金額 { get; set; }

        /// <summary>
        /// 賦課金計差額
        /// </summary>
        public Decimal? 賦課金計差額 { get; set; }

        /// <summary>
        /// 今回引受解除徴収賦課金額
        /// </summary>
        public Decimal? 今回引受解除徴収賦課金額 { get; set; }

        /// <summary>
        /// 今回迄引受解除徴収賦課金額
        /// </summary>
        public Decimal? 今回迄引受解除徴収賦課金額 { get; set; }

        /// <summary>
        /// 徴収年月日
        /// </summary>
        public DateTime? 徴収年月日 { get; set; }

        /// <summary>
        /// 振込引落区分コード
        /// </summary>
        public string? 振込引落区分コード { get; set; } = string.Empty;

        /// <summary>
        /// 区分名称
        /// </summary>
        public string? 区分名称 { get; set; } = string.Empty;

        /// <summary>
        /// 自動振替処理の有無
        /// </summary>
        public string? 自動振替処理の有無 { get; set; } = string.Empty;

        /// <summary>
        /// 徴収区分コード
        /// </summary>
        public string? 徴収区分コード { get; set; } = string.Empty;

        /// <summary>
        /// 徴収区分名_表示
        /// </summary>
        public string? 徴収区分名_表示 { get; set; } = string.Empty;

        /// <summary>
        /// 徴収理由コード
        /// </summary>
        public string? 徴収理由コード { get; set; } = string.Empty;

        /// <summary>
        /// 徴収理由名
        /// </summary>
        public string? 徴収理由名 { get; set; } = string.Empty;

        /// <summary>
        /// 徴収者
        /// </summary>
        public string? 徴収者 { get; set; } = string.Empty;

        /// <summary>
        /// 自動振替フラグ
        /// </summary>
        public string? 自動振替フラグ { get; set; } = string.Empty;

        /// <summary>
        /// 地域集団コード
        /// </summary>
        public string? 地域集団コード { get; set; } = string.Empty;

        /// <summary>
        /// 地域集団名
        /// </summary>
        public string? 地域集団名 { get; set; } = string.Empty;

        /// <summary>
        /// 解除フラグ
        /// </summary>
        public string? 解除フラグ { get; set; } = string.Empty;
    }
}
