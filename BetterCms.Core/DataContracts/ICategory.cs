using Devbridge.Platform.Core.DataContracts;

namespace BetterCms.Core.DataContracts
{
    /// <summary>
    /// Defines interface to access basic content properties.
    /// </summary>
    public interface ICategory : IEntity
    {
        string Name { get; }

        //IList<ICategory> ChildCategories { get; set; }

        //ICategory ParentCategory { get; set; }

        string Macro { get; set; }
    }
}
