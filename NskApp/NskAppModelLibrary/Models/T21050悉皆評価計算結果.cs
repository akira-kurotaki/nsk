using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_21050_»F]¿_vZÊ
    /// </summary>
    [Serializable]
    [Table("t_21050_»F]¿_vZÊ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(knÔ), nameof(ªMÔ))]
    public class T21050»F]¿vZÊ : ModelBase
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
        /// Þæª
        /// </summary>
        [Column("Þæª")]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// ]¿næR[h
        /// </summary>
        [Column("]¿næR[h")]
        [StringLength(4)]
        public string ]¿næR[h { get; set; }

        /// <summary>
        /// Kwæª
        /// </summary>
        [Column("Kwæª")]
        [StringLength(3)]
        public string Kwæª { get; set; }

        /// <summary>
        /// iíR[h
        /// </summary>
        [Column("iíR[h")]
        [StringLength(3)]
        public string iíR[h { get; set; }

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
        /// íQ»èR[h
        /// </summary>
        [Column("íQ»èR[h")]
        [StringLength(1)]
        public string íQ»èR[h { get; set; }

        /// <summary>
        /// êMS¼»è
        /// </summary>
        [Column("êMS¼»è")]
        [StringLength(1)]
        public string êMS¼»è { get; set; }

        /// <summary>
        /// ]¿»èR[h
        /// </summary>
        [Column("]¿»èR[h")]
        [StringLength(2)]
        public string ]¿»èR[h { get; set; }

        /// <summary>
        /// ¹Q]¿ÊÏ
        /// </summary>
        [Column("¹Q]¿ÊÏ")]
        public Decimal? ¹Q]¿ÊÏ { get; set; }

        /// <summary>
        /// ÎÛÊÏ_Ê
        /// </summary>
        [Column("ÎÛÊÏ_Ê")]
        public Decimal? ÎÛÊÏ_Ê { get; set; }

        /// <summary>
        /// ²®ã½ÏPû
        /// </summary>
        [Column("²®ã½ÏPû")]
        public Decimal? ²®ã½ÏPû { get; set; }

        /// <summary>
        /// »FPû
        /// </summary>
        [Column("»FPû")]
        public Decimal? »FPû { get; set; }

        /// <summary>
        /// »FûnÊ
        /// </summary>
        [Column("»FûnÊ")]
        public Decimal? »FûnÊ { get; set; }

        /// <summary>
        /// o×PûÊ_Ê
        /// </summary>
        [Column("o×PûÊ_Ê")]
        public Decimal? o×PûÊ_Ê { get; set; }

        /// <summary>
        /// o×ûnÊ
        /// </summary>
        [Column("o×ûnÊ")]
        public Decimal? o×ûnÊ { get; set; }

        /// <summary>
        /// ª
        /// </summary>
        [Column("ª")]
        public Decimal? ª { get; set; }

        /// <summary>
        /// ª_Ê
        /// </summary>
        [Column("ª_Ê")]
        public Decimal? ª_Ê { get; set; }

        /// <summary>
        /// ª¸ûÊ
        /// </summary>
        [Column("ª¸ûÊ")]
        public Decimal? ª¸ûÊ { get; set; }

        /// <summary>
        /// PC³Ê
        /// </summary>
        [Column("PC³Ê")]
        public Decimal? PC³Ê { get; set; }

        /// <summary>
        /// g]¿Pû
        /// </summary>
        [Column("g]¿Pû")]
        public Decimal? g]¿Pû { get; set; }

        /// <summary>
        /// g]¿ûÊ
        /// </summary>
        [Column("g]¿ûÊ")]
        public Decimal? g]¿ûÊ { get; set; }

        /// <summary>
        /// ûnÊ
        /// </summary>
        [Column("ûnÊ")]
        public Decimal? ûnÊ { get; set; }

        /// <summary>
        /// ¸ûÊ
        /// </summary>
        [Column("¸ûÊ")]
        public Decimal? ¸ûÊ { get; set; }

        /// <summary>
        /// êMS¼¸ûÊ
        /// </summary>
        [Column("êMS¼¸ûÊ")]
        public Decimal? êMS¼¸ûÊ { get; set; }

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
