using System.Text.RegularExpressions;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class DynamicLayoutHelper
    {
        public const string DynamicRegionReplacePattern = "{{{{DYNAMIC_REGION:{0}}}}}";

        public const string DynamicRegionRegexPattern = "{{DYNAMIC_REGION\\:[a-zA-Z0-9]{8}\\-[a-zA-Z0-9]{4}\\-[a-zA-Z0-9]{4}\\-[a-zA-Z0-9]{4}\\-[a-zA-Z0-9]{12}}}";

        /// <summary>
        /// Replaces the given HTML within master page with region identifier.
        /// </summary>
        /// <param name="regionId">The region id.</param>
        /// <param name="replaceIn">The replace in.</param>
        /// <param name="replaceWith">The replace with.</param>
        /// <returns>Replaced HTML</returns>
        public static string ReplaceRegionHtml(System.Guid regionId, string replaceIn, string replaceWith)
        {
            var replacement = string.Format(DynamicRegionReplacePattern, regionId);

            return Regex.Replace(replaceIn, replacement, replaceWith, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Collects the dynamic layouts.
        /// </summary>
        /// <returns>Collected dynamic layouts</returns>
        public static string[] CollectDynamicLayouts()
        {
            // TODO
            return new string[0];
        }
    }
}