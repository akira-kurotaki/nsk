namespace NskReportMain.Models.P1001
{
    /// <summary>
    /// P1001の帳票用モデル（検索条件部）
    /// </summary>
    /// <remarks>
    /// 作成日：2024/10/18
    /// 作成者：YOU EN
    /// </remarks>
    public class P1001SearchCondition
    {
        /// <summary>
        /// 対象年度
        /// </summary>
        public string JoukenNendo { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        public string JoukenTodofukenCd { get; set; }

        /// <summary>
        /// 都道府県名
        /// </summary>
        public string JoukenTodofukenNm { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        public string JoukenKumiaitoCd { get; set; }

        /// <summary>
        /// 組合等名
        /// </summary>
        public string JoukenKumiaitoNm { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        public string JoukenShishoCd { get; set; }

        /// <summary>
        /// 支所名
        /// </summary>
        public string JoukenShishoNm { get; set; }

        /// <summary>
        /// 市町村コード
        /// </summary>
        public string JoukenShichosonCd { get; set; }

        /// <summary>
        /// 市町村名
        /// </summary>
        public string JoukenShichosonNm { get; set; }

        /// <summary>
        /// 大地区コード
        /// </summary>
        public string JoukenDaichikuCd { get; set; }

        /// <summary>
        /// 大地区名
        /// </summary>
        public string JoukenDaichikuNm { get; set; }

        /// <summary>
        /// 小地区コード開始
        /// </summary>
        public string JoukenShochikuCdStart { get; set; }

        /// <summary>
        /// 小地区名開始
        /// </summary>
        public string JoukenShochikuNmStart { get; set; }

        /// <summary>
        /// 小地区コード終了
        /// </summary>
        public string JoukenShochikuCdEnd { get; set; }

        /// <summary>
        /// 小地区名終了
        /// </summary>
        public string JoukenShochikuNmEnd { get; set; }

        /// <summary>
        /// 加入者管理コード（開始）
        /// </summary>
        public string JoukenKanyushaCdStart { get; set; }

        /// <summary>
        /// 加入者管理コード(終了)
        /// </summary>
        public string JoukenKanyushaCdEnd { get; set; }
    }
}
