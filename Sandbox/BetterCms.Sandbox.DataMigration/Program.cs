using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Environment.Assemblies;
using BetterCms.Core.Modules;

using BetterCms.Module.Blog;
using BetterCms.Module.MediaManager;
using BetterCms.Module.Pages;
using BetterCms.Module.Root;
using BetterCms.Module.Navigation;
using BetterCms.Module.Templates;
using BetterCms.Module.Users;

using Common.Logging;

namespace BetterCms.Sandbox.DataMigration
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static List<ModuleDescriptor> descriptors;

        static Program()
        {
             descriptors = 
                    (new ModuleDescriptor[]
                    {
                        new BlogModuleDescriptor(),
                        new NavigationModuleDescriptor(),
                        new TemplatesModuleDescriptor(),
                        new MediaManagerModuleDescriptor(),
                        new PagesModuleDescriptor(),
                        new RootModuleDescriptor(),
                        new UsersModuleDescriptor()
                    })
                    .ToList();
        }

        private static void Migrate(bool up)
        {            
            DefaultMigrationRunner runner = new DefaultMigrationRunner(new DefaultAssemblyLoader());
            runner.Migrate(descriptors, up);
        }

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("-- PRESS ANY KEY TO START DATABASE MIGRATIONS --");
                Console.ReadKey();

                Console.WriteLine("-- Migrate DOWN --");
                
                 Migrate(false);

                Console.WriteLine("-- Migrate  UP --");

                Migrate(true);
                
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
