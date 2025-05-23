using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00010_¤ÏÚI¼Ì
    /// </summary>
    [Serializable]
    [Table("m_00010_¤ÏÚI¼Ì")]
    public class M00010¤ÏÚI¼Ì : ModelBase
    {
        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Required]
        [Key]
        [Column("¤ÏÚIR[h", Order = 1)]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// ¤ÏÚI¼Ì
        /// </summary>
        [Column("¤ÏÚI¼Ì")]
        [StringLength(20)]
        public string ¤ÏÚI¼Ì { get; set; }

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
