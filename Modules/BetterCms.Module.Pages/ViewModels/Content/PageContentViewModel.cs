using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Pages.Mvc.Attributes;

using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.ViewModels.Content
{
    /// <summary>
    /// Add/Edit page content view model
    /// </summary>
    public class PageContentViewModel : IAccessSecuredViewModel, IDraftDestroy
    {
        /// <summary>
        /// Gets or sets the page content id.
        /// </summary>
        /// <value>
        /// The page content id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the page content version.
        /// </summary>
        /// <value>
        /// The page content version.
        /// </value>
        public int Version { get; set; }
        
        /// <summary>
        /// Gets or sets the content version.
        /// </summary>
        /// <value>
        /// The content version.
        /// </value>
        public int ContentVersion { get; set; }

        /// <summary>
        /// Gets or sets the currrent content status.
        /// </summary>
        /// <value>
        /// The current content status.
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
        /// Gets or sets the parent page content identifier.
        /// </summary>
        /// <value>
        /// The parent page content identifier.
        /// </value>
        public Guid ParentPageContentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the page content.
        /// </summary>
        /// <value>
        /// The name of the page content.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "PageContent_ContentName_RequiredMessage")]
        [StringLength(MaxLength.Name, MinimumLength = 1, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "PageContent_ContentName_MaxLengthMessage")]
        public string ContentName { get; set; }

        /// <summary>
        /// Gets or sets the date, from which page is in live.
        /// </summary>
        /// <value>
        /// The date, from which page is in live.
        /// </value>
        [DateValidation(ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "PageContent_LiveFrom_DateNotValidationMessage")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "PageContent_LiveFrom_RequiredMessage")]
        public DateTime LiveFrom { get; set; }

        /// <summary>
        /// Gets or sets the date, to which page is in live.
        /// </summary>
        /// <value>
        /// The date, to which page is in live.
        /// </value>
        [DateValidation(ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "PageContent_LiveTo_DateNotValidationMessage")]
        [EndDateValidation(StartDateProperty = "LiveFrom", ErrorMessageResourceType = typeof(PagesGlobalization), ErrorMessageResourceName = "PageContent_LiveTo_ValidationMessage")]
        public DateTime? LiveTo { get; set; }

        /// <summary>
        /// Gets or sets the content of the page.
        /// </summary>
        /// <value>
        /// The content of the page.
        /// </value>
        [AllowHtml]
        public string PageContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to eanble custom javascript.
        /// </summary>
        /// <value>
        ///   <c>true</c> if custom javascript is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool EnabledCustomJs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable custom CSS.
        /// </summary>
        /// <value>
        ///   <c>true</c> if custom CSS is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool EnabledCustomCss { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether content editor must be opened in source mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if content editor must be opened in source mode; otherwise, <c>false</c>.
        /// </value>
        public bool EditInSourceMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether option to inset dynamic region is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if option to inset dynamic region is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool EnableInsertDynamicRegion { get; set; }

        /// <summary>
        /// Gets or sets the last dynamic region number.
        /// </summary>
        /// <value>
        /// The last dynamic region number.
        /// </value>
        public int LastDynamicRegionNumber { get; set; }

        /// <summary>
        /// Gets or sets the custom JavaSctript.
        /// </summary>
        /// <value>
        /// The custom JavaSctript.
        /// </value>
        [AllowHtml]
        public string CustomJs { get; set; }

        /// <summary>
        /// Gets or sets the custom CSS.
        /// </summary>
        /// <value>
        /// The custom CSS.
        /// </value>
        [AllowHtml]
        public string CustomCss { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether dialog should be opened in the read only mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if dialog should be opened in the read only mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user can destroy draft.
        /// </summary>
        /// <value>
        ///   <c>true</c> if user can destroy draft; otherwise, <c>false</c>.
        /// </value>
        public bool CanDestroyDraft { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user has edit content right.
        /// </summary>
        /// <value>
        ///   <c>true</c> if user has edit content right; otherwise, <c>false</c>.
        /// </value>
        public bool CanEditContent { get; set; }

        /// <summary>
        /// Gets or sets a value whether user confirmed content saving when affecting children pages.
        /// </summary>
        /// <value>
        /// <c>true</c> if user confirmed content saving when affecting children pages; otherwise, <c>false</c>.
        /// </value>
        public bool IsUserConfirmed { get; set; }

        /// <summary>
        /// Determines, if child regions should be included to the results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if child regions should be included to the results; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeChildRegions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether content type is markdown.
        /// </summary>
        /// <value>
        /// <c>true</c> if content type is markdown; otherwise, <c>false</c>.
        /// </value>
        public ContentTextMode ContentTextMode { get; set; }

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