namespace NSK_P109010.Models
{
    /// <summary>
    /// 帳票の出力対象データ
    /// </summary>
    public class NSK_109010PModel
    {
        /// <summary>
        /// 帳票の出力対象データ（メインレポート用）
        /// </summary>
        public required List<TyouhyouDataMain> TyouhyouDatasMain { get; set; }

        /// <summary>
        /// 帳票の出力対象データ（SUB1レポート用）
        /// </summary>
        public required List<TyouhyouDataSub1> TyouhyouDatasSub1 { get; set; }

        /// <summary>
        /// 帳票の出力対象データ（SUB2レポート用）
        /// </summary>
        public required List<TyouhyouDataSub2> TyouhyouDatasSub2 { get; set; }
    }
}
