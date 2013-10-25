using System.IO;
using System.Web.Mvc;

using BetterCms.Core.Web.DynamicLayouts;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Root.Mvc
{
    public static class ViewRenderingExtensions
    {
        public static string RenderViewToString(this CmsControllerBase controller, string viewName, object model, bool enableFormContext = false)
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

        public static string RenderPageToString(this CmsControllerBase controller, RenderPageViewModel renderPageViewModel)
        {
            return RenderRecursively(controller, renderPageViewModel);
        }

        private static string RenderRecursively(CmsControllerBase controller, RenderPageViewModel model)
        {
            if (model.MasterPage != null)
            {
                var renderedMaster = RenderRecursively(controller, model.MasterPage);

                using (var sw = new StringWriter())
                {
                    var viewData = new ViewDataDictionary
                    {
                        Model = model
                    };

                    DynamicHtmlLayoutContentsContainer.Push(model.Id, renderedMaster);

                    // Create view
                    var context = controller.ControllerContext;

                    var masterVirtualPath = DynamicHtmlLayoutContentsContainer.CreateMasterVirtualPath(model.Id, model.Version);

                    var viewResult = ViewEngines.Engines.FindView(context, "~/Areas/bcms-Root/Views/Cms/MasterPage.cshtml", masterVirtualPath);
                    var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);

                    viewResult.View.Render(viewContext, sw);
                    viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

                    var html = sw.GetStringBuilder().ToString();
                    return html;
                }
            }

            var renderedView = RenderViewToString(controller, "~/Areas/bcms-Root/Views/Cms/Index.cshtml", model);
            return renderedView;
        }
    }
}