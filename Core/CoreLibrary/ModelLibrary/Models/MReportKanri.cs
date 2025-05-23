using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    ///  [Ç}X^
    /// </summary>
    [Serializable]
    [Table("m_report_kanri")]
    [PrimaryKey(nameof(ReportControlId), nameof(SerialNumber))]
    public class MReportKanri : ModelBase
    {
        /// <summary>
        ///  [§äID
        /// </summary>
        [Required]
        [Column("report_control_id", Order = 2)]
        [StringLength(10)]
        public string ReportControlId { get; set; }

        /// <summary>
        /// AÔ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("serial_number", Order = 1)]
        public int SerialNumber { get; set; }

        /// <summary>
        /// ob`ÎÛ
        /// </summary>
        [Required]
        [Column("batch_shori_kensu")]
        public short BatchShoriKensu { get; set; }

        /// <summary>
        ///  [§ä¼
        /// </summary>
        [Required]
        [Column("report_control_nm")]
        [StringLength(100)]
        public string ReportControlNm { get; set; }

        /// <summary>
        /// t@C¼
        /// </summary>
        [Required]
        [Column("file_nm")]
        [StringLength(100)]
        public string FileNm { get; set; }

        /// <summary>
        /// \ñ¼
        /// </summary>
        [Required]
        [Column("yoyaku_nm")]
        [StringLength(100)]
        public string YoyakuNm { get; set; }

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
