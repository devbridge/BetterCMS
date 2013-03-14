using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Projections;

namespace BetterCms.Module.Root.ViewModels.Cms
{
    [Serializable]
    public class RenderPageViewModel : IPage
    {
        public RenderPageViewModel(IPage page)
        {
            Id = page.Id;
            IsDeleted = page.IsDeleted;
            Version = page.Version;
            HasSEO = page.HasSEO;
            Title = page.Title;
            PageUrl = page.PageUrl;
            Status = page.Status;
        }

        public RenderPageViewModel()
        {
        }

        public Guid Id { get; private set; }

        public bool IsDeleted { get; private set; }

        DateTime IEntity.CreatedOn
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        DateTime IEntity.ModifiedOn
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        DateTime? IEntity.DeletedOn
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        string IEntity.CreatedByUser
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        string IEntity.ModifiedByUser
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

        public PageStatus Status { get; private set; }
        
        public bool IsPublic { get; private set; }

        public bool HasSEO { get; private set; }

        public string Title { get; private set; }

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