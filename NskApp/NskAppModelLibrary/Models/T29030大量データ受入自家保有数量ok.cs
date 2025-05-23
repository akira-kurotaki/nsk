using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_29030_åÊf[^óü_©ÆÛLÊok
    /// </summary>
    [Serializable]
    [Table("t_29030_åÊf[^óü_©ÆÛLÊok")]
    [PrimaryKey(nameof(óüðid), nameof(sÔ), nameof(æª), nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(gõR[h), nameof(Þæª), nameof(YnÊÁ¿R[h), nameof(c_ÎÛOtO))]
    public class T29030åÊf[^óü©ÆÛLÊok : ModelBase
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
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 8)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// YnÊÁ¿R[h
        /// </summary>
        [Required]
        [Column("YnÊÁ¿R[h", Order = 9)]
        [StringLength(5)]
        public string YnÊÁ¿R[h { get; set; }

        /// <summary>
        /// c_ÎÛOtO
        /// </summary>
        [Required]
        [Column("c_ÎÛOtO", Order = 10)]
        [StringLength(1)]
        public string c_ÎÛOtO { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiP
        /// </summary>
        [Column("©ÆÛLÊ_KiP")]
        public Decimal? ©ÆÛLÊ_KiP { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiQ
        /// </summary>
        [Column("©ÆÛLÊ_KiQ")]
        public Decimal? ©ÆÛLÊ_KiQ { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiR
        /// </summary>
        [Column("©ÆÛLÊ_KiR")]
        public Decimal? ©ÆÛLÊ_KiR { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiS
        /// </summary>
        [Column("©ÆÛLÊ_KiS")]
        public Decimal? ©ÆÛLÊ_KiS { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiT
        /// </summary>
        [Column("©ÆÛLÊ_KiT")]
        public Decimal? ©ÆÛLÊ_KiT { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiU
        /// </summary>
        [Column("©ÆÛLÊ_KiU")]
        public Decimal? ©ÆÛLÊ_KiU { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiV
        /// </summary>
        [Column("©ÆÛLÊ_KiV")]
        public Decimal? ©ÆÛLÊ_KiV { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiW
        /// </summary>
        [Column("©ÆÛLÊ_KiW")]
        public Decimal? ©ÆÛLÊ_KiW { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiX
        /// </summary>
        [Column("©ÆÛLÊ_KiX")]
        public Decimal? ©ÆÛLÊ_KiX { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiPO
        /// </summary>
        [Column("©ÆÛLÊ_KiPO")]
        public Decimal? ©ÆÛLÊ_KiPO { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiPP
        /// </summary>
        [Column("©ÆÛLÊ_KiPP")]
        public Decimal? ©ÆÛLÊ_KiPP { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiPQ
        /// </summary>
        [Column("©ÆÛLÊ_KiPQ")]
        public Decimal? ©ÆÛLÊ_KiPQ { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiPR
        /// </summary>
        [Column("©ÆÛLÊ_KiPR")]
        public Decimal? ©ÆÛLÊ_KiPR { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiPS
        /// </summary>
        [Column("©ÆÛLÊ_KiPS")]
        public Decimal? ©ÆÛLÊ_KiPS { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiPT
        /// </summary>
        [Column("©ÆÛLÊ_KiPT")]
        public Decimal? ©ÆÛLÊ_KiPT { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiPU
        /// </summary>
        [Column("©ÆÛLÊ_KiPU")]
        public Decimal? ©ÆÛLÊ_KiPU { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiPV
        /// </summary>
        [Column("©ÆÛLÊ_KiPV")]
        public Decimal? ©ÆÛLÊ_KiPV { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiPW
        /// </summary>
        [Column("©ÆÛLÊ_KiPW")]
        public Decimal? ©ÆÛLÊ_KiPW { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiPX
        /// </summary>
        [Column("©ÆÛLÊ_KiPX")]
        public Decimal? ©ÆÛLÊ_KiPX { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiQO
        /// </summary>
        [Column("©ÆÛLÊ_KiQO")]
        public Decimal? ©ÆÛLÊ_KiQO { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiQP
        /// </summary>
        [Column("©ÆÛLÊ_KiQP")]
        public Decimal? ©ÆÛLÊ_KiQP { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiQQ
        /// </summary>
        [Column("©ÆÛLÊ_KiQQ")]
        public Decimal? ©ÆÛLÊ_KiQQ { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiQR
        /// </summary>
        [Column("©ÆÛLÊ_KiQR")]
        public Decimal? ©ÆÛLÊ_KiQR { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiQS
        /// </summary>
        [Column("©ÆÛLÊ_KiQS")]
        public Decimal? ©ÆÛLÊ_KiQS { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiQT
        /// </summary>
        [Column("©ÆÛLÊ_KiQT")]
        public Decimal? ©ÆÛLÊ_KiQT { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiQU
        /// </summary>
        [Column("©ÆÛLÊ_KiQU")]
        public Decimal? ©ÆÛLÊ_KiQU { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiQV
        /// </summary>
        [Column("©ÆÛLÊ_KiQV")]
        public Decimal? ©ÆÛLÊ_KiQV { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiQW
        /// </summary>
        [Column("©ÆÛLÊ_KiQW")]
        public Decimal? ©ÆÛLÊ_KiQW { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiQX
        /// </summary>
        [Column("©ÆÛLÊ_KiQX")]
        public Decimal? ©ÆÛLÊ_KiQX { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiRO
        /// </summary>
        [Column("©ÆÛLÊ_KiRO")]
        public Decimal? ©ÆÛLÊ_KiRO { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiRP
        /// </summary>
        [Column("©ÆÛLÊ_KiRP")]
        public Decimal? ©ÆÛLÊ_KiRP { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiRQ
        /// </summary>
        [Column("©ÆÛLÊ_KiRQ")]
        public Decimal? ©ÆÛLÊ_KiRQ { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiRR
        /// </summary>
        [Column("©ÆÛLÊ_KiRR")]
        public Decimal? ©ÆÛLÊ_KiRR { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiRS
        /// </summary>
        [Column("©ÆÛLÊ_KiRS")]
        public Decimal? ©ÆÛLÊ_KiRS { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiRT
        /// </summary>
        [Column("©ÆÛLÊ_KiRT")]
        public Decimal? ©ÆÛLÊ_KiRT { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiRU
        /// </summary>
        [Column("©ÆÛLÊ_KiRU")]
        public Decimal? ©ÆÛLÊ_KiRU { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiRV
        /// </summary>
        [Column("©ÆÛLÊ_KiRV")]
        public Decimal? ©ÆÛLÊ_KiRV { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiRW
        /// </summary>
        [Column("©ÆÛLÊ_KiRW")]
        public Decimal? ©ÆÛLÊ_KiRW { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiRX
        /// </summary>
        [Column("©ÆÛLÊ_KiRX")]
        public Decimal? ©ÆÛLÊ_KiRX { get; set; }

        /// <summary>
        /// ©ÆÛLÊ_KiSO
        /// </summary>
        [Column("©ÆÛLÊ_KiSO")]
        public Decimal? ©ÆÛLÊ_KiSO { get; set; }

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
