using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;

using BetterModules.Core.Web.Mvc.Extensions;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// bcms.authentication.js module descriptor.
    /// </summary>
    public class AuthenticationJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="controllerExtensions">Controller extensions</param>
        public AuthenticationJsModuleIncludeDescriptor(RootModuleDescriptor module, ILoggerFactory loggerFactory, IControllerExtensions controllerExtensions)
            : base(module, "bcms.authentication")
        {
            Links = new IActionUrlProjection[]
                {
                    new JavaScriptModuleLinkTo<AuthenticationController>(this, "logoutUrl", c => c.Logout(), loggerFactory, controllerExtensions)
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "confirmLogoutMessage", () => RootGlobalization.Authentication_LogOutConfirmationMessage, loggerFactory)
                };
        }
    }
}