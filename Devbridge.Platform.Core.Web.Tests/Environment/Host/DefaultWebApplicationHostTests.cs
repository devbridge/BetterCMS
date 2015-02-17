using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

using Devbridge.Platform.Core.DataAccess.DataContext.Migrations;
using Devbridge.Platform.Core.Modules;
using Devbridge.Platform.Core.Modules.Registration;
using Devbridge.Platform.Core.Web.Environment.Host;
using Devbridge.Platform.Core.Web.Modules.Registration;
using Devbridge.Platform.Events;

using Moq;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Environment.Host
{
    [TestFixture]
    public class DefaultWebApplicationHostTests : TestBase
    {
        private HttpApplication application;
        private bool eventFired;

        [Test]
        public void ShouldExecute_OnApplicationStart_Correctly()
        {
            eventFired = false;
            var routesRegistered = false;
            var databaseMigrated = false;

            var registration = new Mock<IWebModulesRegistration>();
            registration
                .Setup(r => r.RegisterKnownModuleRoutes(It.IsAny<RouteCollection>()))
                .Callback<RouteCollection>(rc => routesRegistered = true);

            var moduleDescriptor1 = new ModuleRegistrationContext(new Mock<ModuleDescriptor>().Object);
            var moduleDescriptor2 = new ModuleRegistrationContext(new Mock<ModuleDescriptor>().Object);

            registration
                .Setup(r => r.GetModules())
                .Returns(() => new[] { moduleDescriptor1, moduleDescriptor2 });

            var migrationRunner = new Mock<IMigrationRunner>();
            migrationRunner
                .Setup(r => r.MigrateStructure(It.IsAny<IList<ModuleDescriptor>>()))
                .Callback<IList<ModuleDescriptor>>(
                    descriptors =>
                    {
                        Assert.AreEqual(descriptors.Count, 2);
                        Assert.IsTrue(descriptors.Any(d => d == moduleDescriptor1.ModuleDescriptor));
                        Assert.IsTrue(descriptors.Any(d => d == moduleDescriptor2.ModuleDescriptor));

                        databaseMigrated = true;
                    });

            var host = new DefaultWebApplicationHost(registration.Object, migrationRunner.Object);
            CreateApplication();

            WebCoreEvents.Instance.HostStart += Instance_Fired;
            host.OnApplicationStart(application);
            WebCoreEvents.Instance.HostStart -= Instance_Fired;

            Assert.IsTrue(eventFired);
            Assert.IsTrue(routesRegistered);
            Assert.IsTrue(databaseMigrated);
        }
        
        [Test]
        public void ShouldExecute_OnApplicationEnd_Correctly()
        {
            var host = CreateHost();
            CreateApplication();
            eventFired = false;

            WebCoreEvents.Instance.HostStop += Instance_Fired;
            host.OnApplicationEnd(application);
            WebCoreEvents.Instance.HostStop -= Instance_Fired;

            Assert.IsTrue(eventFired);
        }
        
        [Test]
        public void ShouldExecute_OnApplicationError_Correctly()
        {
            var host = CreateHost();
            CreateApplication();
            eventFired = false;

            WebCoreEvents.Instance.HostError += Instance_Fired;
            host.OnApplicationError(application);
            WebCoreEvents.Instance.HostError -= Instance_Fired;

            Assert.IsTrue(eventFired);
        }
        
        [Test]
        public void ShouldExecute_OnAuthenticateRequest_Correctly()
        {
            var host = CreateHost();
            CreateApplication();
            eventFired = false;
            
            WebCoreEvents.Instance.HostAuthenticateRequest += Instance_Fired;
            host.OnAuthenticateRequest(application);
            WebCoreEvents.Instance.HostAuthenticateRequest -= Instance_Fired;

            Assert.IsTrue(eventFired);
        }

        void Instance_Fired(SingleItemEventArgs<HttpApplication> args)
        {
            Assert.IsNotNull(args);
            Assert.IsNotNull(args.Item);
            Assert.AreEqual(args.Item, application);
            eventFired = true;
        }

        private IWebApplicationHost CreateHost()
        {
            var registration = new Mock<IWebModulesRegistration>();
            var migrationRunner = new Mock<IMigrationRunner>();

            var host = new DefaultWebApplicationHost(registration.Object, migrationRunner.Object);

            return host;
        }

        private void CreateApplication()
        {
            application = new Mock<HttpApplication>().Object;
        }
    }
}
