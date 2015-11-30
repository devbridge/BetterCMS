// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderingPropertyBase.cs" company="Devbridge Group LLC">
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