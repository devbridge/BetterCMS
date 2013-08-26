using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Autofac;

using BetterCms.Configuration;
using BetterCms.Module.Blog;
using BetterCms.Module.MediaManager;
using BetterCms.Module.Pages;
using BetterCms.Module.Root;
using BetterCms.Module.Installation;
using BetterCms.Module.Users;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Common
{
    [TestFixture]
    public class SecurityTest : TestBase
    {
        private readonly RootModuleDescriptor[] moduleDescriptors = new[] { new RootModuleDescriptor(new CmsConfigurationSection()) };
        private readonly ContainerBuilder container = new ContainerBuilder();
        private readonly Mock<ICmsConfiguration> cmsConfigurationMock = new Mock<ICmsConfiguration>();

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
