using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskAppModelLibrary.Models
{
    /// <summary>
    /// t_23020_実測抜取評価
    /// </summary>
    [Serializable]
    [Table("t_23020_実測抜取評価")]
    [PrimaryKey(nameof(組合等コード), nameof(年産), nameof(共済目的コード), nameof(調整班区分), nameof(組合員等コード), nameof(耕地番号), nameof(分筆番号))]
    public class T23020実測抜取評価 : ModelBase
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
        /// 調整班区分
        /// </summary>
        [Required]
        [Column("調整班区分", Order = 4)]
        [StringLength(1)]
        public string 調整班区分 { get; set; }

        /// <summary>
        /// 組合員等コード
        /// </summary>
        [Required]
        [Column("組合員等コード", Order = 5)]
        [StringLength(13)]
        public string 組合員等コード { get; set; }

        /// <summary>
        /// 耕地番号
        /// </summary>
        [Required]
        [Column("耕地番号", Order = 6)]
        [StringLength(5)]
        public string 耕地番号 { get; set; }

        /// <summary>
        /// 分筆番号
        /// </summary>
        [Required]
        [Column("分筆番号", Order = 7)]
        [StringLength(4)]
        public string 分筆番号 { get; set; }

        /// <summary>
        /// 類区分
        /// </summary>
        [Column("類区分")]
        [StringLength(2)]
        public string 類区分 { get; set; }

        /// <summary>
        /// 単当換算係数計算方法
        /// </summary>
        [Column("単当換算係数計算方法")]
        [StringLength(1)]
        public string 単当換算係数計算方法 { get; set; }

        /// <summary>
        /// 刈取方法
        /// </summary>
        [Column("刈取方法")]
        [StringLength(1)]
        public string 刈取方法 { get; set; }

        /// <summary>
        /// 選択個所数
        /// </summary>
        [Column("選択個所数")]
        public Decimal? 選択個所数 { get; set; }

        /// <summary>
        /// 刈り取り株数
        /// </summary>
        [Column("刈り取り株数")]
        public Decimal? 刈り取り株数 { get; set; }

        /// <summary>
        /// 刈り取り箇所数ｍ
        /// </summary>
        [Column("刈り取り箇所数ｍ")]
        public Decimal? 刈り取り箇所数ｍ { get; set; }

        /// <summary>
        /// 刈り取り箇所数円
        /// </summary>
        [Column("刈り取り箇所数円")]
        public Decimal? 刈り取り箇所数円 { get; set; }

        /// <summary>
        /// 半径
        /// </summary>
        [Column("半径")]
        public Decimal? 半径 { get; set; }

        /// <summary>
        /// 畦巾１箇所目
        /// </summary>
        [Column("畦巾１箇所目")]
        public Decimal? 畦巾１箇所目 { get; set; }

        /// <summary>
        /// 畦巾２箇所目
        /// </summary>
        [Column("畦巾２箇所目")]
        public Decimal? 畦巾２箇所目 { get; set; }

        /// <summary>
        /// 畦巾３箇所目
        /// </summary>
        [Column("畦巾３箇所目")]
        public Decimal? 畦巾３箇所目 { get; set; }

        /// <summary>
        /// 株間１箇所目
        /// </summary>
        [Column("株間１箇所目")]
        public Decimal? 株間１箇所目 { get; set; }

        /// <summary>
        /// 株間２箇所目
        /// </summary>
        [Column("株間２箇所目")]
        public Decimal? 株間２箇所目 { get; set; }

        /// <summary>
        /// 株間３箇所目
        /// </summary>
        [Column("株間３箇所目")]
        public Decimal? 株間３箇所目 { get; set; }

        /// <summary>
        /// 畦巾平均
        /// </summary>
        [Column("畦巾平均")]
        public Decimal? 畦巾平均 { get; set; }

        /// <summary>
        /// 株間平均
        /// </summary>
        [Column("株間平均")]
        public Decimal? 株間平均 { get; set; }

        /// <summary>
        /// 単当換算係数
        /// </summary>
        [Column("単当換算係数")]
        public Decimal? 単当換算係数 { get; set; }

        /// <summary>
        /// 未調整生もみ重
        /// </summary>
        [Column("未調整生もみ重")]
        public Decimal? 未調整生もみ重 { get; set; }

        /// <summary>
        /// 未調整乾燥もみ重
        /// </summary>
        [Column("未調整乾燥もみ重")]
        public Decimal? 未調整乾燥もみ重 { get; set; }

        /// <summary>
        /// 推定単当収量
        /// </summary>
        [Column("推定単当収量")]
        public Decimal? 推定単当収量 { get; set; }

        /// <summary>
        /// 実測修正率
        /// </summary>
        [Column("実測修正率")]
        public Decimal? 実測修正率 { get; set; }

        /// <summary>
        /// 修正した理由
        /// </summary>
        [Column("修正した理由")]
        [StringLength(20)]
        public string 修正した理由 { get; set; }

        /// <summary>
        /// 水分含有率
        /// </summary>
        [Column("水分含有率")]
        public Decimal? 水分含有率 { get; set; }

        /// <summary>
        /// 粗玄米重
        /// </summary>
        [Column("粗玄米重")]
        public Decimal? 粗玄米重 { get; set; }

        /// <summary>
        /// ふるいにかけた粗玄米重
        /// </summary>
        [Column("ふるいにかけた粗玄米重")]
        public Decimal? ふるいにかけた粗玄米重 { get; set; }

        /// <summary>
        /// 玄米重
        /// </summary>
        [Column("玄米重")]
        public Decimal? 玄米重 { get; set; }

        /// <summary>
        /// くず米重
        /// </summary>
        [Column("くず米重")]
        public Decimal? くず米重 { get; set; }

        /// <summary>
        /// 総合選別機修正率
        /// </summary>
        [Column("総合選別機修正率")]
        public Decimal? 総合選別機修正率 { get; set; }

        /// <summary>
        /// 実測単収
        /// </summary>
        [Column("実測単収")]
        public Decimal? 実測単収 { get; set; }

        /// <summary>
        /// 実測区分
        /// </summary>
        [Column("実測区分")]
        [StringLength(1)]
        public string 実測区分 { get; set; }

        /// <summary>
        /// 階層区分
        /// </summary>
        [Column("階層区分")]
        [StringLength(3)]
        public string 階層区分 { get; set; }

        /// <summary>
        /// 評価地区コード
        /// </summary>
        [Column("評価地区コード")]
        [StringLength(4)]
        public string 評価地区コード { get; set; }

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
