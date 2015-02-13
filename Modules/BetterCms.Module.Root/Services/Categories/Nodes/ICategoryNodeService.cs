using System.Collections.Generic;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Root.Services.Categories.Nodes
{
    public interface ICategoryNodeService
    {
        Category SaveCategory(
            out bool categoryUpdated,
            CategoryTree categoryTree,
            CategoryNodeModel categoryNodeModel,
            Category parentCategory,
            IEnumerable<Category> categories = null);
    }
}