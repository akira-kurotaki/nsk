using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_21180_o×Ê²¸ì îñ_KiÊ
    /// </summary>
    [Serializable]
    [Table("t_21180_o×Ê²¸ì îñ_KiÊ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(o×]¿næR[h), nameof(o×KwæªR[h), nameof(Þæª), nameof(YnÊÁ¿R[h), nameof(c_ÎÛOtO), nameof(Ki))]
    public class T21180o×Ê²¸ì îñKiÊ : ModelBase
    {
        /// <summary>
        /// gR[h
        /// </summary>
        [Required]
        [Column("gR[h", Order = 1)]
        [StringLength(3)]
        public string gR[h { get; set; }

        /// <summary>
        /// NY
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("NY", Order = 2)]
        public short NY { get; set; }

        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Required]
        [Column("¤ÏÚIR[h", Order = 3)]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// gõR[h
        /// </summary>
        [Required]
        [Column("gõR[h", Order = 4)]
        [StringLength(13)]
        public string gõR[h { get; set; }

        /// <summary>
        /// o×]¿næR[h
        /// </summary>
        [Required]
        [Column("o×]¿næR[h", Order = 5)]
        [StringLength(4)]
        public string o×]¿næR[h { get; set; }

        /// <summary>
        /// o×KwæªR[h
        /// </summary>
        [Required]
        [Column("o×KwæªR[h", Order = 6)]
        [StringLength(3)]
        public string o×KwæªR[h { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 7)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// YnÊÁ¿R[h
        /// </summary>
        [Required]
        [Column("YnÊÁ¿R[h", Order = 8)]
        [StringLength(5)]
        public string YnÊÁ¿R[h { get; set; }

        /// <summary>
        /// c_ÎÛOtO
        /// </summary>
        [Required]
        [Column("c_ÎÛOtO", Order = 9)]
        [StringLength(1)]
        public string c_ÎÛOtO { get; set; }

        /// <summary>
        /// Ki
        /// </summary>
        [Required]
        [Column("Ki", Order = 10)]
        [StringLength(2)]
        public string Ki { get; set; }

        /// <summary>
        /// o×Ê
        /// </summary>
        [Column("o×Ê")]
        public Decimal? o×Ê { get; set; }

        /// <summary>
        /// o^ú
        /// </summary>
        [Column("o^ú")]
        public DateTime? o^ú { get; set; }

        /// <summary>
        /// o^[Uid
        /// </summary>
        [Column("o^[Uid")]
        [StringLength(11)]
        public string o^[Uid { get; set; }

        /// <summary>
        /// XVú
        /// </summary>
        [Column("XVú")]
        public DateTime? XVú { get; set; }

        /// <summary>
        /// XV[Uid
        /// </summary>
        [Column("XV[Uid")]
        [StringLength(11)]
        public string XV[Uid { get; set; }
    }
}
