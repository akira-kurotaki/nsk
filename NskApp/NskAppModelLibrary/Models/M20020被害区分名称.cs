using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20020_”íŠQ‹æ•ª–¼Ì
    /// </summary>
    [Serializable]
    [Table("m_20020_”íŠQ‹æ•ª–¼Ì")]
    public class M20020”íŠQ‹æ•ª–¼Ì : ModelBase
    {
        /// <summary>
        /// ”íŠQ‹æ•ª
        /// </summary>
        [Required]
        [Key]
        [Column("”íŠQ‹æ•ª", Order = 1)]
        [StringLength(1)]
        public string ”íŠQ‹æ•ª { get; set; }

        /// <summary>
        /// ”íŠQ‹æ•ª–¼Ì
        /// </summary>
        [Column("”íŠQ‹æ•ª–¼Ì")]
        [StringLength(20)]
        public string ”íŠQ‹æ•ª–¼Ì { get; set; }

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
