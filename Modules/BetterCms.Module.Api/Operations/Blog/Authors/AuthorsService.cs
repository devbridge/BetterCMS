using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Blog.Authors.Author;
using BetterCms.Module.MediaManager.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.Authors
{
    [RoutePrefix("bcms-api")]
    public class AuthorsController : ApiController, IAuthorsService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The file URL resolver.
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// The author service.
        /// </summary>
        private readonly IAuthorService authorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorsController"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        /// <param name="authorService">The author service.</param>
        public AuthorsController(IRepository repository, IMediaFileUrlResolver fileUrlResolver, IAuthorService authorService)
        {
            this.repository = repository;
            this.fileUrlResolver = fileUrlResolver;
            this.authorService = authorService;
        }

        /// <summary>
        /// Gets authors list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetAuthorsResponse</c> with authors list.
        /// </returns>
        [Route("authors")]
        public GetAuthorsResponse Get([ModelBinder(typeof(JsonModelBinder))]GetAuthorsRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var listResponse = repository
                .AsQueryable<Module.Blog.Models.Author>()
                .Select(author => new AuthorModel
                    {
                        Id = author.Id,
                        Version = author.Version,
                        CreatedBy = author.CreatedByUser,
                        CreatedOn = author.CreatedOn,
                        LastModifiedBy = author.ModifiedByUser,
                        LastModifiedOn = author.ModifiedOn,

                        Name = author.Name,

                        ImageId = author.Image != null && !author.Image.IsDeleted ? author.Image.Id : (Guid?)null,
                        ImageUrl = author.Image != null && !author.Image.IsDeleted ? author.Image.PublicUrl : (string)null,
                        ImageThumbnailUrl = author.Image != null && !author.Image.IsDeleted ? author.Image.PublicThumbnailUrl : (string)null,
                        ImageCaption = author.Image != null && !author.Image.IsDeleted ? author.Image.Caption : (string)null
                    })
                .ToDataListResponse(request);

            foreach (var model in listResponse.Items)
            {
                model.ImageUrl = fileUrlResolver.EnsureFullPathUrl(model.ImageUrl);
                model.ImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.ImageThumbnailUrl);
            }

            return new GetAuthorsResponse
                       {
                           Data = listResponse
                       };
        }

        /// <summary>
        /// Creates a new author.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PostAuthorsResponse</c> with a new author id.
        /// </returns>
        [Route("authors")]
        public PostAuthorResponse Post(PostAuthorRequest request)
        {
            var result =
                authorService.Put(
                    new PutAuthorRequest
                    {
                        Data = request.Data,
                        User = request.User
                    });

            return new PostAuthorResponse { Data = result.Data };
        }
    }
}