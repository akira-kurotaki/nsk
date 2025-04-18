using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24080_KiÊûnÊzªvZ
    /// </summary>
    [Serializable]
    [Table("t_24080_KiÊûnÊzªvZ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(YnÊÁ¿R[h), nameof(c_ÎÛOtO), nameof(Þæª), nameof(¸Zæª))]
    public class T24080KiÊûnÊzªvZ : ModelBase
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
        /// o×Êv
        /// </summary>
        [Column("o×Êv")]
        public Decimal? o×Êv { get; set; }

        /// <summary>
        /// KiÊo×v
        /// </summary>
        [Column("KiÊo×v")]
        public Decimal? KiÊo×v { get; set; }

        /// <summary>
        /// KiÊÊv
        /// </summary>
        [Column("KiÊÊv")]
        public Decimal? KiÊÊv { get; set; }

        /// <summary>
        /// ª¸ûÊv
        /// </summary>
        [Column("ª¸ûÊv")]
        public Decimal? ª¸ûÊv { get; set; }

        /// <summary>
        /// KiÊv
        /// </summary>
        [Column("KiÊv")]
        public Decimal? KiÊv { get; set; }

        /// <summary>
        /// ª¸ûÊvQ
        /// </summary>
        [Column("ª¸ûÊvQ")]
        public Decimal? ª¸ûÊvQ { get; set; }

        /// <summary>
        /// ª¸ûÊvR
        /// </summary>
        [Column("ª¸ûÊvR")]
        public Decimal? ª¸ûÊvR { get; set; }

        /// <summary>
        /// ª¸ûÊvv
        /// </summary>
        [Column("ª¸ûÊvv")]
        public Decimal? ª¸ûÊvv { get; set; }

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
