using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelLibrary.Models
{
    /// <summary>
    /// 農畜品目マスタ
    /// </summary>
    [Serializable]
    [Table("m_nc_himmoku")]
    [PrimaryKey(nameof(NcShuruiCd), nameof(NcHimmokuCd))]
    public class MNcHimmoku : ModelBase
    {
        /// <summary>
        /// 農畜種類コード (FK)
        /// </summary>
        [Required]
        //[Key]
        [Column("nc_shurui_cd", Order = 1)]
        [StringLength(2)]
        public string NcShuruiCd { get; set; }

        /// <summary>
        /// 農畜品目コード
        /// </summary>
        [Required]
        //[Key]
        [Column("nc_himmoku_cd", Order = 2)]
        [StringLength(3)]
        public string NcHimmokuCd { get; set; }

        /// <summary>
        /// 農畜品目名
        /// </summary>
        [Column("nc_himmoku_nm")]
        [StringLength(18)]
        public string NcHimmokuNm { get; set; }

        /// <summary>
        /// 過去申告書様式フラグ
        /// </summary>
        [Required]
        [Column("kakoshinkoku_yoshiki_flg")]
        [StringLength(1)]
        public string KakoshinkokuYoshikiFlg { get; set; }

        /// <summary>
        /// 補助フォーム様式フラグ
        /// </summary>
        [Required]
        [Column("hojoform_yoshiki_flg")]
        [StringLength(1)]
        public string HojoformYoshikiFlg { get; set; }

        /// <summary>
        /// 営農計画様式フラグ
        /// </summary>
        [Required]
        [Column("einokeikaku_yoshiki_flg")]
        [StringLength(1)]
        public string EinokeikakuYoshikiFlg { get; set; }

        /// <summary>
        /// 収入試算様式フラグ
        /// </summary>
        [Required]
        [Column("shunyushisan_yoshiki_flg")]
        [StringLength(1)]
        public string ShunyushisanYoshikiFlg { get; set; }

        /// <summary>
        /// 経営目標様式フラグ
        /// </summary>
        [Required]
        [Column("keieimokuhyo_yoshiki_flg")]
        [StringLength(1)]
        public string KeieimokuhyoYoshikiFlg { get; set; }

        /// <summary>
        /// 過去申告書用途必須フラグ
        /// </summary>
        [Required]
        [Column("kakoshinkoku_yoto_required_flg")]
        [StringLength(1)]
        public string KakoshinkokuYotoRequiredFlg { get; set; }

        /// <summary>
        /// 補助フォーム用途必須フラグ
        /// </summary>
        [Required]
        [Column("hojoform_yoto_required_flg")]
        [StringLength(1)]
        public string HojoformYotoRequiredFlg { get; set; }

        /// <summary>
        /// 営農計画用途必須フラグ
        /// </summary>
        [Required]
        [Column("einokeikaku_yoto_required_flg")]
        [StringLength(1)]
        public string EinokeikakuYotoRequiredFlg { get; set; }

        /// <summary>
        /// 収入試算用途必須フラグ
        /// </summary>
        [Required]
        [Column("shunyushisan_yoto_required_flg")]
        [StringLength(1)]
        public string ShunyushisanYotoRequiredFlg { get; set; }

        /// <summary>
        /// 経営目標用途必須フラグ
        /// </summary>
        [Required]
        [Column("keieimokuhyo_yoto_required_flg")]
        [StringLength(1)]
        public string KeieimokuhyoYotoRequiredFlg { get; set; }

        /// <summary>
        /// 規模拡大除外フラグ
        /// </summary>
        [Required]
        [Column("kibokakudai_jyogai_flg")]
        [StringLength(1)]
        public string KibokakudaiJyogaiFlg { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Required]
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
