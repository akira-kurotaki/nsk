using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    ///  [ð
    /// </summary>
    [Serializable]
    [Table("t_report_jouken")]
    [PrimaryKey(nameof(JoukenId), nameof(SerialNumber))]
    public class TReportJouken : ModelBase
    {
        /// <summary>
        /// ðID
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("jouken_id", Order = 1)]
        public int JoukenId { get; set; }

        /// <summary>
        /// AÔ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("serial_number", Order = 2)]
        public int SerialNumber { get; set; }

        /// <summary>
        /// ð¼
        /// </summary>
        [Required]
        [Column("jouken_nm")]
        [StringLength(100)]
        public string JoukenNm { get; set; }

        /// <summary>
        /// \¦pðl
        /// </summary>
        [Column("jouken_display_value")]
        [StringLength(300)]
        public string JoukenDisplayValue { get; set; }

        /// <summary>
        /// ðl
        /// </summary>
        [Column("jouken_value")]
        [StringLength(300)]
        public string JoukenValue { get; set; }

        /// <summary>
        /// o^[UID
        /// </summary>
        [Required]
        [Column("insert_user_id")]
        [StringLength(11)]
        public string InsertUserId { get; set; }

        /// <summary>
        /// o^ú
        /// </summary>
        [Required]
        [Column("insert_date")]
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// XV[UID
        /// </summary>
        [Required]
        [Column("update_user_id")]
        [StringLength(11)]
        public string UpdateUserId { get; set; }

        /// <summary>
        /// XVú
        /// </summary>
        [Required]
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }
    }
}
