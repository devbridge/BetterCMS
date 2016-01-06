using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;

using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Users.Models;
using BetterCms.Module.Users.Provider;
using BetterCms.Module.Users.Services;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Models;

using NHibernate;

using NUnit.Framework;

namespace BetterCms.Test.Module.Users.ProvidersTests
{
    public class RoleProviderTests : IntegrationTestBase
    {
        [Test]
        public void Should_Call_GetAllRoles_Successfully()
        {
            RunActionInTransaction(session =>
                {
                    var fakeRoles = CreateFakeRoles(session, 3);

                    var roleProvider = GetRoleProvider(session);
                    var roles = roleProvider.GetAllRoles();

                    Assert.NotNull(roles);
                    Assert.GreaterOrEqual(roles.Length, fakeRoles.Length);
                    foreach (var role in fakeRoles)
                    {
                        Assert.IsTrue(roles.Contains(role.Name));
                    }
                });
        }

        [Test]
        public void Should_Call_GetRolesForUser_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var fakeRoles = CreateFakeRoles(session, 3);
                var userRoles = new[] { fakeRoles[0], fakeRoles[1] };
                var fakeUser = CreateFakeUsers(session, userRoles, 1)[0];

                var roleProvider = GetRoleProvider(session);
                var roles = roleProvider.GetRolesForUser(fakeUser.UserName);

