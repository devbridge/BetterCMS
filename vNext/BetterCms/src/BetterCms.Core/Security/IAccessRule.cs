using System;

namespace BetterCms.Core.Security
{    
    public interface IAccessRule
    {
        Guid Id { get; set; }

        string Identity { get; set; }

        bool IsForRole { get; set; }

        AccessLevel AccessLevel { get; set; }
    }
}