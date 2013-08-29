using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using BetterCms.Configuration;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts;
using BetterCms.Core.Security;
using BetterCms.Core.Services.Caching;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.Root.Services
{
    /// <summary>
    /// Implements access control for objects.
    /// </summary>
    public class DefaultAccessControlService : IAccessControlService
    {
        private const string AccessLevelCacheKeyPattern = "bcms-useraccess-{0}";

        private readonly IRepository repository;

        private readonly ICacheService cacheService;

        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAccessControlService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public DefaultAccessControlService(IRepository repository, ICacheService cacheService, ICmsConfiguration cmsConfiguration)
        {
            this.repository = repository;
            this.cacheService = cacheService;
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Gets the access level.
        /// </summary>
        /// <param name="obj">The secured object.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>Access level for current principal</returns>
        public AccessLevel GetAccessLevel<TAccessSecuredObject>(TAccessSecuredObject obj, IPrincipal principal) where TAccessSecuredObject : IAccessSecuredObject
        {
            var key = string.Format(AccessLevelCacheKeyPattern, obj.Id);

            object accessLevel = cacheService.Get(key, TimeSpan.FromMinutes(2), () => (object)GetAccessLevel(obj.AccessRules, principal));

            return (AccessLevel)accessLevel;
        }
       
        /// <summary>
        /// Updates the access control.
        /// </summary>
        /// <param name="obj">The secured object.</param>
        /// <param name="accessRules">The user access list.</param>
        public void UpdateAccessControl<TAccessSecuredObject>(TAccessSecuredObject obj, IList<IAccessRule> accessRules) where TAccessSecuredObject : IAccessSecuredObject
        {
            var entitiesToDelete = obj.AccessRules != null
                                       ? obj.AccessRules.Where(x => accessRules.All(model => model.Identity != x.Identity)).ToList()
                                       : new List<IAccessRule>();

            var entitesToAdd = accessRules
                                  .Where(x => obj.AccessRules == null || obj.AccessRules.All(entity => entity.Identity != x.Identity))
                                  .Select(f => new AccessRule
                                                   {
                                                       Identity = f.Identity,
                                                       AccessLevel = f.AccessLevel
                                                   })
                                  .ToList();


            //var entitiesToUpdate = GetEntitiesToUpdate(accessRules, obj.AccessRules);

            entitiesToDelete.ForEach(obj.RemoveRule);

            //entitiesToUpdate.ForEach(entity => repository.Save(entity));

            entitesToAdd.ForEach(obj.AddRule);
        }

        /// <summary>
        /// Gets the default access list.
        /// </summary>
        /// <returns></returns>
        public IList<IAccessRule> GetDefaultAccessList(IPrincipal principal = null)
        {
            IList<IAccessRule> list = new List<IAccessRule>();

            foreach (AccessControlElement userAccess in cmsConfiguration.DefaultAccessControlList)
            {
                list.Add(new UserAccessViewModel
                             {
                                 Identity = userAccess.Identity,
                                 AccessLevel = (AccessLevel)Enum.Parse(typeof(AccessLevel), userAccess.AccessLevel)
                             });
            }

            if (principal != null && principal.Identity.IsAuthenticated)
            {
                var identityName = principal.Identity.Name;
                var accessLevel = GetAccessLevel(list, principal);

                if (accessLevel != AccessLevel.ReadWrite && list.All(ua => ua.Identity != identityName))
                {
                    list.Add(new UserAccessViewModel
                    {
                        Identity = identityName,
                        AccessLevel = AccessLevel.ReadWrite
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// Gets the access level.
        /// </summary>
        /// <param name="accessList">The access list.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>Access level for current principal</returns>
        private AccessLevel GetAccessLevel(IList<IAccessRule> accessList, IPrincipal principal)
        {
            var accessLevel = AccessLevel.NoPermissions;

            // If there are no permissions, object is accessible to everyone:
            if (accessList == null || !accessList.Any())
            {
                return AccessLevel.ReadWrite;
            }

            // If user is not authenticated, check access level for "Everyone":
            var everyone = accessList.FirstOrDefault(x => string.Equals(x.Identity, SpecialIdentities.Everyone, StringComparison.OrdinalIgnoreCase));

            if (everyone != null)
            {
                accessLevel = everyone.AccessLevel;
            }

            // First check if there are explicit permissions for the user:
            if (principal.Identity.IsAuthenticated)
            {
                var identityName = principal.Identity.Name;
                var identityAccess = accessList.FirstOrDefault(x => string.Equals(x.Identity, identityName, StringComparison.OrdinalIgnoreCase));

                if (identityAccess != null)
                {
                    return identityAccess.AccessLevel;
                }

                // Check access level for "Authenticated User":
                var authenticated = accessList.FirstOrDefault(x => string.Equals(x.Identity, SpecialIdentities.AuthenticatedUsers, StringComparison.OrdinalIgnoreCase));

                if (authenticated != null && authenticated.AccessLevel > accessLevel)
                {
                    accessLevel = authenticated.AccessLevel;
                }

                // Check user or role access level:
                foreach (var userAccess in accessList)
                {
                    if (principal.IsInRole(userAccess.Identity))
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

    
        ///// <summary>
        ///// Gets the entities to update.
        ///// </summary>
        ///// <param name="accessList">The access list.</param>
        ///// <param name="entities">The entities.</param>
        ///// <returns></returns>
        //private static List<TAccess> GetEntitiesToUpdate<TAccess>(List<IAccess> accessList, List<TAccess> entities) where TAccess : Entity, IAccess, new()
        //{
        //    var entitiesToUpdate = new List<TAccess>();

        //    foreach (var entity in entities)
        //    {
        //        // Find user access object with the same Role and different AccessLevel:
        //        var userAccess = accessList.FirstOrDefault(x => x.RoleOrUser == entity.RoleOrUser && x.AccessLevel != entity.AccessLevel);

        //        // If found, add to updatables list:
        //        if (userAccess != null)
        //        {
        //            entity.AccessLevel = userAccess.AccessLevel;
        //            entitiesToUpdate.Add(entity);
        //        }
        //    }

        //    return entitiesToUpdate;
        //}
    }
}