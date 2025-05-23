using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20060_vZ
    /// </summary>
    [Serializable]
    [Table("m_20060_vZ")]
    [PrimaryKey(nameof(øóû®), nameof(âR[h))]
    public class M20060vZ : ModelBase
    {
        /// <summary>
        /// øóû®
        /// </summary>
        [Required]
        [Column("øóû®", Order = 1)]
        [StringLength(1)]
        public string øóû® { get; set; }

        /// <summary>
        /// âR[h
        /// </summary>
        [Required]
        [Column("âR[h", Order = 2)]
        [StringLength(2)]
        public string âR[h { get; set; }

        /// <summary>
        /// s\knûn
        /// </summary>
        [Column("s\knûn")]
        public Decimal? s\knûn { get; set; }

        /// <summary>
        /// x¥Jn¹Q
        /// </summary>
        [Column("x¥Jn¹Q")]
        public Decimal? x¥Jn¹Q { get; set; }

        /// <summary>
        /// x¥Jn¹Q¼Ì
        /// </summary>
        [Column("x¥Jn¹Q¼Ì")]
        [StringLength(20)]
        public string x¥Jn¹Q¼Ì { get; set; }

        /// <summary>
        /// x¥Jn¹Q¼ÌÁá
        /// </summary>
        [Column("x¥Jn¹Q¼ÌÁá")]
        [StringLength(20)]
        public string x¥Jn¹Q¼ÌÁá { get; set; }

        /// <summary>
        /// S¹knx¥Jn
        /// </summary>
        [Column("S¹knx¥Jn")]
        public Decimal? S¹knx¥Jn { get; set; }

        /// <summary>
        /// S¹s\¸û
        /// </summary>
        [Column("S¹s\¸û")]
        public Decimal? S¹s\¸û { get; set; }

        /// <summary>
        /// ¼¹knx¥Jn
        /// </summary>
        [Column("¼¹knx¥Jn")]
        public Decimal? ¼¹knx¥Jn { get; set; }

        /// <summary>
        /// Ááx¥Jn
        /// </summary>
        [Column("Ááx¥Jn")]
        public Decimal? Ááx¥Jn { get; set; }

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
    }
}
