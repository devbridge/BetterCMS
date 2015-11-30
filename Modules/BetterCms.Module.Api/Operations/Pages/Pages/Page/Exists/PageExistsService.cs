// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageExistsService.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists
{
    public class PageExistsService : Service, IPageExistsService
    {
        private readonly IRepository repository;

        private readonly IUrlService urlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageExistsService" /> class.
        /// </summary>
        public PageExistsService(IRepository repository, IUrlService urlService)
        {
            this.repository = repository;
            this.urlService = urlService;
        }

        public PageExistsResponse Get(PageExistsRequest request)
        {
            var url = urlService.FixUrl(request.PageUrl);

            var id = repository
                .AsQueryable<Module.Root.Models.Page>(p => p.PageUrlHash == url.UrlHash())
                .Select(p => p.Id)
                .FirstOrDefault();

            return new PageExistsResponse
                       {
                           Data = new PageModel
                                      {
                                          Exists = !id.HasDefaultValue(),
                                          PageId = !id.HasDefaultValue() ? id : (System.Guid?)null
                                      }
                       };
        }
    }
}