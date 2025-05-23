using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_00050_ðtñ
    /// </summary>
    [Serializable]
    [Table("t_00050_ðtñ")]
    [PrimaryKey(nameof(¤ÏÚIR[h), nameof(NY), nameof(Sàðtæª), nameof(ðtñ))]
    public class T00050ðtñ : ModelBase
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
        /// Sàðtæª
        /// </summary>
        [Required]
        [Column("Sàðtæª", Order = 4)]
        [StringLength(1)]
        public string Sàðtæª { get; set; }

        /// <summary>
        /// ðtñ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ðtñ", Order = 5)]
        public short ðtñ { get; set; }

        /// <summary>
        /// RÃ¯ññ
        /// </summary>
        [Column("RÃ¯ññ")]
        public short? RÃ¯ññ { get; set; }

        /// <summary>
        /// ðtvZÀ{ú
        /// </summary>
        [Column("ðtvZÀ{ú")]
        public DateTime? ðtvZÀ{ú { get; set; }

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
