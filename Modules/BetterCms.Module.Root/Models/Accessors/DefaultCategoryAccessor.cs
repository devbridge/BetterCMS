// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultCategoryAccessor.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Core.DataContracts;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using NHibernate;
using NHibernate.Linq;

namespace BetterCms.Module.Root.Models.Accessors
{
    public class DefaultCategoryAccessor<T> : ICategoryAccessor
        where T : class, IEntityCategory
    {
        private readonly string name;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public DefaultCategoryAccessor(string name)
        {
            this.name = name;
        } 
        public virtual IFutureValue<int> CheckIsUsed(IRepository repository, CategoryTree categoryTree)
        {
            var query = repository.AsQueryable<T>().Where(ec => (CategoryTree)ec.Category.CategoryTree == categoryTree).ToRowCountFutureValue();
            return query;
        }

        public IEnumerable<IEntityCategory> QueryEntityCategories(IRepository repository, ICategory category)
        {
            return repository.AsQueryable<T>().Where(c => c.Category.Id == category.Id).ToFuture();
        }
    }
}