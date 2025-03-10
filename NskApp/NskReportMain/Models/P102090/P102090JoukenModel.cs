namespace NskReportMain.Models.P102090
{
    /// <summary>
    /// P102090の帳票用モデル（条件部）
    /// </summary>
    /// <remarks>
    /// 作成日：2025/3/6
    /// 作成者：NEXT
    /// </remarks>
    public class P102090JoukenModel
    {
        /// <summary>
        /// 年産
        /// </summary>
        public string nensan { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string kyosaiMokutekiCd { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        public string ruikbn { get; set; }

        /// <summary>
        /// 大地区コード
        /// </summary>
        public string daichiku { get; set; }

        /// <summary>
        /// 小地区コード（開始）
        /// </summary>
        public string shochikuStart { get; set; }

        /// <summary>
        /// 小地区コード（終了）
        /// </summary>
        public string shochikuEnd { get; set; }
    }
}
