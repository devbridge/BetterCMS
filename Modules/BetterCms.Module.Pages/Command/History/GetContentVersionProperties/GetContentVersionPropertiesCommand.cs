// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetContentVersionPropertiesCommand.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Projections;

using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.History.GetContentVersionProperties
{
    /// <summary>
    /// Command for getting page content version.
    /// </summary>
    public class GetContentVersionPropertiesCommand : CommandBase, ICommand<Guid, PropertiesPreview>
    {
        private readonly PageContentProjectionFactory projectionFactory;

        public GetContentVersionPropertiesCommand(PageContentProjectionFactory projectionFactory)
        {
            this.projectionFactory = projectionFactory;
        }

        public PropertiesPreview Execute(Guid contentId)
        {
            var content = Repository
                .AsQueryable<Root.Models.Content>(c => c.Id == contentId)
                .FirstOrDefault();

            var accessor = projectionFactory.GetAccessorForType(content);
            return accessor.GetHtmlPropertiesPreview();
        }
    }
}