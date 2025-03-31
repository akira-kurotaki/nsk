using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_11010_個人設定
    /// </summary>
    [Serializable]
    [Table("t_11010_個人設定")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(組合員等コード))]
    public class T11010個人設定 : ModelBase
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
        /// 組合員等コード
        /// </summary>
        [Required]
        [Column("組合員等コード", Order = 4)]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 加入形態
        /// </summary>
        [Column("加入形態")]
        [StringLength(2)]
        public string 加入形態 { get; set; }

        /// <summary>
        /// 交付申請者管理コード
        /// </summary>
        [Column("交付申請者管理コード")]
        [StringLength(18)]
        public string 交付申請者管理コード { get; set; }

        /// <summary>
        /// 共済金額改定時コード
        /// </summary>
        [Column("共済金額改定時コード")]
        [StringLength(1)]
        public string 共済金額改定時コード { get; set; }

        /// <summary>
        /// 未加入フラグ
        /// </summary>
        [Column("未加入フラグ")]
        [StringLength(1)]
        public string 未加入フラグ { get; set; }

        /// <summary>
        /// 継続特約フラグ
        /// </summary>
        [Column("継続特約フラグ")]
        [StringLength(1)]
        public string 継続特約フラグ { get; set; }

        /// <summary>
        /// 地域集団コード
        /// </summary>
        [Column("地域集団コード")]
        [StringLength(13)]
        public string 地域集団コード { get; set; }

        /// <summary>
        /// 共通申請等割引方法コード
        /// </summary>
        [Column("共通申請等割引方法コード")]
        [StringLength(2)]
        public string 共通申請等割引方法コード { get; set; }

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
        /// 共通申請等割引理由
        /// </summary>
        [Column("共通申請等割引理由")]
        public string 共通申請等割引理由 { get; set; }

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
