using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12100_îPûC³Ê
    /// </summary>
    [Serializable]
    [Table("t_12100_îPûC³Ê")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(Þæª), nameof(xR[h))]
    public class T12100îPûC³Ê : ModelBase
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
        /// xR[h
        /// </summary>
        [Required]
        [Column("xR[h", Order = 5)]
        [StringLength(2)]
        public string xR[h { get; set; }

        /// <summary>
        /// Kpøóñ
        /// </summary>
        [Column("Kpøóñ")]
        public short? Kpøóñ { get; set; }

        /// <summary>
        /// îC³Ê
        /// </summary>
        [Column("îC³Ê")]
        public Decimal? îC³Ê { get; set; }

        /// <summary>
        /// OñC³Ê
        /// </summary>
        [Column("OñC³Ê")]
        public Decimal? OñC³Ê { get; set; }

        /// <summary>
        /// C³Ê
        /// </summary>
        [Column("C³Ê")]
        public Decimal? C³Ê { get; set; }

        /// <summary>
        /// îPûC³ÊvZÀ{ú
        /// </summary>
        [Column("îPûC³ÊvZÀ{ú")]
        public DateTime? îPûC³ÊvZÀ{ú { get; set; }

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
