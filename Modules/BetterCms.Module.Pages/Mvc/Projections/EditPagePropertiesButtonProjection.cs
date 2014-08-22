using System;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Services;
using BetterCms.Module.Root;

namespace BetterCms.Module.Pages.Mvc.Projections
{
    public class EditPagePropertiesButtonProjection : ButtonActionProjection
    {
        public EditPagePropertiesButtonProjection(JsIncludeDescriptor parentModuleInclude, Func<IPage, string> onClickAction)
            : base(parentModuleInclude, onClickAction)
        {
        }

        public EditPagePropertiesButtonProjection(JsIncludeDescriptor parentModuleInclude, Func<IPage, string> title, Func<IPage, string> onClickAction)
            : base(parentModuleInclude, title, onClickAction)
        {
        }

        public override bool Render(IPage page, ISecurityService securityService, HtmlHelper html)
        {
            if (page.IsMasterPage)
            {
                AccessRole = RootModuleConstants.UserRoles.MultipleRoles(RootModuleConstants.UserRoles.Administration);
            }
            else
            {
                AccessRole = RootModuleConstants.UserRoles.MultipleRoles(
                    RootModuleConstants.UserRoles.EditContent,
                    RootModuleConstants.UserRoles.PublishContent);
            }

            return base.Render(page, securityService, html);
        }
    }
}