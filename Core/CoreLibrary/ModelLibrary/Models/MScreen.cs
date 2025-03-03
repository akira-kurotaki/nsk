using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 画面マスタ
    /// </summary>
    [Serializable]
    [Table("m_screen")]
    public class MScreen : ModelBase
    {
        /// <summary>
        /// 画面ID
        /// </summary>
        [Required]
        [Key]
        [Column("screen_id", Order = 1)]
        [StringLength(9)]
        public string ScreenId { get; set; }

        /// <summary>
        /// 画面名
        /// </summary>
        [Required]
        [Column("screen_nm")]
        [StringLength(30)]
        public string ScreenNm { get; set; }

        /// <summary>
        /// エリアID
        /// </summary>
        [Column("area_id")]
        [StringLength(5)]
        public string AreaId { get; set; }

        /// <summary>
        /// 第1階層メニューグループ
        /// </summary>
        [Column("first_menu_group")]
        [StringLength(2)]
        public string FirstMenuGroup { get; set; }

        /// <summary>
        /// 第2階層メニュー表示フラグ
        /// </summary>
        [Column("second_menu_display_flg")]
        [StringLength(1)]
        public string SecondMenuDisplayFlg { get; set; }

        /// <summary>
        /// 第2階層メニュー表示順
        /// </summary>
        [Column("second_menu_display_order")]
        public short? SecondMenuDisplayOrder { get; set; }

        /// <summary>
        /// 第2階層メニュー表示画面名
        /// </summary>
        [Column("second_menu_display_nm")]
        [StringLength(30)]
        public string SecondMenuDisplayNm { get; set; }

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
