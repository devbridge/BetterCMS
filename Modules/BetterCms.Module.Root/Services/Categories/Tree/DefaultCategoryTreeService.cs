using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Services.Categories.Nodes;
using BetterCms.Module.Root.Views.Language;

using NHibernate.Criterion;
using NHibernate.Linq;

namespace BetterCms.Module.Root.Services.Categories.Tree
{
    public class DefaultCategoryTreeService : ICategoryTreeService
    {
        private IRepository Repository;

        private IUnitOfWork UnitOfWork;

        public DefaultCategoryTreeService(IRepository repository, IUnitOfWork unitOfWork)
        {
            Repository = repository;
            UnitOfWork = unitOfWork;
        }

        public GetCategoryTreeResponse Get(GetCategoryTreeRequest request)
        {
            var response = new GetCategoryTreeResponse();
            CategoryTree alias = null;

            var query = Repository.AsQueryOver(() => alias).Where(() => !alias.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", request.SearchQuery);
                query =
                    query.Where(Restrictions.Disjunction().Add(Restrictions.InsensitiveLike(NHibernate.Criterion.Projections.Property(() => alias.Title), searchQuery)));
            }

            if (request.WithOrder)
            {
                query = query.AddOrder(request);
            }

            if (request.WithPaging)
            {
                query = query.AddPaging(request);
            }

            response.Items = query.AddSortingAndPaging(request).Future<CategoryTree>();
            response.TotalCount = query.ToRowCountFutureValue().Value;
            return response;
        }

        public CategoryTree Save(SaveCategoryTreeRequest request)
        {
            var createNew = request.Id.HasDefaultValue();

            IEnumerable<Category> categories;
            CategoryTree categoryTree;
            if (createNew)
            {
                categories = new List<Category>();
                categoryTree = new CategoryTree();
            }
            else
            {
                categories = Repository.AsQueryable<Category>().Where(node => node.CategoryTree.Id == request.Id).ToFuture();
                categoryTree = Repository.AsQueryable<CategoryTree>().Where(s => s.Id == request.Id).ToFuture().First();
            }

            UnitOfWork.BeginTransaction();

            categoryTree.Title = request.Title;
            categoryTree.Version = request.Version;
            UnitOfWork.Session.Save(categoryTree);

            // TODO:
//            SaveCategoryTree()

            return new CategoryTree();
        }

        public bool Delete(DeleteCategoryTreeRequest request)
        {
            throw new System.NotImplementedException();
        }


        private void SaveCategoryTree(CategoryTree categoryTree, IEnumerable<CategoryTreeNodeModel> categories, Category parentCategory, IList<Category> categoryList)
        {
            if (categories == null)
            {
                return;
            }

            foreach (var categoryNode in categories)
            {
                var isDeleted = categoryNode.IsDeleted || (parentCategory != null && parentCategory.IsDeleted);
                var create = categoryNode.Id.HasDefaultValue() && !isDeleted;
                var update = !categoryNode.Id.HasDefaultValue() && !isDeleted;
                var delete = !categoryNode.Id.HasDefaultValue() && isDeleted;

                bool updatedInDB;
//                var category = DefaultCategoryNodeService
            }
        }
    }
}