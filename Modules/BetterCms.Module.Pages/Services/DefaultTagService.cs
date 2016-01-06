using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Core.Services;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

using BetterModules.Core.Models;

using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;

namespace BetterCms.Module.Pages.Services
{
    /// <summary>
    /// Helper service for handling page tags
    /// </summary>
    internal class DefaultTagService : ITagService
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IRepository repository;

        private readonly ISecurityService securityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTagService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="securityService">Security service get information about authorization.</param>
        public DefaultTagService(IUnitOfWork unitOfWork, IRepository repository, ISecurityService securityService)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            this.securityService = securityService;
        }

        /// <summary>
        /// Saves the page tags.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="newCreatedTags">The new created tags.</param>
        public void SavePageTags(PageProperties page, IList<string> tags, out IList<Tag> newCreatedTags)
        {
            var trimmedTags = new List<string>();
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    trimmedTags.Add(tag.Trim());
                }
            }

            // remove tags who are equal (tags are case insensitive)
            for (int i = 0; i < trimmedTags.Count; i++)
            {
                for (int j = i + 1; j < trimmedTags.Count; j++)
                {
                    if (i != j && trimmedTags[i].ToLowerInvariant() == trimmedTags[j].ToLowerInvariant())
                    {
                        trimmedTags.RemoveAt(j);
                        --j;
                    }
                }
            }

            newCreatedTags = new List<Tag>();
            
            Tag tagAlias = null;

            // Tags merge:
            IList<PageTag> pageTags = unitOfWork.Session
                .QueryOver<PageTag>()
                .Where(t => !t.IsDeleted && t.Page.Id == page.Id)
                .JoinQueryOver<Tag>(t => t.Tag, JoinType.InnerJoin)
                .Where(t => !t.IsDeleted)
                .List<PageTag>();

            // All page tag list
            var finalTagList = new List<PageTag>();
            // Remove deleted tags:
            for (int i = pageTags.Count - 1; i >= 0; i--)
            {
                string tag = null;
                tag = trimmedTags.FirstOrDefault(s => s.ToLower() == pageTags[i].Tag.Name.ToLower());

                if (tag == null)
                {
                    var tagToRemove = pageTags[i];
                    UpdateModifiedInformation(tagToRemove);
                    unitOfWork.Session.Delete(tagToRemove);
                    finalTagList.Add(tagToRemove);
                }
            }

            // Add new tags:
            List<string> tagsInsert = new List<string>();
            foreach (string tag in trimmedTags)
            {
                PageTag existPageTag = pageTags.FirstOrDefault(pageTag => pageTag.Tag.Name.ToLower() == tag.ToLower());
                if (existPageTag == null)
                {
                    tagsInsert.Add(tag);
                }
            }

            if (tagsInsert.Count > 0)
            {
                // Get existing tags:
                IList<Tag> existingTags = unitOfWork.Session.QueryOver(() => tagAlias)
                                                            .Where(t => !t.IsDeleted)
                                                            .Where(Restrictions.In(Projections.Property(() => tagAlias.Name), tagsInsert))
                                                            .List<Tag>();

                foreach (string tag in tagsInsert)
                {
                    PageTag pageTag = new PageTag();
                    pageTag.Page = page;

                    Tag existTag = existingTags.FirstOrDefault(t => t.Name.ToLower() == tag.ToLower());
                    if (existTag != null)
                    {
                        pageTag.Tag = existTag;
                    }
                    else
                    {
                        Tag newTag = new Tag();
                        newTag.Name = tag;
                        unitOfWork.Session.SaveOrUpdate(newTag);
                        newCreatedTags.Add(newTag);
                        pageTag.Tag = newTag;
                    }

                    UpdateModifiedInformation(pageTag);
                    unitOfWork.Session.SaveOrUpdate(pageTag);
                    finalTagList.Add(pageTag);
                }
            }
            page.PageTags = finalTagList;
        }

        /// <summary>
        /// Gets the future query for the page tag names.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>
        /// The future query for the list fo tag names
        /// </returns>
        public IEnumerable<string> GetPageTagNames(Guid pageId)
        {
            return repository
                .AsQueryable<PageTag>()
                .Where(pt => pt.Page.Id == pageId && !pt.IsDeleted && !pt.Tag.IsDeleted)
                .Select(pt => pt.Tag.Name)
                .ToFuture();
        }

        /// <summary>
        /// Gets the sitemap tag names.
        /// </summary>
        /// <param name="sitemapId">The sitemap identifier.</param>
        /// <returns></returns>
        public IEnumerable<string> GetSitemapTagNames(Guid sitemapId)
        {
            return repository
                .AsQueryable<SitemapTag>()
                .Where(st => st.Sitemap.Id == sitemapId && !st.IsDeleted && !st.Tag.IsDeleted)
                .Select(st => st.Tag.Name)
                .ToFuture();
        }

        /// <summary>
        /// Saves the tags.
        /// </summary>
        /// <param name="sitemap">The sitemap.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="newCreatedTags">The new created tags.</param>
        public void SaveTags(Sitemap sitemap, IList<string> tags, out IList<Tag> newCreatedTags)
        {
            var trimmedTags = new List<string>();
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    trimmedTags.Add(tag.Trim());
                }
            }

            // remove tags who are equal (tags are case insensitive)
            for (int i = 0; i < trimmedTags.Count; i++)
            {
                for (int j = i + 1; j < trimmedTags.Count; j++)
                {
                    if (i != j && trimmedTags[i].ToLowerInvariant() == trimmedTags[j].ToLowerInvariant())
                    {
                        trimmedTags.RemoveAt(j);
                        --j;
                    }
                }
            }

            newCreatedTags = new List<Tag>();

            Tag tagAlias = null;

            // Tags merge:
            var sitemapTags = unitOfWork.Session
                .QueryOver<SitemapTag>()
                .Where(t => !t.IsDeleted && t.Sitemap.Id == sitemap.Id)
                .JoinQueryOver(t => t.Tag, JoinType.InnerJoin)
                .Where(t => !t.IsDeleted)
                .List<SitemapTag>();

            // Remove deleted tags:
            for (var i = sitemapTags.Count - 1; i >= 0; i--)
            {
                var tag = trimmedTags.FirstOrDefault(s => s.ToLower() == sitemapTags[i].Tag.Name.ToLower());
                if (tag == null)
                {
                    UpdateModifiedInformation(sitemapTags[i]);
                    unitOfWork.Session.Delete(sitemapTags[i]);
                }
            }

            // Add new tags:
            List<string> tagsInsert = new List<string>();
            foreach (var tag in trimmedTags)
            {
                var existSitemapTag = sitemapTags.FirstOrDefault(sitemapTag => sitemapTag.Tag.Name.ToLower() == tag.ToLower());
                if (existSitemapTag == null)
                {
                    tagsInsert.Add(tag);
                }
            }

            if (tagsInsert.Count <= 0)
            {
                return;
            }

            // Get existing tags:
            var existingTags = unitOfWork.Session.QueryOver(() => tagAlias)
                                                .Where(t => !t.IsDeleted)
                                                .Where(Restrictions.In(Projections.Property(() => tagAlias.Name), tagsInsert))
                                                .List<Tag>();

            foreach (var tag in tagsInsert)
            {
                var sitemapTag = new SitemapTag { Sitemap = sitemap };

                var existTag = existingTags.FirstOrDefault(t => t.Name.ToLower() == tag.ToLower());
                if (existTag != null)
                {
                    sitemapTag.Tag = existTag;
                }
                else
                {
                    var newTag = new Tag { Name = tag };
                    unitOfWork.Session.SaveOrUpdate(newTag);
                    newCreatedTags.Add(newTag);
                    sitemapTag.Tag = newTag;
                }

                UpdateModifiedInformation(sitemapTag);
                unitOfWork.Session.SaveOrUpdate(sitemapTag);
            }
        }

        /// <summary>
        /// Saves the media tags.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <param name="tags">The tags.</param>
        /// <param name="newTags">The new tags.</param>
        public void SaveMediaTags(Media media, IList<string> tags, out IList<Tag> newTags)
        {
            var trimmedTags = new List<string>();
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    trimmedTags.Add(tag.Trim());
                }
            }

            newTags = new List<Tag>();

            Tag tagAlias = null;

            // Tags merge:
            var mediaTags = unitOfWork.Session
                .QueryOver<MediaTag>()
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
                    UpdateModifiedInformation(mediaTags[i]);
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

            if (tagsInsert.Count <= 0)
            {
                return;
            }

            // Get existing tags:
            var existingTags = unitOfWork.Session.QueryOver(() => tagAlias)
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
                    var newTag = new Tag { Name = tag };
                    unitOfWork.Session.SaveOrUpdate(newTag);
                    newTags.Add(newTag);
                    mediaTag.Tag = newTag;
                }

                UpdateModifiedInformation(mediaTag);
                unitOfWork.Session.SaveOrUpdate(mediaTag);
            }
        }

        private void UpdateModifiedInformation(SitemapTag mediaTag)
        {
            UpdateModifiedInformation(mediaTag.Sitemap);
        }

        private void UpdateModifiedInformation(PageTag mediaTag)
        {
            UpdateModifiedInformation(mediaTag.Page);
        }

        private void UpdateModifiedInformation(MediaTag mediaTag)
        {
            UpdateModifiedInformation(mediaTag.Media);
        }

        private void UpdateModifiedInformation(Entity entity)
        {
            entity.ModifiedOn = DateTime.Now;
            entity.ModifiedByUser = securityService.CurrentPrincipalName;
            unitOfWork.Session.SaveOrUpdate(entity);
        }
    }
}