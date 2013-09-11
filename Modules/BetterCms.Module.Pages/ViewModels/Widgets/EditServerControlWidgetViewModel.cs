using System;
using System.Collections.Generic;

using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    /// <summary>
    /// Widget view model
    /// </summary>
    public class EditServerControlWidgetViewModel : ServerControlWidgetViewModel, IDraftDestroy
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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}", base.ToString());
        }

        /// <summary>
        /// Gets or sets a value indicating whether user can destroy draft.
        /// </summary>
        /// <value>
        ///   <c>true</c> if user can destroy draft; otherwise, <c>false</c>.
        /// </value>
        bool IDraftDestroy.CanDestroyDraft
        {
            get
            {
                return true;
            }
        }
    }
}