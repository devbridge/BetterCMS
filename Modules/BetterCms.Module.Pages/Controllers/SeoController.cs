// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeoController.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Pages.Command.Page.GetPageSeo;
using BetterCms.Module.Pages.Command.Page.SavePageSeo;
using BetterCms.Module.Pages.ViewModels.Seo;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Pages.Controllers
{
    /// <summary>
    /// Defines logic to handle page SEO information.
    /// </summary>
    [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
    [ActionLinkArea(PagesModuleDescriptor.PagesAreaName)]
    public class SeoController : CmsControllerBase
    {
        /// <summary>
        /// Creates edit SEO modal dialog for given page.
        /// </summary>
        /// <param name="pageId">A page id.</param>
        /// <returns>
        /// ViewResult to render edit SEO modal dialog.
        /// </returns>
        [HttpGet]        
        public ActionResult EditSeo(string pageId)
        {
            EditSeoViewModel model = GetCommand<GetPageSeoCommand>().ExecuteCommand(pageId.ToGuidOrDefault());
            var view = RenderView("EditSeo", model);
            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validates and saves page SEO information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json with result status.</returns>
        [HttpPost]
        public ActionResult EditSeo(EditSeoViewModel model)
        {
            bool success = false;            
            if (ModelState.IsValid)
            {
                model = GetCommand<SavePageSeoCommand>().ExecuteCommand(model);
                success = model != null;
            }

            return WireJson(success, model);
        }
    }
}
