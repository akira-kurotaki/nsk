using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00200_·ZWvZû@¼Ì
    /// </summary>
    [Serializable]
    [Table("m_00200_·ZWvZû@¼Ì")]
    public class M00200·ZWvZû@¼Ì : ModelBase
    {
        /// <summary>
        /// P·ZWvZû@
        /// </summary>
        [Required]
        [Key]
        [Column("P·ZWvZû@", Order = 1)]
        [StringLength(1)]
        public string P·ZWvZû@ { get; set; }

        /// <summary>
        /// P·ZWvZû@¼
        /// </summary>
        [Column("P·ZWvZû@¼")]
        [StringLength(20)]
        public string P·ZWvZû@¼ { get; set; }

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
