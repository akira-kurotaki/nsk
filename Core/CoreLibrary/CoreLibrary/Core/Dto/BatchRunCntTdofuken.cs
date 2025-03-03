namespace CoreLibrary.Core.Dto
{
    /// <summary>
    /// バッチ実行件数取得（都道府県）
    /// </summary>
    [Serializable]
    public class BatchRunCntTdofuken
    {
        /// <summary>
        /// 都道府県コード
        /// </summary>
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 都道府県名
        /// </summary>
        public string TodofukenNm { get; set; }

        /// <summary>
        /// 実行中件数
        /// </summary>
        public int RunningCount { get; set; }

        /// <summary>
        /// 処理待ち件数
        /// </summary>
        public int WaitingCount { get; set; }
    }
}
