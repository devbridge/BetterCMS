using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;

using BetterModules.Core.Web.Modules;

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
        public AuthenticationJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.authentication")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<AuthenticationController>(this, "logoutUrl", c => c.Logout())
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "confirmLogoutMessage", () => RootGlobalization.Authentication_LogOutConfirmationMessage)
                };
        }
    }
}