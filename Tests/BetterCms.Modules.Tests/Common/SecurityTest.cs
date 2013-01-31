using System.Collections.Generic;
using System.Linq;

using Autofac;

using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root;

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
            foreach (var descriptor in moduleDescriptors)
            {
                IList<IPageActionProjection> allProjections = new List<IPageActionProjection>();
                IEnumerable<IPageActionProjection> projections;

                projections = descriptor.RegisterSidebarHeaderProjections(container, cmsConfigurationMock.Object);
                if (projections != null)
                {
                    projections.ToList().ForEach(allProjections.Add);
                }

                projections = descriptor.RegisterSidebarMainProjections(container, cmsConfigurationMock.Object);
                if (projections != null)
                {
                    projections.ToList().ForEach(allProjections.Add);
                }

                projections = descriptor.RegisterSidebarSideProjections(container, cmsConfigurationMock.Object);
                if (projections != null)
                {
                    projections.ToList().ForEach(allProjections.Add);
                }

                projections = descriptor.RegisterSiteSettingsProjections(container, cmsConfigurationMock.Object);
                if (projections != null)
                {
                    projections.ToList().ForEach(allProjections.Add);
                }

                foreach (var projection in allProjections)
                {
                    Assert.IsNotNull(projection.IsVisible);
                }
            }
        }
    }
}
