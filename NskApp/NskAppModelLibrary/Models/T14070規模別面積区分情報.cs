using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_14070_KÍÊÊÏæªîñ
    /// </summary>
    [Serializable]
    [Table("t_14070_KÍÊÊÏæªîñ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(xR[h), nameof(KÍÊÊÏæª))]
    public class T14070KÍÊÊÏæªîñ : ModelBase
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
        /// xR[h
        /// </summary>
        [Required]
        [Column("xR[h", Order = 4)]
        [StringLength(2)]
        public string xR[h { get; set; }

        /// <summary>
        /// KÍÊÊÏæª
        /// </summary>
        [Required]
        [Column("KÍÊÊÏæª", Order = 5)]
        [StringLength(2)]
        public string KÍÊÊÏæª { get; set; }

        /// <summary>
        /// ÎÛÊÏãÀ
        /// </summary>
        [Column("ÎÛÊÏãÀ")]
        [StringLength(5)]
        public string ÎÛÊÏãÀ { get; set; }

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
