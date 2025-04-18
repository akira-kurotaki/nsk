using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_01050_ob`ð
    /// </summary>
    [Serializable]
    [Table("t_01050_ob`ð")]
    [PrimaryKey(nameof(ob`ðid), nameof(AÔ))]
    public class T01050ob`ð : ModelBase
    {
        /// <summary>
        /// ob`ðid
        /// </summary>
        [Required]
        [Column("ob`ðid", Order = 1)]
        [StringLength(36)]
        public string ob`ðid { get; set; }

        /// <summary>
        /// AÔ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("AÔ", Order = 2)]
        public int AÔ { get; set; }

        /// <summary>
        /// ð¼Ì
        /// </summary>
        [Column("ð¼Ì")]
        [StringLength(30)]
        public string ð¼Ì { get; set; }

        /// <summary>
        /// \¦pðl
        /// </summary>
        [Column("\¦pðl")]
        [StringLength(30)]
        public string \¦pðl { get; set; }

        /// <summary>
        /// ðl
        /// </summary>
        [Column("ðl")]
        [StringLength(30)]
        public string ðl { get; set; }

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
