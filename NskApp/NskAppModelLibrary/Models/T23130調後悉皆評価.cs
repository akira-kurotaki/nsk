using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_23130_²ã»F]¿
    /// </summary>
    [Serializable]
    [Table("t_23130_²ã»F]¿")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(knÔ), nameof(ªMÔ), nameof(præª))]
    public class T23130²ã»F]¿ : ModelBase
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
        /// knÔ
        /// </summary>
        [Required]
        [Column("knÔ", Order = 5)]
        [StringLength(5)]
        public string knÔ { get; set; }

        /// <summary>
        /// ªMÔ
        /// </summary>
        [Required]
        [Column("ªMÔ", Order = 6)]
        [StringLength(4)]
        public string ªMÔ { get; set; }

        /// <summary>
        /// præª
        /// </summary>
        [Required]
        [Column("præª", Order = 7)]
        [StringLength(3)]
        public string præª { get; set; }

        /// <summary>
        /// ª¸ûÊ
        /// </summary>
        [Column("ª¸ûÊ")]
        public Decimal? ª¸ûÊ { get; set; }

        /// <summary>
        /// PC³Ê
        /// </summary>
        [Column("PC³Ê")]
        public Decimal? PC³Ê { get; set; }

        /// <summary>
        /// g]¿Pû
        /// </summary>
        [Column("g]¿Pû")]
        public Decimal? g]¿Pû { get; set; }

        /// <summary>
        /// g]¿ûÊ
        /// </summary>
        [Column("g]¿ûÊ")]
        public Decimal? g]¿ûÊ { get; set; }

        /// <summary>
        /// ûnÊ
        /// </summary>
        [Column("ûnÊ")]
        public Decimal? ûnÊ { get; set; }

        /// <summary>
        /// ¸ûÊ
        /// </summary>
        [Column("¸ûÊ")]
        public Decimal? ¸ûÊ { get; set; }

        /// <summary>
        /// ¤Ï¸ûÊ
        /// </summary>
        [Column("¤Ï¸ûÊ")]
        public Decimal? ¤Ï¸ûÊ { get; set; }

        /// <summary>
        /// êMS¼¸ûÊ
        /// </summary>
        [Column("êMS¼¸ûÊ")]
        public Decimal? êMS¼¸ûÊ { get; set; }

        /// <summary>
        /// vZút
        /// </summary>
        [Column("vZút")]
        public DateTime? vZút { get; set; }

        /// <summary>
        /// ¤Ïà
        /// </summary>
        [Column("¤Ïà")]
        public Decimal? ¤Ïà { get; set; }

        /// <summary>
        /// x¥¤Ïà
        /// </summary>
        [Column("x¥¤Ïà")]
        public Decimal? x¥¤Ïà { get; set; }

        /// <summary>
        /// â³Ê
        /// </summary>
        [Column("â³Ê")]
        public Decimal? â³Ê { get; set; }

        /// <summary>
        /// KpP¤Ïàz
        /// </summary>
        [Column("KpP¤Ïàz")]
        public Decimal? KpP¤Ïàz { get; set; }

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
