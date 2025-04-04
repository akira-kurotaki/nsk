namespace NSK_B106060.Models
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
        /// 支所
        /// </summary>
        public string? JokenShisho { get; set; } = string.Empty;

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