using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using BetterCms.Configuration;
using BetterCms.Core.DataAccess;
using BetterCms.Core.Security;
using BetterCms.Core.Services.Caching;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.Root.Services
{
    /// <summary>
    /// Implements access control for objects.
    /// </summary>
    public class AccessControlService : IAccessControlService
    {
        private readonly IRepository repository;

        private readonly ICacheService cacheService;

        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessControlService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public AccessControlService(IRepository repository, ICacheService cacheService, ICmsConfiguration cmsConfiguration)
        {
            this.repository = repository;
            this.cacheService = cacheService;
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Gets the access level.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <param name="principal">The principal.</param>
        /// <returns></returns>
        public AccessLevel GetAccessLevel(Guid objectId, IPrincipal principal)
        {
            // TODO: Make cache length configurable value
            var accessList = cacheService.Get("bcms-useraccess-" + objectId, TimeSpan.FromMinutes(2),
                () => repository.AsQueryable<UserAccess>().Where(x => x.ObjectId == objectId).ToList());

            var accessLevel = AccessLevel.NoPermissions;

            // If there are no permissions, object is accessible to everyone:
            if (accessList.Count == 0)
            {
                return AccessLevel.ReadWrite;
            }

            // If user is not authenticated, check access level for "Everyone":
            var everyone = accessList.FirstOrDefault(x => string.Equals(x.RoleOrUser, SpecialIdentities.Everyone, StringComparison.OrdinalIgnoreCase));

            if (everyone != null)
            {
                accessLevel = everyone.AccessLevel;
            }

            // First check if there are explicit permissions for the user:
            if (principal.Identity.IsAuthenticated)
            {
                var identityName = principal.Identity.Name;
                var identityAccess = accessList.FirstOrDefault(x => string.Equals(x.RoleOrUser, identityName, StringComparison.OrdinalIgnoreCase));

                if (identityAccess != null)
                {
                    return identityAccess.AccessLevel;
                }

                // Check access level for "Authenticated User":
                var authenticated = accessList.FirstOrDefault(x => string.Equals(x.RoleOrUser, SpecialIdentities.AuthenticatedUser, StringComparison.OrdinalIgnoreCase));

                if (authenticated != null && authenticated.AccessLevel > accessLevel)
                {
                    accessLevel = authenticated.AccessLevel;
                }

                // Check user or role access level:
                foreach (var userAccess in accessList)
                {
                    if (principal.IsInRole(userAccess.RoleOrUser))
                    {
                        // Highest available privilege wins:
                        if (userAccess.AccessLevel > accessLevel)
                        {
                            accessLevel = userAccess.AccessLevel;
                        }
                    }
                }
            }

            return accessLevel;
        }

        /// <summary>
        /// Updates the access control.
        /// </summary>
        /// <param name="userAccessList">The user access list.</param>
        /// <param name="objectId">The object id.</param>
        public void UpdateAccessControl(IEnumerable<IUserAccess> userAccessList, Guid objectId)
        {
            var accessList = userAccessList.ToList();

            // Ensure that each object has ObjectId:
            accessList.ForEach(x => x.ObjectId = objectId);

            var entities = repository.AsQueryable<UserAccess>()
                                .Where(x => x.ObjectId == objectId)
                                .ToList();

            var entitiesToDelete = entities.Where(x => accessList.All(model => model.RoleOrUser != x.RoleOrUser)).ToList();

            var entitesToAdd = accessList
                                  .Where(x => entities.All(entity => entity.RoleOrUser != x.RoleOrUser))
                                  .Select(ModelToEntity)
                                  .ToList();

            var entitiesToUpdate = GetEntitiesToUpdate(accessList, entities);

            entitiesToDelete.ForEach(entity => repository.Delete(entity));
            entitiesToUpdate.ForEach(entity => repository.Save(entity));
            entitesToAdd.ForEach(entity => repository.Save(entity));
        }

        /// <summary>
        /// Gets the default access list.
        /// </summary>
        /// <returns></returns>
        public List<IUserAccess> GetDefaultAccessList()
        {
            var list = new List<IUserAccess>();

            foreach (AccessControlElement userAccess in cmsConfiguration.DefaultAccessControlList)
            {
                list.Add(new UserAccessViewModel
                             {
                                 RoleOrUser = userAccess.RoleOrUser,
                                 AccessLevel = (AccessLevel)Enum.Parse(typeof(AccessLevel), userAccess.AccessLevel)
                             });
            }

            return list;
        }

        /// <summary>
        /// Models to entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private static UserAccess ModelToEntity(IUserAccess model)
        {
            return new UserAccess
            {
                ObjectId = model.ObjectId,
                RoleOrUser = model.RoleOrUser,
                AccessLevel = model.AccessLevel
            };
        }

        /// <summary>
        /// Gets the entities to update.
        /// </summary>
        /// <param name="accessList">The access list.</param>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        private static List<UserAccess> GetEntitiesToUpdate(List<IUserAccess> accessList, List<UserAccess> entities)
        {
            var entitiesToUpdate = new List<UserAccess>();

            foreach (var entity in entities)
            {
                // Find user access object with the same Role and diferrent AccessLevel:
                var userAccess = accessList.FirstOrDefault(x => x.RoleOrUser == entity.RoleOrUser && x.AccessLevel != entity.AccessLevel);

                // If found, add to updatables list:
                if (userAccess != null)
                {
                    entity.AccessLevel = userAccess.AccessLevel;
                    entitiesToUpdate.Add(entity);
                }
            }

            return entitiesToUpdate;
        }
    }
}