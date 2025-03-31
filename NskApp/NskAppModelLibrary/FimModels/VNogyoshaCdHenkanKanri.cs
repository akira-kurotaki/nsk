using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 農業者管理コード等変換管理
    /// </summary>
    [Serializable]
    [Table("v_nogyosha_cd_henkan_kanri")]
    [PrimaryKey(nameof(YoyakuId), nameof(NogyoshaId))]
    public class VNogyoshaCdHenkanKanri : ModelBase
    {
        /// <summary>
        /// 予約ID
        /// </summary>
        [Required]
        [Column("yoyaku_id", Order = 1)]
        public long YoyakuId { get; set; }

        /// <summary>
        /// 農業者ID (FK)
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nogyosha_id", Order = 2)]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// 変換前農業者管理コード
        /// </summary>
        [Column("before_nogyosha_cd")]
        [StringLength(13)]
        public string BeforeNogyoshaCd { get; set; }

        /// <summary>
        /// 変換後農業者管理コード
        /// </summary>
        [Column("after_nogyosha_cd")]
        [StringLength(13)]
        public string AfterNogyoshaCd { get; set; }

        /// <summary>
        /// 共済事業コード
        /// </summary>
        [Column("kyosai_jigyo_cd")]
        [StringLength(2)]
        public string KyosaiJigyoCd { get; set; }

        /// <summary>
        /// 引受共済目的等コード
        /// </summary>
        [Column("hikiuke_kyosai_mokutekito_cd")]
        [StringLength(15)]
        public string HikiukeKyosaiMokutekitoCd { get; set; }

        /// <summary>
        /// 引受共済目的等名
        /// </summary>
        [Column("hikiuke_kyosai_mokutekito_nm")]
        [StringLength(20)]
        public string HikiukeKyosaiMokutekitoNm { get; set; }

        /// <summary>
        /// データ分類
        /// </summary>
        [Column("data_bunrui")]
        [StringLength(1)]
        public string DataBunrui { get; set; }

        /// <summary>
        /// 指定年産／年度
        /// </summary>
        [Column("shitei_nensan_nendo")]
        public short? ShiteiNensanNendo { get; set; }

        /// <summary>
        /// 開始日時
        /// </summary>
        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 終了日時
        /// </summary>
        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 変換ステータス
        /// </summary>
        [Column("henkan_status")]
        [StringLength(1)]
        public string HenkanStatus { get; set; }

        /// <summary>
        /// 最小変換年産／年度
        /// </summary>
        [Column("min_henkan_nensan_nendo")]
        public short? MinHenkanNensanNendo { get; set; }

        /// <summary>
        /// 最大変換年産／年度
        /// </summary>
        [Column("max_henkan_nensan_nendo")]
        public short? MaxHenkanNensanNendo { get; set; }

        /// <summary>
        /// エラー情報
        /// </summary>
        [Column("error_info")]
        public string ErrorInfo { get; set; }

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
