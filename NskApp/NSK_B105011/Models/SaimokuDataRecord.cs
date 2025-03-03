namespace NSK_B105011.Models
{
    /// <summary>
    /// 細目データ
    /// </summary>
    public class SaimokuDataRecord
    {
        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string 共済目的コード { get; set; } = string.Empty;

        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string 組合員等コード { get; set; } = string.Empty;

        /// <summary>
        /// 耕地番号
        /// </summary>
        public string 耕地番号 { get; set; } = string.Empty;

        /// <summary>
        /// 分筆番号
        /// </summary>
        public string 分筆番号 { get; set; } = string.Empty;

        /// <summary>
        /// 類区分
        /// </summary>
        public string 類区分 { get; set; } = string.Empty;

        /// <summary>
        /// 地名地番
        /// </summary>
        public string 地名地番 { get; set; } = string.Empty;

        /// <summary>
        /// 耕地面積
        /// </summary>
        public decimal 耕地面積 { get; set; }

        /// <summary>
        /// 引受面積
        /// </summary>
        public decimal 引受面積 { get; set; }

        /// <summary>
        /// 転作等面積
        /// </summary>
        public decimal 転作等面積 { get; set; }

        /// <summary>
        /// 受委託区分
        /// </summary>
        public string 受委託区分 { get; set; } = string.Empty;

        /// <summary>
        /// 備考
        /// </summary>
        public string 備考 { get; set; } = string.Empty;

        /// <summary>
        /// 田畑区分
        /// </summary>
        public string 田畑区分 { get; set; } = string.Empty;

        /// <summary>
        /// 区分コード
        /// </summary>
        public string 区分コード { get; set; } = string.Empty;

        /// <summary>
        /// 種類コード
        /// </summary>
        public string 種類コード { get; set; } = string.Empty;

        /// <summary>
        /// 品種コード
        /// </summary>
        public string 品種コード { get; set; } = string.Empty;

        /// <summary>
        /// 収量等級コード
        /// </summary>
        public string 収量等級コード { get; set; } = string.Empty;

        /// <summary>
        /// 参酌コード
        /// </summary>
        public string 参酌コード { get; set; } = string.Empty;

        /// <summary>
        /// 基準単収
        /// </summary>
        public decimal 基準単収 { get; set; }

        /// <summary>
        /// 基準収穫量
        /// </summary>
        public decimal 基準収穫量 { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime 更新日時 { get; set; }

        /// <summary>
        /// 計算日付
        /// </summary>
        public DateTime 計算日付 { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        public int 年産 { get; set; }

        /// <summary>
        /// 実量基準単収
        /// </summary>
        public decimal 実量基準単収 { get; set; }

        /// <summary>
        /// 統計市町村コード
        /// </summary>
        public string 統計市町村コード { get; set; } = string.Empty;

        /// <summary>
        /// 統計単位地域コード
        /// </summary>
        public string 統計単位地域コード { get; set; } = string.Empty;

        /// <summary>
        /// 統計単収
        /// </summary>
        public decimal 統計単収 { get; set; }

        /// <summary>
        /// 用途区分
        /// </summary>
        public string 用途区分 { get; set; } = string.Empty;

        /// <summary>
        /// 産地別銘柄コード
        /// </summary>
        public string 産地別銘柄コード { get; set; } = string.Empty;

        /// <summary>
        /// 受委託者コード
        /// </summary>
        public string 受委託者コード { get; set; } = string.Empty;
    }
}
