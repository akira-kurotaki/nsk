namespace NSK_P109010.Models
{
    /// <summary>
    /// 帳票の出力対象データ（メインレポート用）
    /// </summary>
    public class TyouhyouDataMain
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        public string 組合等コード { get; set; } = string.Empty;

        /// <summary>
        /// 年産
        /// </summary>
        public int 年産 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string 共済目的コード { get; set; } = string.Empty;

        /// <summary>
        /// 引受回
        /// </summary>
        public int 引受回 { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string 組合員等コード { get; set; } = string.Empty;

        /// <summary>
        /// 類区分
        /// </summary>
        public string 類区分 { get; set; } = string.Empty;

        /// <summary>
        /// 統計単位地域コード
        /// </summary>
        public string 統計単位地域コード { get; set; } = string.Empty;

        /// <summary>
        /// 大地区コード
        /// </summary>
        public string 大地区コード { get; set; } = string.Empty;

        /// <summary>
        /// 小地区コード
        /// </summary>
        public string 小地区コード { get; set; } = string.Empty;

        /// <summary>
        /// 継続特約フラグ
        /// </summary>
        public string 継続特約フラグ { get; set; } = string.Empty;

        /// <summary>
        /// 未加入フラグ
        /// </summary>
        public string 未加入フラグ { get; set; } = string.Empty;

        /// <summary>
        /// 共済目的名称
        /// </summary>
        public string 共済目的名称 { get; set; } = string.Empty;

        /// <summary>
        /// 申込者カナ
        /// </summary>
        public string 申込者カナ { get; set; } = string.Empty;

        /// <summary>
        /// 申込者氏名
        /// </summary>
        public string 申込者氏名 { get; set; } = string.Empty;

        /// <summary>
        /// 郵便番号
        /// </summary>
        public string 郵便番号 { get; set; } = string.Empty;

        /// <summary>
        /// 住所
        /// </summary>
        public string 住所 { get; set; } = string.Empty;

        /// <summary>
        /// 電話番号
        /// </summary>
        public string 電話番号 { get; set; } = string.Empty;

        /// <summary>
        /// 耕地番号
        /// </summary>
        public string 耕地番号 { get; set; } = string.Empty;

        /// <summary>
        /// 分筆番号
        /// </summary>
        public string 分筆番号 { get; set; } = string.Empty;

        /// <summary>
        /// 市町村名
        /// </summary>
        public string 市町村名 { get; set; } = string.Empty;

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
        /// 類区分2
        /// </summary>
        public string 類区分2 { get; set; } = string.Empty;

        /// <summary>
        /// 類区分名称
        /// </summary>
        public string 類区分名称 { get; set; } = string.Empty;

        /// <summary>
        /// 品種コード
        /// </summary>
        public string 品種コード { get; set; } = string.Empty;

        /// <summary>
        /// 品種名称
        /// </summary>
        public string 品種名称 { get; set; } = string.Empty;

        /// <summary>
        /// 田畑区分
        /// </summary>
        public string 田畑区分 { get; set; } = string.Empty;

        /// <summary>
        /// 備考
        /// </summary>
        public string 備考 { get; set; } = string.Empty;

        /// <summary>
        /// 収量等級
        /// </summary>
        public string 収量等級 { get; set; } = string.Empty;

        /// <summary>
        /// 参酌
        /// </summary>
        public decimal 参酌 { get; set; }

        /// <summary>
        /// 組合名称
        /// </summary>
        public string 組合名称 { get; set; } = string.Empty;

        /// <summary>
        /// 組合代表者名
        /// </summary>
        public string 組合代表者名 { get; set; } = string.Empty;

        /// <summary>
        /// 加入形態
        /// </summary>
        public string 加入形態 { get; set; } = string.Empty;

        /// <summary>
        /// 耕種BCP区分
        /// </summary>
        public string koshu_bcp_kbn {  get; set; } = string.Empty;
    }
}
