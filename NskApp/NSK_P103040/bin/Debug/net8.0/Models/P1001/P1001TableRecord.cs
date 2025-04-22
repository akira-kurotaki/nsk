namespace NskReportMain.Models.P1001
{
    /// <summary>
    /// P1001の帳票用モデル（明細部）
    /// </summary>
    /// <remarks>
    /// 作成日：2024/10/18
    /// 作成者：YOU EN
    /// </remarks>
    public class P1001TableRecord
    {
        /// <summary>
        /// 対象年度（DB値）
        /// </summary>
        public short Nendo { get; set; }

        /// <summary>
        /// 対象年度
        /// </summary>
        public string NendoDisp3 { get; set; }

        /// <summary>
        /// 加入者管理コード   
        /// </summary>
        public string KanyushaCd { get; set; }

        /// <summary>
        /// 氏名又は法人名  
        /// </summary>
        public string HojinFullNm { get; set; }

        /// <summary>
        /// 耕地住所
        /// </summary>
        public string KochiAddress { get; set; }
    }
}
