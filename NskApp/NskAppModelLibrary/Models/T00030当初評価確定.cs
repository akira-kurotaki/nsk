using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_00030_]¿mè
    /// </summary>
    [Serializable]
    [Table("t_00030_]¿mè")]
    [PrimaryKey(nameof(¤ÏÚIR[h), nameof(NY), nameof(xR[h), nameof(øóû®), nameof(­{Û¯Fèæª))]
    public class T00030]¿mè : ModelBase
    {
        /// <summary>
        /// gR[h
        /// </summary>
        [Required]
        [Column("gR[h", Order = 3)]
        [StringLength(3)]
        public string gR[h { get; set; }

        /// <summary>
        /// ¤ÏÚIR[h
        /// </summary>
        [Required]
        [Column("¤ÏÚIR[h", Order = 1)]
        [StringLength(2)]
        public string ¤ÏÚIR[h { get; set; }

        /// <summary>
        /// NY
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("NY", Order = 2)]
        public short NY { get; set; }

        /// <summary>
        /// xR[h
        /// </summary>
        [Required]
        [Column("xR[h", Order = 4)]
        [StringLength(2)]
        public string xR[h { get; set; }

        /// <summary>
        /// øóû®
        /// </summary>
        [Required]
        [Column("øóû®", Order = 5)]
        [StringLength(1)]
        public string øóû® { get; set; }

        /// <summary>
        /// ­{Û¯Fèæª
        /// </summary>
        [Required]
        [Column("­{Û¯Fèæª", Order = 6)]
        [StringLength(4)]
        public string ­{Û¯Fèæª { get; set; }

        /// <summary>
        /// ]¿vZÀ{ú
        /// </summary>
        [Column("]¿vZÀ{ú")]
        public DateTime? ]¿vZÀ{ú { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("")]
        public int?  { get; set; }

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
