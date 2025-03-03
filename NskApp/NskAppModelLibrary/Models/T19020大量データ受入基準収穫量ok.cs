using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_19020_‘å—Êƒf[ƒ^ó“ü_Šî€ûŠn—Êok
    /// </summary>
    [Serializable]
    [Table("t_19020_‘å—Êƒf[ƒ^ó“ü_Šî€ûŠn—Êok")]
    [PrimaryKey(nameof(ó“ü—š—ğid), nameof(s”Ô†))]
    public class T19020‘å—Êƒf[ƒ^ó“üŠî€ûŠn—Êok : ModelBase
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("s”Ô†", Order = 2)]
        public Decimal s”Ô† { get; set; }

        /// <summary>
        /// ˆ—‹æ•ª
        /// </summary>
        [Column("ˆ—‹æ•ª")]
        [StringLength(1)]
        public string ˆ—‹æ•ª { get; set; }

        /// <summary>
        /// ‘g‡“™ƒR[ƒh
        /// </summary>
        [Column("‘g‡“™ƒR[ƒh")]
        [StringLength(3)]
        public string ‘g‡“™ƒR[ƒh { get; set; }

        /// <summary>
        /// ‘g‡“™–¼
        /// </summary>
        [Column("‘g‡“™–¼")]
        [StringLength(50)]
        public string ‘g‡“™–¼ { get; set; }

        /// <summary>
        /// ”NY
        /// </summary>
        [Column("”NY")]
        public short? ”NY { get; set; }

        /// <summary>
        /// ‹¤Ï–Ú“IƒR[ƒh
        /// </summary>
        [Column("‹¤Ï–Ú“IƒR[ƒh")]
        [StringLength(2)]
        public string ‹¤Ï–Ú“IƒR[ƒh { get; set; }

        /// <summary>
        /// ‹¤Ï–Ú“I–¼
        /// </summary>
        [Column("‹¤Ï–Ú“I–¼")]
        [StringLength(20)]
        public string ‹¤Ï–Ú“I–¼ { get; set; }

        /// <summary>
        /// ˆøó•û®
        /// </summary>
        [Column("ˆøó•û®")]
        [StringLength(1)]
        public string ˆøó•û® { get; set; }

        /// <summary>
        /// ˆøó•û®–¼Ì
        /// </summary>
        [Column("ˆøó•û®–¼Ì")]
        [StringLength(20)]
        public string ˆøó•û®–¼Ì { get; set; }

        /// <summary>
        /// xŠƒR[ƒh
        /// </summary>
        [Column("xŠƒR[ƒh")]
        [StringLength(2)]
        public string xŠƒR[ƒh { get; set; }

        /// <summary>
        /// xŠ–¼
        /// </summary>
        [Column("xŠ–¼")]
        [StringLength(20)]
        public string xŠ–¼ { get; set; }

        /// <summary>
        /// ‘å’n‹æƒR[ƒh
        /// </summary>
        [Column("‘å’n‹æƒR[ƒh")]
        [StringLength(2)]
        public string ‘å’n‹æƒR[ƒh { get; set; }

        /// <summary>
        /// ‘å’n‹æ–¼
        /// </summary>
        [Column("‘å’n‹æ–¼")]
        [StringLength(10)]
        public string ‘å’n‹æ–¼ { get; set; }

        /// <summary>
        /// ¬’n‹æƒR[ƒh
        /// </summary>
        [Column("¬’n‹æƒR[ƒh")]
        [StringLength(4)]
        public string ¬’n‹æƒR[ƒh { get; set; }

        /// <summary>
        /// ¬’n‹æ–¼
        /// </summary>
        [Column("¬’n‹æ–¼")]
        [StringLength(10)]
        public string ¬’n‹æ–¼ { get; set; }

        /// <summary>
        /// ‘g‡ˆõ“™ƒR[ƒh
        /// </summary>
        [Column("‘g‡ˆõ“™ƒR[ƒh")]
        [StringLength(13)]
        public string ‘g‡ˆõ“™ƒR[ƒh { get; set; }

        /// <summary>
        /// ‘g‡ˆõ“™–¼
        /// </summary>
        [Column("‘g‡ˆõ“™–¼")]
        [StringLength(30)]
        public string ‘g‡ˆõ“™–¼ { get; set; }

        /// <summary>
        /// —Ş‹æ•ª
        /// </summary>
        [Column("—Ş‹æ•ª")]
        [StringLength(2)]
        public string —Ş‹æ•ª { get; set; }

        /// <summary>
        /// —Ş‹æ•ª–¼
        /// </summary>
        [Column("—Ş‹æ•ª–¼")]
        [StringLength(40)]
        public string —Ş‹æ•ª–¼ { get; set; }

        /// <summary>
        /// Y’n•Ê–Á•¿ƒR[ƒh
        /// </summary>
        [Column("Y’n•Ê–Á•¿ƒR[ƒh")]
        [StringLength(5)]
        public string Y’n•Ê–Á•¿ƒR[ƒh { get; set; }

        /// <summary>
        /// ‰c”_‘ÎÛŠOƒtƒ‰ƒO
        /// </summary>
        [Column("‰c”_‘ÎÛŠOƒtƒ‰ƒO")]
        [StringLength(1)]
        public string ‰c”_‘ÎÛŠOƒtƒ‰ƒO { get; set; }

        /// <summary>
        /// Y’n•Ê–Á•¿“™–¼Ì
        /// </summary>
        [Column("Y’n•Ê–Á•¿“™–¼Ì")]
        [StringLength(30)]
        public string Y’n•Ê–Á•¿“™–¼Ì { get; set; }

        /// <summary>
        /// •½‹Ï’Pû
        /// </summary>
        [Column("•½‹Ï’Pû")]
        public Decimal? •½‹Ï’Pû { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚P
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚P")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚P { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚Q
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚Q")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚Q { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚R
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚R")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚R { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚S
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚S")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚S { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚T
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚T")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚T { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚U
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚U")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚U { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚V
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚V")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚V { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚W
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚W")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚W { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚X
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚X")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚X { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚P‚O
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚P‚O")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚P‚O { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚P‚P
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚P‚P")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚P‚P { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚P‚Q
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚P‚Q")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚P‚Q { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚P‚R
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚P‚R")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚P‚R { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚P‚S
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚P‚S")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚P‚S { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚P‚T
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚P‚T")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚P‚T { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚P‚U
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚P‚U")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚P‚U { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚P‚V
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚P‚V")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚P‚V { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚P‚W
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚P‚W")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚P‚W { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚P‚X
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚P‚X")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚P‚X { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚O
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚Q‚O")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚O { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚P
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚Q‚P")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚P { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚Q
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚Q‚Q")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚Q { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚R
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚Q‚R")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚R { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚S
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚Q‚S")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚S { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚T
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚Q‚T")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚T { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚U
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚Q‚U")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚U { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚V
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚Q‚V")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚V { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚W
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚Q‚W")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚W { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚X
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚Q‚X")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚Q‚X { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚R‚O
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚R‚O")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚R‚O { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚R‚P
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚R‚P")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚R‚P { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚R‚Q
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚R‚Q")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚R‚Q { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚R‚R
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚R‚R")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚R‚R { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚R‚S
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚R‚S")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚R‚S { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚R‚T
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚R‚T")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚R‚T { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚R‚U
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚R‚U")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚R‚U { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚R‚V
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚R‚V")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚R‚V { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚R‚W
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚R‚W")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚R‚W { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚R‚X
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚R‚X")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚R‚X { get; set; }

        /// <summary>
        /// ‹KŠi•ÊŠ„‡_‹KŠi‚S‚O
        /// </summary>
        [Column("‹KŠi•ÊŠ„‡_‹KŠi‚S‚O")]
        public Decimal? ‹KŠi•ÊŠ„‡_‹KŠi‚S‚O { get; set; }

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
