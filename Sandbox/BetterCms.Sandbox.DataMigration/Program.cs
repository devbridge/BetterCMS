// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Devbridge Group LLC">
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
using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using BetterCms.Configuration;

using BetterCms.Core.Modules;

using BetterCms.Module.Blog;
using BetterCms.Module.ImagesGallery;
using BetterCms.Module.LuceneSearch;
using BetterCms.Module.MediaManager;
using BetterCms.Module.Newsletter;
using BetterCms.Module.Pages;
using BetterCms.Module.Root;
using BetterCms.Module.Installation;
using BetterCms.Module.Search;
using BetterCms.Module.Users;

using BetterModules.Core;
using BetterModules.Core.Configuration;

using Common.Logging;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Dependencies;
using BetterModules.Core.Environment.Assemblies;
using BetterModules.Core.Modules;
using BetterModules.Core.Web.Configuration;

namespace BetterCms.Sandbox.DataMigration
{
    internal class Program
    {
        class VersionCheckerStub : IVersionChecker
        {
            public bool VersionExists(string moduleName, long version)
            {
                return false;
            }

            public void AddVersion(string moduleName, long version)
            {
            }

            public string CacheFilePath { get; private set; }
        }

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static List<CmsModuleDescriptor> descriptors;

        static Program()
        {
             ICmsConfiguration configuration = new CmsConfigurationSection();
             descriptors = 
                    (new CmsModuleDescriptor[]
                    {
                        new BlogModuleDescriptor(configuration),
                        new InstallationModuleDescriptor(configuration),
                        new MediaManagerModuleDescriptor(configuration),
                        new PagesModuleDescriptor(configuration),
                        new RootModuleDescriptor(configuration),
                        new UsersModuleDescriptor(configuration),
                        new NewsletterModuleDescriptor(configuration),
                        new UsersModuleDescriptor(configuration),
                        new ImagesGalleryModuleDescriptor(configuration),
                        new SearchModuleDescriptor(configuration),
                        new LuceneSearchModuleDescriptor(configuration)
                    })
                    .ToList();
        }

        private static void Migrate()
        {
            ICmsConfigurationLoader configurationLoader = new CmsConfigurationLoader();
            ICmsConfiguration cmsConfiguration = configurationLoader.LoadCmsConfiguration();

            var builder = ApplicationContext.InitializeContainer(null, cmsConfiguration);
            builder.RegisterInstance(cmsConfiguration)
                    .As<IConfiguration>()
                    .As<IWebConfiguration>()
                    .As<ICmsConfiguration>()
                    .SingleInstance();
            ContextScopeProvider.RegisterTypes(builder);
            ApplicationContext.LoadAssemblies();

            IVersionChecker versionChecker = new VersionCheckerStub();
            var runner = new DefaultMigrationRunner(new DefaultAssemblyLoader(), cmsConfiguration, versionChecker);
            runner.MigrateStructure(descriptors.Cast<ModuleDescriptor>().ToList());
        }

        private static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0 || args[0] != "auto")
                {
                    Console.WriteLine("-- PRESS ANY KEY TO START DATABASE MIGRATIONS --");
                    Console.ReadKey();
                }

                Console.WriteLine("-- Migrate  UP --");

                Migrate();
                
                Console.WriteLine("-- DONE --");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                Console.ReadKey();
            }
        }
    }
}
