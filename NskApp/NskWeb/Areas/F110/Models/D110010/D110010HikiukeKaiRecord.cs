namespace NskWeb.Areas.F110.Models.D110010
{
    /// <summary>
    /// 引受回レコード
    /// </summary>
    public class D110010HikiukeKaiRecord
    {
        /// <summary>支所コード</summary>
        public string ShishoCd { get; set; } = string.Empty;
        /// <summary>支所名</summary>
        public string ShishoNm { get; set; } = string.Empty;
        /// <summary>引受回</summary>
        public decimal? HikiukeKai { get; set; }
        /// <summary>引受計算実行日</summary>
        public DateTime? HikiukeKeisanJikkobi { get; set; }
        /// <summary>報告回</summary>
        public decimal? HokokuKai { get; set; }
        /// <summary>報告日付</summary>
        public DateTime? HokokuHiduke { get; set; }
        /// <summary>確定引受回</summary>
        public decimal? KakuteiHikiukeKai { get; set; }

        /// <summary>xmin</summary>
        public uint? Xmin { get; set; }
    }
}