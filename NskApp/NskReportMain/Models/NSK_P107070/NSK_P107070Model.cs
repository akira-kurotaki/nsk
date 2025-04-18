namespace NskReportMain.Models.NSK_P107070
{
    /// <summary>
    /// NSK_P107070の帳票用モデル（SQL取得データ）
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class NSK_P107070Model
    {
        #region 取得データ
        /// <summary>
        /// 共済目的正式名称
        /// </summary>
        public string 共済目的名称 { get; set; }

        /// <summary>
        /// 組合等正式名称
        /// </summary>
        public string 組合等正式名称 { get; set; }

        /// <summary>
        /// 大地区コード
        /// </summary>
        public string 大地区コード { get; set; }

        /// <summary>
        /// 大地区名
        /// </summary>
        public string 大地区名 { get; set; }

        /// <summary>
        /// 小地区コード
        /// </summary>
        public string 小地区コード { get; set; }

        /// <summary>
        /// 小地区名
        /// </summary>
        public string 小地区名 { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 氏名又は法人名
        /// </summary>
        public string 氏名又は法人名 { get; set; }

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
        public string 振込引落区分コード { get; set; }

        /// <summary>
        /// 徴収区分
        /// </summary>
        public string 徴収区分 { get; set; }

        /// <summary>
        /// 解除フラグ
        /// </summary>
        public string 解除フラグ { get; set; }

        #endregion

        #region 帳票出力クラス編集項目

        /// <summary>
        /// 年産_表示
        /// </summary>
        public string 年産_表示 { get; set; }

        /// <summary>
        /// 日付
        /// </summary>
        public string 日付_表示 { get; set; }

        /// <summary>
        /// 組合等コード_表示
        /// </summary>
        public string 組合等コード_表示 { get; set; }

        /// <summary>
        /// 特別防災賦課金_表示
        /// </summary>
        public Decimal? 特別防災賦課金 { get; set; }

        /// <summary>
        /// 徴収済額_表示
        /// </summary>
        public Decimal? 徴収済額 { get; set; }

        /// <summary>
        /// 今回徴収額_表示
        /// </summary>
        public Decimal? 今回徴収額 { get; set; }

        /// <summary>
        /// 還付額_表示
        /// </summary>
        public Decimal? 還付額 { get; set; }

        /// <summary>
        /// 徴収年月日_表示
        /// </summary>
        public string 徴収年月日_表示 { get; set; }

        /// <summary>
        /// 未納額_表示
        /// </summary>
        public Decimal? 未納額 { get; set; }

        /// <summary>
        /// TOTAL徴収額_表示
        /// </summary>
        public Decimal? TOTAL徴収額 { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        public string 備考 { get; set; }

        #endregion

    }
}
