using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_15020_gðt
    /// </summary>
    [Serializable]
    [Table("t_15020_gðt")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(Sàðtæª), nameof(ðtñ))]
    public class T15020gðt : ModelBase
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
        /// Sàðtæª
        /// </summary>
        [Required]
        [Column("Sàðtæª", Order = 3)]
        [StringLength(2)]
        public string Sàðtæª { get; set; }

        /// <summary>
        /// ðtñ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ðtñ", Order = 4)]
        public short ðtñ { get; set; }

        /// <summary>
        /// øóÊÏ
        /// </summary>
        [Column("øóÊÏ")]
        public Decimal? øóÊÏ { get; set; }

        /// <summary>
        /// øóûÊ
        /// </summary>
        [Column("øóûÊ")]
        public Decimal? øóûÊ { get; set; }

        /// <summary>
        /// ¤Ïàz
        /// </summary>
        [Column("¤Ïàz")]
        public Decimal? ¤Ïàz { get; set; }

        /// <summary>
        /// Û¯¿z
        /// </summary>
        [Column("Û¯¿z")]
        public Decimal? Û¯¿z { get; set; }

        /// <summary>
        /// gÊÉSà
        /// </summary>
        [Column("gÊÉSà")]
        public Decimal? gÊÉSà { get; set; }

        /// <summary>
        /// gõS¤Ï|à
        /// </summary>
        [Column("gõS¤Ï|à")]
        public Decimal? gõS¤Ï|à { get; set; }

        /// <summary>
        /// gõS¤Ï|à¥ûÏz
        /// </summary>
        [Column("gõS¤Ï|à¥ûÏz")]
        public Decimal? gõS¤Ï|à¥ûÏz { get; set; }

        /// <summary>
        /// ¤Ï|à¥û
        /// </summary>
        [Column("¤Ï|à¥û")]
        public Decimal? ¤Ï|à¥û { get; set; }

        /// <summary>
        /// gðtàÌàz
        /// </summary>
        [Column("gðtàÌàz")]
        public Decimal? gðtàÌàz { get; set; }

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
