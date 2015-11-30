// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionController.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using BetterCms.Core.Security;

using BetterCms.Module.Blog.Commands.GetBlogSettings;
using BetterCms.Module.Blog.Commands.GetTemplates;
using BetterCms.Module.Blog.Commands.SaveBlogPostSetting;
using BetterCms.Module.Blog.Commands.SaveDefaultTemplate;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Blog.ViewModels.Setting;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Blog.Controllers
{
    /// <summary>
    /// Manage blogs options.
    /// </summary>
    [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
    [ActionLinkArea(BlogModuleDescriptor.BlogAreaName)]
    public class OptionController : CmsControllerBase
    {
        /// <summary>
        /// Gets blogs settings list.
        /// </summary>
        /// <returns>Blogs settings list.</returns>
        public ActionResult Settings()
        {
            var templates = GetCommand<GetBlogSettingsCommand>().ExecuteCommand(true);
            var view = RenderView("Settings", null);
            
            return ComboWireJson(templates != null, view, templates, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets templates list.
        /// </summary>
        /// <returns>Template list.</returns>
        public ActionResult Templates()
        {
            var templates = GetCommand<GetTemplatesCommand>().ExecuteCommand(true);
            var view = RenderView("Templates");

            return ComboWireJson(templates != null, view, templates, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the default template.
        /// </summary>
        /// <param name="templateId">The id.</param>
        /// <param name="masterPageId">The master page identifier.</param>
        /// <returns>
        /// Json result
        /// </returns>
        [HttpPost]
        public ActionResult SaveDefaultTemplate(string templateId, string masterPageId)
        {
            var request = new DefaultTemplateViewModel { TemplateId = templateId.ToGuidOrDefault(), MasterPageId = masterPageId.ToGuidOrDefault() };

            var response = GetCommand<SaveDefaultTemplateCommand>().ExecuteCommand(request);

            return WireJson(response);
        }
        
        /// <summary>
        /// Saves the setting value.
        /// </summary>
        /// <returns>
        /// Json result
        /// </returns>
        [HttpPost]
        public ActionResult SaveSetting(SettingItemViewModel request)
        {
            var response = GetCommand<SaveBlogPostSettingCommand>().ExecuteCommand(request);

            return WireJson(request != null, response);
        }
    }
}