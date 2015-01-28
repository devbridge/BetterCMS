using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterCms.Core.DataContracts
{
    public interface ICategorized
    {
        IEnumerable<ICategory> Categories { get; }

        void AddCategory(ICategory category);

        void RemoveCategory(ICategory category);
    }
}
