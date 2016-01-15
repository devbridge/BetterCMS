// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultEntityTrackingService.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;

using Autofac;

using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.DataContracts;
using BetterModules.Core.Dependencies;

using NHibernate.Proxy.DynamicProxy;

namespace BetterCms.Module.Root.Services
{
    public class DefaultEntityTrackingService : IEntityTrackingService
    {
        private readonly ICmsConfiguration configuration;
        private readonly IEntityTrackingCacheService cacheService;

        public DefaultEntityTrackingService(ICmsConfiguration configuration, IEntityTrackingCacheService cacheService)
        {
            this.configuration = configuration;
            this.cacheService = cacheService;
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
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    var unitOfWork = container.Resolve<IUnitOfWork>();
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
                        var accessControlService = container.Resolve<IAccessControlService>();
                        var securityService = container.Resolve<ISecurityService>();

                        var principal = securityService.GetCurrentPrincipal();

                        if (accessControlService.GetAccessLevel((IAccessSecuredObject)securedObject, principal) != AccessLevel.ReadWrite)
                        {
                            throw new ValidationException(
                                () => string.Format(RootGlobalization.Validation_CurrentUserHasNoRightsToUpdateOrDelete_Message, principal.Identity.Name, item.Title),
                                string.Format("Current user {0} has no rights to update or delete secured object {1}.", principal.Identity.Name, item));
                        }
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