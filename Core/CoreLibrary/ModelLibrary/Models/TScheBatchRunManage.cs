using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 定時バッチ起動管理
    /// </summary>
    [Serializable]
    [Table("t_sche_batch_run_manage")]
    [PrimaryKey(nameof(SystemKbn), nameof(BatchNm))]
    public class TScheBatchRunManage : ModelBase
    {
        /// <summary>
        /// システム区分
        /// </summary>
        [Required]
        [Column("system_kbn", Order = 1)]
        [StringLength(2)]
        public string SystemKbn { get; set; }

        /// <summary>
        /// バッチ名
        /// </summary>
        [Required]
        [Column("batch_nm", Order = 2)]
        [StringLength(30)]
        public string BatchNm { get; set; }

        /// <summary>
        /// バッチ起動日
        /// </summary>
        [Column("batch_run_date")]
        public DateTime? BatchRunDate { get; set; }
    }
}
