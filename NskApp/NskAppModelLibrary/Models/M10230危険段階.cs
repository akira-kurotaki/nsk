using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10230_ë¯iK
    /// </summary>
    [Serializable]
    [Table("m_10230_ë¯iK")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(¹¯ÊR[h), nameof(Þæª), nameof(næPÊæª), nameof(øóû®), nameof(Áñæª), nameof(âR[h), nameof(ë¯iKæª))]
    public class M10230ë¯iK : ModelBase
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
        /// næPÊæª
        /// </summary>
        [Required]
        [Column("næPÊæª", Order = 6)]
        [StringLength(5)]
        public string næPÊæª { get; set; }

        /// <summary>
        /// øóû®
        /// </summary>
        [Required]
        [Column("øóû®", Order = 7)]
        [StringLength(1)]
        public string øóû® { get; set; }

        /// <summary>
        /// Áñæª
        /// </summary>
        [Required]
        [Column("Áñæª", Order = 8)]
        [StringLength(1)]
        public string Áñæª { get; set; }

        /// <summary>
        /// âR[h
        /// </summary>
        [Required]
        [Column("âR[h", Order = 9)]
        [StringLength(2)]
        public string âR[h { get; set; }

        /// <summary>
        /// ë¯iKæª
        /// </summary>
        [Required]
        [Column("ë¯iKæª", Order = 10)]
        [StringLength(3)]
        public string ë¯iKæª { get; set; }

        /// <summary>
        /// ë¯iKî¤Ï|à¦
        /// </summary>
        [Column("ë¯iKî¤Ï|à¦")]
        public Decimal? ë¯iKî¤Ï|à¦ { get; set; }

        /// <summary>
        /// ë¯iK¤Ï|à¦
        /// </summary>
        [Column("ë¯iK¤Ï|à¦")]
        public Decimal? ë¯iK¤Ï|à¦ { get; set; }

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
