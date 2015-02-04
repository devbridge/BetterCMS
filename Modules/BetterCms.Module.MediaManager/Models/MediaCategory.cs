using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;
using BetterCms.Module.Root.Models;

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