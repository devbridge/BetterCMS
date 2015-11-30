// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MembershipProviderTests.cs" company="Devbridge Group LLC">
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
using System.Linq;
using System.Web.Security;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.Users.Models;
using BetterCms.Module.Users.Provider;
using BetterCms.Module.Users.Services;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Users.ProvidersTests
{
    public class MembershipProviderTests : IntegrationTestBase
    {
        private const string FakeUserName = "85A36264-1B60-4F40-B8C6-515904856792D4B500B8-8912-4D1A-B882-68934F115DA9";
        private const string FakeUserPassword = "9ECAC589-DAA7-4D23-8E31-F8BCF1C0667177CBD3FC-13E0-4113-A399-C4A4F09131FE";

        /// <summary>
        /// The repository
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// The authentication service
        /// </summary>
        private IAuthenticationService authenticationService;

        [Test]
        public void Should_Call_ValidateUser_Successfully()
        {
            RunActionInTransaction(session =>
            {
                Clear();
                CreateFakeUser(session);

                var provider = GetMembershipProvider(session);
                var result = provider.ValidateUser(FakeUserName, FakeUserPassword);

                Assert.IsTrue(result);
            });
        }
        
        [Test]
        public void Should_Call_ValidateUser_WrongUsername()
        {
            RunActionInTransaction(session =>
            {
                Clear();
                CreateFakeUser(session);

                var provider = GetMembershipProvider(session);
                var result = provider.ValidateUser(FakeUserName + ".", FakeUserPassword);

                Assert.IsFalse(result);
            });
        }
        
        [Test]
        public void Should_Call_ValidateUser_WrongPassword()
        {
            RunActionInTransaction(session =>
            {
                Clear();
                CreateFakeUser(session);

                var provider = GetMembershipProvider(session);
                var result = provider.ValidateUser(FakeUserName + ".", FakeUserPassword);

                Assert.IsFalse(result);
            });
        }

        [Test]
        public void Should_Call_ChangePassword_Successfully()
        {
            RunActionInTransaction(session =>
            {
                Clear();
                CreateFakeUser(session);

                var provider = GetMembershipProvider(session);
                
                var newPassword = "new password";
                Assert.IsTrue(provider.ChangePassword(FakeUserName, FakeUserPassword, newPassword));
                Assert.IsTrue(provider.ValidateUser(FakeUserName, newPassword));
            });
        }

        [Test]
        public void Should_Call_ChangePassword_WrongOldPassword()
        {
            RunActionInTransaction(session =>
            {
                Clear();
                CreateFakeUser(session);

                var provider = GetMembershipProvider(session);

                Assert.IsFalse(provider.ChangePassword(FakeUserName, "wrong password", "new password"));
            });
        }

        [Test]
        public void Should_Call_FindUsersByEmail_Successfully()
        {
            RunActionInTransaction(session =>
            {
                Clear();

                const string userNameSuffix = "F8484E84-064E-4C86-9807-DC28CE782068";
                var user1 = CreateFakeUser(session, u =>
                        {
                            u.UserName = TestDataProvider.ProvideRandomString(100);
                            u.Email = TestDataProvider.ProvideRandomString(5) + userNameSuffix + TestDataProvider.ProvideRandomString(5);
                        });
                var user2 = CreateFakeUser(session, u =>
                        {
                            u.UserName = TestDataProvider.ProvideRandomString(100);
                            u.Email = TestDataProvider.ProvideRandomString(5) + userNameSuffix + TestDataProvider.ProvideRandomString(5);
                        });
                var user3 = CreateFakeUser(session, u =>
                        {
                            u.UserName = TestDataProvider.ProvideRandomString(100);
                            u.Email = TestDataProvider.ProvideRandomString(5) + userNameSuffix + TestDataProvider.ProvideRandomString(5);
                        });
                var user4 = CreateFakeUser(session, u =>
                        {
                            u.UserName = TestDataProvider.ProvideRandomString(100);
                            u.Email = TestDataProvider.ProvideRandomString(5) + userNameSuffix + TestDataProvider.ProvideRandomString(5);
                        });
                var user5 = CreateFakeUser(session, u =>
                        {
                            u.UserName = TestDataProvider.ProvideRandomString(100);
                            u.Email = TestDataProvider.ProvideRandomString(5) + userNameSuffix + TestDataProvider.ProvideRandomString(5);
                        });
                CreateFakeUser(session, u =>
                        {
                            u.UserName = TestDataProvider.ProvideRandomString(100);
                            u.Email = TestDataProvider.ProvideRandomString(100);
                        });
                var allUsers = new[] { user1, user2, user3, user4, user5 }.OrderBy(u => u.UserName).ToArray();

                var provider = GetMembershipProvider(session);

                int totalRecords;
                var users = provider.FindUsersByEmail(userNameSuffix, 2, 2, out totalRecords);

                Assert.IsNotNull(users);
                Assert.AreEqual(totalRecords, 5);
                Assert.AreEqual(users.Count, 2);
                Assert.IsTrue(users.Cast<MembershipUser>().Any(u => u.UserName == allUsers[2].UserName));
                Assert.IsTrue(users.Cast<MembershipUser>().Any(u => u.UserName == allUsers[3].UserName));
            });
        }

        [Test]
        public void Should_Call_FindUsersByName_Successfully()
        {
            RunActionInTransaction(session =>
            {
                Clear();

                const string userNameSuffix = "F8484E84-064E-4C86-9807-DC28CE782068";
                var user1 = CreateFakeUser(session, u => u.UserName = TestDataProvider.ProvideRandomString(5) + userNameSuffix + TestDataProvider.ProvideRandomString(5));
                var user2 = CreateFakeUser(session, u => u.UserName = TestDataProvider.ProvideRandomString(5) + userNameSuffix + TestDataProvider.ProvideRandomString(5));
                var user3 = CreateFakeUser(session, u => u.UserName = TestDataProvider.ProvideRandomString(5) + userNameSuffix + TestDataProvider.ProvideRandomString(5));
                var user4 = CreateFakeUser(session, u => u.UserName = TestDataProvider.ProvideRandomString(5) + userNameSuffix + TestDataProvider.ProvideRandomString(5));
                var user5 = CreateFakeUser(session, u => u.UserName = TestDataProvider.ProvideRandomString(5) + userNameSuffix + TestDataProvider.ProvideRandomString(5));
                CreateFakeUser(session, u => u.UserName = TestDataProvider.ProvideRandomString(100));
                var allUsers = new[] { user1, user2, user3, user4, user5 }.OrderBy(u => u.UserName).ToArray();

                var provider = GetMembershipProvider(session);

                int totalRecords;
                var users = provider.FindUsersByName(userNameSuffix, 2, 2, out totalRecords);

                Assert.IsNotNull(users);
                Assert.AreEqual(totalRecords, 5);
                Assert.AreEqual(users.Count, 2);
                Assert.IsTrue(users.Cast<MembershipUser>().Any(u => u.UserName == allUsers[2].UserName));
                Assert.IsTrue(users.Cast<MembershipUser>().Any(u => u.UserName == allUsers[3].UserName));
            });
        }
        
        [Test]
        public void Should_Call_GetAllUsers_Successfully()
        {
            RunActionInTransaction(session =>
            {
                Clear();

                CreateFakeUser(session, u => u.UserName = TestDataProvider.ProvideRandomString(100));
                CreateFakeUser(session, u => u.UserName = TestDataProvider.ProvideRandomString(100));
                CreateFakeUser(session, u => u.UserName = TestDataProvider.ProvideRandomString(100));

                var provider = GetMembershipProvider(session);

                int totalRecords;
                var users = provider.GetAllUsers(1, 2, out totalRecords);

                Assert.IsNotNull(users);
                Assert.GreaterOrEqual(totalRecords, 3);
                Assert.AreEqual(users.Count, 2);
            });
        }
        
        [Test]
        public void Should_Call_GetUserNameByEmail_Successfully()
        {
            RunActionInTransaction(session =>
            {
                Clear();

                var provider = GetMembershipProvider(session);

                var user = CreateFakeUser(session);
                var username = provider.GetUserNameByEmail(user.Email);

                Assert.IsNotNull(username);
                Assert.AreEqual(username, user.UserName);
            });
        }

        #region Private

        private CmsMembershipProvider GetMembershipProvider(ISession session)
        {
            var userService = new DefaultUserService(GetRepository(session), GetAuthenticationService(session), GetUnitOfWork(session));
            var roleProvider = new CmsMembershipProvider(userService, authenticationService, GetUnitOfWork(session), "CmsMembershipProvider");

            return roleProvider;
        }

        private User CreateFakeUser(ISession session, Action<User> beforeSave = null)
        {
            var service = GetAuthenticationService(session);

            var user = TestDataProvider.CreateNewUser();
            user.UserName = FakeUserName;
            user.Salt = service.GeneratePasswordSalt();
            user.Password = service.CreatePasswordHash(FakeUserPassword, user.Salt);

            if (beforeSave != null)
            {
                beforeSave.Invoke(user);
            }

            session.SaveOrUpdate(user);
            session.Flush();
            session.Clear();

            return user;
        }

        private IRepository GetRepository(ISession session)
        {
            if (repository == null)
            {
                repository = new DefaultRepository(GetUnitOfWork(session));
            }

            return repository;
        }

        private IUnitOfWork GetUnitOfWork(ISession session)
        {
            if (unitOfWork == null)
            {
                unitOfWork = new DefaultUnitOfWork(session);
            }

            return unitOfWork;
        }

        private IAuthenticationService GetAuthenticationService(ISession session)
        {
            if (authenticationService == null)
            {
                authenticationService = new DefaultAuthenticationService(GetRepository(session));
            }

            return authenticationService;
        }


        private void Clear()
        {
            repository = null;
            unitOfWork = null;
            authenticationService = null;
        }

        #endregion
    }
}
