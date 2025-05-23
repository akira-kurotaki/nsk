using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_21060_¤ÏÌîñüÍ
    /// </summary>
    [Serializable]
    [Table("t_21060_¤ÏÌîñüÍ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(knÔ), nameof(ªMÔ))]
    public class T21060¤ÏÌîñüÍ : ModelBase
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
        /// ]¿Nú
        /// </summary>
        [Column("]¿Nú")]
        public DateTime? ]¿Nú { get; set; }

        /// <summary>
        /// ]¿Ò
        /// </summary>
        [Column("]¿Ò")]
        [StringLength(20)]
        public string ]¿Ò { get; set; }

        /// <summary>
        /// ¤ÏÌP
        /// </summary>
        [Column("¤ÏÌP")]
        [StringLength(2)]
        public string ¤ÏÌP { get; set; }

        /// <summary>
        /// ÐQíÞP
        /// </summary>
        [Column("ÐQíÞP")]
        [StringLength(2)]
        public string ÐQíÞP { get; set; }

        /// <summary>
        /// ÐQ­¶NúP
        /// </summary>
        [Column("ÐQ­¶NúP")]
        public DateTime? ÐQ­¶NúP { get; set; }

        /// <summary>
        /// ¤ÏÌQ
        /// </summary>
        [Column("¤ÏÌQ")]
        [StringLength(2)]
        public string ¤ÏÌQ { get; set; }

        /// <summary>
        /// ÐQíÞQ
        /// </summary>
        [Column("ÐQíÞQ")]
        [StringLength(2)]
        public string ÐQíÞQ { get; set; }

        /// <summary>
        /// ÐQ­¶NúQ
        /// </summary>
        [Column("ÐQ­¶NúQ")]
        public DateTime? ÐQ­¶NúQ { get; set; }

        /// <summary>
        /// ¤ÏÌR
        /// </summary>
        [Column("¤ÏÌR")]
        [StringLength(2)]
        public string ¤ÏÌR { get; set; }

        /// <summary>
        /// ÐQíÞR
        /// </summary>
        [Column("ÐQíÞR")]
        [StringLength(2)]
        public string ÐQíÞR { get; set; }

        /// <summary>
        /// ÐQ­¶NúR
        /// </summary>
        [Column("ÐQ­¶NúR")]
        public DateTime? ÐQ­¶NúR { get; set; }

        /// <summary>
        /// ûn\èú
        /// </summary>
        [Column("ûn\èú")]
        public DateTime? ûn\èú { get; set; }

        /// <summary>
        /// Àü\èú
        /// </summary>
        [Column("Àü\èú")]
        public DateTime? Àü\èú { get; set; }

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
