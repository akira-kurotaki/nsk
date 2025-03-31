namespace NSK_B109031.Models
{
    /// <summary>
    /// 規模別分布データPQ
    /// </summary>
    public class KibobetuBunpuDataPQ
    {
        /// <summary>
        /// 組合等正式名称
        /// </summary>
        public string kumiaito_nm { get; set; } = string.Empty;

        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string 共済目的コード { get; set; } = string.Empty;

        /// <summary>
        /// 類区分
        /// </summary>
        public string 類区分 { get; set; } = string.Empty;

        /// <summary>
        /// 都道府県コード
        /// </summary>
        public string todofuken_cd { get; set; } = string.Empty;

        /// <summary>
        /// 組合等コード
        /// </summary>
        public string 組合等コード { get; set; } = string.Empty;

        /// <summary>
        /// 合併特例有無フラグ
        /// </summary>
        public string 合併特例有無フラグ { get; set; } = string.Empty;

        /// <summary>
        /// 合併時識別コード
        /// </summary>
        public string 合併時識別コード { get; set; } = string.Empty;

        /// <summary>
        /// 大地区コード
        /// </summary>
        public string 大地区コード { get; set; } = string.Empty;

        /// <summary>
        /// 引受面積区分
        /// </summary>
        public int 引受面積区分 { get; set; }

        /// <summary>
        /// 引受戸数
        /// </summary>
        public int 引受戸数 { get; set; }

        /// <summary>
        /// 引受面積
        /// </summary>
        public decimal 引受面積 { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        public int 年産 { get; set; }
    }
}
