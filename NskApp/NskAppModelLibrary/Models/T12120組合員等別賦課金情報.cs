using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12120_gõÊÛàîñ
    /// </summary>
    [Serializable]
    [Table("t_12120_gõÊÛàîñ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(øóñ), nameof(gõR[h), nameof(Þæª), nameof(Ûàøóû®), nameof(Áñæª))]
    public class T12120gõÊÛàîñ : ModelBase
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
        /// øóñ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("øóñ", Order = 4)]
        public short øóñ { get; set; }

        /// <summary>
        /// gõR[h
        /// </summary>
        [Required]
        [Column("gõR[h", Order = 5)]
        [StringLength(13)]
        public string gõR[h { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 6)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// Ûàøóû®
        /// </summary>
        [Required]
        [Column("Ûàøóû®", Order = 7)]
        [StringLength(2)]
        public string Ûàøóû® { get; set; }

        /// <summary>
        /// Áñæª
        /// </summary>
        [Required]
        [Column("Áñæª", Order = 8)]
        [StringLength(1)]
        public string Áñæª { get; set; }

        /// <summary>
        /// ånæR[h
        /// </summary>
        [Column("ånæR[h")]
        [StringLength(2)]
        public string ånæR[h { get; set; }

        /// <summary>
        /// êÊÛà
        /// </summary>
        [Column("êÊÛà")]
        public Decimal? êÊÛà { get; set; }

        /// <summary>
        /// hÐÛà
        /// </summary>
        [Column("hÐÛà")]
        public Decimal? hÐÛà { get; set; }

        /// <summary>
        /// ÁÊÛà
        /// </summary>
        [Column("ÁÊÛà")]
        public Decimal? ÁÊÛà { get; set; }

        /// <summary>
        /// hÐP¿
        /// </summary>
        [Column("hÐP¿")]
        public Decimal? hÐP¿ { get; set; }

        /// <summary>
        /// ÁÊP¿
        /// </summary>
        [Column("ÁÊP¿")]
        public Decimal? ÁÊP¿ { get; set; }

        /// <summary>
        /// ÛàãÀz
        /// </summary>
        [Column("ÛàãÀz")]
        public Decimal? ÛàãÀz { get; set; }

        /// <summary>
        /// ÀÛàv
        /// </summary>
        [Column("ÀÛàv")]
        public Decimal? ÀÛàv { get; set; }

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
