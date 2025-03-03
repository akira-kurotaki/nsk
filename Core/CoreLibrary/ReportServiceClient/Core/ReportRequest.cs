namespace ReportService.Core
{
    /// <summary>
    /// リアルタイム帳票の出力条件
    /// </summary>
    public class ReportRequest
    {
        /// <summary>
        /// 帳票制御ID
        /// </summary>
        public string? reportControlId { get; set; }

        /// <summary>
        /// ユーザID
        /// </summary>
        public string? userId { get; set; }

        /// <summary>
        /// 条件ID
        /// </summary>
        public int joukenId { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        public string? todofukenCd { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        public string? kumiaitoCd { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        public string? shishoCd { get; set; }

        /// <summary>
        /// 利用可能支所一覧
        /// </summary>
        public List<string>? shishoList { get; set; }
    }
}
