using System;

namespace BetterCms.Core.Models
{
    /// <summary>
    /// Defines interface to access basic content properties.
    /// </summary>
    public interface IRegion : IEntity
    {
        string Name { get; }

        string RegionIdentifier { get; }
    }
}
