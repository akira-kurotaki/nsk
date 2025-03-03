using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20030_•âŠ„‡–¼Ì
    /// </summary>
    [Serializable]
    [Table("m_20030_•âŠ„‡–¼Ì")]
    public class M20030•âŠ„‡–¼Ì : ModelBase
    {
        /// <summary>
        /// •âŠ„‡ƒR[ƒh
        /// </summary>
        [Required]
        [Key]
        [Column("•âŠ„‡ƒR[ƒh", Order = 1)]
        [StringLength(2)]
        public string •âŠ„‡ƒR[ƒh { get; set; }

        /// <summary>
        /// •âŠ„‡–¼Ì
        /// </summary>
        [Column("•âŠ„‡–¼Ì")]
        [StringLength(20)]
        public string •âŠ„‡–¼Ì { get; set; }

        /// <summary>
        /// •âŠ„‡’Zk–¼Ì
        /// </summary>
        [Column("•âŠ„‡’Zk–¼Ì")]
        [StringLength(20)]
        public string •âŠ„‡’Zk–¼Ì { get; set; }

        /// <summary>
        /// •âŠ„‡
        /// </summary>
        [Column("•âŠ„‡")]
        public Decimal? •âŠ„‡ { get; set; }

        /// <summary>
        /// x•¥ŠJn‘¹ŠQŠ„‡
        /// </summary>
        [Column("x•¥ŠJn‘¹ŠQŠ„‡")]
        public Decimal? x•¥ŠJn‘¹ŠQŠ„‡ { get; set; }

        /// <summary>
        /// “o˜^“ú
        /// </summary>
        [Column("“o˜^“ú")]
        public DateTime? “o˜^“ú { get; set; }

        /// <summary>
        /// “o˜^ƒ†[ƒUid
        /// </summary>
        [Column("“o˜^ƒ†[ƒUid")]
        [StringLength(11)]
        public string “o˜^ƒ†[ƒUid { get; set; }
    }
}
