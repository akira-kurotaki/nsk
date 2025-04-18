using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10120_præªIð
    /// </summary>
    [Serializable]
    [Table("m_10120_præªIð")]
    [PrimaryKey(nameof(¤ÏÚIR[h), nameof(Þæª), nameof(ìtú), nameof(præª))]
    public class M10120præªIð : ModelBase
    {
        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Required]
        [Column("¤ÏÚIR[h", Order = 1)]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 2)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// íÞæª
        /// </summary>
        [Column("íÞæª")]
        [StringLength(1)]
        public string íÞæª { get; set; }

        /// <summary>
        /// ìtú
        /// </summary>
        [Required]
        [Column("ìtú", Order = 3)]
        [StringLength(1)]
        public string ìtú { get; set; }

        /// <summary>
        /// c¨æª
        /// </summary>
        [Column("c¨æª")]
        [StringLength(1)]
        public string c¨æª { get; set; }

        /// <summary>
        /// præª
        /// </summary>
        [Required]
        [Column("præª", Order = 4)]
        [StringLength(3)]
        public string præª { get; set; }

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
