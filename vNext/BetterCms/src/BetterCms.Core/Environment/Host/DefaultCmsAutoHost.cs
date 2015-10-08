using System.Web;

using BetterCms.Core.Environment.Host;

using BetterModules.Core.Web.Environment.Application;
using BetterModules.Core.Web.Environment.Host;

[assembly: WebApplicationHost(typeof(DefaultCmsAutoHost), Order = 100)]
namespace BetterCms.Core.Environment.Host
{
    public class DefaultCmsAutoHost : DefaultWebApplicationAutoHost
    {
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
