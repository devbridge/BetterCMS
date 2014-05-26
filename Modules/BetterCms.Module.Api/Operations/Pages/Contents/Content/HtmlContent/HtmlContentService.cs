using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Services;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Root.Mvc;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    /// <summary>
    /// Default html content CRUD service.
    /// </summary>
    public class HtmlContentService : Service, IHtmlContentService
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
        /// The content service.
        /// </summary>
        private readonly Module.Root.Services.IContentService contentService;

        /// <summary>
        /// The security service.
        /// </summary>
        private readonly ISecurityService securityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlContentService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="contentService">The content service.</param>
        /// <param name="securityService">The security service.</param>
        public HtmlContentService(IRepository repository, IUnitOfWork unitOfWork, Module.Root.Services.IContentService contentService, ISecurityService securityService)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.contentService = contentService;
            this.securityService = securityService;
        }

        /// <summary>
        /// Gets the specified html content.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetHtmlContentResponse</c> with html content.
        /// </returns>
        public GetHtmlContentResponse Get(GetHtmlContentRequest request)
        {
            var model = repository
                .AsQueryable<Module.Pages.Models.HtmlContent>(content => content.Id == request.ContentId)
                .Select(content => new HtmlContentModel
                    {
                        Id = content.Id,
                        Version = content.Version,
                        CreatedBy = content.CreatedByUser,
                        CreatedOn = content.CreatedOn,
                        LastModifiedBy = content.ModifiedByUser,
                        LastModifiedOn = content.ModifiedOn,

                        Name = content.Name,
                        ActivationDate = content.ActivationDate,
                        ExpirationDate = content.ExpirationDate,
                        Html = content.Html,
                        CustomCss = content.CustomCss,
                        UseCustomCss = content.UseCustomCss,
                        CustomJavaScript = content.CustomJs,
                        UseCustomJavaScript = content.UseCustomJs,
                        IsPublished = content.Status == ContentStatus.Published,
                        PublishedByUser = content.Status == ContentStatus.Published ? content.PublishedByUser : null,
                        PublishedOn = content.Status == ContentStatus.Published ? content.PublishedOn : null
                    })
                .FirstOne();

            return new GetHtmlContentResponse { Data = model };
        }

        /// <summary>
        /// Posts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PostHtmlContentResponse</c> with html content id.
        /// </returns>
        public PostHtmlContentResponse Post(PostHtmlContentRequest request)
        {
            var result = Put(new PutHtmlContentRequest
                {
                    Data = request.Data,
                    User = request.User
                });

            return new PostHtmlContentResponse { Data = result.Data };
        }

        /// <summary>
        /// Puts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PutHtmlContentResponse</c> with html content id.
        /// </returns>
        public PutHtmlContentResponse Put(PutHtmlContentRequest request)
        {
            var isNew = !request.Id.HasValue || request.Id.Value.HasDefaultValue();
            Module.Pages.Models.HtmlContent originalContent = null;
            if (!isNew)
            {
                originalContent = repository.FirstOrDefault<Module.Pages.Models.HtmlContent>(request.Id.Value);
                isNew = originalContent == null;
            }

            var contentToSave = new Module.Pages.Models.HtmlContent
            {
                Id = request.Id.GetValueOrDefault(),
                ActivationDate = request.Data.ActivationDate,
                ExpirationDate = TimeHelper.FormatEndDate(request.Data.ExpirationDate),
                Name = request.Data.Name,
                Html = request.Data.Html ?? string.Empty,
                UseCustomCss = request.Data.UseCustomCss,
                CustomCss = request.Data.CustomCss,
                UseCustomJs = request.Data.UseCustomJavaScript,
                CustomJs = request.Data.CustomJavaScript
            };

            if (request.Data.IsPublished)
            {
                if (isNew)
                {
                    if (request.Data.PublishedOn.HasValue)
                    {
                        contentToSave.PublishedOn = request.Data.PublishedOn;
                    }
                    if (!string.IsNullOrEmpty(request.Data.PublishedByUser))
                    {
                        contentToSave.PublishedByUser = request.Data.PublishedByUser;
                    }
                }
                else
                {
                    contentToSave.PublishedOn = originalContent.PublishedOn;
                    contentToSave.PublishedByUser = originalContent.PublishedByUser;
                }
            }

            unitOfWork.BeginTransaction();

            Module.Pages.Models.HtmlContent content;
            if (isNew && contentToSave.Id != default(Guid))
            {
                content = contentToSave;

                content.Status = request.Data.IsPublished ? ContentStatus.Published : ContentStatus.Draft;
                content.Id = contentToSave.Id;

                repository.Save(content);
            }
            else
            {
                if (request.Data.Version > 0)
                {
                    contentToSave.Version = request.Data.Version;
                }

                content = (Module.Pages.Models.HtmlContent)contentService
                    .SaveContentWithStatusUpdate(contentToSave, request.Data.IsPublished ? ContentStatus.Published : ContentStatus.Draft);
            }

            unitOfWork.Commit();

            if (isNew)
            {
                Events.PageEvents.Instance.OnHtmlContentCreated(content);
            }
            else
            {
                Events.PageEvents.Instance.OnHtmlContentUpdated(content);
            }

            return new PutHtmlContentResponse
            {
                Data = content.Id
            };
        }

        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>DeleteHtmlContentResponse</c> with success status.
        /// </returns>
        public DeleteHtmlContentResponse Delete(DeleteHtmlContentRequest request)
        {
            if (request.Data == null || request.Id.HasDefaultValue())
            {
                return new DeleteHtmlContentResponse { Data = false };
            }

            var itemToDelete = repository
                .AsQueryable<Module.Pages.Models.HtmlContent>()
                .Where(p => p.Id == request.Id)
                .FirstOne();

            if (request.Data.Version > 0 && itemToDelete.Version != request.Data.Version)
            {
                throw new ConcurrentDataException(itemToDelete);
            }

            repository.Delete(itemToDelete);
            unitOfWork.Commit();

            Events.PageEvents.Instance.OnHtmlContentDeleted(itemToDelete);

            return new DeleteHtmlContentResponse { Data = true };
        }
    }
}