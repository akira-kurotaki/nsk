using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10010_組合等設定
    /// </summary>
    [Serializable]
    [Table("m_10010_組合等設定")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード))]
    public class M10010組合等設定 : ModelBase
    {
        /// <summary>
        /// 組合等コード
        /// </summary>
        [Required]
        [Column("組合等コード", Order = 1)]
        [StringLength(3)]
        public string 組合等コード { get; set; }

        /// <summary>
        /// 年産
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("年産", Order = 2)]
        public short 年産 { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 3)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 合併特例フラグ
        /// </summary>
        [Column("合併特例フラグ")]
        [StringLength(1)]
        public string 合併特例フラグ { get; set; }

        /// <summary>
        /// 損害開始割合特例フラグ
        /// </summary>
        [Column("損害開始割合特例フラグ")]
        [StringLength(1)]
        public string 損害開始割合特例フラグ { get; set; }

        /// <summary>
        /// 引受計算支所実行単位区分
        /// </summary>
        [Column("引受計算支所実行単位区分")]
        [StringLength(1)]
        public string 引受計算支所実行単位区分 { get; set; }

        /// <summary>
        /// 賦課金採用順位区分
        /// </summary>
        [Column("賦課金採用順位区分")]
        [StringLength(1)]
        public string 賦課金採用順位区分 { get; set; }

        /// <summary>
        /// 認定区分支払時期類分けフラグ
        /// </summary>
        [Column("認定区分支払時期類分けフラグ")]
        [StringLength(1)]
        public string 認定区分支払時期類分けフラグ { get; set; }

        /// <summary>
        /// 総合選別機使用有無フラグ
        /// </summary>
        [Column("総合選別機使用有無フラグ")]
        [StringLength(1)]
        public string 総合選別機使用有無フラグ { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("登録日時")]
        public DateTime? 登録日時 { get; set; }

        /// <summary>
        /// 登録ユーザid
        /// </summary>
        [Column("登録ユーザid")]
        [StringLength(11)]
        public string 登録ユーザid { get; set; }
    }
}
