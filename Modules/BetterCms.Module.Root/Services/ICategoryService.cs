using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Root.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// Gets the list of category lookup values.
        /// </summary>
        /// <returns>List of category lookup values.</returns>
        IEnumerable<LookupKeyValue> GetCategories(string categoryTreeForKey);

        IEnumerable<Guid> GetSelectedCategoriesIds<TEntity, TEntityCategory>(Guid? entityId)
            where TEntity : Entity, ICategorized
            where TEntityCategory : Entity, IEntityCategory;

        IEnumerable<LookupKeyValue> GetSelectedCategories<TEntity, TEntityCategory>(Guid? entityId)
            where TEntity : Entity, ICategorized
            where TEntityCategory : Entity, IEntityCategory;

        void CombineEntityCategories<TEntity, TEntityCategory>(TEntity entity, IEnumerable<LookupKeyValue> currentCategories)
            where TEntity : Entity, ICategorized
            where TEntityCategory : Entity, IEntityCategory, new();

        void CombineEntityCategories<TEntity, TEntityCategory>(TEntity entity, IEnumerable<Guid> currentCategories)
            where TEntity : Entity, ICategorized
            where TEntityCategory : Entity, IEntityCategory, new();

        void DeleteCategoryNode(Guid id, int version, Guid? categoryTreeId = null);
    }
}