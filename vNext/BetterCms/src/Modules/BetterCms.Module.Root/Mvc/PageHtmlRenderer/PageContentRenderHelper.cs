using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.ViewModels.Cms;
using BetterCms.Module.Root.ViewModels.Content;

using HtmlAgilityPack;

using NHibernate.Linq;

namespace BetterCms.Module.Root.Mvc.PageHtmlRenderer
{
    public class PageContentRenderHelper
    {
        /// <summary>
        /// The regex pattern for to find all child widgets in the content
        /// </summary>
        public const string ChildWidgetRegexPattern = "<widget[^>]*>.*?<\\/widget>";

        /// <summary>
        /// The invisible regions placeholder
        /// </summary>
        public const string InvisibleRegionsPlaceholder = "{{BetterCmsInvisibleRegionsPlaceholder}}";

        /// <summary>
        /// The widget identifier attribute name
        /// </summary>
        public const string WidgetIdAttributeName = "data-id";

        /// <summary>
        /// The widget assignment identifier attribute name
        /// </summary>
        public const string WidgetAssignmentIdAttributeName = "data-assign-id";

        /// <summary>
        /// The HTML helper
        /// </summary>
        private readonly HtmlHelper htmlHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageContentRenderHelper"/> class.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        public PageContentRenderHelper(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        /// <summary>
        /// Appends the HTML with HTML, rendered by content projection.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="projection">The projection.</param>
        /// <param name="pageModel">The page model.</param>
        /// <returns></returns>
        public StringBuilder AppendHtml(StringBuilder stringBuilder, PageContentProjection projection, RenderPageViewModel pageModel)
        {
            var renderingPageModel = pageModel.RenderingPage ?? pageModel;
            var content = projection.GetHtml(htmlHelper);
            if (!renderingPageModel.RenderedPageContents.Contains(projection.PageContentId))
            {
                renderingPageModel.RenderedPageContents.Add(projection.PageContentId);
            }

            var childrenContents = projection.GetChildProjections() ?? new List<ChildContentProjection>();
            var parsedWidgets = ParseWidgetsFromHtml(content).Distinct();

            var availableWidgets = childrenContents.Where(cc => parsedWidgets.Any(id => id.AssignmentIdentifier == cc.AssignmentIdentifier));
            foreach (var childProjection in availableWidgets)
            {
                var model = parsedWidgets.First(w => w.AssignmentIdentifier == childProjection.AssignmentIdentifier);
                var replaceWhat = model.Match.Value;
                var replaceWith = AppendHtml(new StringBuilder(), childProjection, renderingPageModel).ToString();

                content = content.Replace(replaceWhat, replaceWith);
            }

            // Widgets, which has no access (e.g. widgets with draft status for public users)
            var invisibleWidgets = parsedWidgets.Where(id => childrenContents.All(cc => cc.AssignmentIdentifier != id.AssignmentIdentifier));
            foreach (var model in invisibleWidgets)
            {
                var replaceWhat = model.Match.Value;
                var replaceWith = string.Empty;

                content = content.Replace(replaceWhat, replaceWith);

            }

            // Add child contents in the master page to child region is possible only if content is widget.
            // If content is regular HTML content, it works as master page contents, and contents may be added only in the child page
            // If page is for preview, doesn't rendering children regions
            if ((!renderingPageModel.IsMasterPage || projection.Content is IChildRegionContainer) && !pageModel.IsPreviewing)
            {
                content = AppendHtmlWithChildRegionContens(content, projection, renderingPageModel);
            }

            stringBuilder.Append(content);

            return stringBuilder;
        }

