using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_00070_ûnÊmFû@¼Ì
    /// </summary>
    [Serializable]
    [Table("m_00070_ûnÊmFû@¼Ì")]
    public class M00070ûnÊmFû@¼Ì : ModelBase
    {
        /// <summary>
        /// ûnÊmFû@
        /// </summary>
        [Required]
        [Key]
        [Column("ûnÊmFû@", Order = 1)]
        [StringLength(2)]
        public string ûnÊmFû@ { get; set; }

        /// <summary>
        /// ûnÊmFû@¼Ì
        /// </summary>
        [Column("ûnÊmFû@¼Ì")]
        [StringLength(30)]
        public string ûnÊmFû@¼Ì { get; set; }

        /// <summary>
        /// Â\æª
        /// </summary>
        [Column("Â\æª")]
        [StringLength(1)]
        public string Â\æª { get; set; }

        /// <summary>
        /// SEÁátO
        /// </summary>
        [Column("SEÁátO")]
        [StringLength(1)]
        public string SEÁátO { get; set; }

        /// <summary>
        /// Ûàøóû®
        /// </summary>
        [Column("Ûàøóû®")]
        [StringLength(2)]
        public string Ûàøóû® { get; set; }

        /// <summary>
        /// Áü\æª
        /// </summary>
        [Column("Áü\æª")]
        [StringLength(1)]
        public string Áü\æª { get; set; }

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
