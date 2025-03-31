using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_13060_引受通知書
    /// </summary>
    [Serializable]
    [Table("t_13060_引受通知書")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(類区分), nameof(農作物政府保険区分), nameof(引受方式))]
    public class T13060引受通知書 : ModelBase
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
        /// 農作物政府保険区分
        /// </summary>
        [Required]
        [Column("農作物政府保険区分", Order = 5)]
        [StringLength(1)]
        public string 農作物政府保険区分 { get; set; }

        /// <summary>
        /// 引受方式
        /// </summary>
        [Required]
        [Column("引受方式", Order = 6)]
        [StringLength(1)]
        public string 引受方式 { get; set; }

        /// <summary>
        /// 引受方式名称
        /// </summary>
        [Column("引受方式名称")]
        [StringLength(20)]
        public string 引受方式名称 { get; set; }

        /// <summary>
        /// 報告回
        /// </summary>
        [Column("報告回")]
        public Decimal? 報告回 { get; set; }

        /// <summary>
        /// 組合等正式名称
        /// </summary>
        [Column("組合等正式名称")]
        [StringLength(50)]
        public string 組合等正式名称 { get; set; }

        /// <summary>
        /// 代表者役職名
        /// </summary>
        [Column("代表者役職名")]
        [StringLength(24)]
        public string 代表者役職名 { get; set; }

        /// <summary>
        /// 組合等長名
        /// </summary>
        [Column("組合等長名")]
        [StringLength(24)]
        public string 組合等長名 { get; set; }

        /// <summary>
        /// 連合会正式名称
        /// </summary>
        [Column("連合会正式名称")]
        [StringLength(32)]
        public string 連合会正式名称 { get; set; }

        /// <summary>
        /// 連合会長名
        /// </summary>
        [Column("連合会長名")]
        [StringLength(24)]
        public string 連合会長名 { get; set; }

        /// <summary>
        /// 共済目的名称
        /// </summary>
        [Column("共済目的名称")]
        [StringLength(20)]
        public string 共済目的名称 { get; set; }

        /// <summary>
        /// 水稲1類県指示単収
        /// </summary>
        [Column("水稲1類県指示単収")]
        public Decimal? 水稲1類県指示単収 { get; set; }

        /// <summary>
        /// 水稲2類県指示単収
        /// </summary>
        [Column("水稲2類県指示単収")]
        public Decimal? 水稲2類県指示単収 { get; set; }

        /// <summary>
        /// 水稲3類県指示単収
        /// </summary>
        [Column("水稲3類県指示単収")]
        public Decimal? 水稲3類県指示単収 { get; set; }

        /// <summary>
        /// 水稲4類県指示単収
        /// </summary>
        [Column("水稲4類県指示単収")]
        public Decimal? 水稲4類県指示単収 { get; set; }

        /// <summary>
        /// 水稲5類県指示単収
        /// </summary>
        [Column("水稲5類県指示単収")]
        public Decimal? 水稲5類県指示単収 { get; set; }

        /// <summary>
        /// 水稲6類県指示単収
        /// </summary>
        [Column("水稲6類県指示単収")]
        public Decimal? 水稲6類県指示単収 { get; set; }

        /// <summary>
        /// 陸稲1類県指示単収
        /// </summary>
        [Column("陸稲1類県指示単収")]
        public Decimal? 陸稲1類県指示単収 { get; set; }

        /// <summary>
        /// 麦1類県指示単収
        /// </summary>
        [Column("麦1類県指示単収")]
        public Decimal? 麦1類県指示単収 { get; set; }

        /// <summary>
        /// 麦5類県指示単収
        /// </summary>
        [Column("麦5類県指示単収")]
        public Decimal? 麦5類県指示単収 { get; set; }

        /// <summary>
        /// 麦9類県指示単収
        /// </summary>
        [Column("麦9類県指示単収")]
        public Decimal? 麦9類県指示単収 { get; set; }

        /// <summary>
        /// 麦12類県指示単収
        /// </summary>
        [Column("麦12類県指示単収")]
        public Decimal? 麦12類県指示単収 { get; set; }

        /// <summary>
        /// 麦15類県指示単収
        /// </summary>
        [Column("麦15類県指示単収")]
        public Decimal? 麦15類県指示単収 { get; set; }

        /// <summary>
        /// 麦2類県指示単収
        /// </summary>
        [Column("麦2類県指示単収")]
        public Decimal? 麦2類県指示単収 { get; set; }

        /// <summary>
        /// 麦6類県指示単収
        /// </summary>
        [Column("麦6類県指示単収")]
        public Decimal? 麦6類県指示単収 { get; set; }

        /// <summary>
        /// 麦16類県指示単収
        /// </summary>
        [Column("麦16類県指示単収")]
        public Decimal? 麦16類県指示単収 { get; set; }

        /// <summary>
        /// 補償1_補償割合コード
        /// </summary>
        [Column("補償1_補償割合コード")]
        [StringLength(2)]
        public string 補償1_補償割合コード { get; set; }

        /// <summary>
        /// 補償1_補償割合名称
        /// </summary>
        [Column("補償1_補償割合名称")]
        [StringLength(20)]
        public string 補償1_補償割合名称 { get; set; }

        /// <summary>
        /// 補償1_組合等計引受戸数
        /// </summary>
        [Column("補償1_組合等計引受戸数")]
        public Decimal? 補償1_組合等計引受戸数 { get; set; }

        /// <summary>
        /// 補償1_引受延戸数
        /// </summary>
        [Column("補償1_引受延戸数")]
        public Decimal? 補償1_引受延戸数 { get; set; }

        /// <summary>
        /// 補償1_組合等計引受面積
        /// </summary>
        [Column("補償1_組合等計引受面積")]
        public Decimal? 補償1_組合等計引受面積 { get; set; }

        /// <summary>
        /// 補償1_組合等計基準収穫量
        /// </summary>
        [Column("補償1_組合等計基準収穫量")]
        public Decimal? 補償1_組合等計基準収穫量 { get; set; }

        /// <summary>
        /// 補償1_組合等計引受収量
        /// </summary>
        [Column("補償1_組合等計引受収量")]
        public Decimal? 補償1_組合等計引受収量 { get; set; }

        /// <summary>
        /// 補償1_組合等計共済金額
        /// </summary>
        [Column("補償1_組合等計共済金額")]
        public Decimal? 補償1_組合等計共済金額 { get; set; }

        /// <summary>
        /// 補償1_組合等計基準共済掛金
        /// </summary>
        [Column("補償1_組合等計基準共済掛金")]
        public Decimal? 補償1_組合等計基準共済掛金 { get; set; }

        /// <summary>
        /// 補償1_組合等計共済掛金
        /// </summary>
        [Column("補償1_組合等計共済掛金")]
        public Decimal? 補償1_組合等計共済掛金 { get; set; }

        /// <summary>
        /// 補償1_組合等計交付病虫割引済額
        /// </summary>
        [Column("補償1_組合等計交付病虫割引済額")]
        public Decimal? 補償1_組合等計交付病虫割引済額 { get; set; }

        /// <summary>
        /// 補償1_組合等計員等病虫割引済額
        /// </summary>
        [Column("補償1_組合等計員等病虫割引済額")]
        public Decimal? 補償1_組合等計員等病虫割引済額 { get; set; }

        /// <summary>
        /// 補償1_組合等計保険金額
        /// </summary>
        [Column("補償1_組合等計保険金額")]
        public Decimal? 補償1_組合等計保険金額 { get; set; }

        /// <summary>
        /// 補償1_組合等保険料総額
        /// </summary>
        [Column("補償1_組合等保険料総額")]
        public Decimal? 補償1_組合等保険料総額 { get; set; }

        /// <summary>
        /// 補償1_水稲1類引受面積
        /// </summary>
        [Column("補償1_水稲1類引受面積")]
        public Decimal? 補償1_水稲1類引受面積 { get; set; }

        /// <summary>
        /// 補償1_水稲1類総基準収穫量
        /// </summary>
        [Column("補償1_水稲1類総基準収穫量")]
        public Decimal? 補償1_水稲1類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償1_水稲2類引受面積
        /// </summary>
        [Column("補償1_水稲2類引受面積")]
        public Decimal? 補償1_水稲2類引受面積 { get; set; }

        /// <summary>
        /// 補償1_水稲2類総基準収穫量
        /// </summary>
        [Column("補償1_水稲2類総基準収穫量")]
        public Decimal? 補償1_水稲2類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償1_水稲3類引受面積
        /// </summary>
        [Column("補償1_水稲3類引受面積")]
        public Decimal? 補償1_水稲3類引受面積 { get; set; }

        /// <summary>
        /// 補償1_水稲3類総基準収穫量
        /// </summary>
        [Column("補償1_水稲3類総基準収穫量")]
        public Decimal? 補償1_水稲3類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償1_水稲4類引受面積
        /// </summary>
        [Column("補償1_水稲4類引受面積")]
        public Decimal? 補償1_水稲4類引受面積 { get; set; }

        /// <summary>
        /// 補償1_水稲4類総基準収穫量
        /// </summary>
        [Column("補償1_水稲4類総基準収穫量")]
        public Decimal? 補償1_水稲4類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償1_水稲5類引受面積
        /// </summary>
        [Column("補償1_水稲5類引受面積")]
        public Decimal? 補償1_水稲5類引受面積 { get; set; }

        /// <summary>
        /// 補償1_水稲5類総基準収穫量
        /// </summary>
        [Column("補償1_水稲5類総基準収穫量")]
        public Decimal? 補償1_水稲5類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償1_水稲6類引受面積
        /// </summary>
        [Column("補償1_水稲6類引受面積")]
        public Decimal? 補償1_水稲6類引受面積 { get; set; }

        /// <summary>
        /// 補償1_水稲6類総基準収穫量
        /// </summary>
        [Column("補償1_水稲6類総基準収穫量")]
        public Decimal? 補償1_水稲6類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償1_陸稲1類引受面積
        /// </summary>
        [Column("補償1_陸稲1類引受面積")]
        public Decimal? 補償1_陸稲1類引受面積 { get; set; }

        /// <summary>
        /// 補償1_陸稲1類総基準収穫量
        /// </summary>
        [Column("補償1_陸稲1類総基準収穫量")]
        public Decimal? 補償1_陸稲1類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償1_麦1類引受面積
        /// </summary>
        [Column("補償1_麦1類引受面積")]
        public Decimal? 補償1_麦1類引受面積 { get; set; }

        /// <summary>
        /// 補償1_麦1類総基準収穫量
        /// </summary>
        [Column("補償1_麦1類総基準収穫量")]
        public Decimal? 補償1_麦1類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償1_麦5類引受面積
        /// </summary>
        [Column("補償1_麦5類引受面積")]
        public Decimal? 補償1_麦5類引受面積 { get; set; }

        /// <summary>
        /// 補償1_麦5類総基準収穫量
        /// </summary>
        [Column("補償1_麦5類総基準収穫量")]
        public Decimal? 補償1_麦5類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償1_麦9類引受面積
        /// </summary>
        [Column("補償1_麦9類引受面積")]
        public Decimal? 補償1_麦9類引受面積 { get; set; }

        /// <summary>
        /// 補償1_麦9類総基準収穫量
        /// </summary>
        [Column("補償1_麦9類総基準収穫量")]
        public Decimal? 補償1_麦9類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償1_麦12類引受面積
        /// </summary>
        [Column("補償1_麦12類引受面積")]
        public Decimal? 補償1_麦12類引受面積 { get; set; }

        /// <summary>
        /// 補償1_麦12類総基準収穫量
        /// </summary>
        [Column("補償1_麦12類総基準収穫量")]
        public Decimal? 補償1_麦12類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償1_麦15類引受面積
        /// </summary>
        [Column("補償1_麦15類引受面積")]
        public Decimal? 補償1_麦15類引受面積 { get; set; }

        /// <summary>
        /// 補償1_麦15類総基準収穫量
        /// </summary>
        [Column("補償1_麦15類総基準収穫量")]
        public Decimal? 補償1_麦15類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償1_麦2類引受面積
        /// </summary>
        [Column("補償1_麦2類引受面積")]
        public Decimal? 補償1_麦2類引受面積 { get; set; }

        /// <summary>
        /// 補償1_麦2類総基準収穫量
        /// </summary>
        [Column("補償1_麦2類総基準収穫量")]
        public Decimal? 補償1_麦2類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償1_麦6類引受面積
        /// </summary>
        [Column("補償1_麦6類引受面積")]
        public Decimal? 補償1_麦6類引受面積 { get; set; }

        /// <summary>
        /// 補償1_麦6類総基準収穫量
        /// </summary>
        [Column("補償1_麦6類総基準収穫量")]
        public Decimal? 補償1_麦6類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償1_麦16類引受面積
        /// </summary>
        [Column("補償1_麦16類引受面積")]
        public Decimal? 補償1_麦16類引受面積 { get; set; }

        /// <summary>
        /// 補償1_麦16類総基準収穫量
        /// </summary>
        [Column("補償1_麦16類総基準収穫量")]
        public Decimal? 補償1_麦16類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_補償割合コード
        /// </summary>
        [Column("補償2_補償割合コード")]
        public Decimal? 補償2_補償割合コード { get; set; }

        /// <summary>
        /// 補償2_補償割合名称
        /// </summary>
        [Column("補償2_補償割合名称")]
        public Decimal? 補償2_補償割合名称 { get; set; }

        /// <summary>
        /// 補償2_組合等計引受戸数
        /// </summary>
        [Column("補償2_組合等計引受戸数")]
        public Decimal? 補償2_組合等計引受戸数 { get; set; }

        /// <summary>
        /// 補償2_引受延戸数
        /// </summary>
        [Column("補償2_引受延戸数")]
        public Decimal? 補償2_引受延戸数 { get; set; }

        /// <summary>
        /// 補償2_組合等計引受面積
        /// </summary>
        [Column("補償2_組合等計引受面積")]
        public Decimal? 補償2_組合等計引受面積 { get; set; }

        /// <summary>
        /// 補償2_組合等計基準収穫量
        /// </summary>
        [Column("補償2_組合等計基準収穫量")]
        public Decimal? 補償2_組合等計基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_組合等計引受収量
        /// </summary>
        [Column("補償2_組合等計引受収量")]
        public Decimal? 補償2_組合等計引受収量 { get; set; }

        /// <summary>
        /// 補償2_組合等計共済金額
        /// </summary>
        [Column("補償2_組合等計共済金額")]
        public Decimal? 補償2_組合等計共済金額 { get; set; }

        /// <summary>
        /// 補償2_組合等計基準共済掛金
        /// </summary>
        [Column("補償2_組合等計基準共済掛金")]
        public Decimal? 補償2_組合等計基準共済掛金 { get; set; }

        /// <summary>
        /// 補償2_組合等計共済掛金
        /// </summary>
        [Column("補償2_組合等計共済掛金")]
        public Decimal? 補償2_組合等計共済掛金 { get; set; }

        /// <summary>
        /// 補償2_組合等計交付病虫割引済額
        /// </summary>
        [Column("補償2_組合等計交付病虫割引済額")]
        public Decimal? 補償2_組合等計交付病虫割引済額 { get; set; }

        /// <summary>
        /// 補償2_組合等計員等病虫割引済額
        /// </summary>
        [Column("補償2_組合等計員等病虫割引済額")]
        public Decimal? 補償2_組合等計員等病虫割引済額 { get; set; }

        /// <summary>
        /// 補償2_組合等計保険金額
        /// </summary>
        [Column("補償2_組合等計保険金額")]
        public Decimal? 補償2_組合等計保険金額 { get; set; }

        /// <summary>
        /// 補償2_組合等保険料総額
        /// </summary>
        [Column("補償2_組合等保険料総額")]
        public Decimal? 補償2_組合等保険料総額 { get; set; }

        /// <summary>
        /// 補償2_水稲1類引受面積
        /// </summary>
        [Column("補償2_水稲1類引受面積")]
        public Decimal? 補償2_水稲1類引受面積 { get; set; }

        /// <summary>
        /// 補償2_水稲1類総基準収穫量
        /// </summary>
        [Column("補償2_水稲1類総基準収穫量")]
        public Decimal? 補償2_水稲1類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_水稲2類引受面積
        /// </summary>
        [Column("補償2_水稲2類引受面積")]
        public Decimal? 補償2_水稲2類引受面積 { get; set; }

        /// <summary>
        /// 補償2_水稲2類総基準収穫量
        /// </summary>
        [Column("補償2_水稲2類総基準収穫量")]
        public Decimal? 補償2_水稲2類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_水稲3類引受面積
        /// </summary>
        [Column("補償2_水稲3類引受面積")]
        public Decimal? 補償2_水稲3類引受面積 { get; set; }

        /// <summary>
        /// 補償2_水稲3類総基準収穫量
        /// </summary>
        [Column("補償2_水稲3類総基準収穫量")]
        public Decimal? 補償2_水稲3類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_水稲4類引受面積
        /// </summary>
        [Column("補償2_水稲4類引受面積")]
        public Decimal? 補償2_水稲4類引受面積 { get; set; }

        /// <summary>
        /// 補償2_水稲4類総基準収穫量
        /// </summary>
        [Column("補償2_水稲4類総基準収穫量")]
        public Decimal? 補償2_水稲4類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_水稲5類引受面積
        /// </summary>
        [Column("補償2_水稲5類引受面積")]
        public Decimal? 補償2_水稲5類引受面積 { get; set; }

        /// <summary>
        /// 補償2_水稲5類総基準収穫量
        /// </summary>
        [Column("補償2_水稲5類総基準収穫量")]
        public Decimal? 補償2_水稲5類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_水稲6類引受面積
        /// </summary>
        [Column("補償2_水稲6類引受面積")]
        public Decimal? 補償2_水稲6類引受面積 { get; set; }

        /// <summary>
        /// 補償2_水稲6類総基準収穫量
        /// </summary>
        [Column("補償2_水稲6類総基準収穫量")]
        public Decimal? 補償2_水稲6類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_陸稲1類引受面積
        /// </summary>
        [Column("補償2_陸稲1類引受面積")]
        public Decimal? 補償2_陸稲1類引受面積 { get; set; }

        /// <summary>
        /// 補償2_陸稲1類総基準収穫量
        /// </summary>
        [Column("補償2_陸稲1類総基準収穫量")]
        public Decimal? 補償2_陸稲1類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_麦1類引受面積
        /// </summary>
        [Column("補償2_麦1類引受面積")]
        public Decimal? 補償2_麦1類引受面積 { get; set; }

        /// <summary>
        /// 補償2_麦1類総基準収穫量
        /// </summary>
        [Column("補償2_麦1類総基準収穫量")]
        public Decimal? 補償2_麦1類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_麦5類引受面積
        /// </summary>
        [Column("補償2_麦5類引受面積")]
        public Decimal? 補償2_麦5類引受面積 { get; set; }

        /// <summary>
        /// 補償2_麦5類総基準収穫量
        /// </summary>
        [Column("補償2_麦5類総基準収穫量")]
        public Decimal? 補償2_麦5類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_麦9類引受面積
        /// </summary>
        [Column("補償2_麦9類引受面積")]
        public Decimal? 補償2_麦9類引受面積 { get; set; }

        /// <summary>
        /// 補償2_麦9類総基準収穫量
        /// </summary>
        [Column("補償2_麦9類総基準収穫量")]
        public Decimal? 補償2_麦9類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_麦12類引受面積
        /// </summary>
        [Column("補償2_麦12類引受面積")]
        public Decimal? 補償2_麦12類引受面積 { get; set; }

        /// <summary>
        /// 補償2_麦12類総基準収穫量
        /// </summary>
        [Column("補償2_麦12類総基準収穫量")]
        public Decimal? 補償2_麦12類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_麦15類引受面積
        /// </summary>
        [Column("補償2_麦15類引受面積")]
        public Decimal? 補償2_麦15類引受面積 { get; set; }

        /// <summary>
        /// 補償2_麦15類総基準収穫量
        /// </summary>
        [Column("補償2_麦15類総基準収穫量")]
        public Decimal? 補償2_麦15類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_麦2類引受面積
        /// </summary>
        [Column("補償2_麦2類引受面積")]
        public Decimal? 補償2_麦2類引受面積 { get; set; }

        /// <summary>
        /// 補償2_麦2類総基準収穫量
        /// </summary>
        [Column("補償2_麦2類総基準収穫量")]
        public Decimal? 補償2_麦2類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_麦6類引受面積
        /// </summary>
        [Column("補償2_麦6類引受面積")]
        public Decimal? 補償2_麦6類引受面積 { get; set; }

        /// <summary>
        /// 補償2_麦6類総基準収穫量
        /// </summary>
        [Column("補償2_麦6類総基準収穫量")]
        public Decimal? 補償2_麦6類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償2_麦16類引受面積
        /// </summary>
        [Column("補償2_麦16類引受面積")]
        public Decimal? 補償2_麦16類引受面積 { get; set; }

        /// <summary>
        /// 補償2_麦16類総基準収穫量
        /// </summary>
        [Column("補償2_麦16類総基準収穫量")]
        public Decimal? 補償2_麦16類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_補償割合コード
        /// </summary>
        [Column("補償3_補償割合コード")]
        public Decimal? 補償3_補償割合コード { get; set; }

        /// <summary>
        /// 補償3_補償割合名称
        /// </summary>
        [Column("補償3_補償割合名称")]
        public Decimal? 補償3_補償割合名称 { get; set; }

        /// <summary>
        /// 補償3_組合等計引受戸数
        /// </summary>
        [Column("補償3_組合等計引受戸数")]
        public Decimal? 補償3_組合等計引受戸数 { get; set; }

        /// <summary>
        /// 補償3_引受延戸数
        /// </summary>
        [Column("補償3_引受延戸数")]
        public Decimal? 補償3_引受延戸数 { get; set; }

        /// <summary>
        /// 補償3_組合等計引受面積
        /// </summary>
        [Column("補償3_組合等計引受面積")]
        public Decimal? 補償3_組合等計引受面積 { get; set; }

        /// <summary>
        /// 補償3_組合等計基準収穫量
        /// </summary>
        [Column("補償3_組合等計基準収穫量")]
        public Decimal? 補償3_組合等計基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_組合等計引受収量
        /// </summary>
        [Column("補償3_組合等計引受収量")]
        public Decimal? 補償3_組合等計引受収量 { get; set; }

        /// <summary>
        /// 補償3_組合等計共済金額
        /// </summary>
        [Column("補償3_組合等計共済金額")]
        public Decimal? 補償3_組合等計共済金額 { get; set; }

        /// <summary>
        /// 補償3_組合等計基準共済掛金
        /// </summary>
        [Column("補償3_組合等計基準共済掛金")]
        public Decimal? 補償3_組合等計基準共済掛金 { get; set; }

        /// <summary>
        /// 補償3_組合等計共済掛金
        /// </summary>
        [Column("補償3_組合等計共済掛金")]
        public Decimal? 補償3_組合等計共済掛金 { get; set; }

        /// <summary>
        /// 補償3_組合等計交付病虫割引済額
        /// </summary>
        [Column("補償3_組合等計交付病虫割引済額")]
        public Decimal? 補償3_組合等計交付病虫割引済額 { get; set; }

        /// <summary>
        /// 補償3_組合等計員等病虫割引済額
        /// </summary>
        [Column("補償3_組合等計員等病虫割引済額")]
        public Decimal? 補償3_組合等計員等病虫割引済額 { get; set; }

        /// <summary>
        /// 補償3_組合等計保険金額
        /// </summary>
        [Column("補償3_組合等計保険金額")]
        public Decimal? 補償3_組合等計保険金額 { get; set; }

        /// <summary>
        /// 補償3_組合等保険料総額
        /// </summary>
        [Column("補償3_組合等保険料総額")]
        public Decimal? 補償3_組合等保険料総額 { get; set; }

        /// <summary>
        /// 補償3_水稲1類引受面積
        /// </summary>
        [Column("補償3_水稲1類引受面積")]
        public Decimal? 補償3_水稲1類引受面積 { get; set; }

        /// <summary>
        /// 補償3_水稲1類総基準収穫量
        /// </summary>
        [Column("補償3_水稲1類総基準収穫量")]
        public Decimal? 補償3_水稲1類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_水稲2類引受面積
        /// </summary>
        [Column("補償3_水稲2類引受面積")]
        public Decimal? 補償3_水稲2類引受面積 { get; set; }

        /// <summary>
        /// 補償3_水稲2類総基準収穫量
        /// </summary>
        [Column("補償3_水稲2類総基準収穫量")]
        public Decimal? 補償3_水稲2類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_水稲3類引受面積
        /// </summary>
        [Column("補償3_水稲3類引受面積")]
        public Decimal? 補償3_水稲3類引受面積 { get; set; }

        /// <summary>
        /// 補償3_水稲3類総基準収穫量
        /// </summary>
        [Column("補償3_水稲3類総基準収穫量")]
        public Decimal? 補償3_水稲3類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_水稲4類引受面積
        /// </summary>
        [Column("補償3_水稲4類引受面積")]
        public Decimal? 補償3_水稲4類引受面積 { get; set; }

        /// <summary>
        /// 補償3_水稲4類総基準収穫量
        /// </summary>
        [Column("補償3_水稲4類総基準収穫量")]
        public Decimal? 補償3_水稲4類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_水稲5類引受面積
        /// </summary>
        [Column("補償3_水稲5類引受面積")]
        public Decimal? 補償3_水稲5類引受面積 { get; set; }

        /// <summary>
        /// 補償3_水稲5類総基準収穫量
        /// </summary>
        [Column("補償3_水稲5類総基準収穫量")]
        public Decimal? 補償3_水稲5類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_水稲6類引受面積
        /// </summary>
        [Column("補償3_水稲6類引受面積")]
        public Decimal? 補償3_水稲6類引受面積 { get; set; }

        /// <summary>
        /// 補償3_陸稲1類引受面積
        /// </summary>
        [Column("補償3_陸稲1類引受面積")]
        public Decimal? 補償3_陸稲1類引受面積 { get; set; }

        /// <summary>
        /// 補償3_陸稲1類総基準収穫量
        /// </summary>
        [Column("補償3_陸稲1類総基準収穫量")]
        public Decimal? 補償3_陸稲1類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_水稲6類総基準収穫量
        /// </summary>
        [Column("補償3_水稲6類総基準収穫量")]
        public Decimal? 補償3_水稲6類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_麦1類引受面積
        /// </summary>
        [Column("補償3_麦1類引受面積")]
        public Decimal? 補償3_麦1類引受面積 { get; set; }

        /// <summary>
        /// 補償3_麦1類総基準収穫量
        /// </summary>
        [Column("補償3_麦1類総基準収穫量")]
        public Decimal? 補償3_麦1類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_麦5類引受面積
        /// </summary>
        [Column("補償3_麦5類引受面積")]
        public Decimal? 補償3_麦5類引受面積 { get; set; }

        /// <summary>
        /// 補償3_麦5類総基準収穫量
        /// </summary>
        [Column("補償3_麦5類総基準収穫量")]
        public Decimal? 補償3_麦5類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_麦9類引受面積
        /// </summary>
        [Column("補償3_麦9類引受面積")]
        public Decimal? 補償3_麦9類引受面積 { get; set; }

        /// <summary>
        /// 補償3_麦9類総基準収穫量
        /// </summary>
        [Column("補償3_麦9類総基準収穫量")]
        public Decimal? 補償3_麦9類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_麦12類引受面積
        /// </summary>
        [Column("補償3_麦12類引受面積")]
        public Decimal? 補償3_麦12類引受面積 { get; set; }

        /// <summary>
        /// 補償3_麦12類総基準収穫量
        /// </summary>
        [Column("補償3_麦12類総基準収穫量")]
        public Decimal? 補償3_麦12類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_麦15類引受面積
        /// </summary>
        [Column("補償3_麦15類引受面積")]
        public Decimal? 補償3_麦15類引受面積 { get; set; }

        /// <summary>
        /// 補償3_麦15類総基準収穫量
        /// </summary>
        [Column("補償3_麦15類総基準収穫量")]
        public Decimal? 補償3_麦15類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_麦2類引受面積
        /// </summary>
        [Column("補償3_麦2類引受面積")]
        public Decimal? 補償3_麦2類引受面積 { get; set; }

        /// <summary>
        /// 補償3_麦2類総基準収穫量
        /// </summary>
        [Column("補償3_麦2類総基準収穫量")]
        public Decimal? 補償3_麦2類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_麦6類引受面積
        /// </summary>
        [Column("補償3_麦6類引受面積")]
        public Decimal? 補償3_麦6類引受面積 { get; set; }

        /// <summary>
        /// 補償3_麦6類総基準収穫量
        /// </summary>
        [Column("補償3_麦6類総基準収穫量")]
        public Decimal? 補償3_麦6類総基準収穫量 { get; set; }

        /// <summary>
        /// 補償3_麦16類引受面積
        /// </summary>
        [Column("補償3_麦16類引受面積")]
        public Decimal? 補償3_麦16類引受面積 { get; set; }

        /// <summary>
        /// 補償3_麦16類総基準収穫量
        /// </summary>
        [Column("補償3_麦16類総基準収穫量")]
        public Decimal? 補償3_麦16類総基準収穫量 { get; set; }

        /// <summary>
        /// 補合_組合等計引受戸数
        /// </summary>
        [Column("補合_組合等計引受戸数")]
        public Decimal? 補合_組合等計引受戸数 { get; set; }

        /// <summary>
        /// 補合_引受延戸数
        /// </summary>
        [Column("補合_引受延戸数")]
        public Decimal? 補合_引受延戸数 { get; set; }

        /// <summary>
        /// 補合_組合等計引受面積
        /// </summary>
        [Column("補合_組合等計引受面積")]
        public Decimal? 補合_組合等計引受面積 { get; set; }

        /// <summary>
        /// 補合_組合等計基準収穫量
        /// </summary>
        [Column("補合_組合等計基準収穫量")]
        public Decimal? 補合_組合等計基準収穫量 { get; set; }

        /// <summary>
        /// 補合_組合等計引受収量
        /// </summary>
        [Column("補合_組合等計引受収量")]
        public Decimal? 補合_組合等計引受収量 { get; set; }

        /// <summary>
        /// 補合_組合等計共済金額
        /// </summary>
        [Column("補合_組合等計共済金額")]
        public Decimal? 補合_組合等計共済金額 { get; set; }

        /// <summary>
        /// 補合_組合等計基準共済掛金
        /// </summary>
        [Column("補合_組合等計基準共済掛金")]
        public Decimal? 補合_組合等計基準共済掛金 { get; set; }

        /// <summary>
        /// 補合_組合等計共済掛金
        /// </summary>
        [Column("補合_組合等計共済掛金")]
        public Decimal? 補合_組合等計共済掛金 { get; set; }

        /// <summary>
        /// 補合_組合等計交付病虫割引済額
        /// </summary>
        [Column("補合_組合等計交付病虫割引済額")]
        public Decimal? 補合_組合等計交付病虫割引済額 { get; set; }

        /// <summary>
        /// 補合_組合等計員等病虫割引済額
        /// </summary>
        [Column("補合_組合等計員等病虫割引済額")]
        public Decimal? 補合_組合等計員等病虫割引済額 { get; set; }

        /// <summary>
        /// 補合_組合等計保険金額
        /// </summary>
        [Column("補合_組合等計保険金額")]
        public Decimal? 補合_組合等計保険金額 { get; set; }

        /// <summary>
        /// 補合_組合等保険料総額
        /// </summary>
        [Column("補合_組合等保険料総額")]
        public Decimal? 補合_組合等保険料総額 { get; set; }

        /// <summary>
        /// 補合_水稲1類引受面積
        /// </summary>
        [Column("補合_水稲1類引受面積")]
        public Decimal? 補合_水稲1類引受面積 { get; set; }

        /// <summary>
        /// 補合_水稲1類総基準収穫量
        /// </summary>
        [Column("補合_水稲1類総基準収穫量")]
        public Decimal? 補合_水稲1類総基準収穫量 { get; set; }

        /// <summary>
        /// 補合_水稲2類引受面積
        /// </summary>
        [Column("補合_水稲2類引受面積")]
        public Decimal? 補合_水稲2類引受面積 { get; set; }

        /// <summary>
        /// 補合_水稲2類総基準収穫量
        /// </summary>
        [Column("補合_水稲2類総基準収穫量")]
        public Decimal? 補合_水稲2類総基準収穫量 { get; set; }

        /// <summary>
        /// 補合_水稲3類引受面積
        /// </summary>
        [Column("補合_水稲3類引受面積")]
        public Decimal? 補合_水稲3類引受面積 { get; set; }

        /// <summary>
        /// 補合_水稲3類総基準収穫量
        /// </summary>
        [Column("補合_水稲3類総基準収穫量")]
        public Decimal? 補合_水稲3類総基準収穫量 { get; set; }

        /// <summary>
        /// 補合_水稲4類引受面積
        /// </summary>
        [Column("補合_水稲4類引受面積")]
        public Decimal? 補合_水稲4類引受面積 { get; set; }

        /// <summary>
        /// 補合_水稲4類総基準収穫量
        /// </summary>
        [Column("補合_水稲4類総基準収穫量")]
        public Decimal? 補合_水稲4類総基準収穫量 { get; set; }

        /// <summary>
        /// 補合_水稲5類引受面積
        /// </summary>
        [Column("補合_水稲5類引受面積")]
        public Decimal? 補合_水稲5類引受面積 { get; set; }

        /// <summary>
        /// 補合_水稲5類総基準収穫量
        /// </summary>
        [Column("補合_水稲5類総基準収穫量")]
        public Decimal? 補合_水稲5類総基準収穫量 { get; set; }

        /// <summary>
        /// 補合_水稲6類引受面積
        /// </summary>
        [Column("補合_水稲6類引受面積")]
        public Decimal? 補合_水稲6類引受面積 { get; set; }

        /// <summary>
        /// 補合_水稲6類総基準収穫量
        /// </summary>
        [Column("補合_水稲6類総基準収穫量")]
        public Decimal? 補合_水稲6類総基準収穫量 { get; set; }

        /// <summary>
        /// 補合_陸稲1類引受面積
        /// </summary>
        [Column("補合_陸稲1類引受面積")]
        public Decimal? 補合_陸稲1類引受面積 { get; set; }

        /// <summary>
        /// 補合_陸稲1類総基準収穫量
        /// </summary>
        [Column("補合_陸稲1類総基準収穫量")]
        public Decimal? 補合_陸稲1類総基準収穫量 { get; set; }

        /// <summary>
        /// 補合_麦1類引受面積
        /// </summary>
        [Column("補合_麦1類引受面積")]
        public Decimal? 補合_麦1類引受面積 { get; set; }

        /// <summary>
        /// 補合_麦1類総基準収穫量
        /// </summary>
        [Column("補合_麦1類総基準収穫量")]
        public Decimal? 補合_麦1類総基準収穫量 { get; set; }

        /// <summary>
        /// 補合_麦5類引受面積
        /// </summary>
        [Column("補合_麦5類引受面積")]
        public Decimal? 補合_麦5類引受面積 { get; set; }

        /// <summary>
        /// 補合_麦5類総基準収穫量
        /// </summary>
        [Column("補合_麦5類総基準収穫量")]
        public Decimal? 補合_麦5類総基準収穫量 { get; set; }

        /// <summary>
        /// 補合_麦9類引受面積
        /// </summary>
        [Column("補合_麦9類引受面積")]
        public Decimal? 補合_麦9類引受面積 { get; set; }

        /// <summary>
        /// 補合_麦9類総基準収穫量
        /// </summary>
        [Column("補合_麦9類総基準収穫量")]
        public Decimal? 補合_麦9類総基準収穫量 { get; set; }

        /// <summary>
        /// 補合_麦12類引受面積
        /// </summary>
        [Column("補合_麦12類引受面積")]
        public Decimal? 補合_麦12類引受面積 { get; set; }

        /// <summary>
        /// 補合_麦12類総基準収穫量
        /// </summary>
        [Column("補合_麦12類総基準収穫量")]
        public Decimal? 補合_麦12類総基準収穫量 { get; set; }

        /// <summary>
        /// 補合_麦15類引受面積
        /// </summary>
        [Column("補合_麦15類引受面積")]
        public Decimal? 補合_麦15類引受面積 { get; set; }

        /// <summary>
        /// 補合_麦15類総基準収穫量
        /// </summary>
        [Column("補合_麦15類総基準収穫量")]
        public Decimal? 補合_麦15類総基準収穫量 { get; set; }

        /// <summary>
        /// 補合_麦2類引受面積
        /// </summary>
        [Column("補合_麦2類引受面積")]
        public Decimal? 補合_麦2類引受面積 { get; set; }

        /// <summary>
        /// 補合_麦2類総基準収穫量
        /// </summary>
        [Column("補合_麦2類総基準収穫量")]
        public Decimal? 補合_麦2類総基準収穫量 { get; set; }

        /// <summary>
        /// 補合_麦6類引受面積
        /// </summary>
        [Column("補合_麦6類引受面積")]
        public Decimal? 補合_麦6類引受面積 { get; set; }

        /// <summary>
        /// 補合_麦6類総基準収穫量
        /// </summary>
        [Column("補合_麦6類総基準収穫量")]
        public Decimal? 補合_麦6類総基準収穫量 { get; set; }

        /// <summary>
        /// 補合_麦16類引受面積
        /// </summary>
        [Column("補合_麦16類引受面積")]
        public Decimal? 補合_麦16類引受面積 { get; set; }

        /// <summary>
        /// 補合_麦16類総基準収穫量
        /// </summary>
        [Column("補合_麦16類総基準収穫量")]
        public Decimal? 補合_麦16類総基準収穫量 { get; set; }

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
