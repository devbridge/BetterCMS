using BetterModules.Core.DataContracts;

namespace BetterCms.Core.DataContracts
{
    /// <summary>
    /// Defines interface to access basic content properties.
    /// </summary>
    public interface IRegion : IEntity
    {       
        string RegionIdentifier { get; }
    }
}
