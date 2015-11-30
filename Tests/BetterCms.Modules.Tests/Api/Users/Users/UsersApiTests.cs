// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersApiTests.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Users.Users;
using BetterCms.Module.Api.Operations.Users.Users.User;

using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

using UserModel = BetterCms.Module.Api.Operations.Users.Users.User.UserModel;

namespace BetterCms.Test.Module.Api.Users.Users
{
    public class UsersApiTests : ApiCrudIntegrationTestBase<
        SaveUserModel, UserModel,
        PostUserRequest, PostUserResponse,
        GetUserRequest, GetUserResponse,
        PutUserRequest, PutUserResponse,
        DeleteUserRequest, DeleteUserResponse>
    {
        [Test]
        public void Should_CRUD_User_Successfully()
        {
            // Attach to events
            Events.UserEvents.Instance.UserCreated += Instance_EntityCreated;
            Events.UserEvents.Instance.UserUpdated += Instance_EntityUpdated;
            Events.UserEvents.Instance.UserDeleted += Instance_EntityDeleted;

            RunApiActionInTransaction((api, session) =>
                Run(session, api.Users.Users.Post, api.Users.User.Get, api.Users.User.Put, api.Users.User.Delete));

            // Detach from events
            Events.UserEvents.Instance.UserCreated -= Instance_EntityCreated;
            Events.UserEvents.Instance.UserUpdated -= Instance_EntityUpdated;
            Events.UserEvents.Instance.UserDeleted -= Instance_EntityDeleted;
        }

        protected override SaveUserModel GetCreateModel(ISession session)
        {
            var user = TestDataProvider.CreateNewUser();
            session.SaveOrUpdate(user.Image);

            return new SaveUserModel
                   {
                       FirstName = user.FirstName,
                       LastName = user.LastName,
                       UserName = user.UserName,
                       Email = user.Email,
                       Password = user.Password,
                       ImageId = user.Image.Id,
                       Roles = new List<string> { TestDataProvider.ProvideRandomString(20), TestDataProvider.ProvideRandomString(20) }
                   };
        }

        protected override GetUserRequest GetGetRequest(BetterCms.Module.Api.Infrastructure.SaveResponseBase saveResponseBase)
        {
            var request =  new GetUserRequest { UserId = saveResponseBase.Data.Value };
            request.Data.IncludeRoles = true;

            return request;
        }

        protected override PutUserRequest GetUpdateRequest(GetUserResponse getResponse)
        {
            var request = getResponse.ToPutRequest();
            request.Data.UserName = TestDataProvider.ProvideRandomString(MaxLength.Name);

            return request;
        }

        protected override void OnAfterGet(GetUserResponse getResponse, SaveUserModel model)
        {
            Assert.IsNotNull(getResponse.Data.FirstName);
            Assert.IsNotNull(getResponse.Data.LastName);
            Assert.IsNotNull(getResponse.Data.UserName);
            Assert.IsNotNull(getResponse.Data.Email);
            Assert.IsNotNull(getResponse.Data.ImageId);
            Assert.IsNotNull(getResponse.Roles);
            Assert.IsNotEmpty(getResponse.Roles);

            Assert.AreEqual(getResponse.Data.FirstName, model.FirstName);
            Assert.AreEqual(getResponse.Data.LastName, model.LastName);
            Assert.AreEqual(getResponse.Data.UserName, model.UserName);
            Assert.AreEqual(getResponse.Data.Email, model.Email);
            Assert.AreEqual(getResponse.Data.ImageId, model.ImageId);
            
            Assert.AreEqual(getResponse.Roles.Count, model.Roles.Count);
            Assert.AreEqual(getResponse.Roles.Count, 2);
            Assert.IsTrue(getResponse.Roles.All(t1 => model.Roles.Any(t2 => t2 == t1.Name)));
        }
    }
}
