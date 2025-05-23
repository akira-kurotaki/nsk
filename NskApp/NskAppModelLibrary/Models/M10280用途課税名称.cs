using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10280_prÛÅ¼Ì
    /// </summary>
    [Serializable]
    [Table("m_10280_prÛÅ¼Ì")]
    [PrimaryKey(nameof(¤ÏÚIR[h), nameof(præª), nameof(ÛÅP¿æª))]
    public class M10280prÛÅ¼Ì : ModelBase
    {
        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Required]
        [Column("¤ÏÚIR[h", Order = 1)]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// præª
        /// </summary>
        [Required]
        [Column("præª", Order = 2)]
        [StringLength(3)]
        public string præª { get; set; }

        /// <summary>
        /// ÛÅP¿æª
        /// </summary>
        [Required]
        [Column("ÛÅP¿æª", Order = 3)]
        [StringLength(1)]
        public string ÛÅP¿æª { get; set; }

        /// <summary>
        /// prÛÅ¼Ì
        /// </summary>
        [Column("prÛÅ¼Ì")]
        [StringLength(30)]
        public string prÛÅ¼Ì { get; set; }

        /// <summary>
        /// prÛÅZk¼Ì
        /// </summary>
        [Column("prÛÅZk¼Ì")]
        [StringLength(10)]
        public string prÛÅZk¼Ì { get; set; }

        /// <summary>
        /// c_ÎÛOtO
        /// </summary>
        [Column("c_ÎÛOtO")]
        [StringLength(1)]
        public string c_ÎÛOtO { get; set; }

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
