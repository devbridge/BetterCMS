using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.Models;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Services
{
    public interface ICategoryService
    {
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

        IEnumerable<Guid> GetChildCategoriesIds(Guid category);

        IEnumerable<Guid> GetCategoriesIds(IEnumerable<string> categoriesNames);
    }
}