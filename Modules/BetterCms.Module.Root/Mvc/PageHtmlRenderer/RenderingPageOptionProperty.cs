using System;
using System.Globalization;
using System.Linq;
using System.Text;

using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Root.Mvc.PageHtmlRenderer
{
    public class RenderingPageOptionProperty : RenderingPagePropertyBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingPageOptionProperty" /> class.
        /// </summary>
        public RenderingPageOptionProperty()
            : base(RenderingPageProperties.PageOption)
        {
        }

        /// <summary>
        /// Gets the replaced HTML.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="model">The model.</param>
        /// <returns>HTML with replaced model values</returns>
        public override StringBuilder GetReplacedHtml(StringBuilder stringBuilder, RenderPageViewModel model)
        {
            foreach (var match in FindAllMatches(stringBuilder))
            {
                string replaceWith = null;

                if (match.Parameters.Length > 0 && model.Options != null)
                {
                    var optionKey = match.Parameters[0];
                    var option = model.Options.FirstOrDefault(o => o.Key == optionKey);
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