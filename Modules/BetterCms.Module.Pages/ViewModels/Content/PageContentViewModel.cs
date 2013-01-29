using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Core.Models;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.Widgets;

namespace BetterCms.Module.Pages.ViewModels.Content
{
    /// <summary>
    /// Add/Edit page content view model
    /// </summary>
    public class PageContentViewModel
    {
        /// <summary>
        /// Gets or sets the page content id.
        /// </summary>
        /// <value>
        /// The page content id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the currrent content status.
        /// </summary>
        /// <value>
        /// The current content status.
        /// </value>
        public ContentStatus CurrentStatus { get; set; }

        /// <summary>
        /// Gets or sets the desirable status.
        /// </summary>
        /// <value>
        /// The desirable status.
        /// </value>
        public ContentStatus DesirableStatus { get; set; }

        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The content id.
        /// </value>
        public Guid ContentId { get; set; }

        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        /// <value>
        /// The region id.
        /// </value>
        public Guid RegionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the page content.
        /// </summary>
        /// <value>
        /// The name of the page content.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "PageContent_ContentName_RequiredMessage")]
        [StringLength(200, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "PageContent_ContentName_MaxLengthMessage")]
        public string ContentName { get; set; }

        /// <summary>
        /// Gets or sets the date, from which page is in live.
        /// </summary>
        /// <value>
        /// The date, from which page is in live.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "PageContent_LiveFrom_RequiredMessage")]
        public DateTime LiveFrom { get; set; }

        /// <summary>
        /// Gets or sets the date, to which page is in live.
        /// </summary>
        /// <value>
        /// The date, to which page is in live.
        /// </value>
        public DateTime? LiveTo { get; set; }

        /// <summary>
        /// Gets or sets the content of the page.
        /// </summary>
        /// <value>
        /// The content of the page.
        /// </value>
        [AllowHtml]
        public string PageContent { get; set; }

        public bool EanbledCustomJs { get; set; }

        public bool EnabledCustomCss { get; set; }

        public string CustomJs { get; set; }

        public string CustomCss { get; set; }

        /// <summary>
        /// Gets or sets the list of the widget categories.
        /// </summary>
        /// <value>
        /// The list of the widget categories.
        /// </value>
        public IList<WidgetCategoryViewModel> WidgetCategories { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, PageId: {2}, Name: {3}", Id, Version, PageId, ContentName);
        }
    }
}