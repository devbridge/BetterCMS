using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.ViewModels.Content;

using HtmlAgilityPack;

using NHibernate.Hql.Ast.ANTLR.Tree;

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

        /// <summary>
        /// The widget's attribute's which identifies creating attribute, name
        /// </summary>
        public const string WidgetIsNewAttributeName = "data-is-new";

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
                var availableWidgets = childrenContents
                    .Where(cc => parsedWidgets.Any(id => id.AssignmentIdentifier == cc.AssignmentIdentifier));
                foreach (var childProjection in availableWidgets)
                {
                    var model = parsedWidgets.First(w => w.AssignmentIdentifier == childProjection.AssignmentIdentifier);
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

        public static string RemoveIsNewAttribute(string html, ChildContentModel model)
        {
            var isNewAttribute = model.WidgetHtmlNode.Attributes.FirstOrDefault(a => a.Name == WidgetIsNewAttributeName);
            if (isNewAttribute != null)
            {
                model.WidgetHtmlNode.Attributes.Remove(isNewAttribute);
                
                var replaceWhat = model.Match.Value;
                var replaceWith = model.WidgetHtmlNode.OuterHtml;

                int pos = html.IndexOf(replaceWhat, StringComparison.InvariantCulture);
                if (pos >= 0)
                {
                    html = string.Concat(html.Substring(0, pos), replaceWith, html.Substring(pos + replaceWhat.Length));
                }
            }

            return html;
        }
    }
}