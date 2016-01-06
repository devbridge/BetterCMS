using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Binders;
using BetterCms.Module.Root.Models;

using Newtonsoft.Json;

namespace BetterCms.Module.Pages.ViewModels.Widgets
{
    /// <summary>
    /// Editable widget view model
    /// </summary>
    public class EditWidgetViewModel : WidgetViewModel
    {
        /// <summary>
        /// Gets or sets the current status for the saved widget.
        /// </summary>
        /// <value>
        /// The current status.
        /// </value>
        public ContentStatus CurrentStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether content has original published content.
        /// </summary>
        /// <value>
        /// <c>true</c> if content has published original content; otherwise, <c>false</c>.
        /// </value>
        public bool HasPublishedContent { get; set; }

        /// <summary>
        /// Gets or sets the desirable status for the saved widget.
        /// </summary>
        /// <value>
        /// The desirable status.
        /// </value>
        public ContentStatus DesirableStatus { get; set; }

        /// <summary>
        /// Gets or sets the page published date.
        /// </summary>
        /// <value>
        /// The page published date.
        /// </value>
        public System.DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the published by user.
        /// </summary>
        /// <value>
        /// The published by user.
        /// </value>
        public string PublishedByUser { get; set; }

        /// <summary>
        /// Gets or sets a value whether user confirmed content saving when affecting children pages.
        /// </summary>
        /// <value>
        /// <c>true</c> if user confirmed content saving when affecting children pages; otherwise, <c>false</c>.
        /// </value>
        public bool IsUserConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the languages.
        /// </summary>
        /// <value>
        /// The languages.
        /// </value>
        public List<LookupKeyValue> Languages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show languages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show languages]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowLanguages { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, CurrentStatus: {1}, DesirableStatus: {2}, HasPublishedContent: {3}", base.ToString(), CurrentStatus, DesirableStatus, HasPublishedContent);
        }
    }
}