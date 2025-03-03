using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// 取込管理
    /// </summary>
    [Serializable]
    [Table("t_torikomi_manage")]
    public class TTorikomiManage : ModelBase
    {
        /// <summary>
        /// バッチID
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("batch_id", Order = 1)]
        public long BatchId { get; set; }

        /// <summary>
        /// 取込対象コード
        /// </summary>
        [Required]
        [Column("torikomi_cd")]
        [StringLength(2)]
        public string TorikomiCd { get; set; }

        /// <summary>
        /// 対象年度
        /// </summary>
        [Required]
        [Column("nendo")]
        public short Nendo { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Required]
        [Column("todofuken_cd")]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Column("kumiaito_cd")]
        [StringLength(3)]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Column("shisho_cd")]
        [StringLength(2)]
        public string ShishoCd { get; set; }

        /// <summary>
        /// 取込ファイルパス
        /// </summary>
        [Column("torikomi_file_path")]
        [StringLength(256)]
        public string TorikomiFilePath { get; set; }

        /// <summary>
        /// 取込ファイル名
        /// </summary>
        [Column("torikomi_file_nm")]
        [StringLength(100)]
        public string TorikomiFileNm { get; set; }

        /// <summary>
        /// ハッシュ値
        /// </summary>
        [Column("hash")]
        [StringLength(64)]
        public string Hash { get; set; }

        /// <summary>
        /// 取込ステータス
        /// </summary>
        [Column("torikomi_sts")]
        [StringLength(2)]
        public string TorikomiSts { get; set; }

        /// <summary>
        /// 取込完了日時
        /// </summary>
        [Column("torikomi_date")]
        public DateTime? TorikomiDate { get; set; }

        /// <summary>
        /// ロック開始日時
        /// </summary>
        [Column("lock_date")]
        public DateTime? LockDate { get; set; }

        /// <summary>
        /// 反映完了日時
        /// </summary>
        [Column("hanei_date")]
        public DateTime? HaneiDate { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("delete_flg")]
        [StringLength(1)]
        public string DeleteFlg { get; set; }

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
