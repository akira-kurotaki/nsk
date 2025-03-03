namespace CoreLibrary.Core.Dto
{
    /// <summary>
    /// CSV出力SQLクラス
    /// </summary>
    public class CsvOutputSql
    {
        /// <summary>
        /// SQL本体
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        /// 検索条件（パラメータ）
        /// </summary>
        public List<SqlParam> SqlParams { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        public string SqlOrder { get; set; }

        /// <summary>
        /// 文字コード
        /// </summary>
        public string CharacterCd { get; set; }

        /// <summary>
        /// CSVファイル名
        /// </summary>
        public string CsvNm { get; set; }

        /// <summary>
        /// ヘッダ有無
        /// </summary>
        public bool HeaderOnOff { get; set; }

        /// <summary>
        /// ヘッダ文字列
        /// </summary>
        public List<string> Headers { get; set; }

        /// <summary>
        /// セパレーター
        /// </summary>
        public string SeparatorFont { get; set; }

        /// <summary>
        /// BOMコード有無
        /// </summary>
        public bool BomOnOff { get; set; }
    }
}
