using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 組合等金融機関マスタ
    /// </summary>
    [Serializable]
    [Table("v_kumiaito_kinyukikan")]
    [PrimaryKey(nameof(TodofukenCd), nameof(KumiaitoCd), nameof(KanriNo))]
    public class VKumiaitoKinyukikan : ModelBase
    {
        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Required]
        [Column("todofuken_cd", Order = 1)]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("kumiaito_cd", Order = 2)]
        [StringLength(3)]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// 管理ＮＯ
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("kanri_no", Order = 3)]
        public short KanriNo { get; set; }

        /// <summary>
        /// 金融機関コード
        /// </summary>
        [Column("kinyukikan_cd")]
        [StringLength(7)]
        public string KinyukikanCd { get; set; }

        /// <summary>
        /// 金融機関名
        /// </summary>
        [Column("kinyukikan_nm")]
        public string KinyukikanNm { get; set; }

        /// <summary>
        /// 預金種別
        /// </summary>
        [Column("yokin_sbt")]
        [StringLength(1)]
        public string YokinSbt { get; set; }

        /// <summary>
        /// 口座番号
        /// </summary>
        [Column("koza_num")]
        [StringLength(10)]
        public string KozaNum { get; set; }

        /// <summary>
        /// 口座名義人
        /// </summary>
        [Column("kozameigi_nm")]
        [StringLength(30)]
        public string KozameigiNm { get; set; }

        /// <summary>
        /// 口座名義人カナ
        /// </summary>
        [Column("kozameigi_kana")]
        [StringLength(30)]
        public string KozameigiKana { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("biko")]
        [StringLength(300)]
        public string Biko { get; set; }

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
