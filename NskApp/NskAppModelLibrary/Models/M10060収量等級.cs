using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10060_ûÊ
    /// </summary>
    [Serializable]
    [Table("m_10060_ûÊ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(Þæª), nameof(xR[h), nameof(ûÊR[h))]
    public class M10060ûÊ : ModelBase
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
        /// íÞæª
        /// </summary>
        [Column("íÞæª")]
        [StringLength(1)]
        public string íÞæª { get; set; }

        /// <summary>
        /// ìtú
        /// </summary>
        [Column("ìtú")]
        [StringLength(1)]
        public string ìtú { get; set; }

        /// <summary>
        /// xR[h
        /// </summary>
        [Required]
        [Column("xR[h", Order = 5)]
        [StringLength(2)]
        public string xR[h { get; set; }

        /// <summary>
        /// ûÊR[h
        /// </summary>
        [Required]
        [Column("ûÊR[h", Order = 6)]
        [StringLength(3)]
        public string ûÊR[h { get; set; }

        /// <summary>
        /// ûÊ
        /// </summary>
        [Column("ûÊ")]
        [StringLength(4)]
        public string ûÊ { get; set; }

        /// <summary>
        /// vZPû
        /// </summary>
        [Column("vZPû")]
        public Decimal? vZPû { get; set; }

        /// <summary>
        /// KpPû
        /// </summary>
        [Column("KpPû")]
        public Decimal? KpPû { get; set; }

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
