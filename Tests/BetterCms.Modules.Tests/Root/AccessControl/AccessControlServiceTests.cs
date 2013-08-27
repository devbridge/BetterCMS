using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using Autofac;

using BetterCms.Configuration;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Security;
using BetterCms.Core.Services.Caching;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.AccessControl
{
    [TestFixture]
    public class AccessControlServiceTests : DatabaseTestBase
    {
        [Test]
        public void Should_Get_Page_Access_Control()
        {
            RunActionInTransaction(session =>
                {
                    var accessRule = TestDataProvider.CreateNewAccessRule();
                    var page = TestDataProvider.CreateNewPage();
                    page.AddRule(accessRule);

                    session.SaveOrUpdate(page);
                    session.Flush();

                    var accessControlService = new DefaultAccessControlService(
                        new DefaultRepository(new DefaultUnitOfWork(session)), Container.Resolve<ICacheService>(), Container.Resolve<ICmsConfiguration>());

                    var level = accessControlService.GetAccessLevel(page, new GenericPrincipal(new GenericIdentity(accessRule.Identity), null));

                    Assert.AreEqual(accessRule.AccessLevel, level);
                });
        }

        [Test]
        public void Should_Get_File_Access_Control()
        {
            RunActionInTransaction(session =>
            {
                var accessRule = TestDataProvider.CreateNewAccessRule();
                var file = TestDataProvider.CreateNewMediaFile();
                file.AddRule(accessRule);

                session.SaveOrUpdate(file);
                session.Flush();

                var accessControlService = new DefaultAccessControlService(
                        new DefaultRepository(new DefaultUnitOfWork(session)), Container.Resolve<ICacheService>(), Container.Resolve<ICmsConfiguration>());

                var level = accessControlService.GetAccessLevel(file, new GenericPrincipal(new GenericIdentity(accessRule.Identity), null));

                Assert.AreEqual(accessRule.AccessLevel, level);
            });
        }

        //[Test]
        //public void Should_Return_Highest_AccessLevel_Based_On_Role()
        //{
        //    var objectId = Guid.NewGuid();
        //    var accesses = new List<IAccessRule>
        //    {
        //        new AccessRule { Identity = "RoleA", AccessLevel = AccessLevel.Deny },
        //        new AccessRule { Identity = "Admin", AccessLevel = AccessLevel.ReadWrite },
        //        new AccessRule { Identity = "RoleB", AccessLevel = AccessLevel.Read }
        //    };

        //    var service = CreateAccessControlService(objectId, accesses);

        //    var principal = new GenericPrincipal(new GenericIdentity("John"), new[] { "Admin" });
            
        //    var accessLevel = service.GetAccessLevel( objectId, principal);

        //    Assert.AreEqual(AccessLevel.ReadWrite, accessLevel);
        //}

        //[Test]
        //public void Should_Return_AccessLevel_Based_On_Identity_Name()
        //{
        //    var objectId = Guid.NewGuid();
        //    var accesses = new List<IAccess>
        //    {
        //        new PageAccessRule { RoleOrUser = "Everyone", AccessLevel = AccessLevel.Read },
        //        new PageAccessRule { RoleOrUser = "RoleA", AccessLevel = AccessLevel.Deny },
        //        new PageAccessRule { RoleOrUser = "John Doe", AccessLevel = AccessLevel.ReadWrite },
        //        new PageAccessRule { RoleOrUser = "Admin", AccessLevel = AccessLevel.Read },
        //        new PageAccessRule { RoleOrUser = "RoleB", AccessLevel = AccessLevel.Read }
        //    };

        //    var service = CreateAccessControlService(objectId, accesses);

        //    var principal = new GenericPrincipal(new GenericIdentity("John Doe"), new string[] { });

        //    var accessLevel = service.GetAccessLevel<PageAccessRule>(objectId, principal);

        //    Assert.AreEqual(AccessLevel.ReadWrite, accessLevel);
        //}

        //[Test]
        //public void Should_Return_AccessLevel_NoPermissions()
        //{
        //    var objectId = Guid.NewGuid();
        //    var accesses = new List<IAccess>
        //    {
        //        new PageAccessRule { RoleOrUser = "RoleA", AccessLevel = AccessLevel.Deny },
        //        new PageAccessRule { RoleOrUser = "RoleB", AccessLevel = AccessLevel.Read }
        //    };

        //    var service = CreateAccessControlService(objectId, accesses);

        //    var principal = new GenericPrincipal(new GenericIdentity("User"), new string[] { });

        //    var accessLevel = service.GetAccessLevel<PageAccessRule>(objectId, principal);

        //    Assert.AreEqual(AccessLevel.NoPermissions, accessLevel);
        //}

        //[Test]
        //public void Should_Return_AccessLevel_Read_For_Anonymous_User()
        //{
        //    var objectId = Guid.NewGuid();
        //    var accesses = new List<IAccess>
        //    {
        //        new PageAccessRule { RoleOrUser = "Everyone", AccessLevel = AccessLevel.Read },
        //        new PageAccessRule { RoleOrUser = "RoleA", AccessLevel = AccessLevel.Deny },
        //        new PageAccessRule { RoleOrUser = "John Doe", AccessLevel = AccessLevel.ReadWrite },
        //        new PageAccessRule { RoleOrUser = "Admin", AccessLevel = AccessLevel.Read },
        //        new PageAccessRule { RoleOrUser = "RoleB", AccessLevel = AccessLevel.Read }
        //    };

        //    var service = CreateAccessControlService(objectId, accesses);

        //    var identity = new GenericIdentity("");

        //    // Make sure identity is not authenticated:
        //    Assert.IsFalse(identity.IsAuthenticated);

        //    var principal = new GenericPrincipal(identity, new string[] { });

        //    var accessLevel = service.GetAccessLevel<PageAccessRule>(objectId, principal);

        //    Assert.AreEqual(AccessLevel.Read, accessLevel);
        //}

        //[Test]
        //public void Should_Return_AccessLevel_Deny()
        //{
        //    var objectId = Guid.NewGuid();
        //    var accesses = new List<IAccess>
        //    {
        //        new PageAccessRule { RoleOrUser = "Everyone", AccessLevel = AccessLevel.Deny },
        //        new PageAccessRule { RoleOrUser = "RoleA", AccessLevel = AccessLevel.Deny },
        //        new PageAccessRule { RoleOrUser = "John Doe", AccessLevel = AccessLevel.ReadWrite },
        //        new PageAccessRule { RoleOrUser = "Admin", AccessLevel = AccessLevel.Read },
        //        new PageAccessRule { RoleOrUser = "RoleB", AccessLevel = AccessLevel.Read }
        //    };

        //    var service = CreateAccessControlService(objectId, accesses);

        //    var principal = new GenericPrincipal(new GenericIdentity("Any Authenticated User"), new string[] { });

        //    var accessLevel = service.GetAccessLevel<PageAccessRule>(objectId, principal);

        //    Assert.AreEqual(AccessLevel.Deny, accessLevel);
        //}

        //[Test]
        //public void Should_Return_AccessLevel_ReadWrite_For_Any_Authenticated_User()
        //{
        //    var objectId = Guid.NewGuid();
        //    var accesses = new List<IAccess>
        //    {
        //        new PageAccessRule { RoleOrUser = "RoleA", AccessLevel = AccessLevel.Deny },
        //        new PageAccessRule { RoleOrUser = "Authenticated Users", AccessLevel = AccessLevel.ReadWrite },
        //        new PageAccessRule { RoleOrUser = "Admin", AccessLevel = AccessLevel.Read },
        //        new PageAccessRule { RoleOrUser = "RoleB", AccessLevel = AccessLevel.Read }
        //    };

        //    var service = CreateAccessControlService(objectId, accesses);

        //    var principal = new GenericPrincipal(new GenericIdentity("Any Authenticated User"), new string[] { });

        //    var accessLevel = service.GetAccessLevel<PageAccessRule>(objectId, principal);

        //    Assert.AreEqual(AccessLevel.ReadWrite, accessLevel);
        //}

        //[Test]
        //public void Should_Return_ReadWrite_If_There_Are_No_UserAccess()
        //{
        //    var objectId = Guid.NewGuid();
        //    var accesses = new List<IAccess>();

        //    var service = CreateAccessControlService(objectId, accesses);

        //    var principal = new GenericPrincipal(new GenericIdentity("Any Authenticated User"), new string[] { });

        //    var accessLevel = service.GetAccessLevel<PageAccessRule>(objectId, principal);

        //    Assert.AreEqual(AccessLevel.ReadWrite, accessLevel);
        //}

        //[Test]
        //public void Should_Return_Empty_Default_List()
        //{
        //    var objectId = Guid.NewGuid();
        //    var accesses = new List<IAccess>();

        //    var service = CreateAccessControlService(objectId, accesses);

        //    var accessLevel = service.GetDefaultAccessList();

        //    Assert.AreEqual(0, accessLevel.Count);
        //}

        //[Test]
        //public void Should_Return_Default_List_Without_Principal()
        //{
        //    var objectId = Guid.NewGuid();
        //    var accesses = new List<IAccess>();

        //    var collection = new AccessControlCollection();
        //    collection.Add(new AccessControlElement { AccessLevel = AccessLevel.Deny.ToString(), RoleOrUser = SpecialIdentities.Everyone });
        //    collection.Add(new AccessControlElement { AccessLevel = AccessLevel.Read.ToString(), RoleOrUser = SpecialIdentities.AuthenticatedUsers });

        //    var service = CreateAccessControlService(objectId, accesses, collection);

        //    var accessLevels = service.GetDefaultAccessList();

        //    Assert.AreEqual(2, accessLevels.Count);
        //    Assert.AreEqual(accessLevels.First(a => a.RoleOrUser == collection[0].RoleOrUser).AccessLevel.ToString(), collection[0].AccessLevel);
        //    Assert.AreEqual(accessLevels.First(a => a.RoleOrUser == collection[1].RoleOrUser).AccessLevel.ToString(), collection[1].AccessLevel);
        //}

        ///// <summary>
        ///// Should add principal with read/write role (as owner)
        ///// </summary>
        //[Test]
        //public void Should_Return_Default_List_Wit_Principal_Added()
        //{
        //    var objectId = Guid.NewGuid();
        //    var accesses = new List<IAccess>();

        //    var collection = new AccessControlCollection();
        //    collection.Add(new AccessControlElement { AccessLevel = AccessLevel.Deny.ToString(), RoleOrUser = SpecialIdentities.Everyone });
        //    collection.Add(new AccessControlElement { AccessLevel = AccessLevel.Read.ToString(), RoleOrUser = SpecialIdentities.AuthenticatedUsers });

        //    var service = CreateAccessControlService(objectId, accesses, collection);

        //    var principal = new GenericPrincipal(new GenericIdentity("John Doe"), new string[] { });
        //    var accessLevels = service.GetDefaultAccessList(principal);

        //    Assert.AreEqual(3, accessLevels.Count);
        //    Assert.AreEqual(accessLevels.First(a => a.RoleOrUser == collection[0].RoleOrUser).AccessLevel.ToString(), collection[0].AccessLevel);
        //    Assert.AreEqual(accessLevels.First(a => a.RoleOrUser == collection[1].RoleOrUser).AccessLevel.ToString(), collection[1].AccessLevel);
        //    Assert.AreEqual(accessLevels.First(a => a.RoleOrUser == principal.Identity.Name).AccessLevel, AccessLevel.ReadWrite);
        //}

        ///// <summary>
        ///// Should not add principal with read/write role, because authenticated users have read/write role
        ///// </summary>
        //[Test]
        //public void Should_Return_Default_List_Wit_Principal_Added_And_Ignored()
        //{
        //    var objectId = Guid.NewGuid();
        //    var accesses = new List<IAccess>();

        //    var collection = new AccessControlCollection();
        //    collection.Add(new AccessControlElement { AccessLevel = AccessLevel.Deny.ToString(), RoleOrUser = SpecialIdentities.Everyone });
        //    collection.Add(new AccessControlElement { AccessLevel = AccessLevel.ReadWrite.ToString(), RoleOrUser = SpecialIdentities.AuthenticatedUsers });

        //    var service = CreateAccessControlService(objectId, accesses, collection);

        //    var principal = new GenericPrincipal(new GenericIdentity("John Doe"), new string[] { });
        //    var accessLevels = service.GetDefaultAccessList(principal);

        //    Assert.AreEqual(2, accessLevels.Count);
        //    Assert.AreEqual(accessLevels.First(a => a.RoleOrUser == collection[0].RoleOrUser).AccessLevel.ToString(), collection[0].AccessLevel);
        //    Assert.AreEqual(accessLevels.First(a => a.RoleOrUser == collection[1].RoleOrUser).AccessLevel.ToString(), collection[1].AccessLevel);
        //}

        private DefaultAccessControlService CreateAccessControlService(Guid objectId, IEnumerable<IAccessRule> accesses, AccessControlCollection defaults = null)
        {
            var repository = GetRepositoryMock(objectId, accesses);
            var cacheService = GetCacheServiceMock();
            var cmsConfiguration = GetCmsConfigurationMock(defaults);

            var service = new DefaultAccessControlService(repository.Object, cacheService.Object, cmsConfiguration.Object);

            return service;
        }

        private Mock<ICmsConfiguration> GetCmsConfigurationMock(AccessControlCollection defaults)
        {
            var cmsConfiguration = new Mock<ICmsConfiguration>();

            cmsConfiguration.Setup(x => x.DefaultAccessControlList).Returns(() => defaults ?? new AccessControlCollection());
            return cmsConfiguration;
        }

        private Mock<ICacheService> GetCacheServiceMock()
        {
            var mock = new Mock<ICacheService>();

            List<IAccessRule> accessList = null;

            mock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<TimeSpan>(), It.IsAny<Func<List<IAccessRule>>>()))
                        .Callback((string cacheKey, TimeSpan timeSpan, Func<List<IAccessRule>> callback) => accessList = callback())
                        .Returns(() => accessList);

            return mock;
        }

        private Mock<IRepository> GetRepositoryMock(Guid objectId, IEnumerable<IAccessRule> accesses)
        {
            var repository = new Mock<IRepository>();

            var accessList = accesses.Select(x => new AccessRule
                                                      {
                                                          AccessLevel = x.AccessLevel,
                                                          Identity = x.Identity
                                                      }).ToList();

            repository.Setup(x => x.AsQueryable<Page>()).Returns(() =>
                {
                    var page = TestDataProvider.CreateNewPage();
                    page.Id = objectId;
                    page.AccessRules = accessList;

                    return new[] { page }.AsQueryable();
                });

            repository.Setup(x => x.AsQueryable<MediaFile>()).Returns(() =>
            {
                var file = TestDataProvider.CreateNewMediaFile();
                file.Id = objectId;
                file.AccessRules = accessList;

                return new[] { file }.AsQueryable();
            });

            return repository;
        }
    }
}
