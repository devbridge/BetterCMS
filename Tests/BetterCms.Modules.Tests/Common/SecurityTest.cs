using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Autofac;

using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Blog;
using BetterCms.Module.MediaManager;
using BetterCms.Module.Pages;
using BetterCms.Module.Root;
using BetterCms.Module.Templates;

using Moq;

using NUnit.Framework;

namespace BetterCms.Test.Module.Common
{
    [TestFixture]
    public class SecurityTest : TestBase
    {
        private readonly RootModuleDescriptor[] moduleDescriptors = new[] { new RootModuleDescriptor() };
        private readonly ContainerBuilder container = new ContainerBuilder();
        private readonly Mock<ICmsConfiguration> cmsConfigurationMock = new Mock<ICmsConfiguration>();

        private class TestItem
        {
            public Type Descriptor { get; set; }
            public List<string> ControllersToSkip { get; set; }
            public List<string> ActionsToSkip { get; set; }
            public TestItem(Type decriptor, List<string> controllersToSkip, List<string> actionsToSkip)
            {
                Descriptor = decriptor;
                ControllersToSkip = controllersToSkip;
                ActionsToSkip = actionsToSkip;
            }

            public List<Type> GetControllers()
            {
                return Descriptor.Assembly.GetTypes()
                    .Where(t => !t.IsAbstract && typeof(Controller).IsAssignableFrom(t) && t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }

        [Test]
        public void All_Controllers_Needs_Authorize_Attribute()
        {
            // Test checks if controller contains [Authorize] attribute.
            //    If controller must be public - write it name to skip check.
            //    But if controller is skipped - all its actions will be checked for [Authorize].
            //       If controller action needs to be available for not logged in user - write it name to skip check.

            return; // TODO: remove this after all controllers/actions will be marked with authorize that need it.

            var testItems = new List<TestItem>
                {
                    new TestItem(typeof(RootModuleDescriptor), new List<string> { "AuthenticationController" }, new List<string> { "" }),
                    new TestItem(typeof(PagesModuleDescriptor),  new List<string> { "" }, new List<string> { "" }),
                    new TestItem(typeof(BlogModuleDescriptor),  new List<string> { "" }, new List<string> { "" }),
                    new TestItem(typeof(MediaManagerModuleDescriptor),  new List<string> { "" }, new List<string> { "" }),
                    new TestItem(typeof(TemplatesModuleDescriptor),  new List<string> { "" }, new List<string> { "" }),
                };

            foreach (var testItem in testItems)
            {
                var controllers = testItem.GetControllers();

                foreach (var controller in controllers)
                {
                    if (!testItem.ControllersToSkip.Contains(controller.Name))
                    {
                         Assert.IsNotNull(Attribute.GetCustomAttribute(controller, typeof(AuthorizeAttribute)), string.Format("{0} class without Authorize attribute.", controller.Name));
                    }
                    else
                    {
                        var actions = controller.GetMethods()
                            .Where(m => m.IsPublic && m.ReturnType == typeof(ActionResult))
                            .Where(m => Attribute.GetCustomAttribute(m, typeof(NonActionAttribute)) == null);

                        foreach (var action in actions)
                        {
                            if (!testItem.ActionsToSkip.Contains(action.Name))
                            {
                                Assert.IsNotNull(Attribute.GetCustomAttribute(action, typeof(AuthorizeAttribute)), string.Format("{0} action {1}(...) without Authorize attribute.", controller.Name, action.Name));
                            }
                        }
                    }
                }
            }
        }

        [Test]
        public void All_Module_Descriptors_Should_Override_RegisterUserRoles()
        {
            foreach (var descriptor in moduleDescriptors)
            {
                var roles = descriptor.RegisterUserRoles(container, cmsConfigurationMock.Object);
                Assert.IsNotNull(roles);
            }
        }

        [Test]
        public void All_Action_Projections_in_Modules_Should_Contain_IsVisible_Setted()
        {
// TODO: update or remove if no longer needed.
//            foreach (var descriptor in moduleDescriptors)
//            {
//                IList<IPageActionProjection> allProjections = new List<IPageActionProjection>();
//                IEnumerable<IPageActionProjection> projections;
//
//                projections = descriptor.RegisterSidebarHeaderProjections(container, cmsConfigurationMock.Object);
//                if (projections != null)
//                {
//                    projections.ToList().ForEach(allProjections.Add);
//                }
//
//                projections = descriptor.RegisterSidebarMainProjections(container, cmsConfigurationMock.Object);
//                if (projections != null)
//                {
//                    projections.ToList().ForEach(allProjections.Add);
//                }
//
//                projections = descriptor.RegisterSidebarSideProjections(container, cmsConfigurationMock.Object);
//                if (projections != null)
//                {
//                    projections.ToList().ForEach(allProjections.Add);
//                }
//
//                projections = descriptor.RegisterSiteSettingsProjections(container, cmsConfigurationMock.Object);
//                if (projections != null)
//                {
//                    projections.ToList().ForEach(allProjections.Add);
//                }
//
//                foreach (var projection in allProjections)
//                {
//                    Assert.IsNotNull(projection.AccessRole);
//                }
//            }
        }
    }
}
