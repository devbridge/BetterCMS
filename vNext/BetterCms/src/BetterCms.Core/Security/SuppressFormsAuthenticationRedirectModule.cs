using System;
using System.Web;

using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace BetterCms.Core.Security
{
    public class SuppressFormsAuthenticationRedirectModule : IHttpModule
    {
        /// <summary>
        /// Indicates if module is starting.
        /// </summary>
        private static bool isStarting;

        private static readonly object SuppressAuthenticationKey = new Object();

        public static void SuppressAuthenticationRedirect(HttpContext context)
        {
            context.Items[SuppressAuthenticationKey] = true;
        }

        public static void SuppressAuthenticationRedirect(HttpContextBase context)
        {
            context.Items[SuppressAuthenticationKey] = true;
        }

        public void Init(HttpApplication context)
        {
            context.PostReleaseRequestState += OnPostReleaseRequestState;
            context.EndRequest += OnEndRequest;
        }

        private void OnPostReleaseRequestState(object source, EventArgs args)
        {
            var context = (HttpApplication)source;
            var response = context.Response;
            var request = context.Request;

            if (response.StatusCode == 401 && request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                SuppressAuthenticationRedirect(context.Context);
            }
        }

        private void OnEndRequest(object source, EventArgs args)
        {
            var context = (HttpApplication)source;
            var response = context.Response;

            if (context.Context.Items.Contains(SuppressAuthenticationKey))
            {
                response.TrySkipIisCustomErrors = true;
                response.ClearContent();
                response.StatusCode = 401;
                response.RedirectLocation = null;
                response.AddHeader("Bcms-Redirect-To", System.Web.Security.FormsAuthentication.LoginUrl);
            }
        }

        /// <summary>
        /// Dynamic the module registration.
        /// </summary>
        public static void DynamicModuleRegistration()
        {
            if (!isStarting)
            {
                isStarting = true;
                DynamicModuleUtility.RegisterModule(typeof(SuppressFormsAuthenticationRedirectModule));
            }
        }

        public void Dispose()
        {
        }
    }
}
