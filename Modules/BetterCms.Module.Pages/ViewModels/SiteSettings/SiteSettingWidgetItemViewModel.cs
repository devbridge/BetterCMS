using System;
using System.Collections.Generic;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Pages.ViewModels.SiteSettings
{
    public class SiteSettingWidgetItemViewModel : IEditableGridItem
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The content id.
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
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
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
        /// Gets or sets the widget name.
        /// </summary>
        /// <value>
        /// The widget name.
        /// </value>
        public string WidgetName { get; set; }

        /// <summary>
        /// Gets or sets the type of the widget entity.
        /// </summary>
        /// <value>
        /// The type of the widget entity.
        /// </value>
        public Type WidgetEntityType { get; set; }

        /// <summary>
        /// Gets or sets the type of the widget.
        /// </summary>
        /// <value>
        /// The type of the widget.
        /// </value>
        public WidgetType? WidgetType { get; set; }

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
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status
        {
            get
            {
                if (IsPublished && HasDraft)
                {
                    return RootGlobalization.ContentStatus_PublishedWithDraft;
                }

                if (IsPublished)
                {
                    return RootGlobalization.ContentStatus_Published;
                }

                if (HasDraft)
                {
                    return RootGlobalization.ContentStatus_Draft;
                }
                
                return null;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, WidgetName: {2}", Id, Version, WidgetName);
        }
        

    }
}