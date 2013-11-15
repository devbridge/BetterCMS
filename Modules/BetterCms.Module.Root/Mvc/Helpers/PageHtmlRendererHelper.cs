using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    /// <summary>
    /// Helper class, helps to render page contents HTML
    /// </summary>
    public class PageHtmlRendererHelper
    {
        public static class ReplacementIds
        {
            public const string PageTitle = "CMSPAGETITLE";
            public const string PageUrl = "CMSPAGEURL";
            public const string PageCreatedOn = "CMSPAGECREATIONDATE";
            public const string PageOption = "CMSPAGEOPTION";
        }

        /// <summary>
        /// The model
        /// </summary>
        private readonly RenderPageViewModel model;

        /// <summary>
        /// The HTML
        /// </summary>
        private string html;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageHtmlRendererHelper" /> class.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="model">The model.</param>
        public PageHtmlRendererHelper(string html, RenderPageViewModel model)
        {
            this.model = model;
            this.html = html;
        }

        /// <summary>
        /// Replaces HTML with data from page view model.
        /// </summary>
        /// <returns>Replaced HTML</returns>
        public string GetReplacedHtml()
        {
            html = ReplaceAllMatches(ReplacementIds.PageTitle, html, model.Title);
            html = ReplaceAllMatches(ReplacementIds.PageUrl, html, model.PageUrl);
            html = ReplaceDates(ReplacementIds.PageCreatedOn, html, model.CreatedOn);
            html = ReplaceOptions(ReplacementIds.PageOption, html, model.Options);

            return html;
        }

        /// <summary>
        /// Replaces the region representation HTML.
        /// </summary>
        public void ReplaceRegionRepresentationHtml()
        {
            html = Regex.Replace(
                html,
                RootModuleConstants.DynamicRegionRegexPattern,
                "<div class=\"bcms-draggable-region\">Content to add</div>",
                RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Replaces the given HTML within master page with region identifier.
        /// </summary>
        /// <param name="regionId">The region id.</param>
        /// <param name="replaceWith">The replace with.</param>
        /// <returns>
        /// Replaced HTML
        /// </returns>
        public void ReplaceRegionHtml(string regionId, string replaceWith)
        {
            var replacement = string.Format(RootModuleConstants.DynamicRegionReplacePattern, regionId);

            html = Regex.Replace(html, replacement, replaceWith, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Replaces the options.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="html">The HTML.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        private string ReplaceOptions(string identifier, string html, IEnumerable<IOptionValue> options)
        {
            foreach (var match in FindAllMatches(identifier, html))
            {
                string replaceWith = null;

                if (match.Parameters.Length > 0 && options != null)
                {
                    var optionKey = match.Parameters[0];
                    var option = options.FirstOrDefault(o => o.Key == optionKey);
                    if (option != null && option.Value != null)
                    {
                        if (option.Value is DateTime)
                        {
                            if (match.Parameters.Length > 1)
                            {
                                try
                                {
                                    replaceWith = ((DateTime)option.Value).ToString(match.Parameters[1]);
                                }
                                catch
                                {
                                    // Do nothing
                                }
                            }
                            else
                            {
                                replaceWith = ((DateTime)option.Value).ToString(CultureInfo.InvariantCulture);
                            }
                        }
                        else
                        {
                            replaceWith = option.Value.ToString();
                        }
                    }
                }

                html = html.Replace(match.GlobalMatch, replaceWith);
            }

            return html;
        }

        /// <summary>
        /// Replaces the dates.
        /// </summary>
        /// <returns>HTML with replacements</returns>
        private string ReplaceDates(string identifier, string html, DateTime replaceWith)
        {
            foreach (var match in FindAllMatches(identifier, html))
            {
                string date;
                if (match.Parameters != null && match.Parameters.Length > 0)
                {
                    try
                    {
                        date = replaceWith.ToString(match.Parameters[0]);
                    }
                    catch
                    {
                        date = string.Empty;
                    }
                }
                else
                {
                    date = replaceWith.ToString(CultureInfo.InvariantCulture);
                }

                html = html.Replace(match.GlobalMatch, date);
            }

            return html;
        }

        /// <summary>
        /// Replaces all the matches within given HTML.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="html">The HTML.</param>
        /// <param name="replaceWith">The replace with.</param>
        /// <returns>HTML with replacements</returns>
        private string ReplaceAllMatches(string identifier, string html, string replaceWith)
        {
            foreach (var match in FindAllMatches(identifier, html))
            {
                html = html.Replace(match.GlobalMatch, replaceWith);
            }

            return html;
        }

        /// <summary>
        /// Finds all matches within given HTML.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="html">The HTML.</param>
        /// <returns>List of all found matches</returns>
        private IEnumerable<PropertyMatch> FindAllMatches(string identifier, string html)
        {
            var matches = new List<PropertyMatch>();
            var pattern = string.Concat("{{", identifier, "(:([^\\:\\{\\}]*))*}}");

            foreach (Match match in Regex.Matches(html, pattern, RegexOptions.IgnoreCase))
            {
                var propertyMatch = new PropertyMatch
                {
                    GlobalMatch = match.Value
                };
                if (match.Groups.Count > 2)
                {
                    propertyMatch.Parameters = new string[match.Groups[2].Captures.Count];
                    var i = 0;

                    foreach (Capture capture in match.Groups[2].Captures)
                    {
                        propertyMatch.Parameters[i] = capture.Value;
                        i++;
                    }
                }

                matches.Add(propertyMatch);
            }

            return matches;
        }

        /// <summary>
        /// Helper class for passing the matches between functions
        /// </summary>
        private class PropertyMatch
        {
            public string GlobalMatch { get; set; }

            public string[] Parameters { get; set; }
        }
    }
}