using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_25020_gõÊx¥îñ
    /// </summary>
    [Serializable]
    [Table("t_25020_gõÊx¥îñ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(x¥ñ))]
    public class T25020gõÊx¥îñ : ModelBase
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
        /// x¥ñ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("x¥ñ", Order = 5)]
        public short x¥ñ { get; set; }

        /// <summary>
        /// x¥¤Ïàz
        /// </summary>
        [Column("x¥¤Ïàz")]
        public Decimal? x¥¤Ïàz { get; set; }

        /// <summary>
        /// ¼nx¥z
        /// </summary>
        [Column("¼nx¥z")]
        public Decimal? ¼nx¥z { get; set; }

        /// <summary>
        /// ÆÓz
        /// </summary>
        [Column("ÆÓz")]
        public Decimal? ÆÓz { get; set; }

        /// <summary>
        /// Àx¥àz
        /// </summary>
        [Column("Àx¥àz")]
        public Decimal? Àx¥àz { get; set; }

        /// <summary>
        /// õl
        /// </summary>
        [Column("õl")]
        [StringLength(100)]
        public string õl { get; set; }

        /// <summary>
        /// èvZút
        /// </summary>
        [Column("èvZút")]
        public DateTime? èvZút { get; set; }

        /// <summary>
        /// x¥Nú
        /// </summary>
        [Column("x¥Nú")]
        public DateTime? x¥Nú { get; set; }

        /// <summary>
        /// x¥Êm­sú
        /// </summary>
        [Column("x¥Êm­sú")]
        public DateTime? x¥Êm­sú { get; set; }

        /// <summary>
        /// óüÏtO
        /// </summary>
        [Column("óüÏtO")]
        [StringLength(1)]
        public string óüÏtO { get; set; }

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
