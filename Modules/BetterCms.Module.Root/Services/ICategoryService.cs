using BetterCms.Module.Root.Models;
using System;
using System.Collections.Generic;

namespace BetterCms.Module.Root.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// Gets the list of category lookup values.
        /// </summary>
        /// <returns>List of category lookup values.</returns>
        IEnumerable<LookupKeyValue> GetCategories();
        
        /// <summary>
        /// Saves the category.
        /// </summary>
        /// <param name="categoryUpdated">true if category updated.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="version">The version.</param>
        /// <param name="name">The name.</param>
        /// <param name="displayOrder">The display order.</param>
        /// <param name="macro">The macro.</param>
        /// <param name="parentCategoryId">The parent category id.</param>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="parentCategory">The parent category.</param>
        /// <param name="categories">List of categories.</param>
        /// <returns>
        /// Updated or newly created category.
        /// </returns>
        Category SaveCategory(out bool categoryUpdated, Guid categoryId, int version, string name, int displayOrder, string macro, Guid parentCategoryId, bool isDeleted = false, Category parentCategory = null, List<Category> categories = null);
    }
}