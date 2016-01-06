using System.Web.Hosting;

using BetterModules.Core.Environment.FileSystem;

namespace BetterCms.Core.Environment.FileSystem
{
    public class CmsWorkingDirectory : DefaultWorkingDirectory
    {
        private readonly ICmsConfiguration configuration;

        public CmsWorkingDirectory(ICmsConfiguration configuration)
        {
            this.configuration = configuration;
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
