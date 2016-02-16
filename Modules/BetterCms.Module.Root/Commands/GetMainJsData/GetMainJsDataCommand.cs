// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetMainJsDataCommand.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Rendering;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.GetMainJsData
{
    public class GetMainJsDataCommand : CommandBase, ICommandOut<RenderMainJsViewModel>
    {
        /// <summary>
        /// The rendering service.
        /// </summary>
        private readonly IRenderingService renderingService;

        /// <summary>
        /// The CMS configuration.
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetMainJsDataCommand" /> class.
        /// </summary>
        /// <param name="renderingService">The rendering service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public GetMainJsDataCommand(IRenderingService renderingService, ICmsConfiguration cmsConfiguration)
        {
            this.renderingService = renderingService;
            this.cmsConfiguration = cmsConfiguration;
        }

        public RenderMainJsViewModel Execute()
        {
            var model = new RenderMainJsViewModel();
            model.JavaScriptModules = renderingService.GetJavaScriptIncludes();
            model.Version = cmsConfiguration.Version;
            model.UseMinReferences = cmsConfiguration.UseMinifiedResources;

#if (DEBUG)
            model.IsDebugMode = true;
#endif
            return model;
        }
    }
}