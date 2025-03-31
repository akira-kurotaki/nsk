using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 農業者関連取込履歴
    /// </summary>
    [Serializable]
    [Table("v_nogyosha_kanren_torikomi_rireki")]
    public class VNogyoshaKanrenTorikomiRireki : ModelBase
    {
        /// <summary>
        /// 取込履歴ID
        /// </summary>
        [Required]
        [Key]
        [Column("torikomi_rireki_id", Order = 1)]
        public long TorikomiRirekiId { get; set; }

        /// <summary>
        /// データ登録日時
        /// </summary>
        [Required]
        [Column("data_insert_date")]
        public DateTime DataInsertDate { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        [Required]
        [Column("todofuken_cd")]
        [StringLength(2)]
        public string TodofukenCd { get; set; }

        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("kumiaito_cd")]
        [StringLength(3)]
        public string KumiaitoCd { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        [Column("status")]
        [StringLength(2)]
        public string Status { get; set; }

        /// <summary>
        /// 農業者関連情報取込対象データ区分
        /// </summary>
        [Column("nogyosha_kanren_torikomi_kbn")]
        [StringLength(3)]
        public string NogyoshaKanrenTorikomiKbn { get; set; }

        /// <summary>
        /// 共済事業_農作物
        /// </summary>
        [Column("kyosai_jigyo_nsk_flg")]
        [StringLength(1)]
        public string KyosaiJigyoNskFlg { get; set; }

        /// <summary>
        /// 共済事業_畑作物
        /// </summary>
        [Column("kyosai_jigyo_hat_flg")]
        [StringLength(1)]
        public string KyosaiJigyoHatFlg { get; set; }

        /// <summary>
        /// 共済事業_果樹
        /// </summary>
        [Column("kyosai_jigyo_kju_flg")]
        [StringLength(1)]
        public string KyosaiJigyoKjuFlg { get; set; }

        /// <summary>
        /// 共済事業_家畜
        /// </summary>
        [Column("kyosai_jigyo_ktk_flg")]
        [StringLength(1)]
        public string KyosaiJigyoKtkFlg { get; set; }

        /// <summary>
        /// 共済事業_園芸施設
        /// </summary>
        [Column("kyosai_jigyo_eng_flg")]
        [StringLength(1)]
        public string KyosaiJigyoEngFlg { get; set; }

        /// <summary>
        /// 共済事業_任意
        /// </summary>
        [Column("kyosai_jigyo_nin_flg")]
        [StringLength(1)]
        public string KyosaiJigyoNinFlg { get; set; }

        /// <summary>
        /// 共済事業_その他
        /// </summary>
        [Column("kyosai_jigyo_sonota_flg")]
        [StringLength(1)]
        public string KyosaiJigyoSonotaFlg { get; set; }

        /// <summary>
        /// 農業者関連情報データ更新区分
        /// </summary>
        [Column("nogyosha_kanren_update_kbn")]
        [StringLength(1)]
        public string NogyoshaKanrenUpdateKbn { get; set; }

        /// <summary>
        /// 対象件数
        /// </summary>
        [Column("target_kensu")]
        public int? TargetKensu { get; set; }

        /// <summary>
        /// エラー件数
        /// </summary>
        [Column("error_kensu")]
        public int? ErrorKensu { get; set; }

        /// <summary>
        /// エラーリスト名
        /// </summary>
        [Column("error_list_nm")]
        [StringLength(100)]
        public string ErrorListNm { get; set; }

        /// <summary>
        /// エラーリストパス
        /// </summary>
        [Column("error_list_path")]
        public string ErrorListPath { get; set; }

        /// <summary>
        /// エラーリストハッシュ値
        /// </summary>
        [Column("error_list_hash")]
        [StringLength(64)]
        public string ErrorListHash { get; set; }

        /// <summary>
        /// 取込ファイル_変更前ファイル名
        /// </summary>
        [Column("original_file_nm")]
        [StringLength(100)]
        public string OriginalFileNm { get; set; }

        /// <summary>
        /// 取込ファイル_変更後ファイル名
        /// </summary>
        [Column("changed_file_nm")]
        [StringLength(100)]
        public string ChangedFileNm { get; set; }

        /// <summary>
        /// 取込ファイルハッシュ値
        /// </summary>
        [Column("original_file_hash")]
        [StringLength(64)]
        public string OriginalFileHash { get; set; }

        /// <summary>
        /// 開始日時
        /// </summary>
        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 終了日時
        /// </summary>
        [Column("end_date")]
        public DateTime? EndDate { get; set; }

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
