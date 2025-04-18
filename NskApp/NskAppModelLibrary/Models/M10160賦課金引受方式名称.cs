using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10160_Ûàøóû®¼Ì
    /// </summary>
    [Serializable]
    [Table("m_10160_Ûàøóû®¼Ì")]
    public class M10160Ûàøóû®¼Ì : ModelBase
    {
        /// <summary>
        /// Ûàøóû®
        /// </summary>
        [Required]
        [Key]
        [Column("Ûàøóû®", Order = 1)]
        [StringLength(2)]
        public string Ûàøóû® { get; set; }

        /// <summary>
        /// Ûàøóû®¼Ì
        /// </summary>
        [Column("Ûàøóû®¼Ì")]
        [StringLength(20)]
        public string Ûàøóû®¼Ì { get; set; }

        /// <summary>
        /// øóû®
        /// </summary>
        [Column("øóû®")]
        [StringLength(1)]
        public string øóû® { get; set; }

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
