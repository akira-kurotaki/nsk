using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// m_10290_共通申請等割引方法
    /// </summary>
    [Serializable]
    [Table("m_10290_共通申請等割引方法")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(共通申請等割引方法コード))]
    public class M10290共通申請等割引方法 : ModelBase
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
        /// 共通申請等割引方法コード
        /// </summary>
        [Required]
        [Column("共通申請等割引方法コード", Order = 4)]
        [StringLength(2)]
        public string 共通申請等割引方法コード { get; set; }

        /// <summary>
        /// 個別設定フラグ
        /// </summary>
        [Column("個別設定フラグ")]
        [StringLength(1)]
        public string 個別設定フラグ { get; set; }

        /// <summary>
        /// 共通申請等割引方法名称
        /// </summary>
        [Column("共通申請等割引方法名称")]
        [StringLength(30)]
        public string 共通申請等割引方法名称 { get; set; }

        /// <summary>
        /// 割引割合
        /// </summary>
        [Column("割引割合")]
        public Decimal? 割引割合 { get; set; }

        /// <summary>
        /// 割引金額
        /// </summary>
        [Column("割引金額")]
        public Decimal? 割引金額 { get; set; }

        /// <summary>
        /// 割増割合
        /// </summary>
        [Column("割増割合")]
        public Decimal? 割増割合 { get; set; }

        /// <summary>
        /// 割増金額
        /// </summary>
        [Column("割増金額")]
        public Decimal? 割増金額 { get; set; }

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

        /// <summary>
        /// 更新日時
        /// </summary>
        [Column("更新日時")]
        public DateTime? 更新日時 { get; set; }

        /// <summary>
        /// 更新ユーザid
        /// </summary>
        [Column("更新ユーザid")]
        [StringLength(11)]
        public string 更新ユーザid { get; set; }
    }
}
