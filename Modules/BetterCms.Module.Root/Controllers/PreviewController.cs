// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PreviewController.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Core.Mvc.Attributes;
using BetterCms.Core.Security;

using BetterCms.Module.Root.Commands.GetPageToRender;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// Preview controller.
    /// </summary>
    [BcmsAuthorize(RootModuleConstants.UserRoles.EditContent)]
    [ActionLinkArea(RootModuleDescriptor.RootAreaName)]
    public class PreviewController : CmsControllerBase
    {
        /// <summary>
        /// The CMS configuration.
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewController"/> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public PreviewController(ICmsConfiguration cmsConfiguration)
        {
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Previews the specified page id.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="pageContentId">The page content id.</param>
        /// <returns>
        /// Returns an action result to render a page preview.
        /// </returns>
        [IgnoreAutoRoute]
        public ActionResult Index(string pageId, string pageContentId)
        {
            var principal = SecurityService.GetCurrentPrincipal();

            var allRoles = new List<string>(RootModuleConstants.UserRoles.AllRoles);
            if (!string.IsNullOrEmpty(cmsConfiguration.Security.FullAccessRoles))
            {
                allRoles.Add(cmsConfiguration.Security.FullAccessRoles);
            }

            var request = new GetPageToRenderRequest
                {
                    PageId = pageId.ToGuidOrDefault(),
                    PreviewPageContentId = pageContentId.ToGuidOrDefault(),
                    IsPreview = true,
                    HasContentAccess = SecurityService.IsAuthorized(principal, RootModuleConstants.UserRoles.MultipleRoles(allRoles.ToArray()))
                };

            var model = GetCommand<GetPageToRenderCommand>().ExecuteCommand(request);

            if (model != null && model.RenderPage != null)
            {
                // Render page with hierarchical master pages
                var html = this.RenderPageToString(model.RenderPage);
                html = PageHtmlRenderer.ReplaceRegionRepresentationHtml(html, string.Empty);

                return Content(html);
            }

            return HttpNotFound();
        }
    }
}