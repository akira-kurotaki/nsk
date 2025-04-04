namespace NSK_B106070.Models
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
        /// 引受回
        /// </summary>
        public Decimal? 引受回 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string? 共済目的コード { get; set; } = string.Empty;

        /// <summary>
        /// 共済目的
        /// </summary>
        public string? 共済目的 { get; set; } = string.Empty;

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
        /// 組合員等コード
        /// </summary>
        public string? 組合員等コード { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等氏名漢字
        /// </summary>
        public string? 組合員等氏名漢字 { get; set; } = string.Empty;

        /// <summary>
        /// 類区分
        /// </summary>
        public string? 類区分 { get; set; } = string.Empty;

        /// <summary>
        /// 類区分名
        /// </summary>
        public string? 類区分名 { get; set; } = string.Empty;

        /// <summary>
        /// 引受方式
        /// </summary>
        public string? 引受方式 { get; set; } = string.Empty;

        /// <summary>
        /// 引受方式名
        /// </summary>
        public string? 引受方式名 { get; set; } = string.Empty;

        /// <summary>
        /// 補償割合コード
        /// </summary>
        public string? 補償割合コード { get; set; } = string.Empty;

        /// <summary>
        /// 補償割合短縮名称
        /// </summary>
        public string? 補償割合短縮名称 { get; set; } = string.Empty;

        /// <summary>
        /// 特約区分
        /// </summary>
        public string? 特約区分 { get; set; } = string.Empty;

        /// <summary>
        /// 特約区分名称
        /// </summary>
        public string? 特約区分名称 { get; set; } = string.Empty;

        /// <summary>
        /// 統計単位地域コード
        /// </summary>
        public string? 統計単位地域コード { get; set; } = string.Empty;

        /// <summary>
        /// 危険段階地域区分
        /// </summary>
        public string? 危険段階地域区分 { get; set; } = string.Empty;

        /// <summary>
        /// 危険段階区分
        /// </summary>
        public string? 危険段階区分 { get; set; } = string.Empty;

        /// <summary>
        /// 共済金額単価
        /// </summary>
        public Decimal? 共済金額単価 { get; set; }

        /// <summary>
        /// 引受筆数
        /// </summary>
        public Decimal? 引受筆数 { get; set; }

        /// <summary>
        /// 引受面積
        /// </summary>
        public Decimal? 引受面積 { get; set; }

        /// <summary>
        /// 基準収穫量
        /// </summary>
        public Decimal? 基準収穫量 { get; set; }

        /// <summary>
        /// 引受収量
        /// </summary>
        public Decimal? 引受収量 { get; set; }

        /// <summary>
        /// 共済金額
        /// </summary>
        public Decimal? 共済金額 { get; set; }

        /// <summary>
        /// 基準共済掛金
        /// </summary>
        public Decimal? 基準共済掛金 { get; set; }

        /// <summary>
        /// 農家負担共済掛金
        /// </summary>
        public Decimal? 農家負担共済掛金 { get; set; }

        /// <summary>
        /// 一般賦課金
        /// </summary>
        public Decimal? 一般賦課金 { get; set; }

        /// <summary>
        /// 防災賦課金
        /// </summary>
        public Decimal? 防災賦課金 { get; set; }

        /// <summary>
        /// 特別賦課金
        /// </summary>
        public Decimal? 特別賦課金 { get; set; }

        /// <summary>
        /// 組合員等割
        /// </summary>
        public Decimal? 組合員等割 { get; set; }

        /// <summary>
        /// 共通申請等割引額
        /// </summary>
        public Decimal? 共通申請等割引額 { get; set; }

        /// <summary>
        /// 賦課金計
        /// </summary>
        public Decimal? 賦課金計 { get; set; }

        /// <summary>
        /// 調定額
        /// </summary>
        public Decimal? 調定額 { get; set; }

        /// <summary>
        /// 地域集団コード
        /// </summary>
        public string? 地域集団コード { get; set; } = string.Empty;

        /// <summary>
        /// 地域集団名
        /// </summary>
        public string? 地域集団名 { get; set; } = string.Empty;

        /// <summary>
        /// 賦課金大量データ取込フラグ
        /// </summary>
        public string? 賦課金大量データ取込フラグ { get; set; } = string.Empty;

        /// <summary>
        /// 賦課金大量データ取込日時
        /// </summary>
        public DateTime? 賦課金大量データ取込日時 { get; set; }
    }
}
