using System;
using System.Security.Principal;

namespace BetterCms.Core.Security
{
    /// <summary>
    /// Defines interface to get object access level.
    /// </summary>
    public interface IAccessControlService
    {
        AccessLevel GetAccessLevel(Guid objectId, IPrincipal principal);

        void UpdateAccessControl(Guid objectId, string roleOrUser, AccessLevel accessLevel);

        void RemoveAccessControl(Guid objectId, string roleOrUser);
    }
}