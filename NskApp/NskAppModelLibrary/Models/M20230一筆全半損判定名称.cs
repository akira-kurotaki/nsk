using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20230_êMS¼¹»è¼Ì
    /// </summary>
    [Serializable]
    [Table("m_20230_êMS¼¹»è¼Ì")]
    public class M20230êMS¼¹»è¼Ì : ModelBase
    {
        /// <summary>
        /// êMS¼»è
        /// </summary>
        [Required]
        [Key]
        [Column("êMS¼»è", Order = 1)]
        [StringLength(1)]
        public string êMS¼»è { get; set; }

        /// <summary>
        /// êMS¼¹»è¼Ì
        /// </summary>
        [Column("êMS¼¹»è¼Ì")]
        [StringLength(20)]
        public string êMS¼¹»è¼Ì { get; set; }

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
