using System.Collections.Generic;

namespace BetterCms.Core.DataContracts
{
    public interface ICategorized
    {
        IEnumerable<IEntityCategory> Categories { get; }

        void AddCategory(IEntityCategory category);

        void RemoveCategory(IEntityCategory category);

        string GetCategorizableItemKey();
    }
}
