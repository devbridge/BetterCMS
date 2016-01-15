// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditPagePropertiesButtonProjection.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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