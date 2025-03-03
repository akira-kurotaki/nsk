using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// v_30020_口座振替結果
    /// </summary>
    [Serializable]
    [Table("v_30020_口座振替結果")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済事業コード), nameof(共済目的コード), nameof(金額区分), nameof(口座振替id))]
    public class V30020口座振替結果 : ModelBase
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
        /// 共済事業コード
        /// </summary>
        [Required]
        [Column("共済事業コード", Order = 3)]
        [StringLength(2)]
        public string 共済事業コード { get; set; }

        /// <summary>
        /// 共済目的コード
        /// </summary>
        [Required]
        [Column("共済目的コード", Order = 4)]
        [StringLength(2)]
        public string 共済目的コード { get; set; }

        /// <summary>
        /// 金額区分
        /// </summary>
        [Required]
        [Column("金額区分", Order = 5)]
        [StringLength(1)]
        public string 金額区分 { get; set; }

        /// <summary>
        /// 口座振替id
        /// </summary>
        [Required]
        [Column("口座振替id", Order = 6)]
        [StringLength(7)]
        public string 口座振替id { get; set; }

        /// <summary>
        /// 引受回
        /// </summary>
        [Column("引受回")]
        public short? 引受回 { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Column("組合員等コード")]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 組合員等氏名
        /// </summary>
        [Column("組合員等氏名")]
        [StringLength(30)]
        public string 組合員等氏名 { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        [Column("住所")]
        [StringLength(40)]
        public string 住所 { get; set; }

        /// <summary>
        /// データ種別
        /// </summary>
        [Column("データ種別")]
        [StringLength(2)]
        public string データ種別 { get; set; }

        /// <summary>
        /// 被仕向銀行番号
        /// </summary>
        [Column("被仕向銀行番号")]
        [StringLength(4)]
        public string 被仕向銀行番号 { get; set; }

        /// <summary>
        /// 被仕向銀行名
        /// </summary>
        [Column("被仕向銀行名")]
        [StringLength(15)]
        public string 被仕向銀行名 { get; set; }

        /// <summary>
        /// 被仕向支店番号
        /// </summary>
        [Column("被仕向支店番号")]
        [StringLength(3)]
        public string 被仕向支店番号 { get; set; }

        /// <summary>
        /// 被仕向支店名
        /// </summary>
        [Column("被仕向支店名")]
        [StringLength(15)]
        public string 被仕向支店名 { get; set; }

        /// <summary>
        /// 手形交換所番号
        /// </summary>
        [Column("手形交換所番号")]
        [StringLength(4)]
        public string 手形交換所番号 { get; set; }

        /// <summary>
        /// 預金種目
        /// </summary>
        [Column("預金種目")]
        [StringLength(1)]
        public string 預金種目 { get; set; }

        /// <summary>
        /// 口座番号
        /// </summary>
        [Column("口座番号")]
        [StringLength(7)]
        public string 口座番号 { get; set; }

        /// <summary>
        /// 受取人名
        /// </summary>
        [Column("受取人名")]
        [StringLength(30)]
        public string 受取人名 { get; set; }

        /// <summary>
        /// 振込金額
        /// </summary>
        [Column("振込金額")]
        public Decimal? 振込金額 { get; set; }

        /// <summary>
        /// 振替指定区分
        /// </summary>
        [Column("振替指定区分")]
        [StringLength(1)]
        public string 振替指定区分 { get; set; }

        /// <summary>
        /// 振替日
        /// </summary>
        [Column("振替日")]
        public DateTime? 振替日 { get; set; }

        /// <summary>
        /// 取引金融機関コード
        /// </summary>
        [Column("取引金融機関コード")]
        [StringLength(4)]
        public string 取引金融機関コード { get; set; }

        /// <summary>
        /// 取引金融機関名
        /// </summary>
        [Column("取引金融機関名")]
        [StringLength(15)]
        public string 取引金融機関名 { get; set; }

        /// <summary>
        /// 取引金融機関支店コード
        /// </summary>
        [Column("取引金融機関支店コード")]
        [StringLength(3)]
        public string 取引金融機関支店コード { get; set; }

        /// <summary>
        /// 取引金融機関支店名
        /// </summary>
        [Column("取引金融機関支店名")]
        [StringLength(15)]
        public string 取引金融機関支店名 { get; set; }

        /// <summary>
        /// 委託者コード
        /// </summary>
        [Column("委託者コード")]
        [StringLength(10)]
        public string 委託者コード { get; set; }

        /// <summary>
        /// 委託者名
        /// </summary>
        [Column("委託者名")]
        [StringLength(30)]
        public string 委託者名 { get; set; }

        /// <summary>
        /// 組合等預金種別
        /// </summary>
        [Column("組合等預金種別")]
        [StringLength(1)]
        public string 組合等預金種別 { get; set; }

        /// <summary>
        /// 組合等口座番号
        /// </summary>
        [Column("組合等口座番号")]
        [StringLength(7)]
        public string 組合等口座番号 { get; set; }

        /// <summary>
        /// 新規コード
        /// </summary>
        [Column("新規コード")]
        [StringLength(1)]
        public string 新規コード { get; set; }

        /// <summary>
        /// 振替結果コード
        /// </summary>
        [Column("振替結果コード")]
        [StringLength(10)]
        public string 振替結果コード { get; set; }

        /// <summary>
        /// 判別コード
        /// </summary>
        [Column("判別コード")]
        [StringLength(1)]
        public string 判別コード { get; set; }
    }
}