                Assert.NotNull(roles);
                Assert.AreEqual(roles.Length, 2);
                foreach (var role in userRoles)
                {
                    Assert.IsTrue(roles.Contains(role.Name));
                }
            });
        }

        [Test]
        public void Should_Call_IsUserInRole_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var fakeRoles = CreateFakeRoles(session, 3);
                var userRoles = new[] { fakeRoles[0], fakeRoles[1] };
                var fakeUser = CreateFakeUsers(session, userRoles, 1)[0];

                var roleProvider = GetRoleProvider(session);
                
                var result = roleProvider.IsUserInRole(fakeUser.UserName, userRoles[0].Name);
                Assert.IsTrue(result);

                result = roleProvider.IsUserInRole(fakeUser.UserName, userRoles[1].Name);
                Assert.IsTrue(result);

                result = roleProvider.IsUserInRole(fakeUser.UserName, fakeRoles[2].Name);
                Assert.IsFalse(result);
            });
        }

        [Test]
        public void Should_Call_RoleExists_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var fakeRoles = CreateFakeRoles(session, 2);
                var roleProvider = GetRoleProvider(session);

                var result = roleProvider.RoleExists(fakeRoles[0].Name);
                Assert.IsTrue(result);

                result = roleProvider.RoleExists(fakeRoles[1].Name);
                Assert.IsTrue(result);

                result = roleProvider.RoleExists(TestDataProvider.ProvideRandomString(MaxLength.Name));
                Assert.IsFalse(result);
            });
        }

        [Test]
        public void Should_Call_AddUsersToRoles_Successfully()
        {
            RunActionInTransaction(session =>
            {
                // Create fake roles and fake users with already assigned one role
                var fakeRoles = CreateFakeRoles(session, 4);
                var fakeUsers = CreateFakeUsers(session, new[] { fakeRoles[2] }, 4);

                // Add duplicated roles and usernames. Also pass role, which already is assigned to users
                var userNames = new[] { fakeUsers[0], fakeUsers[1], fakeUsers[1], fakeUsers[2] }.Select(u => u.UserName).ToArray();
                var roleNames = new[] { fakeRoles[0], fakeRoles[1], fakeRoles[1], fakeRoles[2] }.Select(u => u.Name).ToArray();

                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);

                var roleProvider = GetRoleProvider(session, repository, unitOfWork);
                roleProvider.AddUsersToRoles(userNames, roleNames);

                var user1Roles = repository
                    .AsQueryable<UserRole>(userRole => userRole.User.UserName == userNames[0])
                    .Select(userRole => userRole.Role.Name)
                    .ToArray();
                Assert.AreEqual(user1Roles.Length, 3);
                Assert.IsTrue(user1Roles.Contains(roleNames[0]));
                Assert.IsTrue(user1Roles.Contains(roleNames[1]));
                Assert.IsTrue(user1Roles.Contains(roleNames[2]));

                var user2Roles = repository
                    .AsQueryable<UserRole>(userRole => userRole.User.UserName == userNames[1])
                    .Select(userRole => userRole.Role.Name)
                    .ToArray();
                Assert.AreEqual(user2Roles.Length, 3);
                Assert.IsTrue(user2Roles.Contains(roleNames[0]));
                Assert.IsTrue(user2Roles.Contains(roleNames[1]));
                Assert.IsTrue(user2Roles.Contains(roleNames[2]));
            });
        }

        [Test]
        [ExpectedException(typeof(ProviderException))]
        public void Should_Call_AddUsersToRoles_WithException_NonExistingUser()
        {
            RunActionInTransaction(session =>
            {
                var roleNames = CreateFakeRoles(session, 1).Select(role => role.Name).ToArray();
                var userNames = new[] { TestDataProvider.ProvideRandomString(MaxLength.Name) };

                var roleProvider = GetRoleProvider(session);

                try
                {
                    roleProvider.AddUsersToRoles(userNames, roleNames);
                }
                catch (System.Exception exc)
                {
                    Assert.IsTrue(exc.Message.ToLower().Contains("user") && exc.Message.ToLower().Contains("exist"), "Should throw exception 'User does not exists.'");

                    throw;
                }
            });
        }

        [Test]
        [ExpectedException(typeof(ProviderException))]
        public void Should_Call_AddUsersToRoles_WithException_NonExistingRole()
        {
            RunActionInTransaction(session =>
            {
                var userNames = CreateFakeUsers(session, null, 1).Select(role => role.UserName).ToArray();
                var roleNames = new[] { TestDataProvider.ProvideRandomString(MaxLength.Name) };

                var roleProvider = GetRoleProvider(session);

                try
                {
                    roleProvider.AddUsersToRoles(userNames, roleNames);
                }
                catch (System.Exception exc)
                {
                    Assert.IsTrue(exc.Message.ToLower().Contains("role") && exc.Message.ToLower().Contains("exist"), "Should throw exception 'Role does not exists.'");

                    throw;
                }
            });
        }

        [Test]
        public void Should_Call_CreateRole_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);
                var roleProvider = GetRoleProvider(session, repository, unitOfWork);

                var roleName = TestDataProvider.ProvideRandomString(MaxLength.Name);
                roleProvider.CreateRole(roleName);

                var roles = repository.AsQueryable<Role>(role => role.Name == roleName).ToList();
                
                Assert.NotNull(roles);
                Assert.AreEqual(roles.Count, 1);
                Assert.AreEqual(roles[0].Name, roleName);
            });
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Should_Call_CreateRole_WithException_ExistingRole()
        {
            RunActionInTransaction(session =>
            {
                // Create role
                var role = CreateFakeRoles(session, 1)[0];

                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);
                var roleProvider = GetRoleProvider(session, repository, unitOfWork);

                // Try create existing role
                roleProvider.CreateRole(role.Name);
            });
        }

        [Test]
        public void Should_Call_DeleteRole_Unassigned_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var role = CreateFakeRoles(session, 1)[0];

                var roleProvider = GetRoleProvider(session);
                roleProvider.DeleteRole(role.Name, false);
            });
        }

        [Test]
        public void Should_Call_DeleteRole_Assigned_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var roles = CreateFakeRoles(session, 1, false);
                CreateFakeUsers(session, roles, 1);

                var roleProvider = GetRoleProvider(session);
                roleProvider.DeleteRole(roles[0].Name, false);
            });
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Should_Call_DeleteRole_Assigned_WithException_RoleIsPopulated()
        {
            RunActionInTransaction(session =>
            {
                var roles = CreateFakeRoles(session, 1);
                CreateFakeUsers(session, roles, 1);

                var roleProvider = GetRoleProvider(session);
                try
                {
                    roleProvider.DeleteRole(roles[0].Name, true);
                }
                catch (System.Exception exc)
                {
                    Assert.IsTrue(exc.Message.ToLower().Contains("populated"));
                    throw;
                }
            });
        }

        [Test]
        [ExpectedException(typeof(ValidationException))]
        public void Should_Call_DeleteRole_Assigned_WithException_RoleIsSystematic()
        {
            RunActionInTransaction(session =>
            {
                var roles = CreateFakeRoles(session, 1, true);

                var roleProvider = GetRoleProvider(session);
                try
                {
                    roleProvider.DeleteRole(roles[0].Name, true);
                }
                catch (System.Exception exc)
                {
                    Assert.IsTrue(exc.Message.ToLower().Contains("systematic"));
                    throw;
                }
            });
        }

        [Test]
        public void Should_Call_RemoveUsersFromRoles_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var fakeRoles = CreateFakeRoles(session, 3);
                var fakeUsers = CreateFakeUsers(session, fakeRoles, 3);

                var unitOfWork = new DefaultUnitOfWork(session);
                var repository = new DefaultRepository(unitOfWork);
                var roleProvider = GetRoleProvider(session, repository, unitOfWork);

                // Pass filtered user names and filtered roleNames
                var userNames = new[] { fakeUsers[0].UserName, fakeUsers[1].UserName };
                var roleNames = new[] { fakeRoles[0].Name, fakeRoles[1].Name };
                roleProvider.RemoveUsersFromRoles(userNames, roleNames);

                roleNames = fakeRoles.Select(role => role.Name).ToArray();
                userNames = fakeUsers.Select(user => user.UserName).ToArray();

                var userRoles = repository
                    .AsQueryable<UserRole>(userRole => userNames.Contains(userRole.User.UserName) || roleNames.Contains(userRole.Role.Name))
                    .Select(userRole => new
                        {
                            UserName = userRole.User.UserName,
                            RoleName = userRole.Role.Name
                        })
                    .ToList();

                Assert.NotNull(userRoles);

                // Should be left pairs:
                // 0 user -> 2 role
                // 1 user -> 2 role
                // 2 user -> 0 role
                // 2 user -> 1 role
                // 2 user -> 2 role
                Assert.AreEqual(userRoles.Count, 5);
                Assert.IsTrue(userRoles.Any(ur => ur.UserName == fakeUsers[0].UserName && ur.RoleName == fakeRoles[2].Name));
                Assert.IsTrue(userRoles.Any(ur => ur.UserName == fakeUsers[1].UserName && ur.RoleName == fakeRoles[2].Name));
                Assert.IsTrue(userRoles.Any(ur => ur.UserName == fakeUsers[2].UserName && ur.RoleName == fakeRoles[0].Name));
                Assert.IsTrue(userRoles.Any(ur => ur.UserName == fakeUsers[2].UserName && ur.RoleName == fakeRoles[1].Name));
                Assert.IsTrue(userRoles.Any(ur => ur.UserName == fakeUsers[2].UserName && ur.RoleName == fakeRoles[2].Name));
            });
        }

        [Test]
        public void Should_Call_GetUsersInRole_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var fakeRoles = CreateFakeRoles(session, 2);
                var fakeUsers = CreateFakeUsers(session, new[] { fakeRoles[0] }, 2);
                CreateFakeUsers(session, new[] { fakeRoles[1] }, 1);

                var roleProvider = GetRoleProvider(session);
                var users = roleProvider.GetUsersInRole(fakeRoles[0].Name);

                Assert.IsNotNull(users);
                Assert.AreEqual(users.Length, 2);
                Assert.IsTrue(users.Contains(fakeUsers[0].UserName));
                Assert.IsTrue(users.Contains(fakeUsers[1].UserName));
            });
        }
        
        [Test]
        public void Should_Call_FindUsersInRole_Successfully()
        {
            RunActionInTransaction(session =>
            {
                var fakeRoles = CreateFakeRoles(session, 2);
                var fakeUsers = CreateFakeUsers(session, new[] { fakeRoles[0] }, 3);
                var nonRoleUsers = CreateFakeUsers(session, new[] { fakeRoles[1] }, 1);
                const string userName = "TestUser";

                fakeUsers[0].UserName = "372FDA2E-F881-4E62-97F4-B5B34C3BBEFF-" + userName + "-" + TestDataProvider.ProvideRandomString(100);
                fakeUsers[2].UserName = "9A0433CB-32F7-4A60-85CB-A289942A2DFB-" + userName + "-" + TestDataProvider.ProvideRandomString(100);
                nonRoleUsers[0].UserName = "E228D3C0-5427-47FA-A822-B111F2286F41-" + userName + "-" + TestDataProvider.ProvideRandomString(100);
                session.SaveOrUpdate(fakeUsers[0]);
                session.SaveOrUpdate(fakeUsers[2]);
                session.SaveOrUpdate(nonRoleUsers[0]);
                session.Flush();
                session.Clear();

                var roleProvider = GetRoleProvider(session);
                var users = roleProvider.FindUsersInRole(fakeRoles[0].Name, userName);

                Assert.IsNotNull(users);
                Assert.AreEqual(users.Length, 2);
                Assert.IsTrue(users.Contains(fakeUsers[0].UserName));
                Assert.IsTrue(users.Contains(fakeUsers[2].UserName));
            });
        }

        private CmsRoleProvider GetRoleProvider(ISession session, IRepository repository = null, IUnitOfWork unitOfWork = null)
        {
            if (repository == null || unitOfWork == null)
            {
                unitOfWork = new DefaultUnitOfWork(session);
                repository = new DefaultRepository(unitOfWork);
            }
            var roleService = new DefaultRoleService(repository);

            var roleProvider = new CmsRoleProvider(repository, unitOfWork, roleService);

            return roleProvider;
        }

        private Role[] CreateFakeRoles(ISession session, int rolesCount, bool isSystematic = false)
        {
            var roles = new List<Role>();

            for (int i = 0; i < rolesCount; i++)
            {
                var role = TestDataProvider.CreateNewRole();
                role.IsSystematic = isSystematic;
                role.Name = string.Format("{0}{1}", i, role.Name.Substring(2));

                session.SaveOrUpdate(role);

                roles.Add(role);
            }

            session.Flush();
            session.Clear();

            return roles.ToArray();
        }

        private User[] CreateFakeUsers(ISession session, Role[] roles, int userCount)
        {
            var users = new List<User>();

            for (int i = 0; i < userCount; i ++)
            {
                var user = TestDataProvider.CreateNewUser();
                user.UserName = string.Format("{0}{1}", i, user.UserName.Substring(2));

                if (roles != null)
                {
                    user.UserRoles = new List<UserRole>(roles.Length);

                    foreach (var role in roles)
                    {
                        user.UserRoles.Add(new UserRole
                                               {
                                                   User = user,
                                                   Role = role
                                               });
                    }
                }

                session.SaveOrUpdate(user);

                users.Add(user);
            }

            session.Flush();
            session.Clear();

            return users.ToArray();
        }
    }
}
