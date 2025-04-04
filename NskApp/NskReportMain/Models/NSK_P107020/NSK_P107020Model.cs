namespace NskReportMain.Models.NSK_P107020
{
    /// <summary>
    /// NSK_P107070の帳票用モデル
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class NSK_P107020Model
    {
        #region 取得データ

        /// <summary>
        /// 氏名又は法人名フリガナ
        /// </summary>
        public string 氏名又は法人名フリガナ { get; set; }

        /// <summary>
        /// 氏名又は法人名
        /// </summary>
        public string 氏名又は法人名 { get; set; }

        /// <summary>
        /// 郵便番号
        /// </summary>
        public string 郵便番号 { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        public string 住所 { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        public string 電話番号 { get; set; }

        /// <summary>
        /// 加入形態
        /// </summary>
        public string 加入形態 { get; set; }

        /// <summary>
        /// 耕地番号
        /// </summary>
        public string 耕地番号 { get; set; }

        /// <summary>
        /// 分筆番号
        /// </summary>
        public string 分筆番号 { get; set; }

        /// <summary>
        /// 市町村名
        /// </summary>
        public string 市町村名 { get; set; }

        /// <summary>
        /// 地名地番
        /// </summary>
        public string 地名地番 { get; set; }

        /// <summary>
        /// 耕地面積
        /// </summary>
        public Decimal? 耕地面積 { get; set; }

        /// <summary>
        /// 引受面積
        /// </summary>
        public Decimal? 引受面積 { get; set; }

        /// <summary>
        /// 転作等面積
        /// </summary>
        public Decimal? 転作等面積 { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        public string 類区分 { get; set; }

        /// <summary>
        /// 類短縮名称
        /// </summary>
        public string 類短縮名称 { get; set; }

        /// <summary>
        /// 品種コード
        /// </summary>
        public string 品種コード { get; set; }

        /// <summary>
        /// 品種名等
        /// </summary>
        public string 品種名等 { get; set; }

        /// <summary>
        /// 田畑名称
        /// </summary>
        public string 田畑名称 { get; set; }

        /// <summary>
        /// 受委託者コード
        /// </summary>
        public string 受委託者コード { get; set; }

        /// <summary>
        /// 収量等級コード
        /// </summary>
        public string 収量等級コード { get; set; }

        /// <summary>
        /// 参酌係数
        /// </summary>
        public Decimal? 参酌係数 { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        public string 支所コード { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 大地区コード
        /// </summary>
        public string 大地区コード { get; set; }

        /// <summary>
        /// 小地区コード
        /// </summary>
        public string 小地区コード { get; set; }


        #endregion

        #region 出力制御用
        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string 共済目的コード { get; set; }
        #endregion

        #region 帳票出力クラス編集項目

        /// <summary>
        /// 個人
        /// </summary>
        public bool 個人 { get; set; }
        /// <summary>
        /// 法人
        /// </summary>
        public bool 法人 { get; set; }
        /// <summary>
        /// 農作物資格団体
        /// </summary>
        public bool 農作物資格団体 { get; set; }

        /// <summary>
        /// 類区分_表示
        /// </summary>
        public string 類区分_表示 { get; set; }

        /// <summary>
        /// 栽培上の特殊事情
        /// </summary>
        public string 栽培上の特殊事情 { get; set; }

        /// <summary>
        /// 品種又は転作作物名等
        /// </summary>
        public string 品種又は転作作物名等 { get; set; }

        /// <summary>
        /// ページ_右
        /// </summary>
        public string ページ右 { get; set; }

        #endregion
    }
}
