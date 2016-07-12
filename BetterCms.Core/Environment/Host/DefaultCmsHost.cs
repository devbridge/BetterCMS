using System.Web;

using BetterCms.Core.Services;

using BetterModules.Core.DataAccess.DataContext.Migrations;
using BetterModules.Core.Web.Environment.Host;
using BetterModules.Core.Web.Modules.Registration;

namespace BetterCms.Core.Environment.Host
{
    public class DefaultCmsHost : DefaultWebApplicationHost, ICmsHost
    {
        private readonly IRedirectControl redirectControl;

        public DefaultCmsHost(IWebModulesRegistration modulesRegistration, IMigrationRunner migrationRunner, IRedirectControl redirectControl)
            : base(modulesRegistration, migrationRunner)
        {
            this.redirectControl = redirectControl;
        }

        /// <summary>
        /// Called when the host application authenticates a web request.
        /// </summary>
        /// <param name="application"></param>
        public override void OnAuthenticateRequest(HttpApplication application)
        {
            // Impersonates user as anonymous, if requested
            if (application.Request["bcms-view-page-as-anonymous"] == "1")
            {
                application.Context.User = null;
            }

            base.OnAuthenticateRequest(application);
        }

        /// <summary>
        /// Called when the lifecyle of the request begins.
        /// </summary>
        /// <param name="application">The application.</param>
        public override void OnBeginRequest(HttpApplication application)
        {
            var sourceUrl = application.Request.RawUrl;
            if (redirectControl != null && string.IsNullOrEmpty(application.Request.CurrentExecutionFilePathExtension))
            {
                var redirectUrl = redirectControl.FindRedirect(sourceUrl);
                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    application.Response.RedirectPermanent(redirectUrl);
                }
            }
            base.OnBeginRequest(application);
        }
    }
}
