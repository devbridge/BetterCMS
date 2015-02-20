using System;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.Models;

using BetterModules.Core.DataContracts;
using BetterModules.Core.Models;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class MediaCategory : EquatableEntity<MediaCategory>, IEntityCategory
    {
        public virtual Category Category { get; set; }

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
    }
}