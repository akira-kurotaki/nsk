using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00220_¤ÏÚIÎ
    /// </summary>
    [Serializable]
    [Table("m_00220_¤ÏÚIÎ")]
    [PrimaryKey(nameof(¤ÏÆR[h), nameof(¤ÏÚIR[h_fim), nameof(Uæª), nameof(¤ÏÚIR[h_nsk))]
    public class M00220¤ÏÚIÎ : ModelBase
    {
        /// <summary>
        /// ¤ÏÆR[h
        /// </summary>
        [Required]
        [Column("¤ÏÆR[h", Order = 1)]
        [StringLength(2)]
        public string ¤ÏÆR[h { get; set; }

        /// <summary>
        /// ¤ÏÚIR[h_fim
        /// </summary>
        [Required]
        [Column("¤ÏÚIR[h_fim", Order = 2)]
        [StringLength(2)]
        public string ¤ÏÚIR[h_fim { get; set; }

        /// <summary>
        /// Uæª
        /// </summary>
        [Required]
        [Column("Uæª", Order = 3)]
        [StringLength(1)]
        public string Uæª { get; set; }

        /// <summary>
        /// ¤ÏÚIR[h_nsk
        /// </summary>
        [Required]
        [Column("¤ÏÚIR[h_nsk", Order = 4)]
        [StringLength(2)]
        public string ¤ÏÚIR[h_nsk { get; set; }

        /// <summary>
        /// ÌpÊ
        /// </summary>
        [Column("ÌpÊ")]
        [StringLength(2)]
        public string ÌpÊ { get; set; }

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
