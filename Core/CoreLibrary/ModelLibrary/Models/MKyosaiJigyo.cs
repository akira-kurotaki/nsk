using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 名称M共済事業
    /// </summary>
    [Serializable]
    [Table("m_kyosai_jigyo")]
    public class MKyosaiJigyo : ModelBase
    {
        /// <summary>
        /// 共済事業コード
        /// </summary>
        [Required]
        [Key]
        [Column("kyosai_jigyo_cd", Order = 1)]
        [StringLength(2)]
        public string KyosaiJigyoCd { get; set; }

        /// <summary>
        /// 共済事業名称
        /// </summary>
        [Column("kyosai_jigyo_nm")]
        [StringLength(30)]
        public string KyosaiJigyoNm { get; set; }

        /// <summary>
        /// 共済事業表示順
        /// </summary>
        [Required]
        [Column("kyosai_jigyo_display_order")]
        public short KyosaiJigyoDisplayOrder { get; set; }

        /// <summary>
        /// 共済事業表示フラグ
        /// </summary>
        [Column("kyosai_jigyo_display_flg")]
        [StringLength(1)]
        public string KyosaiJigyoDisplayFlg { get; set; }

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
