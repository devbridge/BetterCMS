using System;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.Models;

using BetterModules.Core.DataContracts;
using BetterModules.Core.Models;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class MediaCategory : EquatableEntity<MediaCategory>, IEntityCategory, IMediaProvider
    {
        public virtual Category Category { get; set; }

        public virtual IEntity Entity
        {
            get
            {
                return Media;
            }
            set
            {
                Media = value as Media;
            }
        }

        public virtual void SetEntity(IEntity entity)
        {
            Media = entity as Media;
        }

        public virtual Media Media { get; set; }

        ICategory IEntityCategory.Category
        {
            get
            {
                return Category;
            }
            set
            {
                Category = value as Category;
            }
        }

        public virtual MediaCategory Clone()
        {
            return CopyDataTo(new MediaCategory());
        }

        public virtual MediaCategory CopyDataTo(MediaCategory mediaCategory)
        {
            mediaCategory.Media = Media;
            mediaCategory.Category = Category;

            return mediaCategory;
        }
    }
}