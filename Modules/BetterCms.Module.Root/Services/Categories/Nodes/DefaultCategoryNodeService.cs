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
        private IRepository Repository;

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
            Guid categoryId,
            int version,
            string name,
            int displayOrder,
            string macro,
            Guid parentCategoryId,
            bool isDeleted = false,
            Category parentCategory = null,
            List<Category> categories = null)
        {
            categoryUpdated = false;

            var category = categoryId.HasDefaultValue()
                ? new Category()
                : categories != null ? categories.First(c => c.Id == categoryId) : Repository.First<Category>(categoryId);

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

                if (category.Name != name)
                {
                    updated = true;
                    category.Name = name;
                }

                if (category.DisplayOrder != displayOrder)
                {
                    updated = true;
                    category.DisplayOrder = displayOrder;
                }

                Category newParent;
                if (parentCategory != null && !parentCategory.Id.HasDefaultValue())
                {
                    newParent = parentCategory;
                }
                else
                {
                    newParent = parentCategoryId.HasDefaultValue()
                        ? null
                        : Repository.AsProxy<Category>(parentCategoryId);
                }

                if (category.ParentCategory != newParent)
                {
                    updated = true;
                    category.ParentCategory = newParent;
                }

                if (cmsConfiguration.EnableMacros && category.Macro != macro)
                {
                    category.Macro = macro;
                    updated = true;
                }

                if (updated)
                {
                    category.Version = version;
                    Repository.Save(category);
                    categoryUpdated = true;
                }
            }

            return category;
        }
    }
}