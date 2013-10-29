using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Root.Mvc.Helpers
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

        /// <summary>
        /// Renders the page to string.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="renderPageViewModel">The render page view model.</param>
        /// <returns>Renders page to string</returns>
        public static string RenderPageToString(this CmsControllerBase controller, RenderPageViewModel renderPageViewModel)
        {
            var htmlHelper = GetHtmlHelper(controller);

            return RenderRecursively(controller, renderPageViewModel, renderPageViewModel, htmlHelper);
        }

        /// <summary>
        /// Renders page to string recursively - going deep to master pages and layouts.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="currentModel">The model.</param>
        /// <param name="pageModel">The page model.</param>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <returns></returns>
        private static string RenderRecursively(CmsControllerBase controller, RenderPageViewModel currentModel, RenderPageViewModel pageModel, HtmlHelper htmlHelper)
        {
            if (currentModel.MasterPage != null)
            {
                var renderedMaster = RenderRecursively(controller, currentModel.MasterPage, pageModel, htmlHelper);

                foreach (var region in currentModel.Regions)
                {
                    var contentsBuilder = new StringBuilder();
                    var projections = currentModel.Contents.Where(c => c.RegionId == region.RegionId).OrderBy(c => c.Order).ToList();

                    using (new LayoutRegionWrapper(contentsBuilder, region, currentModel.AreRegionsEditable))
                    {
                        foreach (var projection in projections)
                        {
                            // Add Html
                            using (new RegionContentWrapper(contentsBuilder, projection, currentModel.CanManageContent && currentModel.AreRegionsEditable))
                            {
                                var content = projection.GetHtml(htmlHelper);
                                contentsBuilder.Append(content);
                            }
                        }
                    }

                    var html = contentsBuilder.ToString();
                    renderedMaster = DynamicLayoutHelper.ReplaceRegionHtml(region.RegionIdentifier, renderedMaster, html);
                }
                
                return renderedMaster;
            }

            // HACK: passing current page id to parent parent page, otherwise master page's id is used
            // TODO: remove of find more clean solution
            var newModel = new RenderPageViewModel(
                new Page
                    {
                        Id = pageModel.Id,
                        IsDeleted = currentModel.IsDeleted,
                        Version = currentModel.Version,
                        Title = currentModel.Title,
                        PageUrl = currentModel.PageUrl,
                        Status = currentModel.Status,
                        CreatedOn = currentModel.CreatedOn,
                        CreatedByUser = currentModel.CreatedByUser,
                        ModifiedOn = currentModel.ModifiedOn,
                        ModifiedByUser = currentModel.ModifiedByUser
                    })
                               {
                                   LayoutPath = currentModel.LayoutPath,
                                   MasterPage = currentModel.MasterPage,
                                   Contents = currentModel.Contents,
                                   Regions = currentModel.Regions,
                                   AreRegionsEditable = currentModel.AreRegionsEditable,
                                   CanManageContent = currentModel.CanManageContent,
                                   Options = currentModel.Options,
                                   Metadata = currentModel.Metadata,
                                   Stylesheets = currentModel.Stylesheets,
                                   JavaScripts = currentModel.JavaScripts,
                                   AccessRules = currentModel.AccessRules,
                                   RequireJsPath = currentModel.RequireJsPath,
                                   MainJsPath = currentModel.MainJsPath,
                                   Html5ShivJsPath = currentModel.Html5ShivJsPath,
                                   Bag = currentModel.Bag,
                                   IsReadOnly = currentModel.IsReadOnly,
                                   HasEditRole = currentModel.HasEditRole,
                                   SaveUnsecured = currentModel.SaveUnsecured,
                               };

            var renderedView = RenderViewToString(controller, "~/Areas/bcms-Root/Views/Cms/Index.cshtml", newModel);
            return renderedView;
        }

        /// <summary>
        /// Gets fake HTML helper.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <returns>Fake HTML helper</returns>
        private static HtmlHelper GetHtmlHelper(this Controller controller)
        {
            var viewContext = new ViewContext(controller.ControllerContext, new FakeView(), controller.ViewData, controller.TempData, TextWriter.Null);
            return new HtmlHelper(viewContext, new ViewPage());
        }

        /// <summary>
        /// Fake razor view
        /// </summary>
        private class FakeView : IView
        {
            public void Render(ViewContext viewContext, TextWriter writer)
            {
                throw new InvalidOperationException();
            }
        }
    }
}