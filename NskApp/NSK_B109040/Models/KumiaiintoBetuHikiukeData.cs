namespace NSK_B109040.Models
{
    public class KumiaiintoBetuHikiukeData
    {
        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string 共済目的コード { get; set; } = string.Empty;

        /// <summary>
        /// 年産
        /// </summary>
        public int 年産 { get; set; }

        /// <summary>
        /// </summary>組合等コード
        /// </summary>
        public string 組合等コード { get; set; } = string.Empty;

        /// <summary>
        /// 支所コード
        /// </summary>
        public string 支所コード { get; set; } = string.Empty;

        /// <summary>
        /// 引受回
        /// </summary>
        public int 引受回 { get; set; }

        /// <summary>
        /// 引受面積区分
        /// </summary>
        public int 引受面積区分 { get; set; }

        /// <summary>
        /// 引受戸数計
        /// </summary>
        public int 引受戸数計 { get; set; }

        /// <summary>
        /// 引受面積計
        /// </summary>
        public decimal 引受面積計 { get; set; }

        /// <summary>
        /// 組合等略称
        /// </summary>
        public string 組合等略称 { get; set; } = string.Empty;

        /// <summary>
        /// 大地区コード
        /// </summary>
        // todo: 設計書修正完了次第修正　No.１４４
        public string 大地区コード { get; set; } = string.Empty;
    }
}
