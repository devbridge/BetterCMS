using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

using ServiceStack.Text;

namespace BetterCms.Module.Api.ApiExtensions
{
    public class JsonModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var instance = bindingContext.ModelType.CreateInstance();
            foreach (var propertyInfo in bindingContext.ModelType.GetProperties())
            {
                var propertyName = propertyInfo.Name.ToLowerInvariant();

                if (propertyName == "data")
                {
                    var value = bindingContext.ValueProvider.GetValue(propertyInfo.Name.ToLowerInvariant());
                    if (value != null)
                    {
                        var deserializedValue = JsonSerializer.DeserializeFromString(value.AttemptedValue, propertyInfo.PropertyType);
                        propertyInfo.SetMethod().Invoke(instance, new[] { deserializedValue });
                    }
                }
                else if (propertyName == "user")
                {
                    var value = bindingContext.ValueProvider.GetValue(propertyInfo.Name.ToLowerInvariant());
                    if (value != null)
                    {
                        var deserializedValue = JsonSerializer.DeserializeFromString(value.AttemptedValue, propertyInfo.PropertyType);
                        propertyInfo.SetMethod().Invoke(instance, new[] { deserializedValue });
                    }
                }
                else
                {
                    var value = bindingContext.ValueProvider.GetValue(propertyInfo.Name.ToLowerInvariant());
                    if (value != null)
                    {
                        var deserializedValue = TypeSerializer.DeserializeFromString(value.AttemptedValue, propertyInfo.PropertyType);
                        propertyInfo.SetMethod().Invoke(instance, new[] { deserializedValue });
                    }
                }
            }

            bindingContext.Model = instance;

            bindingContext.ValidationNode.Validate(actionContext);

            return true;
        }
    }
}