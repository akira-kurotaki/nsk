namespace NSK_P109010.Models
{
    /// <summary>
    /// 帳票の出力対象データ（SUB2レポート用）
    /// </summary>
    public class TyouhyouDataSub2
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        public string 組合等コード { get; set; } = string.Empty;

        /// <summary>
        /// 共済目的コード
        /// </summary>
        public string 共済目的コード { get; set; } = string.Empty;

        /// <summary>
        /// 年産
        /// </summary>
        public int 年産 { get; set; }

        /// <summary>
        /// 引受回
        /// </summary>
        public int 引受回 { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        public string 類区分 { get; set; } = string.Empty;

        /// <summary>
        /// 類短縮名称
        /// </summary>
        public string 類短縮名称 { get; set; } = string.Empty;

        /// <summary>
        /// 統計単位地域コード
        /// </summary>
        public string 統計単位地域コード { get; set; } = string.Empty;

        /// <summary>
        /// 加入申込区分
        /// </summary>
        public string 加入申込区分 { get; set; } = string.Empty;

        /// <summary>
        /// seqnum
        /// </summary>
        public int seqnum { get; set; }
    }
}
