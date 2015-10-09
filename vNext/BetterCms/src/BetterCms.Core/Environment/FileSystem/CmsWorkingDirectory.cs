using BetterCms.Configuration;
using BetterModules.Core.Environment.FileSystem;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;

namespace BetterCms.Core.Environment.FileSystem
{
    public class CmsWorkingDirectory : DefaultWorkingDirectory
    {
        private readonly CmsConfigurationSection configuration;

        public CmsWorkingDirectory(IOptions<CmsConfigurationSection> configuration, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this.configuration = configuration.Options;
        }

        public override string GetWorkingDirectoryPath()
        {
            if (!string.IsNullOrWhiteSpace(configuration.WorkingDirectoryRootPath))
            {
                return HostingEnvironment.MapPath(configuration.WorkingDirectoryRootPath);
            }

            return "~/App_Data/BetterCMS";
        }
    }
}
