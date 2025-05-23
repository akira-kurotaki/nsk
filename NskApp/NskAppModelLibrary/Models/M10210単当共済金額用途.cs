using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10210_P¤Ïàzpr
    /// </summary>
    [Serializable]
    [Table("m_10210_P¤Ïàzpr")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(íÞæª), nameof(ìtú), nameof(øóæª), nameof(præª), nameof(ÛÅP¿æª), nameof(P¤ÏàzÊ))]
    public class M10210P¤Ïàzpr : ModelBase
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
        /// íÞæª
        /// </summary>
        [Required]
        [Column("íÞæª", Order = 4)]
        [StringLength(1)]
        public string íÞæª { get; set; }

        /// <summary>
        /// ìtú
        /// </summary>
        [Required]
        [Column("ìtú", Order = 5)]
        [StringLength(1)]
        public string ìtú { get; set; }

        /// <summary>
        /// øóæª
        /// </summary>
        [Required]
        [Column("øóæª", Order = 6)]
        [StringLength(2)]
        public string øóæª { get; set; }

        /// <summary>
        /// præª
        /// </summary>
        [Required]
        [Column("præª", Order = 7)]
        [StringLength(3)]
        public string præª { get; set; }

        /// <summary>
        /// ÛÅP¿æª
        /// </summary>
        [Required]
        [Column("ÛÅP¿æª", Order = 8)]
        [StringLength(1)]
        public string ÛÅP¿æª { get; set; }

        /// <summary>
        /// P¤ÏàzÊ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("P¤ÏàzÊ", Order = 9)]
        public Decimal P¤ÏàzÊ { get; set; }

        /// <summary>
        /// P¤Ïàz
        /// </summary>
        [Column("P¤Ïàz")]
        public Decimal? P¤Ïàz { get; set; }

        /// <summary>
        /// ÜÊ¥tO
        /// </summary>
        [Column("ÜÊ¥tO")]
        [StringLength(1)]
        public string ÜÊ¥tO { get; set; }

        /// <summary>
        /// §tO
        /// </summary>
        [Column("§tO")]
        [StringLength(1)]
        public string §tO { get; set; }

        /// <summary>
        /// õl
        /// </summary>
        [Column("õl")]
        [StringLength(50)]
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
