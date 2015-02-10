using System.Web.Routing;

using Devbridge.Platform.Core.Modules.Registration;

namespace Devbridge.Platform.Core.Web.Modules.Registration
{
    /// <summary>
    /// Defines the contract for modules registration logic.
    /// </summary>
    public interface IWebModulesRegistration : IModulesRegistration
    {
        /// <summary>
        /// Finds the module by area name.
        /// </summary>
        /// <param name="areaName">Name of the area.</param>
        /// <returns>Known module instance.</returns>
        WebModuleDescriptor FindModuleByAreaName(string areaName);

        /// <summary>
        /// Determines whether module by area name is registered.
        /// </summary>
        /// <param name="areaName">Name of the area.</param>
        /// <returns>
        /// <c>true</c> if module by area name is registered; otherwise, <c>false</c>.
        /// </returns>
        bool IsModuleRegisteredByAreaName(string areaName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        void RegisterKnownModuleRoutes(RouteCollection routes);
    }
}
