namespace NskReportMain.Models.NSK_P107010
{
    /// <summary>
    /// NSK_P107070の組合員等コードヘッダーモデル
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class NSK_P107010HeaderModel
    {
        #region 取得データ

        /// <summary>
        /// 組合等コード
        /// </summary>
        public string 組合等コード { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        public short? 年産 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 引受回
        /// </summary>
        public short? 引受回 { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        public string 支所コード { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 共済目的名称
        /// </summary>
        public string 共済目的名称 { get; set; }

        /// <summary>
        /// 郵便番号
        /// </summary>
        public string 郵便番号 { get; set; }

        /// <summary>
        /// 申込者住所
        /// </summary>
        public string 申込者住所 { get; set; }

        /// <summary>
        /// 氏名又は法人名
        /// </summary>
        public string 氏名又は法人名 { get; set; }

        /// <summary>
        /// 組合住所
        /// </summary>
        public string 組合住所 { get; set; }

        /// <summary>
        /// 組合等正式名称
        /// </summary>
        public string 組合等正式名称 { get; set; }

        /// <summary>
        /// 代表者役職名
        /// </summary>
        public string 代表者役職名 { get; set; }

        /// <summary>
        /// 代表者氏名
        /// </summary>
        public string 代表者氏名 { get; set; }

        /// <summary>
        /// <summary>
        /// ファイルパス
        /// </summary>
        public string ファイルパス { get; set; }

        /// <summary>
        /// ファイル名
        /// </summary>
        public string ファイル名 { get; set; }

        /// <summary>
        /// 組合員等負担共済掛金
        /// </summary>
        public Decimal? 組合員等負担共済掛金 { get; set; }

        /// <summary>
        /// 賦課金計
        /// </summary>
        public Decimal? 賦課金計 { get; set; }

        /// <summary>
        /// 今回迄徴収額
        /// </summary>
        public Decimal? 今回迄徴収額 { get; set; }

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
        /// 払込期限
        /// </summary>
        public string 継続特約フラグ { get; set; }

        /// <summary>
        /// 大地区コード
        /// </summary>
        public string 大地区コード { get; set; }

        /// <summary>
        /// 小地区コード
        /// </summary>
        public string 小地区コード { get; set; }

        #endregion

        #region 帳票出力クラス編集項目

        /// <summary>
        /// 年産_表示
        /// </summary>
        public string 年産_表示 { get; set; }

        /// <summary>
        /// 年産_表示
        /// </summary>
        public string 申込日_表示 { get; set; }

        /// <summary>
        /// 組合代表者印_表示有無
        /// </summary>
        public bool 組合代表者印_表示有無 { get; set; }

        /// <summary>
        /// 文書上段_表示
        /// </summary>
        public string 文書_上段_表示 { get; set; }

        /// <summary>
        /// 文書上段_表示
        /// </summary>
        public string 文書_耕地等情報_表示 { get; set; }

        /// <summary>
        /// 文書上段_表示
        /// </summary>
        public string 文書_下段_表示 { get; set; }

        /// <summary>
        /// 年産_表示
        /// </summary>
        public string 共済関係成立日_表示 { get; set; }

        /// <summary>
        /// 負担共済掛金事務費賦課金の合計_表示
        /// </summary>
        public Decimal? 負担共済掛金事務費賦課金の合計_表示 { get; set; }

        /// <summary>
        /// 既納入額_表示
        /// </summary>
        public Decimal? 既納入額_表示 { get; set; }

        /// <summary>
        /// 納入額_表示
        /// </summary>
        public Decimal? 納入額_表示 { get; set; }

        /// <summary>
        /// 年産_表示
        /// </summary>
        public string 払込期限_表示 { get; set; }

        /// <summary>
        /// 年産_表示
        /// </summary>
        public string 口座振替日_表示 { get; set; }

        /// <summary>
        /// 自動継続特約の有_表示有無
        /// </summary>
        public bool 自動継続特約の有_表示有無 { get; set; }

        /// <summary>
        /// 自動継続特約の無_表示有無
        /// </summary>
        public bool 自動継続特約の無_表示有無 { get; set; }

        /// <summary>
        /// SUB1_表示有無
        /// </summary>
        public bool SUB1_表示有無 { get; set; }

        /// <summary>
        /// SUB2_表示有無
        /// </summary>
        public bool SUB2_表示有無 { get; set; }

        /// <summary>
        /// ページ
        /// </summary>
        public string ページ { get; set; }
        #endregion
    }
}
