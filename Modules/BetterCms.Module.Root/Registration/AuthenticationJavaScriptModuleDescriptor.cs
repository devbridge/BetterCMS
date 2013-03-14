using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// bcms.authentication.js module descriptor.
    /// </summary>
    public class AuthenticationScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public AuthenticationScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.authentication")
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