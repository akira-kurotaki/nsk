using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CoreLibrary.Core.Validator
{
    /// <summary>
    /// 数値(符号有/小数無)チェック
    /// </summary>
    /// <remarks>
    /// 作成日：2017/10/31
    /// 作成者：KAN RI
    /// </remarks>
    public class NumberSignAttribute : ValidationAttribute, IClientModelValidator
    {
        /// <summary>
        /// 整数の最大桁数
        /// </summary>
        public int MaxLength { get; }

        /// <summary>
        /// コントラクター
        /// </summary>
        /// <param name="intMaxLength">整数の最大桁数</param>
        public NumberSignAttribute(int intMaxLength)
        {
            this.MaxLength = intMaxLength;
            ErrorMessage = SystemMessageUtil.Get("ME00005");
        }

        /// <summary>
        /// エラーメッセージを整形
        /// </summary>
        /// <param name="name">エラーメッセージに埋め込み文字列（モデルのプロパティの表示名）</param>
        /// <returns>整形されたエラーメッセージ</returns>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
                                 ErrorMessageString, name, MaxLength, string.Empty);
        }

        /// <summary>
        /// 検証の実処理
        /// </summary>
        /// <param name="value">検証する入力値</param>
        /// <returns>検証結果（true：成功 / false：失敗）</returns>
        public override bool IsValid(object value)
        {
            return NumberUtil.IsNumberSign(value, MaxLength);
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
        //        ValidationType = "numbersign",
        //        ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
        //    };
        //    rule.ValidationParameters["intmaxlength"] = intMaxLength;
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
            MergeAttribute(context.Attributes, "data-val-numbersign", errorMessage);
            MergeAttribute(context.Attributes, "data-val-numbersign-intmaxlength", MaxLength.ToString());
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