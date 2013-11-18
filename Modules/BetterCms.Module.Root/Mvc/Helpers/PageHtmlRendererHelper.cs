using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
            public const string PageTitle = "CmsPageTitle";
            public const string PageUrl = "CmsPageUrl";
            public const string PageId = "CmsPageId";
            public const string PageCreatedOn = "CmsPageCreatedOn";
            public const string PageModifiedOn = "CmsPageModifiedOn";
            public const string PageOption = "CmsPageOption";
            public const string MetaTitle = "CmsPageMetaTitle";
            public const string MetaKeywords = "CmsPageMetaKeywords";
            public const string MetaDescription = "CmsPageMetaDescription";
            // TODO: public const string MainImageUrl = "CmsPageMainImageUrl";
            // TODO: public const string SecondaryImageUrl = "CmsPageSecondaryImageUrl";
            // TODO: public const string FeaturedImageUrl = "CmsPageFeaturedImageUrl";
            // TODO: public const string PageCategory = "CmsPageCategory";
        }

        /// <summary>
        /// The model
        /// </summary>
        private readonly RenderPageViewModel model;

        /// <summary>
        /// The HTML string builder
        /// </summary>
        private StringBuilder stringBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageHtmlRendererHelper" /> class.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="model">The model.</param>
        public PageHtmlRendererHelper(StringBuilder stringBuilder, RenderPageViewModel model)
        {
            this.model = model;
            this.stringBuilder = stringBuilder;
        }

        /// <summary>
        /// Replaces HTML with data from page view model.
        /// </summary>
        /// <returns>Replaced HTML</returns>
        public StringBuilder GetReplacedHtml()
        {
            ReplaceAllMatches(ReplacementIds.PageTitle, model.Title);
            ReplaceAllMatches(ReplacementIds.PageUrl, model.PageUrl);
            ReplaceAllMatches(ReplacementIds.PageId, model.Id.ToString());
            ReplaceAllMatches(ReplacementIds.MetaTitle, model.MetaTitle);
            ReplaceAllMatches(ReplacementIds.MetaKeywords, model.MetaKeywords);
            ReplaceAllMatches(ReplacementIds.MetaDescription, model.MetaDescription);

            ReplaceDates(ReplacementIds.PageCreatedOn, model.CreatedOn);
            ReplaceDates(ReplacementIds.PageModifiedOn, model.ModifiedOn);

            ReplaceOptions(ReplacementIds.PageOption, model.Options);

            return stringBuilder;
        }

        /// <summary>
        /// Replaces the region representation HTML.
        /// </summary>
        public void ReplaceRegionRepresentationHtml()
        {
            stringBuilder = new StringBuilder(Regex.Replace(
                stringBuilder.ToString(),
                RootModuleConstants.DynamicRegionRegexPattern,
                "<div class=\"bcms-draggable-region\">Content to add</div>",
                RegexOptions.IgnoreCase));
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

            stringBuilder = new StringBuilder(Regex.Replace(stringBuilder.ToString(), replacement, replaceWith, RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// Replaces the options.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="options">The options.</param>
        private void ReplaceOptions(string identifier, IEnumerable<IOptionValue> options)
        {
            foreach (var match in FindAllMatches(identifier))
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

                stringBuilder.Replace(match.GlobalMatch, replaceWith);
            }
        }

        /// <summary>
        /// Replaces the dates.
        /// </summary>
        /// <returns>HTML with replacements</returns>
        private void ReplaceDates(string identifier, DateTime replaceWith)
        {
            foreach (var match in FindAllMatches(identifier))
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

                stringBuilder.Replace(match.GlobalMatch, date);
            }
        }

        /// <summary>
        /// Replaces all the matches within given HTML.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="replaceWith">The replace with.</param>
        /// <returns>
        /// HTML with replacements
        /// </returns>
        private void ReplaceAllMatches(string identifier, string replaceWith)
        {
            foreach (var match in FindAllMatches(identifier))
            {
                stringBuilder.Replace(match.GlobalMatch, replaceWith);
            }
        }

        /// <summary>
        /// Finds all matches within given HTML.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>
        /// List of all found matches
        /// </returns>
        private IEnumerable<PropertyMatch> FindAllMatches(string identifier)
        {
            var matches = new List<PropertyMatch>();
            var pattern = string.Concat("{{", identifier, "(:([^\\:\\{\\}]*))*}}");

            foreach (Match match in Regex.Matches(stringBuilder.ToString(), pattern, RegexOptions.IgnoreCase))
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