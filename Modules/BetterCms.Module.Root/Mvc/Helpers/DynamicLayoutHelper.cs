using System.Text.RegularExpressions;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class DynamicLayoutHelper
    {
        

        /// <summary>
        /// Replaces the given HTML within master page with region identifier.
        /// </summary>
        /// <param name="regionId">The region id.</param>
        /// <param name="replaceIn">The replace in.</param>
        /// <param name="replaceWith">The replace with.</param>
        /// <returns>Replaced HTML</returns>
        public static string ReplaceRegionHtml(string regionId, string replaceIn, string replaceWith)
        {
            var replacement = string.Format(RootModuleConstants.DynamicRegionReplacePattern, regionId);

            return Regex.Replace(replaceIn, replacement, replaceWith, RegexOptions.IgnoreCase);
        }
    }
}