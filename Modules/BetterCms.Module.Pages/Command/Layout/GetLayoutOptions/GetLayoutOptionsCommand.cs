// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLayoutOptionsCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Option;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Layout.GetLayoutOptions
{
    public class GetLayoutOptionsCommand : CommandBase, ICommand<GetLayoutOptionsCommandRequest, IList<OptionValueEditViewModel>>
    {
        private readonly ILayoutService layoutService;
        
        private readonly IMasterPageService masterPageService;

        public GetLayoutOptionsCommand(ILayoutService layoutService, IMasterPageService masterPageService)
        {
            this.layoutService = layoutService;
            this.masterPageService = masterPageService;
        }

        public IList<OptionValueEditViewModel> Execute(GetLayoutOptionsCommandRequest request)
        {
            if (request.IsMasterPage)
            {
                return masterPageService.GetMasterPageOptionValues(request.Id);
            }

            return layoutService.GetLayoutOptionValues(request.Id);
        }
    }
}