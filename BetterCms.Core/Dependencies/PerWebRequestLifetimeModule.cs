using System.Web;

using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace BetterCms.Core.Dependencies
{
    /// <summary>
    /// Per web request lifetime http module.
    /// </summary>
    public class PerWebRequestLifetimeModule : IHttpModule
    {
        /// <summary>
        /// Indicates if module is starting.
        /// </summary>
        private static bool isStarting;

        /// <summary>
        /// Dynamic the module registration.
        /// </summary>
        public static void DynamicModuleRegistration()
        {
            if (!isStarting)
            {
                isStarting = true;
                DynamicModuleUtility.RegisterModule(typeof(PerWebRequestLifetimeModule));
            }
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule" />.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication" /> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            context.EndRequest += (sender, e) =>
            {
                PerWebRequestContainerProvider.DisposeCurrentScope(sender, e);
            };
        }
    }
}