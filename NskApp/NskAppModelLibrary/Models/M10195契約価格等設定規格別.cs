using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10195__ñ¿iÝè_KiÊ
    /// </summary>
    [Serializable]
    [Table("m_10195__ñ¿iÝè_KiÊ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(Þæª), nameof(YnÊÁ¿R[h), nameof(c_ÎÛOtO), nameof(S¢èæª), nameof(Ki))]
    public class M10195_ñ¿iÝèKiÊ : ModelBase
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
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 4)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// YnÊÁ¿R[h
        /// </summary>
        [Required]
        [Column("YnÊÁ¿R[h", Order = 5)]
        [StringLength(5)]
        public string YnÊÁ¿R[h { get; set; }

        /// <summary>
        /// c_ÎÛOtO
        /// </summary>
        [Required]
        [Column("c_ÎÛOtO", Order = 6)]
        [StringLength(1)]
        public string c_ÎÛOtO { get; set; }

        /// <summary>
        /// S¢èæª
        /// </summary>
        [Required]
        [Column("S¢èæª", Order = 7)]
        [StringLength(1)]
        public string S¢èæª { get; set; }

        /// <summary>
        /// Ki
        /// </summary>
        [Required]
        [Column("Ki", Order = 8)]
        [StringLength(2)]
        public string Ki { get; set; }

        /// <summary>
        /// _ñ¿i
        /// </summary>
        [Column("_ñ¿i")]
        public Decimal? _ñ¿i { get; set; }

        /// <summary>
        /// i¿w
        /// </summary>
        [Column("i¿w")]
        public Decimal? i¿w { get; set; }

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
