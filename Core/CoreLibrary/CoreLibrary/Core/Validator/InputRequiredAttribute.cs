using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CoreLibrary.Core.Validator
{
    /// <summary>
    /// 入力必須チェック
    /// </summary>
    /// <remarks>
    /// 作成日：2018/01/26
    /// 作成者：Rou I
    /// </remarks>
    public class InputRequiredAttribute : ValidationAttribute, IClientModelValidator
    {
        /// <summary>
        /// コントラクター
        /// </summary>
        public InputRequiredAttribute()
        {
            ErrorMessage = SystemMessageUtil.Get("ME00001");
        }

        /// <summary>
        /// エラーメッセージを整形
        /// </summary>
        /// <param name="name">エラーメッセージに埋め込み文字列（モデルのプロパティの表示名）</param>
        /// <returns>整形されたエラーメッセージ</returns>
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
                     ErrorMessageString, name, string.Empty);
        }

        /// <summary>
        /// 検証の実処理
        /// </summary>
        /// <param name="value">検証する入力値</param>
        /// <returns>検証結果（true：成功 / false：失敗）</returns>
        public override bool IsValid(object value)
        {
            if ((value == null) || (string.IsNullOrEmpty(value.ToString())))
            {
                return false;
            }
            return true;
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
        //        ValidationType = "inputrequired",
        //        ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
        //    };
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
            MergeAttribute(context.Attributes, "data-val-inputrequired", errorMessage);
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