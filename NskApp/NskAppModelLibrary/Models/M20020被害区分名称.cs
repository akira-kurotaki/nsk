using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20020_íQæª¼Ì
    /// </summary>
    [Serializable]
    [Table("m_20020_íQæª¼Ì")]
    public class M20020íQæª¼Ì : ModelBase
    {
        /// <summary>
        /// íQæª
        /// </summary>
        [Required]
        [Key]
        [Column("íQæª", Order = 1)]
        [StringLength(1)]
        public string íQæª { get; set; }

        /// <summary>
        /// íQæª¼Ì
        /// </summary>
        [Column("íQæª¼Ì")]
        [StringLength(20)]
        public string íQæª¼Ì { get; set; }

        /// <summary>
        /// íQæªªÌ
        /// </summary>
        [Column("íQæªªÌ")]
        [StringLength(20)]
        public string íQæªªÌ { get; set; }

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
