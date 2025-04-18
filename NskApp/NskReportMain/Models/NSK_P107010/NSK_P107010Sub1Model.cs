namespace NskReportMain.Models.NSK_P107010
{
    /// <summary>
    /// NSK_P107070の加入承諾書兼共済掛金等払込通知書Sub2モデル
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class NSK_P107010Sub1Model
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
        /// 支所コード
        /// </summary>
        public string 支所コード { get; set; }

        /// <summary>
        /// 引受回
        /// </summary>
        public short? 引受回 { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        public string 類区分 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        public string 統計単位地域コード { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 類短縮名称
        /// </summary>
        public string 類短縮名称 { get; set; }

        /// <summary>
        /// 引受方式名称
        /// </summary>
        public string 引受方式名称 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        public string 補償割合コード { get; set; }

        /// <summary>
        /// 補償割合短縮名称
        /// </summary>
        public string 補償割合短縮名称 { get; set; }

        /// <summary>
        /// 特約区分
        /// </summary>
        public string 特約区分 { get; set; }

        /// <summary>
        /// 共済金額選択順位
        /// </summary>
        public Decimal? 共済金額選択順位 { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        public string 備考 { get; set; }

        /// <summary>
        /// 加入申込区分
        /// </summary>
        public string 加入申込区分 { get; set; }

        #endregion

        #region 帳票出力クラス編集項目

        /// <summary>
        /// 年産_表示
        /// </summary>
        public string 一筆半損特約の有無_表示 { get; set; }

        /// <summary>
        /// 年産_表示
        /// </summary>
        public string 全相殺方式等の収穫量の確認方法_表示 { get; set; }

        #endregion
    }
}
