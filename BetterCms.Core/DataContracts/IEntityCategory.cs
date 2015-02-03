using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterCms.Core.DataContracts
{
    public interface IEntityCategory
    {
        ICategory Category { get; set; }

        void SetEntity(IEntity entity);
    }
}
