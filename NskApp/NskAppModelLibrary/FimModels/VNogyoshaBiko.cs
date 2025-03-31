using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 農業者備考
    /// </summary>
    [Serializable]
    [Table("v_nogyosha_biko")]
    public class VNogyoshaBiko : ModelBase
    {
        /// <summary>
        /// 農業者ID (FK)
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("nogyosha_id", Order = 1)]
        public int NogyoshaId { get; set; }

        /// <summary>
        /// 組合等_抽出集計コード1
        /// </summary>
        [Column("kumiaito_chushutsu_shukei_cd1")]
        [StringLength(13)]
        public string KumiaitoChushutsuShukeiCd1 { get; set; }

        /// <summary>
        /// 組合等_抽出集計コード2
        /// </summary>
        [Column("kumiaito_chushutsu_shukei_cd2")]
        [StringLength(13)]
        public string KumiaitoChushutsuShukeiCd2 { get; set; }

        /// <summary>
        /// 組合等_抽出集計コード3
        /// </summary>
        [Column("kumiaito_chushutsu_shukei_cd3")]
        [StringLength(13)]
        public string KumiaitoChushutsuShukeiCd3 { get; set; }

        /// <summary>
        /// 組合等_抽出集計コード4
        /// </summary>
        [Column("kumiaito_chushutsu_shukei_cd4")]
        [StringLength(13)]
        public string KumiaitoChushutsuShukeiCd4 { get; set; }

        /// <summary>
        /// 組合等_抽出集計コード5
        /// </summary>
        [Column("kumiaito_chushutsu_shukei_cd5")]
        [StringLength(13)]
        public string KumiaitoChushutsuShukeiCd5 { get; set; }

        /// <summary>
        /// 組合等_抽出集計コード6
        /// </summary>
        [Column("kumiaito_chushutsu_shukei_cd6")]
        [StringLength(13)]
        public string KumiaitoChushutsuShukeiCd6 { get; set; }

        /// <summary>
        /// 組合等_集計コード1
        /// </summary>
        [Column("kumiaito_shukei_cd1")]
        [StringLength(10)]
        public string KumiaitoShukeiCd1 { get; set; }

        /// <summary>
        /// 組合等_集計コード2
        /// </summary>
        [Column("kumiaito_shukei_cd2")]
        [StringLength(10)]
        public string KumiaitoShukeiCd2 { get; set; }

        /// <summary>
        /// 組合等_集計コード3
        /// </summary>
        [Column("kumiaito_shukei_cd3")]
        [StringLength(10)]
        public string KumiaitoShukeiCd3 { get; set; }

        /// <summary>
        /// 組合等_集計コード4
        /// </summary>
        [Column("kumiaito_shukei_cd4")]
        [StringLength(10)]
        public string KumiaitoShukeiCd4 { get; set; }

        /// <summary>
        /// 組合等_集計コード5
        /// </summary>
        [Column("kumiaito_shukei_cd5")]
        [StringLength(10)]
        public string KumiaitoShukeiCd5 { get; set; }

        /// <summary>
        /// 組合等_集計コード6
        /// </summary>
        [Column("kumiaito_shukei_cd6")]
        [StringLength(10)]
        public string KumiaitoShukeiCd6 { get; set; }

        /// <summary>
        /// 組合等_その1
        /// </summary>
        [Column("kumiaito_no1")]
        [StringLength(300)]
        public string KumiaitoNo1 { get; set; }

        /// <summary>
        /// 組合等_その2
        /// </summary>
        [Column("kumiaito_no2")]
        [StringLength(300)]
        public string KumiaitoNo2 { get; set; }

        /// <summary>
        /// 全国共通_その1
        /// </summary>
        [Column("zenkokukyotsu_no1")]
        [StringLength(13)]
        public string ZenkokukyotsuNo1 { get; set; }

        /// <summary>
        /// 全国共通_その2
        /// </summary>
        [Column("zenkokukyotsu_no2")]
        [StringLength(13)]
        public string ZenkokukyotsuNo2 { get; set; }

        /// <summary>
        /// 全国共通_その3
        /// </summary>
        [Column("zenkokukyotsu_no3")]
        [StringLength(13)]
        public string ZenkokukyotsuNo3 { get; set; }

        /// <summary>
        /// 全国共通_その4
        /// </summary>
        [Column("zenkokukyotsu_no4")]
        [StringLength(300)]
        public string ZenkokukyotsuNo4 { get; set; }

        /// <summary>
        /// 全国共通_その5
        /// </summary>
        [Column("zenkokukyotsu_no5")]
        [StringLength(300)]
        public string ZenkokukyotsuNo5 { get; set; }

        /// <summary>
        /// 備考1
        /// </summary>
        [Column("biko1")]
        [StringLength(300)]
        public string Biko1 { get; set; }

        /// <summary>
        /// 備考2
        /// </summary>
        [Column("biko2")]
        [StringLength(300)]
        public string Biko2 { get; set; }

        /// <summary>
        /// 備考3
        /// </summary>
        [Column("biko3")]
        [StringLength(300)]
        public string Biko3 { get; set; }

        /// <summary>
        /// 備考4
        /// </summary>
        [Column("biko4")]
        [StringLength(300)]
        public string Biko4 { get; set; }

        /// <summary>
        /// 備考5
        /// </summary>
        [Column("biko5")]
        [StringLength(300)]
        public string Biko5 { get; set; }

        /// <summary>
        /// 備考6
        /// </summary>
        [Column("biko6")]
        [StringLength(300)]
        public string Biko6 { get; set; }

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
