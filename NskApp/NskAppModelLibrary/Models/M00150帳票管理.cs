using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00150_ [Ç
    /// </summary>
    [Serializable]
    [Table("m_00150_ [Ç")]
    public class M00150 [Ç : ModelBase
    {
        /// <summary>
        ///  [Çid
        /// </summary>
        [Required]
        [Key]
        [Column(" [Çid", Order = 1)]
        [StringLength(12)]
        public string  [Çid { get; set; }

        /// <summary>
        ///  [t@Cæª
        /// </summary>
        [Column(" [t@Cæª")]
        [StringLength(1)]
        public string  [t@Cæª { get; set; }

        /// <summary>
        /// Æ±æª
        /// </summary>
        [Column("Æ±æª")]
        [StringLength(1)]
        public string Æ±æª { get; set; }

        /// <summary>
        /// Æ±ªÞ
        /// </summary>
        [Column("Æ±ªÞ")]
        [StringLength(2)]
        public string Æ±ªÞ { get; set; }

        /// <summary>
        ///  [t@C¼Ì
        /// </summary>
        [Column(" [t@C¼Ì")]
        [StringLength(50)]
        public string  [t@C¼Ì { get; set; }

        /// <summary>
        /// oÍob`id
        /// </summary>
        [Column("oÍob`id")]
        [StringLength(12)]
        public string oÍob`id { get; set; }

        /// <summary>
        /// oÍw¦æÊid
        /// </summary>
        [Column("oÍw¦æÊid")]
        [StringLength(12)]
        public string oÍw¦æÊid { get; set; }

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
