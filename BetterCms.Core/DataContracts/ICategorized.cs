using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterCms.Core.DataContracts
{
    public interface ICategorized
    {
        IEnumerable<IEntityCategory> Categories { get; }

        void AddCategory(IEntityCategory category);

        void RemoveCategory(IEntityCategory category);
    }
}
