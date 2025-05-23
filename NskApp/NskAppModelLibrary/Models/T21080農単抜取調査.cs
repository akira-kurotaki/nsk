using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_21080__P²æ²¸
    /// </summary>
    [Serializable]
    [Table("t_21080__P²æ²¸")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(knÔ), nameof(ªMÔ), nameof(Kwæª))]
    public class T21080_P²æ²¸ : ModelBase
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
        /// ¤ÏÚIR[h
        /// </summary>
        [Required]
        [Column("¤ÏÚIR[h", Order = 3)]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// gõR[h
        /// </summary>
        [Required]
        [Column("gõR[h", Order = 4)]
        [StringLength(13)]
        public string gõR[h { get; set; }

        /// <summary>
        /// knÔ
        /// </summary>
        [Required]
        [Column("knÔ", Order = 5)]
        [StringLength(5)]
        public string knÔ { get; set; }

        /// <summary>
        /// ªMÔ
        /// </summary>
        [Required]
        [Column("ªMÔ", Order = 6)]
        [StringLength(4)]
        public string ªMÔ { get; set; }

        /// <summary>
        /// Kwæª
        /// </summary>
        [Required]
        [Column("Kwæª", Order = 7)]
        [StringLength(3)]
        public string Kwæª { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Column("Þæª")]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// ¹Q]¿ÊÏ
        /// </summary>
        [Column("¹Q]¿ÊÏ")]
        public Decimal? ¹Q]¿ÊÏ { get; set; }

        /// <summary>
        /// ]¿næR[h
        /// </summary>
        [Column("]¿næR[h")]
        [StringLength(4)]
        public string ]¿næR[h { get; set; }

        /// <summary>
        /// \ûnÊ
        /// </summary>
        [Column("\ûnÊ")]
        public Decimal? \ûnÊ { get; set; }

        /// <summary>
        /// ²æPû
        /// </summary>
        [Column("²æPû")]
        public Decimal? ²æPû { get; set; }

        /// <summary>
        /// ²æûnÊ
        /// </summary>
        [Column("²æûnÊ")]
        public Decimal? ²æûnÊ { get; set; }

        /// <summary>
        /// ª
        /// </summary>
        [Column("ª")]
        public Decimal? ª { get; set; }

        /// <summary>
        /// íQ»èR[h
        /// </summary>
        [Column("íQ»èR[h")]
        [StringLength(1)]
        public string íQ»èR[h { get; set; }

        /// <summary>
        /// êMS¼»è
        /// </summary>
        [Column("êMS¼»è")]
        [StringLength(1)]
        public string êMS¼»è { get; set; }

        /// <summary>
        /// ªR
        /// </summary>
        [Column("ªR")]
        [StringLength(255)]
        public string ªR { get; set; }

        /// <summary>
        /// ¤ÏÌ
        /// </summary>
        [Column("¤ÏÌ")]
        [StringLength(2)]
        public string ¤ÏÌ { get; set; }

        /// <summary>
        /// ÐQíÞ
        /// </summary>
        [Column("ÐQíÞ")]
        [StringLength(2)]
        public string ÐQíÞ { get; set; }

        /// <summary>
        /// ÐQ­¶Nú
        /// </summary>
        [Column("ÐQ­¶Nú")]
        public DateTime? ÐQ­¶Nú { get; set; }

        /// <summary>
        /// ]¿Nú
        /// </summary>
        [Column("]¿Nú")]
        public DateTime? ]¿Nú { get; set; }

        /// <summary>
        /// ]¿Ò
        /// </summary>
        [Column("]¿Ò")]
        [StringLength(20)]
        public string ]¿Ò { get; set; }

        /// <summary>
        /// »F½fÏtO
        /// </summary>
        [Column("»F½fÏtO")]
        [StringLength(1)]
        public string »F½fÏtO { get; set; }

        /// <summary>
        /// \Pû
        /// </summary>
        [Column("\Pû")]
        public Decimal? \Pû { get; set; }

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
