using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10160_•Š‰Û‹àˆøó•û®–¼Ì
    /// </summary>
    [Serializable]
    [Table("m_10160_•Š‰Û‹àˆøó•û®–¼Ì")]
    public class M10160•Š‰Û‹àˆøó•û®–¼Ì : ModelBase
    {
        /// <summary>
        /// •Š‰Û‹àˆøó•û®
        /// </summary>
        [Required]
        [Key]
        [Column("•Š‰Û‹àˆøó•û®", Order = 1)]
        [StringLength(2)]
        public string •Š‰Û‹àˆøó•û® { get; set; }

        /// <summary>
        /// •Š‰Û‹àˆøó•û®–¼Ì
        /// </summary>
        [Column("•Š‰Û‹àˆøó•û®–¼Ì")]
        [StringLength(20)]
        public string •Š‰Û‹àˆøó•û®–¼Ì { get; set; }

        /// <summary>
        /// ˆøó•û®
        /// </summary>
        [Column("ˆøó•û®")]
        [StringLength(1)]
        public string ˆøó•û® { get; set; }

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
