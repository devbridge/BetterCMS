using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Models;

using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace BetterCms.Module.MediaManager.Services
{
    /// <summary>
    /// Helper service for handling media tags.
    /// </summary>
    internal class DefaultTagService : ITagService
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTagService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public DefaultTagService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the media tag names.
        /// </summary>
        /// <param name="mediaId">The media id.</param>
        /// <returns>Tag list.</returns>
        public IList<string> GetMediaTagNames(System.Guid mediaId)
        {
            Tag tagAlias = null;

            return
                unitOfWork.Session.QueryOver<MediaTag>()
                          .JoinAlias(f => f.Tag, () => tagAlias)
                          .Where(() => !tagAlias.IsDeleted)
                          .Where(w => w.Media.Id == mediaId && !w.IsDeleted)
                          .SelectList(select => select.Select(() => tagAlias.Name))
                          .List<string>();
        }

        /// <summary>
        /// Saves the media tags.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="newCreatedTags">The new created tags.</param>
        public void SaveMediaTags(Media media, IEnumerable<string> tags, out IList<Tag> newCreatedTags)
        {
            var trimmedTags = new List<string>();
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    trimmedTags.Add(tag.Trim());
                }
            }

            newCreatedTags = new List<Tag>();

            Tag tagAlias = null;

            // Tags merge:
            var mediaTags =
                unitOfWork.Session.QueryOver<MediaTag>()
                          .Where(t => !t.IsDeleted && t.Media.Id == media.Id)
                          .JoinQueryOver(t => t.Tag, JoinType.InnerJoin)
                          .Where(t => !t.IsDeleted)
                          .List<MediaTag>();

            // Remove deleted tags:
            for (var i = mediaTags.Count - 1; i >= 0; i--)
            {
                var tag = trimmedTags.FirstOrDefault(s => s.ToLower() == mediaTags[i].Tag.Name.ToLower());

                if (tag == null)
                {
                    unitOfWork.Session.Delete(mediaTags[i]);
                }
            }

            // Add new tags:
            List<string> tagsInsert = new List<string>();
            foreach (var tag in trimmedTags)
            {
                var existMediaTag = mediaTags.FirstOrDefault(mediaTag => mediaTag.Tag.Name.ToLower() == tag.ToLower());
                if (existMediaTag == null)
                {
                    tagsInsert.Add(tag);
                }
            }

            if (tagsInsert.Count > 0)
            {
                // Get existing tags:
                var existingTags =
                    unitOfWork.Session.QueryOver(() => tagAlias)
                              .Where(t => !t.IsDeleted)
                              .Where(Restrictions.In(Projections.Property(() => tagAlias.Name), tagsInsert))
                              .List<Tag>();

                foreach (var tag in tagsInsert)
                {
                    var mediaTag = new MediaTag { Media = media };

                    var existTag = existingTags.FirstOrDefault(t => t.Name.ToLower() == tag.ToLower());
                    if (existTag != null)
                    {
                        mediaTag.Tag = existTag;
                    }
                    else
                    {
                        Tag newTag = new Tag { Name = tag };
                        unitOfWork.Session.SaveOrUpdate(newTag);
                        newCreatedTags.Add(newTag);
                        mediaTag.Tag = newTag;
                    }

                    unitOfWork.Session.SaveOrUpdate(mediaTag);
                }
            }
        }
    }
}