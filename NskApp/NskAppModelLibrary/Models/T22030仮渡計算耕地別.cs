using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_22030_¼nvZknÊ
    /// </summary>
    [Serializable]
    [Table("t_22030_¼nvZknÊ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(Þæª), nameof(gõR[h), nameof(knÔ), nameof(ªMÔ), nameof(¼nñ))]
    public class T22030¼nvZknÊ : ModelBase
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
        /// ¼nñ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("¼nñ", Order = 8)]
        public short ¼nñ { get; set; }

        /// <summary>
        /// præª
        /// </summary>
        [Column("præª")]
        [StringLength(3)]
        public string præª { get; set; }

        /// <summary>
        /// íQ»èR[h
        /// </summary>
        [Column("íQ»èR[h")]
        [StringLength(1)]
        public string íQ»èR[h { get; set; }

        /// <summary>
        /// êMS¼»è
        /// </summary>
        [Column("êMS¼»è")]
        [StringLength(1)]
        public string êMS¼»è { get; set; }

        /// <summary>
        /// ª¸ûÊ
        /// </summary>
        [Column("ª¸ûÊ")]
        public Decimal? ª¸ûÊ { get; set; }

        /// <summary>
        /// ûnÊ
        /// </summary>
        [Column("ûnÊ")]
        public Decimal? ûnÊ { get; set; }

        /// <summary>
        /// x¥Jn¸ûÊ
        /// </summary>
        [Column("x¥Jn¸ûÊ")]
        public Decimal? x¥Jn¸ûÊ { get; set; }

        /// <summary>
        /// ¸ûÊ
        /// </summary>
        [Column("¸ûÊ")]
        public Decimal? ¸ûÊ { get; set; }

        /// <summary>
        /// Áá¸ûÊ
        /// </summary>
        [Column("Áá¸ûÊ")]
        public Decimal? Áá¸ûÊ { get; set; }

        /// <summary>
        /// ¤Ï¸ûÊ
        /// </summary>
        [Column("¤Ï¸ûÊ")]
        public Decimal? ¤Ï¸ûÊ { get; set; }

        /// <summary>
        /// ¤Ïà
        /// </summary>
        [Column("¤Ïà")]
        public Decimal? ¤Ïà { get; set; }

        /// <summary>
        /// vPÊnæR[h
        /// </summary>
        [Column("vPÊnæR[h")]
        [StringLength(5)]
        public string vPÊnæR[h { get; set; }

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
