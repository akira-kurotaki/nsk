using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;

namespace CoreLibrary.Core.ModelBinder
{
    public class ProhibitWordModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            var value = valueProviderResult.FirstValue;
            var modelType = bindingContext.ModelType;

            if (modelType == typeof(string))
            {
                var reg = new Regex(@"\t|""|<[a-zA-Z/?!]|&#", RegexOptions.Compiled);

                if (reg.IsMatch(value.ToString()))
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, "タブ文字、\"、<英字、</、<?、<!、&#は使用できません。");
                }
            }

            bindingContext.Result = ModelBindingResult.Success(string.IsNullOrEmpty(value) ? null : value);
            return Task.CompletedTask;
        }
    }
}
