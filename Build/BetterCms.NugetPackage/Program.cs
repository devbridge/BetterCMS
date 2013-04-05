using System;
using System.IO;
using System.Reflection;

using BetterCms.Core.Environment.Host;

namespace BetterCms.NugetPackage
{
    /// <summary>
    /// Fake program to force solution to build NuGet package.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Mains the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main(string[] args)
        {
            var nugetTemplateFile = args[0];    // BetterCMS.nuspec.template;
            var nugetFile = args[1];            // BetterCMS.nuspec;

            var version = GetVersion();

            using (StreamReader sr = new StreamReader(nugetTemplateFile))
            {
                try
                {
                    var templateFile = sr.ReadToEnd();
                    templateFile = templateFile.Replace("@BetterCMSVersion@", version);

                    using (StreamWriter sw = new StreamWriter(nugetFile))
                    {
                        try
                        {
                            sw.Write(templateFile);
                        }
                        finally
                        {
                            sw.Close();
                        }
                    }
                }
                finally
                {
                    sr.Close();
                }
            }
        }

        private static string GetVersion()
        {
            var cmsCoreAssembly = typeof(ICmsHost).Assembly;
            var assemblyInformationVersion = Attribute.GetCustomAttributes(cmsCoreAssembly, typeof(AssemblyInformationalVersionAttribute));
            
            if (assemblyInformationVersion.Length > 0)
            {
                var informationVersion = ((AssemblyInformationalVersionAttribute)assemblyInformationVersion[0]);

                return informationVersion.InformationalVersion;                
            }

            return cmsCoreAssembly.GetName().Version.ToString(3);
        }
    }
}
