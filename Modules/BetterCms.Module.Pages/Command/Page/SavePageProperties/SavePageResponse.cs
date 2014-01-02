using System;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.SavePageProperties
{
    public class SavePageResponse
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        /// <value>
        /// The page title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the date the page is created on.
        /// </summary>
        /// <value>
        /// The date the page is created on.
        /// </value>
        public string CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the date the page is modified on.
        /// </summary>
        /// <value>
        /// The date the page is modified on.
        /// </value>
        public string ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the page status.
        /// </summary>
        /// <value>
        /// The page status.
        /// </value>
        public PageStatus PageStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether page has SEO.
        /// </summary>
        /// <value>
        ///   <c>true</c> if page has SEO; otherwise, <c>false</c>.
        /// </value>
        public bool HasSEO { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is archived; otherwise, <c>false</c>.
        /// </value>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this page is master.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this page is a master page; otherwise, <c>false</c>.
        /// </value>
        public bool IsMasterPage { get; set; }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        /// <value>
        /// The language id.
        /// </value>
        public Guid? LanguageId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePageResponse" /> class.
        /// </summary>
        /// <param name="page">The page.</param>
        public SavePageResponse(PageProperties page)
        {
            PageId = page.Id;
            Title = page.Title;
            CreatedOn = page.CreatedOn.ToFormattedDateString();
            ModifiedOn = page.ModifiedOn.ToFormattedDateString();
            PageStatus = page.Status;
            HasSEO = ((IPage)page).HasSEO;
            PageUrl = page.PageUrl;
            IsArchived = page.IsArchived;
            IsMasterPage = page.IsMasterPage;
            LanguageId = page.Language != null ? page.Language.Id : (Guid?)null;
        }
    }
}