using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
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
        /// Initializes a new instance of the <see cref="HtmlContentService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public HtmlContentService(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
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
        /// Puts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PutHtmlContentResponse</c> with html content id.
        /// </returns>
        public PutHtmlContentResponse Put(PutHtmlContentRequest request)
        {
            var content = repository.AsQueryable<Module.Pages.Models.HtmlContent>().FirstOrDefault(e => e.Id == request.Data.Id);

            var createImage = content == null;
            if (createImage)
            {
                content = new Module.Pages.Models.HtmlContent { Id = request.Data.Id };
            }
            else
            {
                content.Version = request.Data.Version;
            }

            unitOfWork.BeginTransaction();

            content.Name = request.Data.Name;
            content.ActivationDate = request.Data.ActivationDate;
            content.ExpirationDate = request.Data.ExpirationDate;
            content.Html = request.Data.Html;
            content.CustomCss = request.Data.CustomCss;
            content.UseCustomCss = request.Data.UseCustomCss;
            content.CustomJs = request.Data.CustomJavaScript;
            content.UseCustomJs = request.Data.UseCustomJavaScript;
            content.Status = request.Data.IsPublished ? ContentStatus.Published : ContentStatus.Draft;
            content.PublishedByUser = request.Data.PublishedByUser;
            content.PublishedOn = content.PublishedOn;

            repository.Save(content);
            unitOfWork.Commit();

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
            if (request.Data == null || request.Data.Id.HasDefaultValue())
            {
                return new DeleteHtmlContentResponse { Data = false };
            }

            repository.Delete<Module.Pages.Models.HtmlContent>(request.Data.Id, request.Data.Version);
            unitOfWork.Commit();

            return new DeleteHtmlContentResponse { Data = true };
        }
    }
}