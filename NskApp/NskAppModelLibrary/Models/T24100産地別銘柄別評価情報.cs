using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24100_YnÊÁ¿Ê]¿îñ
    /// </summary>
    [Serializable]
    [Table("t_24100_YnÊÁ¿Ê]¿îñ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(YnÊÁ¿R[h), nameof(c_ÎÛOtO), nameof(Þæª), nameof(¸Zæª))]
    public class T24100YnÊÁ¿Ê]¿îñ : ModelBase
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
        /// gõR[h
        /// </summary>
        [Required]
        [Column("gõR[h", Order = 4)]
        [StringLength(13)]
        public string gõR[h { get; set; }

        /// <summary>
        /// YnÊÁ¿R[h
        /// </summary>
        [Required]
        [Column("YnÊÁ¿R[h", Order = 5)]
        [StringLength(5)]
        public string YnÊÁ¿R[h { get; set; }

        /// <summary>
        /// c_ÎÛOtO
        /// </summary>
        [Required]
        [Column("c_ÎÛOtO", Order = 6)]
        [StringLength(1)]
        public string c_ÎÛOtO { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 7)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// ¸Zæª
        /// </summary>
        [Required]
        [Column("¸Zæª", Order = 8)]
        [StringLength(1)]
        public string ¸Zæª { get; set; }

        /// <summary>
        /// ûnÊv
        /// </summary>
        [Column("ûnÊv")]
        public Decimal? ûnÊv { get; set; }

        /// <summary>
        /// ª¸ûÊv
        /// </summary>
        [Column("ª¸ûÊv")]
        public Decimal? ª¸ûÊv { get; set; }

        /// <summary>
        /// ªãûnÊv
        /// </summary>
        [Column("ªãûnÊv")]
        public Decimal? ªãûnÊv { get; set; }

        /// <summary>
        /// ²®ãûnÊv
        /// </summary>
        [Column("²®ãûnÊv")]
        public Decimal? ²®ãûnÊv { get; set; }

        /// <summary>
        /// îûnÊ
        /// </summary>
        [Column("îûnÊ")]
        public Decimal? îûnÊ { get; set; }

        /// <summary>
        /// ¶Yàzv
        /// </summary>
        [Column("¶Yàzv")]
        public Decimal? ¶Yàzv { get; set; }

        /// <summary>
        /// î¶Yàz
        /// </summary>
        [Column("î¶Yàz")]
        public Decimal? î¶Yàz { get; set; }

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
