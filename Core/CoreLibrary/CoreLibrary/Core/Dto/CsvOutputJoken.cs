namespace CoreLibrary.Core.Dto
{
    /// <summary>
    /// CSV出力条件クラス
    /// </summary>
    public class CsvOutputJoken
    {
        /// <summary>
        /// 検索対象
        /// </summary>
        public string SearchTaget { get; set; }

        /// <summary>
        /// 検索条件（SQL）
        /// </summary>
        public string SqlConf { get; set; }

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
        /// セパレーター
        /// </summary>
        public string SeparatorFont { get; set; }

        /// <summary>
        /// BOMコード有無
        /// </summary>
        public bool BomOnOff { get; set; }
    }
}
