namespace NskReportMain.Models.P1002
{
    /// <summary>
    /// P1002の帳票用モデル
    /// </summary>
    /// <remarks>
    /// 作成日：2024/10/23
    /// 作成者：YOU EN
    /// </remarks>
    public class P1002Model
    {
        /// <summary>
        /// 年度
        /// </summary>
        public short? Nendo { get; set; }

        /// <summary>
        /// 年度（和暦変換）
        /// </summary>
        public string NendoJa{ get; set; }

        /// <summary>
        /// 加入申請年月日
        /// </summary>
        public DateTime? KanyuShinseiYmd { get; set; }

        /// <summary>
        /// 加入申請年月日の年（YYYY）を和暦変換
        /// </summary>
        public string KanyuShinseiYmdJaYY { get; set; }

        /// <summary>
        /// 加入申請年月日の月（MM）
        /// </summary>
        public string KanyuShinseiYmdMM { get; set; }

        /// <summary>
        /// 加入申請年月日の日（DD）
        /// </summary>
        public string KanyuShinseiYmdDD { get; set; }

        /// <summary>
        /// 耕地住所
        /// </summary>
        public string KochiAddress { get; set; }

        /// <summary>
        /// 氏名又は法人名
        /// </summary>
        public string HojinFullNm { get; set; }

        /// <summary>
        /// 加入者管理コード
        /// </summary>
        public string KanyushaCd { get; set; }

        /// <summary>
        /// 都道府県名
        /// </summary>
        public string TodofukenNm { get; set; }

        /// <summary>
        /// 組合等名
        /// </summary>
        public string KumiaitoNm { get; set; }

        /// <summary>
        /// 支所名
        /// </summary>
        public string ShishoNm { get; set; }

        /// <summary>
        /// 市町村名
        /// </summary>
        public string ShichosonNm { get; set; }

        /// <summary>
        /// 大地区名
        /// </summary>
        public string DaichikuNm { get; set; }

        /// <summary>
        /// 小地区名
        /// </summary>
        public string ShochikuNm { get; set; }

        /// <summary>
        /// 耕地面積
        /// </summary>
        public decimal? KouchiMenseki { get; set; }

        /// <summary>
        /// 耕地形態(畑)チェック
        /// </summary>
        public bool IsKouchiKeitaiCdHata { get; set; }

        /// <summary>
        /// 耕地形態(田)チェック
        /// </summary>
        public bool IsKouchiKeitaiCdTa { get; set; }

        /// <summary>
        /// 耕地形態(その他)チェック
        /// </summary>
        public bool IsKouchiKeitaiCdOther { get; set; }

        /// <summary>
        /// 耕地形態(未選択)チェック
        /// </summary>
        public bool IsKouchiKeitaiCdNone { get; set; }

        /// <summary>
        /// 個人情報の取扱いについてチェック
        /// </summary>
        public bool KojinjohoToriatsukaiFlg { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        public string Biko { get; set; }
    }
}
