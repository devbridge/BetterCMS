using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

using BetterCms.Module.Root.Projections;

namespace BetterCms.Module.Root.Mvc.PageHtmlRenderer
{
    public class ContentHtmlRenderer
    {
        private readonly HtmlHelper htmlHelper;

        public ContentHtmlRenderer(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        public StringBuilder AppendHtml(StringBuilder stringBuilder, PageContentProjection projection)
        {
            var content = projection.GetHtml(htmlHelper);

            var childrenContents = projection.GetChildProjections();
            if (childrenContents != null && childrenContents.Any())
            {
                var widgetIds = ParseWidgetsFromHtml(content).Distinct();
                var availableWidgets = childrenContents.Where(cc => widgetIds.Any(id => id == cc.ChildContentId));
                foreach (var childProjection in availableWidgets)
                {
                    var replaceWhat = string.Format(RootModuleConstants.ChildWidgetReplacePattern, 
                        childProjection.ChildContentId.ToString().ToUpperInvariant());
                    var replaceWith = AppendHtml(new StringBuilder(), childProjection).ToString();
                    
                    content = content.Replace(replaceWhat, replaceWith);
                }
            }

            stringBuilder.Append(content);

            return stringBuilder;
        }

        public static System.Guid[] ParseWidgetsFromHtml(string searchIn, string pattern = RootModuleConstants.ChildWidgetRegexPattern)
        {
            if (string.IsNullOrWhiteSpace(searchIn))
            {
                return new System.Guid[0];
            }

            var ids = new List<System.Guid>();

            var matches = Regex.Matches(searchIn, pattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    System.Guid id;
                    if (System.Guid.TryParse(match.Groups[1].Value, out id))
                    {
                        ids.Add(id);
                    }
                }
            }

            return ids.ToArray();
        }
    }
}