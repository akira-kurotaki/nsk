using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// ログイン時遷移先マスタ
    /// </summary>
    [Serializable]
    [Table("m_login_transition")]
    public class MLoginTransition : ModelBase
    {
        /// <summary>
        /// 遷移先番号
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("transition_no", Order = 1)]
        public int TransitionNo { get; set; }

        /// <summary>
        /// キーワード
        /// </summary>
        [Required]
        [Column("keyword")]
        public string Keyword { get; set; }

        /// <summary>
        /// デフォルト遷移先URL
        /// </summary>
        [Required]
        [Column("default_url")]
        public string DefaultUrl { get; set; }

        /// <summary>
        /// 画面表示モードPC
        /// </summary>
        [Required]
        [Column("screen_pc")]
        public string ScreenPc { get; set; }

        /// <summary>
        /// 画面表示モードTablet
        /// </summary>
        [Required]
        [Column("screen_tablet")]
        public string ScreenTablet { get; set; }
    }
}
