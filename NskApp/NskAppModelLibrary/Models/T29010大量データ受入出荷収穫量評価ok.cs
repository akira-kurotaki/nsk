using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_29010_åÊf[^óü_o×ûnÊ]¿ok
    /// </summary>
    [Serializable]
    [Table("t_29010_åÊf[^óü_o×ûnÊ]¿ok")]
    [PrimaryKey(nameof(óüðid), nameof(sÔ), nameof(æª), nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(knÔ), nameof(ªMÔ))]
    public class T29010åÊf[^óüo×ûnÊ]¿ok : ModelBase
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
        [Column("æª", Order = 3)]
        [StringLength(1)]
        public string æª { get; set; }

        /// <summary>
        /// gR[h
        /// </summary>
        [Required]
        [Column("gR[h", Order = 4)]
        [StringLength(3)]
        public string gR[h { get; set; }

        /// <summary>
        /// NY
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("NY", Order = 5)]
        public short NY { get; set; }

        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Required]
        [Column("¤ÏÚIR[h", Order = 6)]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// gõR[h
        /// </summary>
        [Required]
        [Column("gõR[h", Order = 7)]
        [StringLength(13)]
        public string gõR[h { get; set; }

        /// <summary>
        /// knÔ
        /// </summary>
        [Required]
        [Column("knÔ", Order = 8)]
        [StringLength(5)]
        public string knÔ { get; set; }

        /// <summary>
        /// ªMÔ
        /// </summary>
        [Required]
        [Column("ªMÔ", Order = 9)]
        [StringLength(4)]
        public string ªMÔ { get; set; }

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
        /// o×PûÊ_
        /// </summary>
        [Column("o×PûÊ_")]
        public Decimal? o×PûÊ_ { get; set; }

        /// <summary>
        /// o×PûÊ_Ê
        /// </summary>
        [Column("o×PûÊ_Ê")]
        public Decimal? o×PûÊ_Ê { get; set; }

        /// <summary>
        /// ª
        /// </summary>
        [Column("ª")]
        public Decimal? ª { get; set; }

        /// <summary>
        /// ª_Ê
        /// </summary>
        [Column("ª_Ê")]
        public Decimal? ª_Ê { get; set; }

        /// <summary>
        /// ªR
        /// </summary>
        [Column("ªR")]
        [StringLength(255)]
        public string ªR { get; set; }

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
        /// ûn\èú
        /// </summary>
        [Column("ûn\èú")]
        public DateTime? ûn\èú { get; set; }

        /// <summary>
        /// Àü\èú
        /// </summary>
        [Column("Àü\èú")]
        public DateTime? Àü\èú { get; set; }

        /// <summary>
        /// ¤ÏÌP
        /// </summary>
        [Column("¤ÏÌP")]
        [StringLength(2)]
        public string ¤ÏÌP { get; set; }

        /// <summary>
        /// ÐQíÞP
        /// </summary>
        [Column("ÐQíÞP")]
        [StringLength(2)]
        public string ÐQíÞP { get; set; }

        /// <summary>
        /// ÐQ­¶NúP
        /// </summary>
        [Column("ÐQ­¶NúP")]
        public DateTime? ÐQ­¶NúP { get; set; }

        /// <summary>
        /// ¤ÏÌQ
        /// </summary>
        [Column("¤ÏÌQ")]
        [StringLength(2)]
        public string ¤ÏÌQ { get; set; }

        /// <summary>
        /// ÐQíÞQ
        /// </summary>
        [Column("ÐQíÞQ")]
        [StringLength(2)]
        public string ÐQíÞQ { get; set; }

        /// <summary>
        /// ÐQ­¶NúQ
        /// </summary>
        [Column("ÐQ­¶NúQ")]
        public DateTime? ÐQ­¶NúQ { get; set; }

        /// <summary>
        /// ¤ÏÌR
        /// </summary>
        [Column("¤ÏÌR")]
        [StringLength(2)]
        public string ¤ÏÌR { get; set; }

        /// <summary>
        /// ÐQíÞR
        /// </summary>
        [Column("ÐQíÞR")]
        [StringLength(2)]
        public string ÐQíÞR { get; set; }

        /// <summary>
        /// ÐQ­¶NúR
        /// </summary>
        [Column("ÐQ­¶NúR")]
        public DateTime? ÐQ­¶NúR { get; set; }

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
