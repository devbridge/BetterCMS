using System.IO;
using System.Web.Mvc;

using Newtonsoft.Json;

namespace BetterCms.Core.Mvc.Binders
{
    public class JSONDataBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext != null 
                && controllerContext.HttpContext != null 
                && controllerContext.HttpContext.Request != null
                && controllerContext.HttpContext.Request.InputStream != null)
            {
                controllerContext.HttpContext.Request.InputStream.Position = 0;
                var json = new StreamReader(controllerContext.HttpContext.Request.InputStream).ReadToEnd();
                var type = bindingContext.ModelType;

                var model = JsonConvert.DeserializeObject(json, type);
                return model;
            }

            return null;
        }

    } 
}