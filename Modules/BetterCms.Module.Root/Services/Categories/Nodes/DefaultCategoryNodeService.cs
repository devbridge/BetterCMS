// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultCategoryNodeService.cs" company="Devbridge Group LLC">
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.Accessors;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

namespace BetterCms.Module.Root.Services.Categories.Nodes
{
    public class DefaultCategoryNodeService : ICategoryNodeService
    {
        private readonly IRepository Repository;

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly ISessionFactoryProvider sessionFactoryProvider;

        private readonly IUnitOfWork unitOfWork;

        public DefaultCategoryNodeService(IRepository repository, ICmsConfiguration cmsConfiguration, ISessionFactoryProvider sessionFactoryProvider, IUnitOfWork unitOfWork)
        {
            Repository = repository;
            this.cmsConfiguration = cmsConfiguration;
            this.sessionFactoryProvider = sessionFactoryProvider;
            this.unitOfWork = unitOfWork;
        }

        public Category SaveCategory(
            out bool categoryUpdated,
            CategoryTree categoryTree,
            CategoryNodeModel categoryNodeModel,
            Category parentCategory,
            IEnumerable<Category> categories = null)
        {
            categoryUpdated = false;

            Category category = null;
            if (categoryNodeModel.Id.HasDefaultValue())
            {
                category = new Category();
            }
            else
            {
                if (categories != null)
                {
                    category = categories.FirstOrDefault(c => c.Id == categoryNodeModel.Id);
                }
                if (category == null)
                {
                    category = Repository.First<Category>(categoryNodeModel.Id);
                }
            }

            var updated = false;
            if (category.CategoryTree == null)
            {
                category.CategoryTree = categoryTree;
            }

            if (category.Name != categoryNodeModel.Title)
            {
                updated = true;
                category.Name = categoryNodeModel.Title;
            }

            if (category.DisplayOrder != categoryNodeModel.DisplayOrder)
            {
                updated = true;
                category.DisplayOrder = categoryNodeModel.DisplayOrder;
            }

            if (category.ParentCategory != parentCategory)
            {
                updated = true;
                category.ParentCategory = parentCategory;
            }

            if (cmsConfiguration.EnableMacros && category.Macro != categoryNodeModel.Macro)
            {
                category.Macro = categoryNodeModel.Macro;
                updated = true;
            }

            if (updated)
            {
                category.Version = categoryNodeModel.Version;
                Repository.Save(category);
                categoryUpdated = true;
            }

            return category;
        }

        public void DeleteRelations(ICategory category)
        {
            var queries = new List<IEnumerable<IEntityCategory>>();
            foreach (var categoryAccessor in CategoryAccessors.Accessors)
            {
                queries.Add(categoryAccessor.QueryEntityCategories(Repository, category));
            }

            foreach (var enumerable in queries)
            {
                var widgetRelations = enumerable as IList<IEntityCategory> ?? enumerable.ToList();
                foreach (var widgetRelation in widgetRelations)
                {
                    Repository.Delete(widgetRelation);
                }
            }


        }
    }
}