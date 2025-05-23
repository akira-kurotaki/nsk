using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20010_íQ»è¼Ì
    /// </summary>
    [Serializable]
    [Table("m_20010_íQ»è¼Ì")]
    public class M20010íQ»è¼Ì : ModelBase
    {
        /// <summary>
        /// íQ»èR[h
        /// </summary>
        [Required]
        [Key]
        [Column("íQ»èR[h", Order = 1)]
        [StringLength(1)]
        public string íQ»èR[h { get; set; }

        /// <summary>
        /// íQ»è¼Ì
        /// </summary>
        [Column("íQ»è¼Ì")]
        [StringLength(20)]
        public string íQ»è¼Ì { get; set; }

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
