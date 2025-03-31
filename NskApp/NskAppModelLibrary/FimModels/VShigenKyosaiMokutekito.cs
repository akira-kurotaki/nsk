using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 資源用共済目的等マスタ
    /// </summary>
    [Serializable]
    [Table("v_shigen_kyosai_mokutekito")]
    [PrimaryKey(nameof(KyosaiJigyoCd), nameof(KyosaiMokutekitoCd))]
    public class VShigenKyosaiMokutekito : ModelBase
    {
        /// <summary>
        /// 共済事業コード
        /// </summary>
        [Required]
        [Column("kyosai_jigyo_cd", Order = 1)]
        [StringLength(2)]
        public string KyosaiJigyoCd { get; set; }

        /// <summary>
        /// 共済目的等コード
        /// </summary>
        [Required]
        [Column("kyosai_mokutekito_cd", Order = 2)]
        [StringLength(2)]
        public string KyosaiMokutekitoCd { get; set; }

        /// <summary>
        /// 共済目的等名称
        /// </summary>
        [Column("kyosai_mokutekito_nm")]
        [StringLength(30)]
        public string KyosaiMokutekitoNm { get; set; }

        /// <summary>
        /// システム区分
        /// </summary>
        [Column("system_kbn")]
        [StringLength(2)]
        public string SystemKbn { get; set; }

        /// <summary>
        /// 共済目的区分
        /// </summary>
        [Column("kyosai_mokutekito_kbn")]
        [StringLength(3)]
        public string KyosaiMokutekitoKbn { get; set; }

        /// <summary>
        /// 資源量表示パターン
        /// </summary>
        [Column("shigen_hyoji_pattern")]
        [StringLength(3)]
        public string ShigenHyojiPattern { get; set; }

        /// <summary>
        /// 共済目的等表示順
        /// </summary>
        [Required]
        [Column("kyosai_mokutekito_display_order")]
        public short KyosaiMokutekitoDisplayOrder { get; set; }

        /// <summary>
        /// 共済目的等表示フラグ
        /// </summary>
        [Column("kyosai_mokutekito_display_flg")]
        [StringLength(1)]
        public string KyosaiMokutekitoDisplayFlg { get; set; }

        /// <summary>
        /// 共済目的等改行フラグ
        /// </summary>
        [Column("kyosai_mokutekito_kaigyo_flg")]
        [StringLength(1)]
        public string KyosaiMokutekitoKaigyoFlg { get; set; }

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
