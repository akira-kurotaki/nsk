using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_29120_åÊf[^óü_SEÅ±\²¸ok
    /// </summary>
    [Serializable]
    [Table("t_29120_åÊf[^óü_SEÅ±\²¸ok")]
    [PrimaryKey(nameof(óüðid), nameof(sÔ))]
    public class T29120åÊf[^óüSEÅ±\²¸ok : ModelBase
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
        [Column("sÔ", Order = 2)]
        [StringLength(6)]
        public string sÔ { get; set; }

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
        /// Þæª
        /// </summary>
        [Column("Þæª")]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// gõR[h
        /// </summary>
        [Column("gõR[h")]
        [StringLength(13)]
        public string gõR[h { get; set; }

        /// <summary>
        /// præª
        /// </summary>
        [Column("præª")]
        [StringLength(3)]
        public string præª { get; set; }

        /// <summary>
        /// ãÊ
        /// </summary>
        [Column("ãÊ")]
        public Decimal? ãÊ { get; set; }

        /// <summary>
        /// ÆÁïÊ
        /// </summary>
        [Column("ÆÁïÊ")]
        public Decimal? ÆÁïÊ { get; set; }

        /// <summary>
        /// püS¸Ê
        /// </summary>
        [Column("püS¸Ê")]
        public Decimal? püS¸Ê { get; set; }

        /// <summary>
        /// úIµÊ
        /// </summary>
        [Column("úIµÊ")]
        public Decimal? úIµÊ { get; set; }

        /// <summary>
        /// úñIµÊ
        /// </summary>
        [Column("úñIµÊ")]
        public Decimal? úñIµÊ { get; set; }

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
