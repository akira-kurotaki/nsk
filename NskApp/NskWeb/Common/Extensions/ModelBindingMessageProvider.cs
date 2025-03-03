using NskWeb.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace NskWeb.Common.Extensions
{
    public static class ModelBindingMessageProvider
    {
        /// <summary>
        /// モデルバインディングのエラーメッセージを設定する。
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureModelBindingMessages(this IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                // メッセージ生成用のラムダ式。必要に応じて使用すること。
                Func<string, string, string> f1 = (f, a1) => string.Format(f, a1);
                Func<string, string, string, string> f2 = (f, a1, a2) => string.Format(f, a1, a2);

                DefaultModelBindingMessageProvider provider = options.ModelBindingMessageProvider;
                // モデルバインディングのエラーメッセージ
                provider.SetAttemptedValueIsInvalidAccessor((s1, s2) => $"値 '{s1}' は {s2} に対して無効です。");
                provider.SetValueIsInvalidAccessor((s) => $"値 '{s}' は無効です。");
                provider.SetValueMustBeANumberAccessor((x) => f1(Messages.FieldMustBeNumeric, x));

                // DefaultModelBindingMessageProviderには以下のメソッドがある。必要に応じてメッセージを設定すること。
                // 以下を参照
                // https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.modelbinding.metadata.defaultmodelbindingmessageprovider?view=aspnetcore-8.0
                //provider.SetAttemptedValueIsInvalidAccessor((x, y) =>
                //    f2(Resource.ModelBinding_AttemptedValueIsInvalid, x, y));
                //provider.SetMissingBindRequiredValueAccessor((x) =>
                //    f1(Resource.ModelBinding_MissingBindRequiredValue, x));
                //provider.SetMissingKeyOrValueAccessor(() =>
                //    Resource.ModelBinding_MissingKeyOrValue);
                //provider.SetMissingRequestBodyRequiredValueAccessor(() =>
                //    Resource.ModelBinding_MissingRequestBodyRequiredValue);
                //provider.SetNonPropertyAttemptedValueIsInvalidAccessor((x) =>
                //    f1(Resource.ModelBinding_NonPropertyAttemptedValueIsInvalid, x));
                //provider.SetNonPropertyUnknownValueIsInvalidAccessor(() =>
                //    Resource.ModelBinding_NonPropertyUnknownValueIsInvalid);
                //provider.SetNonPropertyValueMustBeANumberAccessor(() =>
                //    Resource.ModelBinding_NonPropertyValueMustBeANumber);
                //provider.SetUnknownValueIsInvalidAccessor((x) =>
                //    f1(Resource.ModelBinding_UnknownValueIsInvalid, x));
                //provider.SetValueIsInvalidAccessor((x) =>
                //    f1(Resource.ModelBinding_ValueIsInvalid, x));
                //provider.SetValueMustBeANumberAccessor((x) =>
                //    f1(Resource.ModelBinding_ValueMustBeANumber, x));
                //provider.SetValueMustNotBeNullAccessor((x) =>
                //    f1(Resource.ModelBinding_ValueMustNotBeNull, x));
            });

            return services;
        }
    }
}
