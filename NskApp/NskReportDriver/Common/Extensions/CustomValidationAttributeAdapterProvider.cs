using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace NskReportDriver.Common.Extensions
{
    public class CustomValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider baseProvider = new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute is RequiredAttribute)
            {
                return new CustomRequiredAttributeAdapter(attribute as RequiredAttribute, stringLocalizer);
            }
            else
            {
                return baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
            }
        }
    }
}
