using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Controllers;

using BetterModules.Core.Web.Modules;

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
        public SecurityJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.security")
        {
            Links = new IActionProjection[]
                {                       
                    new JavaScriptModuleLinkTo<AuthenticationController>(this, "isAuthorized", c => c.IsAuthorized("{0}")),
                    new JavaScriptModuleLinkTo<AuthenticationController>(this, "usersSuggestionServiceUrl", c => c.SuggestUsers(null)),
                    new JavaScriptModuleLinkTo<AuthenticationController>(this, "rolesSuggestionServiceUrl", c => c.SuggestRoles(null)),
                };

            Globalization = new IActionProjection[]
                {
                };
        }
    }
}