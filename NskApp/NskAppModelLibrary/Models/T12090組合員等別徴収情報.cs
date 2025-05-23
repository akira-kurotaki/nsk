using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12090_gõÊ¥ûîñ
    /// </summary>
    [Serializable]
    [Table("t_12090_gõÊ¥ûîñ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(øóñ), nameof(gõR[h))]
    public class T12090gõÊ¥ûîñ : ModelBase
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
        /// ¥ûæªR[h
        /// </summary>
        [Column("¥ûæªR[h")]
        [StringLength(1)]
        public string ¥ûæªR[h { get; set; }

        /// <summary>
        /// ©®UÖtO
        /// </summary>
        [Column("©®UÖtO")]
        [StringLength(1)]
        public string ©®UÖtO { get; set; }

        /// <summary>
        /// ¥ûRR[h
        /// </summary>
        [Column("¥ûRR[h")]
        [StringLength(2)]
        public string ¥ûRR[h { get; set; }

        /// <summary>
        /// ¥ûNú
        /// </summary>
        [Column("¥ûNú")]
        public DateTime? ¥ûNú { get; set; }

        /// <summary>
        /// ¥ûÒ
        /// </summary>
        [Column("¥ûÒ")]
        [StringLength(40)]
        public string ¥ûÒ { get; set; }

        /// <summary>
        /// ¥ûàz
        /// </summary>
        [Column("¥ûàz")]
        public Decimal? ¥ûàz { get; set; }

        /// <summary>
        /// à_ÆS|à
        /// </summary>
        [Column("à_ÆS|à")]
        public Decimal? à_ÆS|à { get; set; }

        /// <summary>
        /// àÛà
        /// </summary>
        [Column("àÛà")]
        public Decimal? àÛà { get; set; }

        /// <summary>
        /// øóð¥ûÛàz
        /// </summary>
        [Column("øóð¥ûÛàz")]
        public Decimal? øóð¥ûÛàz { get; set; }

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
