using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_29090_åÊf[^óü_SEgõÞÊûnÊok
    /// </summary>
    [Serializable]
    [Table("t_29090_åÊf[^óü_SEgõÞÊûnÊok")]
    [PrimaryKey(nameof(óüðid), nameof(sÔ))]
    public class T29090åÊf[^óüSEgõÞÊûnÊok : ModelBase
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
        [Required]
        [Column("æª")]
        [StringLength(1)]
        public string æª { get; set; }

        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Required]
        [Column("¤ÏÚIR[h")]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// gR[h
        /// </summary>
        [Required]
        [Column("gR[h")]
        [StringLength(3)]
        public string gR[h { get; set; }

        /// <summary>
        /// NY
        /// </summary>
        [Required]
        [Column("NY")]
        public short NY { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª")]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// præª
        /// </summary>
        [Required]
        [Column("præª")]
        [StringLength(3)]
        public string præª { get; set; }

        /// <summary>
        /// gõR[h
        /// </summary>
        [Required]
        [Column("gõR[h")]
        [StringLength(13)]
        public string gõR[h { get; set; }

        /// <summary>
        /// ¼Ú{ÝÀüûnÊ
        /// </summary>
        [Column("¼Ú{ÝÀüûnÊ")]
        public Decimal? ¼Ú{ÝÀüûnÊ { get; set; }

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
