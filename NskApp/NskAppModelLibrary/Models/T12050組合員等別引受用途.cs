using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_12050_gõÊøópr
    /// </summary>
    [Serializable]
    [Table("t_12050_gõÊøópr")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(øóñ), nameof(gõR[h), nameof(Þæª), nameof(vPÊnæR[h), nameof(ìtú), nameof(præª))]
    public class T12050gõÊøópr : ModelBase
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
        /// øóñ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("øóñ", Order = 4)]
        public short øóñ { get; set; }

        /// <summary>
        /// gõR[h
        /// </summary>
        [Required]
        [Column("gõR[h", Order = 5)]
        [StringLength(13)]
        public string gõR[h { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 6)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// vPÊnæR[h
        /// </summary>
        [Required]
        [Column("vPÊnæR[h", Order = 7)]
        [StringLength(5)]
        public string vPÊnæR[h { get; set; }

        /// <summary>
        /// ìtú
        /// </summary>
        [Required]
        [Column("ìtú", Order = 8)]
        [StringLength(1)]
        public string ìtú { get; set; }

        /// <summary>
        /// præª
        /// </summary>
        [Required]
        [Column("præª", Order = 9)]
        [StringLength(3)]
        public string præª { get; set; }

        /// <summary>
        /// íÞæª
        /// </summary>
        [Column("íÞæª")]
        [StringLength(1)]
        public string íÞæª { get; set; }

        /// <summary>
        /// c¨æª
        /// </summary>
        [Column("c¨æª")]
        [StringLength(1)]
        public string c¨æª { get; set; }

        /// <summary>
        /// øóM
        /// </summary>
        [Column("øóM")]
        public Decimal? øóM { get; set; }

        /// <summary>
        /// øóÊÏv
        /// </summary>
        [Column("øóÊÏv")]
        public Decimal? øóÊÏv { get; set; }

        /// <summary>
        /// îûnÊv
        /// </summary>
        [Column("îûnÊv")]
        public Decimal? îûnÊv { get; set; }

        /// <summary>
        /// øóûÊ
        /// </summary>
        [Column("øóûÊ")]
        public Decimal? øóûÊ { get; set; }

        /// <summary>
        /// ¤ÏàzIðÊ
        /// </summary>
        [Column("¤ÏàzIðÊ")]
        public Decimal? ¤ÏàzIðÊ { get; set; }

        /// <summary>
        /// ¤ÏàzP¿
        /// </summary>
        [Column("¤ÏàzP¿")]
        public Decimal? ¤ÏàzP¿ { get; set; }

        /// <summary>
        /// ¤Ïàz
        /// </summary>
        [Column("¤Ïàz")]
        public Decimal? ¤Ïàz { get; set; }

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
