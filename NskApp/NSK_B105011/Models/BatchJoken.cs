namespace NSK_B105011.Models
{
    /// <summary>
    /// 条件バッチ
    /// </summary>
    public class BatchJoken
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        public string JokenKumiaitoCd { get; set; } = string.Empty;

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
        /// 市町村
        /// </summary>
        public string JokenShichoson { get; set; } = string.Empty;

        /// <summary>
        /// 大地区
        /// </summary>
        public string JokenDaichiku { get; set; } = string.Empty;

        /// <summary>
        /// 小地区（開始）
        /// </summary>
        public string JokenShochikuStart { get; set; } = string.Empty;

        /// <summary>
        /// 小地区（終了）
        /// </summary>
        public string JokenShochikuEnd { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等コードFrom
        /// </summary>
        public string JokenKumiaiintoCdStart { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等コードTo
        /// </summary>
        public string JokenKumiaiintoCdEnd { get; set; } = string.Empty;

        /// <summary>
        /// 出力順1
        /// </summary>
        public string JokenShuturyokujun1 { get; set; } = string.Empty;

        /// <summary>
        /// 昇順・降順1
        /// </summary>
        public string JokenShojunKojun1 { get; set; } = string.Empty;

        /// <summary>
        /// 出力順2
        /// </summary>
        public string JokenShuturyokujun2 { get; set; } = string.Empty;

        /// <summary>
        /// 昇順・降順2
        /// </summary>
        public string JokenShojunKojun2 { get; set; } = string.Empty;

        /// <summary>
        /// 出力順3
        /// </summary>
        public string JokenShuturyokujun3 { get; set; } = string.Empty;

        /// <summary>
        /// 昇順・降順3
        /// </summary>
        public string JokenShojunKojun3 { get; set; } = string.Empty;

        /// <summary>
        /// ファイル名
        /// </summary>
        public string JokenFileName { get; set; } = string.Empty;

        /// <summary>
        /// 文字コード
        /// </summary>
        public string JokenMojiCd { get; set; } = string.Empty;
    }
}