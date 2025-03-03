using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// ヘルプメニューマスタ
    /// </summary>
    [Serializable]
    [Table("m_help_menu")]
    public class MHelpMenu : ModelBase
    {
        /// <summary>
        /// ヘルプメニュー表示順
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("help_menu_display_order", Order = 1)]
        public short HelpMenuDisplayOrder { get; set; }

        /// <summary>
        /// ヘルプメニュー表示フラグ
        /// </summary>
        [Column("help_menu_display_flg")]
        [StringLength(1)]
        public string HelpMenuDisplayFlg { get; set; }

        /// <summary>
        /// ヘルプメニュー表示機能名
        /// </summary>
        [Column("help_menu_display_kino_nm")]
        [StringLength(30)]
        public string HelpMenuDisplayKinoNm { get; set; }

        /// <summary>
        /// ヘルプファイルパス
        /// </summary>
        [Required]
        [Column("help_file_path")]
        public string HelpFilePath { get; set; }

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
