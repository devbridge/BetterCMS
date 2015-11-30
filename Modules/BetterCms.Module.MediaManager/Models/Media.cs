// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Media.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts;

using BetterModules.Core.Models;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class Media : EquatableEntity<Media>, ICategorized
    {
        public virtual string Title { get; set; }

        public virtual bool IsArchived { get; set; }

        public virtual MediaType Type { get; set; }

        public virtual MediaContentType ContentType { get; set; }

        public virtual MediaFolder Folder { get; set; }

        public virtual Media Original { get; set; }

        public virtual DateTime PublishedOn { get; set; }

        public virtual IList<MediaTag> MediaTags { get; set; }

        public virtual IList<Media> History { get; set; }

        public virtual MediaImage Image { get; set; }

        public virtual string Description { get; set; }

        public virtual IList<MediaCategory> Categories { get; set; }

        public virtual void AddCategory(IEntityCategory category)
        {
            if (Categories == null)
            {
                Categories = new List<MediaCategory>();
            }
            Categories.Add(category as MediaCategory);
        }

        public virtual void RemoveCategory(IEntityCategory category)
        {
            if (Categories != null)
            {
                Categories.Remove(category as MediaCategory);
            }
        }

        public virtual void AddTag(MediaTag tag)
        {
            if (MediaTags == null)
            {
                MediaTags = new List<MediaTag>();
            }
            MediaTags.Add(tag);
        }

        public virtual string GetCategorizableItemKey()
        {
            return null;
        }

        IEnumerable<IEntityCategory> ICategorized.Categories
        {
            get
            {
                return Categories;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Media" /> class.
        /// </summary>
        public Media()
        {
            ContentType = MediaContentType.File;
            PublishedOn = DateTime.Now;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Title={1}, Type={2}", base.ToString(), Title, Type);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public virtual Media Clone()
        {
            return CopyDataTo(new Media());
        }

        /// <summary>
        /// Copies the data to.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <param name="copyCollections">if set to <c>true</c> copy collections.</param>
        /// <returns></returns>
        public virtual Media CopyDataTo(Media media, bool copyCollections = true)
        {
            media.Title = Title;
            media.Description = Description;
            media.IsArchived = IsArchived;
            media.Type = Type;
            media.ContentType = ContentType;
            media.Folder = Folder;
            media.PublishedOn = PublishedOn;
            media.Image = Image;

            if (Categories != null && copyCollections)
            {
                if (media.Categories == null)
                {
                    media.Categories = new List<MediaCategory>();
                }
                foreach (var mediaCategory in Categories.Where(c => !c.IsDeleted))
                {
                    var clonedMediaCategory = mediaCategory.Clone();
                    clonedMediaCategory.Media = media;
                    media.AddCategory(clonedMediaCategory);
                }
            }

            if (MediaTags != null && copyCollections)
            {
                if (media.MediaTags == null)
                {
                    media.MediaTags = new List<MediaTag>();
                }
                foreach (var mediaTag in MediaTags.Where(c => !c.IsDeleted))
                {
                    var clonedMediaTag = mediaTag.Clone();
                    clonedMediaTag.Media = media;
                    media.AddTag(clonedMediaTag);
                }
            }

            return media;
        }
    }
}