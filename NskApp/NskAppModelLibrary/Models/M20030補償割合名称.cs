using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20030_â¼Ì
    /// </summary>
    [Serializable]
    [Table("m_20030_â¼Ì")]
    public class M20030â¼Ì : ModelBase
    {
        /// <summary>
        /// âR[h
        /// </summary>
        [Required]
        [Key]
        [Column("âR[h", Order = 1)]
        [StringLength(2)]
        public string âR[h { get; set; }

        /// <summary>
        /// â¼Ì
        /// </summary>
        [Column("â¼Ì")]
        [StringLength(20)]
        public string â¼Ì { get; set; }

        /// <summary>
        /// âZk¼Ì
        /// </summary>
        [Column("âZk¼Ì")]
        [StringLength(20)]
        public string âZk¼Ì { get; set; }

        /// <summary>
        /// â
        /// </summary>
        [Column("â")]
        public Decimal? â { get; set; }

        /// <summary>
        /// x¥Jn¹Q
        /// </summary>
        [Column("x¥Jn¹Q")]
        public Decimal? x¥Jn¹Q { get; set; }

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
