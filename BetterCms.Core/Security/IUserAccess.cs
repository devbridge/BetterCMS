using System;

namespace BetterCms.Core.Security
{
    public interface IUserAccess
    {
        Guid ObjectId { get; set; }

        string RoleOrUser { get; set; }

        AccessLevel AccessLevel { get; set; }
    }
}