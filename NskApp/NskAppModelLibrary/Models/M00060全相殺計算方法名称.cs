using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00060_SEvZû@¼Ì
    /// </summary>
    [Serializable]
    [Table("m_00060_SEvZû@¼Ì")]
    public class M00060SEvZû@¼Ì : ModelBase
    {
        /// <summary>
        /// SEvZû@
        /// </summary>
        [Required]
        [Key]
        [Column("SEvZû@", Order = 1)]
        [StringLength(1)]
        public string SEvZû@ { get; set; }

        /// <summary>
        /// SEvZû@¼Ì
        /// </summary>
        [Column("SEvZû@¼Ì")]
        [StringLength(30)]
        public string SEvZû@¼Ì { get; set; }

        /// <summary>
        /// ÞêtO
        /// </summary>
        [Column("ÞêtO")]
        [StringLength(1)]
        public string ÞêtO { get; set; }

        /// <summary>
        /// {ÝtO
        /// </summary>
        [Column("{ÝtO")]
        [StringLength(1)]
        public string {ÝtO { get; set; }

        /// <summary>
        /// ntO
        /// </summary>
        [Column("ntO")]
        [StringLength(1)]
        public string ntO { get; set; }

        /// <summary>
        /// n»Ê
        /// </summary>
        [Column("n»Ê")]
        [StringLength(1)]
        public string n»Ê { get; set; }

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
