using System;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.Models;

using BetterModules.Core.DataContracts;
using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class PageCategory : EquatableEntity<PageCategory>, IEntityCategory
    {
        public virtual Category Category { get; set; }

        public virtual IEntity Entity
        {
            get
            {
                return Category;
            }
            set
            {
                Page = value as PageProperties;
            }
        }

        public virtual void SetEntity(IEntity entity)
        {
            Page = entity as PageProperties;
        }

        public virtual PageProperties Page { get; set; }

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