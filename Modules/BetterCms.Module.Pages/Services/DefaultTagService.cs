using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Pages.Models;

using NHibernate.Criterion;

namespace BetterCms.Module.Pages.Services
{
    /// <summary>
    /// Helper service for handling page tags
    /// </summary>
    public class DefaultTagService : ITagService
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTagService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public DefaultTagService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Saves the page tags.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="tags">The tags.</param>
        public void SavePageTags(PageProperties page, IList<string> tags)
        {
            Root.Models.Tag tagAlias = null;

            // Tags merge:
            IList<PageTag> pageTags = unitOfWork.Session.QueryOver<PageTag>()
                                                    .Where(tag => !tag.IsDeleted && tag.Page.Id == page.Id)
                                                    .Fetch(tag => tag.Tag).Eager
                                                    .List<PageTag>();

            // Remove deleted tags:
            for (int i = pageTags.Count - 1; i >= 0; i--)
            {
                string tag = null;
                if (tags != null)
                {
                    tag = tags.FirstOrDefault(s => s.ToLower() == pageTags[i].Tag.Name.ToLower());
                }
                if (tag == null)
                {
                    unitOfWork.Session.Delete(pageTags[i]);
                }
            }

            // Add new tags:
            if (tags != null)
            {
                List<string> tagsInsert = new List<string>();
                foreach (string tag in tags)
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
                    IList<Root.Models.Tag> existingTags = unitOfWork.Session.QueryOver(() => tagAlias)
                                                                .Where(Restrictions.In(NHibernate.Criterion.Projections.Property(() => tagAlias.Name), tagsInsert))
                                                                .List<Root.Models.Tag>();

                    foreach (string tag in tagsInsert)
                    {
                        PageTag pageTag = new PageTag();
                        pageTag.Page = page;

                        Root.Models.Tag existTag = existingTags.FirstOrDefault(t => t.Name.ToLower() == tag.ToLower());
                        if (existTag != null)
                        {
                            pageTag.Tag = existTag;
                        }
                        else
                        {
                            Root.Models.Tag newTag = new Root.Models.Tag();
                            newTag.Name = tag;
                            unitOfWork.Session.SaveOrUpdate(newTag);

                            pageTag.Tag = newTag;
                        }

                        unitOfWork.Session.SaveOrUpdate(pageTag);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the page tag names.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>
        /// List fo tag names
        /// </returns>
        public IList<string> GetPageTagNames(System.Guid pageId)
        {
            Root.Models.Tag tagAlias = null;

            return unitOfWork.Session
                .QueryOver<PageTag>()
                .Where(w => w.Page.Id == pageId && !w.IsDeleted)
                .JoinAlias(f => f.Tag, () => tagAlias)
                .SelectList(select => select.Select(() => tagAlias.Name))
                .List<string>();
        }
    }
}