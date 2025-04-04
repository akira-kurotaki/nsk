using System;

namespace NSK_P109010.Models
{
    /// <summary>
    /// 帳票の出力対象データ（SUB1レポート用）
    /// </summary>
    public class TyouhyouDataSub1
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
        /// 組合員等コード
        /// </summary>
        public string 組合員等コード { get; set; } = string.Empty;

        /// <summary>
        /// 特約区分
        /// </summary>
        public string 特約区分 { get; set; } = string.Empty;

        /// <summary>
        /// 類短縮名称
        /// </summary>
        public string 類短縮名称 { get; set; } = string.Empty;

        /// <summary>
        /// 引受方式
        /// </summary>
        public string 引受方式 { get; set; } = string.Empty;

        /// <summary>
        /// 補償割合
        /// </summary>
        public int 補償割合 { get; set; }

        /// <summary>
        /// 一筆半損特約の有無
        /// </summary>
        public string 一筆半損特約の有無 { get; set; } = string.Empty;

        /// <summary>
        /// 選択順位
        /// </summary>
        public decimal 選択順位 { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        public string 備考 { get; set; } = string.Empty;

        /// <summary>
        /// seqnum
        /// </summary>
        public int seqnum { get; set; }
    }
}
