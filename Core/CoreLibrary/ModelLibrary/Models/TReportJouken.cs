using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 帳票条件
    /// </summary>
    [Serializable]
    [Table("t_report_jouken")]
    [PrimaryKey(nameof(JoukenId), nameof(SerialNumber))]
    public class TReportJouken : ModelBase
    {
        /// <summary>
        /// 条件ID
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("jouken_id", Order = 1)]
        public int JoukenId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("serial_number", Order = 2)]
        public int SerialNumber { get; set; }

        /// <summary>
        /// 条件名
        /// </summary>
        [Required]
        [Column("jouken_nm")]
        [StringLength(100)]
        public string JoukenNm { get; set; }

        /// <summary>
        /// 表示用条件値
        /// </summary>
        [Column("jouken_display_value")]
        [StringLength(300)]
        public string JoukenDisplayValue { get; set; }

        /// <summary>
        /// 条件値
        /// </summary>
        [Column("jouken_value")]
        [StringLength(300)]
        public string JoukenValue { get; set; }

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
