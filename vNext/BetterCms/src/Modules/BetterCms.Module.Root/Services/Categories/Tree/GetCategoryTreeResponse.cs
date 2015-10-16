using System.Collections.Generic;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Root.Services.Categories.Tree
{
    public class GetCategoryTreeResponse
    {
        public IEnumerable<CategoryTree> Items;

        public int TotalCount;
    }
}