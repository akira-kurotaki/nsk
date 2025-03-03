using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20220_•ªŠ„k’n”»’è–¼Ì
    /// </summary>
    [Serializable]
    [Table("m_20220_•ªŠ„k’n”»’è–¼Ì")]
    public class M20220•ªŠ„k’n”»’è–¼Ì : ModelBase
    {
        /// <summary>
        /// •ªŠ„k’n”»’èƒR[ƒh
        /// </summary>
        [Required]
        [Key]
        [Column("•ªŠ„k’n”»’èƒR[ƒh", Order = 1)]
        [StringLength(1)]
        public string •ªŠ„k’n”»’èƒR[ƒh { get; set; }

        /// <summary>
        /// •ªŠ„k’n”»’è–¼Ì
        /// </summary>
        [Column("•ªŠ„k’n”»’è–¼Ì")]
        [StringLength(20)]
        public string •ªŠ„k’n”»’è–¼Ì { get; set; }

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
