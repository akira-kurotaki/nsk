using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 画面機能権限マスタ
    /// </summary>
    [Serializable]
    [Table("m_screen_kino_kengen")]
    [PrimaryKey(nameof(ScreenSosaKengenId), nameof(ScreenKinoCd))]
    public class MScreenKinoKengen : ModelBase
    {
        /// <summary>
        /// 画面操作権限ID (FK)
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("screen_sosa_kengen_id", Order = 1)]
        public int ScreenSosaKengenId { get; set; }

        /// <summary>
        /// 画面機能コード
        /// </summary>
        [Required]
        [Column("screen_kino_cd", Order = 2)]
        [StringLength(20)]
        public string ScreenKinoCd { get; set; }

        /// <summary>
        /// 参照不可フラグ
        /// </summary>
        [Column("no_refer_flg")]
        [StringLength(1)]
        public string NoReferFlg { get; set; }

        /// <summary>
        /// 更新不可フラグ
        /// </summary>
        [Column("no_update_flg")]
        [StringLength(1)]
        public string NoUpdateFlg { get; set; }

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
