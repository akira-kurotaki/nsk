namespace CoreLibrary.Core.Dto
{
    /// <summary>
    /// DB接続情報
    /// </summary>
    [Serializable]
    public class DbConnectionInfo
    {
        /// <summary>
        /// 都道府県コード
        /// </summary>
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        public string ShishoCd { get; set; }

        /// <summary>
        /// システム区分
        /// </summary>
        public string SystemKbn { get; set; }

        /// <summary>
        /// 接続文字列
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// デフォルトスキーマ
        /// </summary>
        public string DefaultSchema { get; set; }
    }
}
