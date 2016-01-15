// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetUsersCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;
using BetterCms.Module.Users.ViewModels.User;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Users.Commands.User.GetUsers
{
    public class GetUsersCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<UserItemViewModel>>
    {
        public SearchableGridViewModel<UserItemViewModel> Execute(SearchableGridOptions request)
        {
            request.SetDefaultSortingOptions("UserName");

            var query = Repository
                .AsQueryable<Models.User>()
                .Select(user => new UserItemViewModel
                    {
                        Id = user.Id,
                        Version = user.Version,
                        UserName = user.UserName,
                        FullName = (user.FirstName ?? string.Empty) 
                            + (user.FirstName != null && user.LastName != null ? " " : string.Empty) 
                            + (user.LastName ?? string.Empty),
                        Email = user.Email
                    });

            // Search
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(user => user.UserName.Contains(request.SearchQuery) 
                    || user.Email.Contains(request.SearchQuery)
                    || user.FullName.Contains(request.SearchQuery));
            }

            // Total count
            var count = query.ToRowCountFutureValue();
            
            // Sorting, Paging
            query = query.AddSortingAndPaging(request);

            return new SearchableGridViewModel<UserItemViewModel>(query.ToFuture().ToList(), request, count.Value);
        }
    }
}