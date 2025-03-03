using CoreLibrary.Core.Base;
using CoreLibrary.Core.Validator;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Areas.F990.Models.D990005
{
    /// <summary>
    /// 入力チェック用のビューモデル
    /// </summary>
    public class D990005Model : CoreViewModel
    {
        [DisplayName("数字")]
        [Required]
        [Numeric]
        public string Numeric { get; set; }

        [DisplayName("数値(符号無/小数無)")]
        [NumberPositive(8)]
        public int? NumberPositive { get; set; }

        [DisplayName("数値(符号無/小数有)")]
        [NumberDec(8, 2)]
        public double? NumberDec { get; set; }

        [DisplayName("数値(符号有/小数無)")]
        [NumberSign(5)]
        public int? NumberSign { get; set; }

        [DisplayName("数値(符号有/小数有)")]
        [NumberSignDec(5, 1)]
        public double? NumberSignDec { get; set; }

        [DisplayName("半角英数")]
        [HalfWidthAlphaNum]
        public string HalfWidthAlphaNum { get; set; }

        [DisplayName("半角英数記号")]
        [HalfWidthAlphaNumSymbol]
        public string HalfWidthAlphaNumSymbol { get; set; }

        [Display(Name = "メールアドレス")]
        [EmailAddress]
        [StringLength(50)]
        public string EmailAddress { get; set; }

        [DisplayName("日付")]
        [DateGYMD]
        public DateTime? DateGYMD { get; set; }

        [DisplayName("ひらがな")]
        [Hiragana]
        public string Hiragana { get; set; }

        [DisplayName("全角カタカナ")]
        [FullWidthKatakana]
        public string FullWidthKatakana { get; set; }

        [DisplayName("半角カタカナ")]
        [HalfWidthKatakana]
        public string HalfWidthKatakana { get; set; }

        [DisplayName("外字以外")]
        [ExceptGaiji]
        public string ExceptGaiji { get; set; }

        [DisplayName("文字数以内")]
        [HalfWidthAlphaNumSymbol]
        [WithinStringLength(5)]
        public string WithinStringLength { get; set; }

        [DisplayName("桁数以内")]
        [Numeric]
        [WithinDigitLength(5)]
        public string WithinNumericLength { get; set; }

        [DisplayName("文字全桁")]
        [ExceptGaiji]
        [FullStringLength(5)]
        public string FullStringLength { get; set; }

        [DisplayName("数字全桁")]
        [Numeric]
        [FullDigitLength(5)]
        public string FullNumericLength { get; set; }

        [DisplayName("禁止文字チェック")]
        public string ProhibitWord { get; set; }

    }
}