using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Root.Services.Categories.Nodes
{
    public class DefaultCategoryNodeService : ICategoryNodeService
    {
        private readonly IRepository Repository;

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly IUnitOfWork unitOfWork;

        public DefaultCategoryNodeService(IRepository repository, IUnitOfWork unitOfWork, ICmsConfiguration cmsConfiguration)
        {
            Repository = repository;
            this.unitOfWork = unitOfWork;
            this.cmsConfiguration = cmsConfiguration;
        }

        public Category SaveCategory(
            out bool categoryUpdated,
            CategoryTree categoryTree,
            CategoryNodeModel categoryNode,
            bool isDeleted,
            Category parentCategory,
            IEnumerable<Category> categories = null)
        {
            categoryUpdated = false;

            var category = categoryNode.Id.HasDefaultValue()
                ? new Category()
                : categories != null ? categories.First(c => c.Id == categoryNode.Id) : Repository.First<Category>(categoryNode.Id);

            if (isDeleted && !category.Id.HasDefaultValue())
            {
                Repository.Delete(category);
                categoryUpdated = true;
            }
            else
            {
                var updated = false;
                if (category.CategoryTree == null)
                {
                    category.CategoryTree = categoryTree;
                }

                if (category.Name != categoryNode.Title)
                {
                    updated = true;
                    category.Name = categoryNode.Title;
                }

                if (category.DisplayOrder != categoryNode.DisplayOrder)
                {
                    updated = true;
                    category.DisplayOrder = categoryNode.DisplayOrder;
                }

                Category newParent;
                if (parentCategory != null && !parentCategory.Id.HasDefaultValue())
                {
                    newParent = parentCategory;
                }
                else
                {
                    newParent = categoryNode.ParentId.HasDefaultValue()
                        ? null
                        : Repository.AsProxy<Category>(categoryNode.ParentId);
                }

                if (category.ParentCategory != newParent)
                {
                    updated = true;
                    category.ParentCategory = newParent;
                }

                if (cmsConfiguration.EnableMacros && category.Macro != categoryNode.Macro)
                {
                    category.Macro = categoryNode.Macro;
                    updated = true;
                }

                if (updated)
                {
                    category.Version = categoryNode.Version;
                    Repository.Save(category);
                    categoryUpdated = true;
                }
            }

            return category;
        }
    }
}