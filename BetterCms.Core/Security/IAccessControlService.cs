using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace BetterCms.Core.Security
{
    /// <summary>
    /// Defines interface to get object access level.
    /// </summary>
    public interface IAccessControlService
    {
        AccessLevel GetAccessLevel(Guid objectId, IPrincipal principal);

        void UpdateAccessControl(IEnumerable<IUserAccess> userAccessList, Guid objectId);
    }
}