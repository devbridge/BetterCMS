using System.Collections.Generic;
using System.Reflection;

using Autofac;

using BetterCms.Core;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.Dependencies;
using BetterCms.Core.Modules.Registration;
using BetterCms.Module.Pages;
using BetterCms.Module.Root;
using BetterCms.Test.Module.Helpers;
using BetterCms.Tests.Helpers;

namespace BetterCms.Test.Module
{
    public abstract class TestBase
    {
        protected static List<Assembly> KnownAssemblies { get; private set; }

        private ILifetimeScope container;

        private RandomTestDataProvider testDataProvider;
        
        static TestBase()
        {
            KnownAssemblies = new List<Assembly>(new[]
                                                     {
                                                         typeof(RootModuleDescriptor).Assembly,
                                                         typeof(PagesModuleDescriptor).Assembly
                                                     });

            EagerFetch.FetchingProvider = () => new FakeEagerFetchingProvider();
        }

        public ILifetimeScope Container
        {
            get
            {
                if (container == null)
                {
                    container = CreateContainer();
                }

                return container;
            }
        }

        public RandomTestDataProvider TestDataProvider
        {
            get
            {
                if (testDataProvider == null)
                {
                    testDataProvider = new RandomTestDataProvider();
                }
                return testDataProvider;
            }
        }

        private static ILifetimeScope CreateContainer()
        {
            ContainerBuilder updater = CmsContext.InitializeContainer();
           
            updater.RegisterType<StubMappingResolver>().As<IMappingResolver>();
            ContextScopeProvider.RegisterTypes(updater);

            var container = ContextScopeProvider.CreateChildContainer();

            IModulesRegistration modulesRegistration = container.Resolve<IModulesRegistration>();
            foreach (var knownAssembly in KnownAssemblies)
            {
                modulesRegistration.AddModuleDescriptorTypeFromAssembly(knownAssembly);
            }            
            modulesRegistration.InitializeModules();                

            return container;
        }      
    }
}
