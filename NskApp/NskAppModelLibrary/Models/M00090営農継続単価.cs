using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00090_c_p±P¿
    /// </summary>
    [Serializable]
    [Table("m_00090_c_p±P¿")]
    [PrimaryKey(nameof(¤ÏÚIR[h), nameof(KpNY))]
    public class M00090c_p±P¿ : ModelBase
    {
        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Required]
        [Column("¤ÏÚIR[h", Order = 1)]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// KpNY
        /// </summary>
        [Required]
        [Column("KpNY", Order = 2)]
        [StringLength(4)]
        public string KpNY { get; set; }

        /// <summary>
        /// c_p±P¿
        /// </summary>
        [Column("c_p±P¿")]
        public Decimal? c_p±P¿ { get; set; }

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
