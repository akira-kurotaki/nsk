using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// w_10230_危険段階
    /// </summary>
    [Serializable]
    [Table("w_10230_危険段階")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(合併時識別コード), nameof(類区分), nameof(地域単位区分), nameof(引受方式), nameof(特約区分), nameof(補償割合コード), nameof(危険段階区分))]
    public class W10230危険段階 : ModelBase
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("組合等コード", Order = 1)]
        [StringLength(3)]
        public string 組合等コード { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("年産", Order = 2)]
        public short 年産 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 3)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 合併時識別コード
        /// </summary>
        [Required]
        [Column("合併時識別コード", Order = 4)]
        [StringLength(3)]
        public string 合併時識別コード { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 5)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 地域単位区分
        /// </summary>
        [Required]
        [Column("地域単位区分", Order = 6)]
        [StringLength(5)]
        public string 地域単位区分 { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 7)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 特約区分
        /// </summary>
        [Required]
        [Column("特約区分", Order = 8)]
        [StringLength(1)]
        public string 特約区分 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        [Required]
        [Column("補償割合コード", Order = 9)]
        [StringLength(2)]
        public string 補償割合コード { get; set; }

        /// <summary>
        /// 危険段階区分
        /// </summary>
        [Required]
        [Column("危険段階区分", Order = 10)]
        [StringLength(3)]
        public string 危険段階区分 { get; set; }

        /// <summary>
        /// 危険段階基準共済掛金率
        /// </summary>
        [Column("危険段階基準共済掛金率")]
        public Decimal? 危険段階基準共済掛金率 { get; set; }

        /// <summary>
        /// 危険段階共済掛金率
        /// </summary>
        [Column("危険段階共済掛金率")]
        public Decimal? 危険段階共済掛金率 { get; set; }
    }
}
