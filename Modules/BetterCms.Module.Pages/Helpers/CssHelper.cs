// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CssHelper.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BetterCms.Module.Pages.Helpers
{
    /// <summary>
    /// Helper class, for work with custom CSS code
    /// </summary>
    public static class CssHelper
    {
        /// <summary>
        /// Fixes the CSS selectors.
        /// </summary>
        /// <param name="css">The CSS.</param>
        /// <returns>
        /// Fixed CSS
        /// </returns>
        public static string FixCss(string css)
        {
            return PrefixCssSelectorsForSingleRule(string.Empty, css);
        }

        /// <summary>
        /// Add the prefix to CSS selector.
        /// </summary>
        /// <param name="css">The CSS.</param>
        /// <param name="selectorPrefix">The selector prefix.</param>
        /// <returns>
        /// CSS with added prefixes
        /// </returns>
        public static string PrefixCssSelectors(string css, string selectorPrefix)
        {
            if (string.IsNullOrWhiteSpace(css))
            {
                return null;
            }

            // Strip CSS Comments:
            var cssWithoutComments = Regex.Replace(css, "/\\*.+?\\*/", string.Empty, RegexOptions.Singleline);

            var rules = Regex.Split(cssWithoutComments, "(?<=\\}).", RegexOptions.Singleline);
            var cssRules = new List<string>();
            var prefix = selectorPrefix + " ";

            foreach (var rule in rules.Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                cssRules.Add(PrefixCssSelectorsForSingleRule(prefix, rule));
            }

            return string.Join(Environment.NewLine, cssRules.ToArray());
        }

        /// <summary>
        /// Add the prefix to CSS selector single rule.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="rule">The rule.</param>
        /// <returns>CSS rule with prefix</returns>
        private static string PrefixCssSelectorsForSingleRule(string prefix, string rule)
        {
            var sanitizedRule = Regex.Replace(rule, "\\s+", " ").Trim();
            var braketPosition = sanitizedRule.IndexOf('{');
            var startsWithSmartTag = sanitizedRule.StartsWith("{{", StringComparison.Ordinal) && sanitizedRule.Contains("}}");

            if (braketPosition <= 0 && !startsWithSmartTag)
            {
                return string.Empty;
            }

            var selectors = sanitizedRule.Substring(0, braketPosition);
            var definition = sanitizedRule.Substring(braketPosition);
            var selectorsArray = selectors.Split(Convert.ToChar(",")).Select(s => prefix + s.Trim()).ToArray();
            var prefixedSelectors = string.Join(", ", selectorsArray);

            return string.Format("{0} {1}", prefixedSelectors, definition);
        }


    }
}