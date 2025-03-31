using CoreLibrary.Core.Validator;
using Microsoft.AspNetCore.Mvc.Rendering;
using NskAppModelLibrary.Context;
using NskAppModelLibrary.Models;
using NskWeb.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NskWeb.Areas.F105.Models.D105030
{
    /// <summary>
    /// 類別設定情報入力明細
    /// </summary>
    public class D105030RuibetsuSetteiRecord : BasePagerRecord
    {
        /// <summary>引受区分</summary>
        [Display(Name = "引受区分")]
        public string HikiukeKbn { get; set; } = string.Empty;
        /// <summary>引受方式</summary>
        [Display(Name = "引受方式")]
        [Required]
        public string HikiukeHoushiki { get; set; } = string.Empty;
        /// <summary>補償割合</summary>
        [Display(Name = "補償割合")]
        [Required]
        public string HoshoWariai { get; set; } = string.Empty;
        /// <summary>付保割合</summary>
        [Display(Name = "付保割合")]
        [Numeric]
        [WithinDigitLength(2)]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? FuhoWariai { get; set; }
        /// <summary>一筆半損特約</summary>
        public string IppitsuHansonTokuyaku { get; set; } = string.Empty;
        /// <summary>選択共済金額</summary>
        public decimal? SelectKyosaiKingaku { get; set; }
        /// <summary>危険段階区分（料率）</summary>
        public string KikenDankaiKbn { get; set; } = string.Empty;
        /// <summary>収穫量確認方法</summary>
        public string SyukakuryoKakuninHouhou { get; set; } = string.Empty;
        /// <summary>全相殺基準単収</summary>
        [Display(Name = "全相殺基準単収")]
        [Numeric]
        [WithinDigitLength(3)]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal? ZensousaiKijunTansyu { get; set; }

        /// <summary>個人設定類xmin</summary>
        public uint? Xmin { get; set; }

        /// <summary>選択共済金額ドロップダウンリスト選択値</summary>
        [NotMapped]
        public List<SelectListItem> SelectKyosaiKingakuList { get; set; } = new();

        /// <summary>
        /// srcオブジェクトとの比較
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public bool Compare(D105030RuibetsuSetteiRecord src)
        {
            return (
                ($"{this.HikiukeKbn}" == $"{src.HikiukeKbn}") &&
                ($"{this.HikiukeHoushiki}" == $"{src.HikiukeHoushiki}") &&
                ($"{this.HoshoWariai}" == $"{src.HoshoWariai}") &&
                ($"{this.FuhoWariai}" == $"{src.FuhoWariai}") &&
                ($"{this.IppitsuHansonTokuyaku}" == $"{src.IppitsuHansonTokuyaku}") &&
                ($"{this.SelectKyosaiKingaku}" == $"{src.SelectKyosaiKingaku}") &&
                ($"{this.KikenDankaiKbn}" == $"{src.KikenDankaiKbn}") &&
                ($"{this.SyukakuryoKakuninHouhou}" == $"{src.SyukakuryoKakuninHouhou}") &&
                ($"{this.ZensousaiKijunTansyu}" == $"{src.ZensousaiKijunTansyu}")
            );
        }

        /// <summary>
        /// ドロップダウンリスト初期化
        /// </summary>
        /// <param name="dbContext"></param>
        public void InitializeDropdonwList(NskAppContext dbContext, D105030SessionInfo sessionInfo)
        {
            // サブクエリ
            M10090引受区分名称? hikiukeKbn = dbContext.M10090引受区分名称s.FirstOrDefault(m =>
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
                (m.引受区分 == this.HikiukeKbn));

            string shuruiKbn = hikiukeKbn?.種類区分 ?? string.Empty;

            // 	(1) m_10210_単当共済金額用途テーブル、単当共済金額順位、単当共済金額を取得する。
            // 	(2) 取得した結果をドロップダウンリストの項目として設定する。
            //      m_10210_単当共済金額用途.課税単価区分 = 0、かつm_10210_単当共済金額用途.推奨フラグ = 1のレコードを順位0として
            //      取得したデータを順位0となるようにドロップダウンリストに設定。
            //      それ以降にm_10210_単当共済金額用途.課税単価区分 = 0のレコードを追加で設定
            SelectKyosaiKingakuList = new();
            SelectKyosaiKingakuList.AddRange(dbContext.M10210単当共済金額用途s.Where(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.年産 == sessionInfo.Nensan) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
                (m.課税単価区分 == "0") &&
                (m.推奨フラグ == "1") &&
                (m.種類区分 == shuruiKbn)
                )?.
                OrderBy(m => m.単当共済金額順位).
                Select(m => new SelectListItem($"0 {m.単当共済金額}", $"{m.単当共済金額順位}")));
            SelectKyosaiKingakuList.AddRange(dbContext.M10210単当共済金額用途s.Where(m =>
                (m.組合等コード == sessionInfo.KumiaitoCd) &&
                (m.年産 == sessionInfo.Nensan) &&
                (m.共済目的コード == sessionInfo.KyosaiMokutekiCd) &&
                (m.種類区分 == shuruiKbn)
                )?.
                OrderBy(m => m.単当共済金額順位).
                Select(m => new SelectListItem($"{m.単当共済金額順位} {m.単当共済金額}", $"{m.単当共済金額順位}")));
        }
    }
}