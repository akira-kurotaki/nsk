using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_21160_knÊÏª]¿îñ
    /// </summary>
    [Serializable]
    [Table("t_21160_knÊÏª]¿îñ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(knÔ), nameof(ªMÔ), nameof(sÔ))]
    public class T21160knÊÏª]¿îñ : ModelBase
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
        /// sÔ
        /// </summary>
        [Required]
        [Column("sÔ", Order = 7)]
        [StringLength(2)]
        public string sÔ { get; set; }

        /// <summary>
        /// ªkn»èR[h
        /// </summary>
        [Column("ªkn»èR[h")]
        [StringLength(1)]
        public string ªkn»èR[h { get; set; }

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
        /// õl
        /// </summary>
        [Column("õl")]
        [StringLength(255)]
        public string õl { get; set; }

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
