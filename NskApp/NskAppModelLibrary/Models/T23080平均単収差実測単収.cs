using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_23080_½ÏPû·_ÀªPû
    /// </summary>
    [Serializable]
    [Table("t_23080_½ÏPû·_ÀªPû")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(²®Çæª), nameof(gõR[h), nameof(knÔ), nameof(ªMÔ))]
    public class T23080½ÏPû·ÀªPû : ModelBase
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
        /// ²®Çæª
        /// </summary>
        [Required]
        [Column("²®Çæª", Order = 4)]
        [StringLength(1)]
        public string ²®Çæª { get; set; }

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
        /// Þæª
        /// </summary>
        [Column("Þæª")]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// ÀªPû
        /// </summary>
        [Column("ÀªPû")]
        public Decimal? ÀªPû { get; set; }

        /// <summary>
        /// Kwæª
        /// </summary>
        [Column("Kwæª")]
        [StringLength(3)]
        public string Kwæª { get; set; }

        /// <summary>
        /// ]¿næR[h
        /// </summary>
        [Column("]¿næR[h")]
        [StringLength(4)]
        public string ]¿næR[h { get; set; }

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
