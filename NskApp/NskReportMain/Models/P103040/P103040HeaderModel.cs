namespace NskReportMain.Models.P103040
{
    /// <summary>
    /// P10304の帳票用モデル（ヘッダー部）
    /// </summary>
    /// <remarks>
    /// 作成日：2025/3/6
    /// 作成者：NEXT
    /// </remarks>
    public class P103040HeaderModel
    {
        /// <summary>
		/// 年産（令和〇〇年産）
		/// </summary>
		public string 年産 { get; set; }

        /// <summary>
        /// 共済目的名称
        /// </summary>
        public string 共済目的名称 { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        public string 類区分 { get; set; }

        /// <summary>
        /// 類名称
        /// </summary>
        public string 類名称 { get; set; }
    }
}

