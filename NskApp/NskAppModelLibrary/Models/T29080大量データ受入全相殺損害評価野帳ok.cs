using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_29080_‘å—Êƒf[ƒ^ó“ü_‘S‘ŠE‘¹ŠQ•]‰¿–ì’ ok
    /// </summary>
    [Serializable]
    [Table("t_29080_‘å—Êƒf[ƒ^ó“ü_‘S‘ŠE‘¹ŠQ•]‰¿–ì’ ok")]
    [PrimaryKey(nameof(ó“ü—š—ğid), nameof(s”Ô†))]
    public class T29080‘å—Êƒf[ƒ^ó“ü‘S‘ŠE‘¹ŠQ•]‰¿–ì’ ok : ModelBase
    {
        /// <summary>
        /// ó“ü—š—ğid
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ó“ü—š—ğid", Order = 1)]
        public long ó“ü—š—ğid { get; set; }

        /// <summary>
        /// s”Ô†
        /// </summary>
        [Required]
        [Column("s”Ô†", Order = 2)]
        [StringLength(6)]
        public string s”Ô† { get; set; }

        /// <summary>
        /// ˆ—‹æ•ª
        /// </summary>
        [Required]
        [Column("ˆ—‹æ•ª")]
        [StringLength(1)]
        public string ˆ—‹æ•ª { get; set; }

        /// <summary>
        /// ‹¤Ï–Ú“IƒR[ƒh
        /// </summary>
        [Required]
        [Column("‹¤Ï–Ú“IƒR[ƒh")]
        [StringLength(2)]
        public string ‹¤Ï–Ú“IƒR[ƒh { get; set; }

        /// <summary>
        /// ‘g‡“™ƒR[ƒh
        /// </summary>
        [Required]
        [Column("‘g‡“™ƒR[ƒh")]
        [StringLength(3)]
        public string ‘g‡“™ƒR[ƒh { get; set; }

        /// <summary>
        /// ”NY
        /// </summary>
        [Required]
        [Column("”NY")]
        public short ”NY { get; set; }

        /// <summary>
        /// ‘g‡ˆõ“™ƒR[ƒh
        /// </summary>
        [Required]
        [Column("‘g‡ˆõ“™ƒR[ƒh")]
        [StringLength(13)]
        public string ‘g‡ˆõ“™ƒR[ƒh { get; set; }

        /// <summary>
        /// k’n”Ô†
        /// </summary>
        [Required]
        [Column("k’n”Ô†")]
        [StringLength(5)]
        public string k’n”Ô† { get; set; }

        /// <summary>
        /// •ª•M”Ô†
        /// </summary>
        [Required]
        [Column("•ª•M”Ô†")]
        [StringLength(4)]
        public string •ª•M”Ô† { get; set; }

        /// <summary>
        /// •]‰¿’n‹æƒR[ƒh
        /// </summary>
        [Column("•]‰¿’n‹æƒR[ƒh")]
        [StringLength(4)]
        public string •]‰¿’n‹æƒR[ƒh { get; set; }

        /// <summary>
        /// ŠK‘w‹æ•ª
        /// </summary>
        [Column("ŠK‘w‹æ•ª")]
        [StringLength(3)]
        public string ŠK‘w‹æ•ª { get; set; }

        /// <summary>
        /// ”íŠQ”»’èƒR[ƒh
        /// </summary>
        [Column("”íŠQ”»’èƒR[ƒh")]
        [StringLength(1)]
        public string ”íŠQ”»’èƒR[ƒh { get; set; }

        /// <summary>
        /// ˆê•M‘S”¼”»’è
        /// </summary>
        [Column("ˆê•M‘S”¼”»’è")]
        [StringLength(1)]
        public string ˆê•M‘S”¼”»’è { get; set; }

        /// <summary>
        /// »ŠF’Pû
        /// </summary>
        [Column("»ŠF’Pû")]
        public Decimal? »ŠF’Pû { get; set; }

        /// <summary>
        /// •ªŠ„Š„‡
        /// </summary>
        [Column("•ªŠ„Š„‡")]
        public Decimal? •ªŠ„Š„‡ { get; set; }

        /// <summary>
        /// •ªŠ„–—R
        /// </summary>
        [Column("•ªŠ„–—R")]
        [StringLength(255)]
        public string •ªŠ„–—R { get; set; }

        /// <summary>
        /// ûŠn—\’è“ú
        /// </summary>
        [Column("ûŠn—\’è“ú")]
        public DateTime? ûŠn—\’è“ú { get; set; }

        /// <summary>
        /// ”À“ü—\’è“ú
        /// </summary>
        [Column("”À“ü—\’è“ú")]
        public DateTime? ”À“ü—\’è“ú { get; set; }

        /// <summary>
        /// ‹¤Ï–ŒÌ‚P
        /// </summary>
        [Column("‹¤Ï–ŒÌ‚P")]
        [StringLength(2)]
        public string ‹¤Ï–ŒÌ‚P { get; set; }

        /// <summary>
        /// ĞŠQí—Ş‚P
        /// </summary>
        [Column("ĞŠQí—Ş‚P")]
        [StringLength(2)]
        public string ĞŠQí—Ş‚P { get; set; }

        /// <summary>
        /// ĞŠQ”­¶”NŒ“ú‚P
        /// </summary>
        [Column("ĞŠQ”­¶”NŒ“ú‚P")]
        public DateTime? ĞŠQ”­¶”NŒ“ú‚P { get; set; }

        /// <summary>
        /// ‹¤Ï–ŒÌ‚Q
        /// </summary>
        [Column("‹¤Ï–ŒÌ‚Q")]
        [StringLength(2)]
        public string ‹¤Ï–ŒÌ‚Q { get; set; }

        /// <summary>
        /// ĞŠQí—Ş‚Q
        /// </summary>
        [Column("ĞŠQí—Ş‚Q")]
        [StringLength(2)]
        public string ĞŠQí—Ş‚Q { get; set; }

        /// <summary>
        /// ĞŠQ”­¶”NŒ“ú‚Q
        /// </summary>
        [Column("ĞŠQ”­¶”NŒ“ú‚Q")]
        public DateTime? ĞŠQ”­¶”NŒ“ú‚Q { get; set; }

        /// <summary>
        /// ‹¤Ï–ŒÌ‚R
        /// </summary>
        [Column("‹¤Ï–ŒÌ‚R")]
        [StringLength(2)]
        public string ‹¤Ï–ŒÌ‚R { get; set; }

        /// <summary>
        /// ĞŠQí—Ş‚R
        /// </summary>
        [Column("ĞŠQí—Ş‚R")]
        [StringLength(2)]
        public string ĞŠQí—Ş‚R { get; set; }

        /// <summary>
        /// ĞŠQ”­¶”NŒ“ú‚R
        /// </summary>
        [Column("ĞŠQ”­¶”NŒ“ú‚R")]
        public DateTime? ĞŠQ”­¶”NŒ“ú‚R { get; set; }

        /// <summary>
        /// •]‰¿”NŒ“ú
        /// </summary>
        [Column("•]‰¿”NŒ“ú")]
        public DateTime? •]‰¿”NŒ“ú { get; set; }

        /// <summary>
        /// ”²æ’²¸”ÇƒR[ƒh
        /// </summary>
        [Column("”²æ’²¸”ÇƒR[ƒh")]
        [StringLength(3)]
        public string ”²æ’²¸”ÇƒR[ƒh { get; set; }

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
