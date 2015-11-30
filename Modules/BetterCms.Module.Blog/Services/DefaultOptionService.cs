// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultOptionService.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Blog.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Blog.Services
{
    internal class DefaultOptionService : IOptionService
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultOptionService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultOptionService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the default template id.
        /// </summary>
        /// <returns>
        /// Default template id or null, if such is not set
        /// </returns>
        public Option GetDefaultOption()
        {
            return repository.AsQueryable<Option>()
                .Fetch(option => option.DefaultLayout)

// In not supported by NHibernate - too deep.
//                .FetchMany(option => option.DefaultLayout.LayoutRegions)
//                .ThenFetch(region => region.Region)

                .Fetch(option => option.DefaultMasterPage)
                .ThenFetchMany(master => master.AccessRules)
                .Distinct()
                .ToList()
                .FirstOrDefault();
        }
    }
}