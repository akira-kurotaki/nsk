using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20230_ˆê•M‘S”¼‘¹”»’è–¼Ì
    /// </summary>
    [Serializable]
    [Table("m_20230_ˆê•M‘S”¼‘¹”»’è–¼Ì")]
    public class M20230ˆê•M‘S”¼‘¹”»’è–¼Ì : ModelBase
    {
        /// <summary>
        /// ˆê•M‘S”¼”»’è
        /// </summary>
        [Required]
        [Key]
        [Column("ˆê•M‘S”¼”»’è", Order = 1)]
        [StringLength(1)]
        public string ˆê•M‘S”¼”»’è { get; set; }

        /// <summary>
        /// ˆê•M‘S”¼‘¹”»’è–¼Ì
        /// </summary>
        [Column("ˆê•M‘S”¼‘¹”»’è–¼Ì")]
        [StringLength(20)]
        public string ˆê•M‘S”¼‘¹”»’è–¼Ì { get; set; }

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
