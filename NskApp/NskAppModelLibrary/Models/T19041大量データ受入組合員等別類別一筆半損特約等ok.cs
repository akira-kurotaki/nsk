using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_19040_åÊf[^óü_gõÊÞÊêM¼¹Áño
    /// </summary>
    [Serializable]
    [Table("t_19041_åÊf[^óü_gõÊÞÊêM¼¹Áñok")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(æª), nameof(gõR[h))]
    public class T19041åÊf[^óügõÊÞÊêM¼¹Áñok : ModelBase
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
        /// æª
        /// </summary>
        [Required]
        [Column("æª", Order = 4)]
        [StringLength(1)]
        public string æª { get; set; }

        /// <summary>
        /// gõR[h
        /// </summary>
        [Required]
        [Column("gõR[h", Order = 5)]
        [StringLength(13)]
        public string gõR[h { get; set; }

        /// <summary>
        /// ånæR[h
        /// </summary>
        [Column("ånæR[h")]
        [StringLength(2)]
        public string ånæR[h { get; set; }

        /// <summary>
        /// ¬næR[h
        /// </summary>
        [Column("¬næR[h")]
        [StringLength(4)]
        public string ¬næR[h { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Column("Þæª")]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// Áñæª
        /// </summary>
        [Column("Áñæª")]
        [StringLength(1)]
        public string Áñæª { get; set; }

        /// <summary>
        /// ûnÊ²¸û@æª
        /// </summary>
        [Column("ûnÊ²¸û@æª")]
        [StringLength(1)]
        public string ûnÊ²¸û@æª { get; set; }

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
