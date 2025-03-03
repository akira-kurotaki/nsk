using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// メニューマスタ
    /// </summary>
    [Serializable]
    [Table("m_menu")]
    public class MMenu : ModelBase
    {
        /// <summary>
        /// 第1階層メニューグループ
        /// </summary>
        [Required]
        [Key]
        [Column("first_menu_group", Order = 1)]
        [StringLength(2)]
        public string FirstMenuGroup { get; set; }

        /// <summary>
        /// 第1階層メニュー表示フラグ
        /// </summary>
        [Column("first_menu_display_flg")]
        [StringLength(1)]
        public string FirstMenuDisplayFlg { get; set; }

        /// <summary>
        /// 第1階層メニュー表示順
        /// </summary>
        [Column("first_menu_display_order")]
        public short? FirstMenuDisplayOrder { get; set; }

        /// <summary>
        /// 第1階層メニュー表示機能名
        /// </summary>
        [Column("first_menu_display_kino_nm")]
        [StringLength(15)]
        public string FirstMenuDisplayKinoNm { get; set; }

        /// <summary>
        /// 表示区分
        /// </summary>
        [Column("hyoji_kbn")]
        [StringLength(11)]
        public string HyojiKbn { get; set; }

        /// <summary>
        /// ユーザ管理権限
        /// </summary>
        [Column("user_kanri_kengen")]
        [StringLength(1)]
        public string UserKanriKengen { get; set; }

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
