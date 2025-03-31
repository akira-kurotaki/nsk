using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 転送区分マスタ
    /// </summary>
    [Serializable]
    [Table("v_tenso_file_kbn")]
    [PrimaryKey(nameof(KyosaiJigyoCd), nameof(TensoCd))]
    public class VTensoFileKbn : ModelBase
    {
        /// <summary>
        /// 共済事業コード
        /// </summary>
        [Required]
        [Column("kyosai_jigyo_cd", Order = 1)]
        [StringLength(2)]
        public string KyosaiJigyoCd { get; set; }

        /// <summary>
        /// 転送区分コード
        /// </summary>
        [Required]
        [Column("tenso_cd", Order = 2)]
        [StringLength(3)]
        public string TensoCd { get; set; }

        /// <summary>
        /// 転送区分名
        /// </summary>
        [Column("tenso_nm")]
        [StringLength(30)]
        public string TensoNm { get; set; }

        /// <summary>
        /// 転送元データ格納場所
        /// </summary>
        [Column("tensomoto_data_path")]
        public string TensomotoDataPath { get; set; }

        /// <summary>
        /// 転送先データ格納場所
        /// </summary>
        [Column("tensosaki_data_path")]
        public string TensosakiDataPath { get; set; }

        /// <summary>
        /// 転送元データ退避場所
        /// </summary>
        [Column("taihi_data_path")]
        public string TaihiDataPath { get; set; }

        /// <summary>
        /// データ転送コード
        /// </summary>
        [Column("data_tenso_cd")]
        [StringLength(2)]
        public string DataTensoCd { get; set; }

        /// <summary>
        /// 出力状況
        /// </summary>
        [Column("output_sts")]
        [StringLength(1)]
        public string OutputSts { get; set; }

        /// <summary>
        /// 使用フラグ
        /// </summary>
        [Required]
        [Column("shiyo_flg")]
        [StringLength(1)]
        public string ShiyoFlg { get; set; }

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
