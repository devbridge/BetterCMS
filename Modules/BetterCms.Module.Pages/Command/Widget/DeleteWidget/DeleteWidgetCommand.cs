// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteWidgetCommand.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Widget.DeleteWidget
{
    /// <summary>
    /// Deletes widget.
    /// </summary>
    public class DeleteWidgetCommand : CommandBase, ICommand<DeleteWidgetRequest, bool>
    {
        private readonly IWidgetService widgetService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteWidgetCommand" /> class.
        /// </summary>
        /// <param name="widgetService">The widget service.</param>
        public DeleteWidgetCommand(IWidgetService widgetService)
        {
            this.widgetService = widgetService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public bool Execute(DeleteWidgetRequest request)
        {
            return widgetService.DeleteWidget(request.WidgetId, request.Version);
        }
    }
}