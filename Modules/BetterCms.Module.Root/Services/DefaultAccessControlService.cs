using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

using BetterCms.Configuration;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.DataContracts;
using BetterModules.Core.Web.Services.Caching;

namespace BetterCms.Module.Root.Services
{
    /// <summary>
    /// Implements access control for objects.
    /// </summary>
    public class DefaultAccessControlService : IAccessControlService
    {
        private const int DefaultCacheTimeoutInSeconds = 30;

        private const string AccessLevelCacheKeyPrefix = "bcms-useraccess";

        private readonly ICacheService cacheService;

        private readonly ICmsConfiguration configuration;

        private readonly ISecurityService securityService;
        
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAccessControlService" /> class.
        /// </summary>
        /// <param name="securityService">The security service.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="configuration">The CMS configuration.</param>
        /// <param name="repository">The repository.</param>
        public DefaultAccessControlService(ISecurityService securityService, ICacheService cacheService,
            ICmsConfiguration configuration, IRepository repository)
        {
            this.securityService = securityService;
            this.cacheService = cacheService;
            this.configuration = configuration;
            this.repository = repository;
        }

        /// <summary>
        /// Demands the access.
        /// </summary>
        /// <typeparam name="TAccessSecuredObject">The type of the access secured object.</typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="accessLevel">The access level.</param>
        /// <param name="roles">The roles.</param>
        /// <exception cref="SecurityException"></exception>
        /// <exception cref="System.NotImplementedException"></exception>
        public void DemandAccess<TAccessSecuredObject>(TAccessSecuredObject obj, IPrincipal principal, AccessLevel accessLevel, params string[] roles)
            where TAccessSecuredObject : IAccessSecuredObject
        {
            DemandAccess(principal, roles);

            var principalAccessLevel = GetAccessLevel(obj, principal);
            if (principalAccessLevel < accessLevel || principalAccessLevel == AccessLevel.Deny)
            {
                throw new SecurityException(string.Format("Forbidden: Access is denied for the object {0}.", obj));
            }
        }

        /// <summary>
        /// Demands the access level by roles only.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="roles">The roles.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void DemandAccess(IPrincipal principal, params string[] roles)
        {          
            if (roles != null && roles.Any() && !securityService.IsAuthorized(principal, string.Join(",", roles)))
            {
                throw new SecurityException("Forbidden: Access is denied.");
            }
        }

        /// <summary>
        /// Gets the access level.
        /// </summary>
        /// <typeparam name="TAccessSecuredObject">The type of the access secured object.</typeparam>
        /// <param name="obj">The secured object.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="useCache">if set to <c>true</c> use cache.</param>
        /// <returns>
        /// Access level for current principal
        /// </returns>
        public AccessLevel GetAccessLevel<TAccessSecuredObject>(TAccessSecuredObject obj, IPrincipal principal, bool useCache = true) where TAccessSecuredObject : IAccessSecuredObject
        {
            return GetAccessLevel(obj.AccessRules, principal, useCache);
        }

        /// <summary>
        /// Gets the access level.
        /// </summary>
        /// <param name="accessRules">The access list.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="useCache">if set to <c>true</c> use cache.</param>
        /// <returns>
        /// Access level for current principal
        /// </returns>
        public AccessLevel GetAccessLevel(IList<IAccessRule> accessRules, IPrincipal principal, bool useCache = true)
        {
            if (!useCache)
            {
                return GetAccessLevelInternal(accessRules, principal);
            }
            
            StringBuilder cacheKeyBuilder = new StringBuilder();

            cacheKeyBuilder.Append(AccessLevelCacheKeyPrefix);
            cacheKeyBuilder.Append("-");
            cacheKeyBuilder.Append(principal.Identity.Name ?? "Anonymous");
            cacheKeyBuilder.Append("-");
            cacheKeyBuilder.Append(principal.Identity.IsAuthenticated);
            cacheKeyBuilder.Append("-");

            StringBuilder accessRulesHasher = new StringBuilder();
            if (accessRules != null && accessRules.Count > 0)
            {
                foreach (var rule in accessRules.Distinct())
                {
                    accessRulesHasher.Append(rule.Identity);
                    accessRulesHasher.Append("-");
                    accessRulesHasher.Append((int)rule.AccessLevel);
                    accessRulesHasher.Append("-");
                    accessRulesHasher.Append(rule.IsForRole ? "role" : "user");
                    accessRulesHasher.Append("-");
                }
            }
            else
            {
                accessRulesHasher.Append("NoAccessRoles");
            }
            cacheKeyBuilder.Append(accessRulesHasher.ToString().GetHashCode());

            object accessLevel = cacheService.Get(cacheKeyBuilder.ToString(), TimeSpan.FromSeconds(DefaultCacheTimeoutInSeconds), () => (object)GetAccessLevelInternal(accessRules, principal));

            return (AccessLevel)accessLevel;
        }

