using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using Autofac;

using BetterCms.Configuration;

using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Services;

using BetterModules.Core.Web.Services.Caching;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Root.AccessControl
{
    [TestFixture]
    public class AccessControlServiceTests : IntegrationTestBase
    {
        private IAccessControlService CreateAccessControlService()
        {
            return new DefaultAccessControlService(Container.Resolve<ISecurityService>() , Container.Resolve<ICacheService>(), Container.Resolve<ICmsConfiguration>(), null);
        }

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

                    var accessControlService = CreateAccessControlService();

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

                var accessControlService = CreateAccessControlService();

                var level = accessControlService.GetAccessLevel(file, new GenericPrincipal(new GenericIdentity(accessRule.Identity), null));

                Assert.AreEqual(accessRule.AccessLevel, level);
            });
        }

        [Test]
        public void Should_Return_Highest_AccessLevel_Based_On_Role()
        {
            RunActionInTransaction(session =>
                {
                    var accessRules = new List<AccessRule>(new[] {
                                                new AccessRule { Identity = "RoleA", AccessLevel = AccessLevel.Deny, IsForRole = true},
                                                new AccessRule { Identity = "Admin", AccessLevel = AccessLevel.ReadWrite, IsForRole = true},
                                                new AccessRule { Identity = "RoleB", AccessLevel = AccessLevel.Read, IsForRole = true}
                                             });

                    var page = TestDataProvider.CreateNewPage();
                    accessRules.ForEach(page.AddRule);

                    session.SaveOrUpdate(page);
                    session.Flush();
                    session.Clear();

                    var service = CreateAccessControlService();

                    var principal = new GenericPrincipal(new GenericIdentity("John"), new[] { "Admin" });
                    var accessLevel = service.GetAccessLevel(page, principal);

                    Assert.AreEqual(AccessLevel.ReadWrite, accessLevel);
                });
        }

        [Test]
        public void Should_Return_AccessLevel_Based_On_Identity_Name()
        {
            RunActionInTransaction(session =>
            {
                var accessRules = new List<AccessRule>(new[] {
                                                                new AccessRule { Identity = "Everyone", AccessLevel = AccessLevel.Read, IsForRole = true },
                                                                new AccessRule { Identity = "RoleA", AccessLevel = AccessLevel.Deny, IsForRole = true },
                                                                new AccessRule { Identity = "John Doe", AccessLevel = AccessLevel.ReadWrite, IsForRole = false },
                                                                new AccessRule { Identity = "Admin", AccessLevel = AccessLevel.Read, IsForRole = true },
                                                                new AccessRule { Identity = "RoleB", AccessLevel = AccessLevel.Read, IsForRole = true }
                                                             });

                var page = TestDataProvider.CreateNewPage();
                accessRules.ForEach(page.AddRule);

                session.SaveOrUpdate(page);
                session.Flush();
                session.Clear();

                var service = CreateAccessControlService();

                var principal = new GenericPrincipal(new GenericIdentity("John Doe"), new string[] { });
                var accessLevel = service.GetAccessLevel(page, principal);

                Assert.AreEqual(AccessLevel.ReadWrite, accessLevel);
            });
        }

        [Test]
        public void Should_Return_AccessLevel_NoPermissions()
        {
            RunActionInTransaction(session =>
            {
                var accessRules = new List<AccessRule>(new[] {
                                                                new AccessRule { Identity = "RoleA", AccessLevel = AccessLevel.Deny, IsForRole = true },
                                                                new AccessRule { Identity = "RoleB", AccessLevel = AccessLevel.Read, IsForRole = true }
                                                             });

                var mediaFile = TestDataProvider.CreateNewMediaFile();
                accessRules.ForEach(mediaFile.AddRule);

                session.SaveOrUpdate(mediaFile);
                session.Flush();
                session.Clear();

                var service = CreateAccessControlService();

                var principal = new GenericPrincipal(new GenericIdentity("John Doe"), new string[] { });
                var accessLevel = service.GetAccessLevel(mediaFile, principal);

                Assert.AreEqual(AccessLevel.Deny, accessLevel);
            });                    
        }

        [Test]
        public void Should_Return_AccessLevel_Read_For_Anonymous_User()
        {
            RunActionInTransaction(session =>
            {
                var accessRules = new List<AccessRule>(new[] {
                                                                new AccessRule { Identity = "Everyone", AccessLevel = AccessLevel.Read, IsForRole = true },
                                                                new AccessRule { Identity = "RoleA", AccessLevel = AccessLevel.Deny, IsForRole = true },
                                                                new AccessRule { Identity = "John Doe", AccessLevel = AccessLevel.ReadWrite, IsForRole = false },
                                                                new AccessRule { Identity = "Admin", AccessLevel = AccessLevel.Read, IsForRole = true },
                                                                new AccessRule { Identity = "RoleB", AccessLevel = AccessLevel.Read, IsForRole = true }
                                                             });

                var mediaFile = TestDataProvider.CreateNewMediaFile();
                accessRules.ForEach(mediaFile.AddRule);

                session.SaveOrUpdate(mediaFile);
                session.Flush();
                session.Clear();

                var service = CreateAccessControlService();

                var identity = new GenericIdentity("");

                // Make sure identity is not authenticated:
                Assert.IsFalse(identity.IsAuthenticated);

                var principal = new GenericPrincipal(identity, new string[] { });

                var accessLevel = service.GetAccessLevel(mediaFile, principal);

                Assert.AreEqual(AccessLevel.Read, accessLevel);
            });    
        }

        [Test]
        public void Should_Return_AccessLevel_Deny()
        {
            RunActionInTransaction(session =>
            {
                var accessRules = new List<AccessRule>(new[] {
                                                                new AccessRule  { Identity = "Everyone", AccessLevel = AccessLevel.Deny, IsForRole = true },
                                                                new AccessRule { Identity = "RoleA", AccessLevel = AccessLevel.Deny, IsForRole = true },
                                                                new AccessRule { Identity = "John Doe", AccessLevel = AccessLevel.ReadWrite, IsForRole = false },
                                                                new AccessRule { Identity = "Admin", AccessLevel = AccessLevel.Read, IsForRole = true },
                                                                new AccessRule { Identity = "RoleB", AccessLevel = AccessLevel.Read, IsForRole = true }
                                                             });

                var mediaFile = TestDataProvider.CreateNewMediaFile();
                accessRules.ForEach(mediaFile.AddRule);

                session.SaveOrUpdate(mediaFile);
                session.Flush();
                session.Clear();

                var service = CreateAccessControlService();

                var principal = new GenericPrincipal(new GenericIdentity("Any Authenticated User"), new string[] { });

                var accessLevel = service.GetAccessLevel(mediaFile, principal);

                Assert.AreEqual(AccessLevel.Deny, accessLevel);
            });           
        }

        [Test]
        public void Should_Return_AccessLevel_ReadWrite_For_Any_Authenticated_User()
        {
            RunActionInTransaction(session =>
            {
                var accessRules = new List<AccessRule>(new[] {
                                                                new AccessRule { Identity = "RoleA", AccessLevel = AccessLevel.Deny, IsForRole = true },
                                                                new AccessRule { Identity = "Authenticated Users", AccessLevel = AccessLevel.ReadWrite, IsForRole = true },
                                                                new AccessRule { Identity = "Admin", AccessLevel = AccessLevel.Read, IsForRole = true },
                                                                new AccessRule { Identity = "RoleB", AccessLevel = AccessLevel.Read, IsForRole = true }
                                                             });

                var mediaFile = TestDataProvider.CreateNewMediaFile();
                accessRules.ForEach(mediaFile.AddRule);

                session.SaveOrUpdate(mediaFile);
                session.Flush();
                session.Clear();

                var service = CreateAccessControlService();

                var identity = new GenericIdentity("");

                // Make sure identity is not authenticated:
                Assert.IsFalse(identity.IsAuthenticated);

                var principal = new GenericPrincipal(new GenericIdentity("Any Authenticated User"), new string[] { });

                var accessLevel = service.GetAccessLevel(mediaFile, principal);

                Assert.AreEqual(AccessLevel.ReadWrite, accessLevel);
            });             
        }

        [Test]
        public void Should_Return_ReadWrite_If_There_Are_No_UserAccess()
        {
            RunActionInTransaction(session =>
            {
                var mediaFile = TestDataProvider.CreateNewMediaFile();

                session.SaveOrUpdate(mediaFile);
                session.Flush();
                session.Clear();

                var service = CreateAccessControlService();

                var identity = new GenericIdentity("");

                // Make sure identity is not authenticated:
                Assert.IsFalse(identity.IsAuthenticated);

                var principal = new GenericPrincipal(new GenericIdentity("Any Authenticated User"), new string[] { });

                var accessLevel = service.GetAccessLevel(mediaFile, principal);

                Assert.AreEqual(AccessLevel.ReadWrite, accessLevel);
            });                
        }

        [Test]
        public void Should_Return_Empty_Default_List()
        {
            RunActionInTransaction(
                session =>
                    {
                        var service = CreateAccessControlService();
                        var accessLevel = service.GetDefaultAccessList();

                        Assert.AreEqual(0, accessLevel.Count);
                    });            
        }

        [Test]
        public void Should_Return_Default_List_Without_Principal()
        {
            RunActionInTransaction(session =>
                {
                    var collection = new AccessControlCollection();
                    collection.Add(new AccessControlElement { AccessLevel = AccessLevel.Deny.ToString(), Identity = SpecialIdentities.Everyone });
                    collection.Add(new AccessControlElement { AccessLevel = AccessLevel.Read.ToString(), Identity = SpecialIdentities.AuthenticatedUsers });

                    var cmsConfig = GetCmsConfigurationMock(collection);

                    var service = new DefaultAccessControlService(Container.Resolve<ISecurityService>(), Container.Resolve<ICacheService>(), cmsConfig.Object, null);

                    var accessLevels = service.GetDefaultAccessList();

                    Assert.AreEqual(2, accessLevels.Count);
                    Assert.AreEqual(accessLevels.First(a => a.Identity == collection[0].Identity).AccessLevel.ToString(), collection[0].AccessLevel);
                    Assert.AreEqual(accessLevels.First(a => a.Identity == collection[1].Identity).AccessLevel.ToString(), collection[1].AccessLevel);
                });
        }

        /// <summary>
        /// Should add principal with read/write role (as owner)
        /// </summary>
        [Test]
        public void Should_Return_Default_List_Wit_Principal_Added()
        {
            RunActionInTransaction(session =>
            {
                var collection = new AccessControlCollection();
                collection.Add(new AccessControlElement { AccessLevel = AccessLevel.Deny.ToString(), Identity = SpecialIdentities.Everyone });
                collection.Add(new AccessControlElement { AccessLevel = AccessLevel.Read.ToString(), Identity = SpecialIdentities.AuthenticatedUsers });

                var cmsConfig = GetCmsConfigurationMock(collection);

                var service = new DefaultAccessControlService(Container.Resolve<ISecurityService>(), Container.Resolve<ICacheService>(), cmsConfig.Object, null);

                var principal = new GenericPrincipal(new GenericIdentity("John Doe"), new string[] { });
                var accessLevels = service.GetDefaultAccessList(principal);

                Assert.AreEqual(3, accessLevels.Count);
                Assert.AreEqual(accessLevels.First(a => a.Identity == collection[0].Identity).AccessLevel.ToString(), collection[0].AccessLevel);
                Assert.AreEqual(accessLevels.First(a => a.Identity == collection[1].Identity).AccessLevel.ToString(), collection[1].AccessLevel);
                Assert.AreEqual(accessLevels.First(a => a.Identity == principal.Identity.Name).AccessLevel, AccessLevel.ReadWrite);
            });
        }

        /// <summary>
        /// Should not add principal with read/write role, because authenticated users have read/write role
        /// </summary>
        [Test]
        public void Should_Return_Default_List_With_Principal_Added_And_Ignored()
        {
            RunActionInTransaction(session =>
            {
                var collection = new AccessControlCollection();
                collection.Add(new AccessControlElement { AccessLevel = AccessLevel.Deny.ToString(), Identity = SpecialIdentities.Everyone });
                collection.Add(new AccessControlElement { AccessLevel = AccessLevel.ReadWrite.ToString(), Identity = SpecialIdentities.AuthenticatedUsers });

                var cmsConfig = GetCmsConfigurationMock(collection);

                var service = new DefaultAccessControlService(Container.Resolve<ISecurityService>(), Container.Resolve<ICacheService>(), cmsConfig.Object, null);

                var principal = new GenericPrincipal(new GenericIdentity("John Doe"), new string[] { });
                var accessLevels = service.GetDefaultAccessList(principal);

                Assert.AreEqual(2, accessLevels.Count);
                Assert.AreEqual(accessLevels.First(a => a.Identity == collection[0].Identity).AccessLevel.ToString(), collection[0].AccessLevel);
                Assert.AreEqual(accessLevels.First(a => a.Identity == collection[1].Identity).AccessLevel.ToString(), collection[1].AccessLevel);
            });            
        }

        private Mock<ICmsConfiguration> GetCmsConfigurationMock(AccessControlCollection accessControlCollection)
        {
            var cmsSecuritySection = new Mock<ICmsSecurityConfiguration>();
            cmsSecuritySection.Setup(x => x.DefaultAccessRules).Returns(() => accessControlCollection ?? new AccessControlCollection());

            var cmsConfiguration = new Mock<ICmsConfiguration>();
            cmsConfiguration.Setup(x => x.Security).Returns(cmsSecuritySection.Object);

            return cmsConfiguration;
        }     
    }
}
