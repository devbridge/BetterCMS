using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Root.ViewModels.Cms
{
    [Serializable]
    public class RenderPageViewModel : IPage
    {
        public RenderPageViewModel(IPage page)
        {
            var rootPage = page as Page;

            Id = page.Id;
            IsDeleted = page.IsDeleted;
            Version = page.Version;
            HasSEO = page.HasSEO;
            Title = page.Title;
            MetaTitle = rootPage != null && !string.IsNullOrEmpty(rootPage.MetaTitle) ? rootPage.MetaTitle : Title;
            PageUrl = page.PageUrl;
            Status = page.Status;
            CreatedOn = page.CreatedOn;
            CreatedByUser = page.CreatedByUser;
            ModifiedOn = page.ModifiedOn;
            ModifiedByUser = page.ModifiedByUser;

            Bag = new DynamicDictionary();
        }

        public RenderPageViewModel()
        {
        }

        public Guid Id { get; private set; }

        public bool IsDeleted { get; private set; }

        DateTime? IEntity.DeletedOn
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        string IEntity.DeletedByUser
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public int Version { get; private set; }
        
        public DateTime CreatedOn { get; private set; }
        
        public DateTime ModifiedOn { get; private set; }
        
        public string CreatedByUser { get; private set; }
        
        public string ModifiedByUser { get; private set; }

        public PageStatus Status { get; private set; }
        
        public bool IsPublic { get; private set; }

        public bool HasSEO { get; private set; }

        public string Title { get; private set; }

        public string MetaTitle { get; private set; }

        public string PageUrl { get; private set; }

        /// <summary>
        /// Gets or sets the layout path.
        /// </summary>
        /// <value>
        /// The layout path.
        /// </value>
        public string LayoutPath { get; set; }
        
        /// <summary>
        /// Gets or sets page content projections list.
        /// </summary>
        /// <value>
        /// The list of page content projections.
        /// </value>
        public List<PageContentProjection> Contents { get; set; }

        /// <summary>
        /// Gets or sets the layout regions.
        /// </summary>
        /// <value>
        /// The layout regions.
        /// </value>
        public List<PageRegionViewModel> Regions { get; set; }
        
        /// <summary>
        /// Gets or sets the page options.
        /// </summary>
        /// <value>
        /// The page options.
        /// </value>
        public List<IOptionValue> Options { get; set; }

        /// <summary>
        /// Gets or sets the list of meta data projections.
        /// </summary>
        /// <value>
        /// The list of meta data projections.
        /// </value>
        public List<IPageActionProjection> Metadata { get; set; }

        /// <summary>
        /// Gets or sets the list of page style projections.
        /// </summary>
        /// <value>
        /// The page style projections.
        /// </value>
        public IList<IStylesheetAccessor> Stylesheets { get; set; }

        /// <summary>
        /// Gets or sets the java scripts.
        /// </summary>
        /// <value>
        /// The java scripts.
        /// </value>
        public IList<IJavaScriptAccessor> JavaScripts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether current user can manage page content.
        /// </summary>
        /// <value>
        /// <c>true</c> if whether current user can manage page content; otherwise, <c>false</c>.
        /// </value>
        public bool CanManageContent { get; set; }

        /// <summary>
        /// Gets or sets the path to a require.js file.
        /// </summary>
        /// <value>
        /// The path to require.js file.
        /// </value>
        public string RequireJsPath { get; set; }

        /// <summary>
        /// Gets or sets the path to a bcms.main.js file.
        /// </summary>
        /// <value>
        /// The path to a bcms.main.js file.
        /// </value>
        public string MainJsPath { get; set; }

        /// <summary>
        /// Gets or sets the path to a html5shiv.js file.
        /// </summary>
        /// <value>
        /// The path to a html5shiv.js file.
        /// </value>
        public string Html5ShivJsPath { get; set; }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        public dynamic Bag { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Title: {1}, PageUrl: {2}, LayoutPath: {3}", Id, Title, PageUrl, LayoutPath);
        }
    }
}