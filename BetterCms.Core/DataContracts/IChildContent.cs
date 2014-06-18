using System;

namespace BetterCms.Core.DataContracts
{
    public interface IChildContent
    {
        Guid Id { get; }
        
        IContent ChildContent { get; }

        Guid AssignmentIdentifier { get; }
    }
}
