using GrapeCity.DataVisualization.TypeScript;
using System.ComponentModel.DataAnnotations.Schema;

namespace NSK_P103040.Models.P103040
{
    /// <summary>
    /// P10304の帳票用モデル（ヘッダー部）
    /// </summary>
    /// <remarks>
    /// 作成日：2025/3/6
    /// 作成者：NEXT
    /// </remarks>
    public class HeaderModel
    {
        /// <summary>
        /// 共済目的名称
        /// </summary>
        [Column("共済目的名称")]
        public string KyosaiMokutekiNm { get; set; } = string.Empty;

        /// <summary>
        /// 類区分
        /// </summary>
        [Column("類区分")]
        public string RuiKbn { get; set; } = string.Empty;

        /// <summary>
        /// 類名称
        /// </summary>
        [Column("類名称")]
        public string ruiKbnNm { get; set; } = string.Empty;
    }
}

