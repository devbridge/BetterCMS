using System;

using BetterCms.Core.DataContracts;

namespace BetterCms.Core.Security
{    
    public interface IAccessRule
    {
        Guid Id { get; set; }

        string Identity { get; set; }

        AccessLevel AccessLevel { get; set; }
    }
}