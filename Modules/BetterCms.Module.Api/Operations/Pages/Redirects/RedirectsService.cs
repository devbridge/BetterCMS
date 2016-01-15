// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedirectsService.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Redirects.Redirect;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Redirects
{
    public class RedirectsService : Service, IRedirectsService
    {
        private readonly IRepository repository;
        
        private readonly IRedirectService redirectService;

        public RedirectsService(IRepository repository, IRedirectService redirectService)
        {
            this.repository = repository;
            this.redirectService = redirectService;
        }

        public GetRedirectsResponse Get(GetRedirectsRequest request)
        {
            request.Data.SetDefaultOrder("PageUrl");

            var listResponse = repository
                .AsQueryable<Module.Pages.Models.Redirect>()
                .Select(redirect => new RedirectModel()
                    {
                        Id = redirect.Id,
                        Version = redirect.Version,
                        CreatedBy = redirect.CreatedByUser,
                        CreatedOn = redirect.CreatedOn,
                        LastModifiedBy = redirect.ModifiedByUser,
                        LastModifiedOn = redirect.ModifiedOn,

                        PageUrl = redirect.PageUrl,
                        RedirectUrl = redirect.RedirectUrl
                    })
                .ToDataListResponse(request);

            return new GetRedirectsResponse
                       {
                           Data = listResponse
                       };
        }

        public PostRedirectResponse Post(PostRedirectRequest request)
        {
            var result = redirectService.Put(new PutRedirectRequest
                {
                    Data = request.Data,
                    User = request.User
                });

            return new PostRedirectResponse { Data = result.Data };
        }
    }
}