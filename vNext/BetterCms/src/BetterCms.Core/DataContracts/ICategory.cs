using System.Collections.Generic;

using BetterModules.Core.DataContracts;

namespace BetterCms.Core.DataContracts
{
    /// <summary>
    /// Defines interface to access basic content properties.
    /// </summary>
    public interface ICategory : IEntity
    {
        ICategoryTree CategoryTree { get; set; }

        string Name { get; set; }

        int DisplayOrder { get; set; }

        IList<ICategory> ChildCategories { get; set; }

        ICategory ParentCategory { get; set; }

        string Macro { get; set; }
    }
}
