using BetterCms.Configuration;
using BetterModules.Core.Environment.FileSystem;
using Microsoft.AspNet.Hosting;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;

namespace BetterCms.Core.Environment.FileSystem
{
    public class CmsWorkingDirectory : DefaultWorkingDirectory
    {
        private readonly CmsConfigurationSection configuration;
        private readonly IHostingEnvironment hostingEnvironment;
        public CmsWorkingDirectory(IOptions<CmsConfigurationSection> configuration, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this.configuration = configuration.Options;
            this.hostingEnvironment = hostingEnvironment;
        }

        // TODO check if this is correct aproach
        public override string GetWorkingDirectoryPath()
        {
            if (!string.IsNullOrWhiteSpace(configuration.WorkingDirectoryRootPath))
            {
                return hostingEnvironment.MapPath(configuration.WorkingDirectoryRootPath);
            }

            return "~/App_Data/BetterCMS";
        }
    }
}
