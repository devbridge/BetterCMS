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

        public static string ReplaceRegionRepresentationHtml(string replaceIn)
        {
            return Regex.Replace(
                replaceIn,
                RootModuleConstants.DynamicRegionRegexPattern,
                // TODO: refractor identification.
                //"<div style=\"outline: 1px dashed #009AEB;width: 100%;height: 100%;/*background-color: rgba(0,154,235,0.1);*/\">" +
                "<p style=\"outline: 1px dashed #009AEB;opacity: 0.5;background-color: rgba(0,154,235,0.1);\">&nbsp;</p>"/* +
                "</div>"*/,
                RegexOptions.IgnoreCase);
        }
    }
}