using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24150_‘g‡“™“–‰•]‰¿‚î•ñ
    /// </summary>
    [Serializable]
    [Table("t_24150_‘g‡“™“–‰•]‰¿‚î•ñ")]
    [PrimaryKey(nameof(‘g‡“™ƒR[ƒh), nameof(”NY), nameof(‹¤Ï–Ú“IƒR[ƒh), nameof(‡•¹¯•Ê), nameof(•âŠ„‡), nameof(—Ş‹æ•ª), nameof(‰c”_’²®ƒtƒ‰ƒO))]
    public class T24150‘g‡“™“–‰•]‰¿‚î•ñ : ModelBase
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
        /// •âŠ„‡
        /// </summary>
        [Required]
        [Column("•âŠ„‡", Order = 5)]
        [StringLength(2)]
        public string •âŠ„‡ { get; set; }

        /// <summary>
        /// —Ş‹æ•ª
        /// </summary>
        [Required]
        [Column("—Ş‹æ•ª", Order = 6)]
        [StringLength(2)]
        public string —Ş‹æ•ª { get; set; }

        /// <summary>
        /// ‰c”_’²®ƒtƒ‰ƒO
        /// </summary>
        [Required]
        [Column("‰c”_’²®ƒtƒ‰ƒO", Order = 7)]
        [StringLength(1)]
        public string ‰c”_’²®ƒtƒ‰ƒO { get; set; }

        /// <summary>
        /// ‡•¹“Á—á—L–³ƒtƒ‰ƒO
        /// </summary>
        [Column("‡•¹“Á—á—L–³ƒtƒ‰ƒO")]
        [StringLength(1)]
        public string ‡•¹“Á—á—L–³ƒtƒ‰ƒO { get; set; }

        /// <summary>
        /// x•¥‹¤Ï‹à_x•¥—¦’²®‘O
        /// </summary>
        [Column("x•¥‹¤Ï‹à_x•¥—¦’²®‘O")]
        public Decimal? x•¥‹¤Ï‹à_x•¥—¦’²®‘O { get; set; }

        /// <summary>
        /// x•¥‹¤Ï‹à
        /// </summary>
        [Column("x•¥‹¤Ï‹à")]
        public Decimal? x•¥‹¤Ï‹à { get; set; }

        /// <summary>
        /// ’ÊíÓ”C‹¤Ï‹àŠz
        /// </summary>
        [Column("’ÊíÓ”C‹¤Ï‹àŠz")]
        public Decimal? ’ÊíÓ”C‹¤Ï‹àŠz { get; set; }

        /// <summary>
        /// ”_ì•¨’ÊíÓ”C‹¤Ï‹àŠz_“Á’è
        /// </summary>
        [Column("”_ì•¨’ÊíÓ”C‹¤Ï‹àŠz_“Á’è")]
        public Decimal? ”_ì•¨’ÊíÓ”C‹¤Ï‹àŠz_“Á’è { get; set; }

        /// <summary>
        /// ’Êí•”•ª•ÛŒ¯‹àŒ©Šz
        /// </summary>
        [Column("’Êí•”•ª•ÛŒ¯‹àŒ©Šz")]
        public Decimal? ’Êí•”•ª•ÛŒ¯‹àŒ©Šz { get; set; }

        /// <summary>
        /// x•¥•ÛŒ¯‹àŒ©Šz
        /// </summary>
        [Column("x•¥•ÛŒ¯‹àŒ©Šz")]
        public Decimal? x•¥•ÛŒ¯‹àŒ©Šz { get; set; }

        /// <summary>
        /// ˆÙí•”•ª•ÛŒ¯‹àŒ©Šz
        /// </summary>
        [Column("ˆÙí•”•ª•ÛŒ¯‹àŒ©Šz")]
        public Decimal? ˆÙí•”•ª•ÛŒ¯‹àŒ©Šz { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™” { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M‘S‘¹
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M‘S‘¹")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M”¼‘¹
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M”¼‘¹")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M”¼‘¹ { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M‘S‘¹”¼‘¹Œv
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M‘S‘¹”¼‘¹Œv")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_ˆê•M‘S‘¹”¼‘¹Œv { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_‡Œv
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_‡Œv")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ‘g‡ˆõ“™”_‡Œv { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_ˆøó–ÊÏ
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_ˆøó–ÊÏ")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_ˆøó–ÊÏ { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ–ÊÏ_ˆê•M‘S‘¹
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_”íŠQ–ÊÏ_ˆê•M‘S‘¹")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ–ÊÏ_ˆê•M‘S‘¹ { get; set; }

        /// <summary>
        /// ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ–ÊÏ_ˆê•M”¼‘¹
        /// </summary>
        [Column("‹¤Ï‹àx•¥‘ÎÛ_”íŠQ–ÊÏ_ˆê•M”¼‘¹")]
        public Decimal? ‹¤Ï‹àx•¥‘ÎÛ_”íŠQ–ÊÏ_ˆê•M”¼‘¹ { get; set; }

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
