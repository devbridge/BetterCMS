using System.Globalization;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    public class LanguageService : Service, ILanguageService
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
        /// Initializes a new instance of the <see cref="LanguageService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public LanguageService(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetLanguageResponse</c> with language data.</returns>
        public GetLanguageResponse Get(GetLanguageRequest request)
        {
            var query = repository.AsQueryable<Module.Root.Models.Language>();

            if (request.LanguageId.HasValue)
            {
                query = query.Where(language => language.Id == request.LanguageId);
            }
            else
            {
                query = query.Where(language => language.Code == request.LanguageCode);
            }

            var model = query
                .Select(language => new LanguageModel
                    {
                        Id = language.Id,
                        Version = language.Version,
                        CreatedBy = language.CreatedByUser,
                        CreatedOn = language.CreatedOn,
                        LastModifiedBy = language.ModifiedByUser,
                        LastModifiedOn = language.ModifiedOn,

                        Name = language.Name,
                        Code = language.Code
                    })
                .FirstOne();

            return new GetLanguageResponse
                       {
                           Data = model
                       };
        }

        /// <summary>
        /// Puts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutLanguageResponse</c> with id of created/updated language.</returns>
        public PutLanguageResponse Put(PutLanguageRequest request)
        {
            var language = repository.AsQueryable<Module.Root.Models.Language>()
                .Where(file => file.Id == request.LanguageId)
                .ToFuture()
                .FirstOrDefault();

            var createLanguage = language == null;
            if (createLanguage)
            {
                language = new Module.Root.Models.Language
                {
                    Id = request.LanguageId.GetValueOrDefault(),
                    Code = request.Data.Code
                };
            }
            else if (request.Data.Version > 0)
            {
                language.Version = request.Data.Version;
            }

            if (language.Code != request.Data.Code)
            {
                throw new CmsApiValidationException("Language code changing is not allowed.");
            }

            if (CultureInfo.GetCultures(CultureTypes.AllCultures).All(c => c.Name != request.Data.Code))
            {
                var message = string.Format("Language with code '{0}' doesn't exist.", request.Data.Code);
                throw new CmsApiValidationException(message);
            }

            unitOfWork.BeginTransaction();

            language.Name = request.Data.Name;

            repository.Save(language);

            unitOfWork.Commit();

            // Fire events.
            if (createLanguage)
            {
                Events.RootEvents.Instance.OnLanguageCreated(language);
            }
            else
            {
                Events.RootEvents.Instance.OnLanguageUpdated(language);
            }

            return new PutLanguageResponse
            {
                Data = language.Id
            };
        }

        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeleteLanguageResponse</c> with success status.</returns>
        public DeleteLanguageResponse Delete(DeleteLanguageRequest request)
        {
            if (request.Data == null || request.LanguageId.HasDefaultValue())
            {
                return new DeleteLanguageResponse { Data = false };
            }

            var futurePages = repository.AsQueryable<Module.Root.Models.Page>()
                .Where(p => p.Language != null && p.Language.Id == request.LanguageId)
                .Select(p => p.Id)
                .ToFuture();

            var itemToDelete = repository
                .AsQueryable<Module.Root.Models.Language>()
                .Where(p => p.Id == request.LanguageId)
                .ToFuture()
                .FirstOne();

            if (request.Data.Version > 0 && itemToDelete.Version != request.Data.Version)
            {
                throw new ConcurrentDataException(itemToDelete);
            }

            if (futurePages.ToList().Any())
            {
                var message = string.Format("Cannot delete language {0}, because it's used in pages.", itemToDelete.Name);
                throw new CmsApiValidationException(message);
            }

            unitOfWork.BeginTransaction();

            repository.Delete(itemToDelete);

            unitOfWork.Commit();

            Events.RootEvents.Instance.OnLanguageDeleted(itemToDelete);

            return new DeleteLanguageResponse { Data = true };
        }
    }
}