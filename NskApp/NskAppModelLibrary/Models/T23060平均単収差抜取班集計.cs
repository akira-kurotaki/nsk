using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_23060_½ÏPû·_²æÇWv
    /// </summary>
    [Serializable]
    [Table("t_23060_½ÏPû·_²æÇWv")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(xR[h), nameof(Þæª), nameof(øóû®), nameof(âR[h), nameof(²æ²¸ÇR[h))]
    public class T23060½ÏPû·²æÇWv : ModelBase
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
        /// ²æ²¸ÇR[h
        /// </summary>
        [Required]
        [Column("²æ²¸ÇR[h", Order = 8)]
        [StringLength(3)]
        public string ²æ²¸ÇR[h { get; set; }

        /// <summary>
        /// ÀªM
        /// </summary>
        [Column("ÀªM")]
        public Decimal? ÀªM { get; set; }

        /// <summary>
        /// ²æ²¸©Pû
        /// </summary>
        [Column("²æ²¸©Pû")]
        public Decimal? ²æ²¸©Pû { get; set; }

        /// <summary>
        /// ÀªPû
        /// </summary>
        [Column("ÀªPû")]
        public Decimal? ÀªPû { get; set; }

        /// <summary>
        /// ©PûC³W
        /// </summary>
        [Column("©PûC³W")]
        public Decimal? ©PûC³W { get; set; }

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
