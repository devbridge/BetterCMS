using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Services.Description;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Models;
using BetterCms.Core.Security;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using FluentNHibernate.Utils;

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
        /// The CMS configuration.
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCategoryService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DefaultCategoryService(IRepository repository, ICmsConfiguration cmsConfiguration, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.cmsConfiguration = cmsConfiguration;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the list of category lookup values.
        /// </summary>
        /// <returns>
        /// List of category lookup values
        /// </returns>
        public IEnumerable<LookupKeyValue> GetCategories()
        {
            return repository
                .AsQueryable<Category>()
                .Where(c => !c.CategoryTree.IsDeleted)
                .Select(c => new LookupKeyValue
                {
                    Key = c.Id.ToString().ToLowerInvariant(),
                    Value = c.Name
                })
                .ToFuture();
        }

        public IEnumerable<Guid> GetSelectedCategoriesIds<TEntity, TEntityCategory>(Guid? entityId) 
            where TEntity : Entity, ICategorized
            where TEntityCategory : Entity, IEntityCategory
        {
            TEntity entityAlias = null;
            TEntityCategory categoryAlias = default(TEntityCategory);

            return repository.AsQueryOver<Category>().Where(c => !c.IsDeleted)
                             .WithSubquery.WhereProperty(c => c.Id)
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
            Category categoryAliasa = null;
            TEntity entityAlias = null;
            TEntityCategory categoryAlias = default(TEntityCategory);
            LookupKeyValue valueAlias = null;

            return repository.AsQueryOver(() => categoryAliasa).Where(c => !c.IsDeleted)
                             .WithSubquery.WhereProperty(c => c.Id)
                             .In(QueryOver.Of(() => entityAlias)
                                     .JoinQueryOver(() => entityAlias.Categories, () => categoryAlias)
                                     .Where(() => entityAlias.Id == entityId && !categoryAlias.IsDeleted)
                                     .SelectList(list => list.Select(() => categoryAlias.Category.Id)))
                                     .SelectList(l => l
                                         .Select(NHibernate.Criterion.Projections.Cast(NHibernate.NHibernateUtil.String, NHibernate.Criterion.Projections.Property(() => categoryAliasa.Id))).WithAlias(() => valueAlias.Key)
                                         .Select(() => categoryAliasa.Name).WithAlias(() => valueAlias.Value))
                                     .TransformUsing(Transformers.AliasToBean<LookupKeyValue>())
                                     .Future<LookupKeyValue>();
        }
        public Category SaveCategory(out bool categoryUpdated, CategoryTree categoryTree, Guid categoryId, int version, string name, int displayOrder, string macro, Guid parentCategoryId, bool isDeleted = false, Category parentCategory = null, List<Category> categories = null)
        {
            categoryUpdated = false;

            var category = categoryId.HasDefaultValue()
                ? new Category()
                : categories != null ? categories.First(c => c.Id == categoryId) : repository.First<Category>(categoryId);

            if (isDeleted && !category.Id.HasDefaultValue())
            {
                repository.Delete(category);
                categoryUpdated = true;
            }
            else
            {
                var updated = false;
                if (category.CategoryTree == null)
                {
                    category.CategoryTree = categoryTree;
                }

                if (category.Name != name)
                {
                    updated = true;
                    category.Name = name;
                }

                if (category.DisplayOrder != displayOrder)
                {
                    updated = true;
                    category.DisplayOrder = displayOrder;
                }

                Category newParent;
                if (parentCategory != null && !parentCategory.Id.HasDefaultValue())
                {
                    newParent = parentCategory;
                }
                else
                {
                    newParent = parentCategoryId.HasDefaultValue()
                        ? null
                        : repository.AsProxy<Category>(parentCategoryId);
                }

                if (category.ParentCategory != newParent)
                {
                    updated = true;
                    category.ParentCategory = newParent;
                }

                if (cmsConfiguration.EnableMacros && category.Macro != macro)
                {
                    category.Macro = macro;
                    updated = true;
                }

                if (updated)
                {
                    category.Version = version;
                    repository.Save(category);
                    categoryUpdated = true;
                }
            }

            return category;
        }

        public void DeleteCategoryTree(Guid id, int version, IPrincipal currentUser)
        {
            var categoryTree = repository
                .AsQueryable<CategoryTree>()
                .Where(map => map.Id == id)
                // TODO:                .FetchMany(map => map.AccessRules)
                .Distinct()
                .ToList()
                .First();

            // TODO:            // Security.
            //            if (cmsConfiguration.Security.AccessControlEnabled)
            //            {
            //                var roles = new[] { RootModuleConstants.UserRoles.EditContent };
            //                accessControlService.DemandAccess(sitemap, currentUser, AccessLevel.ReadWrite, roles);
            //            }

            // Concurrency.
            if (version > 0 && categoryTree.Version != version)
            {
                throw new ConcurrentDataException(categoryTree);
            }

            unitOfWork.BeginTransaction();

            // TODO:
            //            if (sitemap.AccessRules != null)
            //            {
            //                var rules = sitemap.AccessRules.ToList();
            //                rules.ForEach(sitemap.RemoveRule);
            //            }

            repository.Delete(categoryTree);

            unitOfWork.Commit();

            // Events.
            Events.RootEvents.Instance.OnCategoryTreeDeleted(categoryTree);
        }

        public void CombineEntityCategories<TEntity, TEntityCategory>(TEntity entity, IEnumerable<System.Guid> currentCategories) 
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

                    foreach (var removedCategory in removedCategories)
                    {
                        //entity.RemoveCategory(removedCategory);
                        unitOfWork.Session.Delete(removedCategory);
                    }
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

       public void CombineEntityCategories<TEntity, TEntityCategory>(TEntity entity, IEnumerable<LookupKeyValue> currentCategories) where TEntity : Entity, ICategorized
            where TEntityCategory : Entity, IEntityCategory, new()
        {
            CombineEntityCategories<TEntity, TEntityCategory>(entity, currentCategories != null ? currentCategories.Select(c => Guid.Parse(c.Key)) : Enumerable.Empty<Guid>());
        }

        public void DeleteCategoryNode(Guid id, int version, Guid? categoryTreeId = null)
        {
            IList<Category> deletedNodes = new List<Category>();

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

            var updatedSitemaps = new List<CategoryTree>();
            foreach (var deletedNode in deletedNodes)
            {
                Events.RootEvents.Instance.OnCategoryDeleted(deletedNode);
                if (!updatedSitemaps.Contains(deletedNode.CategoryTree))
                {
                    updatedSitemaps.Add(deletedNode.CategoryTree);
                }
            }

            foreach (var sitemap in updatedSitemaps)
            {
                Events.RootEvents.Instance.OnCategoryTreeUpdated(sitemap);
            }
        }

        private void DeleteCategoryNode(Category node, ref IList<Category> deletedNodes)
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

    }
}