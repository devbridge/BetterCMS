using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.ViewModels.Content;

using HtmlAgilityPack;

namespace BetterCms.Module.Root.Mvc.PageHtmlRenderer
{
    public class ChildContentRenderHelper
    {
        /// <summary>
        /// The regex pattern for to find all child widgets in the content
        /// </summary>
        public const string ChildWidgetRegexPattern = "<widget[^>]*>.*?<\\/widget>";

        /// <summary>
        /// The widget identifier attribute name
        /// </summary>
        public const string WidgetIdAttributeName = "data-id";

        /// <summary>
        /// The widget assignment identifier attribute name
        /// </summary>
        public const string WidgetAssignmentIdAttributeName = "data-assign-id";

        private readonly HtmlHelper htmlHelper;

        public ChildContentRenderHelper(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        public StringBuilder AppendHtml(StringBuilder stringBuilder, PageContentProjection projection)
        {
            var content = projection.GetHtml(htmlHelper);

            var childrenContents = projection.GetChildProjections();
            if (childrenContents != null && childrenContents.Any())
            {
                var parsedWidgets = ParseWidgetsFromHtml(content).Distinct();
                var availableWidgets = childrenContents.Where(cc => parsedWidgets.Any(id => id.ChildContentId == cc.ChildContentId));
                foreach (var childProjection in availableWidgets)
                {
                    var model = parsedWidgets.First(w => w.ChildContentId == childProjection.ChildContentId);
                    var replaceWhat = model.Match.Value;
                    var replaceWith = AppendHtml(new StringBuilder(), childProjection).ToString();

                    content = content.Replace(replaceWhat, replaceWith);
                }
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
                System.Guid widgetId;
                if (widgetIdAttribute == null || string.IsNullOrWhiteSpace(widgetIdAttribute.Value) || !System.Guid.TryParse(widgetIdAttribute.Value, out widgetId))
                {
                    if (throwException)
                    {
                        // TODO: add to translations
                        const string message = "Failed while parsing child widgets from content. Child widget tag should contain data-id attribute with child widget id.";
                        throw new ValidationException(() => message, message);
                    }

                    continue;
                }
                model.WidgetId = widgetId;
                model.Title = htmlDocument.DocumentNode.InnerText;
                model.WidgetHtmlNode = widgetNode;
                model.Match = match;

                var childContentIdAttribute = widgetNode.Attributes.FirstOrDefault(a => a.Name == WidgetAssignmentIdAttributeName);
                System.Guid childContentId;
                if (childContentIdAttribute != null && !string.IsNullOrWhiteSpace(childContentIdAttribute.Value) && System.Guid.TryParse(childContentIdAttribute.Value, out childContentId))
                {
                    model.ChildContentId = childContentId;
                }

                result.Add(model);
            }

            return result;
        }
    }
}