        /// <summary>
        /// Updates the access control.
        /// </summary>
        /// <param name="securedObject">The secured object.</param>
        /// <param name="updatedRules">The user access list.</param>
        public void UpdateAccessControl<TAccessSecuredObject>(TAccessSecuredObject securedObject, IList<IAccessRule> updatedRules) where TAccessSecuredObject : IAccessSecuredObject
        {
            var accessRulesEquals = new Func<IAccessRule, IAccessRule, bool>((a, b) =>
                {
                    return a.Identity.Equals(b.Identity, StringComparison.OrdinalIgnoreCase) && a.IsForRole == b.IsForRole;
                });

            var entitiesToDelete = securedObject.AccessRules != null
                                       ? securedObject.AccessRules.Where(storedRule => updatedRules.All(updatedRule => !accessRulesEquals(updatedRule, storedRule))).ToList()
                                       : new List<IAccessRule>();

            var entitesToAdd = updatedRules
                                  .Where(updatedRule => securedObject.AccessRules == null || securedObject.AccessRules.All(storedRule => !accessRulesEquals(updatedRule, storedRule)))
                                  .Select(f => new AccessRule
                                                   {
                                                       Identity = f.Identity,
                                                       AccessLevel = f.AccessLevel,
                                                       IsForRole = f.IsForRole
                                                   })
                                  .ToList();


            entitiesToDelete.ForEach(securedObject.RemoveRule);

            entitesToAdd.ForEach(securedObject.AddRule);

            UpdateChangedRules(securedObject, updatedRules);

            var readWriteIsDefault = string.Equals(configuration.Security.DefaultAccessRules.DefaultAccessLevel, AccessLevel.ReadWrite.ToString(), StringComparison.OrdinalIgnoreCase);

            if (securedObject.AccessRules == null && !readWriteIsDefault  || 
                securedObject.AccessRules != null && !securedObject.AccessRules.Any() && !readWriteIsDefault ||
                securedObject.AccessRules != null && securedObject.AccessRules.Any() && securedObject.AccessRules.All(f => f.AccessLevel != AccessLevel.ReadWrite))
            {
                if (securityService.HasFullAccess(securityService.GetCurrentPrincipal()))
                {
                    return;
                }

                throw new ValidationException(() => RootGlobalization.Validation_SecuredObjectShouldHaveAccess_Message, 
                    string.Format("An '{0}' secured object can't be saved because of the complete access lose.", securedObject));
            }
        }

        /// <summary>
        /// Gets the default access list.
        /// </summary>
        /// <returns></returns>
        public IList<IAccessRule> GetDefaultAccessList(IPrincipal principal = null)
        {
            IList<IAccessRule> list = new List<IAccessRule>();

            foreach (AccessControlElement userAccess in configuration.Security.DefaultAccessRules)
            {
                list.Add(new UserAccessViewModel
                             {
                                 Identity = userAccess.Identity,
                                 AccessLevel = (AccessLevel)Enum.Parse(typeof(AccessLevel), userAccess.AccessLevel),
                                 IsForRole = userAccess.IsRole
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
                        AccessLevel = AccessLevel.ReadWrite,
                        IsForRole = false
                    });
                }
            }

            return list.OrderBy(x => x.IsForRole).ThenBy(x => x.Identity).ToList();
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
                    var rule = updatedRules.FirstOrDefault(x => x.Identity == entity.Identity && x.IsForRole == entity.IsForRole && x.AccessLevel != entity.AccessLevel);

