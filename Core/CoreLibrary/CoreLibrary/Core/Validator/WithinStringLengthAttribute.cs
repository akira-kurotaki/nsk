﻿using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CoreLibrary.Core.Validator
{
    /// <summary>
    /// 文字数以内入力チェック
    /// </summary>
    /// <remarks>
    /// 作成日：2017/12/15
    /// 作成者：KAN RI
    /// </remarks>
    public class WithinStringLengthAttribute : ValidationAttribute, IClientModelValidator
    {
        /// <summary>
        /// 最大桁数
        /// </summary>
        public int MaxLength { get; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="maxLength">最大桁数</param>
        public WithinStringLengthAttribute(int maxLength)
        {
            this.MaxLength = maxLength;
            ErrorMessage = SystemMessageUtil.Get("ME00014");
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
            if (value == null || (string.IsNullOrEmpty(value.ToString())))
            {
                return true;
            }

            if (MaxLength < value.ToString().Length)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// クライアント側モデルの検証
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val-withinstringlength", errorMessage);
            MergeAttribute(context.Attributes, "data-val-withinstringlength-maxlength", MaxLength.ToString());
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