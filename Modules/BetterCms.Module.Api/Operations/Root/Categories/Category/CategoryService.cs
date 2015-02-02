using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class CategoryService : Service, ICategoryService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CategoryService(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetCategoryResponse</c> with category data.</returns>
        public GetCategoryResponse Get(GetCategoryRequest request)
        {
            var query = repository.AsQueryable<Module.Root.Models.Category>();

            if (request.CategoryId.HasValue)
            {
                query = query.Where(category => category.Id == request.CategoryId);
            }
            else
            {
                query = query.Where(category => category.Name == request.CategoryName);
            }

            var model = query
                .Select(category => new CategoryModel
                    {
                        Id = category.Id,
                        Version = category.Version,
                        CreatedBy = category.CreatedByUser,
                        CreatedOn = category.CreatedOn,
                        LastModifiedBy = category.ModifiedByUser,
                        LastModifiedOn = category.ModifiedOn,

                        Name = category.Name
                    })
                .FirstOne();

            return new GetCategoryResponse
                       {
                           Data = model
                       };
        }

        /// <summary>
        /// Puts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutCategoryResponse</c> with created or updated item id.</returns>
        public PutCategoryResponse Put(PutCategoryRequest request)
        {
            var categories = repository.AsQueryable<Module.Root.Models.Category>()
                .Where(l => l.Id == request.Id || l.Name == request.Data.Name)
                .ToList();

            var categoryToSave = categories.FirstOrDefault(l => l.Id == request.Id);

            var createCategory = categoryToSave == null;
            if (createCategory)
            {
                categoryToSave = new Module.Root.Models.Category
                {
                    Id = request.Id.GetValueOrDefault()
                };
            }
            else if (request.Data.Version > 0)
            {
                categoryToSave.Version = request.Data.Version;
            }

            categoryToSave.CategoryTree = repository.AsProxy<CategoryTree>(request.Data.CategoryTreeId);

            if (categories.Any(l => l.Id != request.Id && l.Name == request.Data.Name))
            {
                var message = string.Format("Category with name '{0}' already exists.", request.Data.Name);
                throw new CmsApiValidationException(message);
            }

            unitOfWork.BeginTransaction();

            categoryToSave.Name = request.Data.Name;

            repository.Save(categoryToSave);

            unitOfWork.Commit();

            // Fire events.
            if (createCategory)
            {
                Events.RootEvents.Instance.OnCategoryCreated(categoryToSave);
            }
            else
            {
                Events.RootEvents.Instance.OnCategoryUpdated(categoryToSave);
            }

            return new PutCategoryResponse
            {
                Data = categoryToSave.Id
            };
        }

        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteCategoryResponse</c> with success status.</returns>
        public DeleteCategoryResponse Delete(DeleteCategoryRequest request)
        {
            if (request.Data == null || request.Id.HasDefaultValue())
            {
                return new DeleteCategoryResponse { Data = false };
            }

            var itemToDelete = repository
                .AsQueryable<Module.Root.Models.Category>()
                .Where(p => p.Id == request.Id)
                .FirstOne();

            if (request.Data.Version > 0 && itemToDelete.Version != request.Data.Version)
            {
                throw new ConcurrentDataException(itemToDelete);
            }

            unitOfWork.BeginTransaction();

            repository.Delete(itemToDelete);

            unitOfWork.Commit();

            Events.RootEvents.Instance.OnCategoryDeleted(itemToDelete);

            return new DeleteCategoryResponse { Data = true };
        }
    }
}