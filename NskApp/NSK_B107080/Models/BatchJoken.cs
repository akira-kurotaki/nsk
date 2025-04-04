namespace NSK_B107080.Models
{
    /// <summary>
    /// 条件バッチ
    /// </summary>
    public class BatchJoken
    {
        /// <summary>
        /// 年産
        /// </summary>
        public string? JokenNensan { get; set; } = string.Empty;

        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string? JokenKyosaiMokutekiCd { get; set; } = string.Empty;

        /// <summary>
        /// 大地区
        /// </summary>
        public string? JokenDaichiku { get; set; } = string.Empty;

        /// <summary>
        /// 小地区（開始）
        /// </summary>
        public string? JokenShochikuStart { get; set; } = string.Empty;

        /// <summary>
        /// 小地区（終了）
        /// </summary>
        public string? JokenShochikuEnd { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等コードFrom
        /// </summary>
        public string? JokenKumiaiintoCdStart { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等コードTo
        /// </summary>
        public string? JokenKumiaiintoCdEnd { get; set; } = string.Empty;

        /// <summary>
        /// ファイル名
        /// </summary>
        public string? JokenFileName { get; set; } = string.Empty;

        /// <summary>
        /// 文字コード
        /// </summary>
        public string? JokenMojiCd { get; set; } = string.Empty;
    }
}