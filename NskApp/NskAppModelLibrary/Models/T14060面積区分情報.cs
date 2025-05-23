using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_14060_ÊÏæªîñ
    /// </summary>
    [Serializable]
    [Table("t_14060_ÊÏæªîñ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(seqno))]
    public class T14060ÊÏæªîñ : ModelBase
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
        /// seqno
        /// </summary>
        [Required]
        [Column("seqno", Order = 4)]
        [StringLength(3)]
        public string seqno { get; set; }

        /// <summary>
        /// øóÊÏæªP
        /// </summary>
        [Column("øóÊÏæªP")]
        [StringLength(5)]
        public string øóÊÏæªP { get; set; }

        /// <summary>
        /// øóÊÏæªQ
        /// </summary>
        [Column("øóÊÏæªQ")]
        [StringLength(5)]
        public string øóÊÏæªQ { get; set; }

        /// <summary>
        /// øóÊÏæªR
        /// </summary>
        [Column("øóÊÏæªR")]
        [StringLength(5)]
        public string øóÊÏæªR { get; set; }

        /// <summary>
        /// øóÊÏæªS
        /// </summary>
        [Column("øóÊÏæªS")]
        [StringLength(5)]
        public string øóÊÏæªS { get; set; }

        /// <summary>
        /// øóÊÏæªT
        /// </summary>
        [Column("øóÊÏæªT")]
        [StringLength(5)]
        public string øóÊÏæªT { get; set; }

        /// <summary>
        /// øóÊÏæªU
        /// </summary>
        [Column("øóÊÏæªU")]
        [StringLength(5)]
        public string øóÊÏæªU { get; set; }

        /// <summary>
        /// øóÊÏæªV
        /// </summary>
        [Column("øóÊÏæªV")]
        [StringLength(5)]
        public string øóÊÏæªV { get; set; }

        /// <summary>
        /// øóÊÏæªW
        /// </summary>
        [Column("øóÊÏæªW")]
        [StringLength(5)]
        public string øóÊÏæªW { get; set; }

        /// <summary>
        /// øóÊÏæªX
        /// </summary>
        [Column("øóÊÏæªX")]
        [StringLength(5)]
        public string øóÊÏæªX { get; set; }

        /// <summary>
        /// øóÊÏæªPO
        /// </summary>
        [Column("øóÊÏæªPO")]
        [StringLength(5)]
        public string øóÊÏæªPO { get; set; }

        /// <summary>
        /// øóÊÏæªPP
        /// </summary>
        [Column("øóÊÏæªPP")]
        [StringLength(5)]
        public string øóÊÏæªPP { get; set; }

        /// <summary>
        /// øóÊÏæªPQ
        /// </summary>
        [Column("øóÊÏæªPQ")]
        [StringLength(5)]
        public string øóÊÏæªPQ { get; set; }

        /// <summary>
        /// øóÊÏæªPR
        /// </summary>
        [Column("øóÊÏæªPR")]
        [StringLength(5)]
        public string øóÊÏæªPR { get; set; }

        /// <summary>
        /// øóÊÏæªPS
        /// </summary>
        [Column("øóÊÏæªPS")]
        [StringLength(5)]
        public string øóÊÏæªPS { get; set; }

        /// <summary>
        /// øóÊÏæªPT
        /// </summary>
        [Column("øóÊÏæªPT")]
        [StringLength(5)]
        public string øóÊÏæªPT { get; set; }

        /// <summary>
        /// øóÊÏæªPU
        /// </summary>
        [Column("øóÊÏæªPU")]
        [StringLength(5)]
        public string øóÊÏæªPU { get; set; }

        /// <summary>
        /// øóÊÏæªPV
        /// </summary>
        [Column("øóÊÏæªPV")]
        [StringLength(5)]
        public string øóÊÏæªPV { get; set; }

        /// <summary>
        /// øóÊÏæªPW
        /// </summary>
        [Column("øóÊÏæªPW")]
        [StringLength(5)]
        public string øóÊÏæªPW { get; set; }

        /// <summary>
        /// øóÊÏæªPX
        /// </summary>
        [Column("øóÊÏæªPX")]
        [StringLength(5)]
        public string øóÊÏæªPX { get; set; }

        /// <summary>
        /// øóÊÏæªQO
        /// </summary>
        [Column("øóÊÏæªQO")]
        [StringLength(5)]
        public string øóÊÏæªQO { get; set; }

        /// <summary>
        /// øóÊÏæªQP
        /// </summary>
        [Column("øóÊÏæªQP")]
        [StringLength(5)]
        public string øóÊÏæªQP { get; set; }

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
