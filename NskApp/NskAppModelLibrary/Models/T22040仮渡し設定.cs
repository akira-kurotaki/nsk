using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_22040_¼nµÝè
    /// </summary>
    [Serializable]
    [Table("t_22040_¼nµÝè")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(Þæª), nameof(øóû®), nameof(â), nameof(Áñæª))]
    public class T22040¼nµÝè : ModelBase
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
        /// øóû®
        /// </summary>
        [Required]
        [Column("øóû®", Order = 5)]
        [StringLength(1)]
        public string øóû® { get; set; }

        /// <summary>
        /// â
        /// </summary>
        [Required]
        [Column("â", Order = 6)]
        [StringLength(2)]
        public string â { get; set; }

        /// <summary>
        /// Áñæª
        /// </summary>
        [Required]
        [Column("Áñæª", Order = 7)]
        [StringLength(1)]
        public string Áñæª { get; set; }

        /// <summary>
        /// ¼nÀ{íQ
        /// </summary>
        [Column("¼nÀ{íQ")]
        public Decimal? ¼nÀ{íQ { get; set; }

        /// <summary>
        /// ¼nx¥¦
        /// </summary>
        [Column("¼nx¥¦")]
        public Decimal? ¼nx¥¦ { get; set; }

        /// <summary>
        /// ¼n[R[h
        /// </summary>
        [Column("¼n[R[h")]
        [StringLength(1)]
        public string ¼n[R[h { get; set; }

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
