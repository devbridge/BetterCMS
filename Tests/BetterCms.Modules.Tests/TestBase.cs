// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestBase.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
