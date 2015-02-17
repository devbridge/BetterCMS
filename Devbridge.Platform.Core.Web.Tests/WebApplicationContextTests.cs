using System.Linq;
using System.Web.Mvc;

using Autofac;

using Devbridge.Platform.Core.Configuration;
using Devbridge.Platform.Core.DataAccess;
using Devbridge.Platform.Core.DataAccess.DataContext;
using Devbridge.Platform.Core.DataAccess.DataContext.Fetching;
using Devbridge.Platform.Core.DataAccess.DataContext.Migrations;
using Devbridge.Platform.Core.Dependencies;
using Devbridge.Platform.Core.Environment.Assemblies;
using Devbridge.Platform.Core.Environment.FileSystem;
using Devbridge.Platform.Core.Modules.Registration;
using Devbridge.Platform.Core.Security;
using Devbridge.Platform.Core.Web.Configuration;
using Devbridge.Platform.Core.Web.Dependencies;
using Devbridge.Platform.Core.Web.Environment.Assemblies;
using Devbridge.Platform.Core.Web.Environment.Host;
using Devbridge.Platform.Core.Web.Modules.Registration;
using Devbridge.Platform.Core.Web.Mvc;
using Devbridge.Platform.Core.Web.Mvc.Commands;
using Devbridge.Platform.Core.Web.Mvc.Extensions;
using Devbridge.Platform.Core.Web.Mvc.Routes;
using Devbridge.Platform.Core.Web.Security;
using Devbridge.Platform.Core.Web.Services.Caching;
using Devbridge.Platform.Core.Web.Web;
using Devbridge.Platform.Core.Web.Web.EmbeddedResources;
using Devbridge.Platform.Sample.Web.Module;

using NUnit.Framework;

using RazorGenerator.Mvc;

namespace Devbridge.Platform.Core.Web.Tests
{
    [TestFixture]
    public class WebApplicationContextTests : TestBase
    {
        [Test]
        public void Dependecies_Should_Be_Initialized_Correctly()
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                // Core services
                Assert.IsNotNull(container.Resolve<IConfiguration>());
                Assert.IsNotNull(container.Resolve<IModulesRegistration>());
                Assert.IsNotNull(container.Resolve<ISessionFactoryProvider>());
                Assert.IsNotNull(container.Resolve<IAssemblyLoader>());
                Assert.IsNotNull(container.Resolve<IUnitOfWorkFactory>());
                Assert.IsNotNull(container.Resolve<IMappingResolver>());
                Assert.IsNotNull(container.Resolve<IWorkingDirectory>());
                Assert.IsNotNull(container.Resolve<IFetchingProvider>());
                Assert.IsNotNull(container.Resolve<IUnitOfWork>());
                Assert.IsNotNull(container.Resolve<IRepository>());
                Assert.IsNotNull(container.Resolve<IVersionChecker>());
                Assert.IsNotNull(container.Resolve<IMigrationRunner>());
                Assert.IsNotNull(container.Resolve<IPrincipalProvider>());
                Assert.IsNotNull(container.Resolve<IAssemblyManager>());

                // Web services
                Assert.IsNotNull(container.Resolve<IWebConfiguration>());
                Assert.IsNotNull(container.Resolve<IWebModulesRegistration>());
                Assert.IsNotNull(container.Resolve<DefaultWebControllerFactory>());
                Assert.IsNotNull(container.Resolve<IEmbeddedResourcesProvider>());
                Assert.IsNotNull(container.Resolve<IHttpContextAccessor>());
                Assert.IsNotNull(container.Resolve<IControllerExtensions>());
                Assert.IsNotNull(container.Resolve<ICommandResolver>());
                Assert.IsNotNull(container.Resolve<IRouteTable>());
                Assert.IsNotNull(container.Resolve<PerWebRequestContainerProvider>());
                Assert.IsNotNull(container.Resolve<ICacheService>());
                Assert.IsNotNull(container.Resolve<IWebApplicationHost>());

                // Core Overrided instances
                Assert.AreEqual(container.Resolve<IConfiguration>().GetType(), typeof(DefaultWebConfigurationSection));
                Assert.AreEqual(container.Resolve<IModulesRegistration>().GetType(), typeof(DefaultWebModulesRegistration));
                Assert.AreEqual(container.Resolve<IPrincipalProvider>().GetType(), typeof(DefaultWebPrincipalProvider));
                Assert.AreEqual(container.Resolve<IAssemblyManager>().GetType(), typeof(DefaultWebAssemblyManager));
            }
        }

        [Test]
        public void Correct_Modules_Should_Be_Loaded()
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                var modulesRegistration = container.Resolve<IModulesRegistration>();
                var modules = modulesRegistration.GetModules();

                Assert.IsNotNull(modules);
                Assert.AreEqual(modules.Count(), 1);
                Assert.AreEqual(modules.First().ModuleDescriptor.GetType(), typeof(SampleWebModuleDescriptor));
            }
        }

        [Test]
        public void Correct_Controller_Factory_Should_Be_Registered()
        {
            Assert.IsTrue(ControllerBuilder.Current.GetControllerFactory() is DefaultWebControllerFactory); 
        }
        
        [Test]
        public void Precompiled_Views_Engine_Should_Be_Registered()
        {
            Assert.IsTrue(ViewEngines.Engines.Any(e => e is CompositePrecompiledMvcEngine));
        }

        [Test]
        public void Should_Retrieve_Registered_Host_Successfully()
        {
            var host = WebApplicationContext.RegisterHost();
            
            Assert.IsNotNull(host);
            Assert.IsTrue(host is DefaultWebApplicationHost);
        }
    }
}
