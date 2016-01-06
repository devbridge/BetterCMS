using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;

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
                        if (option.Type == OptionType.DateTime)
                        {
                            var dateValue = (DateTime)Convert.ChangeType(option.Value, typeof(DateTime));
                            if (match.Parameters.Length > 1)
                            {
                                try
                                {
                                    replaceWith = (dateValue).ToString(match.Parameters[1]);
                                }
                                catch
                                {
                                    // Do nothing
                                }
                            }
                            else
                            {
                                replaceWith = (dateValue).ToString(System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern);
                            }
                        }
//                        else if (option.Value is decimal)
                        else if (option.Type == OptionType.Float)
                        {
                            var decimalValue = (decimal)Convert.ChangeType(option.Value, typeof(decimal));
                            if (match.Parameters.Length > 1)
                            {
                                try
                                {
                                    replaceWith = (decimalValue).ToString(match.Parameters[1]);
                                }
                                catch
                                {
                                    // Do nothing
                                }
                            }
                            else
                            {
                                replaceWith = (decimalValue).ToString(CultureInfo.InvariantCulture);
                            }
                        }
                        else
                        {
                            replaceWith = option.Value;
                        }
                    }
                }

                stringBuilder.Replace(match.GlobalMatch, replaceWith);
            }

            return stringBuilder;
        }
    }
}