using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using BetterCms.Configuration;
using BetterCms.Core.DataAccess;
using BetterCms.Core.Security;
using BetterCms.Core.Services.Caching;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.AccessControl
{
    [TestFixture]
    public class AccessControlServiceTests
    {
        [Test]
        public void Should_Return_Highest_AccessLevel_Based_On_Role()
        {
            var objectId = Guid.NewGuid();
            var accesses = new List<IUserAccess>
            {
                new UserAccess { RoleOrUser = "RoleA", AccessLevel = AccessLevel.Deny },
                new UserAccess { RoleOrUser = "Admin", AccessLevel = AccessLevel.ReadWrite },
                new UserAccess { RoleOrUser = "RoleB", AccessLevel = AccessLevel.Read }
            };

            var service = CreateAccessControlService(objectId, accesses);

            var principal = new GenericPrincipal(new GenericIdentity("John"), new[] { "Admin" });

            var accessLevel = service.GetAccessLevel(objectId, principal);

            Assert.AreEqual(AccessLevel.ReadWrite, accessLevel);
        }

        [Test]
        public void Should_Return_AccessLevel_Based_On_Identity_Name()
        {
            var objectId = Guid.NewGuid();
            var accesses = new List<IUserAccess>
            {
                new UserAccess { RoleOrUser = "Everyone", AccessLevel = AccessLevel.Read },
                new UserAccess { RoleOrUser = "RoleA", AccessLevel = AccessLevel.Deny },
                new UserAccess { RoleOrUser = "John Doe", AccessLevel = AccessLevel.ReadWrite },
                new UserAccess { RoleOrUser = "Admin", AccessLevel = AccessLevel.Read },
                new UserAccess { RoleOrUser = "RoleB", AccessLevel = AccessLevel.Read }
            };

            var service = CreateAccessControlService(objectId, accesses);

            var principal = new GenericPrincipal(new GenericIdentity("John Doe"), new string[] { });

            var accessLevel = service.GetAccessLevel(objectId, principal);

            Assert.AreEqual(AccessLevel.ReadWrite, accessLevel);
        }

        [Test]
        public void Should_Return_AccessLevel_NoPermissions()
        {
            var objectId = Guid.NewGuid();
            var accesses = new List<IUserAccess>
            {
                new UserAccess { RoleOrUser = "RoleA", AccessLevel = AccessLevel.Deny },
                new UserAccess { RoleOrUser = "RoleB", AccessLevel = AccessLevel.Read }
            };

            var service = CreateAccessControlService(objectId, accesses);

            var principal = new GenericPrincipal(new GenericIdentity("User"), new string[] { });

            var accessLevel = service.GetAccessLevel(objectId, principal);

            Assert.AreEqual(AccessLevel.NoPermissions, accessLevel);
        }

        [Test]
        public void Should_Return_AccessLevel_Read_For_Anonymous_User()
        {
            var objectId = Guid.NewGuid();
            var accesses = new List<IUserAccess>
            {
                new UserAccess { RoleOrUser = "Everyone", AccessLevel = AccessLevel.Read },
                new UserAccess { RoleOrUser = "RoleA", AccessLevel = AccessLevel.Deny },
                new UserAccess { RoleOrUser = "John Doe", AccessLevel = AccessLevel.ReadWrite },
                new UserAccess { RoleOrUser = "Admin", AccessLevel = AccessLevel.Read },
                new UserAccess { RoleOrUser = "RoleB", AccessLevel = AccessLevel.Read }
            };

            var service = CreateAccessControlService(objectId, accesses);

            var identity = new GenericIdentity("");

            // Make sure identity is not authenticated:
            Assert.IsFalse(identity.IsAuthenticated);

            var principal = new GenericPrincipal(identity, new string[] { });

            var accessLevel = service.GetAccessLevel(objectId, principal);

            Assert.AreEqual(AccessLevel.Read, accessLevel);
        }

        [Test]
        public void Should_Return_AccessLevel_Deny()
        {
            var objectId = Guid.NewGuid();
            var accesses = new List<IUserAccess>
            {
                new UserAccess { RoleOrUser = "Everyone", AccessLevel = AccessLevel.Deny },
                new UserAccess { RoleOrUser = "RoleA", AccessLevel = AccessLevel.Deny },
                new UserAccess { RoleOrUser = "John Doe", AccessLevel = AccessLevel.ReadWrite },
                new UserAccess { RoleOrUser = "Admin", AccessLevel = AccessLevel.Read },
                new UserAccess { RoleOrUser = "RoleB", AccessLevel = AccessLevel.Read }
            };

            var service = CreateAccessControlService(objectId, accesses);

            var principal = new GenericPrincipal(new GenericIdentity("Any Authenticated User"), new string[] { });

            var accessLevel = service.GetAccessLevel(objectId, principal);

            Assert.AreEqual(AccessLevel.Deny, accessLevel);
        }

        [Test]
        public void Should_Return_AccessLevel_ReadWrite_For_Any_Authenticated_User()
        {
            var objectId = Guid.NewGuid();
            var accesses = new List<IUserAccess>
            {
                new UserAccess { RoleOrUser = "RoleA", AccessLevel = AccessLevel.Deny },
                new UserAccess { RoleOrUser = "Authenticated User", AccessLevel = AccessLevel.ReadWrite },
                new UserAccess { RoleOrUser = "Admin", AccessLevel = AccessLevel.Read },
                new UserAccess { RoleOrUser = "RoleB", AccessLevel = AccessLevel.Read }
            };

            var service = CreateAccessControlService(objectId, accesses);

            var principal = new GenericPrincipal(new GenericIdentity("Any Authenticated User"), new string[] { });

            var accessLevel = service.GetAccessLevel(objectId, principal);

            Assert.AreEqual(AccessLevel.ReadWrite, accessLevel);
        }

        [Test]
        public void Should_Return_ReadWrite_If_There_Are_No_UserAccess()
        {
            var objectId = Guid.NewGuid();
            var accesses = new List<IUserAccess>();

            var service = CreateAccessControlService(objectId, accesses);

            var principal = new GenericPrincipal(new GenericIdentity("Any Authenticated User"), new string[] { });

            var accessLevel = service.GetAccessLevel(objectId, principal);

            Assert.AreEqual(AccessLevel.ReadWrite, accessLevel);
        }

        [Test]
        public void Should_Return_Default_List()
        {
            var objectId = Guid.NewGuid();
            var accesses = new List<IUserAccess>();

            var service = CreateAccessControlService(objectId, accesses);

            var accessLevel = service.GetDefaultAccessList();

            Assert.AreEqual(0, accessLevel.Count);
        }

        private static AccessControlService CreateAccessControlService(Guid objectId, IEnumerable<IUserAccess> accesses)
        {
            var repository = GetRepositoryMock(objectId, accesses);
            var cacheService = GetCacheServiceMock();
            var cmsConfiguration = GetCmsConfigurationMock();

            var service = new AccessControlService(repository.Object, cacheService.Object, cmsConfiguration.Object);

            return service;
        }

        private static Mock<ICmsConfiguration> GetCmsConfigurationMock()
        {
            var cmsConfiguration = new Mock<ICmsConfiguration>();

            cmsConfiguration.Setup(x => x.DefaultAccessControlList).Returns(() => new AccessControlCollection());
            return cmsConfiguration;
        }

        private static Mock<ICacheService> GetCacheServiceMock()
        {
            var mock = new Mock<ICacheService>();

            List<UserAccess> accessList = null;

            mock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<TimeSpan>(), It.IsAny<Func<List<UserAccess>>>()))
                        .Callback((string cacheKey, TimeSpan timeSpan, Func<List<UserAccess>> callback) => accessList = callback())
                        .Returns(() => accessList);

            return mock;
        }

        private static Mock<IRepository> GetRepositoryMock(Guid objectId, IEnumerable<IUserAccess> accesses)
        {
            var repository = new Mock<IRepository>();

            var accessList = accesses.Select(x => new UserAccess
                                                      {
                                                          ObjectId = objectId,
                                                          AccessLevel = x.AccessLevel,
                                                          RoleOrUser = x.RoleOrUser
                                                      }).ToList();

            repository.Setup(x => x.AsQueryable<UserAccess>()).Returns(accessList.AsQueryable);

            return repository;
        }
    }
}
