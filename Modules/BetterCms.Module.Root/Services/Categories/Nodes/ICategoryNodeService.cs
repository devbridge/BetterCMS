using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Root.Services.Categories.Nodes
{
    public interface ICategoryNodeService
    {
        Category SaveCategory(
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
            List<Category> categories = null);

    }
}