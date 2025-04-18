using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24060_knÊª]¿îñ
    /// </summary>
    [Serializable]
    [Table("t_24060_knÊª]¿îñ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(knÔ), nameof(ªMÔ), nameof(ªkn»èR[h))]
    public class T24060knÊª]¿îñ : ModelBase
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
        /// ªkn»èR[h
        /// </summary>
        [Required]
        [Column("ªkn»èR[h", Order = 7)]
        [StringLength(1)]
        public string ªkn»èR[h { get; set; }

        /// <summary>
        /// c_ÎÛOtO
        /// </summary>
        [Column("c_ÎÛOtO")]
        [StringLength(1)]
        public string c_ÎÛOtO { get; set; }

        /// <summary>
        /// YnÊÁ¿R[h
        /// </summary>
        [Column("YnÊÁ¿R[h")]
        [StringLength(5)]
        public string YnÊÁ¿R[h { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Column("Þæª")]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// øóÊÏ
        /// </summary>
        [Column("øóÊÏ")]
        public Decimal? øóÊÏ { get; set; }

        /// <summary>
        /// ²®ã½ÏPû
        /// </summary>
        [Column("²®ã½ÏPû")]
        public Decimal? ²®ã½ÏPû { get; set; }

        /// <summary>
        /// ªÎÛÊÏ
        /// </summary>
        [Column("ªÎÛÊÏ")]
        public Decimal? ªÎÛÊÏ { get; set; }

        /// <summary>
        /// ªÎÛÊÏ
        /// </summary>
        [Column("ªÎÛÊÏ")]
        public Decimal? ªÎÛÊÏ { get; set; }

        /// <summary>
        /// o×PûÊ_
        /// </summary>
        [Column("o×PûÊ_")]
        public Decimal? o×PûÊ_ { get; set; }

        /// <summary>
        /// o×PûÊ_Ê
        /// </summary>
        [Column("o×PûÊ_Ê")]
        public Decimal? o×PûÊ_Ê { get; set; }

        /// <summary>
        /// o×PûÊ_vZ
        /// </summary>
        [Column("o×PûÊ_vZ")]
        public Decimal? o×PûÊ_vZ { get; set; }

        /// <summary>
        /// ª¸ûÊ_
        /// </summary>
        [Column("ª¸ûÊ_")]
        public Decimal? ª¸ûÊ_ { get; set; }

        /// <summary>
        /// ª¸ûÊ_Ê
        /// </summary>
        [Column("ª¸ûÊ_Ê")]
        public Decimal? ª¸ûÊ_Ê { get; set; }

        /// <summary>
        /// ª¸ûÊ_vZ
        /// </summary>
        [Column("ª¸ûÊ_vZ")]
        public Decimal? ª¸ûÊ_vZ { get; set; }

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
