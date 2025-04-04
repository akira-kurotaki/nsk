namespace NSK_B106060.Models
{
    /// <summary>
    /// 細目データ
    /// </summary>
    public class SaimokuDataRecord
    {
        /// <summary>
        /// 年産
        /// </summary>
        public int? 年産 { get; set; }

        /// <summary>
        /// 共済目的
        /// </summary>
        public string? 共済目的 { get; set; } = string.Empty;

        /// <summary>
        /// 類区分
        /// </summary>
        public string? 類区分 { get; set; } = string.Empty;

        /// <summary>
        /// 類名称
        /// </summary>
        public string? 類名称 { get; set; } = string.Empty;

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
        /// 等級
        /// </summary>
        public string? 等級 { get; set; } = string.Empty;

        /// <summary>
        /// 基準単収
        /// </summary>
        public Decimal? 基準単収 { get; set; }

        /// <summary>
        /// 品種コード
        /// </summary>
        public string? 品種コード { get; set; } = string.Empty;

        /// <summary>
        /// 品種名等
        /// </summary>
        public string? 品種名等 { get; set; } = string.Empty;

        /// <summary>
        /// 品種係数
        /// </summary>
        public Decimal? 品種係数 { get; set; }

        /// <summary>
        /// 参酌コード
        /// </summary>
        public string? 参酌コード { get; set; } = string.Empty;

        /// <summary>
        /// 参酌係数
        /// </summary>
        public Decimal? 参酌係数 { get; set; }

        /// <summary>
        /// 修正単収
        /// </summary>
        public Decimal? 修正単収 { get; set; }

        /// <summary>
        /// 筆数
        /// </summary>
        public Decimal? 筆数 { get; set; }

        /// <summary>
        /// 引受面積
        /// </summary>
        public Decimal? 引受面積 { get; set; }

        /// <summary>
        /// 修正前基準収穫量
        /// </summary>
        public Decimal? 修正前基準収穫量 { get; set; }

        /// <summary>
        /// 基準単収100％
        /// </summary>
        public Decimal? 基準単収100 { get; set; }

        /// <summary>
        /// 決定単収
        /// </summary>
        public Decimal? 決定単収 { get; set; }

        /// <summary>
        /// 修正後基準収穫量
        /// </summary>
        public Decimal? 修正後基準収穫量 { get; set; }

        /// <summary>
        /// 県通知指示単収
        /// </summary>
        public Decimal? 県通知指示単収 { get; set; }

        /// <summary>
        /// 目標値
        /// </summary>
        public Decimal? 目標値 { get; set; }

        /// <summary>
        /// 修正量
        /// </summary>
        public Decimal? 修正量 { get; set; }

    }
}
