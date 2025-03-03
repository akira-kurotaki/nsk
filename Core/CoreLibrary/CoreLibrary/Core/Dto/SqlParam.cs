namespace CoreLibrary.Core.Dto
{
    /// <summary>
    /// パラメータ保持クラス
    /// </summary>
    public class SqlParam
    {
        /// <summary>
        /// パラメータ名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// パラメータ値
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// パラメータ型
        /// </summary>
        public int type { get; set; }
    }
}
