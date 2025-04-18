using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10130_íÞæª¼Ì
    /// </summary>
    [Serializable]
    [Table("m_10130_íÞæª¼Ì")]
    [PrimaryKey(nameof(¤ÏÚIR[h), nameof(íÞæª))]
    public class M10130íÞæª¼Ì : ModelBase
    {
        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Required]
        [Column("¤ÏÚIR[h", Order = 1)]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// íÞæª
        /// </summary>
        [Required]
        [Column("íÞæª", Order = 2)]
        [StringLength(1)]
        public string íÞæª { get; set; }

        /// <summary>
        /// íÞæª¼Ì
        /// </summary>
        [Column("íÞæª¼Ì")]
        [StringLength(20)]
        public string íÞæª¼Ì { get; set; }

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
