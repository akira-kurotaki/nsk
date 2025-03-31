using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20240_\‹æ•ª–¼Ì
    /// </summary>
    [Serializable]
    [Table("m_20240_\‹æ•ª–¼Ì")]
    public class M20240\‹æ•ª–¼Ì : ModelBase
    {
        /// <summary>
        /// \‹æ•ª
        /// </summary>
        [Required]
        [Key]
        [Column("\‹æ•ª", Order = 1)]
        [StringLength(1)]
        public string \‹æ•ª { get; set; }

        /// <summary>
        /// \‹æ•ª–¼Ì
        /// </summary>
        [Column("\‹æ•ª–¼Ì")]
        [StringLength(20)]
        public string \‹æ•ª–¼Ì { get; set; }

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
