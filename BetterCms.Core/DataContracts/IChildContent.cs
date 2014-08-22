using System;
using System.Collections.Generic;

namespace BetterCms.Core.DataContracts
{
    public interface IChildContent
    {
        Guid Id { get; }
        
        IContent ChildContent { get; }

        Guid AssignmentIdentifier { get; }

        IEnumerable<IOption> Options { get; }
    }
}
