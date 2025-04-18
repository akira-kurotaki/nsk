using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_29060_åÊf[^óü__P\²æ²¸ok
    /// </summary>
    [Serializable]
    [Table("t_29060_åÊf[^óü__P\²æ²¸ok")]
    [PrimaryKey(nameof(óüðid), nameof(sÔ))]
    public class T29060åÊf[^óü_P\²æ²¸ok : ModelBase
    {
        /// <summary>
        /// óüðid
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("óüðid", Order = 1)]
        public long óüðid { get; set; }

        /// <summary>
        /// sÔ
        /// </summary>
        [Required]
        [Column("sÔ", Order = 2)]
        [StringLength(6)]
        public string sÔ { get; set; }

        /// <summary>
        /// æª
        /// </summary>
        [Required]
        [Column("æª")]
        [StringLength(1)]
        public string æª { get; set; }

        /// <summary>
        /// gR[h
        /// </summary>
        [Required]
        [Column("gR[h")]
        [StringLength(3)]
        public string gR[h { get; set; }

        /// <summary>
        /// NY
        /// </summary>
        [Required]
        [Column("NY")]
        public short NY { get; set; }

        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Required]
        [Column("¤ÏÚIR[h")]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// gõR[h
        /// </summary>
        [Required]
        [Column("gõR[h")]
        [StringLength(13)]
        public string gõR[h { get; set; }

        /// <summary>
        /// knÔ
        /// </summary>
        [Required]
        [Column("knÔ")]
        [StringLength(5)]
        public string knÔ { get; set; }

        /// <summary>
        /// ªMÔ
        /// </summary>
        [Required]
        [Column("ªMÔ")]
        [StringLength(4)]
        public string ªMÔ { get; set; }

        /// <summary>
        /// Kwæª
        /// </summary>
        [Column("Kwæª")]
        [StringLength(3)]
        public string Kwæª { get; set; }

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
        /// ²æ²¸ÇR[h
        /// </summary>
        [Column("²æ²¸ÇR[h")]
        [StringLength(3)]
        public string ²æ²¸ÇR[h { get; set; }

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
    }
}
