using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_21110_{ÝS²¸
    /// </summary>
    [Serializable]
    [Table("t_21110_{ÝS²¸")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(Þæª), nameof(præª), nameof(gõR[h))]
    public class T21110{ÝS²¸ : ModelBase
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
        /// præª
        /// </summary>
        [Required]
        [Column("præª", Order = 5)]
        [StringLength(3)]
        public string præª { get; set; }

        /// <summary>
        /// gõR[h
        /// </summary>
        [Required]
        [Column("gõR[h", Order = 6)]
        [StringLength(13)]
        public string gõR[h { get; set; }

        /// <summary>
        /// {ÝÀüûnÊ
        /// </summary>
        [Column("{ÝÀüûnÊ")]
        public Decimal? {ÝÀüûnÊ { get; set; }

        /// <summary>
        /// ª¸ûÊ
        /// </summary>
        [Column("ª¸ûÊ")]
        public Decimal? ª¸ûÊ { get; set; }

        /// <summary>
        /// F³ÊÏ
        /// </summary>
        [Column("F³ÊÏ")]
        public Decimal? F³ÊÏ { get; set; }

        /// <summary>
        /// F³îûnÊ
        /// </summary>
        [Column("F³îûnÊ")]
        public Decimal? F³îûnÊ { get; set; }

        /// <summary>
        /// s\ÊÏ
        /// </summary>
        [Column("s\ÊÏ")]
        public Decimal? s\ÊÏ { get; set; }

        /// <summary>
        /// s\îûnÊ
        /// </summary>
        [Column("s\îûnÊ")]
        public Decimal? s\îûnÊ { get; set; }

        /// <summary>
        /// s\ûnÊ
        /// </summary>
        [Column("s\ûnÊ")]
        public Decimal? s\ûnÊ { get; set; }

        /// <summary>
        /// ]ìÊÏ
        /// </summary>
        [Column("]ìÊÏ")]
        public Decimal? ]ìÊÏ { get; set; }

        /// <summary>
        /// ]ìîûnÊ
        /// </summary>
        [Column("]ìîûnÊ")]
        public Decimal? ]ìîûnÊ { get; set; }

        /// <summary>
        /// ]ìûnÊ
        /// </summary>
        [Column("]ìûnÊ")]
        public Decimal? ]ìûnÊ { get; set; }

        /// <summary>
        /// prÊîûnÊ
        /// </summary>
        [Column("prÊîûnÊ")]
        public Decimal? prÊîûnÊ { get; set; }

        /// <summary>
        /// s\knÀüûnÊ
        /// </summary>
        [Column("s\knÀüûnÊ")]
        public Decimal? s\knÀüûnÊ { get; set; }

        /// <summary>
        /// s\knÀüûnÊâ³Ê
        /// </summary>
        [Column("s\knÀüûnÊâ³Ê")]
        public Decimal? s\knÀüûnÊâ³Ê { get; set; }

        /// <summary>
        /// gõûnÊâ³Ê
        /// </summary>
        [Column("gõûnÊâ³Ê")]
        public Decimal? gõûnÊâ³Ê { get; set; }

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
