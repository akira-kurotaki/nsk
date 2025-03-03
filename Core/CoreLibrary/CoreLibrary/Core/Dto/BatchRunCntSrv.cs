namespace CoreLibrary.Core.Dto
{
    /// <summary>
    /// バッチ実行件数取得（サーバ）
    /// </summary>
    [Serializable]
    public class BatchRunCntSrv
    {
        /// <summary>
        /// 実行サーバ
        /// </summary>
        public string BatchRunServer { get; set; }

        /// <summary>
        /// 件数
        /// </summary>
        public int Count { get; set; }
    }
}
