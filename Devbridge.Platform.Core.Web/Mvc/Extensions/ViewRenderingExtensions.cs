using System.IO;
using System.Web.Mvc;

namespace Devbridge.Platform.Core.Web.Mvc.Extensions
{
    public static class ViewRenderingExtensions
    {
        /// <summary>
        /// Renders the view to string.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <param name="enableFormContext">if set to <c>true</c> enable form context.</param>
        /// <returns>View, rendered to string</returns>
        public static string RenderViewToString(this CoreControllerBase controller, string viewName, object model, bool enableFormContext = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");
            }

            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                if (enableFormContext && viewContext.FormContext == null)
                {
                    viewContext.FormContext = new FormContext();
                }

                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
