using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00120_¿¦
    /// </summary>
    [Serializable]
    [Table("m_00120_¿¦")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(¹¯ÊR[h), nameof(Þæª), nameof(øóû®), nameof(Áñæª), nameof(âR[h), nameof(vPÊnæR[h))]
    public class M00120¿¦ : ModelBase
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
        /// ¹¯ÊR[h
        /// </summary>
        [Required]
        [Column("¹¯ÊR[h", Order = 4)]
        [StringLength(3)]
        public string ¹¯ÊR[h { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 5)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// øóû®
        /// </summary>
        [Required]
        [Column("øóû®", Order = 6)]
        [StringLength(1)]
        public string øóû® { get; set; }

        /// <summary>
        /// Áñæª
        /// </summary>
        [Required]
        [Column("Áñæª", Order = 7)]
        [StringLength(1)]
        public string Áñæª { get; set; }

        /// <summary>
        /// âR[h
        /// </summary>
        [Required]
        [Column("âR[h", Order = 8)]
        [StringLength(2)]
        public string âR[h { get; set; }

        /// <summary>
        /// vPÊnæR[h
        /// </summary>
        [Required]
        [Column("vPÊnæR[h", Order = 9)]
        [StringLength(5)]
        public string vPÊnæR[h { get; set; }

        /// <summary>
        /// ÓCÛ¯à
        /// </summary>
        [Column("ÓCÛ¯à")]
        public Decimal? ÓCÛ¯à { get; set; }

        /// <summary>
        /// ¤Ï|àW¦
        /// </summary>
        [Column("¤Ï|àW¦")]
        public Decimal? ¤Ï|àW¦ { get; set; }

        /// <summary>
        /// ÊíWíQ¦
        /// </summary>
        [Column("ÊíWíQ¦")]
        public Decimal? ÊíWíQ¦ { get; set; }

        /// <summary>
        /// Û¯¿îb¦
        /// </summary>
        [Column("Û¯¿îb¦")]
        public Decimal? Û¯¿îb¦ { get; set; }

        /// <summary>
        /// ÙíWíQ¦
        /// </summary>
        [Column("ÙíWíQ¦")]
        public Decimal? ÙíWíQ¦ { get; set; }

        /// <summary>
        /// ÄÛ¯¿îb¦
        /// </summary>
        [Column("ÄÛ¯¿îb¦")]
        public Decimal? ÄÛ¯¿îb¦ { get; set; }

        /// <summary>
        /// ÉS
        /// </summary>
        [Column("ÉS")]
        public Decimal? ÉS { get; set; }

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
