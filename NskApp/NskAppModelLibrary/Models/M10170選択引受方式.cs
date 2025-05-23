using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10170_Iðøóû®
    /// </summary>
    [Serializable]
    [Table("m_10170_Iðøóû®")]
    [PrimaryKey(nameof(gR[h), nameof(¤ÏÚIR[h), nameof(øóû®), nameof(Áñæª), nameof(âR[h), nameof(Þæª))]
    public class M10170Iðøóû® : ModelBase
    {
        /// <summary>
        /// gR[h
        /// </summary>
        [Required]
        [Column("gR[h", Order = 1)]
        [StringLength(3)]
        public string gR[h { get; set; }

        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Required]
        [Column("¤ÏÚIR[h", Order = 2)]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// øóû®
        /// </summary>
        [Required]
        [Column("øóû®", Order = 3)]
        [StringLength(1)]
        public string øóû® { get; set; }

        /// <summary>
        /// Áñæª
        /// </summary>
        [Required]
        [Column("Áñæª", Order = 4)]
        [StringLength(1)]
        public string Áñæª { get; set; }

        /// <summary>
        /// âR[h
        /// </summary>
        [Required]
        [Column("âR[h", Order = 5)]
        [StringLength(2)]
        public string âR[h { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 6)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// \¦
        /// </summary>
        [Column("\¦")]
        public short? \¦ { get; set; }

        /// <summary>
        /// øóû®¼Ì
        /// </summary>
        [Column("øóû®¼Ì")]
        [StringLength(20)]
        public string øóû®¼Ì { get; set; }

        /// <summary>
        /// Áñæª¼Ì
        /// </summary>
        [Column("Áñæª¼Ì")]
        [StringLength(20)]
        public string Áñæª¼Ì { get; set; }

        /// <summary>
        /// â¼Ì
        /// </summary>
        [Column("â¼Ì")]
        [StringLength(40)]
        public string â¼Ì { get; set; }

        /// <summary>
        /// ÞZk¼Ì
        /// </summary>
        [Column("ÞZk¼Ì")]
        [StringLength(20)]
        public string ÞZk¼Ì { get; set; }

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

        /// <summary>
        /// XVú
        /// </summary>
        [Column("XVú")]
        public DateTime? XVú { get; set; }

        /// <summary>
        /// XV[Uid
        /// </summary>
        [Column("XV[Uid")]
        [StringLength(11)]
        public string XV[Uid { get; set; }
    }
}
