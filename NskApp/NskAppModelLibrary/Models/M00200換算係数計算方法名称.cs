using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00200_Š·ZŒW”ŒvZ•û–@–¼Ì
    /// </summary>
    [Serializable]
    [Table("m_00200_Š·ZŒW”ŒvZ•û–@–¼Ì")]
    public class M00200Š·ZŒW”ŒvZ•û–@–¼Ì : ModelBase
    {
        /// <summary>
        /// ’P“–Š·ZŒW”ŒvZ•û–@
        /// </summary>
        [Required]
        [Key]
        [Column("’P“–Š·ZŒW”ŒvZ•û–@", Order = 1)]
        [StringLength(1)]
        public string ’P“–Š·ZŒW”ŒvZ•û–@ { get; set; }

        /// <summary>
        /// ’P“–Š·ZŒW”ŒvZ•û–@–¼
        /// </summary>
        [Column("’P“–Š·ZŒW”ŒvZ•û–@–¼")]
        [StringLength(20)]
        public string ’P“–Š·ZŒW”ŒvZ•û–@–¼ { get; set; }

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
