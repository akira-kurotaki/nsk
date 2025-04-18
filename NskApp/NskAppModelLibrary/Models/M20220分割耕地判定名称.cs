using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20220_ªkn»è¼Ì
    /// </summary>
    [Serializable]
    [Table("m_20220_ªkn»è¼Ì")]
    public class M20220ªkn»è¼Ì : ModelBase
    {
        /// <summary>
        /// ªkn»èR[h
        /// </summary>
        [Required]
        [Key]
        [Column("ªkn»èR[h", Order = 1)]
        [StringLength(1)]
        public string ªkn»èR[h { get; set; }

        /// <summary>
        /// ªkn»è¼Ì
        /// </summary>
        [Column("ªkn»è¼Ì")]
        [StringLength(20)]
        public string ªkn»è¼Ì { get; set; }

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
