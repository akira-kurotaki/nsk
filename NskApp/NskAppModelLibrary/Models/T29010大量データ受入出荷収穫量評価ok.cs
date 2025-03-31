using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_29010_‘å—Êƒf[ƒ^ó“ü_o‰×ûŠn—Ê•]‰¿ok
    /// </summary>
    [Serializable]
    [Table("t_29010_‘å—Êƒf[ƒ^ó“ü_o‰×ûŠn—Ê•]‰¿ok")]
    [PrimaryKey(nameof(ó“ü—š—ğid), nameof(s”Ô†), nameof(ˆ—‹æ•ª), nameof(‘g‡“™ƒR[ƒh), nameof(”NY), nameof(‹¤Ï–Ú“IƒR[ƒh), nameof(‘g‡ˆõ“™ƒR[ƒh), nameof(k’n”Ô†), nameof(•ª•M”Ô†))]
    public class T29010‘å—Êƒf[ƒ^ó“üo‰×ûŠn—Ê•]‰¿ok : ModelBase
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
        [Column("ˆ—‹æ•ª", Order = 3)]
        [StringLength(1)]
        public string ˆ—‹æ•ª { get; set; }

        /// <summary>
        /// ‘g‡“™ƒR[ƒh
        /// </summary>
        [Required]
        [Column("‘g‡“™ƒR[ƒh", Order = 4)]
        [StringLength(3)]
        public string ‘g‡“™ƒR[ƒh { get; set; }

        /// <summary>
        /// ”NY
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("”NY", Order = 5)]
        public short ”NY { get; set; }

        /// <summary>
        /// ‹¤Ï–Ú“IƒR[ƒh
        /// </summary>
        [Required]
        [Column("‹¤Ï–Ú“IƒR[ƒh", Order = 6)]
        [StringLength(2)]
        public string ‹¤Ï–Ú“IƒR[ƒh { get; set; }

        /// <summary>
        /// ‘g‡ˆõ“™ƒR[ƒh
        /// </summary>
        [Required]
        [Column("‘g‡ˆõ“™ƒR[ƒh", Order = 7)]
        [StringLength(13)]
        public string ‘g‡ˆõ“™ƒR[ƒh { get; set; }

        /// <summary>
        /// k’n”Ô†
        /// </summary>
        [Required]
        [Column("k’n”Ô†", Order = 8)]
        [StringLength(5)]
        public string k’n”Ô† { get; set; }

        /// <summary>
        /// •ª•M”Ô†
        /// </summary>
        [Required]
        [Column("•ª•M”Ô†", Order = 9)]
        [StringLength(4)]
        public string •ª•M”Ô† { get; set; }

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
        /// o‰×’P“–û—Ê_Š„‡
        /// </summary>
        [Column("o‰×’P“–û—Ê_Š„‡")]
        public Decimal? o‰×’P“–û—Ê_Š„‡ { get; set; }

        /// <summary>
        /// o‰×’P“–û—Ê_”—Ê
        /// </summary>
        [Column("o‰×’P“–û—Ê_”—Ê")]
        public Decimal? o‰×’P“–û—Ê_”—Ê { get; set; }

        /// <summary>
        /// •ªŠ„Š„‡
        /// </summary>
        [Column("•ªŠ„Š„‡")]
        public Decimal? •ªŠ„Š„‡ { get; set; }

        /// <summary>
        /// •ªŠ„Š„‡_”—Ê
        /// </summary>
        [Column("•ªŠ„Š„‡_”—Ê")]
        public Decimal? •ªŠ„Š„‡_”—Ê { get; set; }

        /// <summary>
        /// •ªŠ„–—R
        /// </summary>
        [Column("•ªŠ„–—R")]
        [StringLength(255)]
        public string •ªŠ„–—R { get; set; }

        /// <summary>
        /// •]‰¿”NŒ“ú
        /// </summary>
        [Column("•]‰¿”NŒ“ú")]
        public DateTime? •]‰¿”NŒ“ú { get; set; }

        /// <summary>
        /// •]‰¿Ò
        /// </summary>
        [Column("•]‰¿Ò")]
        [StringLength(20)]
        public string •]‰¿Ò { get; set; }

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
