using System.Collections.Generic;
using System.Text;

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Root.ViewModels.Cms
{
    /// <summary>
    /// Render view model for server widgets.
    /// </summary>
    public class RenderWidgetViewModel
    {
        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        public IRenderPage Page { get; set; }

        /// <summary>
        /// Gets or sets the widget.
        /// </summary>
        /// <value>
        /// The widget.
        /// </value>
        public IWidget Widget { get; set; }

        /// <summary>
        /// Gets or sets the widget options.
        /// </summary>
        /// <value>
        /// The widget options.
        /// </value>
        public IList<IOptionValue> Options { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Page != null)
            {
                sb.AppendFormat("Page: Id={0}, Title={1}", Page.Id, Page.Title);
            }
            else
            {
                sb.AppendFormat("Page property is empty");
            }

            sb.Append("; ");

            if (Widget != null)
            {
                sb.AppendFormat("Widget: Id={0}, Name={1}", Widget.Id, Widget.Name);
            }
            else
            {
                sb.AppendFormat("Widget property is empty");
            }

            sb.Append(".");

            return sb.ToString();
        }
    }
}