using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_11030_ÂlÝèÞ
    /// </summary>
    [Serializable]
    [Table("t_11030_ÂlÝèÞ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(Þæª), nameof(øóæª))]
    public class T11030ÂlÝèÞ : ModelBase
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
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 5)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// øóæª
        /// </summary>
        [Required]
        [Column("øóæª", Order = 6)]
        [StringLength(2)]
        public string øóæª { get; set; }

        /// <summary>
        /// øóû®
        /// </summary>
        [Column("øóû®")]
        [StringLength(1)]
        public string øóû® { get; set; }

        /// <summary>
        /// Áñæª
        /// </summary>
        [Column("Áñæª")]
        [StringLength(1)]
        public string Áñæª { get; set; }

        /// <summary>
        /// âR[h
        /// </summary>
        [Column("âR[h")]
        [StringLength(2)]
        public string âR[h { get; set; }

        /// <summary>
        /// tÛ
        /// </summary>
        [Column("tÛ")]
        public Decimal? tÛ { get; set; }

        /// <summary>
        /// íÞæª
        /// </summary>
        [Column("íÞæª")]
        [StringLength(1)]
        public string íÞæª { get; set; }

        /// <summary>
        /// ìtú
        /// </summary>
        [Column("ìtú")]
        [StringLength(1)]
        public string ìtú { get; set; }

        /// <summary>
        /// c¨æª
        /// </summary>
        [Column("c¨æª")]
        [StringLength(1)]
        public string c¨æª { get; set; }

        /// <summary>
        /// ¤ÏàzIðÊ
        /// </summary>
        [Column("¤ÏàzIðÊ")]
        public Decimal? ¤ÏàzIðÊ { get; set; }

        /// <summary>
        /// ûnÊmFû@
        /// </summary>
        [Column("ûnÊmFû@")]
        [StringLength(2)]
        public string ûnÊmFû@ { get; set; }

        /// <summary>
        /// SEîPû
        /// </summary>
        [Column("SEîPû")]
        public Decimal? SEîPû { get; set; }

        /// <summary>
        /// c_ÎÛOtO
        /// </summary>
        [Column("c_ÎÛOtO")]
        [StringLength(1)]
        public string c_ÎÛOtO { get; set; }

        /// <summary>
        /// Sè_Ææª
        /// </summary>
        [Column("Sè_Ææª")]
        [StringLength(1)]
        public string Sè_Ææª { get; set; }

        /// <summary>
        /// SEóõÒ¼Ì
        /// </summary>
        [Column("SEóõÒ¼Ì")]
        [StringLength(30)]
        public string SEóõÒ¼Ì { get; set; }

        /// <summary>
        /// õl
        /// </summary>
        [Column("õl")]
        [StringLength(30)]
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
