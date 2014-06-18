using System.Text.RegularExpressions;

using HtmlAgilityPack;

namespace BetterCms.Module.Root.ViewModels.Content
{
    public class ChildContentModel
    {
        public string Title { get; set; }

        public System.Guid WidgetId { get; set; }

        public System.Guid AssignmentIdentifier { get; set; }

        public HtmlNode WidgetHtmlNode { get; set; }

        public Match Match { get; set; }

        public bool IsNew { get; set; }
    }
}