using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.Controllers;

namespace BetterCms.Module.Users.Registration
{
    /// <summary>
    /// bcms.role.js module descriptor.
    /// </summary>
    public class RoleJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public RoleJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.role", "/file/bcms-users/scripts/bcms.role")
        {
            Links = new IActionProjection[]
                        {
                            new JavaScriptModuleLinkTo<RoleController>(this, "loadCreatRoleUrl", c => c.CreatRoleView()),
                            new JavaScriptModuleLinkTo<RoleController>(this, "loadEditRoleUrl", c => c.EditRoleView("{0}")),
                            new JavaScriptModuleLinkTo<RoleController>(this, "loadSiteSettingsRoleUrl", c => c.RolesListView(null)) 
                        };

            Globalization = new IActionProjection[]
                        {
                            new JavaScriptModuleGlobalization(this, "rolesListTabTitle", () => UsersGlobalization.SiteSettings_Roles_ListTab_Title),
                            new JavaScriptModuleGlobalization(this, "rolesAddNewTitle", () => UsersGlobalization.Role_AddNeww_Dialog_Title) 
                        };
        }
    }
}