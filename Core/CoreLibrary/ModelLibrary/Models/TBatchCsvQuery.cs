using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// バッチCSV取得条件
    /// </summary>
    [Serializable]
    [Table("t_batch_csv_query")]
    [PrimaryKey(nameof(BatchId), nameof(SerialNumber))]
    public class TBatchCsvQuery : ModelBase
    {
        /// <summary>
        /// バッチID
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("batch_id", Order = 1)]
        public long BatchId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("serial_number", Order = 2)]
        public int SerialNumber { get; set; }

        /// <summary>
        /// 検索対象
        /// </summary>
        [Required]
        [Column("target_entity")]
        [StringLength(100)]
        public string TargetEntity { get; set; }

        /// <summary>
        /// 検索条件（SQL）
        /// </summary>
        [Required]
        [Column("query_condition")]
        public string QueryCondition { get; set; }

        /// <summary>
        /// 検索条件（パラメータ）
        /// </summary>
        [Required]
        [Column("query_param")]
        public string QueryParam { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        [Required]
        [Column("sort")]
        public string Sort { get; set; }

        /// <summary>
        /// 文字コード
        /// </summary>
        [Required]
        [Column("character_cd")]
        [StringLength(1)]
        public string CharacterCd { get; set; }

        /// <summary>
        /// CSVファイル名
        /// </summary>
        [Required]
        [Column("csv_file_name")]
        [StringLength(100)]
        public string CsvFileName { get; set; }

        /// <summary>
        /// ヘッダ有無
        /// </summary>
        [Required]
        [Column("header_umu")]
        [StringLength(1)]
        public string HeaderUmu { get; set; }

        /// <summary>
        /// ヘッダ文字列リスト
        /// </summary>
        [Column("header_list")]
        public string HeaderList { get; set; }

        /// <summary>
        /// セパレーター
        /// </summary>
        [Required]
        [Column("separator")]
        [StringLength(1)]
        public string Separator { get; set; }

        /// <summary>
        /// BOMコード有無
        /// </summary>
        [Required]
        [Column("bom_cd_umu")]
        [StringLength(1)]
        public string BomCdUmu { get; set; }

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
