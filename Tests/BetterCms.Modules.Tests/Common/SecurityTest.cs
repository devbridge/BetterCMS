using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

using Autofac;

using BetterCms.Core.Services;

using BetterCms.Module.Blog;
using BetterCms.Module.MediaManager;
using BetterCms.Module.Pages;
using BetterCms.Module.Root;
using BetterCms.Module.Installation;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Users;

using BetterModules.Core.Web.Web;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Common
{
    [TestFixture]
    public class SecurityTest : TestBase
    {
        [Test]
        public void SecurityService_Should_Get_An_Anonymous_If_There_Is_No_Web_Context()
        {
            var securityService = Container.Resolve<ISecurityService>();            
            Assert.AreEqual("Anonymous", securityService.CurrentPrincipalName);
        }

        [Test]
        public void SecurityService_Should_Get_An_UseName_If_There_Exists_Web_Context()
        {
            const string useName = "John Smith";

            var httpContextMock = new Mock<IHttpContextAccessor>();
            var httpContextWrapper = new HttpContextWrapper(new HttpContext(new HttpRequest("test", "http://www.bettercms.com/tests/", null), new HttpResponse(TextWriter.Null)));
            httpContextWrapper.User = new GenericPrincipal(new GenericIdentity(useName), null);
            httpContextMock.Setup(f => f.GetCurrent()).Returns(() => httpContextWrapper);


            var securityService = new DefaultSecurityService(Container.Resolve<ICmsConfiguration>(), httpContextMock.Object);

            Assert.AreEqual(useName, securityService.CurrentPrincipalName);
        }

        /// <summary>
        /// Test checks if all controllers contains [BcmsAuthorize] attribute.
        /// </summary>
        [Test]
        public void All_Controllers_Needs_BcmsAuthorize_Attribute()
        {
            var controllersToSkip = new Dictionary<string, string[]>
                {
                    // Controller Name                          Actions
                    { "CmsController",                  new[] { "Index" } },
                    { "EmbeddedResourcesController",    new[] { "Index" } },
                    { "RenderingController",            new[] { "*" } },
                    { "FilesController",                new[] { "Download" } },
                    { "AuthenticationController",       new[] { "Login", "Logout", "IsAuthorized" } },
                    { "RegistrationController",         new[] { "*" } },
                };

            var testItems = new List<Type>
                {
                    typeof(RootModuleDescriptor),
                    typeof(PagesModuleDescriptor),
                    typeof(BlogModuleDescriptor),
                    typeof(MediaManagerModuleDescriptor),
                    typeof(InstallationModuleDescriptor),
                    typeof(UsersModuleDescriptor),
                };

            foreach (var testItem in testItems)
            {
                var controllers = testItem.Assembly.GetTypes()
                    .Where(t => !t.IsAbstract && typeof(Controller).IsAssignableFrom(t) && t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                foreach (var controller in controllers)
                {
                    if (!controllersToSkip.ContainsKey(controller.Name))
                    {
                        Assert.IsNotNull(
                            Attribute.GetCustomAttribute(controller, typeof(AuthorizeAttribute)),
                            string.Format("{0} class without BcmsAuthorize attribute.", controller.Name));
                    }
                    else
                    {
                        var actionsToSkip = controllersToSkip[controller.Name];

                        if (actionsToSkip.Length > 0 && actionsToSkip[0] == "*")
                        {
                            // Skip all controller actions.
                        }
                        else
                        {
                            var actions =
                                controller.GetMethods()
                                          .Where(m => m.IsPublic && m.ReturnType == typeof(ActionResult))
                                          .Where(m => Attribute.GetCustomAttribute(m, typeof(NonActionAttribute)) == null);

                            foreach (var action in actions.Where(action => !actionsToSkip.Any(a => a == action.Name)))
                            {
                                Assert.IsNotNull(
                                    Attribute.GetCustomAttribute(action, typeof(AuthorizeAttribute)),
                                    string.Format("{0} action {1}(...) without BcmsAuthorize attribute.", controller.Name, action.Name));
                            }
                        }
                    }
                }
            }
        }
    }
}
