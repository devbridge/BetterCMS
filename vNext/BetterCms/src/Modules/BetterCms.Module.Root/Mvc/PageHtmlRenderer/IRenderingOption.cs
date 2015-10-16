using System.Collections.Generic;
using System.Text;

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Root.Mvc.PageHtmlRenderer
{
    public interface IRenderingOption
    {
        /// <summary>
        /// Gets the string builder with replaced HTML.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// The string builder with replaced HTML.
        /// </returns>
        StringBuilder GetReplacedHtml(StringBuilder stringBuilder, IEnumerable<IOptionValue> options);
    }
}