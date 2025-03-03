using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00230_‰c”_‘ÎÛ–¼Ì
    /// </summary>
    [Serializable]
    [Table("m_00230_‰c”_‘ÎÛ–¼Ì")]
    public class M00230‰c”_‘ÎÛ–¼Ì : ModelBase
    {
        /// <summary>
        /// ‰c”_‘ÎÛŠOƒtƒ‰ƒO
        /// </summary>
        [Required]
        [Key]
        [Column("‰c”_‘ÎÛŠOƒtƒ‰ƒO", Order = 1)]
        [StringLength(1)]
        public string ‰c”_‘ÎÛŠOƒtƒ‰ƒO { get; set; }

        /// <summary>
        /// ‰c”_‘ÎÛƒtƒ‰ƒO–¼Ì
        /// </summary>
        [Column("‰c”_‘ÎÛƒtƒ‰ƒO–¼Ì")]
        [StringLength(10)]
        public string ‰c”_‘ÎÛƒtƒ‰ƒO–¼Ì { get; set; }

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
