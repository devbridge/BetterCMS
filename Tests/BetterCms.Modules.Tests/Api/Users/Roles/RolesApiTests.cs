// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RolesApiTests.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Users.Roles;
using BetterCms.Module.Api.Operations.Users.Roles.Role;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api.Users.Roles
{
    public class RolesApiTests : ApiCrudIntegrationTestBase<
        SaveRoleModel, RoleModel,
        PostRoleRequest, PostRoleResponse,
        GetRoleRequest, GetRoleResponse,
        PutRoleRequest, PutRoleResponse,
        DeleteRoleRequest, DeleteRoleResponse>
    {
        [Test]
        public void Should_CRUD_Role_Successfully()
        {
            // Attach to events
            Events.UserEvents.Instance.RoleCreated += Instance_EntityCreated;
            Events.UserEvents.Instance.RoleUpdated += Instance_EntityUpdated;
            Events.UserEvents.Instance.RoleDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Users.Roles.Post, api.Users.Role.Get, api.Users.Role.Put, api.Users.Role.Delete));

            // Detach from events
            Events.UserEvents.Instance.RoleCreated -= Instance_EntityCreated;
            Events.UserEvents.Instance.RoleUpdated -= Instance_EntityUpdated;
            Events.UserEvents.Instance.RoleDeleted -= Instance_EntityDeleted;
        }

        protected override SaveRoleModel GetCreateModel(ISession session)
        {
            return new SaveRoleModel
                   {
                       Name = TestDataProvider.ProvideRandomString(MaxLength.Name),
                       Description = TestDataProvider.ProvideRandomString(MaxLength.Name)
                   };
        }

        protected override GetRoleRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            return new GetRoleRequest { RoleId = saveResponseBase.Data.Value };
        }

        protected override PutRoleRequest GetUpdateRequest(GetRoleResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.Name = TestDataProvider.ProvideRandomString(MaxLength.Name);

            return request;
        }

        protected override void OnAfterGet(GetRoleResponse getResponse, SaveRoleModel model)
        {
            Assert.IsNotNull(getResponse.Data.Name);
            Assert.IsNotNull(getResponse.Data.Description);

            Assert.AreEqual(getResponse.Data.Name, model.Name);
            Assert.AreEqual(getResponse.Data.Description, model.Description);
        }
    }
}
