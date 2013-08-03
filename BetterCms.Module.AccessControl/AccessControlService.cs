using System;
using System.Linq;
using System.Security.Principal;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Security;
using BetterCms.Module.AccessControl.Models;

namespace BetterCms.Module.AccessControl
{
    /// <summary>
    /// Implements access control for objects.
    /// </summary>
    public class AccessControlService : IAccessControlService
    {
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessControlService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public AccessControlService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the access level.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <param name="principal">The principal.</param>
        /// <returns></returns>
        public AccessLevel GetAccessLevel(Guid objectId, IPrincipal principal)
        {
            var accessList = repository.AsQueryable<UserAccess>().Where(x => x.ObjectId == objectId);

            var accessLevel = AccessLevel.NoPermissions;

            // If use is not authenticated, check if anonymous user has access:
            if (!principal.Identity.IsAuthenticated)
            {
                var userAccess = accessList.FirstOrDefault(x => string.Equals(x.RoleOrUser, "Anonymous", StringComparison.OrdinalIgnoreCase));

                if (userAccess != null)
                {
                    accessLevel = userAccess.AccessLevel;
                }

                return accessLevel;
            }

            // Check user or role access level:
            foreach (var userAccess in accessList)
            {
                if (principal.IsInRole(userAccess.RoleOrUser) || principal.Identity.Name == userAccess.RoleOrUser)
                {
                    // Highest available privilege wins:
                    if (userAccess.AccessLevel > accessLevel)
                    {
                        accessLevel = userAccess.AccessLevel;
                    }
                }
            }

            return accessLevel;
        }

        /// <summary>
        /// Updates the access control.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <param name="roleOrUser">The user.</param>
        /// <param name="accessLevel">The access level.</param>
        public void UpdateAccessControl(Guid objectId, string roleOrUser, AccessLevel accessLevel)
        {
            var userAccess = repository.FirstOrDefault<UserAccess>(x => x.ObjectId == objectId && x.RoleOrUser == roleOrUser);

            if (userAccess == null)
            {
                userAccess = new UserAccess
                                 {
                                     ObjectId = objectId,
                                     RoleOrUser = roleOrUser,
                                     AccessLevel = accessLevel
                                 };
            }

            repository.Save(userAccess);
        }

        /// <summary>
        /// Removes the access control.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <param name="roleOrUser">The user.</param>
        public void RemoveAccessControl(Guid objectId, string roleOrUser)
        {
            var userAccess = repository.FirstOrDefault<UserAccess>(x => x.ObjectId == objectId && x.RoleOrUser == roleOrUser);

            if (userAccess == null)
            {
                return;
            }

            repository.Delete(userAccess);
        }
    }
}