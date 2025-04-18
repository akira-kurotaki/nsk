using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00190_åÊf[^ÎÛf[^
    /// </summary>
    [Serializable]
    [Table("m_00190_åÊf[^ÎÛf[^")]
    public class M00190åÊf[^ÎÛf[^ : ModelBase
    {
        /// <summary>
        /// ÎÛf[^æª
        /// </summary>
        [Required]
        [Key]
        [Column("ÎÛf[^æª", Order = 1)]
        [StringLength(2)]
        public string ÎÛf[^æª { get; set; }

        /// <summary>
        /// Æ±æª
        /// </summary>
        [Column("Æ±æª")]
        [StringLength(1)]
        public string Æ±æª { get; set; }

        /// <summary>
        /// óüÎÛf[^¼Ì
        /// </summary>
        [Column("óüÎÛf[^¼Ì")]
        [StringLength(30)]
        public string óüÎÛf[^¼Ì { get; set; }

        /// <summary>
        /// óüÎÛf[^ªÌ
        /// </summary>
        [Column("óüÎÛf[^ªÌ")]
        [StringLength(30)]
        public string óüÎÛf[^ªÌ { get; set; }

        /// <summary>
        /// óüob`id
        /// </summary>
        [Column("óüob`id")]
        [StringLength(12)]
        public string óüob`id { get; set; }

        /// <summary>
        /// æob`id
        /// </summary>
        [Column("æob`id")]
        [StringLength(12)]
        public string æob`id { get; set; }

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
