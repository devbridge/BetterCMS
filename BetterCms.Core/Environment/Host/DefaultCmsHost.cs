using System.Web;

using Devbridge.Platform.Core.DataAccess.DataContext.Migrations;
using Devbridge.Platform.Core.Web.Environment.Host;
using Devbridge.Platform.Core.Web.Modules.Registration;

namespace BetterCms.Core.Environment.Host
{
    public class DefaultCmsHost : DefaultWebApplicationHost, ICmsHost
    {
        public DefaultCmsHost(IWebModulesRegistration modulesRegistration, IMigrationRunner migrationRunner)
            : base(modulesRegistration, migrationRunner)
        {
        }

        /// <summary>
        /// Called when the host application authenticates a web request.
        /// </summary>
        /// <param name="application"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void OnAuthenticateRequest(HttpApplication application)
        {
            // Impersonates user as anonymous, if requested
            if (application.Request["bcms-view-page-as-anonymous"] == "1")
            {
                application.Context.User = null;
            }

            base.OnAuthenticateRequest(application);
        }
    }
}
