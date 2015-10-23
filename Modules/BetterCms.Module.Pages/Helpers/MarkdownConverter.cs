using System.Text.RegularExpressions;

using MarkdownSharp;

namespace BetterCms.Module.Pages.Helpers
{
    public static class MarkdownConverter
    {
        private const string WidgetFindPattern = "<p>\\s*<widget ([^>]*>.*?)<\\/widget>\\s*</p>";
        private const string WidgetReplacePattern = "<widget $1</widget>";

        private static Markdown markdownParser;

        private static Markdown MarkdownParser
        {
            get
            {
                if (markdownParser == null)
                {
                    markdownParser = new Markdown();
                }

                return markdownParser;
            }
        }

        public static string ToHtml(string markdown)
        {
            string html = MarkdownParser.Transform(markdown);

            html = Regex.Replace(html, WidgetFindPattern, WidgetReplacePattern);

            return html;
        }
    }
}