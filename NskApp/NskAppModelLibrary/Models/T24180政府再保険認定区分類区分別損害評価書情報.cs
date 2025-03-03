using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24180_­•{Ä•ÛŒ¯”F’è‹æ•ª—Ş‹æ•ª•Ê‘¹ŠQ•]‰¿‘î•ñ
    /// </summary>
    [Serializable]
    [Table("t_24180_­•{Ä•ÛŒ¯”F’è‹æ•ª—Ş‹æ•ª•Ê‘¹ŠQ•]‰¿‘î•ñ")]
    [PrimaryKey(nameof(‘g‡“™ƒR[ƒh), nameof(”NY), nameof(‹¤Ï–Ú“IƒR[ƒh), nameof(‡•¹¯•Ê), nameof(¿‹‰ñ), nameof(­•{•ÛŒ¯”F’è‹æ•ª), nameof(•âŠ„‡), nameof(—Ş‹æ•ª), nameof(‰c”_’²®ƒtƒ‰ƒO))]
    public class T24180­•{Ä•ÛŒ¯”F’è‹æ•ª—Ş‹æ•ª•Ê‘¹ŠQ•]‰¿‘î•ñ : ModelBase
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
        /// ‡•¹¯•Ê
        /// </summary>
        [Required]
        [Column("‡•¹¯•Ê", Order = 4)]
        [StringLength(3)]
        public string ‡•¹¯•Ê { get; set; }

        /// <summary>
        /// ¿‹‰ñ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("¿‹‰ñ", Order = 5)]
        public short ¿‹‰ñ { get; set; }

        /// <summary>
        /// ­•{•ÛŒ¯”F’è‹æ•ª
        /// </summary>
        [Required]
        [Column("­•{•ÛŒ¯”F’è‹æ•ª", Order = 6)]
        [StringLength(4)]
        public string ­•{•ÛŒ¯”F’è‹æ•ª { get; set; }

        /// <summary>
        /// •âŠ„‡
        /// </summary>
        [Required]
        [Column("•âŠ„‡", Order = 7)]
        [StringLength(2)]
        public string •âŠ„‡ { get; set; }

        /// <summary>
        /// —Ş‹æ•ª
        /// </summary>
        [Required]
        [Column("—Ş‹æ•ª", Order = 8)]
        [StringLength(2)]
        public string —Ş‹æ•ª { get; set; }

        /// <summary>
        /// ‰c”_’²®ƒtƒ‰ƒO
        /// </summary>
        [Required]
        [Column("‰c”_’²®ƒtƒ‰ƒO", Order = 9)]
        [StringLength(1)]
        public string ‰c”_’²®ƒtƒ‰ƒO { get; set; }

        /// <summary>
        /// ˆøóŒË”
        /// </summary>
        [Column("ˆøóŒË”")]
        public Decimal? ˆøóŒË” { get; set; }

        /// <summary>
        /// ˆøóÀŒË”
        /// </summary>
        [Column("ˆøóÀŒË”")]
        public Decimal? ˆøóÀŒË” { get; set; }

        /// <summary>
        /// ˆøó–ÊÏ
        /// </summary>
        [Column("ˆøó–ÊÏ")]
        public Decimal? ˆøó–ÊÏ { get; set; }

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
        /// ‹¤ÏŒÀ“xŠz
        /// </summary>
        [Column("‹¤ÏŒÀ“xŠz")]
        public Decimal? ‹¤ÏŒÀ“xŠz { get; set; }

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
        /// ‹¤Ï‹àŠz
        /// </summary>
        [Column("‹¤Ï‹àŠz")]
        public Decimal? ‹¤Ï‹àŠz { get; set; }

        /// <summary>
        /// ’ÊíÓ”C‹¤Ï‹àŠz
        /// </summary>
        [Column("’ÊíÓ”C‹¤Ï‹àŠz")]
        public Decimal? ’ÊíÓ”C‹¤Ï‹àŠz { get; set; }

        /// <summary>
        /// ’ÊíÓ”C•ÛŒ¯•à‡
        /// </summary>
        [Column("’ÊíÓ”C•ÛŒ¯•à‡")]
        public Decimal? ’ÊíÓ”C•ÛŒ¯•à‡ { get; set; }

        /// <summary>
        /// ”_ì•¨’Êí•W€”íŠQ—¦
        /// </summary>
        [Column("”_ì•¨’Êí•W€”íŠQ—¦")]
        public Decimal? ”_ì•¨’Êí•W€”íŠQ—¦ { get; set; }

        /// <summary>
        /// ”_ì•¨ˆÙíÓ”C•ÛŒ¯‹àŠz
        /// </summary>
        [Column("”_ì•¨ˆÙíÓ”C•ÛŒ¯‹àŠz")]
        public Decimal? ”_ì•¨ˆÙíÓ”C•ÛŒ¯‹àŠz { get; set; }

        /// <summary>
        /// x•¥•ÛŒ¯‹à
        /// </summary>
        [Column("x•¥•ÛŒ¯‹à")]
        public Decimal? x•¥•ÛŒ¯‹à { get; set; }

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
        /// ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™” { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_ˆøó–ÊÏ
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_ˆøó–ÊÏ")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_ˆøó–ÊÏ { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_Šî€ûŠn—Ê
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_Šî€ûŠn—Ê")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_Šî€ûŠn—Ê { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_‹¤ÏŒÀ“xŠz
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_‹¤ÏŒÀ“xŠz")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_‹¤ÏŒÀ“xŠz { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_Šî€¶Y‹àŠz
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_Šî€¶Y‹àŠz")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_Šî€¶Y‹àŠz { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_Œ¸û—Ê
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_Œ¸û—Ê")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_Œ¸û—Ê { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_¶Y‹àŠz‚ÌŒ¸­Šz
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_¶Y‹àŠz‚ÌŒ¸­Šz")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_¶Y‹àŠz‚ÌŒ¸­Šz { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M‘S‘¹
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M‘S‘¹")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ–ÊÏ_ˆê•M‘S‘¹
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_”íŠQ–ÊÏ_ˆê•M‘S‘¹")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ–ÊÏ_ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_¶Y‹àŠz‚ÌŒ¸­Šz_ˆê•M‘S‘¹
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_¶Y‹àŠz‚ÌŒ¸­Šz_ˆê•M‘S‘¹")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_¶Y‹àŠz‚ÌŒ¸­Šz_ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M”¼‘¹
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M”¼‘¹")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ–ÊÏ_ˆê•M”¼‘¹
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_”íŠQ–ÊÏ_ˆê•M”¼‘¹")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ–ÊÏ_ˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_¶Y‹àŠz‚ÌŒ¸­Šz_ˆê•M”¼‘¹
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_¶Y‹àŠz‚ÌŒ¸­Šz_ˆê•M”¼‘¹")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_¶Y‹àŠz‚ÌŒ¸­Šz_ˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M‘S”¼‘¹Œv
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M‘S”¼‘¹Œv")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M‘S”¼‘¹Œv { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ–ÊÏ_ˆê•M‘S”¼‘¹Œv
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_”íŠQ–ÊÏ_ˆê•M‘S”¼‘¹Œv")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ–ÊÏ_ˆê•M‘S”¼‘¹Œv { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_¶Y‹àŠz‚ÌŒ¸­Šz_ˆê•M‘S”¼‘¹Œv
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_¶Y‹àŠz‚ÌŒ¸­Šz_ˆê•M‘S”¼‘¹Œv")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_¶Y‹àŠz‚ÌŒ¸­Šz_ˆê•M‘S”¼‘¹Œv { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_‡Œv
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_‡Œv")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_‡Œv { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_¶Y‹àŠz‚ÌŒ¸­Šz_‡Œv
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_¶Y‹àŠz‚ÌŒ¸­Šz_‡Œv")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_¶Y‹àŠz‚ÌŒ¸­Šz_‡Œv { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_ˆøó–ÊÏ_‡Œv
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_ˆøó–ÊÏ_‡Œv")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_ˆøó–ÊÏ_‡Œv { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_Šî€ûŠn—Ê_‡Œv
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_Šî€ûŠn—Ê_‡Œv")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_Šî€ûŠn—Ê_‡Œv { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_Šî€¶Y‹àŠz_‡Œv
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_Šî€¶Y‹àŠz_‡Œv")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_Šî€¶Y‹àŠz_‡Œv { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_‹¤ÏŒÀ“xŠz_‡Œv
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_‹¤ÏŒÀ“xŠz_‡Œv")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_‹¤ÏŒÀ“xŠz_‡Œv { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_Œ¸û—Ê_‡Œv
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_Œ¸û—Ê_‡Œv")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_Œ¸û—Ê_‡Œv { get; set; }

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
        /// x•¥Ä•ÛŒ¯‹à
        /// </summary>
        [Column("x•¥Ä•ÛŒ¯‹à")]
        public Decimal? x•¥Ä•ÛŒ¯‹à { get; set; }

        /// <summary>
        /// ‹àŠz”íŠQ—¦
        /// </summary>
        [Column("‹àŠz”íŠQ—¦")]
        public Decimal? ‹àŠz”íŠQ—¦ { get; set; }

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
