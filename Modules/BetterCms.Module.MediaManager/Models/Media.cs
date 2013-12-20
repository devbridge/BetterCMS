using System;
using System.Collections.Generic;

using BetterCms.Core.Models;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class Media : EquatableEntity<Media>
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
        /// <returns></returns>
        public virtual Media CopyDataTo(Media media)
        {
            media.Title = Title;
            media.Description = Description;
            media.IsArchived = IsArchived;
            media.Type = Type;
            media.ContentType = ContentType;
            media.Folder = Folder;
            media.PublishedOn = PublishedOn;
            media.Image = Image;

            return media;
        }
    }
}