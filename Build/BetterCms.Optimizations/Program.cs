using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using BetterCms.Core.Modules;
using BetterCms.Module.Blog;
using BetterCms.Module.MediaManager;
using BetterCms.Module.Pages;
using BetterCms.Module.Root;
using BetterCms.Module.Templates;
using BetterCms.Module.Users;
using BetterCms.Optimizations.JsOptimization;

namespace BetterCms.Optimizations
{
    class Program
    {
        /// <summary>
        /// Mains the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        static void Main(string[] args)
        {
            var descriptors =
                      (new ModuleDescriptor[]
                    {
                        new BlogModuleDescriptor(),
                        new TemplatesModuleDescriptor(),
                        new MediaManagerModuleDescriptor(),
                        new PagesModuleDescriptor(),
                        new RootModuleDescriptor(),
                        new UsersModuleDescriptor()
                    }).ToList();

            RequireJsConfigRenderer requireJsConfigRenderer = new RequireJsConfigRenderer(descriptors);

            string fileName = "bcms.build.config.js";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (var file = File.Open(fileName, FileMode.CreateNew, FileAccess.Write))
            {                
                requireJsConfigRenderer.Write(file);
            }
        }
    }
}
