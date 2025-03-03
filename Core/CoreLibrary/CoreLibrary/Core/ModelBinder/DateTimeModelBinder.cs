using CoreLibrary.Core.Utility;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CoreLibrary.Core.ModelBinder
{
    /// <summary>
    /// 新元号対応モデルバインダ―
    /// 
    /// クライアントから送信された値の和暦から西暦への変換はMicrosoftのライブラリより行われる。
    /// H30/01/01 → 2018/01/01
    /// Microsoftのライブラリ提供前に新元号変換を実現するため当クラスを使用する。 
    /// 
    /// このモデルバインダーを利用するにはGlobal.asax のApplication_Startに下記を追加する
    ///  ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
    ///  ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
    /// </summary>
    /// <remarks>
    /// 作成日：2018/05/10
    /// 作成者：MATSUBAYASHI Atsushi
    /// </remarks>
    public class DateTimeModelBinder : IModelBinder
    {
        //public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        //{
        //    var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        //    if (value != null && value.AttemptedValue != null && value.AttemptedValue != "")
        //    {
        //        DateTime parsedDate;
        //        if (DateUtil.TryParseJapaneseDate(value.AttemptedValue, out parsedDate))
        //        {
        //            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, value);
        //            return parsedDate;
        //        }
        //        else
        //        {
        //            return base.BindModel(controllerContext, bindingContext);
        //        }
        //    }
        //    else
        //    {
        //        return base.BindModel(controllerContext, bindingContext);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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

            if (!string.IsNullOrEmpty(value))
            {
                DateTime parsedDate;
                if (DateUtil.TryParseJapaneseDate(value, out parsedDate))
                {
                    bindingContext.Result = ModelBindingResult.Success(parsedDate);
                    return Task.CompletedTask;
                }
                else
                {
                    return Task.CompletedTask;
                }
            }
            else
            {
                return Task.CompletedTask;
            }
        }
    }
}