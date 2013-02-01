using System;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

using BetterCms.Core;
using BetterCms.Core.Environment.Host;

namespace BetterCms.Sandbox.Mvc4
{
    public class MvcApplication : HttpApplication
    {
        private static ICmsHost cmsHost;

        protected void Application_Start()
        {     
            cmsHost = BetterCmsContext.RegisterHost();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            cmsHost.OnApplicationStart(this);            
        }

        protected void Application_BeginRequest()
        {
            cmsHost.OnBeginRequest(this);
        }

        protected void Application_EndRequest()
        {
            cmsHost.OnEndRequest(this);
        }

        protected void Application_Error()
        {            
            cmsHost.OnApplicationError(this);
        }

        protected void Application_End()
        {
            cmsHost.OnApplicationEnd(this);
        }

        /// <summary>
        /// Handles the AuthenticateRequest event of the Application control.
        /// TODO: remove when authentication will be implemented
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {            
            string roles = System.Configuration.ConfigurationManager.AppSettings["TestUserRoles"];
            string name = System.Configuration.ConfigurationManager.AppSettings["TestUserName"];

            string[] rolesList;
            if (!string.IsNullOrWhiteSpace(roles))
            {
                rolesList = roles.Split(',');
            }
            else
            {
                rolesList = new string[0];
            }


            var principal = new GenericPrincipal(new GenericIdentity(name), rolesList);
            HttpContext.Current.User = principal;
        }
    }
}