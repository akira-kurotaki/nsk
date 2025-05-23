using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24240_YnÊÁ¿Ê]¿îñ_êMS¼¹_c__KiÊ
    /// </summary>
    [Serializable]
    [Table("t_24240_YnÊÁ¿Ê]¿îñ_êMS¼¹_c__KiÊ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(YnÊÁ¿R[h), nameof(c_ÎÛOtO), nameof(êMS¼¹æª), nameof(Þæª), nameof(Ki), nameof(¸Zæª))]
    public class T24240YnÊÁ¿Ê]¿îñêMS¼¹c_KiÊ : ModelBase
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
        /// êMS¼¹æª
        /// </summary>
        [Required]
        [Column("êMS¼¹æª", Order = 7)]
        [StringLength(1)]
        public string êMS¼¹æª { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 8)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// Ki
        /// </summary>
        [Required]
        [Column("Ki", Order = 9)]
        [StringLength(2)]
        public string Ki { get; set; }

        /// <summary>
        /// ¸Zæª
        /// </summary>
        [Required]
        [Column("¸Zæª", Order = 10)]
        [StringLength(1)]
        public string ¸Zæª { get; set; }

        /// <summary>
        /// ²®ãûnÊ
        /// </summary>
        [Column("²®ãûnÊ")]
        public Decimal? ²®ãûnÊ { get; set; }

        /// <summary>
        /// Ìûüz
        /// </summary>
        [Column("Ìûüz")]
        public Decimal? Ìûüz { get; set; }

        /// <summary>
        /// Ê¥z
        /// </summary>
        [Column("Ê¥z")]
        public Decimal? Ê¥z { get; set; }

        /// <summary>
        /// o×ÀÑÉÎ·éÊ¥_²®O
        /// </summary>
        [Column("o×ÀÑÉÎ·éÊ¥_²®O")]
        public Decimal? o×ÀÑÉÎ·éÊ¥_²®O { get; set; }

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
