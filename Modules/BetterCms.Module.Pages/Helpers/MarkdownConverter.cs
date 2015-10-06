using MarkdownSharp;

namespace BetterCms.Module.Pages.Helpers
{
    public static class MarkdownConverter
    {
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

            return html;
        }
    }
}