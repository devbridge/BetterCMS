using HtmlAgilityPack;

namespace BetterCMS.Module.LuceneSearch.Helpers
{
    public static class HtmlAgilityPackHelper
    {
        public static void FixMissingTagClosings()
        {
            HtmlNode.ElementsFlags["form"] = HtmlElementFlag.Closed;
        }
    }
}
