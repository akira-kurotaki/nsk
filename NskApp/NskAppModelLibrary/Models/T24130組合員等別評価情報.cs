using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24130_‘g‡ˆõ“™•Ê•]‰¿î•ñ
    /// </summary>
    [Serializable]
    [Table("t_24130_‘g‡ˆõ“™•Ê•]‰¿î•ñ")]
    [PrimaryKey(nameof(‘g‡“™ƒR[ƒh), nameof(”NY), nameof(‹¤Ï–Ú“IƒR[ƒh), nameof(‘g‡ˆõ“™ƒR[ƒh), nameof(‰c”_’²®ƒtƒ‰ƒO), nameof(¸Z‹æ•ª))]
    public class T24130‘g‡ˆõ“™•Ê•]‰¿î•ñ : ModelBase
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
        /// ‘g‡ˆõ“™ƒR[ƒh
        /// </summary>
        [Required]
        [Column("‘g‡ˆõ“™ƒR[ƒh", Order = 4)]
        [StringLength(13)]
        public string ‘g‡ˆõ“™ƒR[ƒh { get; set; }

        /// <summary>
        /// ‰c”_’²®ƒtƒ‰ƒO
        /// </summary>
        [Required]
        [Column("‰c”_’²®ƒtƒ‰ƒO", Order = 5)]
        [StringLength(1)]
        public string ‰c”_’²®ƒtƒ‰ƒO { get; set; }

        /// <summary>
        /// ¸Z‹æ•ª
        /// </summary>
        [Required]
        [Column("¸Z‹æ•ª", Order = 6)]
        [StringLength(1)]
        public string ¸Z‹æ•ª { get; set; }

        /// <summary>
        /// •âŠ„‡
        /// </summary>
        [Column("•âŠ„‡")]
        [StringLength(2)]
        public string •âŠ„‡ { get; set; }

        /// <summary>
        /// —Ş‹æ•ª
        /// </summary>
        [Column("—Ş‹æ•ª")]
        [StringLength(2)]
        public string —Ş‹æ•ª { get; set; }

        /// <summary>
        /// ‹¤ÏŠ|‹à
        /// </summary>
        [Column("‹¤ÏŠ|‹à")]
        public Decimal? ‹¤ÏŠ|‹à { get; set; }

        /// <summary>
        /// •Š‰Û‹à‡Œv
        /// </summary>
        [Column("•Š‰Û‹à‡Œv")]
        public Decimal? •Š‰Û‹à‡Œv { get; set; }

        /// <summary>
        /// ˆøó•M”
        /// </summary>
        [Column("ˆøó•M”")]
        public Decimal? ˆøó•M” { get; set; }

        /// <summary>
        /// ˆøó–ÊÏ
        /// </summary>
        [Column("ˆøó–ÊÏ")]
        public Decimal? ˆøó–ÊÏ { get; set; }

        /// <summary>
        /// ”íŠQ–ÊÏ_ˆê•M‘S‘¹
        /// </summary>
        [Column("”íŠQ–ÊÏ_ˆê•M‘S‘¹")]
        public Decimal? ”íŠQ–ÊÏ_ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// ”íŠQ–ÊÏ_ˆê•M”¼‘¹
        /// </summary>
        [Column("”íŠQ–ÊÏ_ˆê•M”¼‘¹")]
        public Decimal? ”íŠQ–ÊÏ_ˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// Šî€ûŠn—Ê
        /// </summary>
        [Column("Šî€ûŠn—Ê")]
        public Decimal? Šî€ûŠn—Ê { get; set; }

        /// <summary>
        /// Šî€¶Y‹àŠz
        /// </summary>
        [Column("Šî€¶Y‹àŠz")]
        public Decimal? Šî€¶Y‹àŠz { get; set; }

        /// <summary>
        /// Šî€¶Y‹àŠz_ˆê•M‘S‘¹
        /// </summary>
        [Column("Šî€¶Y‹àŠz_ˆê•M‘S‘¹")]
        public Decimal? Šî€¶Y‹àŠz_ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// Šî€¶Y‹àŠz_ˆê•M”¼‘¹
        /// </summary>
        [Column("Šî€¶Y‹àŠz_ˆê•M”¼‘¹")]
        public Decimal? Šî€¶Y‹àŠz_ˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// ‹¤ÏŒÀ“xŠz
        /// </summary>
        [Column("‹¤ÏŒÀ“xŠz")]
        public Decimal? ‹¤ÏŒÀ“xŠz { get; set; }

        /// <summary>
        /// ‹¤Ï‹àŠz
        /// </summary>
        [Column("‹¤Ï‹àŠz")]
        public Decimal? ‹¤Ï‹àŠz { get; set; }

        /// <summary>
        /// “ü‹àŠz
        /// </summary>
        [Column("“ü‹àŠz")]
        public Decimal? “ü‹àŠz { get; set; }

        /// <summary>
        /// ©‰Æ•Û—L”—Ê
        /// </summary>
        [Column("©‰Æ•Û—L”—Ê")]
        public Decimal? ©‰Æ•Û—L”—Ê { get; set; }

        /// <summary>
        /// o‰×”—Ê“™
        /// </summary>
        [Column("o‰×”—Ê“™")]
        public Decimal? o‰×”—Ê“™ { get; set; }

        /// <summary>
        /// ûŠn—Ê
        /// </summary>
        [Column("ûŠn—Ê")]
        public Decimal? ûŠn—Ê { get; set; }

        /// <summary>
        /// •ªŠ„Œ¸û—Ê
        /// </summary>
        [Column("•ªŠ„Œ¸û—Ê")]
        public Decimal? •ªŠ„Œ¸û—Ê { get; set; }

        /// <summary>
        /// •ªŠ„ŒãûŠn—Ê
        /// </summary>
        [Column("•ªŠ„ŒãûŠn—Ê")]
        public Decimal? •ªŠ„ŒãûŠn—Ê { get; set; }

        /// <summary>
        /// ’²®ŒãûŠn—Ê
        /// </summary>
        [Column("’²®ŒãûŠn—Ê")]
        public Decimal? ’²®ŒãûŠn—Ê { get; set; }

        /// <summary>
        /// ¶Y‹àŠz
        /// </summary>
        [Column("¶Y‹àŠz")]
        public Decimal? ¶Y‹àŠz { get; set; }

        /// <summary>
        /// ˆÚA•s”\k’n’²®Šz_ˆê•M‘S‘¹
        /// </summary>
        [Column("ˆÚA•s”\k’n’²®Šz_ˆê•M‘S‘¹")]
        public Decimal? ˆÚA•s”\k’n’²®Šz_ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// ¶Y‹àŠz_ˆê•M‘S‘¹
        /// </summary>
        [Column("¶Y‹àŠz_ˆê•M‘S‘¹")]
        public Decimal? ¶Y‹àŠz_ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// ¶Y‹àŠz_ˆê•M”¼‘¹
        /// </summary>
        [Column("¶Y‹àŠz_ˆê•M”¼‘¹")]
        public Decimal? ¶Y‹àŠz_ˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// Œ¸û—Ê
        /// </summary>
        [Column("Œ¸û—Ê")]
        public Decimal? Œ¸û—Ê { get; set; }

        /// <summary>
        /// ¶Y‹àŠz‚ÌŒ¸­Šz
        /// </summary>
        [Column("¶Y‹àŠz‚ÌŒ¸­Šz")]
        public Decimal? ¶Y‹àŠz‚ÌŒ¸­Šz { get; set; }

        /// <summary>
        /// ¶Y‹àŠz‚ÌŒ¸­Šz_ˆê•M‘S‘¹
        /// </summary>
        [Column("¶Y‹àŠz‚ÌŒ¸­Šz_ˆê•M‘S‘¹")]
        public Decimal? ¶Y‹àŠz‚ÌŒ¸­Šz_ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// ¶Y‹àŠz‚ÌŒ¸­Šz_ˆê•M”¼‘¹
        /// </summary>
        [Column("¶Y‹àŠz‚ÌŒ¸­Šz_ˆê•M”¼‘¹")]
        public Decimal? ¶Y‹àŠz‚ÌŒ¸­Šz_ˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// ¶Y‹àŠz‚ÌŒ¸­Šz_Œˆ’èŠz
        /// </summary>
        [Column("¶Y‹àŠz‚ÌŒ¸­Šz_Œˆ’èŠz")]
        public Decimal? ¶Y‹àŠz‚ÌŒ¸­Šz_Œˆ’èŠz { get; set; }

        /// <summary>
        /// ‘g“–x•¥‘ÎÛƒtƒ‰ƒO
        /// </summary>
        [Column("‘g“–x•¥‘ÎÛƒtƒ‰ƒO")]
        [StringLength(1)]
        public string ‘g“–x•¥‘ÎÛƒtƒ‰ƒO { get; set; }

        /// <summary>
        /// x•¥‹¤Ï‹à_x•¥—¦’²®‘O
        /// </summary>
        [Column("x•¥‹¤Ï‹à_x•¥—¦’²®‘O")]
        public Decimal? x•¥‹¤Ï‹à_x•¥—¦’²®‘O { get; set; }

        /// <summary>
        /// x•¥‹¤Ï‹à_x•¥—¦’²®‘O_“à’´‰ß”íŠQ
        /// </summary>
        [Column("x•¥‹¤Ï‹à_x•¥—¦’²®‘O_“à’´‰ß”íŠQ")]
        public Decimal? x•¥‹¤Ï‹à_x•¥—¦’²®‘O_“à’´‰ß”íŠQ { get; set; }

        /// <summary>
        /// x•¥‹¤Ï‹à_x•¥—¦’²®‘O_“àˆê•M‘S‘¹
        /// </summary>
        [Column("x•¥‹¤Ï‹à_x•¥—¦’²®‘O_“àˆê•M‘S‘¹")]
        public Decimal? x•¥‹¤Ï‹à_x•¥—¦’²®‘O_“àˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// x•¥‹¤Ï‹à_x•¥—¦’²®‘O_“àˆê•M”¼‘¹
        /// </summary>
        [Column("x•¥‹¤Ï‹à_x•¥—¦’²®‘O_“àˆê•M”¼‘¹")]
        public Decimal? x•¥‹¤Ï‹à_x•¥—¦’²®‘O_“àˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// x•¥‹¤Ï‹à
        /// </summary>
        [Column("x•¥‹¤Ï‹à")]
        public Decimal? x•¥‹¤Ï‹à { get; set; }

        /// <summary>
        /// x•¥‹¤Ï‹à_“à’´‰ß”íŠQ
        /// </summary>
        [Column("x•¥‹¤Ï‹à_“à’´‰ß”íŠQ")]
        public Decimal? x•¥‹¤Ï‹à_“à’´‰ß”íŠQ { get; set; }

        /// <summary>
        /// x•¥‹¤Ï‹à_“àˆê•M‘S‘¹
        /// </summary>
        [Column("x•¥‹¤Ï‹à_“àˆê•M‘S‘¹")]
        public Decimal? x•¥‹¤Ï‹à_“àˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// x•¥‹¤Ï‹à_“àˆê•M”¼‘¹
        /// </summary>
        [Column("x•¥‹¤Ï‹à_“àˆê•M”¼‘¹")]
        public Decimal? x•¥‹¤Ï‹à_“àˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// –ÆÓŠz
        /// </summary>
        [Column("–ÆÓŠz")]
        public Decimal? –ÆÓŠz { get; set; }

        /// <summary>
        /// Àx•¥‹¤Ï‹à
        /// </summary>
        [Column("Àx•¥‹¤Ï‹à")]
        public Decimal? Àx•¥‹¤Ï‹à { get; set; }

        /// <summary>
        /// Àx•¥‹¤Ï‹à_“à’´‰ß”íŠQ
        /// </summary>
        [Column("Àx•¥‹¤Ï‹à_“à’´‰ß”íŠQ")]
        public Decimal? Àx•¥‹¤Ï‹à_“à’´‰ß”íŠQ { get; set; }

        /// <summary>
        /// Àx•¥‹¤Ï‹à_“àˆê•M‘S‘¹
        /// </summary>
        [Column("Àx•¥‹¤Ï‹à_“àˆê•M‘S‘¹")]
        public Decimal? Àx•¥‹¤Ï‹à_“àˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// Àx•¥‹¤Ï‹à_“àˆê•M”¼‘¹
        /// </summary>
        [Column("Àx•¥‹¤Ï‹à_“àˆê•M”¼‘¹")]
        public Decimal? Àx•¥‹¤Ï‹à_“àˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// ¡‰ñx•¥‹¤Ï‹à
        /// </summary>
        [Column("¡‰ñx•¥‹¤Ï‹à")]
        public Decimal? ¡‰ñx•¥‹¤Ï‹à { get; set; }

        /// <summary>
        /// ‹àŠz”íŠQ—¦
        /// </summary>
        [Column("‹àŠz”íŠQ—¦")]
        public Decimal? ‹àŠz”íŠQ—¦ { get; set; }

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
