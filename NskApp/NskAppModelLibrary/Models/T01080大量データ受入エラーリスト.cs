using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_01080_åÊf[^óü_G[Xg
    /// </summary>
    [Serializable]
    [Table("t_01080_åÊf[^óü_G[Xg")]
    [PrimaryKey(nameof(æª), nameof(ðid), nameof(}Ô))]
    public class T01080åÊf[^óüG[Xg : ModelBase
    {
        /// <summary>
        /// æª
        /// </summary>
        [Required]
        [Column("æª", Order = 1)]
        [StringLength(1)]
        public string æª { get; set; }

        /// <summary>
        /// ðid
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ðid", Order = 2)]
        public long ðid { get; set; }

        /// <summary>
        /// }Ô
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("}Ô", Order = 3)]
        public long }Ô { get; set; }

        /// <summary>
        /// sÔ
        /// </summary>
        [Column("sÔ")]
        [StringLength(7)]
        public string sÔ { get; set; }

        /// <summary>
        /// G[àe
        /// </summary>
        [Column("G[àe")]
        public string G[àe { get; set; }

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
