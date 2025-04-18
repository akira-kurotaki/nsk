namespace NSK_B107041.Models
{
    /// <summary>
    /// 条件バッチ
    /// </summary>
    public class BatchJoken
    {
        /// <summary>
        /// 年産
        /// </summary>
        public string JokenNensan { get; set; } = string.Empty;

        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string JokenKyosaiMokutekitoCd { get; set; } = string.Empty;

        /// <summary>
        /// 支所
        /// </summary>
        public string JokenShisho { get; set; } = string.Empty;

        /// <summary>
        /// 引受回
        /// </summary>
        public string JokenHikiukeKai { get; set; } = string.Empty;

        /// <summary>
        /// 対象データ振替日
        /// </summary>
        public string JokenTaishoFurikaeDate { get; set; } = string.Empty;
    }
}