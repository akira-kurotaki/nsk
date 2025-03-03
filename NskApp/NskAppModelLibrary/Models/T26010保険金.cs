using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_26010_•ÛŒ¯‹à
    /// </summary>
    [Serializable]
    [Table("t_26010_•ÛŒ¯‹à")]
    [PrimaryKey(nameof(‘g‡“™ƒR[ƒh), nameof(”NY), nameof(‹¤Ï–Ú“IƒR[ƒh), nameof(‡•¹¯•ÊƒR[ƒh), nameof(ˆøó•û®), nameof(•âŠ„‡ƒR[ƒh), nameof(¿‹‰ñ))]
    public class T26010•ÛŒ¯‹à : ModelBase
    {
        /// <summary>
        /// ‘g‡“™ƒR[ƒh
        /// </summary>
        [Required]
        [Column("‘g‡“™ƒR[ƒh", Order = 1)]
        [StringLength(3)]
        public string ‘g‡“™ƒR[ƒh { get; set; }

        /// <summary>
        /// ”NY
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("”NY", Order = 2)]
        public short ”NY { get; set; }

        /// <summary>
        /// ‹¤Ï–Ú“IƒR[ƒh
        /// </summary>
        [Required]
        [Column("‹¤Ï–Ú“IƒR[ƒh", Order = 3)]
        [StringLength(2)]
        public string ‹¤Ï–Ú“IƒR[ƒh { get; set; }

        /// <summary>
        /// ‡•¹¯•ÊƒR[ƒh
        /// </summary>
        [Required]
        [Column("‡•¹¯•ÊƒR[ƒh", Order = 4)]
        [StringLength(3)]
        public string ‡•¹¯•ÊƒR[ƒh { get; set; }

        /// <summary>
        /// ˆøó•û®
        /// </summary>
        [Required]
        [Column("ˆøó•û®", Order = 5)]
        [StringLength(1)]
        public string ˆøó•û® { get; set; }

        /// <summary>
        /// •âŠ„‡ƒR[ƒh
        /// </summary>
        [Required]
        [Column("•âŠ„‡ƒR[ƒh", Order = 6)]
        [StringLength(2)]
        public string •âŠ„‡ƒR[ƒh { get; set; }

        /// <summary>
        /// ¿‹‰ñ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("¿‹‰ñ", Order = 7)]
        public short ¿‹‰ñ { get; set; }

        /// <summary>
        /// x•¥‘ÎÛŒË”
        /// </summary>
        [Column("x•¥‘ÎÛŒË”")]
        public Decimal? x•¥‘ÎÛŒË” { get; set; }

        /// <summary>
        /// ‹¤ÏŒ¸û—Ê
        /// </summary>
        [Column("‹¤ÏŒ¸û—Ê")]
        public Decimal? ‹¤ÏŒ¸û—Ê { get; set; }

        /// <summary>
        /// ’´‰ß”íŠQx•¥‹¤Ï‹à
        /// </summary>
        [Column("’´‰ß”íŠQx•¥‹¤Ï‹à")]
        public Decimal? ’´‰ß”íŠQx•¥‹¤Ï‹à { get; set; }

        /// <summary>
        /// ˆê•M‘S‘¹x•¥‹¤Ï‹à
        /// </summary>
        [Column("ˆê•M‘S‘¹x•¥‹¤Ï‹à")]
        public Decimal? ˆê•M‘S‘¹x•¥‹¤Ï‹à { get; set; }

        /// <summary>
        /// ˆê•M”¼‘¹x•¥‹¤Ï‹à
        /// </summary>
        [Column("ˆê•M”¼‘¹x•¥‹¤Ï‹à")]
        public Decimal? ˆê•M”¼‘¹x•¥‹¤Ï‹à { get; set; }

        /// <summary>
        /// x•¥‹¤Ï‹à
        /// </summary>
        [Column("x•¥‹¤Ï‹à")]
        public Decimal? x•¥‹¤Ï‹à { get; set; }

        /// <summary>
        /// x•¥•ÛŒ¯‹à
        /// </summary>
        [Column("x•¥•ÛŒ¯‹à")]
        public Decimal? x•¥•ÛŒ¯‹à { get; set; }

        /// <summary>
        /// ’ÊíÓ”C‹¤Ï‹àŠz
        /// </summary>
        [Column("’ÊíÓ”C‹¤Ï‹àŠz")]
        public Decimal? ’ÊíÓ”C‹¤Ï‹àŠz { get; set; }

        /// <summary>
        /// Ó”C•ÛŒ¯•à‡
        /// </summary>
        [Column("Ó”C•ÛŒ¯•à‡")]
        public Decimal? Ó”C•ÛŒ¯•à‡ { get; set; }

        /// <summary>
        /// ‹àŠz”íŠQ—¦
        /// </summary>
        [Column("‹àŠz”íŠQ—¦")]
        public Decimal? ‹àŠz”íŠQ—¦ { get; set; }

        /// <summary>
        /// ‹¤Ï‹àŠz
        /// </summary>
        [Column("‹¤Ï‹àŠz")]
        public Decimal? ‹¤Ï‹àŠz { get; set; }

        /// <summary>
        /// ”_ì•¨’Êí•”•ª•ÛŒ¯‹à
        /// </summary>
        [Column("”_ì•¨’Êí•”•ª•ÛŒ¯‹à")]
        public Decimal? ”_ì•¨’Êí•”•ª•ÛŒ¯‹à { get; set; }

        /// <summary>
        /// ”_ì•¨ˆÙí•”•ª•ÛŒ¯‹à
        /// </summary>
        [Column("”_ì•¨ˆÙí•”•ª•ÛŒ¯‹à")]
        public Decimal? ”_ì•¨ˆÙí•”•ª•ÛŒ¯‹à { get; set; }

        /// <summary>
        /// x•¥•ÛŒ¯‹àŠùó—ÌŠz
        /// </summary>
        [Column("x•¥•ÛŒ¯‹àŠùó—ÌŠz")]
        public Decimal? x•¥•ÛŒ¯‹àŠùó—ÌŠz { get; set; }

        /// <summary>
        /// x•¥•ÛŒ¯‹à¡‰ñ¿‹Šz
        /// </summary>
        [Column("x•¥•ÛŒ¯‹à¡‰ñ¿‹Šz")]
        public Decimal? x•¥•ÛŒ¯‹à¡‰ñ¿‹Šz { get; set; }

        /// <summary>
        /// –ÆÓŒË”
        /// </summary>
        [Column("–ÆÓŒË”")]
        public Decimal? –ÆÓŒË” { get; set; }

        /// <summary>
        /// –ÆÓŠz
        /// </summary>
        [Column("–ÆÓŠz")]
        public Decimal? –ÆÓŠz { get; set; }

        /// <summary>
        /// “o˜^“ú
        /// </summary>
        [Column("“o˜^“ú")]
        public DateTime? “o˜^“ú { get; set; }

        /// <summary>
        /// “o˜^ƒ†[ƒUid
        /// </summary>
        [Column("“o˜^ƒ†[ƒUid")]
        [StringLength(11)]
        public string “o˜^ƒ†[ƒUid { get; set; }
    }
}