                    // If found, update.
                    if (rule != null)
                    {
                        entity.AccessLevel = rule.AccessLevel;
                    }
                }
            }
        }

        private AccessLevel GetAccessLevelInternal(IList<IAccessRule> accessRules, IPrincipal principal)
        {
            // If user is in full access role - object is accessible to him.
            if (securityService.HasFullAccess(principal))
            {
                return AccessLevel.ReadWrite;
            }

            // If there are no permissions, object is accessible to everyone:
            if (accessRules == null || !accessRules.Any())
            {
                string defaultAccessLevelConfig = configuration.Security.DefaultAccessRules.DefaultAccessLevel;

                if (!string.IsNullOrWhiteSpace(defaultAccessLevelConfig))
                {
                    AccessLevel defaultAccessLevel;
                    if (Enum.TryParse(defaultAccessLevelConfig, true, out defaultAccessLevel))
                    {
                        return defaultAccessLevel;
                    }

                    string formatErrorMessage =
                        string.Format(
                            "A defaultAccessLevel property '{0}' does not matches any possible value in the cms.config <security> section. Available values are ReadWrite|Read|Deny.",
                            defaultAccessLevelConfig);

                    throw new CmsException(formatErrorMessage);
                }

                StringBuilder notDefinedMessage = new StringBuilder();
                notDefinedMessage.AppendLine("A defaultAccessLevel property is not defined in the cms.config <security> section. This access level is used for objects with no access rules.");
                notDefinedMessage.AppendLine("<security><defaultAccessRules defaultAccessLevel=\"ReadWrite|Read|Deny\">...</defaultAccessRules></security>");

                throw new CmsException(notDefinedMessage.ToString());
            }

            var accessLevel = AccessLevel.Deny;

            // If user is not authenticated, check access level for "Everyone" role:
            var everyone = accessRules.FirstOrDefault(rule => rule.IsForRole && string.Equals(rule.Identity, SpecialIdentities.Everyone, StringComparison.OrdinalIgnoreCase));

            if (everyone != null)
            {
                accessLevel = everyone.AccessLevel;
            }

            // First check if there are explicit permissions for the user:
            if (principal.Identity.IsAuthenticated)
            {
                var identityName = principal.Identity.Name;
                var identityAccess = accessRules.FirstOrDefault(rule => !rule.IsForRole && string.Equals(rule.Identity, identityName, StringComparison.OrdinalIgnoreCase));

                if (identityAccess != null)
                {
                    return identityAccess.AccessLevel;
                }

                // Check access level for "Authenticated User" role:
                var authenticated = accessRules.FirstOrDefault(x => x.IsForRole && string.Equals(x.Identity, SpecialIdentities.AuthenticatedUsers, StringComparison.OrdinalIgnoreCase));

                if (authenticated != null && authenticated.AccessLevel > accessLevel)
                {
                    accessLevel = authenticated.AccessLevel;
                }

                // Check principal access level by roles:
                foreach (var rule in accessRules.Where(rule => rule.IsForRole))
                {
                    if (principal.IsInRole(rule.Identity))
                    {
                        // Highest available privilege wins:
                        if (rule.AccessLevel > accessLevel)
                        {
                            accessLevel = rule.AccessLevel;
                        }
                    }
                }
            }

            return accessLevel;
        }

        /// <summary>
        /// Gets the denied objects.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="useCache">if set to <c>true</c> use cache.</param>
        /// <returns></returns>
        public IEnumerable<Guid> GetDeniedObjects<TEntity>(bool useCache = true)
            where TEntity : IEntity, IAccessSecuredObject
        {
            var principal = securityService.GetCurrentPrincipal();

            return GetPrincipalDeniedObjects<TEntity>(principal, useCache);
        }

        /// <summary>
        /// Gets the principal denied objects.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="principal">The principal.</param>
        /// <param name="useCache">if set to <c>true</c> use cache.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<Guid> GetPrincipalDeniedObjects<TEntity>(IPrincipal principal, bool useCache = true)
             where TEntity : IEntity, IAccessSecuredObject
        {
            IEnumerable<TEntity> list;

            if (useCache)
            {
                var cacheKey = string.Format("CMS_{1}_{0}_C9E7517250F64F84ADC8-B991C8391306", principal.Identity.Name, typeof(TEntity));
                list = cacheService.Get(cacheKey, new TimeSpan(0, 0, 0, DefaultCacheTimeoutInSeconds), LoadDeniedObjects<TEntity>);
            }
            else
            {
                list = LoadDeniedObjects<TEntity>();
            }

            foreach (var entity in list)
            {
                var accessLevel = GetAccessLevel(entity, principal, useCache);
                if (accessLevel == AccessLevel.Deny)
                {
                    yield return ((IEntity)entity).Id;
                }
            }
        }

        /// <summary>
        /// Loads the list of denied objects.
        /// </summary>
        /// <returns>The list of denied page</returns>
        private IEnumerable<TModel> LoadDeniedObjects<TModel>() where TModel : IEntity, IAccessSecuredObject
        {
            return repository
                .AsQueryable<TModel>()
                .Where(f => f.AccessRules.Any(b => b.AccessLevel == AccessLevel.Deny))
                .FetchMany(f => f.AccessRules)
                .ToList()
                .Distinct();
        }
    }
}