// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WidgetCategoryAccessor.cs" company="Devbridge Group LLC">
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
using BetterCms.Core.DataContracts.Enums;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using NHibernate;
using NHibernate.Linq;

namespace BetterCms.Module.Root.Models.Accessors
{
    public class WidgetCategoryAccessor : ICategoryAccessor
    {
        public string Name
        {
            get
            {
                return Widget.CategorizableItemKeyForWidgets;
            } 
        }

        public IFutureValue<int> CheckIsUsed(IRepository repository, CategoryTree categoryTree)
        {
            var query = repository.AsQueryable<WidgetCategory>().Where(ec => ec.Category.CategoryTree == categoryTree
            && !ec.Widget.IsDeleted
            && (ec.Widget.Status == ContentStatus.Draft || ec.Widget.Status == ContentStatus.Published)).ToRowCountFutureValue();

            return query;
        }

        public IEnumerable<IEntityCategory> QueryEntityCategories(IRepository repository, ICategory category)
        {
            return repository.AsQueryable<WidgetCategory>().Where(wc => wc.Category.Id == category.Id).ToFuture();
        }
    }
}