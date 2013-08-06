using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using BetterCms.Core.DataAccess;
using BetterCms.Core.Security;
using BetterCms.Module.AccessControl;
using BetterCms.Module.AccessControl.Models;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.AccessControl
{
    [TestFixture]
    [Ignore("Will be fixed in the near future")]
    public class AccessControlServiceTests
    {
        [Test]
        public void Should_Return_Highest_AccessLevel_Based_On_Role()
        {
            var objectId = Guid.NewGuid();

            var service = CreateAccessControlService(objectId);

            var principal = new GenericPrincipal(new GenericIdentity("John"), new[] { "Admin" });

            var accessLevel = service.GetAccessLevel(objectId, principal);

            Assert.AreEqual(AccessLevel.ReadWrite, accessLevel);
        }

        [Test]
        public void Should_Return_Highest_AccessLevel_Based_On_Identity_Name()
        {
            var objectId = Guid.NewGuid();

            var service = CreateAccessControlService(objectId);

            var principal = new GenericPrincipal(new GenericIdentity("Admin"), new string[] { });

            var accessLevel = service.GetAccessLevel(objectId, principal);

            Assert.AreEqual(AccessLevel.ReadWrite, accessLevel);
        }

        [Test]
        public void Should_Return_AccessLevel_NoPermissions()
        {
            var objectId = Guid.NewGuid();

            var service = CreateAccessControlService(objectId);

            var principal = new GenericPrincipal(new GenericIdentity("User"), new string[] { });

            var accessLevel = service.GetAccessLevel(objectId, principal);

            Assert.AreEqual(AccessLevel.NoPermissions, accessLevel);
        }

        [Test]
        public void Should_Return_AccessLevel_Read_For_Anonymous_User()
        {
            var objectId = Guid.NewGuid();

            var service = CreateAccessControlService(objectId);

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

            var service = CreateAccessControlService(objectId);

            var principal = new GenericPrincipal(new GenericIdentity("Deny"), new string[] { });

            var accessLevel = service.GetAccessLevel(objectId, principal);

            Assert.AreEqual(AccessLevel.Deny, accessLevel);
        }

        private static AccessControlService CreateAccessControlService(Guid objectId)
        {
            var repository = GetRepositoryMock(objectId);

            var service = new AccessControlService(repository.Object, null /* TODO: add service or mock instead of null */);

            return service;
        }

        private static Mock<IRepository> GetRepositoryMock(Guid objectId)
        {
            var repository = new Mock<IRepository>();

            var accessList = new List<UserAccess>();

            accessList.Add(new UserAccess { ObjectId = objectId, AccessLevel = AccessLevel.Read, RoleOrUser = "Anonymous" });
            accessList.Add(new UserAccess { ObjectId = objectId, AccessLevel = AccessLevel.Deny, RoleOrUser = "Deny" });
            accessList.Add(new UserAccess { ObjectId = objectId, AccessLevel = AccessLevel.Deny, RoleOrUser = "Admin" });
            accessList.Add(new UserAccess { ObjectId = objectId, AccessLevel = AccessLevel.ReadWrite, RoleOrUser = "Admin" });
            accessList.Add(new UserAccess { ObjectId = objectId, AccessLevel = AccessLevel.Read, RoleOrUser = "Admin" });

            repository.Setup(x => x.AsQueryable<UserAccess>()).Returns(accessList.AsQueryable);

            return repository;
        }
    }
}
