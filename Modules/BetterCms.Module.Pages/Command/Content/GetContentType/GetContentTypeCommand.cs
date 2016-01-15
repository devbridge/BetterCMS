// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetContentTypeCommand.cs" company="Devbridge Group LLC">
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

using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Content.GetContentType
{
    public class GetContentTypeCommand : CommandBase, ICommand<Guid, GetContentTypeCommandResponse>
    {
        public GetContentTypeCommandResponse Execute(Guid request)
        {
            var result =  Repository
                .AsQueryable<Root.Models.Content>(w => w.Id == request)
                .Select(w => new
                             {
                                 Id = w.Id,
                                 Type = w.GetType()
                             })
                 .FirstOne();

            var response = new GetContentTypeCommandResponse { Id = result.Id };
            if (typeof(ServerControlWidget).IsAssignableFrom(result.Type))
            {
                response.Type = WidgetType.ServerControl.ToString();
            }
            else if (typeof(HtmlContentWidget).IsAssignableFrom(result.Type))
            {
                response.Type = WidgetType.HtmlContent.ToString();
            }

            return response;
        }
    }
}