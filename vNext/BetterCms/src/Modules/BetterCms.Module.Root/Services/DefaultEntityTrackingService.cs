using System;
using BetterCms.Configuration;
using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.DataContracts;
using Microsoft.Framework.OptionsModel;
using NHibernate.Proxy.DynamicProxy;

namespace BetterCms.Module.Root.Services
{
    public class DefaultEntityTrackingService : IEntityTrackingService
    {
        private readonly CmsConfigurationSection configuration;
        private readonly IEntityTrackingCacheService cacheService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IAccessControlService accessControlService;
        private readonly ISecurityService securityService;

        public DefaultEntityTrackingService(IOptions<CmsConfigurationSection> configuration, IEntityTrackingCacheService cacheService, 
            IUnitOfWork unitOfWork, IAccessControlService accessControlService, ISecurityService securityService)
        {
            this.configuration = configuration.Value;
            this.cacheService = cacheService;
            this.unitOfWork = unitOfWork;
            this.accessControlService = accessControlService;
            this.securityService = securityService;
        }

        public void OnEntityUpdate(IEntity entity)
        {
            CheckAccessControl(entity);
        }

        public void OnEntityDelete(IEntity entity)
        {
            CheckAccessControl(entity);
        }

        private void CheckAccessControl(IEntity entity)
        {
            if (configuration.Security.AccessControlEnabled)
            {
                if (entity is IAccessSecuredObject && !(entity is IAccessControlDisabled) && entity.Id != Guid.Empty)
                {
                    DemandReadWriteRule((IAccessSecuredObject)entity);
                }

                if (entity is IAccessSecuredObjectDependency)
                {
                    var dependency = (IAccessSecuredObjectDependency)entity;
                    if (dependency.SecuredObject.Id != Guid.Empty && !(dependency.SecuredObject is IAccessControlDisabled))
                    {
                        DemandReadWriteRule(dependency.SecuredObject);
                    }
                }
            }
        }

        private void DemandReadWriteRule(IAccessSecuredObject item)
        {
            // Do not demand access, if user set SaveUnsecured to true explicilty
            if (item.SaveUnsecured)
            {
                return;
            }

            try
            {
                Type itemType;

                if (item is IProxy)
                {
                    itemType = item.GetType().BaseType;
                }
                else
                {
                    itemType = item.GetType();
                }

                object securedObject;
                if (!cacheService.GetEntity(itemType, item.Id, out securedObject))
                {
                    securedObject = unitOfWork.Session.Get(itemType, item.Id);
                    cacheService.AddEntity(itemType, item.Id, securedObject);
                }

                if (securedObject != null)
                {

                    var principal = securityService.GetCurrentPrincipal();

                    if (accessControlService.GetAccessLevel((IAccessSecuredObject)securedObject, principal) != AccessLevel.ReadWrite)
                    {
                        throw new ValidationException(
                            () => string.Format(RootGlobalization.Validation_CurrentUserHasNoRightsToUpdateOrDelete_Message, principal.Identity.Name, item.Title),
                            $"Current user {principal.Identity.Name} has no rights to update or delete secured object {item}.");
                    }
                }
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CmsException(string.Format("Failed to check an access level of current user for the record {0}.", item), ex);
            }
        }
    }
}