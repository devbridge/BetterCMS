using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

using BetterCms.Configuration;
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
        private const string AccessLevelCacheKeyPrefix = "bcms-useraccess-";

        private readonly ICacheService cacheService;

        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAccessControlService" /> class.
        /// </summary>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public DefaultAccessControlService(ICacheService cacheService, ICmsConfiguration cmsConfiguration)
        {
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
            StringBuilder cacheKeyBuilder = new StringBuilder();

            cacheKeyBuilder.Append(AccessLevelCacheKeyPrefix);
            cacheKeyBuilder.Append("-");
            cacheKeyBuilder.Append(principal.Identity.Name ?? "Anonymous");
            cacheKeyBuilder.Append("-");
            cacheKeyBuilder.Append(principal.Identity.IsAuthenticated);
            cacheKeyBuilder.Append("-");

            StringBuilder accessRulesHasher = new StringBuilder();
            if (obj.AccessRules != null && obj.AccessRules.Count > 0)
            {                
                foreach (var rule in obj.AccessRules)
                {
                    accessRulesHasher.Append(rule.Identity);
                    accessRulesHasher.Append("-");
                    accessRulesHasher.Append((int)rule.AccessLevel);
                    accessRulesHasher.Append("-");
                }                 
            }
            else
            {
                accessRulesHasher.Append("NoAccessRoles");
            }
            cacheKeyBuilder.Append(accessRulesHasher.ToString().GetHashCode());

            object accessLevel = cacheService.Get(cacheKeyBuilder.ToString(), TimeSpan.FromMinutes(2), () => (object)GetAccessLevel(obj.AccessRules, principal));

            return (AccessLevel)accessLevel;
        }
       
        /// <summary>
        /// Updates the access control.
        /// </summary>
        /// <param name="securedObject">The secured object.</param>
        /// <param name="updatedRules">The user access list.</param>
        public void UpdateAccessControl<TAccessSecuredObject>(TAccessSecuredObject securedObject, IList<IAccessRule> updatedRules) where TAccessSecuredObject : IAccessSecuredObject
        {
            var entitiesToDelete = securedObject.AccessRules != null
                                       ? securedObject.AccessRules.Where(x => updatedRules.All(model => model.Identity != x.Identity)).ToList()
                                       : new List<IAccessRule>();

            var entitesToAdd = updatedRules
                                  .Where(x => securedObject.AccessRules == null || securedObject.AccessRules.All(entity => entity.Identity != x.Identity))
                                  .Select(f => new AccessRule
                                                   {
                                                       Identity = f.Identity,
                                                       AccessLevel = f.AccessLevel
                                                   })
                                  .ToList();


            entitiesToDelete.ForEach(securedObject.RemoveRule);

            entitesToAdd.ForEach(securedObject.AddRule);

            UpdateChangedRules(securedObject, updatedRules);
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


        /// <summary>
        /// Update the access rule entities.
        /// </summary>
        /// <param name="securedObject">The secured object.</param>
        /// <param name="updatedRules">The access list.</param>        
        private void UpdateChangedRules(IAccessSecuredObject securedObject, IList<IAccessRule> updatedRules)
        {
            if (updatedRules != null && updatedRules.Count > 0 && securedObject.AccessRules != null)
            {
                var existingAccessRules = securedObject.AccessRules.ToList();

                foreach (var entity in existingAccessRules)
                {
                    // Find access rule object with the same Role and different AccessLevel.
                    var rule = updatedRules.FirstOrDefault(x => x.Identity == entity.Identity && x.AccessLevel != entity.AccessLevel);

                    // If found, update.
                    if (rule != null)
                    {
                        entity.AccessLevel = rule.AccessLevel;
                    }
                }
            }
        }
    }
}