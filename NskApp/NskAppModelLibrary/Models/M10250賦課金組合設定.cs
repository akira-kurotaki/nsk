using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10250_Ûà_gÝè
    /// </summary>
    [Serializable]
    [Table("m_10250_Ûà_gÝè")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h))]
    public class M10250ÛàgÝè : ModelBase
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
        /// ¥ûPÊÅè¥ûz_gptO
        /// </summary>
        [Column("¥ûPÊÅè¥ûz_gptO")]
        [StringLength(1)]
        public string ¥ûPÊÅè¥ûz_gptO { get; set; }

        /// <summary>
        /// ¥ûPÊÅè¥ûz
        /// </summary>
        [Column("¥ûPÊÅè¥ûz")]
        public Decimal? ¥ûPÊÅè¥ûz { get; set; }

        /// <summary>
        /// ¥ûPÊÛàãÀz_gptO
        /// </summary>
        [Column("¥ûPÊÛàãÀz_gptO")]
        [StringLength(1)]
        public string ¥ûPÊÛàãÀz_gptO { get; set; }

        /// <summary>
        /// ¥ûPÊÛàãÀz
        /// </summary>
        [Column("¥ûPÊÛàãÀz")]
        public Decimal? ¥ûPÊÛàãÀz { get; set; }

        /// <summary>
        /// _ÆS¤Ï|à_ãÀtO
        /// </summary>
        [Column("_ÆS¤Ï|à_ãÀtO")]
        [StringLength(1)]
        public string _ÆS¤Ï|à_ãÀtO { get; set; }

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
