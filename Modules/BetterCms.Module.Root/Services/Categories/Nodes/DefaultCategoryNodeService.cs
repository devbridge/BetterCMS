using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess;

namespace BetterCms.Module.Root.Services.Categories.Nodes
{
    public class DefaultCategoryNodeService : ICategoryNodeService
    {
        private readonly IRepository Repository;

        private readonly ICmsConfiguration cmsConfiguration;

        public DefaultCategoryNodeService(IRepository repository, ICmsConfiguration cmsConfiguration)
        {
            Repository = repository;
            this.cmsConfiguration = cmsConfiguration;
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
    }
}