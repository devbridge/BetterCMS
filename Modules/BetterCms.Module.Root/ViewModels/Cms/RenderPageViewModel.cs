using System;
using System.Collections.Generic;

using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Projections;

namespace BetterCms.Module.Root.ViewModels.Cms
{
    [Serializable]
    public class RenderPageViewModel : IPage
    {
        public RenderPageViewModel()
        {
        }

        public RenderPageViewModel(IPage page)
        {
            Id = page.Id;
            IsPublished = page.IsPublished;
            HasSEO = page.HasSEO;
            Title = page.Title;
            PageUrl = page.PageUrl;
        }

        public Guid Id { get; set; }

        public bool IsPublished { get; set; }

        public bool HasSEO { get; set; }

        public string Title { get; set; }

        public string PageUrl { get; set; }

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
        /// Gets or sets a value indicating whether current user can manage page content.
        /// </summary>
        /// <value>
        /// <c>true</c> if whether current user can manage page content; otherwise, <c>false</c>.
        /// </value>
        public bool CanManageContent { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, IsPublished: {1}, Title: {2}, PageUrl: {3}, LayoutPath: {4}", Id, IsPublished, Title, PageUrl, LayoutPath);
        }
    }
}