using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20090_¤ÏÌíÞ
    /// </summary>
    [Serializable]
    [Table("m_20090_¤ÏÌíÞ")]
    public class M20090¤ÏÌíÞ : ModelBase
    {
        /// <summary>
        /// ¤ÏÌR[h
        /// </summary>
        [Required]
        [Key]
        [Column("¤ÏÌR[h", Order = 1)]
        [StringLength(2)]
        public string ¤ÏÌR[h { get; set; }

        /// <summary>
        /// ¤ÏÌ¼
        /// </summary>
        [Column("¤ÏÌ¼")]
        [StringLength(20)]
        public string ¤ÏÌ¼ { get; set; }

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
