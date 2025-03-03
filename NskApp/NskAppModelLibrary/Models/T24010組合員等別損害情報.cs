using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_24010_組合員等別損害情報
    /// </summary>
    [Serializable]
    [Table("t_24010_組合員等別損害情報")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(類区分), nameof(組合員等コード), nameof(統計単位地域コード))]
    public class T24010組合員等別損害情報 : ModelBase
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
        /// 類区分
        /// </summary>
        [Required]
        [Column("類区分", Order = 4)]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Required]
        [Column("組合員等コード", Order = 5)]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 統計単位地域コード
        /// </summary>
        [Required]
        [Column("統計単位地域コード", Order = 6)]
        [StringLength(5)]
        public string 統計単位地域コード { get; set; }

        /// <summary>
        /// 全相殺計算方法
        /// </summary>
        [Column("全相殺計算方法")]
        [StringLength(1)]
        public string 全相殺計算方法 { get; set; }

        /// <summary>
        /// 収穫量確認方法
        /// </summary>
        [Column("収穫量確認方法")]
        [StringLength(2)]
        public string 収穫量確認方法 { get; set; }

        /// <summary>
        /// 政府保険認定区分
        /// </summary>
        [Column("政府保険認定区分")]
        [StringLength(4)]
        public string 政府保険認定区分 { get; set; }

        /// <summary>
        /// 営農対象外フラグ
        /// </summary>
        [Column("営農対象外フラグ")]
        [StringLength(1)]
        public string 営農対象外フラグ { get; set; }

        /// <summary>
        /// 担手農家区分
        /// </summary>
        [Column("担手農家区分")]
        [StringLength(1)]
        public string 担手農家区分 { get; set; }

        /// <summary>
        /// 営農支払担手コード
        /// </summary>
        [Column("営農支払担手コード")]
        [StringLength(1)]
        public string 営農支払担手コード { get; set; }

        /// <summary>
        /// 悉皆筆数
        /// </summary>
        [Column("悉皆筆数")]
        public Decimal? 悉皆筆数 { get; set; }

        /// <summary>
        /// 悉皆筆面積
        /// </summary>
        [Column("悉皆筆面積")]
        public Decimal? 悉皆筆面積 { get; set; }

        /// <summary>
        /// 一般被害筆数
        /// </summary>
        [Column("一般被害筆数")]
        public Decimal? 一般被害筆数 { get; set; }

        /// <summary>
        /// 一般被害面積
        /// </summary>
        [Column("一般被害面積")]
        public Decimal? 一般被害面積 { get; set; }

        /// <summary>
        /// 一般被害収穫量
        /// </summary>
        [Column("一般被害収穫量")]
        public Decimal? 一般被害収穫量 { get; set; }

        /// <summary>
        /// 一般被害減収量
        /// </summary>
        [Column("一般被害減収量")]
        public Decimal? 一般被害減収量 { get; set; }

        /// <summary>
        /// 皆無筆数
        /// </summary>
        [Column("皆無筆数")]
        public Decimal? 皆無筆数 { get; set; }

        /// <summary>
        /// 皆無面積
        /// </summary>
        [Column("皆無面積")]
        public Decimal? 皆無面積 { get; set; }

        /// <summary>
        /// 皆無収穫量
        /// </summary>
        [Column("皆無収穫量")]
        public Decimal? 皆無収穫量 { get; set; }

        /// <summary>
        /// 皆無減収量
        /// </summary>
        [Column("皆無減収量")]
        public Decimal? 皆無減収量 { get; set; }

        /// <summary>
        /// 不能筆数
        /// </summary>
        [Column("不能筆数")]
        public Decimal? 不能筆数 { get; set; }

        /// <summary>
        /// 不能面積
        /// </summary>
        [Column("不能面積")]
        public Decimal? 不能面積 { get; set; }

        /// <summary>
        /// 不能収穫量
        /// </summary>
        [Column("不能収穫量")]
        public Decimal? 不能収穫量 { get; set; }

        /// <summary>
        /// 不能減収量
        /// </summary>
        [Column("不能減収量")]
        public Decimal? 不能減収量 { get; set; }

        /// <summary>
        /// 転作等筆数
        /// </summary>
        [Column("転作等筆数")]
        public Decimal? 転作等筆数 { get; set; }

        /// <summary>
        /// 転作等面積
        /// </summary>
        [Column("転作等面積")]
        public Decimal? 転作等面積 { get; set; }

        /// <summary>
        /// 転作等収穫量
        /// </summary>
        [Column("転作等収穫量")]
        public Decimal? 転作等収穫量 { get; set; }

        /// <summary>
        /// 転作等減収量
        /// </summary>
        [Column("転作等減収量")]
        public Decimal? 転作等減収量 { get; set; }

        /// <summary>
        /// 分割減収量
        /// </summary>
        [Column("分割減収量")]
        public Decimal? 分割減収量 { get; set; }

        /// <summary>
        /// 全数調査収穫量
        /// </summary>
        [Column("全数調査収穫量")]
        public Decimal? 全数調査収穫量 { get; set; }

        /// <summary>
        /// 不能耕地搬入収穫量
        /// </summary>
        [Column("不能耕地搬入収穫量")]
        public Decimal? 不能耕地搬入収穫量 { get; set; }

        /// <summary>
        /// 不能耕地搬入収穫量補正量
        /// </summary>
        [Column("不能耕地搬入収穫量補正量")]
        public Decimal? 不能耕地搬入収穫量補正量 { get; set; }

        /// <summary>
        /// 組合員等収穫量補正量
        /// </summary>
        [Column("組合員等収穫量補正量")]
        public Decimal? 組合員等収穫量補正量 { get; set; }

        /// <summary>
        /// 直接施設搬入収穫量
        /// </summary>
        [Column("直接施設搬入収穫量")]
        public Decimal? 直接施設搬入収穫量 { get; set; }

        /// <summary>
        /// 収穫量
        /// </summary>
        [Column("収穫量")]
        public Decimal? 収穫量 { get; set; }

        /// <summary>
        /// 超過被害共済減収量
        /// </summary>
        [Column("超過被害共済減収量")]
        public Decimal? 超過被害共済減収量 { get; set; }

        /// <summary>
        /// 一筆全損皆無筆数
        /// </summary>
        [Column("一筆全損皆無筆数")]
        public Decimal? 一筆全損皆無筆数 { get; set; }

        /// <summary>
        /// 一筆全損皆無面積
        /// </summary>
        [Column("一筆全損皆無面積")]
        public Decimal? 一筆全損皆無面積 { get; set; }

        /// <summary>
        /// 一筆全損皆無基準収穫量
        /// </summary>
        [Column("一筆全損皆無基準収穫量")]
        public Decimal? 一筆全損皆無基準収穫量 { get; set; }

        /// <summary>
        /// 一筆全損皆無減収量
        /// </summary>
        [Column("一筆全損皆無減収量")]
        public Decimal? 一筆全損皆無減収量 { get; set; }

        /// <summary>
        /// 一筆全損不能筆数
        /// </summary>
        [Column("一筆全損不能筆数")]
        public Decimal? 一筆全損不能筆数 { get; set; }

        /// <summary>
        /// 一筆全損不能面積
        /// </summary>
        [Column("一筆全損不能面積")]
        public Decimal? 一筆全損不能面積 { get; set; }

        /// <summary>
        /// 一筆全損不能基準収穫量
        /// </summary>
        [Column("一筆全損不能基準収穫量")]
        public Decimal? 一筆全損不能基準収穫量 { get; set; }

        /// <summary>
        /// 一筆全損不能減収量
        /// </summary>
        [Column("一筆全損不能減収量")]
        public Decimal? 一筆全損不能減収量 { get; set; }

        /// <summary>
        /// 一筆全損支払開始減収量
        /// </summary>
        [Column("一筆全損支払開始減収量")]
        public Decimal? 一筆全損支払開始減収量 { get; set; }

        /// <summary>
        /// 一筆全損共済減収量
        /// </summary>
        [Column("一筆全損共済減収量")]
        public Decimal? 一筆全損共済減収量 { get; set; }

        /// <summary>
        /// 当初一筆半損耕地筆数
        /// </summary>
        [Column("当初一筆半損耕地筆数")]
        public Decimal? 当初一筆半損耕地筆数 { get; set; }

        /// <summary>
        /// 当初一筆半損面積
        /// </summary>
        [Column("当初一筆半損面積")]
        public Decimal? 当初一筆半損面積 { get; set; }

        /// <summary>
        /// 当初一筆半損耕地基準収穫量
        /// </summary>
        [Column("当初一筆半損耕地基準収穫量")]
        public Decimal? 当初一筆半損耕地基準収穫量 { get; set; }

        /// <summary>
        /// 当初一筆半損耕地減収量
        /// </summary>
        [Column("当初一筆半損耕地減収量")]
        public Decimal? 当初一筆半損耕地減収量 { get; set; }

        /// <summary>
        /// 当初一筆半損支払開始減収量
        /// </summary>
        [Column("当初一筆半損支払開始減収量")]
        public Decimal? 当初一筆半損支払開始減収量 { get; set; }

        /// <summary>
        /// 当初一筆半損共済減収量
        /// </summary>
        [Column("当初一筆半損共済減収量")]
        public Decimal? 当初一筆半損共済減収量 { get; set; }

        /// <summary>
        /// 当初一筆全半共済減収量
        /// </summary>
        [Column("当初一筆全半共済減収量")]
        public Decimal? 当初一筆全半共済減収量 { get; set; }

        /// <summary>
        /// 被害区分
        /// </summary>
        [Column("被害区分")]
        [StringLength(1)]
        public string 被害区分 { get; set; }

        /// <summary>
        /// 当初被害筆数
        /// </summary>
        [Column("当初被害筆数")]
        public Decimal? 当初被害筆数 { get; set; }

        /// <summary>
        /// 当初被害面積
        /// </summary>
        [Column("当初被害面積")]
        public Decimal? 当初被害面積 { get; set; }

        /// <summary>
        /// 当初共済減収量
        /// </summary>
        [Column("当初共済減収量")]
        public Decimal? 当初共済減収量 { get; set; }

        /// <summary>
        /// 当初共済金見込額
        /// </summary>
        [Column("当初共済金見込額")]
        public Decimal? 当初共済金見込額 { get; set; }

        /// <summary>
        /// 当初分割減収量
        /// </summary>
        [Column("当初分割減収量")]
        public Decimal? 当初分割減収量 { get; set; }

        /// <summary>
        /// 全数調査共済減収量
        /// </summary>
        [Column("全数調査共済減収量")]
        public Decimal? 全数調査共済減収量 { get; set; }

        /// <summary>
        /// 支払対象区分
        /// </summary>
        [Column("支払対象区分")]
        [StringLength(1)]
        public string 支払対象区分 { get; set; }

        /// <summary>
        /// 決定共済減収量
        /// </summary>
        [Column("決定共済減収量")]
        public Decimal? 決定共済減収量 { get; set; }

        /// <summary>
        /// 支払分割減収量
        /// </summary>
        [Column("支払分割減収量")]
        public Decimal? 支払分割減収量 { get; set; }

        /// <summary>
        /// 共済金
        /// </summary>
        [Column("共済金")]
        public Decimal? 共済金 { get; set; }

        /// <summary>
        /// 免責フラグ
        /// </summary>
        [Column("免責フラグ")]
        [StringLength(1)]
        public string 免責フラグ { get; set; }

        /// <summary>
        /// 免責額
        /// </summary>
        [Column("免責額")]
        public Decimal? 免責額 { get; set; }

        /// <summary>
        /// 実支払共済金
        /// </summary>
        [Column("実支払共済金")]
        public Decimal? 実支払共済金 { get; set; }

        /// <summary>
        /// 支払対象面積
        /// </summary>
        [Column("支払対象面積")]
        public Decimal? 支払対象面積 { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Column("引受方式")]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 特約区分
        /// </summary>
        [Column("特約区分")]
        [StringLength(1)]
        public string 特約区分 { get; set; }

        /// <summary>
        /// 補償割合コード
        /// </summary>
        [Column("補償割合コード")]
        [StringLength(2)]
        public string 補償割合コード { get; set; }

        /// <summary>
        /// 合併時識別コード
        /// </summary>
        [Column("合併時識別コード")]
        [StringLength(3)]
        public string 合併時識別コード { get; set; }

        /// <summary>
        /// 大地区コード
        /// </summary>
        [Column("大地区コード")]
        [StringLength(2)]
        public string 大地区コード { get; set; }

        /// <summary>
        /// 小地区コード
        /// </summary>
        [Column("小地区コード")]
        [StringLength(4)]
        public string 小地区コード { get; set; }

        /// <summary>
        /// 支所コード
        /// </summary>
        [Column("支所コード")]
        [StringLength(2)]
        public string 支所コード { get; set; }

        /// <summary>
        /// 引受確定日付
        /// </summary>
        [Column("引受確定日付")]
        public DateTime? 引受確定日付 { get; set; }

        /// <summary>
        /// 当初計算日付
        /// </summary>
        [Column("当初計算日付")]
        public DateTime? 当初計算日付 { get; set; }

        /// <summary>
        /// 決定計算日付
        /// </summary>
        [Column("決定計算日付")]
        public DateTime? 決定計算日付 { get; set; }

        /// <summary>
        /// 統計評価単収
        /// </summary>
        [Column("統計評価単収")]
        public Decimal? 統計評価単収 { get; set; }

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
