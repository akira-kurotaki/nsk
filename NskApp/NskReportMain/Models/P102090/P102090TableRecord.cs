using System.ComponentModel.DataAnnotations.Schema;

namespace NskReportMain.Models.P102090
{
    /// <summary>
    /// P102090の帳票用モデル
    /// </summary>
    /// <remarks>
    /// 作成日：2025/3/5
    /// 作成者：NEXT
    /// </remarks>
    public class P102090TableRecord
    {
        /// <summary>
        /// 共済目的名称
        /// </summary>
        [Column("共済目的名称")]
        public string kyousaimokuteki { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        public string ruikbn { get; set; }

        /// <summary>
        /// 類名称
        /// </summary>
        public string ruiName { get; set; }

        /// <summary>
        /// 大地区コード
        /// </summary>
        public string daichikuCdVal { get; set; }

        /// <summary>
        /// 大地区名
        /// </summary>
        public string daichikuNameVal { get; set; }

        /// <summary>
        /// 小地区コード
        /// </summary>
        public string shouchikuCdVal { get; set; }

        /// <summary>
        /// 小地区名
        /// </summary>
        public string shouchikuNameVal { get; set; }

        /// <summary>
        /// 一筆7割
        /// </summary>
        public string ippitsu7wariVal { get; set; }

        /// <summary>
        /// 一筆6割
        /// </summary>
        public string ippitsu6wariVal { get; set; }

        /// <summary>
        /// 一筆5割
        /// </summary>
        public string ippitsu5wariVal { get; set; }

        /// <summary>
        /// 半相殺・一般8割
        /// </summary>
        public string hanIppan8wari { get; set; }

        /// <summary>
        /// 半相殺・一般7割
        /// </summary>
        public string hanIppan7wari { get; set; }

        /// <summary>
        /// 半相殺・一般6割
        /// </summary>
        public string hanIppan6wari { get; set; }

        /// <summary>
        /// 半相殺・半損特約8割
        /// </summary>
        public string hanHanToku8wari { get; set; }

        /// <summary>
        /// 半相殺・半損特約7割
        /// </summary>
        public string hanHanToku7wari { get; set; }

        /// <summary>
        /// 半相殺・半損特約6割
        /// </summary>
        public string hanHanToku6wari { get; set; }

        /// <summary>
        /// 全相殺・一般9割
        /// </summary>
        public string zenIppan9wari { get; set; }

        /// <summary>
        /// 全相殺・一般8割
        /// </summary>
        public string zenIppan8wari { get; set; }

        /// <summary>
        /// 全相殺・一般7割
        /// </summary>
        public string zenIppan7wari { get; set; }

        /// <summary>
        /// 全相殺・半損特約9割
        /// </summary>
        public string zenHanToku9wari { get; set; }

        /// <summary>
        /// 全相殺・半損特約8割
        /// </summary>
        public string zenHanToku8wari { get; set; }

        /// <summary>
        /// 全相殺・半損特約7割
        /// </summary>
        public string zenHanToku7wari { get; set; }
    }
}
