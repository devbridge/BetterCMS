using System.Collections.Generic;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// Gets the list of category lookup values.
        /// </summary>
        /// <returns>List of category lookup values.</returns>
        IEnumerable<LookupKeyValue> GetCategories();
    }
}