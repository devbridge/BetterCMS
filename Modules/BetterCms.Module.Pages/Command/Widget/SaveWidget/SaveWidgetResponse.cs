using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Pages.ViewModels.Content;

namespace BetterCms.Module.Pages.Command.Widget.SaveWidget
{
    /// <summary>
    /// 
    /// </summary>
    public class SaveWidgetResponse
    {
        /// <summary>
        /// Gets or sets the widget id.
        /// </summary>
        /// <value>
        /// The widget id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the original id.
        /// </summary>
        /// <value>
        /// The original id.
        /// </value>
        public Guid OriginalId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the original version.
        /// </summary>
        /// <value>
        /// The original version.
        /// </value>
        public int OriginalVersion { get; set; }

        /// <summary>
        /// Gets or sets the name of the widget.
        /// </summary>
        /// <value>
        /// The name of the widget.
        /// </value>
        public string WidgetName { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the type of the widget.
        /// </summary>
        /// <value>
        /// The type of the widget.
        /// </value>
        public string WidgetType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this widget is published.
        /// </summary>
        /// <value>
        /// <c>true</c> if this widget is published; otherwise, <c>false</c>.
        /// </value>
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this widget has draft version.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this widget has draft; otherwise, <c>false</c>.
        /// </value>
        public bool HasDraft { get; set; }

        /// <summary>
        /// Gets or sets the desirable status for this save action.
        /// </summary>
        /// <value>
        /// The desirable status.
        /// </value>
        public ContentStatus DesirableStatus { get; set; }

        /// <summary>
        /// Gets or sets a page content id to preview this widget.
        /// </summary>
        /// <value>
        /// The page content id to preview this widget.
        /// </value>
        public Guid? PreviewOnPageContentId { get; set; }

        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        /// <value>
        /// The regions.
        /// </value>
        public List<PageContentChildRegionViewModel> Regions { get; set; }
    }
}