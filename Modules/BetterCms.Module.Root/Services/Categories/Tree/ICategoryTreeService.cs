using System.Collections.Generic;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Root.Services.Categories.Tree
{
    public interface ICategoryTreeService
    {
        GetCategoryTreeResponse Get(GetCategoryTreeRequest request);

        CategoryTree Save(SaveCategoryTreeRequest request);

        bool Delete(DeleteCategoryTreeRequest request);
    }
}
