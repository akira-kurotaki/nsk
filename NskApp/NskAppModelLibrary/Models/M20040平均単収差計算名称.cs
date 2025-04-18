using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20040_½ÏPû·vZ¼Ì
    /// </summary>
    [Serializable]
    [Table("m_20040_½ÏPû·vZ¼Ì")]
    public class M20040½ÏPû·vZ¼Ì : ModelBase
    {
        /// <summary>
        /// ½ÏPû·vZû@
        /// </summary>
        [Required]
        [Key]
        [Column("½ÏPû·vZû@", Order = 1)]
        [StringLength(1)]
        public string ½ÏPû·vZû@ { get; set; }

        /// <summary>
        /// ½ÏPû·vZ¼Ì
        /// </summary>
        [Column("½ÏPû·vZ¼Ì")]
        [StringLength(20)]
        public string ½ÏPû·vZ¼Ì { get; set; }

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
