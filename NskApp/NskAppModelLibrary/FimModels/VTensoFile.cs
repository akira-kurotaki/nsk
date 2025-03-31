using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.FimModels
{
    /// <summary>
    /// 転送ファイルマスタ
    /// </summary>
    [Serializable]
    [Table("v_tenso_file")]
    [PrimaryKey(nameof(KyosaiJigyoCd), nameof(TensoCd), nameof(FileNo), nameof(TensoShosaiCd))]
    public class VTensoFile : ModelBase
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
        /// ファイル番号
        /// </summary>
        [Required]
        [Column("file_no", Order = 3)]
        [StringLength(2)]
        public string FileNo { get; set; }

        /// <summary>
        /// 転送詳細部類コード
        /// </summary>
        [Required]
        [Column("tenso_shosai_cd", Order = 4)]
        [StringLength(4)]
        public string TensoShosaiCd { get; set; }

        /// <summary>
        /// 転送データ種別名称
        /// </summary>
        [Column("tenso_data_shubetsu_nm")]
        [StringLength(50)]
        public string TensoDataShubetsuNm { get; set; }

        /// <summary>
        /// 転送元ファイル
        /// </summary>
        [Column("tensomoto_file")]
        public string TensomotoFile { get; set; }

        /// <summary>
        /// 転送先ファイル
        /// </summary>
        [Column("tensosaki_file")]
        public string TensosakiFile { get; set; }

        /// <summary>
        /// 使用フラグ
        /// </summary>
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
