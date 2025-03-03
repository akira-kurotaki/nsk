using NskWeb.Resources;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace NskWeb.Common.Extensions
{
    /// <summary>
    /// 必須エラーメッセージをカスタマイズするためのアダプタークラス
    /// 
    /// 作成日：2017/12/21
    /// 作成者：KAN RI
    /// </summary>
    public class CustomRequiredAttributeAdapter : AttributeAdapterBase<RequiredAttribute>
    {
        public CustomRequiredAttributeAdapter(RequiredAttribute attribute, IStringLocalizer? stringLocalizer)
            : base(attribute, stringLocalizer)
        {
            // 作成したリソースファイルを指定
            attribute.ErrorMessageResourceType = typeof(Messages);
            // リソースファイルに記述した名前を指定
            attribute.ErrorMessageResourceName = "PropertyValueRequired";
            attribute.ErrorMessage = null;
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-required", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }
}