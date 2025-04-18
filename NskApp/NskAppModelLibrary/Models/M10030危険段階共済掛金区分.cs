using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10030_ë¯iK¤Ï|àæª
    /// </summary>
    [Serializable]
    [Table("m_10030_ë¯iK¤Ï|àæª")]
    [PrimaryKey(nameof(¤ÏÆë¯iK), nameof(¤ÏÚIë¯iK), nameof(ë¯iK¤Ï|àæª))]
    public class M10030ë¯iK¤Ï|àæª : ModelBase
    {
        /// <summary>
        /// ¤ÏÆë¯iK
        /// </summary>
        [Required]
        [Column("¤ÏÆë¯iK", Order = 1)]
        [StringLength(2)]
        public string ¤ÏÆë¯iK { get; set; }

        /// <summary>
        /// ¤ÏÚIë¯iK
        /// </summary>
        [Required]
        [Column("¤ÏÚIë¯iK", Order = 2)]
        [StringLength(2)]
        public string ¤ÏÚIë¯iK { get; set; }

        /// <summary>
        /// ë¯iK¤Ï|àæª
        /// </summary>
        [Required]
        [Column("ë¯iK¤Ï|àæª", Order = 3)]
        [StringLength(16)]
        public string ë¯iK¤Ï|àæª { get; set; }

        /// <summary>
        /// ë¯iK¤Ï|àæª¼Ì
        /// </summary>
        [Column("ë¯iK¤Ï|àæª¼Ì")]
        [StringLength(60)]
        public string ë¯iK¤Ï|àæª¼Ì { get; set; }

        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Column("¤ÏÚIR[h")]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// øóû®
        /// </summary>
        [Column("øóû®")]
        [StringLength(1)]
        public string øóû® { get; set; }

        /// <summary>
        /// Áñæª
        /// </summary>
        [Column("Áñæª")]
        [StringLength(1)]
        public string Áñæª { get; set; }

        /// <summary>
        /// âR[h
        /// </summary>
        [Column("âR[h")]
        [StringLength(2)]
        public string âR[h { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Column("Þæª")]
        [StringLength(2)]
        public string Þæª { get; set; }

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
