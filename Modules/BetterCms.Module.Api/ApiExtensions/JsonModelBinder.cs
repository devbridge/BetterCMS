using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                        var attemptedValue = value.AttemptedValue;
                        var value1 = ServiceStack.Text.JsonSerializer.DeserializeFromString(attemptedValue, propertyInfo.PropertyType);
                        propertyInfo.SetMethod().Invoke(instance, new[] { value1 });
                    }
                }
                else if (propertyName == "user")
                {
                    var value = bindingContext.ValueProvider.GetValue(propertyInfo.Name.ToLowerInvariant());
                    if (value != null)
                    {
                        var attemptedValue = value.AttemptedValue;
                        var value1 = ServiceStack.Text.JsonSerializer.DeserializeFromString(attemptedValue, propertyInfo.PropertyType);
                        propertyInfo.SetMethod().Invoke(instance, new[] { value1 });
                    }
                }
                else
                {
                    var value = bindingContext.ValueProvider.GetValue(propertyInfo.Name.ToLowerInvariant());
                    if (value != null)
                    {
                        var attemptedValue = value.AttemptedValue;
                        var value1 = TypeSerializer.DeserializeFromString(attemptedValue, propertyInfo.PropertyType);
                        propertyInfo.SetMethod().Invoke(instance, new[] { value1 });
                    }
                }
            }

            bindingContext.Model = instance;

            bindingContext.ValidationNode.Validate(actionContext);

            return true;
        }
    }
}