using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_19070_åÊf[^óü_Áü\ok
    /// </summary>
    [Serializable]
    [Table("t_19070_åÊf[^óü_Áü\ok")]
    [PrimaryKey(nameof(óüðid), nameof(sÔ))]
    public class T19070åÊf[^óüÁü\ok : ModelBase
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
        /// ÍÍ
        /// </summary>
        [Required]
        [Column("ÍÍ")]
        [StringLength(2)]
        public string ÍÍ { get; set; }

        /// <summary>
        /// NYÍÍ
        /// </summary>
        [Required]
        [Column("NYÍÍ")]
        public short NYÍÍ { get; set; }

        /// <summary>
        /// ¤ÏÚIR[hÍÍ
        /// </summary>
        [Column("¤ÏÚIR[hÍÍ")]
        [StringLength(2)]
        public string ¤ÏÚIR[hÍÍ { get; set; }

        /// <summary>
        /// oæª
        /// </summary>
        [Column("oæª")]
        [StringLength(1)]
        public string oæª { get; set; }

        /// <summary>
        /// ÍÍp[^P
        /// </summary>
        [Column("ÍÍp[^P")]
        [StringLength(13)]
        public string ÍÍp[^P { get; set; }

        /// <summary>
        /// ÍÍp[^Q
        /// </summary>
        [Column("ÍÍp[^Q")]
        [StringLength(13)]
        public string ÍÍp[^Q { get; set; }

        /// <summary>
        /// ÍÍp[^R
        /// </summary>
        [Column("ÍÍp[^R")]
        [StringLength(4)]
        public string ÍÍp[^R { get; set; }

        /// <summary>
        /// út
        /// </summary>
        [Column("út")]
        public DateTime? út { get; set; }

        /// <summary>
        /// gisf[^oÍÌ^Cv
        /// </summary>
        [Column("gisf[^oÍÌ^Cv")]
        [StringLength(1)]
        public string gisf[^oÍÌ^Cv { get; set; }

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
        /// knÔ
        /// </summary>
        [Column("knÔ")]
        [StringLength(5)]
        public string knÔ { get; set; }

        /// <summary>
        /// ªMÔ
        /// </summary>
        [Column("ªMÔ")]
        [StringLength(4)]
        public string ªMÔ { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Column("Þæª")]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// n¼nÔ
        /// </summary>
        [Column("n¼nÔ")]
        [StringLength(40)]
        public string n¼nÔ { get; set; }

        /// <summary>
        /// knÊÏ
        /// </summary>
        [Column("knÊÏ")]
        public Decimal? knÊÏ { get; set; }

        /// <summary>
        /// øóÊÏ
        /// </summary>
        [Column("øóÊÏ")]
        public Decimal? øóÊÏ { get; set; }

        /// <summary>
        /// ]ìÊÏ
        /// </summary>
        [Column("]ìÊÏ")]
        public Decimal? ]ìÊÏ { get; set; }

        /// <summary>
        /// óÏõæª
        /// </summary>
        [Column("óÏõæª")]
        [StringLength(1)]
        public string óÏõæª { get; set; }

        /// <summary>
        /// õl
        /// </summary>
        [Column("õl")]
        [StringLength(40)]
        public string õl { get; set; }

        /// <summary>
        /// c¨æª
        /// </summary>
        [Column("c¨æª")]
        [StringLength(1)]
        public string c¨æª { get; set; }

        /// <summary>
        /// æªR[h
        /// </summary>
        [Column("æªR[h")]
        [StringLength(2)]
        public string æªR[h { get; set; }

        /// <summary>
        /// íÞR[h
        /// </summary>
        [Column("íÞR[h")]
        [StringLength(2)]
        public string íÞR[h { get; set; }

        /// <summary>
        /// iíR[h
        /// </summary>
        [Column("iíR[h")]
        [StringLength(3)]
        public string iíR[h { get; set; }

        /// <summary>
        /// ûÊR[h
        /// </summary>
        [Column("ûÊR[h")]
        [StringLength(3)]
        public string ûÊR[h { get; set; }

        /// <summary>
        /// QÞR[h
        /// </summary>
        [Column("QÞR[h")]
        [StringLength(3)]
        public string QÞR[h { get; set; }

        /// <summary>
        /// îPû
        /// </summary>
        [Column("îPû")]
        public Decimal? îPû { get; set; }

        /// <summary>
        /// îûnÊ
        /// </summary>
        [Column("îûnÊ")]
        public Decimal? îûnÊ { get; set; }

        /// <summary>
        /// C³út
        /// </summary>
        [Column("C³út")]
        public DateTime? C³út { get; set; }

        /// <summary>
        /// vZút
        /// </summary>
        [Column("vZút")]
        public DateTime? vZút { get; set; }

        /// <summary>
        /// NY
        /// </summary>
        [Column("NY")]
        public short? NY { get; set; }

        /// <summary>
        /// ÀÊîPû
        /// </summary>
        [Column("ÀÊîPû")]
        public Decimal? ÀÊîPû { get; set; }

        /// <summary>
        /// rsæª
        /// </summary>
        [Column("rsæª")]
        [StringLength(2)]
        public string rsæª { get; set; }

        /// <summary>
        /// Çs¹{§R[h
        /// </summary>
        [Column("Çs¹{§R[h")]
        [StringLength(4)]
        public string Çs¹{§R[h { get; set; }

        /// <summary>
        /// sæ¬ºR[h
        /// </summary>
        [Column("sæ¬ºR[h")]
        [StringLength(3)]
        public string sæ¬ºR[h { get; set; }

        /// <summary>
        /// åR[h
        /// </summary>
        [Column("åR[h")]
        [StringLength(8)]
        public string åR[h { get; set; }

        /// <summary>
        /// ¬R[h
        /// </summary>
        [Column("¬R[h")]
        [StringLength(4)]
        public string ¬R[h { get; set; }

        /// <summary>
        /// nÔ
        /// </summary>
        [Column("nÔ")]
        [StringLength(16)]
        public string nÔ { get; set; }

        /// <summary>
        /// }Ô
        /// </summary>
        [Column("}Ô")]
        [StringLength(14)]
        public string }Ô { get; set; }

        /// <summary>
        /// qÔ
        /// </summary>
        [Column("qÔ")]
        [StringLength(10)]
        public string qÔ { get; set; }

        /// <summary>
        /// ·Ô
        /// </summary>
        [Column("·Ô")]
        [StringLength(10)]
        public string ·Ô { get; set; }

        /// <summary>
        /// vs¬ºR[h
        /// </summary>
        [Column("vs¬ºR[h")]
        [StringLength(5)]
        public string vs¬ºR[h { get; set; }

        /// <summary>
        /// vPÊnæR[h
        /// </summary>
        [Column("vPÊnæR[h")]
        [StringLength(5)]
        public string vPÊnæR[h { get; set; }

        /// <summary>
        /// vPû
        /// </summary>
        [Column("vPû")]
        public Decimal? vPû { get; set; }

        /// <summary>
        /// præª
        /// </summary>
        [Column("præª")]
        [StringLength(3)]
        public string præª { get; set; }

        /// <summary>
        /// YnÊÁ¿R[h
        /// </summary>
        [Column("YnÊÁ¿R[h")]
        [StringLength(5)]
        public string YnÊÁ¿R[h { get; set; }

        /// <summary>
        /// óÏõÒR[h
        /// </summary>
        [Column("óÏõÒR[h")]
        [StringLength(13)]
        public string óÏõÒR[h { get; set; }

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
