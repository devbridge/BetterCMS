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

            if (braketPosition <= 0)
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