        public static List<ChildContentModel> ParseWidgetsFromHtml(string searchIn, bool throwException = false)
        {
            if (string.IsNullOrWhiteSpace(searchIn))
            {
                return new List<ChildContentModel>(0);
            }

            var result = new List<ChildContentModel>();

            var matches = Regex.Matches(searchIn, ChildWidgetRegexPattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                var model = new ChildContentModel();
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(match.Value);

                var widgetNode = htmlDocument.DocumentNode.Descendants("widget").First();
                var widgetIdAttribute = widgetNode.Attributes.FirstOrDefault(a => a.Name == WidgetIdAttributeName);
                Guid widgetId;
                if (widgetIdAttribute == null || string.IsNullOrWhiteSpace(widgetIdAttribute.Value) || !Guid.TryParse(widgetIdAttribute.Value, out widgetId))
                {
                    if (throwException)
                    {
                        var message = RootGlobalization.ChildContent_FailedToParseWidgetId_Message;
                        throw new ValidationException(() => message, message);
                    }

                    continue;
                }
                model.WidgetId = widgetId;
                model.Title = htmlDocument.DocumentNode.InnerText;
                model.WidgetHtmlNode = widgetNode;
                model.Match = match;

                var assignmentIdAttribute = widgetNode.Attributes.FirstOrDefault(a => a.Name == WidgetAssignmentIdAttributeName);
                Guid assignmentId;
                if (assignmentIdAttribute == null || string.IsNullOrWhiteSpace(assignmentIdAttribute.Value) || !Guid.TryParse(assignmentIdAttribute.Value, out assignmentId))
                {
                    if (throwException)
                    {
                        var message = RootGlobalization.ChildContent_FailedToParseAssignmentId_MEssage;
                        throw new ValidationException(() => message, message);
                    }

                    continue;
                }
                model.AssignmentIdentifier = assignmentId;

                result.Add(model);
            }

            return result;
        }

        private string AppendHtmlWithChildRegionContens(string html, PageContentProjection projection, RenderPageViewModel pageModel)
        {
            // Render contents from children regions
            var childRegionContents = projection.GetChildRegionContentProjections() ?? new List<PageContentProjection>();
            if (projection.Content is IChildRegionContainer
                && projection.Content.ContentRegions != null
                && projection.Content.ContentRegions.Any())
            {
                var stringBuilder = new StringBuilder(html);
                var pageHtmlHelper = new PageHtmlRenderer(stringBuilder, pageModel);

                foreach (var region in projection.Content.ContentRegions.Distinct())
                {
                    var contentsBuilder = new StringBuilder();
                    var regionModel = new PageRegionViewModel
                        {
                            RegionId = region.Region.Id,
                            RegionIdentifier = region.Region.RegionIdentifier
                        };
                    var childRegionContentProjections = childRegionContents.Where(c => c.RegionId == regionModel.RegionId).OrderBy(c => c.Order).ToList();

                    var canEditRegion = projection.PageId == pageModel.Id && pageModel.AreRegionsEditable;
                    using (new LayoutRegionWrapper(contentsBuilder, regionModel, canEditRegion))
                    {
                        foreach (var childRegionContentProjection in childRegionContentProjections)
                        {
                            // Add Html
                            using (new RegionContentWrapper(contentsBuilder, childRegionContentProjection, pageModel.CanManageContent && canEditRegion))
                            {
                                // Pass current model as view data model
                                var modelBefore = htmlHelper.ViewData.Model;
                                htmlHelper.ViewData.Model = pageModel;

                                contentsBuilder = AppendHtml(contentsBuilder, childRegionContentProjection, pageModel);

                                // Restore model, which was before changes
                                htmlHelper.ViewData.Model = modelBefore;
                            }
                        }
                    }

                    // Insert region to master page
                    var regionHtml = contentsBuilder.ToString();
                    pageHtmlHelper.ReplaceRegionHtml(regionModel.RegionIdentifier, regionHtml);
                }

                return pageHtmlHelper.GetReplacedHtml().ToString();
            }

            return html;
        }

