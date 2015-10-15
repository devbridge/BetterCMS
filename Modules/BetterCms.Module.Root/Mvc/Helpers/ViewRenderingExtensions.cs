using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;
using BetterCms.Module.Root.ViewModels.Cms;

using BetterModules.Core.Web.Mvc.Extensions;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class ViewRenderingExtensions
    {
        /// <summary>
        /// Renders the page to string.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="renderPageViewModel">The render page view model.</param>
        /// <returns>Renders page to string</returns>
        public static string RenderPageToString(this CmsControllerBase controller, RenderPageViewModel renderPageViewModel)
        {
            var htmlHelper = GetHtmlHelper(controller);

            return RenderRecursively(controller, renderPageViewModel, renderPageViewModel, htmlHelper).ToString();
        }

        /// <summary>
        /// Renders page to string recursively - going deep to master pages and layouts.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="currentModel">The model.</param>
        /// <param name="pageModel">The page model.</param>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <returns>
        /// String builder with updated data
        /// </returns>
        private static StringBuilder RenderRecursively(CmsControllerBase controller, RenderPageViewModel currentModel, RenderPageViewModel pageModel, HtmlHelper htmlHelper)
        {
            if (currentModel.MasterPage != null)
            {
                var renderedMaster = RenderRecursively(controller, currentModel.MasterPage, pageModel, htmlHelper);

                var pageHtmlHelper = new PageHtmlRenderer.PageHtmlRenderer(renderedMaster, pageModel);
                var contentHtmlHelper = new PageContentRenderHelper(htmlHelper);

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
                                // Pass current model as view data model
                                htmlHelper.ViewData.Model = pageModel;

                                contentsBuilder = contentHtmlHelper.AppendHtml(contentsBuilder, projection, currentModel);
                            }
                        }
                    }

                    // Insert region to master page
                    var html = contentsBuilder.ToString();
                    pageHtmlHelper.ReplaceRegionHtml(region.RegionIdentifier, html);
                }

                if (currentModel.AreRegionsEditable)
                {
                    pageHtmlHelper.ReplaceRegionRepresentationHtml();
                }
                renderedMaster = pageHtmlHelper.GetReplacedHtml();

                if (pageModel == currentModel)
                {
                    renderedMaster = contentHtmlHelper.GetReplacedInvisibleRegions(pageModel, renderedMaster);
                }

                return renderedMaster;
            }

            var newModel = currentModel.Clone();
            newModel.Id = pageModel.Id;
            newModel.PageUrl = pageModel.PageUrl;
            newModel.Title = pageModel.Title;
            newModel.MetaTitle = pageModel.MetaTitle;
            newModel.MetaKeywords = pageModel.MetaKeywords;
            newModel.MetaDescription = pageModel.MetaDescription;
            newModel.RenderingPage = pageModel;
            newModel.Metadata = pageModel.Metadata;
            newModel.IsReadOnly = pageModel.IsReadOnly;
            newModel.CreatedOn = pageModel.CreatedOn;
            newModel.ModifiedOn = pageModel.ModifiedOn;
            newModel.CreatedByUser = pageModel.CreatedByUser;
            newModel.ModifiedByUser = pageModel.ModifiedByUser;
            newModel.IsMasterPage = pageModel.IsMasterPage;
            newModel.LanguageCode = pageModel.LanguageCode;
            newModel.LanguageId = pageModel.LanguageId;

            PopulateCollections(newModel, pageModel);

            var renderedView = controller.RenderViewToString("~/Areas/bcms-Root/Views/Cms/Index.cshtml", newModel);
            return new StringBuilder(renderedView);
        }

        /// <summary>
        /// Populates the collections.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="source">The source.</param>
        private static void PopulateCollections(RenderPageViewModel destination, RenderPageViewModel source)
        {
            if (source.MasterPage != null)
            {
                PopulateCollections(destination, source.MasterPage);
            }

            if (source.JavaScripts != null)
            {
                if (destination.JavaScripts == null)
                {
                    destination.JavaScripts = new List<IJavaScriptAccessor>();
                }
                foreach (var js in source.JavaScripts)
                {
                    if (!destination.JavaScripts.Contains(js))
                    {
                        destination.JavaScripts.Add(js);
                    }
                }
            }

            if (source.Stylesheets != null)
            {
                if (destination.Stylesheets == null)
                {
                    destination.Stylesheets = new List<IStylesheetAccessor>();
                }
                foreach (var css in source.Stylesheets)
                {
                    if (!destination.Stylesheets.Contains(css))
                    {
                        destination.Stylesheets.Add(css);
                    }
                }
            }

            if (source.OptionsAsDictionary != null)
            {
                if (destination.OptionsAsDictionary == null)
                {
                    destination.OptionsAsDictionary = new Dictionary<string, IOptionValue>();
                }
                foreach (var option in source.OptionsAsDictionary)
                {
                    destination.OptionsAsDictionary[option.Key] = option.Value;
                }
            }
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