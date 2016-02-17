// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultTagService.cs" company="Devbridge Group LLC">
//
// Copyright (C) 2015,2016 Devbridge Group LLC
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
//
// Website: https://www.bettercms.com
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.Services;
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
        private readonly ISecurityService securityService;
        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTagService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="securityService">Security service get information about authorization.</param>
        public DefaultTagService(IUnitOfWork unitOfWork, ISecurityService securityService)
        {
            this.unitOfWork = unitOfWork;
            this.securityService = securityService;
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
                    UpdateModifiedInformation(mediaTags[i]);
                    unitOfWork.Session.Delete(mediaTags[i]);
                }
            }

            // Add new tags:
            var tagsInsert = new List<string>();
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
                        var newTag = new Tag { Name = tag };
                        unitOfWork.Session.SaveOrUpdate(newTag);
                        newCreatedTags.Add(newTag);
                        mediaTag.Tag = newTag;
                        UpdateModifiedInformation(mediaTag);
                    }

                    unitOfWork.Session.SaveOrUpdate(mediaTag);
                }
            }
        }

        private void UpdateModifiedInformation(MediaTag mediaTag)
        {
            var media = mediaTag.Media;
            media.ModifiedOn = DateTime.Now;
            media.ModifiedByUser = securityService.CurrentPrincipalName;
            unitOfWork.Session.SaveOrUpdate(media);
        }
    }
}