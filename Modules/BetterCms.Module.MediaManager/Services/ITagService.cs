using System;
using System.Collections.Generic;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.MediaManager.Services
{
    /// <summary>
    /// Media tagging service contract.
    /// </summary>
    public interface ITagService
    {
        /// <summary>
        /// Gets the media tag names.
        /// </summary>
        /// <param name="mediaId">The media id.</param>
        /// <returns>Tag list.</returns>
        IList<string> GetMediaTagNames(Guid mediaId);

        /// <summary>
        /// Saves the media tags.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="newCreatedTags">The new created tags.</param>
        void SaveMediaTags(Media media, IEnumerable<string> tags, out IList<Tag> newCreatedTags);
    }
}