using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Controllers;

using BetterModules.Core.Web.Modules;
using BetterModules.Core.Web.Mvc.Extensions;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// Security module descriptor.
    /// </summary>
    public class SecurityJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityJsModuleIncludeDescriptor"/> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="controllerExtensions">Controller extensions</param>
        public SecurityJsModuleIncludeDescriptor(RootModuleDescriptor module, ILoggerFactory loggerFactory, IControllerExtensions controllerExtensions)
            : base(module, "bcms.security")
        {
            Links = new IActionUrlProjection[]
                {                       
                    new JavaScriptModuleLinkTo<AuthenticationController>(this, "isAuthorized", c => c.IsAuthorized("{0}"), loggerFactory, controllerExtensions),
                    new JavaScriptModuleLinkTo<AuthenticationController>(this, "usersSuggestionServiceUrl", c => c.SuggestUsers(null), loggerFactory, controllerExtensions),
                    new JavaScriptModuleLinkTo<AuthenticationController>(this, "rolesSuggestionServiceUrl", c => c.SuggestRoles(null), loggerFactory, controllerExtensions),
                };

            Globalization = new IActionProjection[]
                {
                };
        }
    }
}