using System.Web;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Web.Environment.Host;
using BetterModules.Core.Web.Modules.Registration;

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
