using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Services;
using BetterCms.Events;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Category;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Exceptions.DataTier;
using BetterModules.Core.Models;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;

namespace BetterCms.Module.Root.Services
{
    internal class DefaultCategoryService : ICategoryService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        private readonly ISecurityService securityService;  

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCategoryService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="securityService">Security service get information about authorization.</param>
        public DefaultCategoryService(IRepository repository, IUnitOfWork unitOfWork, ISecurityService securityService)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.securityService = securityService;
        }

        public IEnumerable<Guid> GetSelectedCategoriesIds<TEntity, TEntityCategory>(Guid? entityId) 
            where TEntity : Entity, ICategorized
            where TEntityCategory : Entity, IEntityCategory
        {
            CategoryTree categoryTreeAlias = null;
            Category categoryAliasa = null;
            TEntity entityAlias = null;
            TEntityCategory categoryAlias = default(TEntityCategory);

            return repository.AsQueryOver(() => categoryAliasa)
                             .JoinQueryOver(() => categoryAliasa.CategoryTree, () => categoryTreeAlias)
                             .Where(() => !categoryTreeAlias.IsDeleted && !categoryAliasa.IsDeleted)
                             .WithSubquery.WhereProperty(() => categoryAliasa.Id)
                             .In(QueryOver.Of(() => entityAlias)
                                     .JoinQueryOver(() => entityAlias.Categories, () => categoryAlias)
                                     .Where(() => entityAlias.Id == entityId && !categoryAlias.IsDeleted)
                                     .SelectList(list => list.Select(() => categoryAlias.Category.Id)))
                             .Select(category => category.Id)
                             .Future<Guid>();
        }

        public IEnumerable<LookupKeyValue> GetSelectedCategories<TEntity, TEntityCategory>(Guid? entityId)
            where TEntity : Entity, ICategorized
            where TEntityCategory : Entity, IEntityCategory
        {
            CategoryTree categoryTreeAlias = null;
            Category categoryAliasa = null;
            TEntity entityAlias = null;
            TEntityCategory categoryAlias = default(TEntityCategory);
            LookupKeyValue valueAlias = null;

            return repository.AsQueryOver(() => categoryAliasa)
                             .JoinQueryOver(() => categoryAliasa.CategoryTree, () => categoryTreeAlias)
                             .Where(() => !categoryTreeAlias.IsDeleted && !categoryAliasa.IsDeleted)
                             .WithSubquery.WhereProperty(() => categoryAliasa.Id)
                             .In(QueryOver.Of(() => entityAlias)
                                     .JoinQueryOver(() => entityAlias.Categories, () => categoryAlias)
                                     .Where(() => entityAlias.Id == entityId && !categoryAlias.IsDeleted)
                                     .SelectList(list => list.Select(() => categoryAlias.Category.Id)))
                             .SelectList(l => l
                                 .Select(NHibernate.Criterion.Projections.Cast(NHibernateUtil.String, NHibernate.Criterion.Projections.Property(() => categoryAliasa.Id))).WithAlias(() => valueAlias.Key)
                                 .Select(() => categoryAliasa.Name).WithAlias(() => valueAlias.Value))
                             .TransformUsing(Transformers.AliasToBean<LookupKeyValue>())
                             .Future<LookupKeyValue>();
        }

        public void CombineEntityCategories<TEntity, TEntityCategory>(TEntity entity, IEnumerable<Guid> currentCategories) 
            where TEntity : Entity, ICategorized
            where TEntityCategory : Entity, IEntityCategory, new()
        {
            var categories = currentCategories != null ? currentCategories.ToList() : new List<Guid>();

            if (entity != null)
            {
                var newCategoryIds = entity.Categories != null ? categories.Where(cId => entity.Categories.All(pc => pc.Category.Id != cId)).ToArray() : categories.ToArray();
                var newCategories = repository.AsQueryOver<Category>().WhereRestrictionOn(t => t.Id).IsIn(newCategoryIds).Future<Category>();

                if (entity.Categories != null)
                {
                    // Remove categories
                    var removedCategories = entity.Categories.Where(c => !categories.Contains(c.Category.Id)).ToList();

                    if (removedCategories.Any())
                    {
                        UpdateModifiedInformation(entity);
                    }
                    foreach (var removedCategory in removedCategories)
                    {
                        //entity.RemoveCategory(removedCategory);
                        unitOfWork.Session.Delete(removedCategory);
                    }
                }

                if (newCategories.Any())
                {
                    UpdateModifiedInformation(entity);
                }

                // Attach new categories
                foreach (var newCategory in newCategories)
                {
                    var newentityCategory = new TEntityCategory();
                    newentityCategory.Category = newCategory;
                    newentityCategory.SetEntity(entity);
                   // newentityCategory.Version = 0;
                    entity.AddCategory(newentityCategory);
                }              
            }
        }

        public void CombineEntityCategories<TEntity, TEntityCategory>(TEntity entity, IEnumerable<LookupKeyValue> currentCategories)
            where TEntity : Entity, ICategorized
            where TEntityCategory : Entity, IEntityCategory, new()
        {
            CombineEntityCategories<TEntity, TEntityCategory>(entity, currentCategories != null ? currentCategories.Select(c => Guid.Parse(c.Key)) : Enumerable.Empty<Guid>());
        }

        public void DeleteCategoryNode(Guid id, int version, Guid? categoryTreeId = null)
        {
            IList<ICategory> deletedNodes = new List<ICategory>();

            var node = repository.AsQueryable<Category>()
                .Where(sitemapNode => sitemapNode.Id == id && (!categoryTreeId.HasValue || sitemapNode.CategoryTree.Id == categoryTreeId.Value))
                .Fetch(sitemapNode => sitemapNode.CategoryTree)
                .Distinct()
                .First();

            // Concurrency.
            if (version > 0 && node.Version != version)
            {
                throw new ConcurrentDataException(node);
            }

            unitOfWork.BeginTransaction();

//            ArchiveSitemap(node.Sitemap.Id);

            DeleteCategoryNode(node, ref deletedNodes);

            unitOfWork.Commit();

            var updatedCategories = new List<ICategoryTree>();
            foreach (var deletedNode in deletedNodes)
            {
                RootEvents.Instance.OnCategoryDeleted(deletedNode);
                if (!updatedCategories.Contains(deletedNode.CategoryTree))
                {
                    updatedCategories.Add(deletedNode.CategoryTree);
                }
            }

            foreach (var sitemap in updatedCategories)
            {
                RootEvents.Instance.OnCategoryTreeUpdated(sitemap);
            }
        }

        public IEnumerable<Guid> GetChildCategoriesIds(Guid categoryId)
        {
            var allCategoryNodes = repository.AsQueryable<Category>().Where(c =>
                                                    repository.AsQueryable<Category>().Where(cat => cat.Id == categoryId && !cat.IsDeleted)
                                                    .Any(catT => catT.CategoryTree.Id == c.CategoryTree.Id && !catT.IsDeleted)
                                                    ).Select(c => new CategoryViewModel()
                                                    {
                                                        Id = c.Id,
                                                        Title = c.Name,
                                                        DisplayOrder = c.DisplayOrder,
                                                        CategoryTreeId = c.CategoryTree.Id,
                                                        ParentCategoryId = c.ParentCategory != null ? c.ParentCategory.Id : (Guid?)null
                                                    }).ToList();
            // Find given category
            var mainCategory = allCategoryNodes.First(c => c.Id == categoryId);
            List<CategoryViewModel> childCategories = new List<CategoryViewModel>() { mainCategory };

            FillChildCategories(allCategoryNodes, mainCategory, childCategories);

            return childCategories.Select(c => c.Id);
        }

        public IEnumerable<Guid> GetCategoriesIds(IEnumerable<string> categoriesNames)
        {
            return repository.AsQueryable<Category>()
                        .Where(c => categoriesNames.Contains(c.Name) && !c.IsDeleted)
                        .Select(c => c.Id).ToList();
        }

        private void FillChildCategories(List<CategoryViewModel> allItems, CategoryViewModel mainCategory, List<CategoryViewModel> childCategories)
        {
            var childItems = allItems.Where(item => item.ParentCategoryId == mainCategory.Id && item.Id != mainCategory.Id).OrderBy(node => node.DisplayOrder).ToList();

            foreach (var item in childItems)
            {
                childCategories.Add(item);

                FillChildCategories(allItems.Except(childCategories).ToList(), item, childCategories);
            }
        }

        private void DeleteCategoryNode(ICategory node, ref IList<ICategory> deletedNodes)
        {
            if (node.ChildCategories != null)
            {
                foreach (var childNode in node.ChildCategories)
                {
                    DeleteCategoryNode(childNode, ref deletedNodes);
                }
            }

            repository.Delete(node);
            deletedNodes.Add(node);
        }

        private void UpdateModifiedInformation(Entity entity)
        {
            entity.ModifiedOn = DateTime.Now;
            entity.ModifiedByUser = securityService.CurrentPrincipalName;
            unitOfWork.Session.SaveOrUpdate(entity);
        }

    }
}