using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Root.Mvc.PageHtmlRenderer
{
    public class RenderingOptionPropertyBase : RenderingPropertyBase, IRenderingOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingPageOptionProperty" /> class.
        /// </summary>
        public RenderingOptionPropertyBase(string identifier)
            : base(identifier)
        {
        }

        /// <summary>
        /// Gets the replaced HTML.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// HTML with replaced model values
        /// </returns>
        public StringBuilder GetReplacedHtml(StringBuilder stringBuilder, IEnumerable<IOptionValue> options)
        {
            foreach (var match in FindAllMatches(stringBuilder))
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
                                replaceWith = ((DateTime)option.Value).ToString(System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            }
                        }
                        else if (option.Value is decimal)
                        {
                            if (match.Parameters.Length > 1)
                            {
                                try
                                {
                                    replaceWith = ((decimal)option.Value).ToString(match.Parameters[1]);
                                }
                                catch
                                {
                                    // Do nothing
                                }
                            }
                            else
                            {
                                replaceWith = ((decimal)option.Value).ToString(CultureInfo.InvariantCulture);
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

            return stringBuilder;
        }
    }
}