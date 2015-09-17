using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BetterCms.Module.Root.Mvc.PageHtmlRenderer
{
    public abstract class RenderingPropertyBase : IRenderingProperty
    {
        /// <summary>
        /// The identifier
        /// </summary>
        protected readonly string identifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingPagePropertyBase" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public RenderingPropertyBase(string identifier)
        {
            this.identifier = identifier;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Identifier
        {
            get
            {
                return identifier;
            }
        }

        /// <summary>
        /// Gets the string builder with replaced HTML.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="getReplaceWith">The object to replace within string.</param>
        /// <returns>
        /// The string builder with replaced HTML.
        /// </returns>
        protected StringBuilder GetReplacedHtml(StringBuilder stringBuilder, Func<string> getReplaceWith)
        {
            if (getReplaceWith == null)
            {
                return stringBuilder;
            }

            var matches = FindAllMatches(stringBuilder);
            if (matches.Count > 0)
            {
                var replaceWith = getReplaceWith();
                foreach (var match in matches)
                {
                    stringBuilder.Replace(match.GlobalMatch, replaceWith);
                }
            }

            return stringBuilder;
        }

        /// <summary>
        /// Gets the string builder with replaced HTML.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="replaceWith">The object to replace within string.</param>
        /// <returns>
        /// The string builder with replaced HTML.
        /// </returns>
        protected StringBuilder GetReplacedHtml(StringBuilder stringBuilder, DateTime? replaceWith)
        {
            foreach (var match in FindAllMatches(stringBuilder))
            {
                string date;
                if (replaceWith.HasValue)
                {
                    if (match.Parameters != null && match.Parameters.Length > 0)
                    {
                        try
                        {
                            date = replaceWith.Value.ToString(match.Parameters[0]);
                        }
                        catch
                        {
                            date = string.Empty;
                        }
                    }
                    else
                    {
                        date = replaceWith.Value.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern);
                    }
                }
                else
                {
                    date = null;
                }

                stringBuilder.Replace(match.GlobalMatch, date);
            }

            return stringBuilder;
        }

        /// <summary>
        /// Finds all matches within given HTML.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <returns>
        /// List of all found matches
        /// </returns>
        protected IList<PropertyMatch> FindAllMatches(StringBuilder stringBuilder)
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
        protected class PropertyMatch
        {
            public string GlobalMatch { get; set; }

            public string[] Parameters { get; set; }
        }
    }
}