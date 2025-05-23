using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_22010_gõÊ¼nîñ
    /// </summary>
    [Serializable]
    [Table("t_22010_gõÊ¼nîñ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(¼nñ))]
    public class T22010gõÊ¼nîñ : ModelBase
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
        /// ¼nñ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("¼nñ", Order = 5)]
        public short ¼nñ { get; set; }

        /// <summary>
        /// ¼nÎÛtO
        /// </summary>
        [Column("¼nÎÛtO")]
        [StringLength(1)]
        public string ¼nÎÛtO { get; set; }

        /// <summary>
        /// ¼nvZz
        /// </summary>
        [Column("¼nvZz")]
        public Decimal? ¼nvZz { get; set; }

        /// <summary>
        /// ¼nx¥z
        /// </summary>
        [Column("¼nx¥z")]
        public Decimal? ¼nx¥z { get; set; }

        /// <summary>
        /// õl
        /// </summary>
        [Column("õl")]
        [StringLength(100)]
        public string õl { get; set; }

        /// <summary>
        /// ¼nx¥ÎÛtO
        /// </summary>
        [Column("¼nx¥ÎÛtO")]
        [StringLength(1)]
        public string ¼nx¥ÎÛtO { get; set; }

        /// <summary>
        /// ¼nx¥ú
        /// </summary>
        [Column("¼nx¥ú")]
        public DateTime? ¼nx¥ú { get; set; }

        /// <summary>
        /// ¹¯ÊR[h
        /// </summary>
        [Column("¹¯ÊR[h")]
        [StringLength(3)]
        public string ¹¯ÊR[h { get; set; }

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
