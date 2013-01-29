using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    /// <summary>
    /// A widget view model.
    /// </summary>
    public class EditHtmlContentWidgetViewModel : HtmlContentWidgetViewModel
    {
        /// <summary>
        /// Gets or sets the page content id to preview this widget.
        /// </summary>
        /// <value>
        /// The page content id to preview this widget.
        /// </value>
        public Guid? PreviewOnPageContentId { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public IList<LookupKeyValue> Categories { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", base.ToString());
        }
    }
}