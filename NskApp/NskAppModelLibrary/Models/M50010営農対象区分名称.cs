using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_50010_c_ÎÛæª¼Ì
    /// </summary>
    [Serializable]
    [Table("m_50010_c_ÎÛæª¼Ì")]
    public class M50010c_ÎÛæª¼Ì : ModelBase
    {
        /// <summary>
        /// c_ÎÛæª
        /// </summary>
        [Required]
        [Key]
        [Column("c_ÎÛæª", Order = 1)]
        [StringLength(1)]
        public string c_ÎÛæª { get; set; }

        /// <summary>
        /// c_ÎÛæª¼Ì
        /// </summary>
        [Column("c_ÎÛæª¼Ì")]
        [StringLength(20)]
        public string c_ÎÛæª¼Ì { get; set; }

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
