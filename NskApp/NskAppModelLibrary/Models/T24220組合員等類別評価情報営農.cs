using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24220_‘g‡ˆõ“™—Ş•Ê•]‰¿î•ñ_‰c”_
    /// </summary>
    [Serializable]
    [Table("t_24220_‘g‡ˆõ“™—Ş•Ê•]‰¿î•ñ_‰c”_")]
    [PrimaryKey(nameof(‘g‡“™ƒR[ƒh), nameof(”NY), nameof(‹¤Ï–Ú“IƒR[ƒh), nameof(‘g‡ˆõ“™ƒR[ƒh), nameof(—Ş‹æ•ª), nameof(¸Z‹æ•ª))]
    public class T24220‘g‡ˆõ“™—Ş•Ê•]‰¿î•ñ‰c”_ : ModelBase
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
        /// —Ş‹æ•ª
        /// </summary>
        [Required]
        [Column("—Ş‹æ•ª", Order = 5)]
        [StringLength(2)]
        public string —Ş‹æ•ª { get; set; }

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
        /// ­•{•ÛŒ¯”F’è‹æ•ª
        /// </summary>
        [Column("­•{•ÛŒ¯”F’è‹æ•ª")]
        [StringLength(4)]
        public string ­•{•ÛŒ¯”F’è‹æ•ª { get; set; }

        /// <summary>
        /// ˆøó–ÊÏ
        /// </summary>
        [Column("ˆøó–ÊÏ")]
        public Decimal? ˆøó–ÊÏ { get; set; }

        /// <summary>
        /// ˆøó–ÊÏ_‰c”_‘ÎÛŠO
        /// </summary>
        [Column("ˆøó–ÊÏ_‰c”_‘ÎÛŠO")]
        public Decimal? ˆøó–ÊÏ_‰c”_‘ÎÛŠO { get; set; }

        /// <summary>
        /// ˆøó–ÊÏ_‰c”_‘ÎÛ
        /// </summary>
        [Column("ˆøó–ÊÏ_‰c”_‘ÎÛ")]
        public Decimal? ˆøó–ÊÏ_‰c”_‘ÎÛ { get; set; }

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
        /// ¶Y‹àŠz_‰c”_’²®‘O
        /// </summary>
        [Column("¶Y‹àŠz_‰c”_’²®‘O")]
        public Decimal? ¶Y‹àŠz_‰c”_’²®‘O { get; set; }

        /// <summary>
        /// ”Ì”„û“ü‘Š“–Šz‡Œv
        /// </summary>
        [Column("”Ì”„û“ü‘Š“–Šz‡Œv")]
        public Decimal? ”Ì”„û“ü‘Š“–Šz‡Œv { get; set; }

        /// <summary>
        /// ”Ì”„û“ü‘Š“–Šz‡Œv_ˆê•M‘S‘¹
        /// </summary>
        [Column("”Ì”„û“ü‘Š“–Šz‡Œv_ˆê•M‘S‘¹")]
        public Decimal? ”Ì”„û“ü‘Š“–Šz‡Œv_ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// ”Ì”„û“ü‘Š“–Šz‡Œv_ˆê•M”¼‘¹
        /// </summary>
        [Column("”Ì”„û“ü‘Š“–Šz‡Œv_ˆê•M”¼‘¹")]
        public Decimal? ”Ì”„û“ü‘Š“–Šz‡Œv_ˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// ‰c”_Œp‘±x•¥‘Š“–Šz
        /// </summary>
        [Column("‰c”_Œp‘±x•¥‘Š“–Šz")]
        public Decimal? ‰c”_Œp‘±x•¥‘Š“–Šz { get; set; }

        /// <summary>
        /// ‰c”_Œp‘±x•¥‘Š“–Šz‚P_ˆê•M‘S‘¹
        /// </summary>
        [Column("‰c”_Œp‘±x•¥‘Š“–Šz‚P_ˆê•M‘S‘¹")]
        public Decimal? ‰c”_Œp‘±x•¥‘Š“–Šz‚P_ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// ‰c”_Œp‘±x•¥‘Š“–Šz‚P_ˆê•M”¼‘¹
        /// </summary>
        [Column("‰c”_Œp‘±x•¥‘Š“–Šz‚P_ˆê•M”¼‘¹")]
        public Decimal? ‰c”_Œp‘±x•¥‘Š“–Šz‚P_ˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// ‰c”_Œp‘±x•¥‘Š“–Šz‚Q_ˆê•M‘S‘¹
        /// </summary>
        [Column("‰c”_Œp‘±x•¥‘Š“–Šz‚Q_ˆê•M‘S‘¹")]
        public Decimal? ‰c”_Œp‘±x•¥‘Š“–Šz‚Q_ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// ‰c”_Œp‘±x•¥‘Š“–Šz‚Q_ˆê•M”¼‘¹
        /// </summary>
        [Column("‰c”_Œp‘±x•¥‘Š“–Šz‚Q_ˆê•M”¼‘¹")]
        public Decimal? ‰c”_Œp‘±x•¥‘Š“–Šz‚Q_ˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// ‰c”_Œp‘±x•¥‘Š“–Šz_Œˆ’èŠz__ˆê•M‘S‘¹
        /// </summary>
        [Column("‰c”_Œp‘±x•¥‘Š“–Šz_Œˆ’èŠz__ˆê•M‘S‘¹")]
        public Decimal? ‰c”_Œp‘±x•¥‘Š“–Šz_Œˆ’èŠz__ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// ‰c”_Œp‘±x•¥‘Š“–Šz_Œˆ’èŠz_ˆê•M”¼‘¹
        /// </summary>
        [Column("‰c”_Œp‘±x•¥‘Š“–Šz_Œˆ’èŠz_ˆê•M”¼‘¹")]
        public Decimal? ‰c”_Œp‘±x•¥‘Š“–Šz_Œˆ’èŠz_ˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// ”—Ê•¥‘Š“–Šz
        /// </summary>
        [Column("”—Ê•¥‘Š“–Šz")]
        public Decimal? ”—Ê•¥‘Š“–Šz { get; set; }

        /// <summary>
        /// o‰×ÀÑ‚É‘Î‚·‚é”—Ê•¥_ˆê•M‘S‘¹
        /// </summary>
        [Column("o‰×ÀÑ‚É‘Î‚·‚é”—Ê•¥_ˆê•M‘S‘¹")]
        public Decimal? o‰×ÀÑ‚É‘Î‚·‚é”—Ê•¥_ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// o‰×ÀÑ‚É‘Î‚·‚é”—Ê•¥_ˆê•M”¼‘¹
        /// </summary>
        [Column("o‰×ÀÑ‚É‘Î‚·‚é”—Ê•¥_ˆê•M”¼‘¹")]
        public Decimal? o‰×ÀÑ‚É‘Î‚·‚é”—Ê•¥_ˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// ’²®ŒãŠî€¶Y‹àŠz_ˆê•M‘S‘¹
        /// </summary>
        [Column("’²®ŒãŠî€¶Y‹àŠz_ˆê•M‘S‘¹")]
        public Decimal? ’²®ŒãŠî€¶Y‹àŠz_ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// ’²®ŒãŠî€¶Y‹àŠz_ˆê•M”¼‘¹
        /// </summary>
        [Column("’²®ŒãŠî€¶Y‹àŠz_ˆê•M”¼‘¹")]
        public Decimal? ’²®ŒãŠî€¶Y‹àŠz_ˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// ¶Y‹àŠz
        /// </summary>
        [Column("¶Y‹àŠz")]
        public Decimal? ¶Y‹àŠz { get; set; }

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
        /// ”íŠQ‹æ•ª
        /// </summary>
        [Column("”íŠQ‹æ•ª")]
        public Decimal? ”íŠQ‹æ•ª { get; set; }

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
