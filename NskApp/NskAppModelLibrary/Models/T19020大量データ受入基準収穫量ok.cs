using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_19020_åÊf[^óü_îûnÊok
    /// </summary>
    [Serializable]
    [Table("t_19020_åÊf[^óü_îûnÊok")]
    [PrimaryKey(nameof(óüðid), nameof(sÔ))]
    public class T19020åÊf[^óüîûnÊok : ModelBase
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("sÔ", Order = 2)]
        public Decimal sÔ { get; set; }

        /// <summary>
        /// æª
        /// </summary>
        [Column("æª")]
        [StringLength(1)]
        public string æª { get; set; }

        /// <summary>
        /// gR[h
        /// </summary>
        [Column("gR[h")]
        [StringLength(3)]
        public string gR[h { get; set; }

        /// <summary>
        /// g¼
        /// </summary>
        [Column("g¼")]
        [StringLength(50)]
        public string g¼ { get; set; }

        /// <summary>
        /// NY
        /// </summary>
        [Column("NY")]
        public short? NY { get; set; }

        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Column("¤ÏÚIR[h")]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// ¤ÏÚI¼
        /// </summary>
        [Column("¤ÏÚI¼")]
        [StringLength(20)]
        public string ¤ÏÚI¼ { get; set; }

        /// <summary>
        /// øóû®
        /// </summary>
        [Column("øóû®")]
        [StringLength(1)]
        public string øóû® { get; set; }

        /// <summary>
        /// øóû®¼Ì
        /// </summary>
        [Column("øóû®¼Ì")]
        [StringLength(20)]
        public string øóû®¼Ì { get; set; }

        /// <summary>
        /// xR[h
        /// </summary>
        [Column("xR[h")]
        [StringLength(2)]
        public string xR[h { get; set; }

        /// <summary>
        /// x¼
        /// </summary>
        [Column("x¼")]
        [StringLength(20)]
        public string x¼ { get; set; }

        /// <summary>
        /// ånæR[h
        /// </summary>
        [Column("ånæR[h")]
        [StringLength(2)]
        public string ånæR[h { get; set; }

        /// <summary>
        /// ånæ¼
        /// </summary>
        [Column("ånæ¼")]
        [StringLength(10)]
        public string ånæ¼ { get; set; }

        /// <summary>
        /// ¬næR[h
        /// </summary>
        [Column("¬næR[h")]
        [StringLength(4)]
        public string ¬næR[h { get; set; }

        /// <summary>
        /// ¬næ¼
        /// </summary>
        [Column("¬næ¼")]
        [StringLength(10)]
        public string ¬næ¼ { get; set; }

        /// <summary>
        /// gõR[h
        /// </summary>
        [Column("gõR[h")]
        [StringLength(13)]
        public string gõR[h { get; set; }

        /// <summary>
        /// gõ¼
        /// </summary>
        [Column("gõ¼")]
        [StringLength(30)]
        public string gõ¼ { get; set; }

        /// <summary>
        /// Þæª
        /// </summary>
        [Column("Þæª")]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// Þæª¼
        /// </summary>
        [Column("Þæª¼")]
        [StringLength(40)]
        public string Þæª¼ { get; set; }

        /// <summary>
        /// YnÊÁ¿R[h
        /// </summary>
        [Column("YnÊÁ¿R[h")]
        [StringLength(5)]
        public string YnÊÁ¿R[h { get; set; }

        /// <summary>
        /// c_ÎÛOtO
        /// </summary>
        [Column("c_ÎÛOtO")]
        [StringLength(1)]
        public string c_ÎÛOtO { get; set; }

        /// <summary>
        /// YnÊÁ¿¼Ì
        /// </summary>
        [Column("YnÊÁ¿¼Ì")]
        [StringLength(30)]
        public string YnÊÁ¿¼Ì { get; set; }

        /// <summary>
        /// ½ÏPû
        /// </summary>
        [Column("½ÏPû")]
        public Decimal? ½ÏPû { get; set; }

        /// <summary>
        /// KiÊ_KiP
        /// </summary>
        [Column("KiÊ_KiP")]
        public Decimal? KiÊ_KiP { get; set; }

        /// <summary>
        /// KiÊ_KiQ
        /// </summary>
        [Column("KiÊ_KiQ")]
        public Decimal? KiÊ_KiQ { get; set; }

        /// <summary>
        /// KiÊ_KiR
        /// </summary>
        [Column("KiÊ_KiR")]
        public Decimal? KiÊ_KiR { get; set; }

        /// <summary>
        /// KiÊ_KiS
        /// </summary>
        [Column("KiÊ_KiS")]
        public Decimal? KiÊ_KiS { get; set; }

        /// <summary>
        /// KiÊ_KiT
        /// </summary>
        [Column("KiÊ_KiT")]
        public Decimal? KiÊ_KiT { get; set; }

        /// <summary>
        /// KiÊ_KiU
        /// </summary>
        [Column("KiÊ_KiU")]
        public Decimal? KiÊ_KiU { get; set; }

        /// <summary>
        /// KiÊ_KiV
        /// </summary>
        [Column("KiÊ_KiV")]
        public Decimal? KiÊ_KiV { get; set; }

        /// <summary>
        /// KiÊ_KiW
        /// </summary>
        [Column("KiÊ_KiW")]
        public Decimal? KiÊ_KiW { get; set; }

        /// <summary>
        /// KiÊ_KiX
        /// </summary>
        [Column("KiÊ_KiX")]
        public Decimal? KiÊ_KiX { get; set; }

        /// <summary>
        /// KiÊ_KiPO
        /// </summary>
        [Column("KiÊ_KiPO")]
        public Decimal? KiÊ_KiPO { get; set; }

        /// <summary>
        /// KiÊ_KiPP
        /// </summary>
        [Column("KiÊ_KiPP")]
        public Decimal? KiÊ_KiPP { get; set; }

        /// <summary>
        /// KiÊ_KiPQ
        /// </summary>
        [Column("KiÊ_KiPQ")]
        public Decimal? KiÊ_KiPQ { get; set; }

        /// <summary>
        /// KiÊ_KiPR
        /// </summary>
        [Column("KiÊ_KiPR")]
        public Decimal? KiÊ_KiPR { get; set; }

        /// <summary>
        /// KiÊ_KiPS
        /// </summary>
        [Column("KiÊ_KiPS")]
        public Decimal? KiÊ_KiPS { get; set; }

        /// <summary>
        /// KiÊ_KiPT
        /// </summary>
        [Column("KiÊ_KiPT")]
        public Decimal? KiÊ_KiPT { get; set; }

        /// <summary>
        /// KiÊ_KiPU
        /// </summary>
        [Column("KiÊ_KiPU")]
        public Decimal? KiÊ_KiPU { get; set; }

        /// <summary>
        /// KiÊ_KiPV
        /// </summary>
        [Column("KiÊ_KiPV")]
        public Decimal? KiÊ_KiPV { get; set; }

        /// <summary>
        /// KiÊ_KiPW
        /// </summary>
        [Column("KiÊ_KiPW")]
        public Decimal? KiÊ_KiPW { get; set; }

        /// <summary>
        /// KiÊ_KiPX
        /// </summary>
        [Column("KiÊ_KiPX")]
        public Decimal? KiÊ_KiPX { get; set; }

        /// <summary>
        /// KiÊ_KiQO
        /// </summary>
        [Column("KiÊ_KiQO")]
        public Decimal? KiÊ_KiQO { get; set; }

        /// <summary>
        /// KiÊ_KiQP
        /// </summary>
        [Column("KiÊ_KiQP")]
        public Decimal? KiÊ_KiQP { get; set; }

        /// <summary>
        /// KiÊ_KiQQ
        /// </summary>
        [Column("KiÊ_KiQQ")]
        public Decimal? KiÊ_KiQQ { get; set; }

        /// <summary>
        /// KiÊ_KiQR
        /// </summary>
        [Column("KiÊ_KiQR")]
        public Decimal? KiÊ_KiQR { get; set; }

        /// <summary>
        /// KiÊ_KiQS
        /// </summary>
        [Column("KiÊ_KiQS")]
        public Decimal? KiÊ_KiQS { get; set; }

        /// <summary>
        /// KiÊ_KiQT
        /// </summary>
        [Column("KiÊ_KiQT")]
        public Decimal? KiÊ_KiQT { get; set; }

        /// <summary>
        /// KiÊ_KiQU
        /// </summary>
        [Column("KiÊ_KiQU")]
        public Decimal? KiÊ_KiQU { get; set; }

        /// <summary>
        /// KiÊ_KiQV
        /// </summary>
        [Column("KiÊ_KiQV")]
        public Decimal? KiÊ_KiQV { get; set; }

        /// <summary>
        /// KiÊ_KiQW
        /// </summary>
        [Column("KiÊ_KiQW")]
        public Decimal? KiÊ_KiQW { get; set; }

        /// <summary>
        /// KiÊ_KiQX
        /// </summary>
        [Column("KiÊ_KiQX")]
        public Decimal? KiÊ_KiQX { get; set; }

        /// <summary>
        /// KiÊ_KiRO
        /// </summary>
        [Column("KiÊ_KiRO")]
        public Decimal? KiÊ_KiRO { get; set; }

        /// <summary>
        /// KiÊ_KiRP
        /// </summary>
        [Column("KiÊ_KiRP")]
        public Decimal? KiÊ_KiRP { get; set; }

        /// <summary>
        /// KiÊ_KiRQ
        /// </summary>
        [Column("KiÊ_KiRQ")]
        public Decimal? KiÊ_KiRQ { get; set; }

        /// <summary>
        /// KiÊ_KiRR
        /// </summary>
        [Column("KiÊ_KiRR")]
        public Decimal? KiÊ_KiRR { get; set; }

        /// <summary>
        /// KiÊ_KiRS
        /// </summary>
        [Column("KiÊ_KiRS")]
        public Decimal? KiÊ_KiRS { get; set; }

        /// <summary>
        /// KiÊ_KiRT
        /// </summary>
        [Column("KiÊ_KiRT")]
        public Decimal? KiÊ_KiRT { get; set; }

        /// <summary>
        /// KiÊ_KiRU
        /// </summary>
        [Column("KiÊ_KiRU")]
        public Decimal? KiÊ_KiRU { get; set; }

        /// <summary>
        /// KiÊ_KiRV
        /// </summary>
        [Column("KiÊ_KiRV")]
        public Decimal? KiÊ_KiRV { get; set; }

        /// <summary>
        /// KiÊ_KiRW
        /// </summary>
        [Column("KiÊ_KiRW")]
        public Decimal? KiÊ_KiRW { get; set; }

        /// <summary>
        /// KiÊ_KiRX
        /// </summary>
        [Column("KiÊ_KiRX")]
        public Decimal? KiÊ_KiRX { get; set; }

        /// <summary>
        /// KiÊ_KiSO
        /// </summary>
        [Column("KiÊ_KiSO")]
        public Decimal? KiÊ_KiSO { get; set; }

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
