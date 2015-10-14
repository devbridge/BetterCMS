using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BetterCms.Module.Api.ApiExtensions
{
    public class UrlPopulatorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var model = actionContext.ActionArguments.Values.FirstOrDefault();
            if (model == null) return;
            var modelType = model.GetType();
            var routeParams = actionContext.ControllerContext.RouteData.Values;

            foreach (var key in routeParams.Keys.Where(k => k != "controller"))
            {
                var prop = modelType.GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (prop != null)
                {
                    var descriptor = TypeDescriptor.GetConverter(prop.PropertyType);
                    if (descriptor.CanConvertFrom(typeof(string)))
                    {
                        prop.SetValue(model, descriptor.ConvertFromString(routeParams[key] as string));
                    }
                }
            }
        }
    }
}