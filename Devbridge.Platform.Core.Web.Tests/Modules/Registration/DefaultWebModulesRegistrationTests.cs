using System.Linq;
using System.Reflection;
using System.Web.Routing;

using Devbridge.Platform.Core.Environment.Assemblies;
using Devbridge.Platform.Core.Web.Modules;
using Devbridge.Platform.Core.Web.Modules.Registration;
using Devbridge.Platform.Core.Web.Mvc.Extensions;
using Devbridge.Platform.Sample.Web.Module;

using Moq;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Modules.Registration
{
    [TestFixture]
    public class DefaultWebModulesRegistrationTests : TestBase
    {
        [Test]
        public void Should_Find_Module_By_AreaName()
        {
            var loader = new Mock<IAssemblyLoader>();
            loader
                .Setup(l => l.GetLoadableTypes(It.IsAny<Assembly>()))
                .Returns<Assembly>(r => new[] { typeof(SampleWebModuleDescriptor) });

            var service = new DefaultWebModulesRegistration(loader.Object, new Mock<IControllerExtensions>().Object);
            service.AddModuleDescriptorTypeFromAssembly(GetType().Assembly);
            service.InitializeModules();

            var sampleDescriptor = new SampleWebModuleDescriptor();

            WebModuleDescriptor descriptor = service.FindModuleByAreaName(sampleDescriptor.AreaName);
            Assert.IsNotNull(descriptor);
            Assert.AreEqual(descriptor.Name, sampleDescriptor.Name);
        }
        
        [Test]
        public void Should_Not_Find_Module_By_AreaName()
        {
            var service = new DefaultWebModulesRegistration(new Mock<IAssemblyLoader>().Object, new Mock<IControllerExtensions>().Object);
            var descriptor = service.FindModuleByAreaName("Test");

            Assert.IsNull(descriptor);
        }

        [Test]
        public void Should_Find_Is_Module_Registered_By_AreaName()
        {
            var loader = new Mock<IAssemblyLoader>();
            loader
                .Setup(l => l.GetLoadableTypes(It.IsAny<Assembly>()))
                .Returns<Assembly>(r => new[] { typeof(SampleWebModuleDescriptor) });

            var service = new DefaultWebModulesRegistration(loader.Object, new Mock<IControllerExtensions>().Object);
            service.AddModuleDescriptorTypeFromAssembly(GetType().Assembly);
            service.InitializeModules();

            var sampleDescriptor = new SampleWebModuleDescriptor();

            var isRegisteted = service.IsModuleRegisteredByAreaName(sampleDescriptor.AreaName);
            Assert.IsTrue(isRegisteted);
        }

        [Test]
        public void Should_Not_Find_Is_Module_Registered_By_AreaName()
        {
            var service = new DefaultWebModulesRegistration(new Mock<IAssemblyLoader>().Object, new Mock<IControllerExtensions>().Object);
            var isRegisteted = service.IsModuleRegisteredByAreaName("Test");

            Assert.IsFalse(isRegisteted);
        }

        [Test]
        public void Should_Register_Route_Correctly()
        {
            var loader = new Mock<IAssemblyLoader>();
            loader
                .Setup(l => l.GetLoadableTypes(It.IsAny<Assembly>()))
                .Returns<Assembly>(r => new[] { typeof(SampleWebModuleDescriptor) });

            var service = new DefaultWebModulesRegistration(loader.Object, new Mock<IControllerExtensions>().Object);
            service.AddModuleDescriptorTypeFromAssembly(GetType().Assembly);
            service.InitializeModules();

            var routes = new RouteCollection();
            service.RegisterKnownModuleRoutes(routes);

            Assert.IsNotEmpty(routes);
            Assert.IsTrue(routes.Any(r => ((Route)r).Url == "module-dbwebsample/{controller}/{action}"));
        }
    }
}
