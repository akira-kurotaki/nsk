using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 帳票処理管理マスタ
    /// </summary>
    [Serializable]
    [Table("m_report_kanri")]
    [PrimaryKey(nameof(ReportControlId), nameof(SerialNumber))]
    public class MReportKanri : ModelBase
    {
        /// <summary>
        /// 帳票制御ID
        /// </summary>
        [Required]
        [Column("report_control_id", Order = 2)]
        [StringLength(10)]
        public string ReportControlId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("serial_number", Order = 1)]
        public int SerialNumber { get; set; }

        /// <summary>
        /// バッチ処理対象件数
        /// </summary>
        [Required]
        [Column("batch_shori_kensu")]
        public short BatchShoriKensu { get; set; }

        /// <summary>
        /// 帳票制御名
        /// </summary>
        [Required]
        [Column("report_control_nm")]
        [StringLength(100)]
        public string ReportControlNm { get; set; }

        /// <summary>
        /// ファイル名
        /// </summary>
        [Required]
        [Column("file_nm")]
        [StringLength(100)]
        public string FileNm { get; set; }

        /// <summary>
        /// 予約処理名
        /// </summary>
        [Required]
        [Column("yoyaku_nm")]
        [StringLength(100)]
        public string YoyakuNm { get; set; }

        /// <summary>
        /// 登録ユーザID
        /// </summary>
        [Required]
        [Column("insert_user_id")]
        [StringLength(11)]
        public string InsertUserId { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Required]
        [Column("insert_date")]
        public DateTime InsertDate { get; set; }

        /// <summary>
        /// 更新ユーザID
        /// </summary>
        [Required]
        [Column("update_user_id")]
        [StringLength(11)]
        public string UpdateUserId { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Required]
        [Column("update_date")]
        public DateTime UpdateDate { get; set; }
    }
}
