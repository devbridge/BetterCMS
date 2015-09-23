using System.Collections.Generic;
using System.Reflection;

using Autofac;

using BetterCMS.Module.LuceneSearch.Helpers;

using BetterCms.Core;
using BetterCms.Core.Modules.Registration;

using BetterCms.Module.Api;
using BetterCms.Module.Blog;
using BetterCms.Module.ImagesGallery;
using BetterCms.Module.MediaManager;
using BetterCms.Module.Newsletter;
using BetterCms.Module.Pages;
using BetterCms.Module.Root;
using BetterCms.Module.Users;
using BetterCms.Module.Users.Api;

using BetterCms.Test.Module.Helpers;
using BetterCms.Tests.Helpers;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Dependencies;

namespace BetterCms.Test.Module
{
    public abstract class TestBase
    {
        protected static List<Assembly> KnownAssemblies { get; private set; }

        private ILifetimeScope container;

        private RandomTestDataProvider testDataProvider;

        protected TestBase()
        {
            KnownAssemblies = new List<Assembly>(new[]
                                                     {
                                                         typeof(RootModuleDescriptor).Assembly,
                                                         typeof(PagesModuleDescriptor).Assembly,
                                                         typeof(BlogModuleDescriptor).Assembly,
                                                         typeof(NewsletterModuleDescriptor).Assembly,
                                                         typeof(MediaManagerModuleDescriptor).Assembly,
                                                         typeof(UsersModuleDescriptor).Assembly,
                                                         typeof(ApiModuleDescriptor).Assembly,
                                                         typeof(UsersApiModuleDescriptor).Assembly,
                                                         typeof(ImagesGalleryModuleDescriptor).Assembly
                                                     });
            container = CreateContainer();

            HtmlAgilityPackHelper.FixMissingTagClosings();
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
            updater.RegisterType<FakeEagerFetchingProvider>().As<IFetchingProvider>();

            ContextScopeProvider.RegisterTypes(updater);

            var container = ContextScopeProvider.CreateChildContainer();

            ICmsModulesRegistration modulesRegistration = container.Resolve<ICmsModulesRegistration>();
            foreach (var knownAssembly in KnownAssemblies)
            {
                modulesRegistration.AddModuleDescriptorTypeFromAssembly(knownAssembly);
            }            
            modulesRegistration.InitializeModules();                

            return container;
        }      
    }
}
