using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00230_c_ÎÛ¼Ì
    /// </summary>
    [Serializable]
    [Table("m_00230_c_ÎÛ¼Ì")]
    public class M00230c_ÎÛ¼Ì : ModelBase
    {
        /// <summary>
        /// c_ÎÛOtO
        /// </summary>
        [Required]
        [Key]
        [Column("c_ÎÛOtO", Order = 1)]
        [StringLength(1)]
        public string c_ÎÛOtO { get; set; }

        /// <summary>
        /// c_ÎÛtO¼Ì
        /// </summary>
        [Column("c_ÎÛtO¼Ì")]
        [StringLength(10)]
        public string c_ÎÛtO¼Ì { get; set; }

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