        /// <summary>
        /// Renders the invisible regions:.
        /// - Layout regions:
        /// -- When switching from layout A to layout B, and layout B has nor regions, which were in layout A
        /// -- When layout regions are deleted in Site Settings -> Page Layouts -> Templates
        /// - Widget regions:
        /// -- When region was deleted from the widget, and page has a content, assigned to that region
        /// </summary>
        /// <param name="model">The  model.</param>
        /// <returns></returns>
        public string RenderInvisibleRegions(RenderPageViewModel model)
        {
            var renderingModel = model;
            if (model.RenderingPage != null)
            {
                renderingModel = model.RenderingPage;
            }

            if (!renderingModel.CanManageContent)
            {
                return null;
            }

            var contentsBuilder = new StringBuilder();
            var invisibleContentProjections = CollectInvisibleRegionsRecursively(renderingModel, renderingModel.Contents, new List<InvisibleContentProjection>());
            invisibleContentProjections.Where(i => i.Parent != null && i.IsInvisible && !i.Parent.IsInvisible).ForEach(i => i.Parent = null);
            contentsBuilder = RenderInvisibleRegionsRecursively(contentsBuilder, invisibleContentProjections.Where(i => i.IsInvisible));

            var html = contentsBuilder.ToString();
            if (!string.IsNullOrWhiteSpace(html))
            {
                return html;
            }

            return null;
        }

        public StringBuilder GetReplacedInvisibleRegions(RenderPageViewModel model, StringBuilder html)
        {
            var invisibleRegionsHtml = RenderInvisibleRegions(model);

            return html.Replace(InvisibleRegionsPlaceholder, invisibleRegionsHtml);
        }

        private StringBuilder RenderInvisibleRegionsRecursively(StringBuilder contentsBuilder,
            IEnumerable<InvisibleContentProjection> invisibleContentProjections, InvisibleContentProjection parent = null)
        {
            foreach (var group in invisibleContentProjections.Where(i => i.Parent == parent).GroupBy(i => i.ContentProjection.RegionId))
            {
                var regionModel = new PageRegionViewModel { RegionId = group.Key, RegionIdentifier = group.First().ContentProjection.RegionIdentifier };
                using (new LayoutRegionWrapper(contentsBuilder, regionModel, true, true))
                {
                    foreach (var projection in group)
                    {
                        using (new RegionContentWrapper(contentsBuilder, projection.ContentProjection, true, true))
                        {
                            RenderInvisibleRegionsRecursively(contentsBuilder, invisibleContentProjections, projection);
                        }
                    }
                }

            }

            return contentsBuilder;
        }

        private List<InvisibleContentProjection> CollectInvisibleRegionsRecursively(
            RenderPageViewModel renderingPageModel,
            IEnumerable<PageContentProjection> contentProjections,
            List<InvisibleContentProjection> invisibleContentProjections,
            InvisibleContentProjection parentContentProjection = null)
        {
            // Add all invisible contents to the invisible list
            foreach (var projection in contentProjections)
            {
                var invisibleProjection = new InvisibleContentProjection
                {
                    Parent = parentContentProjection,
                    ContentProjection = projection
                };
                invisibleContentProjections.Add(invisibleProjection);

                if (!renderingPageModel.RenderedPageContents.Contains(projection.PageContentId) || (parentContentProjection != null && parentContentProjection.IsInvisible))
                {
                    invisibleProjection.IsInvisible = true;
                }

                var childContentRegionProjections = projection.GetChildRegionContentProjections();
                if (childContentRegionProjections != null && childContentRegionProjections.Any())
                {
                    invisibleContentProjections = CollectInvisibleRegionsRecursively(renderingPageModel, childContentRegionProjections.Distinct(), invisibleContentProjections, invisibleProjection);
                }
            }

            return invisibleContentProjections;
        }

        private class InvisibleContentProjection
        {
            public InvisibleContentProjection Parent { get; set; }

            public PageContentProjection ContentProjection { get; set; }

            public bool IsInvisible { get; set; }
        }
    }
}