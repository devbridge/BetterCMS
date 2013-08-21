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
        public RoleJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.role")
        {
            Links = new IActionProjection[]
                        {
                            new JavaScriptModuleLinkTo<RoleController>(this, "loadCreateRoleUrl", c => c.CreateRoleView()),
                            new JavaScriptModuleLinkTo<RoleController>(this, "loadEditRoleUrl", c => c.EditRoleView("{0}")),
                            new JavaScriptModuleLinkTo<RoleController>(this, "loadSiteSettingsRoleUrl", c => c.RolesListView(null)),
                            new JavaScriptModuleLinkTo<RoleController>(this, "deleteRoleUrl", c=> c.DeleteRole("{0}", "{1}")),
                            new JavaScriptModuleLinkTo<RoleController>(this, "roleSuggestionSeviceUrl", c=> c.SuggestRoles("{0}"))
                        };

            Globalization = new IActionProjection[]
                        {
                            new JavaScriptModuleGlobalization(this, "rolesListTabTitle", () => UsersGlobalization.SiteSettings_Roles_ListTab_Title),
                            new JavaScriptModuleGlobalization(this, "rolesAddNewTitle", () => UsersGlobalization.Role_AddNeww_Dialog_Title),
                            new JavaScriptModuleGlobalization(this, "deleteRoleConfirmMessage" , ()=> UsersGlobalization.DeleteRole_Confirmation_Message), 
                        };
        }
    }
}