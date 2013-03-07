using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

using Autofac;

using BetterCms.Core;
using BetterCms.Core.Dependencies;
using BetterCms.Core.Environment.Host;
using BetterCms.Core.Modules.Registration;

namespace BetterCms.Sandbox.Mvc4
{
    public class MvcApplication : HttpApplication
    {
        public const string UserRolesKey = "UserRoles";

        private static ICmsHost cmsHost;

        protected void Application_Start()
        {     
            cmsHost = BetterCmsContext.RegisterHost();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var container = ContextScopeProvider.CreateChildContainer();
            var modulesRegistration = container.Resolve<IModulesRegistration>();
            var userRoles = new List<string> { "User", "Admin" };
            var roles = modulesRegistration.GetUserAccessRoles().Select(m => m.Name).ToList();
            userRoles.AddRange(roles);
            Application[UserRolesKey] = userRoles;

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
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    var identity = new GenericIdentity(authTicket.Name, "Forms");
                    var principal = new GenericPrincipal(identity, ((List<string>)Application[MvcApplication.UserRolesKey]).ToArray());
                    Context.User = principal;
                }
            }
        }
    }
}