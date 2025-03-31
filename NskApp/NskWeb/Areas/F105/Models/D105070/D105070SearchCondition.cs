using CoreLibrary.Core.Consts;
using CoreLibrary.Core.Validator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F105.Models.D105070
{
    /// <summary>
    /// 検索条件
    /// </summary>
    public class D105070SearchCondition
    {
        /// <summary>地番</summary>
        [Display(Name = "地番")]
        [ExceptGaiji]
        [WithinStringLength(20)]
        public string Chiban { get; set; } = string.Empty;
        /// <summary>耕地番号（開始）</summary>
        [Display(Name = "耕地番号（開始）")]
        [Numeric]
        [WithinDigitLength(5)]
        public string KouchiNoFrom { get; set; } = string.Empty;
        /// <summary>耕地番号（終了）</summary>
        [Display(Name = "耕地番号（終了）")]
        [Numeric]
        [WithinDigitLength(5)]
        public string KouchiNoTo { get; set; } = string.Empty;

        /// <summary>表示数</summary>
        [Display(Name = "表示数")]
        public int? DisplayCount { get; set; } = CoreConst.PAGE_SIZE;
        /// <summary>表示順キー１</summary>
        [Display(Name = "表示順")]
        public DisplaySortType? DisplaySort1 { get; set; }
        /// <summary>表示順１</summary>
        public CoreConst.SortOrder DisplaySortOrder1 { get; set; }
        /// <summary>表示順キー２</summary>
        [Display(Name = "表示順")]
        public DisplaySortType? DisplaySort2 { get; set; }
        /// <summary>表示順２</summary>
        public CoreConst.SortOrder DisplaySortOrder2 { get; set; }


        /// <summary>選択(局都道府県)</summary>
        [Display(Name = "局都道府県")]
        public bool CheckKyokuTodofuken { get; set; } = false;
        /// <summary>局都道府県</summary>
        [Display(Name = "局都道府県")]
        [Numeric]
        [WithinDigitLength(4)]
        public string IkkatsuKyokuTodofuken { get; set; } = string.Empty;
        /// <summary>選択(市町村)</summary>
        [Display(Name = "市町村")]
        public bool CheckShichoson { get; set; } = false;
        /// <summary>市町村</summary>
        [Display(Name = "市町村")]
        [Numeric]
        [WithinDigitLength(3)]
        public string IkkatsuShichoson { get; set; } = string.Empty;
        /// <summary>選択(大字)</summary>
        [Display(Name = "大字")]
        public bool CheckOoaza { get; set; } = false;
        /// <summary>大字（一括入力）</summary>
        [Display(Name = "大字（一括入力）")]
        [Numeric]
        [WithinDigitLength(8)]
        public string IkkatsuOoaza { get; set; } = string.Empty;
        /// <summary>選択(小字)</summary>
        [Display(Name = "小字")]
        public bool CheckKoaza { get; set; } = false;
        /// <summary>小字（一括入力）</summary>
        [Display(Name = "小字（一括入力）")]
        [Numeric]
        [WithinDigitLength(4)]
        public string IkkatsuKoaza { get; set; } = string.Empty;
        /// <summary>選択(地番)</summary>
        [Display(Name = "地番")]
        public bool CheckChiban { get; set; } = false;
        /// <summary>地番（一括入力）</summary>
        [Display(Name = "地番（一括入力）")]
        [Numeric]
        [WithinDigitLength(16)]
        public string IkkatsuChiban { get; set; } = string.Empty;
        /// <summary>選択(枝番)</summary>
        [Display(Name = "枝番")]
        public bool CheckEdaban { get; set; } = false;
        /// <summary>枝番（一括入力）</summary>
        [Display(Name = "枝番（一括入力）")]
        [Numeric]
        [WithinDigitLength(14)]
        public string IkkatsuEdaban { get; set; } = string.Empty;
        /// <summary>選択(子番)</summary>
        [Display(Name = "子番")]
        public bool CheckKoban { get; set; } = false;
        /// <summary>子番（一括入力）</summary>
        [Display(Name = "子番（一括入力）")]
        [Numeric]
        [WithinDigitLength(10)]
        public string IkkatsuKoban { get; set; } = string.Empty;
        /// <summary>選択(孫番)</summary>
        [Display(Name = "孫番")]
        public bool CheckMagoban { get; set; } = false;
        /// <summary>孫番（一括入力）</summary>
        [Display(Name = "孫番（一括入力）")]
        [Numeric]
        [WithinDigitLength(10)]
        public string IkkatsuMagoban { get; set; } = string.Empty;
        /// <summary>選択(RS区分)</summary>
        [Display(Name = "RS区分")]
        public bool CheckRsKbn { get; set; } = false;
        /// <summary>RS区分（一括入力）</summary>
        [Display(Name = "RS区分（一括入力）")]
        [Numeric]
        [WithinDigitLength(2)]
        public string IkkatsuRsKbn { get; set; } = string.Empty;

        /// <summary>
        /// 画面入力値をこのこのクラスに反映する
        /// </summary>
        /// <param name="src"></param>
        public void ApplyInput(D105070SearchCondition src)
        {
            this.Chiban = src.Chiban;
            this.KouchiNoFrom = src.KouchiNoFrom;
            this.KouchiNoTo = src.KouchiNoTo;
            this.DisplayCount = src.DisplayCount;
            this.DisplaySort1 = src.DisplaySort1;
            this.DisplaySortOrder1 = src.DisplaySortOrder1;
            this.DisplaySort2 = src.DisplaySort2;
            this.DisplaySortOrder2 = src.DisplaySortOrder2;
            this.CheckKyokuTodofuken = src.CheckKyokuTodofuken;
            this.IkkatsuKyokuTodofuken = src.IkkatsuKyokuTodofuken;
            this.CheckShichoson = src.CheckShichoson;
            this.IkkatsuShichoson = src.IkkatsuShichoson;
            this.CheckOoaza = src.CheckOoaza;
            this.IkkatsuOoaza = src.IkkatsuOoaza;
            this.CheckKoaza = src.CheckKoaza;
            this.IkkatsuKoaza = src.IkkatsuKoaza;
            this.CheckChiban = src.CheckChiban;
            this.IkkatsuChiban = src.IkkatsuChiban;
            this.CheckEdaban = src.CheckEdaban;
            this.IkkatsuEdaban = src.IkkatsuEdaban;
            this.CheckKoban = src.CheckKoban;
            this.IkkatsuKoban = src.IkkatsuKoban;
            this.CheckMagoban = src.CheckMagoban;
            this.IkkatsuMagoban = src.IkkatsuMagoban;
            this.CheckRsKbn = src.CheckRsKbn;
            this.IkkatsuRsKbn = src.IkkatsuRsKbn;
        }

        /// <summary>
        /// 表示順ドロップダウンリスト要素
        /// </summary>
        [Flags]
        public enum DisplaySortType
        {
            [Description("地番")]
            Chiban,
            [Description("耕地番号")]
            KouchBango,
        }
    }
}
