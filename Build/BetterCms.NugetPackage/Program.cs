using System.IO;
using System.Reflection;

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

            const string sufix = "-pre";
            var version = Assembly.GetEntryAssembly().GetName().Version;
            var textVersion = string.Format("{0}.{1}.{2}{3}", version.Major, version.Minor, version.Build, sufix);

            using (StreamReader sr = new StreamReader(nugetTemplateFile))
            {
                try
                {
                    var templateFile = sr.ReadToEnd();
                    templateFile = templateFile.Replace("@BetterCMSVersion@", textVersion);

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
    }
}
