using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_29010_åÊf[^óü_ªîñOK
    /// </summary>
    [Serializable]
    [Table("t_29010_åÊf[^óü_ªîñOK")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(æª), nameof(gõR[h), nameof(knÔ), nameof(ªMÔ), nameof(íQ»èR[h))]
    public class T29010åÊf[^óüªîñok : ModelBase
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
        /// æª
        /// </summary>
        [Required]
        [Column("æª", Order = 4)]
        [StringLength(1)]
        public string æª { get; set; }

        /// <summary>
        /// gõR[h
        /// </summary>
        [Required]
        [Column("gõR[h", Order = 5)]
        [StringLength(13)]
        public string gõR[h { get; set; }

        /// <summary>
        /// knÔ
        /// </summary>
        [Required]
        [Column("knÔ", Order = 6)]
        [StringLength(5)]
        public string knÔ { get; set; }

        /// <summary>
        /// ªMÔ
        /// </summary>
        [Required]
        [Column("ªMÔ", Order = 7)]
        [StringLength(4)]
        public string ªMÔ { get; set; }

        /// <summary>
        /// íQ»èR[h
        /// </summary>
        [Required]
        [Column("íQ»èR[h", Order = 8)]
        [StringLength(1)]
        public string íQ»èR[h { get; set; }

        /// <summary>
        /// ånæR[h
        /// </summary>
        [Column("ånæR[h")]
        [StringLength(2)]
        public string ånæR[h { get; set; }

        /// <summary>
        /// ¬næR[h
        /// </summary>
        [Column("¬næR[h")]
        [StringLength(4)]
        public string ¬næR[h { get; set; }

        /// <summary>
        /// ªÎÛÊÏ
        /// </summary>
        [Column("ªÎÛÊÏ")]
        public Decimal? ªÎÛÊÏ { get; set; }

        /// <summary>
        /// ªP¸ûÊ_o×
        /// </summary>
        [Column("ªP¸ûÊ_o×")]
        public Decimal? ªP¸ûÊ_o× { get; set; }

        /// <summary>
        /// ªP¸ûÊ_øó
        /// </summary>
        [Column("ªP¸ûÊ_øó")]
        public Decimal? ªP¸ûÊ_øó { get; set; }

        /// <summary>
        /// YnÊÁ¿R[h
        /// </summary>
        [Column("YnÊÁ¿R[h")]
        [StringLength(5)]
        public string YnÊÁ¿R[h { get; set; }

        /// <summary>
        /// õl
        /// </summary>
        [Column("õl")]
        [StringLength(10)]
        public string õl { get; set; }

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
    }
}
