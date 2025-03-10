using System.ComponentModel.DataAnnotations.Schema;

namespace NskReportMain.Models.P103040
{
    /// <summary>
    /// P103040の帳票用モデル（明細部）
    /// </summary>
    /// <remarks>
    /// 作成日：2025/3/5
    /// 作成者：NEXT
    /// </remarks>
    public class P103040TableRecord
    {
        /// <summary>
        /// 統計単収地域コード
        /// </summary>
        [Column("統計単収地域コード")]
        public short toukeitansyuChiikiCd { get; set; }

        /// <summary>
        /// 統計単収地域名
        /// </summary>
        [Column("統計単収地域名")]
        public string toukeitansyuChiikMei { get; set; }

        /// <summary>
        /// 統計単収
        /// </summary>
        [Column("統計単収")]
        public string toukeitansyu { get; set; }
    }
}
