using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10050_îPû
    /// </summary>
    [Serializable]
    [Table("m_10050_îPû")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(Þæª), nameof(xR[h))]
    public class M10050îPû : ModelBase
    {
        /// <summary>
        /// gR[h
        /// </summary>
        [Required]
        [Column("gR[h", Order = 1)]
        [StringLength(3)]
        public string gR[h { get; set; }

        /// <summary>
        /// NY
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("NY", Order = 2)]
        public short NY { get; set; }

        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Required]
        [Column("¤ÏÚIR[h", Order = 3)]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 4)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// xR[h
        /// </summary>
        [Required]
        [Column("xR[h", Order = 5)]
        [StringLength(2)]
        public string xR[h { get; set; }

        /// <summary>
        /// §Êmw¦Pû
        /// </summary>
        [Column("§Êmw¦Pû")]
        public Decimal? §Êmw¦Pû { get; set; }

        /// <summary>
        /// ÚWl
        /// </summary>
        [Column("ÚWl")]
        public Decimal? ÚWl { get; set; }

        /// <summary>
        /// ÅûÊ
        /// </summary>
        [Column("ÅûÊ")]
        public Decimal? ÅûÊ { get; set; }

        /// <summary>
        /// Â
        /// </summary>
        [Column("Â")]
        public Decimal? Â { get; set; }

        /// <summary>
        /// Ð
        /// </summary>
        [Column("Ð")]
        public Decimal? Ð { get; set; }

        /// <summary>
        /// â¿ÚC³W
        /// </summary>
        [Column("â¿ÚC³W")]
        public Decimal? â¿ÚC³W { get; set; }

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
