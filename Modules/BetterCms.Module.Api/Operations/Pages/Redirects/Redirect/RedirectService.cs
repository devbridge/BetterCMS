// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedirectService.cs" company="Devbridge Group LLC">
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
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.Api.Extensions;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    public class RedirectService : Service, IRedirectService
    {
        private readonly IRepository repository;

        private readonly Module.Pages.Services.IRedirectService redirectService;

        public RedirectService(IRepository repository, Module.Pages.Services.IRedirectService redirectService)
        {
            this.repository = repository;
            this.redirectService = redirectService;
        }

        public GetRedirectResponse Get(GetRedirectRequest request)
        {
            var model = repository
                .AsQueryable<Module.Pages.Models.Redirect>(redirect => redirect.Id == request.RedirectId)
                .Select(redirect => new RedirectModel
                    {
                        Id = redirect.Id,
                        Version = redirect.Version,
                        CreatedBy = redirect.CreatedByUser,
                        CreatedOn = redirect.CreatedOn,
                        LastModifiedBy = redirect.ModifiedByUser,
                        LastModifiedOn = redirect.ModifiedOn,

                        PageUrl = redirect.PageUrl,
                        RedirectUrl = redirect.RedirectUrl,
                    })
                .FirstOne();

            return new GetRedirectResponse
                       {
                           Data = model
                       };
        }

        public PutRedirectResponse Put(PutRedirectRequest request)
        {
            var model = request.Data.ToServiceModel();
            if (request.Id.HasValue)
            {
                model.Id = request.Id.Value;
            }

            var redirect = redirectService.SaveRedirect(model, true);

            return new PutRedirectResponse { Data = redirect.Id };
        }

        public DeleteRedirectResponse Delete(DeleteRedirectRequest request)
        {
            var result = redirectService.DeleteRedirect(request.Id, request.Data.Version);

            return new DeleteRedirectResponse { Data = result };
        }
    }
}