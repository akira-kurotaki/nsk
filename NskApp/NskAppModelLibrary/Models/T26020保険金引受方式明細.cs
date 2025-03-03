using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_26020_•ÛŒ¯‹à_ˆøó•û®–¾×
    /// </summary>
    [Serializable]
    [Table("t_26020_•ÛŒ¯‹à_ˆøó•û®–¾×")]
    [PrimaryKey(nameof(‘g‡“™ƒR[ƒh), nameof(”NY), nameof(‹¤Ï–Ú“IƒR[ƒh), nameof(—Ş‹æ•ª), nameof(‡•¹¯•ÊƒR[ƒh), nameof(ˆøó•û®), nameof(•âŠ„‡ƒR[ƒh), nameof(’P“–‹¤Ï‹àŠz), nameof(¿‹‰ñ))]
    public class T26020•ÛŒ¯‹àˆøó•û®–¾× : ModelBase
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
        /// —Ş‹æ•ª
        /// </summary>
        [Required]
        [Column("—Ş‹æ•ª", Order = 4)]
        [StringLength(2)]
        public string —Ş‹æ•ª { get; set; }

        /// <summary>
        /// ‡•¹¯•ÊƒR[ƒh
        /// </summary>
        [Required]
        [Column("‡•¹¯•ÊƒR[ƒh", Order = 5)]
        [StringLength(3)]
        public string ‡•¹¯•ÊƒR[ƒh { get; set; }

        /// <summary>
        /// ˆøó•û®
        /// </summary>
        [Required]
        [Column("ˆøó•û®", Order = 6)]
        [StringLength(1)]
        public string ˆøó•û® { get; set; }

        /// <summary>
        /// •âŠ„‡ƒR[ƒh
        /// </summary>
        [Required]
        [Column("•âŠ„‡ƒR[ƒh", Order = 7)]
        [StringLength(2)]
        public string •âŠ„‡ƒR[ƒh { get; set; }

        /// <summary>
        /// ’P“–‹¤Ï‹àŠz
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("’P“–‹¤Ï‹àŠz", Order = 8)]
        public Decimal ’P“–‹¤Ï‹àŠz { get; set; }

        /// <summary>
        /// ¿‹‰ñ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("¿‹‰ñ", Order = 9)]
        public short ¿‹‰ñ { get; set; }

        /// <summary>
        /// ˆøóŒË”
        /// </summary>
        [Column("ˆøóŒË”")]
        public Decimal? ˆøóŒË” { get; set; }

        /// <summary>
        /// ˆøó–ÊÏ
        /// </summary>
        [Column("ˆøó–ÊÏ")]
        public Decimal? ˆøó–ÊÏ { get; set; }

        /// <summary>
        /// ˆøóû—Ê
        /// </summary>
        [Column("ˆøóû—Ê")]
        public Decimal? ˆøóû—Ê { get; set; }

        /// <summary>
        /// ‹¤Ï‹àŠz
        /// </summary>
        [Column("‹¤Ï‹àŠz")]
        public Decimal? ‹¤Ï‹àŠz { get; set; }

        /// <summary>
        /// ’´‰ßx•¥‘ÎÛˆøó–ÊÏ
        /// </summary>
        [Column("’´‰ßx•¥‘ÎÛˆøó–ÊÏ")]
        public Decimal? ’´‰ßx•¥‘ÎÛˆøó–ÊÏ { get; set; }

        /// <summary>
        /// ’´‰ßx•¥‘ÎÛŒË”
        /// </summary>
        [Column("’´‰ßx•¥‘ÎÛŒË”")]
        public Decimal? ’´‰ßx•¥‘ÎÛŒË” { get; set; }

        /// <summary>
        /// ’´‰ßx•¥‘ÎÛ–ÊÏ
        /// </summary>
        [Column("’´‰ßx•¥‘ÎÛ–ÊÏ")]
        public Decimal? ’´‰ßx•¥‘ÎÛ–ÊÏ { get; set; }

        /// <summary>
        /// ’´‰ßx•¥‘ÎÛˆøóû—Ê
        /// </summary>
        [Column("’´‰ßx•¥‘ÎÛˆøóû—Ê")]
        public Decimal? ’´‰ßx•¥‘ÎÛˆøóû—Ê { get; set; }

        /// <summary>
        /// ’´‰ßx•¥‹¤ÏŒ¸û—Ê
        /// </summary>
        [Column("’´‰ßx•¥‹¤ÏŒ¸û—Ê")]
        public Decimal? ’´‰ßx•¥‹¤ÏŒ¸û—Ê { get; set; }

        /// <summary>
        /// ’´‰ß”íŠQx•¥‹¤Ï‹à
        /// </summary>
        [Column("’´‰ß”íŠQx•¥‹¤Ï‹à")]
        public Decimal? ’´‰ß”íŠQx•¥‹¤Ï‹à { get; set; }

        /// <summary>
        /// ˆê•M‘S‘¹x•¥‘ÎÛŒË”
        /// </summary>
        [Column("ˆê•M‘S‘¹x•¥‘ÎÛŒË”")]
        public Decimal? ˆê•M‘S‘¹x•¥‘ÎÛŒË” { get; set; }

        /// <summary>
        /// ˆê•M‘S‘¹x•¥‘ÎÛ–ÊÏ
        /// </summary>
        [Column("ˆê•M‘S‘¹x•¥‘ÎÛ–ÊÏ")]
        public Decimal? ˆê•M‘S‘¹x•¥‘ÎÛ–ÊÏ { get; set; }

        /// <summary>
        /// ˆê•M‘S‘¹x•¥‹¤ÏŒ¸û—Ê
        /// </summary>
        [Column("ˆê•M‘S‘¹x•¥‹¤ÏŒ¸û—Ê")]
        public Decimal? ˆê•M‘S‘¹x•¥‹¤ÏŒ¸û—Ê { get; set; }

        /// <summary>
        /// ˆê•M‘S‘¹x•¥‹¤Ï‹à
        /// </summary>
        [Column("ˆê•M‘S‘¹x•¥‹¤Ï‹à")]
        public Decimal? ˆê•M‘S‘¹x•¥‹¤Ï‹à { get; set; }

        /// <summary>
        /// ˆê•M”¼‘¹x•¥‘ÎÛŒË”
        /// </summary>
        [Column("ˆê•M”¼‘¹x•¥‘ÎÛŒË”")]
        public Decimal? ˆê•M”¼‘¹x•¥‘ÎÛŒË” { get; set; }

        /// <summary>
        /// ˆê•M”¼‘¹x•¥‘ÎÛ–ÊÏ
        /// </summary>
        [Column("ˆê•M”¼‘¹x•¥‘ÎÛ–ÊÏ")]
        public Decimal? ˆê•M”¼‘¹x•¥‘ÎÛ–ÊÏ { get; set; }

        /// <summary>
        /// ˆê•M”¼‘¹x•¥‹¤ÏŒ¸û—Ê
        /// </summary>
        [Column("ˆê•M”¼‘¹x•¥‹¤ÏŒ¸û—Ê")]
        public Decimal? ˆê•M”¼‘¹x•¥‹¤ÏŒ¸û—Ê { get; set; }

        /// <summary>
        /// ˆê•M”¼‘¹x•¥‹¤Ï‹à
        /// </summary>
        [Column("ˆê•M”¼‘¹x•¥‹¤Ï‹à")]
        public Decimal? ˆê•M”¼‘¹x•¥‹¤Ï‹à { get; set; }

        /// <summary>
        /// ˆê•M“Á—áx•¥‘ÎÛŒË”
        /// </summary>
        [Column("ˆê•M“Á—áx•¥‘ÎÛŒË”")]
        public Decimal? ˆê•M“Á—áx•¥‘ÎÛŒË” { get; set; }

        /// <summary>
        /// ˆê•M“Á—áx•¥‘ÎÛ–ÊÏ
        /// </summary>
        [Column("ˆê•M“Á—áx•¥‘ÎÛ–ÊÏ")]
        public Decimal? ˆê•M“Á—áx•¥‘ÎÛ–ÊÏ { get; set; }

        /// <summary>
        /// ˆê•M“Á—áx•¥‹¤ÏŒ¸û—Ê
        /// </summary>
        [Column("ˆê•M“Á—áx•¥‹¤ÏŒ¸û—Ê")]
        public Decimal? ˆê•M“Á—áx•¥‹¤ÏŒ¸û—Ê { get; set; }

        /// <summary>
        /// ˆê•M“Á—áx•¥‹¤Ï‹à
        /// </summary>
        [Column("ˆê•M“Á—áx•¥‹¤Ï‹à")]
        public Decimal? ˆê•M“Á—áx•¥‹¤Ï‹à { get; set; }

        /// <summary>
        /// x•¥‘ÎÛŒË”
        /// </summary>
        [Column("x•¥‘ÎÛŒË”")]
        public Decimal? x•¥‘ÎÛŒË” { get; set; }

        /// <summary>
        /// x•¥‘ÎÛ–ÊÏ
        /// </summary>
        [Column("x•¥‘ÎÛ–ÊÏ")]
        public Decimal? x•¥‘ÎÛ–ÊÏ { get; set; }

        /// <summary>
        /// x•¥‘ÎÛ‹¤ÏŒ¸û—Ê
        /// </summary>
        [Column("x•¥‘ÎÛ‹¤ÏŒ¸û—Ê")]
        public Decimal? x•¥‘ÎÛ‹¤ÏŒ¸û—Ê { get; set; }

        /// <summary>
        /// x•¥‹¤Ï‹à
        /// </summary>
        [Column("x•¥‹¤Ï‹à")]
        public Decimal? x•¥‹¤Ï‹à { get; set; }

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
