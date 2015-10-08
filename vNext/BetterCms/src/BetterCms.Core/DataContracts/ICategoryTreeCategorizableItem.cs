using BetterModules.Core.DataContracts;

namespace BetterCms.Core.DataContracts
{
    public interface ICategoryTreeCategorizableItem : IEntity
    {
        ICategoryTree CategoryTree { get; set; }
        ICategorizableItem CategorizableItem { get; set; }
    }
}
