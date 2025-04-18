using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_19050_åÊf[^óü_gõÞÊÝèok
    /// </summary>
    [Serializable]
    [Table("t_19050_åÊf[^óü_gõÞÊÝèok")]
    [PrimaryKey(nameof(óüðid), nameof(sÔ))]
    public class T19050åÊf[^óügõÞÊÝèok : ModelBase
    {
        /// <summary>
        /// óüðid
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("óüðid", Order = 1)]
        public long óüðid { get; set; }

        /// <summary>
        /// sÔ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("sÔ", Order = 2)]
        public Decimal sÔ { get; set; }

        /// <summary>
        /// æª
        /// </summary>
        [Column("æª")]
        [StringLength(1)]
        public string æª { get; set; }

        /// <summary>
        /// gR[h
        /// </summary>
        [Column("gR[h")]
        [StringLength(3)]
        public string gR[h { get; set; }

        /// <summary>
        /// NY
        /// </summary>
        [Column("NY")]
        public short? NY { get; set; }

        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Column("¤ÏÚIR[h")]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// gõR[h
        /// </summary>
        [Column("gõR[h")]
        [StringLength(13)]
        public string gõR[h { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Column("Þæª")]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// øóæª
        /// </summary>
        [Column("øóæª")]
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
        [StringLength(2)]
        public string tÛ { get; set; }

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
        [StringLength(2)]
        public string c¨æª { get; set; }

        /// <summary>
        /// ¤ÏàzIðÊ
        /// </summary>
        [Column("¤ÏàzIðÊ")]
        [StringLength(2)]
        public string ¤ÏàzIðÊ { get; set; }

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
        /// c_x¥ÈOtO
        /// </summary>
        [Column("c_x¥ÈOtO")]
        [StringLength(1)]
        public string c_x¥ÈOtO { get; set; }

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
    }
}
