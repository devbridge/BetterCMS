using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using BetterCms.Configuration;
using BetterCms.Core.Modules;

using Common.Logging;

namespace BetterCms.Optimizations.JsOptimization
{
    public class RequireJsConfigRenderer
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private List<ModuleDescriptor> descriptors;

        public RequireJsConfigRenderer(List<ModuleDescriptor> descriptors)
        {
            this.descriptors = descriptors;
        }

        public void Write(Stream stream)
        {
            using (TextWriter writer = new StreamWriter(stream))
            {
                foreach (var module in descriptors)
                {
                    var jsModules = module.RegisterJavaScriptModules(null);
                    if (jsModules != null)
                    {
                        foreach (var jsModule in jsModules)
                        {
                            string path = string.Format("'{0}' : '{1}',", jsModule.Name, jsModule.Path.Replace("/file/", "./"));
                            writer.WriteLine(path);
                        }
                    }
                }
            }
        }        
    }
}
