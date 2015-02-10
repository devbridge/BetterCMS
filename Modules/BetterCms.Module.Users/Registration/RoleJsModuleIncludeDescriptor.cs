using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.Controllers;

namespace BetterCms.Module.Users.Registration
{
    /// <summary>
    /// bcms.role.js module descriptor.
    /// </summary>
    public class RoleJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public RoleJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.role")
        {
            Links = new IActionProjection[]
                        {
                            new JavaScriptModuleLinkTo<RoleController>(this, "saveRoleUrl", c => c.SaveRole(null)),
                            new JavaScriptModuleLinkTo<RoleController>(this, "deleteRoleUrl", c=> c.DeleteRole("{0}", "{1}")),
                            new JavaScriptModuleLinkTo<RoleController>(this, "loadSiteSettingsRoleUrl", c => c.ListTemplate()),
                            new JavaScriptModuleLinkTo<RoleController>(this, "loadRolesUrl", c => c.RolesList(null)),
                            new JavaScriptModuleLinkTo<RoleController>(this, "roleSuggestionServiceUrl", c=> c.SuggestRoles(null))
                        };

            Globalization = new IActionProjection[]
                        {
                            new JavaScriptModuleGlobalization(this, "rolesListTabTitle", () => UsersGlobalization.SiteSettings_Roles_ListTab_Title),
                            new JavaScriptModuleGlobalization(this, "deleteRoleConfirmMessage" , ()=> UsersGlobalization.DeleteRole_Confirmation_Message), 
                        };
        }
    }
}