using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CoreLibrary.Core.ModelBinder
{
    public class ProhibitWordModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(string))
            {
                return new ProhibitWordModelBinder();
            }

            return null;
        }
    }
}
