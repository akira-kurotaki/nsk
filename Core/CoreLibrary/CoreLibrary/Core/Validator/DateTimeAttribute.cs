using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CoreLibrary.Core.Validator
{
    /// <summary>
    /// 日時のチェック
    /// </summary>
    /// <remarks>
    /// 作成日：2024/12/02
    /// 作成者：楊 I
    /// </remarks>
    public class DateTimeAttribute : ValidationAttribute, IClientModelValidator
    {
        /// <summary>
        /// フォーマット
        /// </summary>
        private string format;

        /// <summary>
        /// コントラクター
        /// </summary>
        public DateTimeAttribute()
        {
            ErrorMessage = SystemMessageUtil.Get("ME80013");
        }

        /// <summary>
        /// コントラクター
        /// </summary>
        public DateTimeAttribute(string format)
        {
            this.format = format;
            ErrorMessage = SystemMessageUtil.Get("ME80013");
        }

        /// <summary>
        /// エラーメッセージを整形
        /// </summary>
        /// <param name="name">エラーメッセージに埋め込み文字列（モデルのプロパティの表示名）</param>
        /// <returns>整形されたエラーメッセージ</returns>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
                                 ErrorMessageString, name, string.IsNullOrEmpty(format) ? string.Empty: "（" + format + "）");
        }

        /// <summary>
        /// 検証の実処理
        /// </summary>
        /// <param name="value">検証する入力値</param>
        /// <returns>検証結果（true：成功 / false：失敗）</returns>
        public override bool IsValid(object value)
        {
            // value は常にnullなので検証できないので、trueを返す。
            return true;
        }

        /// <summary>
        /// バリデーション（サーバーサイド）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }

        /// <summary>
        /// クライアント側モデルの検証
        /// </summary>
        /// <param name="context"></param>
        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val-datetime", errorMessage);
            MergeAttribute(context.Attributes, "data-val-datetime-format", (format ?? string.Empty));
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