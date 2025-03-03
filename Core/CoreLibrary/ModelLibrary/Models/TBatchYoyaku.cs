using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// バッチ予約
    /// </summary>
    [Serializable]
    [Table("t_batch_yoyaku")]
    public class TBatchYoyaku : ModelBase
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
        /// バッチ分類
        /// </summary>
        [Required]
        [Column("batch_bunrui")]
        [StringLength(2)]
        public string BatchBunrui { get; set; }

        /// <summary>
        /// システム区分
        /// </summary>
        [Required]
        [Column("system_kbn")]
        [StringLength(2)]
        public string SystemKbn { get; set; }

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
        /// 予約日時
        /// </summary>
        [Column("batch_yoyaku_date")]
        public DateTime? BatchYoyakuDate { get; set; }

        /// <summary>
        /// 予約ユーザID
        /// </summary>
        [Column("batch_yoyaku_id")]
        [StringLength(11)]
        public string BatchYoyakuId { get; set; }

        /// <summary>
        /// 予約を実行した処理名
        /// </summary>
        [Column("batch_yoyaku_shori")]
        [StringLength(30)]
        public string BatchYoyakuShori { get; set; }

        /// <summary>
        /// バッチ名
        /// </summary>
        [Column("batch_nm")]
        [StringLength(30)]
        public string BatchNm { get; set; }

        /// <summary>
        /// バッチ条件
        /// </summary>
        [Column("batch_joken")]
        [StringLength(100)]
        public string BatchJoken { get; set; }

        /// <summary>
        /// バッチ条件（画面表示用）
        /// </summary>
        [Column("batch_joken_disp")]
        public string BatchJokenDisp { get; set; }

        /// <summary>
        /// 複数実行禁止フラグ
        /// </summary>
        [Column("multi_run_ng_flg")]
        [StringLength(1)]
        public string MultiRunNgFlg { get; set; }

        /// <summary>
        /// 複数実行禁止ID
        /// </summary>
        [Column("multi_run_ng_id")]
        [StringLength(100)]
        public string MultiRunNgId { get; set; }

        /// <summary>
        /// バッチ種類
        /// </summary>
        [Required]
        [Column("batch_type")]
        [StringLength(1)]
        public string BatchType { get; set; }

        /// <summary>
        /// 定時実行バッチ予約日時
        /// </summary>
        [Column("batch_sche_datetime")]
        public DateTime? BatchScheDatetime { get; set; }

        /// <summary>
        /// ロック対象フラグ
        /// </summary>
        [Column("lock_target_flg")]
        [StringLength(1)]
        public string LockTargetFlg { get; set; }

        /// <summary>
        /// バッチステータス
        /// </summary>
        [Column("batch_status")]
        [StringLength(2)]
        public string BatchStatus { get; set; }

        /// <summary>
        /// バッチ開始日時
        /// </summary>
        [Column("batch_start_date")]
        public DateTime? BatchStartDate { get; set; }

        /// <summary>
        /// バッチ完了日時
        /// </summary>
        [Column("batch_end_date")]
        public DateTime? BatchEndDate { get; set; }

        /// <summary>
        /// 実行サーバ
        /// </summary>
        [Column("batch_run_server")]
        [StringLength(30)]
        public string BatchRunServer { get; set; }

        /// <summary>
        /// エラー情報
        /// </summary>
        [Column("error_info")]
        [StringLength(500)]
        public string ErrorInfo { get; set; }

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
