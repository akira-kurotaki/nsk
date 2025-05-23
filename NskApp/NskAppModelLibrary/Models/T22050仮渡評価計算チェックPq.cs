using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_22050_Όn]ΏvZ`FbN_pq
    /// </summary>
    [Serializable]
    [Table("t_22050_Όn]ΏvZ`FbN_pq")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(€ΟΪIR[h), nameof(gυR[h), nameof(knΤ), nameof(ͺMΤ))]
    public class T22050Όn]ΏvZ`FbNPq : ModelBase
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
        /// €ΟΪIR[h
        /// </summary>
        [Required]
        [Column("€ΟΪIR[h", Order = 3)]
        [StringLength(2)]
        public string €ΟΪIR[h { get; set; }

        /// <summary>
        /// gυR[h
        /// </summary>
        [Required]
        [Column("gυR[h", Order = 4)]
        [StringLength(13)]
        public string gυR[h { get; set; }

        /// <summary>
        /// knΤ
        /// </summary>
        [Required]
        [Column("knΤ", Order = 5)]
        [StringLength(5)]
        public string knΤ { get; set; }

        /// <summary>
        /// ͺMΤ
        /// </summary>
        [Required]
        [Column("ͺMΤ", Order = 6)]
        [StringLength(4)]
        public string ͺMΤ { get; set; }

        /// <summary>
        /// ]ΏERRtO
        /// </summary>
        [Column("]ΏERRtO")]
        [StringLength(1)]
        public string ]ΏERRtO { get; set; }

        /// <summary>
        /// ]ΏWARtO
        /// </summary>
        [Column("]ΏWARtO")]
        [StringLength(1)]
        public string ]ΏWARtO { get; set; }

        /// <summary>
        /// ]ΏERRNO
        /// </summary>
        [Column("]ΏERRNO")]
        [StringLength(30)]
        public string ]ΏERRNO { get; set; }

        /// <summary>
        /// ]ΏSUBJECT
        /// </summary>
        [Column("]ΏSUBJECT")]
        [StringLength(100)]
        public string ]ΏSUBJECT { get; set; }

        /// <summary>
        /// ]ΏERRbZ[W
        /// </summary>
        [Column("]ΏERRbZ[W")]
        [StringLength(512)]
        public string ]ΏERRbZ[W { get; set; }

        /// <summary>
        /// YnΚΑΏR[h
        /// </summary>
        [Column("YnΚΑΏR[h")]
        [StringLength(5)]
        public string YnΚΑΏR[h { get; set; }

        /// <summary>
        /// ήζͺ
        /// </summary>
        [Column("ήζͺ")]
        [StringLength(2)]
        public string ήζͺ { get; set; }

        /// <summary>
        /// c_ΞΫOtO
        /// </summary>
        [Column("c_ΞΫOtO")]
        [StringLength(1)]
        public string c_ΞΫOtO { get; set; }

        /// <summary>
        /// o^ϊ
        /// </summary>
        [Column("o^ϊ")]
        public DateTime? o^ϊ { get; set; }

        /// <summary>
        /// o^[Uid
        /// </summary>
        [Column("o^[Uid")]
        [StringLength(11)]
        public string o^[Uid { get; set; }
    }
}
