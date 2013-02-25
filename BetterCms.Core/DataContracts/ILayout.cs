using System.Collections.Generic;

namespace BetterCms.Core.DataContracts
{
    public interface ILayout : IEntity
    {
        string Name { get; }

        string LayoutPath { get; }

        string PreviewUrl { get; }

        IEnumerable<IRegion> Regions { get; }
    }
}
