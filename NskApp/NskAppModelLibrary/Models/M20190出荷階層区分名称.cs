using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_20190_o×Kwæª¼Ì
    /// </summary>
    [Serializable]
    [Table("m_20190_o×Kwæª¼Ì")]
    [PrimaryKey(nameof(gR[h), nameof(NY), nameof(¤ÏÚIR[h), nameof(o×]¿næR[h), nameof(o×Kwæª))]
    public class M20190o×Kwæª¼Ì : ModelBase
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
        /// o×]¿næR[h
        /// </summary>
        [Required]
        [Column("o×]¿næR[h", Order = 4)]
        [StringLength(4)]
        public string o×]¿næR[h { get; set; }

        /// <summary>
        /// o×Kwæª
        /// </summary>
        [Required]
        [Column("o×Kwæª", Order = 5)]
        [StringLength(3)]
        public string o×Kwæª { get; set; }

        /// <summary>
        /// o×Kwæª¼Ì
        /// </summary>
        [Column("o×Kwæª¼Ì")]
        [StringLength(20)]
        public string o×Kwæª¼Ì { get; set; }

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
