using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 即時CSV出力条件件数マスタ
    /// </summary>
    [Serializable]
    [Table("m_immed_csv_max_count")]
    [PrimaryKey(nameof(SystemKbn), nameof(TableNm))]
    public class MImmedCsvMaxCount : ModelBase
    {
        /// <summary>
        /// システム区分
        /// </summary>
        [Required]
        [Column("system_kbn", Order = 1)]
        [StringLength(2)]
        public string SystemKbn { get; set; }

        /// <summary>
        /// テーブル名
        /// </summary>
        [Required]
        [Column("table_nm", Order = 2)]
        [StringLength(100)]
        public string TableNm { get; set; }

        /// <summary>
        /// 即時CSV出力上限件数
        /// </summary>
        [Required]
        [Column("immed_csv_max_count")]
        public int ImmedCsvMaxCount { get; set; }

        /// <summary>
        /// 登録ユーザID
        /// </summary>
        [Column("insert_user_id")]
        [StringLength(11)]
        public string InsertUserId { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("insert_date")]
        public DateTime? InsertDate { get; set; }

        /// <summary>
        /// 更新ユーザID
        /// </summary>
        [Column("update_user_id")]
        [StringLength(11)]
        public string UpdateUserId { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
    }
}
