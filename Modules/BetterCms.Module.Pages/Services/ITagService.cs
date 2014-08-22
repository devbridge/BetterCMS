using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Services
{
    public interface ITagService
    {
        /// <summary>
        /// Saves the page tags.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="newCreatedTags">A list of new tags.</param>
        void SavePageTags(PageProperties page, IList<string> tags, out IList<Tag> newCreatedTags);

        /// <summary>
        /// Gets the future query for the page tag names.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>The future query for the list fo tag names</returns>
        IEnumerable<string> GetPageTagNames(Guid pageId);

        /// <summary>
        /// Gets the sitemap tag names.
        /// </summary>
        /// <param name="sitemapId">The sitemap identifier.</param>
        /// <returns></returns>
        IEnumerable<string> GetSitemapTagNames(Guid sitemapId);

        /// <summary>
        /// Saves the tags.
        /// </summary>
        /// <param name="sitemap">The sitemap.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="newTags">The new tags.</param>
        void SaveTags(Sitemap sitemap, IList<string> tags, out IList<Tag> newTags);

        /// <summary>
        /// Saves the media tags.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="newTags">The new tags.</param>
        void SaveMediaTags(Media media, IList<string> tags, out IList<Tag> newTags);
    }
}