using BetterModules.Core.DataContracts;

namespace BetterCms.Core.DataContracts
{
    public interface ICategorizableItem : IEntity
    {
        string Name { get; set; }
    }
}
