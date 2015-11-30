// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersModuleServiceValidationTest.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api;

using NUnit.Framework;

namespace BetterCms.Test.Module.Api
{
    [TestFixture]
    public class UsersModuleServiceValidationTest : TestBase
    {
        [Test]
        public void ShouldCreateAndDisposeApiFacade()
        {
            using (var api = ApiFactory.Create())
            {
                Assert.IsNotNull(api.Users);
                Assert.IsNotNull(api.Users.User);
                Assert.IsNotNull(api.Users.Users);
                Assert.IsNotNull(api.Users.Role);
                Assert.IsNotNull(api.Users.Roles);

                Assert.AreEqual(api.Users.Users.GetType(), System.Type.GetType("BetterCms.Module.Users.Api.Operations.Users.Users.UsersService,BetterCms.Module.Users.Api"));
                Assert.AreEqual(api.Users.User.GetType(), System.Type.GetType("BetterCms.Module.Users.Api.Operations.Users.Users.User.UserService,BetterCms.Module.Users.Api"));
                Assert.AreEqual(api.Users.Role.GetType(), System.Type.GetType("BetterCms.Module.Users.Api.Operations.Users.Roles.Role.RoleService,BetterCms.Module.Users.Api"));
                Assert.AreEqual(api.Users.Roles.GetType(), System.Type.GetType("BetterCms.Module.Users.Api.Operations.Users.Roles.RolesService,BetterCms.Module.Users.Api"));
            }
        }
    }
}
