using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_23030_½ÏPû·Ið_²æÇ
    /// </summary>
    [Serializable]
    [Table("t_23030_½ÏPû·Ið_²æÇ")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(xR[h), nameof(Þæª), nameof(øóû®), nameof(âR[h), nameof(Kwæª))]
    public class T23030½ÏPû·Ið²æÇ : ModelBase
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
        /// Þæª
        /// </summary>
        [Required]
        [Column("Þæª", Order = 5)]
        [StringLength(2)]
        public string Þæª { get; set; }

        /// <summary>
        /// øóû®
        /// </summary>
        [Required]
        [Column("øóû®", Order = 6)]
        [StringLength(1)]
        public string øóû® { get; set; }

        /// <summary>
        /// âR[h
        /// </summary>
        [Required]
        [Column("âR[h", Order = 7)]
        [StringLength(2)]
        public string âR[h { get; set; }

        /// <summary>
        /// Kwæª
        /// </summary>
        [Required]
        [Column("Kwæª", Order = 8)]
        [StringLength(3)]
        public string Kwæª { get; set; }

        /// <summary>
        /// ½ÏPû·vZû@
        /// </summary>
        [Column("½ÏPû·vZû@")]
        [StringLength(1)]
        public string ½ÏPû·vZû@ { get; set; }

        /// <summary>
        /// ²®Ç½ÏPû·vZtO
        /// </summary>
        [Column("²®Ç½ÏPû·vZtO")]
        [StringLength(1)]
        public string ²®Ç½ÏPû·vZtO { get; set; }

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
