using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20210_øóû®Ê»è
    /// </summary>
    [Serializable]
    [Table("m_20210_øóû®Ê»è")]
    [PrimaryKey(nameof(øóû®), nameof(ÞêtO), nameof(íQ»èR[h))]
    public class M20210øóû®Ê»è : ModelBase
    {
        /// <summary>
        /// øóû®
        /// </summary>
        [Required]
        [Column("øóû®", Order = 1)]
        [StringLength(1)]
        public string øóû® { get; set; }

        /// <summary>
        /// ÞêtO
        /// </summary>
        [Required]
        [Column("ÞêtO", Order = 2)]
        [StringLength(1)]
        public string ÞêtO { get; set; }

        /// <summary>
        /// íQ»èR[h
        /// </summary>
        [Required]
        [Column("íQ»èR[h", Order = 3)]
        [StringLength(1)]
        public string íQ»èR[h { get; set; }

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
