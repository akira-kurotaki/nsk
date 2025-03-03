using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CoreLibrary.Core.Validator
{
    /// <summary>
    /// 数値(符号有/小数有)チェック
    /// </summary>
    /// <remarks>
    /// 作成日：2017/11/09
    /// 作成者：Hiratake Minori
    /// </remarks>
    public class NumberSignDecAttribute : ValidationAttribute, IClientModelValidator
    {
        /// <summary>
        /// 最大桁数
        /// </summary>
        public int FullMaxLength { get; }

        /// <summary>
        /// 整数部の最大桁数
        /// </summary>
        public int IntMaxLength { get; }

        /// <summary>
        /// 小数部の最大桁数
        /// </summary>
        public int DecMaxLength { get; }

        /// <summary>
        /// コントラクター
        /// </summary>
        /// <param name="fullMaxLength">最大桁数</param>
        /// <param name="decMaxLength">小数部の最大桁数</param>
        public NumberSignDecAttribute(int fullMaxLength, int decMaxLength)
        {
            this.FullMaxLength = fullMaxLength;
            this.IntMaxLength = fullMaxLength - decMaxLength;
            this.DecMaxLength = decMaxLength;
            ErrorMessage = SystemMessageUtil.Get("ME00006");
        }

        /// <summary>
        /// エラーメッセージを整形
        /// </summary>
        /// <param name="name">エラーメッセージに埋め込み文字列（モデルのプロパティの表示名）</param>
        /// <returns>整形されたエラーメッセージ</returns>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
                                 ErrorMessageString, name, IntMaxLength, DecMaxLength, string.Empty);
        }

        /// <summary>
        /// 検証の実処理
        /// </summary>
        /// <param name="value">検証する入力値</param>
        /// <returns>検証結果（true：成功 / false：失敗）</returns>
        public override bool IsValid(object value)
        {
            return NumberUtil.IsNumberSignDec(value, IntMaxLength, DecMaxLength);
        }

        /// <summary>
        /// クライアントサイドに検証実行に必要な情報を準備する
        /// </summary>
        /// <param name="metadata">モデルのメタ情報</param>
        /// <param name="context">コントローラのコンテキスト</param>
        /// <returns>検証情報リスト</returns>
        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{
        //    var rule = new ModelClientValidationRule
        //    {
        //        ValidationType = "numbersigndec",
        //        ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
        //    };
        //    rule.ValidationParameters["intmaxlength"] = intMaxLength;
        //    rule.ValidationParameters["decmaxlength"] = decMaxLength;
        //    yield return rule;
        //}

        /// <summary>
        /// クライアント側モデルの検証
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val-numbersigndec", errorMessage);
            MergeAttribute(context.Attributes, "data-val-numbersigndec-intmaxlength", IntMaxLength.ToString());
            MergeAttribute(context.Attributes, "data-val-numbersigndec-decmaxlength", DecMaxLength.ToString());
        }

        /// <summary>
        /// 上のAddValidationメソッドで使うヘルパーメソッド
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }
    }
}