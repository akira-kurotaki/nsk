using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24070_knÊêMS¹¼¹]¿îñ
    /// </summary>
    [Serializable]
    [Table("t_24070_knÊêMS¹¼¹]¿îñ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(knÔ), nameof(ªMÔ))]
    public class T24070knÊêMS¹¼¹]¿îñ : ModelBase
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
        /// knÔ
        /// </summary>
        [Required]
        [Column("knÔ", Order = 5)]
        [StringLength(5)]
        public string knÔ { get; set; }

        /// <summary>
        /// ªMÔ
        /// </summary>
        [Required]
        [Column("ªMÔ", Order = 6)]
        [StringLength(4)]
        public string ªMÔ { get; set; }

        /// <summary>
        /// YnÊÁ¿R[h
        /// </summary>
        [Column("YnÊÁ¿R[h")]
        [StringLength(5)]
        public string YnÊÁ¿R[h { get; set; }

        /// <summary>
        /// c_ÎÛOtO
        /// </summary>
        [Column("c_ÎÛOtO")]
        [StringLength(1)]
        public string c_ÎÛOtO { get; set; }

        /// <summary>
        /// êMS¼¹æª
        /// </summary>
        [Column("êMS¼¹æª")]
        [StringLength(1)]
        public string êMS¼¹æª { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Column("Þæª")]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// â
        /// </summary>
        [Column("â")]
        [StringLength(2)]
        public string â { get; set; }

        /// <summary>
        /// øóÊÏ
        /// </summary>
        [Column("øóÊÏ")]
        public Decimal? øóÊÏ { get; set; }

        /// <summary>
        /// íQ»èR[h
        /// </summary>
        [Column("íQ»èR[h")]
        [StringLength(1)]
        public string íQ»èR[h { get; set; }

        /// <summary>
        /// îûnÊ
        /// </summary>
        [Column("îûnÊ")]
        public Decimal? îûnÊ { get; set; }

        /// <summary>
        /// î¶Yàz
        /// </summary>
        [Column("î¶Yàz")]
        public Decimal? î¶Yàz { get; set; }

        /// <summary>
        /// ¼¹kn¶Yàz
        /// </summary>
        [Column("¼¹kn¶Yàz")]
        public Decimal? ¼¹kn¶Yàz { get; set; }

        /// <summary>
        /// c_p±x¥zÎÛtO
        /// </summary>
        [Column("c_p±x¥zÎÛtO")]
        [StringLength(1)]
        public string c_p±x¥zÎÛtO { get; set; }

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
