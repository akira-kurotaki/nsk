using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_23050_½ÏPû·
    /// </summary>
    [Serializable]
    [Table("t_23050_½ÏPû·")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(xR[h), nameof(Þæª), nameof(øóû®), nameof(âR[h), nameof(]¿næR[h), nameof(Kwæª))]
    public class T23050½ÏPû· : ModelBase
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
        /// xR[h
        /// </summary>
        [Required]
        [Column("xR[h", Order = 4)]
        [StringLength(2)]
        public string xR[h { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 5)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// øóû®
        /// </summary>
        [Required]
        [Column("øóû®", Order = 6)]
        [StringLength(1)]
        public string øóû® { get; set; }

        /// <summary>
        /// âR[h
        /// </summary>
        [Required]
        [Column("âR[h", Order = 7)]
        [StringLength(2)]
        public string âR[h { get; set; }

        /// <summary>
        /// ]¿næR[h
        /// </summary>
        [Required]
        [Column("]¿næR[h", Order = 8)]
        [StringLength(4)]
        public string ]¿næR[h { get; set; }

        /// <summary>
        /// Kwæª
        /// </summary>
        [Required]
        [Column("Kwæª", Order = 9)]
        [StringLength(3)]
        public string Kwæª { get; set; }

        /// <summary>
        /// »F²¸ÊÏ
        /// </summary>
        [Column("»F²¸ÊÏ")]
        public Decimal? »F²¸ÊÏ { get; set; }

        /// <summary>
        /// ²æM
        /// </summary>
        [Column("²æM")]
        public Decimal? ²æM { get; set; }

        /// <summary>
        /// ÀªM
        /// </summary>
        [Column("ÀªM")]
        public Decimal? ÀªM { get; set; }

        /// <summary>
        /// ©Pûv
        /// </summary>
        [Column("©Pûv")]
        public Decimal? ©Pûv { get; set; }

        /// <summary>
        /// ²æMÌ»FPûv
        /// </summary>
        [Column("²æMÌ»FPûv")]
        public Decimal? ²æMÌ»FPûv { get; set; }

        /// <summary>
        /// ÀªPûv
        /// </summary>
        [Column("ÀªPûv")]
        public Decimal? ÀªPûv { get; set; }

        /// <summary>
        /// ÀªMÌ©Pûv
        /// </summary>
        [Column("ÀªMÌ©Pûv")]
        public Decimal? ÀªMÌ©Pûv { get; set; }

        /// <summary>
        /// ½ÏPû·
        /// </summary>
        [Column("½ÏPû·")]
        public Decimal? ½ÏPû· { get; set; }

        /// <summary>
        /// ²®½ÏPû·
        /// </summary>
        [Column("²®½ÏPû·")]
        public Decimal? ²®½ÏPû· { get; set; }

        /// <summary>
        /// è½ÏPû·
        /// </summary>
        [Column("è½ÏPû·")]
        public Decimal? è½ÏPû· { get; set; }

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
